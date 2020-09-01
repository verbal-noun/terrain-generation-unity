using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour
{
    public float initHeight = 10f;
    public int numVertices = 50;
    void Start()
    {
        // Add a MeshFilter component to this entity. This essentially comprises of a
        // mesh definition, which in this example is a collection of vertices, colours 
        // and triangles (groups of three vertices). 
        MeshFilter cubeMesh = this.gameObject.AddComponent<MeshFilter>();
        cubeMesh.mesh = this.CreateTerrainMesh();

        // Add a MeshRenderer component. This component actually renders the mesh that
        // is defined by the MeshFilter component.
        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        renderer.material.shader = Shader.Find("Unlit/VertexColorShader");
    }

    // Method to create terrain mesh using the diamond square algorithm
    Mesh CreateTerrainMesh()
    {
        Mesh m = new Mesh();
        m.name = "Terrain";

        Vector3[,] nodes = new Vector3[numVertices, numVertices];
        for (int x = 0; x < numVertices; x++){ 
            for (int z = 0; z < numVertices; z++){
                nodes[x,z] = new Vector3(x, 0, z);
            }
        }

        Vector3[] vertices = new Vector3[numVertices * numVertices];
        List<Vector3> newVertices = new List<Vector3>();
        List<Color> colors = new List<Color>();
        for (int x = 0; x < numVertices - 1; x++){ 
            for (int z = 0; z < numVertices - 1; z++){
                newVertices.Add(nodes[x, z]);
                newVertices.Add(nodes[x , z + 1]);
                newVertices.Add(nodes[x + 1, z + 1]);

                newVertices.Add(nodes[x, z]);
                newVertices.Add(nodes[x + 1, z + 1]);
                newVertices.Add(nodes[x+1, z]);
            }
        }

        for (int i = 0; i < newVertices.Count; i++){
            colors.Add(Color.white);
        }

        m.vertices = newVertices.ToArray();
        m.colors = colors.ToArray();
        
        int[] triangles = new int[m.vertices.Length];
        for (int i = 0; i < m.vertices.Length; i++)
            triangles[i] = i;
        
        m.triangles = triangles;

        return m;
    }
}
