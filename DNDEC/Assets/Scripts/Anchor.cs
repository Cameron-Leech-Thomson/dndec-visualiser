using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{
    private void OnDestroy() {
        if (transform.parent != null){
            CreateAnchors anchors = GetComponentInParent<CreateAnchors>();
            anchors.removeAnchor(gameObject);
        }
    }
}
