using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// Adoted from https://github.com/Comp3interactive/FieldOfView/
public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0,360)]
    public float angle;

    public List<string> tagsToCompare = new List<string>() { "Player", "PlayerLightSource" };

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    UnityEvent onTargetSpottedCallback;

    private void Start()
    {
        //StartCoroutine(FOVRoutine());
    }
    // Original code from Git - commented out redundant parts
    #region OriginalCode
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }
    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                //if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                //    canSeePlayer = true;
                //else
                //    canSeePlayer = false;
            }
            //else
            //    canSeePlayer = false;
        }
        //else if (canSeePlayer)
        //    canSeePlayer = false;
    }
    #endregion
    // Rewritten code based loosely on code from WIU4 teammate
    public bool FieldOfViewCheck(out List<Collider> seenObjects)
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        List<Collider> foundObjects = new List<Collider>();
        if (rangeChecks.Length != 0)
        {
            foreach (Collider c in rangeChecks)
            {
                foreach (string tag in tagsToCompare)
                {
                    if (c.gameObject.CompareTag(tag))
                    {
                        Transform target = c.transform;
                        Vector3 directionToTarget = (target.position - transform.position).normalized;
                        if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                        {
                            float distanceToTarget = Vector3.Distance(transform.position, target.position);
                            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                            {
                                foundObjects.Add(c);
                            }
                        }
                    }
                }
            }
            if (foundObjects.Count > 0)
            {
                seenObjects = foundObjects;
                return true;
            }
           
        }
        seenObjects = new List<Collider>();
        return false;
    }
}
