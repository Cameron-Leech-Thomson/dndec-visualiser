using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveCharacter : MonoBehaviour
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
        targetLayer = LayerMask.GetMask("Screencast Target", "Character Anchor", "Character");
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
                // Snapping to Anchors:
                if (objectHit.layer == LayerMask.NameToLayer("Character Anchor")){
                    selectedObject.transform.position = objectHit.transform.position;
                    canPlace = true;
                } else{
                    selectedObject.transform.position = hit.point;
                }                

                if(Input.GetKeyDown(KeyCode.Mouse0) && canPlace){
                    selectedObject.layer = LayerMask.NameToLayer("Character");
                    selectedObject.transform.parent = objectHit.transform.parent;
                    selectedObject.transform.position = objectHit.transform.position;
                    Destroy(objectHit);
                    shouldMove = false;
                } else if (Input.GetKeyDown(KeyCode.Mouse1)){
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
            tile.gameObject.GetComponent<Tile>().UpdateMarkers();
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
        foreach(Transform tile in targetParent.transform){
            tile.gameObject.GetComponent<CreateAnchors>().recalculateAnchors();
            tile.gameObject.GetComponent<Tile>().UpdateMarkers();
        }
    }

    public bool isMoving(){
        return shouldMove;
    }
}
