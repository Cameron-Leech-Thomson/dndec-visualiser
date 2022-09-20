using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Linq;

public class PlacementController : MonoBehaviour
{
    public GameObject target;
    public GameObject menuContainer;
    public Camera mainCamera;
    public TileOptions options;

    private List<GameObject> tiles = new List<GameObject>();
    private List<PlaceBlockFromClick> buttons;
    private MaterialPropertyBlock materialPropertyBlock;

    private string emissionID = "_EmissionColor";
    private string emissionBool = "_EMISSION";

    // Start is called before the first frame update
    void Start()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlock.SetColor(emissionID, Color.red);
        foreach (Transform child in target.transform){
            tiles.Add(child.gameObject);
            Renderer renderer = child.gameObject.GetComponent<Renderer>();
            renderer.material.DisableKeyword(emissionBool);
            renderer.SetPropertyBlock(materialPropertyBlock);
        }
        buttons = menuContainer.GetComponentsInChildren<PlaceBlockFromClick>().ToList<PlaceBlockFromClick>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for new tiles:
        if (target.transform.childCount != tiles.Count){
            tiles.Clear();
            foreach (Transform child in target.transform){
                tiles.Add(child.gameObject);
            }
        }

        // ---------------------------- SELECTION HIGHLIGHTS:
        // Check if user is trying to place a tile:
        bool isPlacing = false;
        foreach(PlaceBlockFromClick tilePicker in buttons){
            if (tilePicker.isPlacing()){
                isPlacing = true;
            }
        }
        // Only allow user to select a tile if they aren't already trying to place one:
        if (!isPlacing){
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // If the user is hovering over a tile:
            if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Tile"))){
                GameObject obj = hit.collider.gameObject;
                // Apply an emission on the object:
                Renderer renderer = obj.GetComponent<Renderer>();
                renderer.material.EnableKeyword(emissionBool);

                // ---------------------------- SELECTION:
                bool isDestroyed = false;
                // If the user selects the tile:
                if(Input.GetKeyDown(KeyCode.Mouse0)){
                    options.selectObject(obj);
                }

                // Apply coroutine to turn it off after a set time, but only if the object hasn't been destroyed:
                if (!isDestroyed){
                    StartCoroutine(resetEmission(renderer));
                }                
            }
        }

        

    }
    

    private IEnumerator resetEmission(Renderer renderer){
        yield return new WaitForSeconds(0.5f);            
        if (renderer != null){
            renderer.material.DisableKeyword(emissionBool);
        }
    }
}
