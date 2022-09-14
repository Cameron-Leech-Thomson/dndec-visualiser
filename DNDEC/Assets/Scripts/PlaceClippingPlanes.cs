using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceClippingPlanes : MonoBehaviour
{

    public GameObject planePrefab;
    public bool startWithPlanes = false;

    private float distance = 2.165f / 2f;
    private float angle = 60f;

    // Start is called before the first frame update
    void Start()
    {
        if(startWithPlanes){
            createPlanes();
        }
    }

    void createPlanes(){
        for (int i = 0; i < 6; i++)
        {
            Vector3 pos = Quaternion.Euler(0f, angle * i, 0f) * (transform.forward * distance) + transform.position;
            pos.y += 0.5f;

            Instantiate(planePrefab, pos, Quaternion.Euler(90f, angle * i, 0f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
