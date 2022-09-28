using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MoveCharacter : MonoBehaviour
{

    public GameObject uiContainer;

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

    private void setInteractable(bool val){
        foreach(Transform child in uiContainer.transform){
            Button button = child.gameObject.GetComponent<Button>();
            if (button == null){
                foreach(Transform subChild in child){
                    Button subButton = subChild.gameObject.GetComponent<Button>();
                    if (subButton != null){
                        subButton.interactable = val;
                    }
                }
            } else{
                button.interactable = val;
            }
        }
    }

    public IEnumerator movingObject(GameObject selectedObject){
        shouldMove = true;
        this.selectedObject = selectedObject;
        setInteractable(false);
        foreach(Transform tile in targetParent.transform){
            tile.gameObject.GetComponent<Tile>().UpdateMarkers();
            if (tile.gameObject != selectedObject){
                tile.gameObject.GetComponent<CreateAnchors>().recalculateAnchors();
            }
        }
        while (shouldMove){
            yield return null;
        }
        setInteractable(true);
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
