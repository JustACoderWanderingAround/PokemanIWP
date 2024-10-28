using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeanController : MonoBehaviour
{
    public Animator animator;
    public LayerMask layers;
    public RaycastHit Hit;

    void Update()
    {
        if (Input.GetAxis("LeanR") > 0 && (Input.GetAxis("LeanL") > 0))
        {
            animator.ResetTrigger("Right");
            animator.ResetTrigger("Left");
            animator.SetTrigger("Idle");
        }
        if (Input.GetAxis("LeanL") > 0)
        {
            //if (!Physics.Raycast(transform.position, -transform.right, out Hit, 1f, layers))
            //{
                animator.ResetTrigger("Idle");
                animator.ResetTrigger("Right");
                animator.SetTrigger("Left");
            //}
        }
        else if (Input.GetAxis("LeanR") > 0)
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
