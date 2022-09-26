using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSkybox : MonoBehaviour
{
    public Material skybox = null;

    public void SetMaterial(){
        if (skybox == null) return;

        RenderSettings.skybox = skybox;
    }    
}
