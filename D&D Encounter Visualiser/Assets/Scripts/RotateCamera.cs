using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public Transform target;

    void Start()
    {
        transform.LookAt(target);  
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(target.position, Vector3.up, 25 * Time.deltaTime);
    }
}
