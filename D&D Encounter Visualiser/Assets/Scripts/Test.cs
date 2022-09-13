using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in gameObject.transform){
            children.Add(child);
        }
        Debug.Log(Vector3.Distance(children[0].position, children[1].position));
        Debug.Log(Vector3.Distance(children[0].position, children[2].position));
        Debug.Log(Vector3.Distance(children[1].position, children[2].position));
    }
}
