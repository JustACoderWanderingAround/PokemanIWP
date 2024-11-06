using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentMovementController : MonoBehaviour
{
    NavMeshAgent m_agent;
    // Start is called before the first frame update
    void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
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
    }
    public void ResumeNavigation()
    {
        m_agent.isStopped = false;
    }
    // Pick the next point iwthin the room to move to
    // Function taken from https://www.youtube.com/watch?v=dYs0WRzzoRc
    public bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
