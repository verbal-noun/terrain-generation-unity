using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GenerateTerrain : MonoBehaviour
{
    public int n=7;
    public int startHeight = 5;
    public float maxHeightDiff = 6.0f;
    public float heightDepreciation = 0.7f;
    private int size;
    private int numVertices;
    private System.Random random = new System.Random();

    Mesh mesh;
    MeshCollider meshCollider;
    Vector3[] vertices;
    int[] triangles;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh; 
        MakeTerrain();
        UpdateMesh();
    }

    void Update()
    {
        // Generate new terrain when space is pressed
        if (Input.GetKeyDown("space")) {
            MakeTerrain();
            UpdateMesh();
        }
    }

    void MakeTerrain() {
        size = (int)Math.Pow(2, n)+1;
        numVertices = size*size;
        vertices = new Vector3[numVertices];

        // Create flat plane of vertices
        for (int z=0; z<size; z++) {
            for (int x=0; x<size; x++) {
                vertices[z*size+x] = new Vector3(x, startHeight, z);
            }
        }

        //Start diamond square algorithm on the vertices
        RecursiveDSquare(size-1, maxHeightDiff);

        // Render triangles of vertices
        triangles = new int[(size-1)*(size-1)*6];
        for (int z=0; z<size-1; z++) {
            for (int x=0; x<size-1; x++) {
                triangles[(z*(size-1)+x)*6] = z*size+x;
                triangles[(z*(size-1)+x)*6+1] = z*size+x+size; 
                triangles[(z*(size-1)+x)*6+2] = z*size+x+1;
                triangles[(z*(size-1)+x)*6+3] = z*size+x+1;
                triangles[(z*(size-1)+x)*6+4] = z*size+x+size;
                triangles[(z*(size-1)+x)*6+5] = z*size+x+size+1;
            }
        }
    }

    void RecursiveDSquare(int dim, float heightDiff) {
        if (dim == 1) {     // base case of lowest granularity step
            return;
        }

        // perform diamond step on respective vertices
        for (int z = 0; z < size-1; z+=dim){
            for (int x = 0; x < size-1; x+=dim){
                int centre = (int) ((z*size+x) + (dim*0.5*size + dim*0.5));
                DiamondStep(centre, dim, heightDiff);
            }
        }
        
        // perform square step on alternating rows
        for (int z = (int) (dim*0.5); z < size; z += dim) {
            for (int x = 0; x < size; x += dim){
                SquareStep(z*size + x, dim, heightDiff);
            }
        }
        
        // perform square step on other alternating rows
        for (int z = 0; z < size; z += dim) {
            for (int x = (int) (dim*0.5); x < size; x += dim) {
                SquareStep(z*size + x, dim, heightDiff);
            }
        }
        
        // reduce random height being added
        float newHeightDiff = (float) (heightDiff*heightDepreciation);

        RecursiveDSquare((int) (dim*0.5), newHeightDiff);
    }
    void DiamondStep(int centre, int dim, float heightDiff) {
        // calculate index values of the respective corner points
        int botL = (int) (centre-(size+1)*(dim/2));
        int botR = (int) (centre-(size-1)*(dim/2));
        int topL = (int) (centre+(size-1)*(dim/2));
        int topR = (int) (centre+(size+1)*(dim/2));

        // find average height of corner points
        float average = (float) ((vertices[botL].y + vertices[botR].y 
                                + vertices[topL].y + vertices[topR].y)/4);

        // add random value to average height
        float newY = (float) (average+random.NextDouble()*2*heightDiff - heightDiff);
        if (newY < 0) {
            newY = 0;
        }

        // set to new height
        Vector3 v = vertices[centre];
        vertices[centre] = new Vector3(v.x, newY , v.z);
    }
    void SquareStep(int centre, int dim, float heightDiff) {
        // calculate index values of the respective corner points
        int top = (int) (centre+size*(dim/2));
        int bot = (int) (centre-size*(dim/2));
        int left = (int) (centre-(dim/2));
        int right = (int) (centre+(dim/2));
        int validVertices = 3;
        
        // Mark any invalid corner as -1
        if (centre % size == 0) {
            left = -1;
        } else if ((centre+1)%size == 0) {
            right = -1;
        } else if (centre - size < 0) {
            bot = -1;
        } else if (centre + size >= size*size) {
            top = -1;
        } else {    // all 4 corners are valid so later divide by 4 instead
            validVertices = 4;
        }

        // add all heights of corners, excluding any invalid corners
        int[] vertIndices = {top, bot, left, right};
        float totalHeight = 0;
        for (int i=0; i<4; i++) {
            if (vertIndices[i] == -1) {
                continue;
            }
            totalHeight += vertices[vertIndices[i]].y;
        }

        // find average and calculate new height
        float average = (float) (totalHeight/validVertices);
        float newY = (float)(average+random.NextDouble()*2*heightDiff - heightDiff);
        if (newY < 0) {
            newY = 0;
        }

        // set new height
        Vector3 v = vertices[centre];
        vertices[centre] = new Vector3(v.x, newY , v.z);
    }
    void UpdateMesh() {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

}
