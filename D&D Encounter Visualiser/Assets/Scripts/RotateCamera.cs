using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [Tooltip("The parent object containing the object(s) that should be in focus.")]
    public Transform target;

    private Vector3 center = Vector3.zero;

    private bool shouldMove = true;

    void Start()
    {
        if (target.childCount > 0){
            // Get center of all GameObjects:
            foreach (Transform child in target){
                center += child.position;
            }
            center = center / target.childCount;
        } else{
            center = target.position;
        }
        transform.LookAt(center);        
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMove) transform.RotateAround(center, Vector3.up, 25 * Time.deltaTime);
    }

    public void pauseCamera(){
        shouldMove = false;
    }

    public void moveCamera(){
        shouldMove = true;
    }
}
