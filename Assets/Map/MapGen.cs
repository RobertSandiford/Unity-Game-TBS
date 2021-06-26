using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenTile
{
    public int x;
    public int y;
    public int z;
    public int terrain;
    public bool road;

    public MapGenTile(int X, int Y, int Z, int Terrain)
    {
        x = X;
        y = Y;
        z = Z;
        terrain = Terrain;
        road = false;
    }
}
public class MapGen
{
    System.Random random;

    private int width;
    private int height;
    private Dictionary<string, MapGenTile> tiles;
    private bool done;
    private int lowestHeight = 999;

    private double baseHeightMultiplier;

    int tilesCreated;
    Dictionary<int, int> terrainsCreated;

    Dictionary<int, double> terrainProbs;

    private float sin60 = 0.86602540378443864676372317075294F;

    public MapGen(Map map)
    {
        //int seed = 2644;
        //int seed = 1734734;
        int seed = 463211;
        random = new System.Random(seed);

        baseHeightMultiplier = map.baseHeightMultiplier;

        tilesCreated = 0;
        terrainsCreated = new Dictionary<int, int> {
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
        };

        terrainProbs = new Dictionary<int, double> {
            { 1, 0.72 },
            { 2, 0.25 },
            { 3, 0.03 },
        };
    }

    public Dictionary<string, MapGenTile> Make(int Width, int Height, List<Road> roads)
    {
        width = Width;
        height = Height;
        done = false;


        int iteration = 0;
        tiles = new Dictionary<string, MapGenTile>();
        MapGenTile firstTile = new MapGenTile(1, 0, 0, RandomTerrain(terrainProbs));
        tiles.Add("1-0", firstTile);

        while (!done)
        {
            iteration++;

            int x = 0; int z = 0;
            for (int i = 1; i <= iteration + 1; i++)
            {
                x = (iteration * 2) + 1 - (i - 1);
                z = i - 1;

                MakeTile(x, z, terrainProbs);
            }
            while (x >= 2)
            {
                x -= 2;

                MakeTile(x, z, terrainProbs);
            }
        }

        MakeHills(ref tiles);
        //MakeRoads(ref tiles, roads);
        SmoothingPass(ref tiles);
        SmoothingPass(ref tiles);
        SmoothingPass(ref tiles);
        SmoothingPass(ref tiles);
        SmoothingPass(ref tiles);

        foreach (KeyValuePair<string, MapGenTile> kvp in tiles)
        {
            if (tiles[kvp.Key].y < lowestHeight) lowestHeight = tiles[kvp.Key].y;
        }
        //Debug.Log("Lowest height: " + lowestHeight);
        foreach (KeyValuePair<string, MapGenTile> kvp in tiles)
        {
            tiles[kvp.Key].y -= lowestHeight;
        }
        return tiles;
    }

    public List<MapGenTile> NearbyTiles(int x, int z)
    {
        List<MapGenTile> retTiles = new List<MapGenTile>();
        string key = "";
        key = (x - 2) + "-" + (z); if (tiles.ContainsKey(key)) retTiles.Add(tiles[key]);
        key = (x - 1) + "-" + (z - 1); if (tiles.ContainsKey(key)) retTiles.Add(tiles[key]);
        key = (x + 1) + "-" + (z - 1); if (tiles.ContainsKey(key)) retTiles.Add(tiles[key]);
        key = (x + 2) + "-" + (z); if (tiles.ContainsKey(key)) retTiles.Add(tiles[key]);
        return retTiles;
    }

    public List<MapGenTile> NearbyTilesFull(MapGenTile tile)
    {
        List<MapGenTile> retTiles = new List<MapGenTile>();
        string key = "";
        key = (tile.x - 2) + "-" + (tile.z); if (tiles.ContainsKey(key)) retTiles.Add(tiles[key]);
        key = (tile.x - 1) + "-" + (tile.z - 1); if (tiles.ContainsKey(key)) retTiles.Add(tiles[key]);
        key = (tile.x + 1) + "-" + (tile.z - 1); if (tiles.ContainsKey(key)) retTiles.Add(tiles[key]);
        key = (tile.x + 2) + "-" + (tile.z); if (tiles.ContainsKey(key)) retTiles.Add(tiles[key]);
        key = (tile.x + 1) + "-" + (tile.z + 1); if (tiles.ContainsKey(key)) retTiles.Add(tiles[key]);
        key = (tile.x - 1) + "-" + (tile.z + 1); if (tiles.ContainsKey(key)) retTiles.Add(tiles[key]);
        return retTiles;
    }

