using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RotateCamera : MonoBehaviour
{
    [Tooltip("The parent object containing the object(s) that should be in focus.")]
    public Transform target;

    private int currentChildren = 0;
    private Vector3 center = Vector3.zero;
    private Vector3 defaultPosition; 
    private Vector3 editPosition = new Vector3(0, 20f, 0);
    private bool shouldMove = false;

    void Start()
    {
        defaultPosition = transform.position;
        if (target.childCount > 0){
            currentChildren = target.childCount;
            // Get center of all GameObjects:
            foreach (Transform child in target){
                center += child.position;
            }
            center = center / target.childCount;
        } else{
            center = target.position;
        }
        transform.LookAt(center);      
        editPosition = new Vector3(center.x, 20f, center.z);
        shouldMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMove){
            transform.RotateAround(center, Vector3.up, 20 * Time.deltaTime);
        } else{
            if (currentChildren != target.childCount){
                // Get center of all GameObjects:
                foreach (Transform child in target){
                    center += child.position;
                }
                center = center / target.childCount;    
                editPosition = new Vector3(center.x, 20f, center.z);
            }
        }
    }

    private void zoom(float inc){
        shouldMove = false;
        defaultPosition = new Vector3(defaultPosition.x, defaultPosition.y, defaultPosition.z + inc);
        transform.position = defaultPosition;
        transform.LookAt(center); 
        shouldMove = true;
    }

    public void zoomIn(){
        zoom(5f);
    }

    public void zoomOut(){
        zoom(-5f);
    }

    public void cameraEdit(){
        shouldMove = false;
        transform.position = editPosition;
        transform.LookAt(center);
    }

    public void pauseCamera(){
        shouldMove = false;
    }

    public void playCamera(){
        shouldMove = true;
    }

    public void moveCamera(){
        transform.position = defaultPosition;
        transform.LookAt(center);
        shouldMove = true;
    }
}
