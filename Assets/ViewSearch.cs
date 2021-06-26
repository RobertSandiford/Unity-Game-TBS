using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ViewHex
{
    public int[] tile;
    public double startAngle;
    public double endAngle;
    public double vAngle;
    public bool visible;
    public ViewHex(int[] Tile, double StartAngle, double EndAngle, double VAngle, bool Visible)
    {
        tile = Tile;
        startAngle = StartAngle;
        endAngle = EndAngle;
        vAngle = VAngle;
        visible = Visible;
    }
}

public struct ViewRing
{
    public ViewHex[] hexes;
    public ViewRing(ViewHex[] Hexes)
    {
        hexes = Hexes;
    }
}

public class ViewSearch
{
    List<ViewRing> rings;
    List<ViewHex> visibleHexes;
    Map map;
    int altitude;
    Dictionary<int, double> heights;

    static double angleFudge = 0.05;
    private double sin60 = 0.86602540378443864676372317075294;

    bool debug = false;
    double unitHeight = 1.5;

    double scale;

    public ViewSearch(Map Map)
    {
        map = Map;
        altitude = 0;

        Init();
    }

    public ViewSearch(Map Map, int Altitude = 0)
    {
        map = Map;
        altitude = Altitude;

        Init();
    }

    public void Init() {
        scale = map.scale;
        heights = new Dictionary<int, double> {
            { 2, (map.forestHeight) },
            { 3, (map.urbanHeight) }
        };
    }

    double TileHeight(Tile tile)
    {
        if (tile.terrain.id == 2) return heights[2];
        if (tile.terrain.id == 3) return heights[3];
        return 0.0;
    }

    public List<ViewHex> FindVisibleTiles(Tile startTile)
    {
        //Debug.Log("<color=red>Searching</color>");
        rings = new List<ViewRing>();
        visibleHexes = new List<ViewHex>();
        visibleHexes.Add(new ViewHex(new int[] { startTile.x, startTile.z}, 0, 0, 0, true)); // add the starting hex
        Make1stRing(startTile);
        //int nMax = 36;
        int nMax = (int)Math.Round(5000 / MapDefs.hexWidth);
        for (int n = 2; n <= nMax; n++)
        {
            MakeNRing(startTile, n);
        }
        return visibleHexes;
    }

    public void Make1stRing(Tile startTile)
    {
        double sixth = (Math.PI / 3.0);

        int[][] hexes = new int[][]
        {
        new int[] { startTile.x + 1, startTile.z + 1 },
        new int[] { startTile.x + 2, startTile.z },
        new int[] { startTile.x + 1, startTile.z - 1 },
        new int[] { startTile.x - 1, startTile.z - 1 },
        new int[] { startTile.x - 2, startTile.z },
        new int[] { startTile.x - 1, startTile.z + 1 },
        };
        List<ViewHex> viewHexes = new List<ViewHex>();
        for (int i = 0; i < 6; i++)
        {
            if (map.TileExists(hexes[i][0], hexes[i][1]))
            {
                Tile toTile = map.GetTileFromArray(hexes[i]);
                double dist = map.GetRealDistanceBetweenHexes(startTile, toTile);
                double heightDiffB = toTile.y + TileHeight(toTile) - (startTile.y + altitude + unitHeight);
                

                double vAngle = Math.Atan(heightDiffB / dist);
                

                ViewHex vh = new ViewHex(hexes[i], StandardiseAngleRadians(i * sixth - angleFudge), StandardiseAngleRadians((i + 1) * sixth + angleFudge), vAngle, true);
                viewHexes.Add(vh);
                visibleHexes.Add(vh);
            }
            else
            {
                //Debug.Log("Tile didn't eixst");
            }
        }
        ViewHex[] viewHexesArray = new ViewHex[viewHexes.Count];
        for (int i = 0; i < viewHexes.Count; i++)
        {
            viewHexesArray[i] = viewHexes[i];
        }
        ViewRing viewRing = new ViewRing(viewHexesArray);

        rings.Insert(0, viewRing);
    }

