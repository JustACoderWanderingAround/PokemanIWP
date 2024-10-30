using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeanController : MonoBehaviour
{
    public Animator animator;

    public void LeanPlayer(PlayerIntegratedMovementController.LeanState newState)
    {

       
        if (newState == PlayerIntegratedMovementController.LeanState.LeanStateL)
        {
            //if (!Physics.Raycast(transform.position, -transform.right, out Hit, 1f, layers))
            //{
            animator.ResetTrigger("Idle");
            animator.ResetTrigger("Right");
            animator.SetTrigger("Left");
            //}
        }
        else if (newState == PlayerIntegratedMovementController.LeanState.LeanStateR)
        {
            //if (!Physics.Raycast(transform.position, transform.right, out Hit, 1f, layers))
            //{
            animator.ResetTrigger("Idle");
            animator.ResetTrigger("Left");
            animator.SetTrigger("Right");
            //}
        }
        else
        {
            animator.ResetTrigger("Right");
            animator.ResetTrigger("Left");
            animator.SetTrigger("Idle");
        }
    }
}
