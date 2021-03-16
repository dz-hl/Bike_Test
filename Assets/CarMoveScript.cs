using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMoveScript : MonoBehaviour
{
    public Transform path;
    public float maxSteerAngle = 40f;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;

    private List<Transform> nodes;
    private int currentNode = 0;
    private void Start()
    {


        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
    }
    private void FixedUpdate()
    {
        ApplySteer();
        Drive();
        CheckWaypointDistance();
    }
    void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }
    void Drive()
    {
        wheelFL.motorTorque = 100f;
        wheelFR.motorTorque = 100f;
    }

    void CheckWaypointDistance()
    {
        if(Vector3.Distance(transform.position, nodes[currentNode].position) < 1f)
        {
            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            } else
            {
                currentNode++;
            }
        }
    }
}
