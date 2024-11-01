using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerCameraController : MonoBehaviour
{
    /// <summary>
    /// HANDLES:
    /// HEAD ROTATION
    /// CAMERA SHAKE
    /// FOV CHANGE IF NECESSARY
    ///
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void RotateHeadXAxis(float rotAngle)
    {
        Vector3 newRot = new Vector3(rotAngle, 0, 0);
        transform.parent.localRotation = Quaternion.Euler(newRot);
        //transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}