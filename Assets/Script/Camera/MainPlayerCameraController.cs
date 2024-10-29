using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerCameraController : MonoBehaviour
{
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
        Vector3 newRot = new Vector3(rotAngle, transform.parent.rotation.y, 0);
        transform.parent.rotation = Quaternion.Euler(newRot);
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
}