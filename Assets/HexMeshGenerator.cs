using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MeshFilter))]
public class HexMeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    double scale = 1.0;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        // define hex top
        vertices = new Vector3[30];
        
        vertices[0] = new Vector3( 0, 0, (float) scale );
        vertices[1] = new Vector3( (float) (scale * Math.Sin(Math.PI / 3.0)), 0, (float) (scale * Math.Cos(Math.PI / 3.0)) );
        vertices[2] = new Vector3( (float) (scale * Math.Sin(Math.PI / 3.0)), 0, (float) ((-1.0 * scale * Math.Cos(Math.PI / 3.0))) );
        vertices[3] = new Vector3( 0, 0, (float) (-1 * scale) );
        vertices[4] = new Vector3( (float) ((-1.0 * scale * Math.Sin(Math.PI / 3.0))), 0, (float) ((-1.0 * scale * Math.Cos(Math.PI / 3.0))) );
        vertices[5] = new Vector3( (float) ((-1.0 * scale * Math.Sin(Math.PI / 3.0))), 0, (float) (scale * Math.Cos(Math.PI / 3.0)) );

        // define hex bottom
        int num = 6;

        for (int i = 0; i < num; i++)
        {
            Vector3 vert;

            vertices[num + i*4] = vertices[i];
            vert = vertices[i]; vert.y = (float)(-2 * scale);
            vertices[num + i*4 +1] = vert;

            int nextVertI = (i + 1) % 6;
            vertices[num + i*4 +2] = vertices[nextVertI];
            vert = vertices[nextVertI]; vert.y = (float)(-2 * scale);
            vertices[num + i*4 +3] = vert;
        }

        int numTrisVerts = 4 * 3 + 6 * 2 * 3;

        triangles = new int[numTrisVerts];


        int[] topTris = new int[]
        {
            0, 1, 2,
            0, 2, 3,
            0, 3, 4,
            0, 4, 5
        };
        int trisI = 0;
        for ( ; trisI < topTris.Length; trisI++)
        {
            triangles[trisI] = topTris[trisI];
        }

        for (int i = 0; i < 6; i++)
        {
            triangles[trisI] = num + i*4;
            trisI++;
            triangles[trisI] = num + i*4 + 1;
            trisI++;
            triangles[trisI] = num + i*4 + 3;
            trisI++;
            triangles[trisI] = num + i*4 + 3;
            trisI++;
            triangles[trisI] = num + i*4 + 2;
            trisI++;
            triangles[trisI] = num + i*4;
            trisI++;
        }

    
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        

        mesh.RecalculateNormals();
    }

}