    public void MakeTile(int x, int z, Dictionary<int, double> terrainProbs)
    {

        if (x < 0) return;
        if (z < 0) return;
        if (x > (width * 2) - 2) return;
        if (z > height - 1) return;

        int low = 999;
        int high = -999;


        List<MapGenTile> nearbyTiles = NearbyTiles(x, z);

        var terrains = new Dictionary<int, int>
        {
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
        };
        foreach (MapGenTile tile in nearbyTiles)
        {
            if (tile.y > high) high = tile.y;
            if (tile.y < low) low = tile.y;
            terrains[tile.terrain] = terrains[tile.terrain] + 1;
        }

        int terrainSum = 0;
        foreach (KeyValuePair<int, int> kvp in terrains)
        {
            terrainSum += kvp.Value;
        }
        /*foreach (KeyValuePair<int, int> kvp in terrains)
        {
            terrains[kvp.Key] = ((double)kvp.Value / (double)terrainSum);
        }*/

        //double neighbourMulti = 1.5;
        //double neighbourMulti = 3.5;
        double neighbourMulti = 4.5; // increase grouping to test scale change
        double baseMulti = 1.0;

        Dictionary<int, double> adjustedTerrainProbs = AdjustedTerrainProbablities();
        //Dictionary<int, double> adjustedTerrainProbs = terrainProbs;

        Dictionary<int, double> newTerrainProbs = new Dictionary<int, double>();

        foreach (KeyValuePair<int, double> kvp in adjustedTerrainProbs)
        {
            newTerrainProbs.Add(kvp.Key, ((((double)terrains[kvp.Key] / (double)terrainSum) * neighbourMulti) + (kvp.Value * baseMulti)) / (neighbourMulti + baseMulti));
            // reduce chances when a lot of this terrain exist already
            // tilesCreated
            // terrainsCreated
        }

        //Debug.Log("---");
        //Debug.Log(newTerrainProbs[1]);
        //Debug.Log(newTerrainProbs[2]);
        //Debug.Log(newTerrainProbs[3]);

        int minHeight = high - 1;
        int maxHeight = low + 1;
        int y = RandomHeight(low, high, minHeight, maxHeight);
        int terrain = RandomTerrain(newTerrainProbs);
        MapGenTile t = new MapGenTile(x, y, z, terrain);

        tiles.Add(x + "-" + z, t);

        tilesCreated++;
        terrainsCreated[terrain]++;

        //if (y < lowestHeight) lowestHeight = y;

        //Debug.Log(x + "-" + z);

        if (z == (height - 1) && (x == ((width * 2) - 2) || x == ((width * 2) - 3))) done = true;

    }

    public int RandomHeight(int low, int high, int min, int max)
    {
        int rh;

        int h = random.Next(min, max + 1);
        if (h != low && h != high) h = random.Next(min, max + 1);
        //if (h != low && h != high) h = random.Next(min, max + 1);

        //rh = h;

       // if (h != low && h != high) h = random.Next(min, max + 1);
        //if (h != low && h != high) h = random.Next(min, max + 1);
        //if (h != low && h != high) h = random.Next(min, max + 1);
        //if (h != low && h != high) h = random.Next(min, max + 1);
        //if (h != low && h != high) h = random.Next(min, max + 1);
        //if (h != low && h != high) h = random.Next(min, max + 1);
        //if (h != low && h != high) h = random.Next(min, max + 1);
        //if (h != low && h != high) h = random.Next(min, max + 1);

        rh = h;

        /*int t;
        t = random.Next(min, max + 1);
        t = random.Next(min, max + 1);
        t = random.Next(min, max + 1);
        t = random.Next(min, max + 1);
        t = random.Next(min, max + 1);
        t = random.Next(min, max + 1);
        t = random.Next(min, max + 1);
        t = random.Next(min, max + 1);*/

        return rh;

        //return random.Next(min, max + 1);
    }

