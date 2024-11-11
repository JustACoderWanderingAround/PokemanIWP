using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentMovementController : MonoBehaviour
{
    [SerializeField]
    float runningSpeed;
    NavMeshAgent m_agent;
    float defaultSpeed;
    // Start is called before the first frame update
    void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        defaultSpeed = m_agent.speed;
    }
    public void SetTarget(Vector3 newTarget)
    {
        m_agent.destination = newTarget;
    }
    public NavMeshAgent GetAgent() => m_agent;

    public void StopNavigation()
    {
        m_agent.speed = 0;
        m_agent.isStopped = true;
        m_agent.destination = transform.position;
    }
    public void ResumeNavigation()
    {
        m_agent.isStopped = false;
        m_agent.speed = defaultSpeed;
    }
    public float GetRemainingDistance()
    {
        return m_agent.remainingDistance;
    }
    public float GetStoppingDistance()
    {
        return m_agent.stoppingDistance;
    }
    // Pick the next point iwthin the room to move to
    // Function taken from https://www.youtube.com/watch?v=dYs0WRzzoRc
    public bool GetNextTargetPos(Vector3 center, float range, out Vector3 result)
    {

        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
