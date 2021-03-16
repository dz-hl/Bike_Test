using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateReplay;

public class GOForward : MonoBehaviour
{
    [SerializeField] GameObject moveProp;
    public float speed = 6.0f;
    public Vector3 forwardDir = Vector3.forward;
    private bool isCollided = false;


    private void Update()
    {
        if (isCollided)
        {
            moveProp.transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        isCollided = true;
    }

}
