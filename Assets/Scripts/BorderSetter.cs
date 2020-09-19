using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creates a rectangular prism that will act as a collider for the camera
// so that it does not go beyond the boundaries
public class BorderSetter : MonoBehaviour
{
    public float size = 128f;
    public float borderHeight = 200f;
    Mesh mesh;
    MeshCollider meshCollider;
    Vector3[] vertices;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        
        // Altered version of cube creation from workshops so that we create a
        // suitable prism to use as our borders
        vertices = new[] {
            new Vector3(size, borderHeight, size),
            new Vector3(0f, borderHeight, size),
            new Vector3(0f, borderHeight, 0f),    // Top
            new Vector3(size, borderHeight, 0f),
            new Vector3(size, borderHeight, size),
            new Vector3(0f, borderHeight, 0f),

            new Vector3(0f, 0f, size),
            new Vector3(size, 0f, size),
            new Vector3(0f, 0f, 0f),               // Bottom
            new Vector3(size, 0f, size),
            new Vector3(size, 0f, 0f),
            new Vector3(0f, 0f, 0f),

            new Vector3(0f, borderHeight, size),
            new Vector3(0f, 0f, size),
            new Vector3(0f, 0f, 0f),           // Left
            new Vector3(0f, borderHeight, 0f),
            new Vector3(0f, borderHeight, size),
            new Vector3(0f, 0f, 0f),

            new Vector3(size, 0f, size),
            new Vector3(size, borderHeight, size),
            new Vector3(size, 0f, 0f),            // Right
            new Vector3(size, borderHeight, size),
            new Vector3(size, borderHeight, 0f),
            new Vector3(size, 0f, 0f),

            new Vector3(0f, borderHeight, size),
            new Vector3(size, borderHeight, size),
            new Vector3(size, 0f, size),             // front
            new Vector3(size, 0f, size), 
            new Vector3(0f, 0f, size),
            new Vector3(0f, borderHeight, size),

            new Vector3(0f, borderHeight, 0f),
            new Vector3(size, 0f, 0f),            // back            
            new Vector3(size, borderHeight, 0f),
            new Vector3(size, 0f, 0f),
            new Vector3(0f, borderHeight, 0f),            
            new Vector3(0f, 0f, 0f), 
        };

        // Assign the triangles with the vertices
        triangles = new int[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
            triangles[i] = i;

        UpdateMesh();
    }

    void UpdateMesh() {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

}
