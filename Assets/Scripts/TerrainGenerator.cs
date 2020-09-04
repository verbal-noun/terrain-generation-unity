
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TerrainGenerator : MonoBehaviour
{
    public float initHeight = 60f;
    [Range(5, 30)]
    public float startingRandomHeight = 10f;
    public int size = 65;

    private float randomHeight;

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

        Vector3[,] nodes = new Vector3[size, size];
        for (int x = 0; x < size; x++){ 
            for (int z = 0; z < size; z++){
                nodes[x,z] = new Vector3(x, 0, z);
            }
        }

        GenerateHeightMap(nodes);

        Vector3[] vertices = new Vector3[size * size];
        List<Vector3> newVertices = new List<Vector3>();
        List<Color> colors = new List<Color>();
        for (int x = 0; x < size - 1; x++){ 
            for (int z = 0; z < size - 1; z++){
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

    void GenerateHeightMap(Vector3[,] nodes){
        int n = size;
        randomHeight = startingRandomHeight;
        float height = Random.Range(0, initHeight);
        nodes[0,0].y = height;
        nodes[0, n - 1].y = height;
        nodes[n-1, 0].y = height;
        nodes[n-1, n-1].y = height;
        DiamondSquareAlgo(nodes, size - 1);
    }

    void DiamondSquareAlgo(Vector3[,] nodes, int n){
        int half = n/2;
        if (half < 1) return;
        for (int x = 0; x < size - 1; x+=n){
            for (int z = 0; z < size - 1; z+=n){
                DiamondStep(nodes, x, z, half, n);
            }
        }

        for(int x = 0; x < size - 1; x += half){
            for (int z = (x + half) % n; z < size - 1; z += n){
                SquareStep(nodes, x, z, half, n);
            }
        }
        randomHeight = Mathf.Max(randomHeight / 2, 1);
        DiamondSquareAlgo(nodes, half);
    }

    void DiamondStep(Vector3[,] nodes, int x, int z, int halfWidth, int n){
        Vector3 botLeft = nodes[x,z];
        Vector3 topRight = nodes [x + n, z + n];
        Vector3 topLeft = nodes[x, z + n];
        Vector3 botRight = nodes[x + n, z];
        float newHeight = (topLeft.y + topRight.y + botRight.y + botLeft.y)/4 + Random.Range(-randomHeight, randomHeight); 

        nodes[x + halfWidth, z + halfWidth].y = newHeight;

    }

    void SquareStep(Vector3[,] nodes, int x, int z, int halfWidth, int n){
        Vector3 left = nodes[(x - halfWidth + size - 1) % (size - 1),z];
        Vector3 right = nodes[(x + halfWidth) % (size - 1), z];
        Vector3 bottom = nodes[x,(z - halfWidth + size - 1) % (size - 1)];
        Vector3 top = nodes[x, (z + halfWidth) % (size - 1)];
        float newHeight = (top.y + left.y + right.y + bottom.y)/4 + Random.Range(-randomHeight, randomHeight); 
        nodes[x,z].y = newHeight;

        
        if (x == 0){
            nodes[size - 1, z].y = newHeight;
        }
        if (z == 0){
            nodes[x, size - 1].y = newHeight;
        }   
    }
}
