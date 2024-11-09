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
    NavMeshAgent agent;
    // FOV and Player spotting related things
    FieldOfView fov;
    List<Collider> spottedObjects;
    [SerializeField]
    Animator animator;

    // AI Related vars
    float stateTimer;
    Vector3 nextTarget;
    enum EnemyState
    {
        STATE_DEAD,
        STATE_IDLE,
        STATE_PATROL,
        STATE_CHASE,
        STATE_ATTACK
    }
    EnemyState state;

    void Start()
    {
        spottedObjects = new List<Collider>();
        fov = GetComponent<FieldOfView>();
        state = EnemyState.STATE_PATROL;
        Vector3 point;
        movementController.RandomPoint(transform.position, nextTargetMaxRange, out point);
        agent = movementController.GetAgent();
        agent.destination = point;
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
                        if (!(state == EnemyState.STATE_CHASE || state == EnemyState.STATE_ATTACK))
                        {
                            ChangeState(EnemyState.STATE_CHASE);
                            movementController.SetTarget(obj.transform.position);
                            Debug.Log("1");
                        }
                        break;
                    }
                    else if (obj.CompareTag("PlayerLightSource"))
                    {
                        ChangeState(EnemyState.STATE_PATROL);
                        agent.destination = obj.transform.position;
                        Debug.Log("2");
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
                    movementController.SetTarget(nextTarget);
                    ChangeState(EnemyState.STATE_PATROL);
                }
                break;
            case EnemyState.STATE_PATROL:
                if (agent.remainingDistance <= agent.stoppingDistance) //done with path
                {
                    Vector3 point;
                    if (movementController.RandomPoint(transform.position, Random.Range(nextTargetMaxRange / 2, nextTargetMaxRange), out point)) //pass in our centre point and radius of area
                    {
                        ChangeState(EnemyState.STATE_IDLE);
                        nextTarget = point;
                    }
                }
                break;
            case EnemyState.STATE_ATTACK:
                if (spottedObjects.Count > 0)
                {
                    foreach (var obj in spottedObjects)
                    {
                        if (obj.CompareTag("Player") && Vector3.Distance(transform.position, obj.transform.position) >= 3)
                        {
                            state = EnemyState.STATE_CHASE;
                        }
                    }
                }
                break;
            case EnemyState.STATE_CHASE:
                //Debug.Log(agent.remainingDistance);
                if (agent.remainingDistance <= 1)
                {
                    ChangeState(EnemyState.STATE_ATTACK);
                }
                else if (stateTimer > 10.0f)
                {
                    if (spottedObjects.Count > 0)
                    {
                        stateTimer = 0;
                    }
                    Vector3 point;
                    if (movementController.RandomPoint(transform.position, Random.Range(nextTargetMaxRange / 2, nextTargetMaxRange), out point)) //pass in our centre point and radius of area
                    {
                        ChangeState(EnemyState.STATE_PATROL);
                        nextTarget = point;
                    }
                }
                break;
        }
    }
    void ChangeState(EnemyState newState)
    {
        if (state != newState)
        {
            Debug.Log("State changed");
            state = newState;
            stateTimer = 0;
            switch (state)
            {
                case EnemyState.STATE_IDLE:
                    if (!animator.GetBool("Idle"))
                        animator.SetTrigger("Idle");
                    movementController.ResumeNavigation();
                    Debug.Log("Idle");
                    break;
                case EnemyState.STATE_PATROL:
                    if (!animator.GetBool("Patrol"))
                        animator.SetTrigger("Patrol");
                    movementController.ResumeNavigation();
                    Debug.Log("Patrol");
                    break;
                case EnemyState.STATE_ATTACK:
                    if (!animator.GetBool("Attack"))
                        animator.SetTrigger("Attack");
                    movementController.StopNavigation();
                    Debug.Log("Attack");
                    break;
                case EnemyState.STATE_CHASE:
                    if (!animator.GetBool("Chase"))
                        animator.SetTrigger("Patrol");
                    movementController.ResumeNavigation();
                    Debug.Log("Chase");
                    break;
                case EnemyState.STATE_DEAD:
                    animator.SetTrigger("Dead");
                    break;
            }
           
        }
    }

    public void OnSoundHeard(Vector3 soundPos)
    {
        ChangeState(EnemyState.STATE_PATROL);
        movementController.SetTarget(soundPos);
    }
}
