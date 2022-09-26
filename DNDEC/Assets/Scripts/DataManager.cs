using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using TMPro;

public class DataManager : MonoBehaviour
{
    public GameObject target;
    public GameObject uiContainer;
    public GameObject saveMenu;

    [SerializeField] private Target _Target = new Target();

    public void SaveAsJson(){
        UpdateTargetData();
        string target = JsonUtility.ToJson(_Target);

        string name = saveMenu.GetComponentInChildren<TMP_InputField>().text;
        if (name == "" || name == null){
            name = ((System.DateTimeOffset)System.DateTime.Now).ToUnixTimeSeconds().ToString();
        }
        
        System.IO.File.WriteAllText(Application.persistentDataPath + "/SavedEncounters/" + name + ".json", target);
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
}

[System.Serializable]
public class TileData{
    // Type of object (grass, stone, dirt, etc)
    public string type;

    // x, y, z positions:
    public float[] pos;
    
    // number of times height was increased; 
    public int height;
}
