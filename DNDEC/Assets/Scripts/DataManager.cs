using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.ProBuilder;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor.Events;

public class DataManager : MonoBehaviour
{
    public GameObject target;
    public GameObject uiContainer;
    public GameObject saveMenu;
    public GameObject loadMenu;

    [SerializeField] private Target _Target = new Target();

    private string path;

    private void Start() {
        path = Application.persistentDataPath + "/SavedEncounters/";
        LoadAllFiles();
    }

    public void LoadAllFiles(){
        string[] files = Directory.GetFiles(path, "*.json");
        string str = "";
        GameObject scrollContent = loadMenu.GetComponentInChildren<ScrollRect>().content.gameObject;
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

    public void LoadFile(string path){
        FindObjectOfType<PlacementController>().SetHighlightBool(true);
        uiContainer.SetActive(true);
        loadMenu.SetActive(false);
        Target loadedTarget = JsonUtility.FromJson<Target>(System.IO.File.ReadAllText(path));

        // Destroy all current tiles:
        foreach(Transform child in target.transform){
            Destroy(child.gameObject);
        }

        // Load each new tile:
        foreach(TileData tile in loadedTarget.tiles){
            AddTileFromData(tile);
        }
    }

    private void AddTileFromData(TileData data){
        Vector3 pos = new Vector3(data.pos[0], 0f, data.pos[2]);
        
        GameObject tile = Instantiate(Resources.Load<GameObject>("Prefabs/" + data.type), pos, new Quaternion(0f, 0f, 0f, 1f), target.transform);
        
        TileOptions options = FindObjectOfType<TileOptions>();
        options.selectObject(tile);
        for(int i = 0; i < data.height; i++){
            options.increaseHeight();
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
        tmp.horizontalAlignment = HorizontalAlignmentOptions.Left;
        return textObject;
    }

    public void SaveAsJson(){
        UpdateTargetData();
        string target = JsonUtility.ToJson(_Target);

        string name = saveMenu.GetComponentInChildren<TMP_InputField>().text;
        if (name == "" || name == null){
            name = ((System.DateTimeOffset)System.DateTime.Now).ToUnixTimeSeconds().ToString();
        }

        System.IO.File.WriteAllText(path + name + ".json", target);
        LoadAllFiles();
    }

    private void UpdateTargetData() {
        _Target.tiles.Clear();
        foreach(Transform tile in target.transform){
            TileData data = new TileData();
            // Get object type:
            string name = tile.gameObject.name;
            if (name.EndsWith("(Clone)")){
                int index = name.IndexOf('(');
                name = name.Substring(0, index);
            }
            data.type = name;
            // Get object position:
            Vector3 pos = tile.position;
            data.pos = new float[3]{
                pos.x,
                pos.y,
                pos.z
            };
            // Get object height:
            ProBuilderMesh mesh = tile.gameObject.GetComponent<ProBuilderMesh>();
            Vertex[] vertices = mesh.GetVertices();
            float height = (Mathf.Abs(vertices[0].position.y) / 0.25f) - 1;
            data.height = (int)height;
            // Add it to target data:
            _Target.tiles.Add(data);
        }
    }
}

[System.Serializable]
public class Target{
    public List<TileData> tiles = new List<TileData>();

    public override string ToString()
    {
        string str = "";

        for(int i = 0; i < tiles.Count; i++){
            str += i + ": " + tiles[i].ToString() + "\n";
        }

        return str;
    }
}

[System.Serializable]
public class TileData{
    // Type of object (grass, stone, dirt, etc)
    public string type;

    // x, y, z positions:
    public float[] pos;
    
    // number of times height was increased; 
    public int height;

    public override string ToString()
    {
        return "Tile of Type: " + type + " at Position: (" + pos[0] + ", " + pos[1] + ", " + pos[2] + ") with Height: " + height;
    }
}