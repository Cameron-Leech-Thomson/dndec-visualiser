using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveTile : MonoBehaviour
{

    private GameObject targetParent;
    private LayerMask targetLayer;
    private Camera cam;
    private bool shouldMove = false;
    private GameObject selectedObject = null;

    // Start is called before the first frame update
    void Start()
    {
        targetParent = GetComponent<PlacementController>().target;
        cam = Camera.main;
        targetLayer = LayerMask.GetMask("Screencast Target", "Tile", "Tile Anchor");
    }

    void Update(){
        if (shouldMove){
            if (selectedObject == null){
                shouldMove = false;
            }

            bool canPlace = false;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, targetLayer)){
                GameObject objectHit = hit.collider.gameObject;
                Tile tile = selectedObject.GetComponent<Tile>();
                float height = tile.getHeight();
                // Snapping to Anchors:
                if (objectHit.layer == LayerMask.NameToLayer("Tile Anchor")){
                    selectedObject.transform.position = objectHit.transform.position + new Vector3(0f, height - 0.25f, 0f);
                    canPlace = true;
                } // Stopping overlap: 
                else if (objectHit.layer != LayerMask.NameToLayer("Tile")){
                    bool tooClose = false;
                    canPlace = false;
                    foreach (Transform pos in targetParent.transform){
                        if(Vector3.Distance(pos.position, hit.point) < 2.165f){
                            tooClose = true;
                        }
                    }
                    if(!tooClose){
                        selectedObject.transform.position = new Vector3(hit.point.x, height - 0.25f, hit.point.z); 
                    }         
                }

                if(Input.GetKeyDown(KeyCode.Mouse0) && canPlace){
                    selectedObject.layer = LayerMask.NameToLayer("Tile");
                    selectedObject.GetComponent<CreateAnchors>().createAnchors();
                    Destroy(objectHit);
                    shouldMove = false;
                } else if (Input.GetKeyDown(KeyCode.Mouse1) && targetParent.transform.childCount > 1){
                    Destroy(selectedObject);
                    shouldMove = false;
                }
            }
        }
    }

    public IEnumerator movingObject(GameObject selectedObject){
        shouldMove = true;
        this.selectedObject = selectedObject;
        foreach(Transform tile in targetParent.transform){
            if (tile.gameObject != selectedObject){
                tile.gameObject.GetComponent<CreateAnchors>().recalculateAnchors();
            }
        }
        while (shouldMove){
            yield return null;
        }
        stopMovement();
        yield return new WaitForSeconds(0.1f);
        (cam.GetComponent<RotateCamera>() as RotateCamera).moveCamera();
    }

    private void stopMovement(){
        gameObject.GetComponent<PlacementController>().options.stopPlacing();
        List<PlaceBlockFromClick> placements = GameObject.FindObjectsOfType<PlaceBlockFromClick>().ToList();
        foreach(PlaceBlockFromClick pbc in placements){
            pbc.stopPlacing();
        }
        foreach(Transform child in targetParent.transform){
            child.gameObject.GetComponent<CreateAnchors>().recalculateAnchors();
        }
    }

    public bool isMoving(){
        return shouldMove;
    }
}
