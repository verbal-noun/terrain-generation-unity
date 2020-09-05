using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GenerateTerrain : MonoBehaviour
{
    Mesh mesh;
    public int n=2;
    public int startHeight = 2;
    public float maxHeightDiff = 2.0f;
    public float heightDepreciation = 0.7f;
    private int size;
    private int numVertices;
    private System.Random random = new System.Random();
    Vector3[] vertices;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        MakeTerrain();
        UpdateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void MakeTerrain() {
        size = (int)Math.Pow(2, n)+1;
        numVertices = size*size;
        vertices = new Vector3[numVertices];
        for (int z=0; z<size; z++) {
            for (int x=0; x<size; x++) {
                vertices[z*size+x] = new Vector3(x, startHeight, z);
            }
        }
        RecursiveDSquare(0, size-1, size*(size-1), size*size-1, maxHeightDiff);
    
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

    void RecursiveDSquare(int botL, int botR, int topL, int topR, float heightDiff) {
        if (botL == botR - 1) {
            return;
        }
        // if (heightDiff <= maxHeightDiff*0.7*0.7*0.7*0.7) {
        //     return;
        // }
        int left = (int) ((topL-botL)/2+botL);
        int dim = botR-botL;
        int centre = (int) (left+dim/2);
        DiamondStep(centre, dim, size, heightDiff);
        int top = (int) (centre+size*(dim/2));
        int bot = (int) (centre-size*(dim/2));
        int right = (int) (centre+(dim/2));
        SqaureStep(top, dim, size, heightDiff);
        SqaureStep(bot, dim, size, heightDiff);
        SqaureStep(left, dim, size, heightDiff);
        SqaureStep(right, dim, size, heightDiff);

        float newHeightDiff = (float) (heightDiff*heightDepreciation);
        RecursiveDSquare(left, centre, topL, top, newHeightDiff);
        RecursiveDSquare(centre, right, top, topR, newHeightDiff);
        RecursiveDSquare(botL, bot, left, centre, newHeightDiff);
        RecursiveDSquare(bot, botR, centre, right, newHeightDiff);
    }
    void DiamondStep(int centre, int dim, int size, float heightDiff) {
        int botL = (int) (centre-(size+1)*(dim/2));
        int botR = (int) (centre-(size-1)*(dim/2));
        int topL = (int) (centre+(size-1)*(dim/2));
        int topR = (int) (centre+(size+1)*(dim/2));
        float average = (float) ((vertices[botL].y + vertices[botR].y 
                                + vertices[topL].y + vertices[topR].y)/4);

        float newY = (float) (average+random.NextDouble()*2*heightDiff - heightDiff);
        if (newY < 0) {
            newY = 0;
        }
        Vector3 v = vertices[centre];
        vertices[centre] = new Vector3(v.x, newY , v.z);
    }
    void SqaureStep(int centre, int dim, int size, float heightDiff) {
        int top = (int) (centre+size*(dim/2));
        int bot = (int) (centre-size*(dim/2));
        int left = (int) (centre-(dim/2));
        int right = (int) (centre+(dim/2));
        int validVertices = 3;
        
        if (centre % size == 0) {
            left = -1;
        } else if ((centre+1)%size == 0) {
            right = -1;
        } else if (centre - size < 0) {
            bot = -1;
        } else if (centre + size >= size*size) {
            top = -1;
        } else {
            validVertices = 4;
        }

        int[] vertIndices = {top, bot, left, right};
        float totalHeight = 0;

        for (int i=0; i<4; i++) {
            if (vertIndices[i] == -1) {
                continue;
            }
            totalHeight += vertices[vertIndices[i]].y;
        }
        float average = (float) (totalHeight/validVertices);
        float newY = (float)(average+random.NextDouble()*2*heightDiff - heightDiff);
        if (newY < 0) {
            newY = 0;
        }
        Vector3 v = vertices[centre];
        vertices[centre] = new Vector3(v.x, newY , v.z);
    }
    void UpdateMesh() {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    // private void OnDrawGizmos() {
    //     if (vertices == null) {
    //         return;
    //     }
    //     for (int i=0; i<size*size; i++) {
    //         Gizmos.DrawSphere(vertices[i], .1f);
    //     }
    // }
}
