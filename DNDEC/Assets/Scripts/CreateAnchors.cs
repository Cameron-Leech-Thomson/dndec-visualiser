using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAnchors : MonoBehaviour
{

    public GameObject anchor;
    public bool startWithAnchors = false;

    private float angle = 60f;
    private float distance = 2.165f;

    private List<GameObject> anchors = new List<GameObject>();

    void Start() {
        if (startWithAnchors){
            createAnchors();
            recalculateAnchors();
        }
    }

    public void createAnchors() {
        for (int i = 0; i < 6; i++)
        {
            Vector3 pos = Quaternion.Euler(0f, angle * i, 0f) * (transform.forward * distance) + transform.position;

            Collider[] hitColliders = Physics.OverlapSphere(pos, distance / 4, LayerMask.GetMask("Tile", "Tile Anchor"));
            if (hitColliders.Length == 0){
                placeAnchor(pos);
            }
        }
    }

    public void recalculateAnchors(){
        StartCoroutine(createNewAnchors());
    }

    private IEnumerator removeAnchors(){
        destroyAnchors();
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator createNewAnchors(){
        yield return StartCoroutine(removeAnchors());
        createAnchors();
    }

    // ONLY USE ON DESTROY OF A TILE:
    public void destroyAnchors(){
        foreach(GameObject anchor in anchors){
            Destroy(anchor);
        }
        anchors.Clear();
    }

    void placeAnchor(Vector3 position){
        GameObject obj = Instantiate(anchor, position, new Quaternion(0f, 0f, 0f, 1f)) as GameObject;
        obj.transform.parent = gameObject.transform;
        obj.GetComponent<SphereCollider>().radius = distance / 2;
        anchors.Add(obj);
    }

    public void removeAnchor(GameObject anchor){
        if (anchors.Contains(anchor)){
            anchors.Remove(anchor);
        } else{
            Debug.LogWarning("Attempted to remove anchor " + anchor.name +
                " (of " + anchor.transform.parent.gameObject.name + ") from tile " + gameObject.name + ".");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
