using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    // Start is called before the first frame update
    public int size = 10;
    Mesh mesh;
    MeshCollider meshCollider;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;
    private int numVertices;
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh; 
        CreatePlane();
        UpdateMesh();
        
    }

    void CreatePlane(){
        numVertices = size * size;
        vertices = new Vector3[numVertices];

        // Create flat plane of vertices
        for (int z = 0; z < size; z++) {
            for (int x = 0; x < size; x++) {
                vertices[z * size + x] = new Vector3 (x, 0, z);
            }
        }
        // Render triangles of vertices
        triangles = new int[(size - 1) * (size - 1) * 6];
        for (int z = 0; z < size - 1; z++) {
            for (int x = 0; x < size - 1; x++) {
                triangles[(z * (size - 1) + x) * 6] = z * size + x;
                triangles[(z * (size - 1) + x) * 6 + 1] = z * size + x + size;
                triangles[(z * (size - 1) + x) * 6 + 2] = z * size + x + 1;
                triangles[(z * (size - 1) + x) * 6 + 3] = z * size + x + 1;
                triangles[(z * (size - 1) + x) * 6 + 4] = z * size + x + size;
                triangles[(z * (size - 1) + x) * 6 + 5] = z * size + x + size + 1;
            }
        }
        // Defining the uvs 
        uvs = new Vector2[numVertices];
        for (int i = 0, z = 0; z < size; z++) {
            for (int x = 0; x < size; x++) {
                uvs[i] = new Vector2 ((float) x / size, (float) z / size);
                i++;
            }
        }
    }
    void UpdateMesh () {
        mesh.Clear ();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals ();
        GetComponent<MeshCollider> ().sharedMesh = mesh;
    }

}
