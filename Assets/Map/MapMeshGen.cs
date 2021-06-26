using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapMeshGen
{
    Map map;
    Dictionary<string, MapGenTile> genTiles;
    Dictionary<int, Tile> tiles;

    //float scale = 100f;
    //double sin60 = 0.08888;
    
    int width;
    int realWidth;
    int height;

    public MapMeshGen(Map Map, Dictionary<int, Tile> Tiles, int Width, int Height) {
        map = Map;
        tiles = Tiles;
        width = Width;
        realWidth = Width + Width - 1;
        height = Height;

        //scale = map.scale;
    }

    public void Generate() {

        Vector3[] vertices = new Vector3[tiles.Count];

        List<int> tileList = tiles.Keys.ToList();
        tileList.Sort();

        int i = 0;
        foreach (int index in tileList) {
            //int index = kvp.Key;
            //Debug.Log("<color=green>" + index + "</color>");
            //Tile tile = kvp.Value;
            vertices[i] = tiles[index].position;
            i++;
        }

        int trisPerRow = width + width - 3; // (w-1) + (w-2)
        int numTris = trisPerRow
            * (height - 1); // for every row except the first or last

        int numTriVerts = numTris * 3; // 3 verts per tri

        int[] triangles = new int[numTriVerts];
        

        int tri = 0;
        for (int z = 0; z < (height-1); z++) { // height-1 to skip last row

            int trisThisRow = 0;

            for (int x = 0; x < realWidth; x++) {

                if (map.TileExists(x, z)) {

                    if (x >= 1 && x <= (realWidth - 2)) {


                        //triangles[tri * 3 + 0] = x + (z * realWidth);
                        //triangles[tri * 3 + 1] = (x - 1) + ((z + 1) * realWidth);
                        //triangles[tri * 3 + 2] = (x + 1) + ((z + 1) * realWidth);

                        /*
                        Debug.Log(x + ", " + z);

                        Debug.Log( x/2 );
                        Debug.Log( (z * (width-1) + z/2) );

                        Debug.Log( (x-1)/2 );
                        Debug.Log( ((z+1) * (width-1) + (z+1)/2) );

                        Debug.Log( (x+1)/2 );
                        Debug.Log( ((z+1) * (width-1) + (z+1)/2) );
                        */

                        triangles[tri * 3 + 0] = x/2     + (z * (width-1) + z/2);
                        triangles[tri * 3 + 1] = (x-1)/2 + ((z+1) * (width-1) + (z+1)/2);
                        triangles[tri * 3 + 2] = (x+1)/2 + ((z+1) * (width-1) + (z+1)/2);

                        //Debug.Log( triangles[tri * 3 + 0] );
                        //Debug.Log( triangles[tri * 3 + 1] );
                        //Debug.Log( triangles[tri * 3 + 2] );
                        
                        trisThisRow++;

                        tri++;
                    }

                    if (x <= (realWidth - 3)) {


                        //triangles[tri * 3 + 0] = x + (z * realWidth);
                        //triangles[tri * 3 + 1] = (x + 1) + ((z+1) * realWidth);
                        //triangles[tri * 3 + 2] = (x + 2) + ((z+1) * realWidth);

                        triangles[tri * 3 + 0] = x/2     + (z * (width-1) + z/2);
                        triangles[tri * 3 + 1] = (x+1)/2 + ((z+1) * (width-1) + (z+1)/2);
                        triangles[tri * 3 + 2] = (x+2)/2 + (z * (width-1) + z/2);
                        
                        //Debug.Log( triangles[tri * 3 + 0] );
                        //Debug.Log( triangles[tri * 3 + 1] );
                        //Debug.Log( triangles[tri * 3 + 2] );

                        trisThisRow++;

                        tri++;
                    }

                }

            }
            
            
        }

        //End:

        Mesh mesh = new Mesh();
        
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        Global global = (Global)GameObject.FindObjectOfType<Global>();
        global.mapMesh.GetComponent<MeshFilter>().mesh = mesh;

        Debug.Log("Setting Mesh");

        // <MapGenTile> tiles
        //int size = tiles.Legnth;
        //Vector3[] points = Vector3[size];

        //z
        //x
        // if valid
        // add point to grid

        // make tris
    }
}