    public void MakeNRing(Tile startTile, int ring = 2)
    {
        //double twelth = (Math.PI / 6.0);
        double ringAngleFudge = angleFudge / (double)ring;

        int size = ring * 6;

        int[,] hexes = new int[size, 2];
        for (int i = 0; i < size; i++)
        {
            if (i < ring)
            {
                hexes[i, 0] = startTile.x + ring + i;
                hexes[i, 1] = startTile.z + ring - i;
            }
            if (i >= ring && i < ring * 2)
            {
                int j = i - ring;
                hexes[i, 0] = startTile.x + ring * 2 - j;
                hexes[i, 1] = startTile.z - j;
            }
            if (i >= ring * 2 && i < ring * 3)
            {
                int j = i - ring * 2;
                hexes[i, 0] = startTile.x + ring - j * 2;
                hexes[i, 1] = startTile.z - ring;
            }
            if (i >= ring * 3 && i < ring * 4)
            {
                int j = i - ring * 3;
                hexes[i, 0] = startTile.x - ring - j;
                hexes[i, 1] = startTile.z - ring + j;
            }
            if (i >= ring * 4 && i < ring * 5)
            {
                int j = i - ring * 4;
                hexes[i, 0] = startTile.x - ring * 2 + j;
                hexes[i, 1] = startTile.z + j;
            }
            if (i >= ring * 5 && i < ring * 6)
            {
                int j = i - ring * 5;
                hexes[i, 0] = startTile.x - ring + j * 2;
                hexes[i, 1] = startTile.z + ring;
            }
        }

        List<ViewHex> viewHexes = new List<ViewHex>();
        for (int i = 0; i < size; i++)
        {
            int x = hexes[i, 0];
            int z = hexes[i, 1];
            int[] hex = new int[2] { x, z };

            if (map.TileExists(hex[0], hex[1]))
            {
                //if ( hex[0] == (startTile.x +1) && hex[1] == (startTile.z + 3) ) debug = true;

                Tile toTile = map.GetTileFromArray(hex);
                Vector3 vec = toTile.position - startTile.position;
                
                //Debug.Log("-----");
                //Debug.Log(startTile.position);
                //Debug.Log(toTile.position);
                //Vector2 vec2 = new Vector2(vec.x, vec.z);

                if (debug) Debug.Log("<color=blue>"+ (toTile.x - startTile.x) + "," + (toTile.z - startTile.z) + "</color>");

                double hAngle = Math.Atan(vec.x / vec.z);
                if (vec.z < 0.0) hAngle += Math.PI;
                hAngle = StandardiseAngleRadians(hAngle);
                //if (hAngle < 0.0) hAngle += Math.PI + Math.PI;

                if (debug) Debug.Log("<color=blue>hAngle" + hAngle + "</color>");

                double dist = map.GetRealDistanceBetweenHexes(startTile, toTile);
                double heightDiff = toTile.y - (startTile.y + altitude);
                double heightDiffB = toTile.y + TileHeight(toTile) - (startTile.y + altitude + unitHeight); ;
                double vAngle = Math.Atan(heightDiff / dist);
                double vAngleB = Math.Atan(heightDiffB / dist);

                bool visible = true;
                for (int r = 1; r < ring; r++)
                {

                    if (debug) Debug.Log("<color=green>r" + r + "</color>");
                    for (int h = 0; h < rings[r - 1].hexes.Length; h++)
                    {
                        ViewHex iHex = rings[r - 1].hexes[h];

                        if (debug) Debug.Log("<color=green>" + (iHex.tile[0] - startTile.x) + "," + (iHex.tile[1] - startTile.z) + "</color>");
                        if (debug) Debug.Log("<color=green>sA " + iHex.startAngle + "   eA " + iHex.endAngle + "</color>");

                        if (
                            StandardiseAngleRadians( hAngle - iHex.startAngle )
                            <=
                            StandardiseAngleRadians( iHex.endAngle - iHex.startAngle )
                        )
                        //if (hAngle + 0.01 > iHex.startAngle && hAngle - 0.01 < iHex.endAngle)
                        {

                            //double verticalAngleFudge = 0.2 * map.baseHeightMultiplier;
                            double verticalAngleFudge = 0.0;

                            Tile iTile = map.GetTileFromArray(iHex.tile);

                            if (debug) Debug.Log("<color=red>vAngle " + vAngle + "</color>");
                            if (debug) Debug.Log("<color=red>iHex vAngle " + iHex.vAngle + "   fdg " + (verticalAngleFudge / (double)ring) + "</color>");

                            //bool thisDebug = (x - startTile.x == -2 && z - startTile.z == 2);
                            //if (thisDebug) Debug.Log("<color=red>vAngle " + vAngle + "</color>");
                            //if (thisDebug) Debug.Log("<color=red>iHex vAngle " + iHex.vAngle + "   fdg " + (verticalAngleFudge / (double)ring) + "</color>");

                            //if (vAngle < iHex.vAngle - 0.05)
                            if (vAngle < iHex.vAngle - (verticalAngleFudge / (double)ring) )
                            {
                                visible = false;
                                goto Break;
                            }
                           
                        }
                    }
                }
            Break:

                double theta = Math.Atan((sin60 * scale) / (new Vector2(vec.x, vec.z)).magnitude) + ringAngleFudge;

                /*double startAngle = hAngle - twelf / ring;
                startAngle = StandardiseAngleRadians(startAngle - angleFudge); // 0.01 fudge
                double endAngle = hAngle + twelf / ring;
                endAngle = StandardiseAngleRadians(endAngle + angleFudge); // 0.01 fudge*/

                double startAngle = StandardiseAngleRadians(hAngle - theta);
                double endAngle = StandardiseAngleRadians(hAngle + theta);

                ViewHex vh = new ViewHex(hex, startAngle, endAngle, vAngleB, visible);
                viewHexes.Add(vh);
                if (visible) visibleHexes.Add(vh);

                //debug = false; 
            }
        }
        ViewHex[] viewHexesArray = new ViewHex[viewHexes.Count];
        for (int i = 0; i < viewHexes.Count; i++)
        {
            viewHexesArray[i] = viewHexes[i];
        }
        ViewRing viewRing = new ViewRing(viewHexesArray);

        rings.Insert(ring - 1, viewRing);


    }

    public double StandardiseAngleRadians(double angle)
    {
        while (angle > 2.0 * Math.PI) {
            angle -= 2.0 * Math.PI;
        }
        while (angle < 0.0) {
            angle += 2.0 * Math.PI;
        }
        return angle;
    }

}