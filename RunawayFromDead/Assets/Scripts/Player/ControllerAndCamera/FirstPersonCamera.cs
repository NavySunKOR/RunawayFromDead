using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour {
    [Range(0,88)]
    public float upperAngleLimit;
    [Range(-88, 0)]
    public float underAngleLimit;
    public float mouseSensivity;
    [Range(-1,1)]
    public float invertMouseY;
    public Transform cameraTr;
    public bool CanControl
    {
        get
        {
            return canControl;
        }
        set
        {
            canControl = value;
        }
    }

    private float horizontal;
    private float vertical;
    private Vector3 rotation;
    private bool canControl;

    private void Start()
    {
        canControl = true;
        rotation = new Vector3(0, 0, 0);
    }



    private void Update () {
        horizontal += Input.GetAxis("Mouse X") * mouseSensivity;
        vertical += Input.GetAxis("Mouse Y") * -invertMouseY * mouseSensivity;

        if(vertical >= upperAngleLimit)
        {
            vertical = upperAngleLimit;
        }
        else if(vertical <= underAngleLimit)
        {
            vertical = underAngleLimit;
        }

        rotation.x = vertical;
        rotation.y = horizontal;
        cameraTr.rotation = Quaternion.Euler(rotation);
        transform.forward = cameraTr.forward;
    }
}
