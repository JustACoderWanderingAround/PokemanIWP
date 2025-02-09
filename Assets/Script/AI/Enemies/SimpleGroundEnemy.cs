using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleGroundEnemy : Enemy, ISoundListener
{
    [SerializeField]
    NavAgentMovementController movementController;
    [SerializeField]
    private float nextTargetMaxRange = 5.0f;
    // FOV and Player spotting related things
    FieldOfView fov;
    List<Collider> spottedObjects;
    [SerializeField]
    Animator animator;
    [SerializeField]
    float runSpeed = 2.5f;
    [SerializeField]
    float walkSpeed = 1.2f;
    [SerializeField]
    EntityRandomSoundPlayer randomSoundPlayer;
    [SerializeField]
    float maxHitStunTiming = 0.1f;
    float hitStunTiming;

    // AI Related vars
    float stateTimer;
    Vector3 nextTarget;
    bool playerSpotted;
    public enum EnemyState
    {
        STATE_IDLE = 0,
        STATE_PATROL,
        STATE_CHASE,
        STATE_ATTACK,
        STATE_DEAD
    }
    public EnemyState state;

    void Start()
    {
        spottedObjects = new List<Collider>();
        fov = GetComponent<FieldOfView>();
        ChangeState(EnemyState.STATE_IDLE);
        //animator.SetTrigger("Patrol");
        Vector3 point;
        movementController.GetNextTargetPos(transform.position, nextTargetMaxRange, out point);
        movementController.SetTarget(point);
        StartCoroutine(FOVRoutine());
    }
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            fov.FieldOfViewCheck(out spottedObjects);
            // TODO: Player distance check logic + sound check logic
            if (spottedObjects.Count > 0)
            {
                foreach (var obj in spottedObjects)
                {
                    if (obj.CompareTag("Player"))
                    {
                        if (!playerSpotted)
                        {
                            if (state < EnemyState.STATE_CHASE)
                            {
                                Debug.Log("1");
                                ChangeState(EnemyState.STATE_CHASE);
                                movementController.SetTarget(obj.transform.position);
                                playerSpotted = true;
                                Debug.Log("Player spotted");
                            }
                        }
                        break;
                    }
                    else if (obj.CompareTag("PlayerLightSource") && !playerSpotted)
                    {
                        if (fov.CheckLineOfSight(transform, obj.transform))
                        {
                            if (Vector3.Distance(obj.transform.position, transform.position) > 2 && state < EnemyState.STATE_CHASE)
                            {
                                if (state < EnemyState.STATE_CHASE)
                                {
                                    Debug.Log("2");
                                    ChangeState(EnemyState.STATE_CHASE);
                                    movementController.SetTarget(obj.transform.position);
                                    Debug.Log("Light source spotted");
                                }
                            }
                        }
                        /// TODO:
                        /// Make it so that zombie moves towards player when light source is cast on them.
                        /// Plan: 
                    }
                }
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (GetCurrentHealth() > 0)
            UpdateStateMachine();
        else
        {
            ChangeState(EnemyState.STATE_DEAD);
        }
        if (hitStunTiming > 0)
        {
            hitStunTiming -= Time.deltaTime;
        }
        else
        {
            movementController.ResumeNavigation();
        }
    }
    // plan: randomly walk around
    // after a few seconds, idle
    // keep walking
    void UpdateStateMachine()
    {
        if (spottedObjects.Count < 1)
        {
            playerSpotted = false;
            if (state > EnemyState.STATE_CHASE)
            {
                movementController.SetTarget(transform.position);
                ChangeState(EnemyState.STATE_IDLE);
            }
        }
        stateTimer += Time.deltaTime;
        //Debug.Log("ST: " + stateTimer);
        switch (state)
        {
            case EnemyState.STATE_IDLE:
                if (stateTimer > 5.0f)
                {
                    ChangeState(EnemyState.STATE_PATROL);
                }
                break;
            case EnemyState.STATE_PATROL:
                if (movementController.GetRemainingDistance()<= movementController.GetStoppingDistance()) //done with path
                {
                    ChangeState(EnemyState.STATE_IDLE);
                }
                break;
            case EnemyState.STATE_ATTACK:
                if (spottedObjects.Count > 0)
                {
                    foreach (var obj in spottedObjects)
                    {
                        if (obj.CompareTag("Player") && Vector3.Distance(transform.position, obj.transform.position) >= 5)
                        {
                            Debug.Log("3");
                            ChangeState(EnemyState.STATE_CHASE);
                        }
                    }
                }
                else
                {
                    ChangeState(EnemyState.STATE_PATROL);
                }
                break;
            case EnemyState.STATE_CHASE:
                if (movementController.GetRemainingDistance()<= 1)
                {
                    if (spottedObjects.Count > 0)
                    {
                        foreach (var obj in spottedObjects)
                        {
                            if (obj.CompareTag("Player"))
                            {
                                if (Vector3.Distance(transform.position, obj.transform.position) <= 1.5f)
                                    ChangeState(EnemyState.STATE_ATTACK);
                                else
                                {
                                    movementController.SetTarget(obj.transform.position);
                                }
                            }
                        }
                    }
                    else
                    {
                        ChangeState(EnemyState.STATE_PATROL);
                    }
                }
                break;
        }
    }
    void ChangeState(EnemyState newState)
    {
        Debug.Log("ChangeState called - currState = " + state + " newstate = " + newState);
        stateTimer = 0;
        ResetAllTriggers();
        if (state != newState)
        {
            state = newState;
            switch (state)
            {
                case EnemyState.STATE_IDLE:
                    animator.CrossFade("Idle", 0.05f);
                    movementController.ResumeNavigation();
                    Debug.Log("Change state: Idle");
                    break;
                case EnemyState.STATE_PATROL:
                    ReturnToPatrol();
                    animator.CrossFade("Patrol", 0.05f);
                    Debug.Log("Animator: Patrol");
                    break;
                case EnemyState.STATE_ATTACK:
                    animator.CrossFade("Attack", 0.05f);
                    Debug.Log("Animator: Attack");
                    movementController.StopNavigation();
                    Debug.Log("Change state: Attack");
                    break;
                case EnemyState.STATE_CHASE:
                    animator.CrossFade("Chase", 0.05f);
                    Debug.Log("Animator: Chase");
                    movementController.ResumeNavigation();
                    Debug.Log("Change state: Chase");
                    OnPlayerObserved();
                    break;
                case EnemyState.STATE_DEAD:
                    OnDeath();
                    animator.CrossFade("Dead", 0.05f);
                    movementController.StopNavigation();
                    StopAllCoroutines();
                    randomSoundPlayer.StopPlayer();
                    GetComponent<Collider>().enabled = false;
                    break;
            }
           
        }
    }

    public void OnSoundHeard(Vector3 soundPos)
    {
        if (state < EnemyState.STATE_PATROL)
        {
            ChangeState(EnemyState.STATE_PATROL);
        }
        if (state < EnemyState.STATE_CHASE)
            movementController.SetTarget(soundPos);
    }
    
    void ReturnToPatrol()
    {
        Vector3 point;
        if (movementController.GetNextTargetPos(transform.position, nextTargetMaxRange, out point)) //pass in our centre point and radius of area
        {
            ChangeState(EnemyState.STATE_PATROL);
            movementController.SetTarget(point);
            Debug.Log("New patrol target set");
        }
        else
        {
            Debug.Log(gameObject.name + ": no new patrol target set :(");
        }
        movementController.ResumeNavigation();
        Debug.Log("Change state: Patrol");
    }
    public override void Damage(int dmg)
    {
        base.Damage(dmg);
        animator.SetTrigger("Damaged");
        hitStunTiming = maxHitStunTiming;
    }
    private void ResetAllTriggers()
    {
        foreach (var param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(param.name);
            }
        }
    }
}
