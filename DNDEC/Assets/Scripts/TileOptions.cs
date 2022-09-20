using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TileOptions : MonoBehaviour
{
    public GameObject target;
    public ErrorMessage errorMessage;
    private GameObject selectedObject = null;
    private List<Button> buttons;

    void Start() {
        buttons = GetComponentsInChildren<Button>().ToList<Button>();
        setInteractable(false);
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

    public void selectObject(GameObject obj){
        selectedObject = obj;
        setInteractable(true);
        deselectObject();
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
