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
                updateCenter();
            }
        }
    }

    private void updateCenter(){
        Bounds bounds = target.gameObject.GetComponent<Renderer>().bounds;
        // Get center of all GameObjects:
        foreach (Transform child in target){
            center += child.position;
            bounds.Encapsulate(child.gameObject.GetComponent<Renderer>().bounds);
        }
        currentChildren = target.childCount;
        float width = bounds.size.x;
        float height = bounds.size.z;

        float y = 20f;
        if (width > 20f || height > 10f){
            float widthExcess = width - 20f;
            float heightExcess = height - 10f;
            if (widthExcess < 0 && heightExcess > 0){
                y = 20f + (heightExcess / 2);
            } else if (widthExcess > 0 && heightExcess < 0){
                y = 20f + (widthExcess / 2);
            } else{
                y = 20f + ((widthExcess + heightExcess) / 2);
            }
        }

        center = bounds.center;
        editPosition = new Vector3(center.x, y, center.z);
    }

    private void zoom(float inc){
        shouldMove = false;
        defaultPosition = new Vector3(defaultPosition.x, defaultPosition.y, defaultPosition.z + inc);
        transform.position = defaultPosition;
        transform.LookAt(center); 
        shouldMove = true;
    }

    public void zoomIn(){
        if (defaultPosition.z + 5f <= 0){
            zoom(5f);
        }        
    }

    public void zoomOut(){
        zoom(-5f);
    }

    public void cameraEdit(){
        shouldMove = false;
        transform.position = editPosition;
        updateCenter();
        transform.LookAt(center);
    }

    public void pauseCamera(){
        shouldMove = false;
    }

    public void playCamera(){
        shouldMove = true;
    }

    public void moveCamera(){
        updateCenter();
        transform.position = defaultPosition;
        transform.LookAt(center);
        shouldMove = true;
    }
}
