using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    public void loadScene(string sceneName){
        Scene currentScene = SceneManager.GetActiveScene();
        // Load new scene:
        SceneManager.LoadScene(sceneName);
        // Unload old scene:
        SceneManager.UnloadSceneAsync(currentScene.name);
    }

}
