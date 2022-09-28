using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.ProBuilder;
using TMPro;
using UnityEngine.UI;

public class DataManagerMenu : MonoBehaviour
{
    public GameObject scrollMenu;
    private string path;

    private void Start(){
        path = Application.persistentDataPath + "/SavedEncounters/";
        if (!Directory.Exists(path)){
            Directory.CreateDirectory(path);
        }
        LoadAllFiles();
    }

    public void LoadAllFiles(){
        string[] files = Directory.GetFiles(path, "*.json");
        string str = "";
        GameObject scrollContent = scrollMenu.GetComponentInChildren<ScrollRect>().content.gameObject;
        foreach(Transform child in scrollContent.transform){
            Destroy(child.gameObject);
        }

        foreach(string path in files){
            int start = path.LastIndexOf('/') + 1;
            int len = path.Length - start - 5;
            string fileName = path.Substring(start, len);
            str += fileName + ", ";

            GameObject textObject = Instantiate(CreateTextObject(fileName), scrollContent.transform);
            Button button = textObject.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener((delegate { LoadFile(path); } ));
        }
    }

    private GameObject CreateTextObject(string text){
        GameObject textObject = new GameObject("File - " + text);
        TextMeshProUGUI tmp = textObject.AddComponent<TextMeshProUGUI>();
        Button button = textObject.AddComponent<Button>();
        ColorBlock colours = button.colors;
        colours.highlightedColor = new Color(0.5f, 0.5f, 0.5f, 1f);
        button.colors = colours;
        tmp.text = text;
        tmp.fontSize = 30;
        tmp.verticalAlignment = VerticalAlignmentOptions.Middle;
        tmp.horizontalAlignment = HorizontalAlignmentOptions.Center;
        return textObject;
    }

    public void LoadFile(string path){
        PlayerPrefs.SetString("toLoad", path);

        SceneLoader sceneLoader = GetComponent<SceneLoader>();
        sceneLoader.loadScene("Game");
    }
}
