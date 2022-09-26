using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

    public Collider vertexCheck;
    int xSize = 50;
    int zSize = 50;
    
    Mesh terrainMesh;

    List<Vector3> vertices;
    List<int> vertexIndex;
    List<int> edgeVertices;
    List<Vector2> uvs;
    List<int> triangles;

    // Start is called before the first frame update
    void Start()
    {
        terrainMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = terrainMesh;
        // GetTileMesh();
        CreateShape();
        removeInvalidPoints();
        UpdateMesh();
    }

    void GetTileMesh(){
        ProBuilderMesh mesh = GetComponentInParent<ProBuilderMesh>();
        List<Vertex> verts = mesh.GetVertices().ToList<Vertex>();
        verts.RemoveAll(v => v.position.y < 0);
        string str = "";
        foreach(Vertex v in verts){
            str += transform.TransformPoint(v.position).ToString() + ", ";
        }
    }

    private bool inBounds(Vector3 point){
        Vector3 closest = vertexCheck.ClosestPoint(transform.TransformPoint(point));
        return closest == transform.TransformPoint(point);
    }

    private bool checkEdge(int index){
        // Check above & below:
        if(index > xSize && index + xSize < vertices.Count){
            // Bottom Edge:
            if (inBounds(vertices[index + xSize]) && !inBounds(vertices[index - xSize])){
                return true;
            }
            // Top edge:
            if (inBounds(vertices[index - xSize]) && !inBounds(vertices[index + xSize])){
                return true;
            } 
        }
        // Check left & right:
        if (index % 50 >= 2 && index % 50 <= 48){   
            // Right Edge:
            if (inBounds(vertices[index + 1]) && !inBounds(vertices[index - 1])){
                return true;
            }
            // Left edge:
            if (inBounds(vertices[index - 1]) && !inBounds(vertices[index + 1])){
                return true;
            } 
        }
        return false;
    }

    private void removeInvalidPoints(){
        vertexIndex = new List<int>();

        for (int i = 0; i < vertices.Count; i++){
            if (!inBounds(vertices[i])){
                vertexIndex.Add(i);
            } else{
                // Also check for edges:
                if (checkEdge(i)){
                    vertices[i] = new Vector3(vertices[i].x, 0f, vertices[i].z);
                }
            }
        }
        
        foreach(int index in vertexIndex){
            while(triangles.Contains(index)){
                int pos = triangles.IndexOf(index);
                int trianglePos = pos % 3;
                
                triangles.RemoveRange(pos - trianglePos, 3);
            }
        }
    }

    void CreateShape(){
        vertices = new List<Vector3>();
        uvs = new List<Vector2>();

        for(int i = 0, z = 0; z <= zSize; z++){
            for(int x = 0; x <= xSize; x++){
                float y;
                if (x == 0 || x == xSize || z == 0 || z == zSize){
                    y = 0f;
                } else{
                    y = Mathf.PerlinNoise((x) * 0.3f, (z) * 0.3f) / 10f;
                }
                Vector3 vertex = new Vector3((float)x / 20f, y, (float)z / 20f);
                vertices.Add(vertex);
                uvs.Add(new Vector2(x / (float)xSize, z / (float)zSize));
                i++;
            }
        }

        triangles = new List<int>(new int[zSize * xSize * 6]);
        int vert = 0;
        int tri = 0;
        for(int z = 0; z < zSize; z++){
            for(int x = 0; x < xSize; x++){
                triangles[tri + 0] = vert;
                triangles[tri + 1] = vert + xSize + 1;
                triangles[tri + 2] = vert + 1;
                triangles[tri + 3] = vert + 1;
                triangles[tri + 4] = vert + xSize + 1;
                triangles[tri + 5] = vert + xSize + 2;

                vert++;
                tri += 6;
            }
            vert++;
        }
    }

    void UpdateMesh(){
        terrainMesh.vertices = vertices.ToArray<Vector3>();
        terrainMesh.triangles = triangles.ToArray<int>();
        terrainMesh.uv = uvs.ToArray<Vector2>();
        terrainMesh.RecalculateNormals();
    }
}
