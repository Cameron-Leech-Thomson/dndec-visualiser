using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.ProBuilder;

public class TileOptions : MonoBehaviour
{
    public GameObject target;
    public MoveTile moveTile;
    public ErrorMessage errorMessage;
    private GameObject selectedObject = null;
    private List<Button> buttons;
    private List<Coroutine> coroutines = new List<Coroutine>();
    private bool placing = false;
    private bool donePlacing = false;
    private Camera cam;
    private float minHeight = 0.25f;
    private float maxHeight = 5f;

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

    public void increaseHeight(){
        stopAllCoroutines();
        changeHeight(0.25f);
        StartCoroutine(deselectObject());
    }

    public void decreaseHeight(){
        stopAllCoroutines();
        changeHeight(-0.25f);
        StartCoroutine(deselectObject());
    }

    private void changeHeight(float diff){
        // Get the mesh data from ProBuilder:
        ProBuilderMesh mesh = selectedObject.GetComponent<ProBuilderMesh>();
        Vertex[] vertices = mesh.GetVertices();
        if (heightOutOfBounds(vertices[0], diff)){
            return;
        }
        List<Vector3> positions = new List<Vector3>();
        // Update the mesh data:
        foreach(Vertex v in vertices){
            float newY = v.position.y;
            if (newY > 0f){
                newY += diff;
            }
            else{
                newY -= diff;
            }
            Vector3 newPosition = new Vector3(v.position.x, newY, v.position.z);
            v.position = newPosition;
            positions.Add(newPosition);
        }
        // Apply the changes and rebuild the mesh:
        mesh.SetVertices(vertices);
        mesh.RebuildWithPositionsAndFaces(positions, mesh.faces);
        mesh.Refresh();
        // Move the object to be flush with the rest:
        Vector3 pos = selectedObject.transform.position;
        selectedObject.transform.position = new Vector3(pos.x, pos.y + diff, pos.z);
        // Update the anchor positions:
        selectedObject.GetComponent<CreateAnchors>().recalculateAnchors();
    }

    private bool heightOutOfBounds(Vertex v, float diff){
        // This is absolutely grim but I'm too tired to figure out a better way:
        float currentHeight = Mathf.Abs(v.position.y);
        if (currentHeight >= maxHeight || currentHeight <= minHeight){
            string message = null;
            if (currentHeight >= maxHeight && diff > 0f){
                message = "Maximum height reached.";
            } 
            if (currentHeight <= minHeight && diff < 0f){
                message = "Minimum height reached.";
            }
            if (message == null){
                return false;
            }
            if (transform.parent.GetComponentInChildren<ErrorMessage>() == null){
                ErrorMessage error = Instantiate(errorMessage, Vector3.zero, new Quaternion(0f, 0f, 0f, 1f), transform.parent) as ErrorMessage;
                error.createErrorMessage(message);
            }
            return true;
        } else{
            return false;
        }
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
            stopAllCoroutines();
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
        stopAllCoroutines();
        selectedObject = obj;
        setInteractable(true);
        StartCoroutine(deselectObject());
    }

    private void stopAllCoroutines(){
        foreach(Coroutine deselect in coroutines){
            StopCoroutine(deselect);
        }
        coroutines.Clear();
    }

    private void setInteractable(bool val){
        foreach(Button button in buttons){
            button.interactable = val;
        }
    }

    private IEnumerator deselectObject(){
        Coroutine deselection = StartCoroutine(deselectingObject());
        coroutines.Add(deselection);
        yield return deselection;
        coroutines.Remove(deselection);
    }

    private IEnumerator deselectingObject(){
        yield return new WaitForSeconds(10f);
        setInteractable(false);
        selectedObject = null;
    }
}
