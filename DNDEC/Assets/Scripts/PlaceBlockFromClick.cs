using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBlockFromClick : MonoBehaviour
{

    public GameObject objectToPlace;
    public GameObject targetParent;
    public MoveTile moveTile;
    public bool rotateTop = false;
    private GameObject placingObject = null;
    private bool placing = false;
    private bool donePlacing = false;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    public void stopPlacing(){
        this.donePlacing = true;
    }

    public void placeObject(){
        if (!placing){
            placing = true;
            placingObject = Instantiate(objectToPlace) as GameObject;
            if (rotateTop){
                Transform top = placingObject.transform.GetChild(0);
                float rotation = Random.Range(0,6);
                top.rotation = Quaternion.Euler(0f, 60 * rotation, 0f);
            }
            placingObject.transform.parent = targetParent.transform;
            (cam.GetComponent<RotateCamera>() as RotateCamera).cameraEdit();
            StartCoroutine(moveTile.movingObject(placingObject));
        }        
    }

    public bool isPlacing(){
        return placing;
    }

    private void Update() {
        if (donePlacing){
            placingObject = null;
            placing = false;
            donePlacing = false;
        }
    }
}
