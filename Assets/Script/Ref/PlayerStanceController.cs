using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStanceController : MonoBehaviour
{
    enum StanceList
    {
        STANCE_NORMAL = 0,
        STANCE_CROUCH = 1,
        STANCE_PRONE = 2,
        NUM_STANCE
    }
    private StanceList currStance;
    private Rigidbody rb;
    [SerializeField]
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        currStance = StanceList.STANCE_NORMAL;
        rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("StanceDown") > 0 && currStance < StanceList.NUM_STANCE)
        {
            currStance += 1;
        }
        else if (Input.GetAxisRaw("StanceUp") > 0 && (int)currStance > 0)
        {
            currStance -= 1;
        }
    }
    /// TODO: Implement actual leaning - somehow angle the capsule to rotate and shit like that??
}
