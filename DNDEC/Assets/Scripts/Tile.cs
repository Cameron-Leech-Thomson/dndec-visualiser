using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private bool isQuitting = false;

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
