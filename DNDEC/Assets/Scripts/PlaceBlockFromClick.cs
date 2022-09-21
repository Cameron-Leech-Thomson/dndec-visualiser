using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBlockFromClick : MonoBehaviour
{

    public GameObject objectToPlace;
    public GameObject targetParent;
    public MoveTile moveTile;
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
