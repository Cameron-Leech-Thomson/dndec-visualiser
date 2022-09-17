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
        target = LayerMask.GetMask("Screencast Target", "Tile", "Tile Anchor");
        foreach (Transform child in targetParent.transform){
            sceneObjects.Add(child);
        }
    }

    public void placeObject(){
        if (!placing){
            placing = true;
            placingObject = Instantiate(objectToPlace) as GameObject;
            placingObject.transform.parent = targetParent.transform;
            placingObject.GetComponent<Renderer>().material = objectMaterial;
            (cam.GetComponent<RotateCamera>() as RotateCamera).pauseCamera();
        }        
    }

    public bool isPlacing(){
        return placing;
    }

    private void Update() {
        if (placing){
            bool canPlace = false;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, target)){
                GameObject objectHit = hit.collider.gameObject;
                // Snapping to Anchors:
                if (objectHit.layer == LayerMask.NameToLayer("Tile Anchor")){
                    placingObject.transform.position = objectHit.transform.position;
                    canPlace = true;
                } // Stopping overlap: 
                else if (objectHit.layer != LayerMask.NameToLayer("Tile")){
                    bool tooClose = false;
                    canPlace = false;
                    foreach (Transform pos in sceneObjects){
                        if(Vector3.Distance(pos.position, hit.point) < 2.165f){
                            tooClose = true;
                        }
                    }
                    if(!tooClose){
                        placingObject.transform.position = new Vector3(hit.point.x, 0f, hit.point.z); 
                    }         
                }

                if(Input.GetKeyDown(KeyCode.Mouse0) && canPlace){
                    placing = false;
                    placingObject.layer = LayerMask.NameToLayer("Tile");
                    placingObject.GetComponent<CreateAnchors>().createAnchors();
                    placingObject = null;
                    Destroy(objectHit);
                    (cam.GetComponent<RotateCamera>() as RotateCamera).moveCamera();
                }
            }
        }
    }
}
