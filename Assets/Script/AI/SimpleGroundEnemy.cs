using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleGroundEnemy : DamagableEntity
{
    [SerializeField]
    NavAgentMovementController movementController;
    [SerializeField]
    private float nextTargetMaxRange = 5.0f;
    NavMeshAgent agent;
    FieldOfView fov;

    // AI Related vars
    float stateTimer;
    Vector3 nextTarget;
    enum EnemyState
    {
        STATE_IDLE,
        STATE_PATROL,
        STATE_ATTACK
    }
    EnemyState state;

    protected override void Awake()
    {
        base.Awake();
        agent = movementController.GetAgent();
        state = EnemyState.STATE_PATROL;
        Vector3 point;
        movementController.RandomPoint(transform.position, nextTargetMaxRange, out point);
        movementController.GetAgent().destination = point;
    }

    // plan: randomly walk around
    // after a few seconds, idle
    // keep walking

    // Update is called once per frame
    void Update()
    {
        // TODO: Player distance check logic + sound check logic
        UpdateStateMachine();
    }
    void UpdateStateMachine()
    {
        stateTimer += Time.deltaTime;
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
                    if (movementController.RandomPoint(transform.position, Random.Range(0, nextTargetMaxRange), out point)) //pass in our centre point and radius of area
                    {
                        ChangeState(EnemyState.STATE_IDLE);
                        nextTarget = point;
                    }
                }
                break;
            case EnemyState.STATE_ATTACK:
                break;
        }
    }
    void ChangeState(EnemyState newState)
    {
        Debug.Log("State changed");
        stateTimer = 0;
        state = newState;
    }
}
