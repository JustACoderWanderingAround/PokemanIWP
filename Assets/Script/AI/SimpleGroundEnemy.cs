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
                        ChangeState(EnemyState.STATE_ATTACK);
                        movementController.SetTarget(obj.transform.position);
                        break;
                    }
                    else if (obj.CompareTag("PlayerLightSource"))
                    {
                        agent.destination = obj.transform.position;
                    }
                }
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
        UpdateStateMachine();
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
                if (stateTimer > 10.0f)
                {
                    if (spottedObjects.Count > 0)
                    {
                        stateTimer = 0;
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
        Debug.Log("State changed");
        state = newState;
        stateTimer = 0;
    }

    public void OnSoundHeard(Vector3 soundPos)
    {
        ChangeState(EnemyState.STATE_PATROL);
        movementController.SetTarget(soundPos);
    }
}
