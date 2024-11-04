using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGroundEnemy : DamagableEntity
{
    [SerializeField]
    NavAgentMovementController m_MovementController;

    // AI Related vars

    float stateTimer;
    enum EnemyState
    {
        STATE_IDLE,
        STATE_PATROL,
        STATE_ATTACK
    }

    protected override void Awake()
    {
        base.Awake();
    }

    // plan: randomly walk around
    // after a few seconds, idle
    // keep walking

    void Update()
    {
        // 
    }
}
