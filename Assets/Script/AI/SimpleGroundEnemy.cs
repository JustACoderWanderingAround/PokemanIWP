using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleGroundEnemy : DamagableEntity, ISoundListener
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

    // AI Related vars
    float stateTimer;
    Vector3 nextTarget;
    bool playerSpotted;
    public enum EnemyState
    {
        STATE_DEAD = -1,
        STATE_IDLE = 0,
        STATE_PATROL,
        STATE_CHASE,
        STATE_ATTACK
    }
    public EnemyState state;

    void Start()
    {
        spottedObjects = new List<Collider>();
        fov = GetComponent<FieldOfView>();
        state = EnemyState.STATE_PATROL;
        animator.SetTrigger("Patrol");
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
                        if (state < EnemyState.STATE_CHASE)
                        {
                            ChangeState(EnemyState.STATE_CHASE);
                            movementController.SetTarget(obj.transform.position);
                            playerSpotted = true;
                            Debug.Log("Player spotted");
                        }
                        break;
                    }
                    else if (obj.CompareTag("PlayerLightSource") && !playerSpotted)
                    {
                        if (Vector3.Distance(obj.transform.position, transform.position) > 2 && state < EnemyState.STATE_CHASE)
                        {
                            ChangeState(EnemyState.STATE_PATROL);
                            movementController.SetTarget(obj.ClosestPoint(transform.position));
                            Debug.Log("Light source spotted");
                        }
                        break;
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
    }
    // plan: randomly walk around
    // after a few seconds, idle
    // keep walking
    void UpdateStateMachine()
    {
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
                        if (obj.CompareTag("Player") && Vector3.Distance(transform.position, obj.transform.position) >= 3)
                        {
                            ChangeState(EnemyState.STATE_CHASE);
                        }
                    }
                }
                else
                {
                    playerSpotted = false;
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
                                    movementController.SetTarget(obj.ClosestPoint(transform.position));
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
        if (state != newState)
        {
            state = newState;
            stateTimer = 0;
            switch (state)
            {
                case EnemyState.STATE_IDLE:
                    if (!animator.GetBool("Idle"))
                        animator.SetTrigger("Idle");
                    movementController.ResumeNavigation();
                    Debug.Log("Change state: Idle");
                    break;
                case EnemyState.STATE_PATROL:
                    ReturnToPatrol();
                    animator.SetTrigger("Patrol");
                    Debug.Log("Animator: Patrol");
                    break;
                case EnemyState.STATE_ATTACK:
                    if (!animator.GetBool("Attack"))
                        animator.SetTrigger("Attack");
                    movementController.StopNavigation();
                    Debug.Log("Change state: Attack");
                    break;
                case EnemyState.STATE_CHASE:
                    animator.SetTrigger("Patrol");
                    Debug.Log("Animator: Patrol");
                    movementController.ResumeNavigation();
                    Debug.Log("Change state: Chase");
                    break;
                case EnemyState.STATE_DEAD:
                    animator.SetTrigger("Dead");
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
            Debug.Log("no new patrol target set :(");
        }
        movementController.ResumeNavigation();
        Debug.Log("Change state: Patrol");
    }
}