    public Dictionary<int, double> AdjustedTerrainProbablities() {

        if (tilesCreated == 0) return terrainProbs;

        var adjusted = new Dictionary<int, double> {
            { 1, 1.0 },
            { 2, 1.0 },
            { 3, 1.0 },
        };

        double totalRelativeProbs = 0.0;

        foreach (var kvp in terrainProbs) {
            //Debug.Log(kvp.Key);
            //Debug.Log("terrs" + terrainsCreated[kvp.Key]);
            //Debug.Log("tiles" + tilesCreated);
            double r = 3.0; // default incase the next thing is 0
            if ( ((double)terrainsCreated[kvp.Key] / (double)tilesCreated) != 0 )
                r = ((double)terrainsCreated[kvp.Key] / (double)tilesCreated) / terrainProbs[kvp.Key];
            //Debug.Log("r" + r);
            adjusted[kvp.Key] = terrainProbs[kvp.Key] * (1/r);
            totalRelativeProbs += terrainProbs[kvp.Key] * (1/r);
            //Debug.Log("adj" + adjusted[kvp.Key]);
        }

        foreach (var kvp in terrainProbs) {
            adjusted[kvp.Key] = adjusted[kvp.Key] / totalRelativeProbs;
        }


        return adjusted;

    }

    public int RandomTerrain(Dictionary<int, double> probs)
    {
        double ran = random.NextDouble();
        double cum = 0.0;
        int lastKey = 0;
        foreach (KeyValuePair<int, double> kvp in probs)
        {
            cum += kvp.Value;
            if (ran < cum) return kvp.Key;
            lastKey = kvp.Key;
        }
        return 1;
    }


    void MakeHills(ref Dictionary<string, MapGenTile> tiles)
    {
        double minHeight = 5.0;
        double maxHeight = 6.0 * baseHeightMultiplier * 2.0;
        
        double minWideMulti = 0.4;
        double maxWideMulti = 2.3;

        int minHills = (int)Math.Round(width * height * 0.0025);
        int maxHills = (int)Math.Round(width * height * 0.025);

        int numHills = random.Next(minHills, maxHills + 1);


        //Debug.Log("Min Hills: " + minHills);
        //Debug.Log("Max Hills: " + maxHills);
        //Debug.Log("----");
        //Debug.Log("Num Hills: " + numHills);

        for (int i = 1; i <= numHills; i++)
        {
            double margin = maxHeight * maxWideMulti; // this is max width
            
            double tall = minHeight + random.NextDouble() * (maxHeight - minHeight);
            double wide = tall * (minWideMulti + random.NextDouble() * (maxWideMulti - minWideMulti) );
            wide *= 1.66; // scale up test
            
            double x = (wide* -2) + random.NextDouble() * (width + wide + wide)*2;
            double z = (wide * -1) + random.NextDouble() * (height + wide + wide);
            
            //Debug.Log("<color=blue>Hill: "+ x + "," + z + "</color>");

            //Debug.Log("<color=blue>T/W: " + tall + "," + wide + "</color>");

            foreach (var kvp in tiles)
            {
                string key = kvp.Key;
                MapGenTile tile = kvp.Value;
                Vector2 vector = new Vector2((float)x, (float)z) - new Vector2((float)tile.x, (float)tile.z);
                vector.x *= sin60;
                vector.y *= 1.5f;

                double dist = vector.magnitude;

                // double dist = (double)Vector2.Distance( new Vector2((float)tile.x, (float)tile.z), new Vector2((float)x, (float)z) );

                dist /= wide;
                if (dist < 1.0)
                {
                    //double extraHeight = Math.Round((1 - dist) * tall);
                    double extraHeight = 1.0 / (
                        1 + Math.Exp(
                            -1 * (0.5 - dist) * 10.0
                        )
                    ) * tall;
                    tiles[key].y += (int)extraHeight;
                }
               
            }
            // random Pos

            // random heiht

            // random size (ran * height)

            // imcrement tiles
        }
    }

