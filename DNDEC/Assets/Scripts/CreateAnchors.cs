using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class CreateAnchors : MonoBehaviour
{

    public GameObject anchor;
    public GameObject charAnchor;
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
            pos = new Vector3(pos.x, 0f, pos.z);

            Collider[] hitColliders = Physics.OverlapSphere(pos, distance / 4, LayerMask.GetMask("Tile", "Tile Anchor"));
            if (hitColliders.Length == 0){
                placeAnchor(pos);
            }
        }
        placeCharacterAnchor();
    }

    public void recalculateAnchors(){
        if (transform.parent.gameObject.activeSelf){
            StartCoroutine(createNewAnchors());
        }        
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
        if (transform.parent.gameObject.activeSelf){
            foreach(GameObject anchor in anchors){
                Destroy(anchor);
            }
            anchors.Clear();
        }
    }

    void placeAnchor(Vector3 position){
        GameObject obj = Instantiate(anchor, position, new Quaternion(0f, 0f, 0f, 1f), transform) as GameObject;
        obj.GetComponent<SphereCollider>().radius = distance / 2;
        anchors.Add(obj);
    }

    public void placeCharacterAnchor(){
        ProBuilderMesh mesh = gameObject.GetComponent<ProBuilderMesh>();
        Vertex[] vertices = mesh.GetVertices();
        float height = Mathf.Abs(vertices[0].position.y);
        Vector3 anchorPos = transform.position + new Vector3(0f, 1f + height, 0f);

        Collider[] hitColliders = Physics.OverlapSphere(anchorPos, distance / 4, LayerMask.GetMask("Character", "Character Anchor"));
        if (hitColliders.Length == 0){
            GameObject obj = Instantiate(charAnchor, anchorPos, Quaternion.Euler(0f,0f,0f), transform) as GameObject;
            obj.GetComponent<SphereCollider>().radius = distance / 4;
            anchors.Add(obj);
        }
    }

    public GameObject getCharacterAnchor(){
        foreach(GameObject anchor in anchors){
            if (anchor.layer == LayerMask.NameToLayer("Character Anchor")){
                return anchor;
            }
        }
        return null;
    }

    public void removeAnchor(GameObject anchor){
        if (anchors.Contains(anchor)){
            anchors.Remove(anchor);
            Destroy(anchor);
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
