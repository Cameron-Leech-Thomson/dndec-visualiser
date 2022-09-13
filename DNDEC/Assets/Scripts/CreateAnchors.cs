using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAnchors : MonoBehaviour
{

    public GameObject anchor;
    public bool startWithAnchors = false;

    private float angle = 60f;
    private float distance = 2.165f;

    void Start() {
        if (startWithAnchors){
            createAnchors();
        }
    }

    public void createAnchors() {
        for (int i = 0; i < 6; i++)
        {
            Vector3 pos = Quaternion.Euler(0f, angle * i, 0f) * (transform.forward * distance) + transform.position;

            Collider[] hitColliders = Physics.OverlapSphere(pos, distance / 4, LayerMask.GetMask("Tile", "Anchor"));
            if (hitColliders.Length == 0){
                placeAnchor(pos);
            }
        }
    }

    void placeAnchor(Vector3 position){
        GameObject obj = Instantiate(anchor, position, new Quaternion(0f, 0f, 0f, 1f)) as GameObject;
        obj.GetComponent<SphereCollider>().radius = distance / 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
