using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook  : MonoBehaviour
{
    public float sensitivityHor = 5.0f;
    public float sensitivityVer = 5.0f;

    public float minVerticalDeg = -45.0f;
    public float maxVerticalDeg = 45.0f;

    private float _rotationX = 0;
    public enum RotationAxes
    {
        MouseX = 0,
        MouseY = 1,
        MouseXandY = 2
    }

    public RotationAxes axes = RotationAxes.MouseX;

    
    void Update()
    {
        if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityVer, 0);
        }
        else if (axes == RotationAxes.MouseY)
        {
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVer;
            _rotationX = Mathf.Clamp(_rotationX, minVerticalDeg, maxVerticalDeg);
            float rotationY = transform.localEulerAngles.y;
            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        }
        else
        {
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVer;
            _rotationX = Mathf.Clamp(_rotationX, minVerticalDeg, maxVerticalDeg);
            float delta = Input.GetAxis("Mouse X") * sensitivityHor;
            float rotationY = transform.localEulerAngles.y + delta;

            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        }
    }
}
