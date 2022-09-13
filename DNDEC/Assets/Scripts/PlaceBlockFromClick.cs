using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBlockFromClick : MonoBehaviour
{

    public GameObject objectToPlace;
    public Material objectMaterial;
    public GameObject targetParent;

    private List<Transform> sceneObjects = new List<Transform>();
    private LayerMask target;
    private GameObject placingObject = null;
    private bool placing = false;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        target = LayerMask.GetMask("Screencast Target", "Tile");
        foreach (Transform child in targetParent.transform){
            sceneObjects.Add(child);
        }
    }

    public void placeObject(){
        if (!placing){
            placing = true;
            placingObject = Instantiate(objectToPlace) as GameObject;
            placingObject.GetComponent<Renderer>().material = objectMaterial;
            (cam.GetComponent<RotateCamera>() as RotateCamera).pauseCamera();
        }        
    }

    private void Update() {
        if (placing){
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, target)){
                if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Tile")){
                    bool tooClose = false;
                    foreach (Transform pos in sceneObjects){
                        if(Vector3.Distance(pos.position, hit.point) < 2.165f){
                            tooClose = true;
                        }
                    }
                    if(!tooClose){
                        placingObject.transform.position = new Vector3(hit.point.x, 0f, hit.point.z); 
                    }         
                }        
            }

            if(Input.GetKeyDown(KeyCode.Mouse0)){
                placing = false;
                placingObject.layer = LayerMask.NameToLayer("Tile");
                placingObject = null;
                (cam.GetComponent<RotateCamera>() as RotateCamera).moveCamera();
            }
        }
    }
}
