using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementController))]
public class PlayerCrouchProneController : MonoBehaviour
{
    PlayerMovementController movementController;

    private Vector3 defaultScale = new Vector3(1, 1, 1);
    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<PlayerMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(movementController.stanceState)
        {
            case PlayerMovementController.StanceState.State_Sprint:
            case PlayerMovementController.StanceState.State_Stand:
                gameObject.transform.localScale = defaultScale;
                break;
            case PlayerMovementController.StanceState.State_Crouch:
                gameObject.transform.localScale = new Vector3(1, 0.65f, 1);
                break;
            case PlayerMovementController.StanceState.State_Prone:
                gameObject.transform.localScale = new Vector3(1, 0.2f, 1);
                break;
        }
    }
}
