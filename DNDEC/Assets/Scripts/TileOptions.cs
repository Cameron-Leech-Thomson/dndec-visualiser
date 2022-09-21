using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TileOptions : MonoBehaviour
{
    public GameObject target;
    public MoveTile moveTile;
    public ErrorMessage errorMessage;
    private GameObject selectedObject = null;
    private List<Button> buttons;
    private bool placing = false;
    private bool donePlacing = false;
    private Camera cam;

    void Start() {
        cam = Camera.main;
        buttons = GetComponentsInChildren<Button>().ToList<Button>();
        setInteractable(false);
    }

    void Update() {
        if (donePlacing){
            selectedObject = null;
            placing = false;
            donePlacing = false;
        }
    }

    public void stopPlacing(){
        this.donePlacing = true;
    }

    public void deleteObject(){
        if (target.transform.childCount > 1){
            Destroy(selectedObject);
            setInteractable(false);
            selectedObject = null;
        } else{
            if (transform.parent.GetComponentInChildren<ErrorMessage>() == null){
                ErrorMessage error = Instantiate(errorMessage, Vector3.zero, new Quaternion(0f, 0f, 0f, 1f), transform.parent) as ErrorMessage;
                error.createErrorMessage("Cannot delete. You must keep at least one tile in the scene.");
            }            
        }
    }

    public void moveObject(){
        if (target.transform.childCount > 1){
            setInteractable(false);
            StopCoroutine(deselectObject());
            cam.GetComponent<RotateCamera>().cameraEdit();
            selectedObject.GetComponent<CreateAnchors>().destroyAnchors();
            selectedObject.layer = 0;
            StartCoroutine(moveTile.movingObject(selectedObject));
            placing = true;
        } else{
            if (transform.parent.GetComponentInChildren<ErrorMessage>() == null){
                ErrorMessage error = Instantiate(errorMessage, Vector3.zero, new Quaternion(0f, 0f, 0f, 1f), transform.parent) as ErrorMessage;
                error.createErrorMessage("Cannot Move Tile. There must be another tile to attach for a tile to be moved.");
            }            
        }
    }

    public void selectObject(GameObject obj){
        selectedObject = obj;
        setInteractable(true);
        StartCoroutine(deselectObject());
    }

    private void setInteractable(bool val){
        foreach(Button button in buttons){
            button.interactable = val;
        }
    }

    private IEnumerator deselectObject(){
        yield return new WaitForSeconds(10f);
        setInteractable(false);
        selectedObject = null;
    }
}
