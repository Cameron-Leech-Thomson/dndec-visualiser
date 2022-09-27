using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using TMPro;

public class Tile : MonoBehaviour
{

    private bool character = false;

    private bool isQuitting = false;

    private bool difficultTerrain = false;

    public bool hasCharacter(){
        return character;
    }

    public void addCharacter(string name, bool ally){
        if (!hasCharacter()){
            GameObject characterMarker;
            float height = getHeight();
            if (ally){
                characterMarker = Instantiate(Resources.Load<GameObject>("Prefabs/Character Marker"), transform.position + new Vector3(0f, 1f + height, 0f),
                    Quaternion.Euler(0f, 0f, 0f), transform);
            } else{
                characterMarker = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy Marker"), transform.position + new Vector3(0f, 1f + height, 0f),
                    Quaternion.Euler(0f, 0f, 0f), transform);
            }
            characterMarker.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
            characterMarker.GetComponentInChildren<TextMeshProUGUI>().text = name;

            character = true;

            CreateAnchors anchors = GetComponent<CreateAnchors>();
            anchors.removeAnchor(anchors.getCharacterAnchor());

            if (isDifficultTerrain()){
                GameObject terrainMarker = null;
                foreach(Transform child in transform){
                    if (child.gameObject.CompareTag("DifficultTerrain")){
                        terrainMarker = child.gameObject;
                    }
                }
                if (terrainMarker != null){
                    Vector3 pos = terrainMarker.transform.position;
                    terrainMarker.transform.position = new Vector3(pos.x, pos.y + 2.25f, pos.z);
                }
            }
        }        
    }

    public void removeCharacter(){
        if (hasCharacter()){
            GameObject characterMarker = null;
            foreach(Transform child in transform){
                if(child.gameObject.layer == LayerMask.NameToLayer("Character")){
                    characterMarker = child.gameObject;
                }
            }
            if (characterMarker != null){
                character = false;
                Destroy(characterMarker);

                CreateAnchors anchors = GetComponent<CreateAnchors>();
                anchors.placeCharacterAnchor();

                if(isDifficultTerrain()){
                    GameObject terrainMarker = null;
                    foreach(Transform child in transform){
                        if (child.gameObject.CompareTag("DifficultTerrain")){
                            terrainMarker = child.gameObject;
                        }
                    }
                    if (terrainMarker != null){
                        Vector3 pos = terrainMarker.transform.position;
                        terrainMarker.transform.position = new Vector3(pos.x, 1 + getHeight(), pos.z);
                    }
                }
            }
        }
    }

    private float getHeight(){
        ProBuilderMesh mesh = gameObject.GetComponent<ProBuilderMesh>();
        Vertex[] vertices = mesh.GetVertices();
        return Mathf.Abs(vertices[0].position.y);
    }

    public bool isDifficultTerrain(){
        return difficultTerrain;
    }

    public void setDifficultTerrain(bool val){
        difficultTerrain = val;
        if(difficultTerrain){
            GameObject terrainMarker = null;
            foreach(Transform child in transform){
                if (child.gameObject.CompareTag("DifficultTerrain")){
                    terrainMarker = child.gameObject;
                }
            }
            if (terrainMarker == null){
                float height = getHeight();

                if (hasCharacter()) height += 2.25f;

                Instantiate(Resources.Load<GameObject>("Prefabs/Difficult Terrain Marker"), transform.position + new Vector3(0f, 1f + height, 0f),
                    Quaternion.Euler(0f, 0f, 0f), transform);
            }
        }
        if (!difficultTerrain){
            GameObject terrainMarker = null;
            foreach(Transform child in transform){
                if (child.gameObject.CompareTag("DifficultTerrain")){
                    terrainMarker = child.gameObject;
                }
            }
            if (terrainMarker != null){
                Destroy(terrainMarker);
            }
        }
    }

    void OnApplicationQuit(){
        isQuitting = true;
    }

    private void OnDestroy() {
        if (!isQuitting){
            Transform parent = transform.parent;
            foreach(Transform child in parent){
                CreateAnchors anchors = child.gameObject.GetComponent<CreateAnchors>();
                if (!child.gameObject.Equals(gameObject)){
                    anchors.recalculateAnchors();
                } else{
                    anchors.destroyAnchors();
                }
            }
        }
    }
}