    public void MakeRoads(Map map, List<Road> roads)
    {
        foreach (Road road in roads)
        {
            for (int i = 1; i < road.checkpoints.Count; i++)
            {
                double[] checkpoint1 = road.checkpoints[i - 1];
                Tile checkPoint1Tile = map.GetNearestTileToProportionalPos(checkpoint1);
                double[] checkpoint2 = road.checkpoints[i];
                Tile checkPoint2Tile = map.GetNearestTileToProportionalPos(checkpoint2);

                checkPoint1Tile.SetRoad(true);
                checkPoint2Tile.SetRoad(true);

                AStarNode route = Pathfinder.FindBestPath(map, checkPoint1Tile, checkPoint2Tile);

                foreach (Tile tile in route.path)
                {
                    //Debug.Log(tile.x + "-" + tile.z);
                    tile.SetRoad();
                }
            }

            //double[] checkpoint1 = road.checkpoints[0];
            //Tile checkPoint1Tile = map.GetNearestTileToProportionalPos(checkpoint1);
            //double[] checkpoint2 = road.checkpoints[road.checkpoints.Count -1];
            //Tile checkPoint2Tile = map.GetNearestTileToProportionalPos(checkpoint2);

            //checkPoint1Tile.SetRoad(true);
            //checkPoint2Tile.SetRoad(true);

            //AStarNode route = Pathfinder.FindBestPath(map, checkPoint1Tile, checkPoint2Tile);

            //foreach (Tile tile in route.path)
            //{
            //    tile.SetRoad();
            //}


        }
    }

    /*void MakeRoads(ref Dictionary<string, MapGenTile> tiles, List<Road> roads)
    {
        foreach (Road road in roads)
        {
            int i = 1;
            //for (int i = 1; i < road.checkpoints.Count; i++)
            //{
            double[] checkpoint1 = road.checkpoints[i-1];
            MapGenTile checkPoint1Tile = GetNearestTileToProportionalPos(tiles, checkpoint1);
            double[] checkpoint2 = road.checkpoints[i];
            MapGenTile checkPoint2Tile = GetNearestTileToProportionalPos(tiles, checkpoint2);

            checkPoint1Tile.road = true;
            checkPoint2Tile.road = true;

            AStarNode route = Pathfinder.FindBestPath(tiles, checkPoint1Tile, checkPoint2Tile);
            //}
        }
    }*/

    void SmoothingPass(ref Dictionary<string, MapGenTile> tiles, double prob1 = 0.35, double prob2 = 0.65)
    {
        foreach (var kvp in tiles)
        {
            MapGenTile mapGenTile = kvp.Value;
            double avgHeightDiff = averageHeightDiffSurroundingTiles(mapGenTile);

            if (avgHeightDiff >= 2.0)
            {
                mapGenTile.y ++; //Debug.Log("Inc " + mapGenTile.x + "," + mapGenTile.z);
            } else if (avgHeightDiff >= 1.0)
            {
                if (random.NextDouble() < prob2)
                {
                    mapGenTile.y ++; //Debug.Log("Inc " + mapGenTile.x + "," + mapGenTile.z);
                }
            } else if (avgHeightDiff >= 0.65)
            {
                if (random.NextDouble() < prob1)
                {
                    mapGenTile.y++; //Debug.Log("Inc " + mapGenTile.x + "," + mapGenTile.z);
                }
            }

            if (avgHeightDiff <= -2.0)
            {
                mapGenTile.y --; //Debug.Log("Dec " + mapGenTile.x + "," + mapGenTile.z);
            }
            else if (avgHeightDiff <= -1.0)
            {
                if (random.NextDouble() < prob2)
                {
                    mapGenTile.y --; //Debug.Log("Dec " + mapGenTile.x + "," + mapGenTile.z);
                }
            }
            else if (avgHeightDiff < -0.65)
            {
                if (random.NextDouble() < prob1)
                {
                    mapGenTile.y--; //Debug.Log("Dec " + mapGenTile.x + "," + mapGenTile.z);
                }
            }

        }
    }

    double averageHeightDiffSurroundingTiles(MapGenTile tile)
    {
        List<MapGenTile> nearbyTiles = NearbyTilesFull(tile);
        int heightDiff = 0;
        foreach (MapGenTile nearTile in nearbyTiles)
        {
            heightDiff += nearTile.y - tile.y;
        }
        return heightDiff / (double)nearbyTiles.Count;
    }

    MapGenTile GetNearestTileToProportionalPos(Dictionary<string, MapGenTile> tiles, double[] pos)
    {
        int realWidth = width + width - 1;

        int x;
        int z = (int)Math.Round((double)(height - 1) * pos[1]);

        if (z % 2 == 0) x = (int)(Math.Round(((double)(realWidth -1) * pos[0] + 1.0) / 2.0) * 2.0 - 1.0);
        else x = (int)(Math.Round((double)(width+width-1 -1) * pos[0] / 2.0) * 2.0);

        return tiles[x+"-"+z];
    }

}