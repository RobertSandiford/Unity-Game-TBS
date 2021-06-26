using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public struct ArialMove
{
    public Tile tile;
    public int altitude;
    public double cost;

    public ArialMove(Tile Tile, int Altitude, double Cost)
    {
        tile = Tile;
        altitude = Altitude;
        cost = Cost;
    }
}


public class Tile
{
    public int dataI { get; }
    public int index { get; }
    public int x { get; }
    public int y { get; }
    public int z { get; }
    public Terrain terrain;
    public int objective { get; set; }
    public bool road = false;
    public Vector3 position { get; }
    public Hex hex { get; }
    public GameObject hexObject { get; }
    public Unit unit { get; set; }
    public Fort fort { get; set; }
    public List<GameObject> trees { get; set; }
    public List<GameObject> houses { get; set; }


    public Tile()
    {
        unit = null;
        fort = null;
        trees = new List<GameObject>();
        houses = new List<GameObject>();
    }

    public Tile(int DataI, int Index, int X, int Y, int Z, Terrain Terrain, int Objective, bool Road, Vector3 Position, Hex HexScript, GameObject HexObject)
    {
        dataI = DataI;
        index = Index;
        x = X;
        y = Y;
        z = Z;
        terrain = Terrain;
        objective = Objective;
        road = Road;
        position = Position;
        hex = HexScript;
        hexObject = HexObject;
        unit = null;
        fort = null;
        trees = new List<GameObject>();
        houses = new List<GameObject>();
    }

    public void SetRoad(bool status = true)
    {
        road = status;
        hex.road = status;
        //hex.SetMaterials();
        //hex.UpdateTexture();
    }

    public override string ToString()
    {
        return "Tile(" + x.ToString() + "," + z.ToString() + ")";
    }

    //public override string ToString() => $"({X}, {Y})";
}



public class Objective
{
    public Objective(int IdMain, int IdCore, int Owner)
    {
        idMain = IdMain;
        idCore = IdCore;
        owner = Owner;
    }

    public int idMain { get; set; }
    public int idCore { get; set; }
    public int owner { get; set; }
}

public struct Objective2
{
    public int id;
    //public int[] centerTile;
    public double[] centerPos;
    public int halfWidth;
    public int halfHeight;

    //public List<int[]> tiles;

    public Objective2(int Id, double[] CenterPos, int HalfWidth, int HalfHeight)
    {
        id = Id;
        centerPos = CenterPos;
        halfWidth = HalfWidth;
        halfHeight = HalfHeight;
    }

    /*public Objective2(int Id, List<int[]> Tiles)
    {
        id = Id;
        tiles = Tiles;
    }*/

}

public enum LineType
{
    NORMAL = 0,
    X = 1,
    Y = 2
}

public struct Line
{
    public Line(Vector3 StartPosition, Vector3 EndPosition, double M, double C, double XEquals, double YEquals, LineType LineType)
    {
        startPosition = StartPosition;
        endPosition = EndPosition;
        m = M;
        c = C;
        xEquals = XEquals;
        yEquals = YEquals;
        lineType = LineType;
    }

    public Line(Vector3 StartPosition, Vector3 EndPosition)
    {
        startPosition = StartPosition;
        endPosition = EndPosition;

        m = 0; c = 0; xEquals = 0; yEquals = 0; lineType = LineType.NORMAL;
        calculate();
    }
    public Line(Vector2 StartPosition, Vector2 EndPosition)
    {
        startPosition = new Vector3(StartPosition.x, 0, StartPosition.y);
        endPosition = new Vector3(EndPosition.x, 0, EndPosition.y);

        m = 0; c = 0; xEquals = 0; yEquals = 0; lineType = LineType.NORMAL;
        calculate();
    }

    private void calculate()
    {

        if (startPosition.x == endPosition.x)
        {
            xEquals = startPosition.x;
            lineType = LineType.X;
        }
        /*else if (startPosition.z == endPosition.z)
        {
            yEquals = startPosition.z;
            lineType = LineType.Y;
        }*/
        else
        {
            double gradient = (endPosition.z - startPosition.z) / (endPosition.x - startPosition.x);
            double intersection = startPosition.z - (startPosition.x * gradient);

            m = gradient;
            c = intersection;
            lineType = LineType.NORMAL;
        }

    }

    public bool intersects (Line line2/*, ref Vector3 intersection*/)
    {
        



        double intersectionX = 0;
        double intersectionY = 0;


        // deal with 2 verticles lines first
        if (lineType == LineType.X && line2.lineType == LineType.X)
        {
            if (xEquals == line2.xEquals)
                return true;
            else
                return false;
        }


        // set intersection points for these 3 alternatives, then process them together below
        else if ((lineType == LineType.NORMAL || lineType == LineType.Y) && (line2.lineType == LineType.NORMAL || line2.lineType == LineType.Y))
        {
            intersectionX = (line2.c - c) / (m - line2.m);
            intersectionY = m * intersectionX + c;
            
        }

        else if (lineType == LineType.X && line2.lineType != LineType.X) {
            intersectionX = xEquals;
            intersectionY = line2.m * intersectionX + line2.c;
        }

        else if (lineType != LineType.X && line2.lineType == LineType.X)
        {
            intersectionX = line2.xEquals;
            intersectionY = m * intersectionX + c;
        }

        else
        {
            Debug.Log("<color=red>error - LineType outside of expected options</color>");
        }

        // process the above 3 cases, checking for valid intersections
        if (
            (
                ((float)intersectionX >= startPosition.x && (float)intersectionX <= endPosition.x)
                ||
                ((float)intersectionX <= startPosition.x && (float)intersectionX >= endPosition.x)
            )
            &&
            (
                ((float)intersectionY >= startPosition.z && (float)intersectionY <= endPosition.z)
                ||
                ((float)intersectionY <= startPosition.z && (float)intersectionY >= endPosition.z)
            )
            &&
            (
                ((float)intersectionX >= line2.startPosition.x && (float)intersectionX <= line2.endPosition.x)
                ||
                ((float)intersectionX <= line2.startPosition.x && (float)intersectionX >= line2.endPosition.x)
            )
            &&
            (
                ((float)intersectionY >= line2.startPosition.z && (float)intersectionY <= line2.endPosition.z)
                ||
                ((float)intersectionY <= line2.startPosition.z && (float)intersectionY >= line2.endPosition.z)
            )
        )
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public Vector3 startPosition { get; set; }
    public Vector3 endPosition { get; set; }
    public double m { get; set; }
    public double c { get; set; }
    public double xEquals { get; set; }
    public double yEquals { get; set; }
    private LineType lineType { get; set; }

    //public override string ToString() => $"({X}, {Y})";
}


public class Terrain
{
    public int id;
    public string name;
    public Dictionary<TargetType, double> cover;
    // move cost

    public Terrain(int Id, string Name, Dictionary<TargetType, double> Cover)
    {
        id = Id;
        name = Name;
        cover = Cover;
    }

    public double GetCover(TargetType targetType)
    {
        if ( ! cover.ContainsKey(targetType)) return 0.0;
        return cover[targetType];
    }
}

public static class MapDefs {
    public static double hexWidth = 100.0;
    public static double hexHeight = 10.0;
}

public class Map : MonoBehaviour
{

    Global global;

    public int width;
    public int height;
    private int realWidth;
    //public List<Tile> tiles;
    public Dictionary<int, Tile> tiles;
    public Dictionary<int, int> tileMap;
    public List<Vector3> map;
    public List<GameObject> mapHexes;
    List<Objective> objectives;
    public Vector2 firstHex;
    public GameObject hexObject;
    public GameObject unitObject;
    public GameObject platoonPrefab;
    //public GameObject fortObject;
    public Material hexNormalMaterial;
    public Material hexObjectiveMaterial;
    public Material hexHighlightMaterial;
    public Material hexHighlight2Material;
    public Material hexShootableMaterial;
    public Material hexActiveMaterial;
    public Material hexShootingMaterial;
    public Material hexBlockingMaterial;
    Dictionary<string, List<Tile>> cachedVisibleTiles;
    private float sin60 = 0.86602540378443864676372317075294F;
    public GameObject moveTarget;
    List<GameObject> trees;

    private System.Random random;

    //public double hexWidth = 125.0;
    //public double hexHeight = 10.0;

    //public double hexWidth = 50.0;
    //public double hexHeight = 5.0;

    public GameObject tree1;
    public double forestHeight = 1.0;

    public List<GameObject> houses;
    public double urbanHeight = 0.2;

    public double scale;
    public double baseHeightMultiplier = 6.66;

    public int maxViewDist = 5;

    public Level level;

    public Dictionary<int, Terrain> terrains;

    bool initialised = false;

    System.TimeSpan profilingTime;

    public static class Terrains
    {
        public static Terrain Open = new Terrain(
            1,
            "Open",
            new Dictionary<TargetType, double>
            {
                { TargetType.Infantry, 0.0 },
                { TargetType.Light_Armor, 0.0 },
                { TargetType.Heavy_Armor, 0.0 },
            }
        );
        public static Terrain Forest = new Terrain(
            2,
            "Forest",
            new Dictionary<TargetType, double>
            {
                { TargetType.Infantry, 0.15 },
                { TargetType.Light_Armor, 0.1 },
                { TargetType.Heavy_Armor, 0.1 },
            }
        );
        public static Terrain Urban = new Terrain(
            3,
            "Urban",
            new Dictionary<TargetType, double>
            {
                { TargetType.Infantry, 0.25 },
                { TargetType.Light_Armor, 0.1 },
                { TargetType.Heavy_Armor, 0.1 },
            }
        );
    }

    // Start is called before the first frame update
    void Start()
    {
        //urbanHeight = 0.4;

        //ShowTime("Start");

        random = new System.Random();

        this.trees = new List<GameObject>();

        terrains = new Dictionary<int, Terrain>
        {
            { 1, Terrains.Open },
            { 2, Terrains.Forest },
            { 3, Terrains.Urban }
        };

        cachedVisibleTiles = new Dictionary<string, List<Tile>>();
        
        InitialiseLevel(LevelDefs.level8);
        
    }

    void InitialiseLevel(Level newLevel) {

        level = newLevel;

        //width = 20;
        //height = 23;

        width = level.width;
        height = level.height;

        realWidth = width + width - 1;
        
        var hexesTerrain = new List<int>();
        var hexes = new List<int>();
        var generatedTiles = new Dictionary<string, MapGenTile>();
        
        var hexesObjectives = level.objectives;

        objectives = level.objectivesDefs;
        
        //ShowTime("Pre Hex Gen");

        if (level.randomMap) 
        {
            firstHex = new Vector2(1, 0);
            MapGen mapGen = new MapGen(this);
            generatedTiles = mapGen.Make(width, height, level.roads);
            
            AssignHexes(hexes, hexesTerrain, hexesObjectives, firstHex, level.randomMap, generatedTiles);
            AssignObjectives(level.objectivesRandom);
            
            //ShowTime("Post Hex Gen");

            mapGen.MakeRoads(this, level.roads);

            //ShowTime("Post Roads Gen");

        
            //MapMeshGen mapMeshGen = new MapMeshGen(this, tiles, width, height);
            //mapMeshGen.Generate();

        }
        else
        {
            firstHex = new Vector2(1, 0);
            hexesTerrain = level.terrains;
            hexes = level.heights;

            AssignHexes(hexes, hexesTerrain, hexesObjectives, firstHex, level.randomMap, generatedTiles);
        }

        var treesObjs = FindObjectsOfType<Tree>();
        foreach (Tree tree in treesObjs) {
            tree.cameraObj = global.cameraObj;
        }

        //gameCamera = ((Global)FindObjectOfType<Global>()).gameCamera;

        //var hexesUnits = level.units;
        // Dictionary<int, MapUnit> units = level.unitsDefs;

        if (level.randomMap)
        {
            //CreateUnitsRandom(level.unitsDefsRandom, level.bases);
            //CreateForts(level.forts, level.fortsDefs);
            CreatePlatoons(level.platoons, level.bases);
        } 
        else
        {
            Debug.Log("No random map currently disabled");
            //CreateUnits(level.units, level.unitsDefs);
            //CreateForts(level.forts, level.fortsDefs);
        }

        if (level.virtualHexes != null) MakeVirtualHexes(level.virtualHexes);

        //ShowTime("Finished Making Map");

        Debug.Log("Number of Trees: " + trees.Count);

        Store.PopulateUnits();
        HideForts();

        Detection.CalculateAndShowHideVisibleEnemies(1);

        //ShowTime("Finished Final Setup");
    }

    // Update is called once per frame
    void Update()
    {
        if ( ! initialised )
        {
            Initialise();
        }
    }

    void Initialise()
    {
        //ShowTime("Map Initialisation");

        ShowTeamVision(1);
        initialised = true;
        
    }

    public void Awake()
    {

        global = (Global)FindObjectOfType<Global>();
        profilingTime = DateTime.Now.TimeOfDay;
        
        //forestHeight *= baseHeightMultiplier;
        //urbanHeight *= baseHeightMultiplier;
    }

    public void AssignUnitToTile(Unit unit, int x, int z)
    {
        Tile tile = GetTileFromXZ(x, z);
        AssignUnitToTile(unit, tile);
    }
    public void AssignUnitToTile(Unit unit, Tile tile)
    {
        tile.unit = unit;
        //tiles[tile.index] = tile;

    }

    public void UnassignUnitFromTile(int x, int z)
    {
        Tile tile = GetTileFromXZ(x, z);
        UnassignUnitFromTile(tile);
    }
    public void UnassignUnitFromTile(Tile tile)
    {
        tile.unit = null;
        //tiles[tile.index] = tile;
    }

    public Tile GetTileFromArray(int[] arr)
    {
        return GetTileFromXZ(arr[0], arr[1]);
    }
    public Tile GetTileFromXZ(int x, int z)
    {
        int index = GetIndexFromTile(x, z);

        //Debug.Log("X: " + x + " Z: " + z);
        return tiles[index];
    }

    public int[] GetTileFromIndex(int index)
    {
        int y = (index / height);
        int x = index - (y * width);
        return new int[] { x, y };
    }

    public int GetIndexFromTile(Vector3 tile)
    {
        return GetIndexFromTile((int)Math.Round(tile.x), (int)Math.Round(tile.z));
    }

    public int GetIndexFromTile(int[] tile)
    {
        return GetIndexFromTile(tile[0], tile[1]);
    }

    public int GetIndexFromTile(int x, int z)
    {
        int index = x + z * realWidth;
        //if (firstHex.x == 0) index++; // don't see why this is required

        return index;
    }


    public bool TileExists(int x, int z)
    {
        return TileExists(new int[] { x, z });
    }

    public bool TileExists(int[] tile)
    {
        if (tile[0] < 0 || tile[0] >= realWidth)
            return false;
        if (tile[1] < 0 || tile[1] >= height)
            return false;

        if ((tile[0] % 2) + (tile[1] % 2) != firstHex.x) // use modulo x to see if x is odd, and modulo y to see if y is odd. compare to location of first hex to create a correct checkerboard
            return false;

        return true;
    }

    public bool IndexExists(int index)
    {
        if (index < 0 || index >= realWidth * height)
            return false;

        return true;
    }

    public int IndexCount()
    {
        return realWidth * height;
    }

    public int GetMinTileAlt(Tile tile)
    {
        switch (tile.terrain.id)
        {
            case 1: // plain
                return 1;
            case 2: // forest
                return 3;
            case 3: // urban
                return 2;
            default:
                return 1;
        }
    }

    public bool CanLandOnTile(Tile tile)
    {
        switch (tile.terrain.id)
        {
            case 1: // plain
                return true;
            case 2: // forest
                return false;
            case 3: // urban
                return false;
            default:
                return false;
        }
    }

    public List<ArialMove> GetArialMoves(Tile fromTile, int moveDist, int currAlt, double gainAltCost, double loseAltPayment)
    {
        int currentHeight = fromTile.y + currAlt;

        gainAltCost /= baseHeightMultiplier;
        loseAltPayment /= baseHeightMultiplier;

        //Debug.Log("<color=red>Y: " + fromTile.y + " Alt: " + currAlt + " ch: " + currentHeight + "</color>");

        // Get all tiles that the aircraft may be able to move to, considering move gain from losing altitude
        int maxMoveDist = moveDist + (int)Math.Floor(Math.Round((double)moveDist * loseAltPayment, 2));
        List<Tile> tiles = GetTilesInMoveRange(fromTile, maxMoveDist);
        tiles.Insert(0, fromTile);

        List<ArialMove> ret = new List<ArialMove>();
        foreach (Tile tile in tiles)
        {
            //// don't add this if there is a unit there
            if (tile != fromTile && tile.unit != null) continue;

            int minHeight = tile.y + GetMinTileAlt(tile);
            //Debug.Log("<color=purple>XZ: " + tile.x + "," + tile.z + " y: " + tile.y + " MH: " + minHeight + "</color>");
            if (minHeight < currentHeight)
            {
                int altChange = currentHeight - minHeight;
                double altChangePayment = Math.Round(altChange * loseAltPayment, 2);
                double moveCost = (double)GetMoveRange(fromTile, tile) - altChangePayment;
                //Debug.Log("Dist: " + GetMoveRange(fromTile, tile).ToString() + " Drop: " + altChange.ToString() + " Payment: " + altChangePayment.ToString() + " Total cost: " + moveCost.ToString());
                if (moveCost <= (double)moveDist)
                {
                    //Debug.Log("<color=green>Dist: " + GetMoveRange(fromTile, tile).ToString() + " Drop: " + altChange.ToString() + " Payment: " + altChangePayment.ToString() + " Total cost: " + moveCost.ToString() + "</color>");
                    ret.Add(new ArialMove(tile, tile.y, moveCost));
                }
            }
            if (minHeight > currentHeight)
            {
                int altChange = minHeight - currentHeight;
                double altChangeCost = Math.Round(altChange * gainAltCost, 2);
                double moveCost = (double)GetMoveRange(fromTile, tile) + altChangeCost;
                //Debug.Log("Dist: " + GetMoveRange(fromTile, tile).ToString() + " Gain: " + altChange.ToString() + " Cost: " + altChangeCost.ToString() + " Total cost: " + moveCost.ToString());
                if (moveCost <= (double)moveDist)
                {
                    //Debug.Log("<color=green>Dist: " + GetMoveRange(fromTile, tile).ToString() + " Gain: " + altChange.ToString() + " Cost: " + altChangeCost.ToString() + " Total cost: " + moveCost.ToString() + "</color>");
                    ret.Add(new ArialMove(tile, tile.y, moveCost));
                }
            }
            if (minHeight == currentHeight)
            {
                double moveCost = (double)GetMoveRange(fromTile, tile);
                //Debug.Log("<color=yellow>cost: " + moveCost + " dist:" + moveDist + "</color>");
                if (moveCost <= (double)moveDist)
                {
                    ret.Add(new ArialMove(tile, tile.y, moveCost));
                }
            }
        }

        return ret;
    }

    public List<Tile> GetMovesReturnTiles(Tile tile, int moveDist = 1)
    {
        return GetMoves(tile, moveDist);
    }

    public List<Tile> GetMoves(Tile tile, int moveDist = 1)
    {
        List<Tile> moves = new List<Tile>();

        List<Tile> tiles = GetTilesInMoveRange(tile, moveDist);

        foreach (Tile thisTile in tiles)
        {
            if (thisTile.unit == null)
            {
                moves.Add(thisTile);
            }
        }

        return moves;
    }

    public List<Tile> GetMoves (int[] tile, int moveDist = 1)
    {
        return GetMoves(GetTileFromArray(tile), moveDist);
    }

    public List<Tile> GetTilesInMoveRange(Tile fromTile, int moveDist = 1, bool removeStartTile = false)
    {

        //int[] tile = new int[] { fromTile.x, fromTile.z };
        List<int[]> tiles = new List<int[]>();
        List<Tile> returnTile = new List<Tile>();

        for (int x = fromTile.x - moveDist*2; x <= fromTile.x + moveDist*2; x++)
        {
            for (int z = fromTile.z - moveDist; z <= fromTile.z + moveDist; z++)
            {
                if (TileExists(x, z))
                {
                    tiles.Add( new int[] { x, z } );
                }
            }
        }

        foreach (int[] thisTile in tiles)
        {

            Tile thisTileObj = GetTileFromXZ(thisTile[0], thisTile[1]);

            // skip start tile if flagged
            if (removeStartTile && thisTileObj == fromTile) continue;

            if (GetGameRange(fromTile, thisTileObj) <= moveDist)
                returnTile.Add(thisTileObj);

        }

        return returnTile;
    }

    /*public int ConvertIndexToDataIndex(int index)
    {
        return
    }*/

    public Vector3 GetHexLocation(Tile tile)
    {
        return GetHexLocation(tile.x, tile.z);
    }
    public Vector3 GetHexLocation(Vector3 tile)
    {
        return GetHexLocation(
            (int)Math.Round(tile.x),
            (int)Math.Round(tile.z));
    }
    public Vector3 GetHexLocation(int[] tile)
    {
        return GetHexLocation(tile[0], tile[1]);
    }
    public Vector3 GetHexLocation(int x, int z)
    {
        int index = z * realWidth + x;
        
        Tile tile = tiles[index];

        return new Vector3(x * sin60 * (float)scale, tile.y * YMultiplier() * (float)scale, z * 1.5F * (float)scale);
    }

    public Vector3 GetHexLocation(int x, int y, int z)
    {
        //Debug.Log(x * sin60 * (float)scale);
        //Debug.Log(y * YMultiplier() * (float)scale);
        //Debug.Log(z * 1.5F * (float)scale));
        return new Vector3(x * sin60 * (float)scale, y * YMultiplier() * (float)scale, z * 1.5F * (float)scale);
    }

    public int GetMoveRange(Tile from, Tile to)
    {
        return GetMoveDistance(from, to);
    }

    public int GetMoveDistance(Tile from, Tile to)
    {
        //// placeholder
        return GetGameRange(from, to);
    }

    public int GetGameRange(Tile from, Tile to)
    {
        double realRange = GetRealDistanceBetweenHexes(from, to);
        return (int) Math.Ceiling(realRange - 0.002);
    }

    public double GetRealDistanceBetweenHexes(int[] from, int[] to)
    {
        return GetRealDistanceBetweenHexes(GetTileFromArray(from), GetTileFromArray(to));
    }
    public double GetRealDistanceBetweenHexes(Tile from, Tile to)
    {
        return ( Math.Pow(Math.Pow(to.position.x - from.position.x, 2) + Math.Pow(to.position.z - from.position.z, 2), 0.5) / (sin60 *2 *scale) );
    }

    public float YMultiplier()
    {
        //return 0.4f;
        return 0.05f;
    }

    public Vector2 RotateY(Vector2 vector, double degrees)
    {
        double rads = degrees / 180 * Math.PI;

        //Debug.Log("<color=yellow>" + ((float)Math.Cos(rads)).ToString() + "</color>");
        //Debug.Log("<color=yellow>" + ((float)Math.Sin(rads)).ToString() + "</color>");

        return new Vector2(
            vector.x * (float)Math.Cos(rads) - vector.y * (float)Math.Sin(rads), 
            vector.x * (float)Math.Sin(rads) + vector.y * (float)Math.Cos(rads)
        );
    }

    private bool LineIntersectsHex(Line line, int[] tile)
    {
        return LineIntersectsHex(
            line,
            GetHexLocation(tile),
            0
        ); // tileIndex fudge
    }

    private bool LineIntersectsHex(Line line, Vector3 tilePosition, int tileIndex)
    {
        //Debug.Log("<color=yellow>" + line.startPosition.ToString() + "</color>");
        //Debug.Log("<color=yellow>" + line.endPosition.ToString() + "</color>");
        //Debug.Log("<color=yellow>m " + line.m.ToString() + "</color>");
        //Debug.Log("<color=yellow>c " + line.c.ToString() + "</color>");
        //line.startPosition
        //line.endPosition
        //line.m
        //line.c

        //float hexRadius = 1f;
        //float hexRadius = 0.99f; // allows excessive vertical vision
        float hexRadius = 1.001f;
        Vector2 hexPointVector = new Vector2(0, hexRadius);

        for (int i = 0; i < 6; i++)
        {
            Vector2 vec1 = RotateY(hexPointVector, (double)i * 60.0);
            Vector2 vec2 = RotateY(hexPointVector, (double)(i+1) * 60.0);
            //Debug.Log("<color=green>" + (i * 60.0).ToString() + "</color>");
            //Debug.Log("<color=green>" + ((i + 1) * 60.0).ToString() + "</color>");
            //Debug.Log("<color=green>" + vec1.ToString() + "</color>");
            //Debug.Log("<color=green>" + vec2.ToString() + "</color>");
            Vector2 point1 = new Vector2(tilePosition.x + vec1.x, tilePosition.z + vec1.y);
            Vector2 point2 = new Vector2(tilePosition.x + vec2.x, tilePosition.z + vec2.y);

            Line hexLine = new Line(point1, point2);

            if (hexLine.intersects(line)) return true;

            /******
            double sectionGradient = (point2.y - point1.y) / (point2.x - point1.x);
            double sectionIntersection = point1.y - (point1.x * sectionGradient);

            double intersectionX = (sectionIntersection - line.c) / (line.m - sectionGradient);
            double intersectionY = line.m * intersectionX + line.c;

            //Debug.Log("hexLine " + i.ToString() + " grad c");
            //Debug.Log("<color=purple>" + point1.ToString() + "</color>");
            //Debug.Log("<color=purple>" + point2.ToString() + "</color>");
            //Debug.Log(sectionGradient);
            //Debug.Log(sectionIntersection);
            //Debug.Log("intersection");
            //Debug.Log("<color=orange>" + intersectionX.ToString() + "</color>");
            //Debug.Log("<color=red>" + intersectionY.ToString() + "</color>");

            // check for intersection
            if (
                (
                    ((float)intersectionX >= line.startPosition.x && (float)intersectionX <= line.endPosition.x)
                    ||
                    ((float)intersectionX <= line.startPosition.x && (float)intersectionX >= line.endPosition.x)
                )
                &&
                (
                    ((float)intersectionY >= line.startPosition.z && (float)intersectionY <= line.endPosition.z)
                    ||
                    ((float)intersectionY <= line.startPosition.z && (float)intersectionY >= line.endPosition.z)
                )
                &&
                (
                    ((float)intersectionX >= point1.x && (float)intersectionX <= point2.x)
                    ||
                    ((float)intersectionX <= point1.x && (float)intersectionX >= point2.x)
                )
                &&
                (
                    ((float)intersectionY >= point1.y && (float)intersectionY <= point2.y)
                    ||
                    ((float)intersectionY <= point1.y && (float)intersectionY >= point2.y)
                )
            )
            {

                return true;

            }
            */////
        }

        return false;
    }

    /*public bool CanSee(Unit fromUnit, Unit toUnit, bool showBlockingHex = false)
    {
        return CanSee(fromUnit.tile, toUnit.tile, fromUnit.altitude, toUnit.altitude, showBlockingHex);
    }*/

    /*public bool CanSee(Tile fromTile, Tile toTile, int altitude = 0, int targetAltitude = 0, bool showBlockingHex = false)
    {
        //return CanSee(new int[] { fromTile.x, fromTile.z }, new int[] { toTile.x, toTile.z }, altitude, showBlockingHex);

        Vector3 startPosition = GetHexLocation(fromTile);
        Vector3 endPosition = GetHexLocation(toTile);

        //Debug.Log("start");
        //Debug.Log(startPosition);
        //Debug.Log("end");
        //Debug.Log(endPosition);

        Line line = new Line(startPosition, endPosition);

        //float shooterElevation = (float)altitude + 0.18f; // add 0.25 height units to unit's position representing viewer height
        float shooterElevation = (float)altitude + 0.3f;
        shooterElevation *= YMultiplier(); // multiplier by vertical scale

        //float targetElevation = (float)targetAltitude + 0.18f;
        float targetElevation = (float)targetAltitude + 0.3f;
        targetElevation *= YMultiplier();

        double lineDistance = Math.Pow(Math.Pow(endPosition.x - startPosition.x, 2) + Math.Pow(endPosition.z - startPosition.z, 2), 0.5);
        Line yLine = new Line(new Vector3(0f, 0f, startPosition.y + shooterElevation), new Vector3((float)lineDistance, 0f, endPosition.y + targetElevation));


        int startX = 0;
        int endX = 0;

        if (fromTile.x <= toTile.x)
        {
            startX = fromTile.x - 1;
            endX = toTile.x + 1;
        }
        else
        {
            startX = fromTile.x + 1;
            endX = toTile.x - 1;
        }

        int startZ = fromTile.z;
        int endZ = toTile.z;



        int loopNum = 0;
        bool blocked = false;

        // loop
        if (startX <= endX)
        {
            for (int x = startX; x <= endX; x++)
            {
                //if (showBlockingHex) Debug.Log("<color=green>"+x.ToString()+"</color>");
                if (startZ <= endZ)
                {
                    for (int z = startZ; z <= endZ; z++)
                    {
                        //if (showBlockingHex) Debug.Log("<color=green>"+ x.ToString() + " " + y.ToString() + "</color>");
                        if (CanSeeIteration(fromTile, toTile, startPosition, endPosition, line, yLine, x, z, showBlockingHex))
                        {
                            blocked = true;
                            goto End_Loop;
                        }
                        loopNum++;
                        if (loopNum > 9999) { Debug.Log("fail"); goto Fail; }
                    }
                }
                else
                {
                    for (int z = startZ; z >= endZ; z--)
                    {
                        //if (showBlockingHex) Debug.Log("<color=green>" + x.ToString() + " " + y.ToString() + "</color>");
                        if (CanSeeIteration(fromTile, toTile, startPosition, endPosition, line, yLine, x, z, showBlockingHex))
                        {
                            blocked = true;
                            goto End_Loop;
                        }
                        loopNum++;
                        if (loopNum > 9999) { Debug.Log("fail"); goto Fail; }
                    }
                }
            }
        }
        else
        {
            for (int x = startX; x >= endX; x--)
            {
                //if (showBlockingHex) Debug.Log("<color=orange>" + x.ToString() + "</color>");
                if (startZ <= endZ)
                {
                    for (int z = startZ; z <= endZ; z++)
                    {
                        //if (showBlockingHex) Debug.Log("<color=green>" + x.ToString() + " " + y.ToString() + "</color>");
                        if (CanSeeIteration(fromTile, toTile, startPosition, endPosition, line, yLine, x, z, showBlockingHex))
                        {
                            blocked = true;
                            goto End_Loop;
                        }
                        loopNum++;
                        if (loopNum > 9999) goto Fail;
                    }
                }
                else
                {
                    for (int z = startZ; z >= endZ; z--)
                    {
                        //if (showBlockingHex) Debug.Log("<color=green>" + x.ToString() + " " + y.ToString() + "</color>");
                        if (CanSeeIteration(fromTile, toTile, startPosition, endPosition, line, yLine, x, z, showBlockingHex))
                        {
                            blocked = true;
                            goto End_Loop;
                        }
                        loopNum++;
                        if (loopNum > 9999) goto Fail;
                    }
                }
            }
        }

        goto End_Loop;

    Fail:

        Debug.Log("<color=red>Fail</color>");

    End_Loop:

        return !blocked;
    }*/

    /*public bool CanSee(int[] fromTile, int[] toTile, int altitude = 0, int targetAltitude = 0, bool showBlockingHex = false)
    {
        return CanSee(GetTileFromArray(fromTile), GetTileFromArray(toTile), altitude, targetAltitude, showBlockingHex);

 
    }*/

    private bool CanSeeIteration(Tile fromTile, Tile toTile, Vector3 startPosition, Vector3 endPosition, Line line, Line yLine, int x, int z, bool showBlockingHex = false)
    {

        if (!TileExists(x, z)) return false;

        Tile tile = GetTileFromXZ(x, z);

        //if (showBlockingHex)
        //{
        //    tile.hexObject.GetComponent<MeshRenderer>().material = hexBlockingMaterial;
        //}

        // remove unused hexes
        //if (tile.y >= 0) return false; // should not be needed

        //if (CompareTiles(fromTile, tile)) return false;
        //if (CompareTiles(toTile, tile)) return false;

        if (fromTile == tile) return false;
        if (toTile == tile) return false;

        Vector3 tilePosition = tile.position;

        //Debug.Log("<color=blue>tile" + tile.ToString() + "</color>");
        //Debug.Log("<color=green>pos" + tilePosition.ToString() + "</color>");

        // see if the hex is on our path
        if (LineIntersectsHex(line, tilePosition, tile.index))
        {

            if (showBlockingHex)
            {
                //tile.hexObject.GetComponent<MeshRenderer>().material = hexBlockingMaterial;
                //tile.hex.isBlocking = true;
            }

            // highlight
            //Debug.Log("<color=red>intersects hex</color>");
            //Debug.Log("<color=red>" + tilePosition.ToString() + "</color>");

            double hexDistance = Math.Pow(Math.Pow(tilePosition.x - startPosition.x, 2) + Math.Pow(tilePosition.z - startPosition.z, 2), 0.5);
            double hexHeight = tilePosition.y;


            //// pretend the hex is forestHeight units higher if it's forested
            if (tile.terrain.id == 2) // Forest
            {
                //Debug.Log("forest height " + forestHeight.ToString());
                hexHeight += forestHeight * YMultiplier();
            }
            if (tile.terrain.id == 3)
            {
                //Debug.Log("y " + hexHeight);
                //Debug.Log("urb height " + urbanHeight.ToString());
                hexHeight += urbanHeight * YMultiplier();
            }

            double pathHeightAtHexDistance = yLine.startPosition.z + yLine.m * hexDistance;
            //if (showBlockingHex) Debug.Log("<color=green>" + pathHeightAtHexDistance.ToString() + "</color>");
            

            // return hex blocking if the height blocks the line
            if (pathHeightAtHexDistance <= (hexHeight - 0.01))
            {
                //Debug.Log("<color=orange>hex blocks LOS</color>");
                //// HEX BLOCKER
                if (showBlockingHex)
                {
                    //tile.hexObject.GetComponent<MeshRenderer>().material = hexBlockingMaterial;
                    tile.hex.isBlocking = true;
                    //tile.hexObject.GetComponent<MeshRenderer>().material = hexNormalMaterial;
                }
                return true;
            }
            else
            {
            }
            
        }

        return false;

    }


    private bool CompareTileArrayToVector(int[] tileArray, Vector3 tileVector)
    {
        return (tileArray[0] == (int)Math.Round(tileVector.x) && tileArray[1] == (int)Math.Round(tileVector.z));
    }
    public bool CompareTiles(int[] tileArray, Tile tile)
    {
        return (tileArray[0] == tile.x && tileArray[1] == tile.z);
    }

    private int[] GetTileArrayFromVector3(Vector3 tile)
    {
        return new int[] { (int)Math.Round(tile.x), (int)Math.Round(tile.z) };
    }

    public int hexDistance(int[] start, int[] end)
    {
        return 1;
    }

    public void ShowVision(Tile tile, int altitude = 0)
    {
        List<Tile> visibleTiles = GetAllVisibleTiles(tile, 0, altitude);
    }

    public void ShowVision(Tile tile, Unit unit, int altitude = 0)
    {
        List<Tile> visibleTiles = GetAllVisibleTiles(tile, unit.GetMaxWeaponRange(), altitude);
    }

    public List<Tile> GetUnitVision(Unit unit)
    {
        //Debug.Log("<color=blue>Getting Unit Vision</color>");
        return GetAllVisibleTiles(unit.tile, unit.altitude, unit.GetMaxWeaponRange());
    }
    public List<Tile> GetVisionFor(Tile tile, int altitude, int weaponRange = 0)
    {
        List<Tile> visibleTiles = GetAllVisibleTiles(tile, altitude, weaponRange);
        ResetUnitVision();
        MarkHexesUnitVisible(visibleTiles);
        return visibleTiles;
    }

    public List<Tile> GetAllVisibleTiles(int[] fromTile, int altitude = 0, int maxWeaponRange = 0)
    {
        return GetAllVisibleTiles(GetTileFromArray(fromTile), altitude, maxWeaponRange);
    }

    public List<Tile> GetAllVisibleTiles(Tile fromTile, int altitude = 0, int maxWeaponRange = 0)
    {
        
        /////// Return cached vision if available

        // key needs to involve altitude now
        string key = (fromTile.x.ToString()) + "-" + (fromTile.z.ToString()) + "-" + altitude.ToString();
        
        if (cachedVisibleTiles.ContainsKey(key))
        {
            //Debug.Log("<color=purple>Found cached visible tiles</color>");
            return cachedVisibleTiles[key];
        }


        /////// otherwise, do a new search

        List<Tile> visibleTiles = new List<Tile>();
        //visibleTiles.Add(fromTile); // include starting tile as visible, as ViewSearch does not return this.
        
        ViewSearch viewSearch = new ViewSearch(this, altitude);

        List<ViewHex> visibleHexes = viewSearch.FindVisibleTiles(fromTile);
        for (int i = 0; i < visibleHexes.Count; i++)
        {
            visibleTiles.Add( GetTileFromArray(visibleHexes[i].tile) );
        }

        /////// cache
        cachedVisibleTiles[key] = visibleTiles;

        return visibleTiles;

        ///////////////////////////////////




        

        // otherwise, find visibleTiles

        /*foreach (KeyValuePair<int, Tile> toTilePair in tiles)
        {
            Tile toTile = toTilePair.Value;

            int range = GetGameRange(fromTile, toTile);
            if (range > maxViewDist) { continue; }

            //Debug.Log(range);

            if (toTile.y >= 0)
            {

                bool canSee = CanSee(fromTile, toTile, altitude);

                if (canSee)
                {
                    int tileIndex = toTile.index;

                    //double tileDist = GetRealDistanceBetweenHexes(fromTile, toTile);
                    

                    //if (tileDist <= (maxWeaponRange + 0.002))
                    //{
                    //    ////toTile.hexObject.GetComponent<MeshRenderer>().material = hexHighlight2Material;
                    //}
                    //else
                    //{
                    //    ////toTile.hexObject.GetComponent<MeshRenderer>().material = hexHighlightMaterial;
                    //}

                    visibleTiles.Add(toTile);

                }
            }
        }

        cachedVisibleTiles.Add(key, visibleTiles);

        //UpdateHexStatusesForVisibleTiles(visibleTiles);

        return visibleTiles;*/
    }

    public void ShowTeamVision(int team)
    {
        //Debug.Log("<color=orange>Show team Vision</color>");
        ResetTeamVision();

        List<Unit> teamUnits = Store.GetAliveTeamUnits(team);
        foreach (Unit teamUnit in teamUnits)
        {
            List<Tile> vis = GetAllVisibleTiles(teamUnit.tile, teamUnit.altitude);
            MarkHexesTeamVisible(vis);
            // quick fix
            ShowFortsHere(vis);
        }
    }

    public void ResetTeamVision()
    {
        //HideForts();
        foreach (Tile tile in tiles.Values)
        {
            tile.hex.SetInTeamView(false);
        }
    }

    public void ResetUnitVision()
    {
        foreach (Tile tile in tiles.Values)
        {
            tile.hex.SetInUnitView(false);
            tile.hex.SetHasShootableTarget(false);
        }
    }

    public void ResetBlocking()
    {
        foreach (Tile tile in tiles.Values)
        {
            tile.hex.SetBlocking(false);
        }
    }

    public void ResetArtilleryTargets()
    {
        foreach (Tile tile in tiles.Values)
        {
            tile.hex.SetArtyTarget(false);
        }
    }

    public void HideForts()
    {
        foreach (Tile tile in tiles.Values)
        {
            if (tile.fort != null)
            {
                tile.fort.Hide();
            }
        }
    }

    public void ShowFortsHere(List<Tile> visibleTiles)
    {
        //HideForts();
        foreach (Tile tile in visibleTiles)
        {
            if (tile.fort != null)
            {
                tile.fort.Show();
            }
        }
    }

    public void MarkHexesTeamVisible(List<Tile> visibleTiles)
    {
        foreach (Tile tile in visibleTiles)
        {
            tile.hex.SetInTeamView(true);
        }
    }

    public void MarkHexesTeamVisible(List<Tile> visibleTiles, bool status = true)
    {
        foreach (Tile tile in visibleTiles)
        {
            tile.hex.SetInTeamView(status);
        }
    }

    public void MarkHexesUnitVisible(List<Tile> visibleTiles)
    {
        foreach (Tile tile in visibleTiles)
        {
            //Debug.Log("Setting this unit visible");
            tile.hex.SetInUnitView(true);
        }
    }

    public void UpdateHexStatusesForVisibleTiles(List<Tile> visibleTiles)
    {
        foreach (Tile tile in visibleTiles)
        {
            tile.hex.SetInTeamView(true);
        }
    }

    /*public void MarkHexNormal(Tile tile)
    {
        //tile.hexObject.GetComponent<MeshRenderer>().material = hexNormalMaterial;
        tile.hex.ResetFlags();
    }*/

    public void MarkHexShootable(Tile tile)
    {
        //tile.hexObject.GetComponent<MeshRenderer>().material = hexShootableMaterial;
        tile.hex.SetHasShootableTarget(true);
        
    }

    public void MarkHexAiNormal(Tile tile)
    {
        tile.hex.ResetAiFlags();
    }

    public void MarkHexAiActive(Tile tile)
    {
        tile.hex.SetAiActive(true);
    }

    public void MarkHexAiShooting(Tile tile)
    {
        tile.hex.SetAiShooting(true);
    }

    public void ResetAiHexModifiers()
    {
        foreach (Tile tile in tiles.Values)
        {
            tile.hex.SetAiActive(false);
            tile.hex.SetAiShooting(false);
        }
    }

    public void ResetAllHexMaterials()
    {
        foreach (Tile tile in tiles.Values)
        {
            if (tile.y >= 0)
            {
                //if (tile.objective > 0)
                //    tile.hexObject.GetComponent<MeshRenderer>().material = hexObjectiveMaterial;
                //else 
                //    tile.hexObject.GetComponent<MeshRenderer>().material = hexNormalMaterial;

                tile.hex.ResetFlags();
            }
        }
    }

    public void ResetVisibility()
    {
        ResetAllHexMaterials();
    }

    void AssignHexes(List<int> hexes, List<int> hexesTerrain, List<int> hexesObjectives, Vector2 firstHex, bool randomMap, Dictionary<string, MapGenTile> generatedHexes, List<Objective2> objectivesRandom = null)
    {
        

        tiles = new Dictionary<int, Tile>();
        tileMap = new Dictionary<int, int>();
        

        int i = 0;
        for (int zBase = height - 1; zBase >= 0; zBase--)
        {
            for (int x = 0; x < realWidth; x++)
            {
                int z = zBase;
                int index = z * realWidth + x;

                if ((x % 2) + (z % 2) == firstHex.x)
                { // use modulo x to see if x is odd, and modulo y to see if y is odd. compare to location of first hex to create a correct checkerboard


                    int y = 0;
                    

                    if (randomMap)
                    {
                        y = generatedHexes[x + "-" + z].y;
                    }
                    else
                    {
                        y = hexes[i];
                    }
                    


                    Vector3 position = GetHexLocation(x, y, z);

                    GameObject hex = Instantiate(hexObject);

                    Vector3 s = hex.transform.localScale;
                    s.y = (1.0f + (y * YMultiplier() / 2.0f)) * (float)scale;
                    hex.transform.localScale = s;

                    Hex hexScript = (Hex)(hex.GetComponent(typeof(Hex)));
                    hexScript.x = x;
                    hexScript.y = y;
                    hexScript.z = z;
                    hex.transform.position = position;

                    int objectiveId = 0;
                    if (randomMap)
                    {
                     
                    } else if (i < hexesObjectives.Count && hexesObjectives[i] >= 1)
                    {
                        hexScript.objective = true;
                        objectiveId = hexesObjectives[i];
                    }
                    

                    int terrainId = 0;
                    if (randomMap)
                    {
                        terrainId = generatedHexes[x + "-" + z].terrain;
                    }
                    else
                    {
                        terrainId = hexesTerrain[i];
                    }

           
                    bool road = false;
                    if (randomMap) road = generatedHexes[x + "-" + z].road;
                    hexScript.road = road;

                    //Debug.Log("New Tile");

                    Tile tile = new Tile(i, index, x, y, z, terrains[terrainId], objectiveId, road, position, hexScript, hex);
                    // save tile back to the hex object
                    hexScript.tile = tile;

                    AddTerrainToTile(tile);

                    tiles.Add(index, tile);

                    tileMap.Add(i, index);

                    i++;
                }
                else
                {
                    
                }
            }
        }
    }

    public Tile GetNearestTileToProportionalPos(double[] pos)
    {
        int x;
        int z = (int)Math.Round((double)(height-1) * pos[1]);

        if (z % 2 == 0) x = (int)(Math.Round(((double)(realWidth-1) * pos[0] + 1.0) / 2.0) * 2.0 - 1.0);
        else x = (int)(Math.Round((double)(realWidth-1) * pos[0] / 2.0) * 2.0);

        return GetTileFromXZ(x, z);
    }

    void AssignObjectives(List<Objective2> objectivesRandom)
    {
        foreach (Objective2 obj in objectivesRandom)
        {
            Tile objCenterTile = GetNearestTileToProportionalPos(obj.centerPos);

            for (int x = objCenterTile.x - obj.halfWidth * 2; x <= objCenterTile.x + obj.halfWidth * 2; x++)
            {
                for (int z = objCenterTile.z - obj.halfHeight; z <= objCenterTile.z + obj.halfHeight; z++)
                {
                    if (TileExists(new int[] { x, z } ))
                    {
                        Tile tile = GetTileFromXZ(x, z);
                        tile.objective = obj.id;
                        tile.hex.objective = true;
                    }
                }

            }

            /*foreach (int[] tileArray in obj.tiles)
            {
                Tile tile = GetTileFromArray(tileArray);
                tile.objective = obj.id;
                tile.hex.objective = true;
            }*/


        }
    }

    public double degsToRads(double angle)
    {
        return (angle / 180.0) * Math.PI;
    }
    
    public double[] GetRandomPositionInHex()
    {
        double height = 1.0f;
        double width = Math.Cos(degsToRads(30));
        double margin = 0.12;

        height -= margin;
        width -= margin;

        double x = 0;
        double y = 0;

        do
        {
            x = random.NextDouble();
            x -= 0.5;
            x *= width * 2;
            y = random.NextDouble();
            y -= 0.5;
            y *= height * 2;

            //x = 0.7;
            //y = 0.7;
        } while ( ! InHex(x, y, margin) );

        return new double[] { x * scale, y * scale };
    }

    ///////// buggy
    public bool InHex (double x, double y, double margin)
    {
        //double gradient = -0.5 / Math.Cos(degsToRads(30));

        double gradient = (-0.5 * (1-margin)) / (Math.Cos(degsToRads(30)) * (1 - margin));

        double a = (1 - margin);
        

        if ((Math.Abs(y) + Math.Abs(x) * gradient * -1) > a)
        {
            return false;
        }
        
        return true;
    }

    private bool positionCollidesWithExistingObject(Vector3 pos, List<GameObject> objects, double radius)
    {
        foreach (GameObject gObject in objects) {
            double dist = Math.Pow(Math.Pow(gObject.transform.position.x - pos.x, 2) + Math.Pow(gObject.transform.position.z - pos.z, 2), 0.5);
            if (dist <= radius * 2) {
                return true;
            }
        }
        return false;
    }

    private void AddTerrainToTile(Tile tile)
    {

        if (tile.terrain.id == 2) // forest
        {
            AddForestToTile(tile);
        }
        if (tile.terrain.id == 3) // urban
        {
            AddHousesToTile(tile);
        }
    }

    private void AddForestToTile(Tile tile)
    {
        for (int i = 0; i < 4; i++)
        {

            double treeRadius;
            //treeRadius = 0.1 * scale;
            treeRadius = 0.15 * scale;
            //treeRadius = 0.01 * scale;
            double[] pos = new double[] { };
            Vector3 treePos;

            int failureCount = 0;
            do
            {
                if (failureCount > 25)
                {
                    goto End;
                }

                pos = GetRandomPositionInHex();
                treePos = new Vector3(tile.position.x + (float)pos[0], tile.position.y, tile.position.z + (float)pos[1]);
                
                failureCount++;
            } while (positionCollidesWithExistingObject(treePos, tile.trees, treeRadius));

            GameObject tree = Instantiate(tree1);
            double ra = random.NextDouble();
            ra *= 0.06;
            //treePos.y += (0.05f + (float)ra) * (float)scale; // fudge for origin
            treePos.y += (-0.03f + (float)ra) * (float)scale; // fudge for origin

            tree.transform.position = treePos;
            tile.trees.Add(tree);
            trees.Add(tree);

            End:
            continue;

        }
    }

    private void AddHousesToTile(Tile tile)
    {
        for (int i = 0; i < 5; i++)
        {

            double goRadius = 0.25 * scale;
            double[] pos = new double[] { };
            Vector3 goPos;

            int failureCount = 0;
            do
            {
                if (failureCount > 25)
                {
                    goto End;
                }

                pos = GetRandomPositionInHex();
                goPos = new Vector3(tile.position.x + (float)pos[0], tile.position.y, tile.position.z + (float)pos[1]);


                failureCount++;
                
            } while (positionCollidesWithExistingObject(goPos, tile.houses, goRadius));
            
            GameObject go = Instantiate(houses[0]);
            go.transform.position = goPos;
            go.transform.rotation = Quaternion.Euler(0f, (float)(random.NextDouble() * 360.0), 0f);
            tile.houses.Add(go);

            //double ra = random.NextDouble();
            //ra *= 0.06;
            //goPos.y += 0.05f + (float)ra; // fudge for origin


            //Quaternion rot = go.transform.rotation;
            //rot.y += (float)(random.NextDouble() * 360.0);


            End:
            continue;

        }
    }

    /*void MakeTileForestTile(Tile tile)
    {

    }*/

    /*void MakeHexes()
    {
        mapHexes = new List<GameObject>();

        int w = width + width - 1;
        for (int i = 0, z = 0; z < height; z++)
        {
            for (int x = 0; x < w; x++)
            {
                //if (i < 6) {
                if ((x % 2) + (z % 2) == firstHex.x)
                {
                    GameObject hex = Instantiate(hexObject);
                    Hex hexScript = (Hex)(hex.GetComponent(typeof(Hex)));
                    hexScript.x = x;
                    hexScript.z = z;
                    hex.transform.position = GetHexLocation(x, z);

                    mapHexes.Add(hex);

                    i++;
                } else
                {
                    mapHexes.Add(null);
                }
                //}
            }
        }
    }*/

    bool GroupLocationValid(Tile groupLoc, List<Tile> groupLocs, int distReq = 3)
    {
        if (groupLoc.x == 0 || groupLoc.z == 0) return false;
        foreach(Tile tile in groupLocs)
        {
            if (GetMoveRange(groupLoc, tile) < distReq) return false;
        }
        return true;
    }

    /*
    private void CreateUnitsRandom(List<MapUnitGroup> unitGroups, Dictionary<int, double[][]> bases)
    {
        foreach (var kvp in bases)
        {
            int team = kvp.Key;
            double[][] teamBases = kvp.Value;

            double[] firstBase = teamBases[0];
            Tile baseLoc = GetNearestTileToProportionalPos(firstBase);

            if (baseLoc.z < 2) baseLoc = GetTileFromXZ(baseLoc.x, baseLoc.z +2);

            //int[] baseLoc = teamBase.Value;

            //if (baseLoc[0] < 0) baseLoc[0] = realWidth + baseLoc[0];
            //if (baseLoc[1] < 0) baseLoc[1] = height + baseLoc[1];

            //Debug.Log("baseloc " + baseLoc.x + ", " + baseLoc.y);

            List<Tile> baseTiles = GetTilesInMoveRange(baseLoc, 4);
            List<Tile> groupLocs = new List<Tile>();

            foreach (MapUnitGroup group in unitGroups)
            {
                if (team == group.team)
                {
                    Tile groupLoc = null;

                    if (group.pos != null)
                    {
                        //Debug.Log("x" + baseLoc.x + " --- " + group.pos[0]);
                        //Debug.Log("z" + baseLoc.z + " --- " + group.pos[1]);
                        groupLoc = GetTileFromXZ(baseLoc.x + group.pos[0], baseLoc.z + group.pos[1]);
                    } else {
                        groupLoc = baseTiles[Lib.random.Next(0, baseTiles.Count)];

                        int failed1 = 0;
                        int distReq = 3;
                        while (!GroupLocationValid(groupLoc, groupLocs, distReq) && failed1 < 100) {
                            if (failed1 == 50) { distReq = 2; }
                            if (failed1 == 80) { distReq = 1; }
                            groupLoc = baseTiles[Lib.random.Next(0, baseTiles.Count)];
                            failed1++;
                        }
                        if (failed1 >= 100) { Debug.Log("<color=red>Warning: could not place group</color>"); continue; }
                    }
                    groupLocs.Add(groupLoc);


                    Color groupColor = new Color(0.2f - (float)Lib.random.NextDouble() * 0.4f, 0.2f - (float)Lib.random.NextDouble() * 0.4f, 0.2f - (float)Lib.random.NextDouble() * 0.4f);

                    int n = 1;
                    int m = 0;
                    Unit lastUnit = null;
                    foreach (MapUnit mapUnit in group.units)
                    {

                        bool spawnAsCargo = false;
                        
                        //if (mapUnit.location != null) { // Place this unit on a specific tile, include virtual tiles
                        //    continue;
                        //}

                        if (group.team == 1) // only pre-load units for the player
                        { 
                            if (lastUnit != null)
                            {
                                UnitClass unitClass = mapUnit.squadDef.unitClass;
                                //Debug.Log("---unitClass");
                                //Debug.Log(unitClass);

                                //UnitDef unitDef = lastUnit.unitDef;
                                //CargoDef cargoDef = unitDef.cargoDef;
                                //List<UnitClass> transportable = cargoDef.transportable;

                                //Debug.Log("---transportable");
                                //Debug.Log(lastUnit.unitDef.cargoDef.transportable);

                                if (lastUnit.squadDefs[0].cargoDef.transportable != null && lastUnit.squadDefs[0].cargoDef.transportable.Contains(unitClass) )
                                {
                                    spawnAsCargo = true;
                                }
                            }
                        }

                        if (spawnAsCargo)
                        {

                            Unit newUnit = MakeUnit(mapUnit, lastUnit.tile, group.team, group.id, groupColor);
                            newUnit.PutIntoCargoSilent(lastUnit);
                            lastUnit = newUnit;

                        }
                        else
                        {

                            if (n == 2) m = 1;
                            if (n == 6) m = 2;
                            List<Tile> groupTiles = GetTilesInMoveRange(groupLoc, m);

                            Tile tile = groupTiles[Lib.random.Next(0, groupTiles.Count)];

                            int failed2 = 0;
                            while (tile.unit != null)
                            {
                                failed2++;
                                if (failed2 == 30 || failed2 == 60) { m++; groupTiles = GetTilesInMoveRange(groupLoc, m); }
                                if (failed2 > 100) { Debug.Log("<color=red>Warning: could not place unit " + mapUnit.squadDef.name + " (could not find empty space)</color>"); break; }
                                tile = groupTiles[Lib.random.Next(0, groupTiles.Count)];
                            }

                            if (failed2 <= 100)
                            {
                                Unit newUnit = MakeUnit(mapUnit, tile, group.team, group.id, groupColor);
                                lastUnit = newUnit;
                            } else
                            {
                                lastUnit = null;
                            }
                            n++;

                        }



                        
                    }
                }
            }
        }
    }
    */

    //Unit MakeUnit(MapUnit mapUnit, Tile tile, int team, int group, Color color) {

    //    GameObject thisUnitObject = Instantiate(unitObject);
    //    Unit unitScript = (Unit)thisUnitObject.GetComponent(typeof(Unit));
   //     unitScript.team = team;
    //    unitScript.group = group;
    //    unitScript.groupColor = color;
    //    unitScript.tile = tile;
    //    unitScript.unitDef = ObjectExtensions.Copy(mapUnit.squadDef);
    //    unitScript.map = this;
    //    tile.unit = unitScript;

    //    unitScript.Init0();

    //    return unitScript;

    //}
    //Unit MakeUnit(MapUnit mapUnit, Tile tile, int team) {

     //   GameObject thisUnitObject = Instantiate(unitObject);
     //   Unit unitScript = (Unit)thisUnitObject.GetComponent(typeof(Unit));
     //   unitScript.team = team;
    //    unitScript.tile = tile;
    //    unitScript.unitDef = ObjectExtensions.Copy(mapUnit.unitDef);
    //    unitScript.map = this;
    //    tile.unit = unitScript;

    //    unitScript.Init0();

    //    return unitScript;

    //}

    List<Unit> MakePlatoon(MapPlatoon mapPlatoon, Tile tile, int team) {

        //return MakeCombinedPlatoon(mapPlatoon, tile, team);
        return MakeSeparatePlatoons(mapPlatoon, tile, team);

    }
    
    List<Unit> MakeCombinedPlatoon(MapPlatoon mapPlatoon, Tile tile, int team) {

        Unit unit = MakeSinglePlatoon(mapPlatoon, tile, team);

        return new List<Unit> { unit };

    }
    
    List<Unit> MakeSeparatePlatoons(MapPlatoon mapPlatoon, Tile tile, int team) {

        // main/dismount platoon
        Unit mainPlatoon = MakeSinglePlatoon(mapPlatoon.MainPlatoon(), tile, team);
        
        // transporting platoon
        MapPlatoon tpd = mapPlatoon.TransportPlatoon();
        if (tpd.SquadDefs().Count > 0) {
            Unit transportPlatoon = MakeSinglePlatoon(tpd, tile, team);

            mainPlatoon.PutIntoCargoSilent(transportPlatoon);

            return new List<Unit> { mainPlatoon, transportPlatoon };

        }

        return new List<Unit> { mainPlatoon };

    }

    Unit MakeSinglePlatoon(MapPlatoon mapPlatoon, Tile tile, int team) {
        
        GameObject thisUnitObject = Instantiate(unitObject);
        Unit unitScript = (Unit)thisUnitObject.GetComponent(typeof(Unit));
        unitScript.team = team;
        unitScript.tile = tile;
        
        unitScript.platoonGroupDef = mapPlatoon.PlatoonGroupDef();
        unitScript.platoonDefs = mapPlatoon.PlatoonDefs();
        unitScript.squadDefs = mapPlatoon.SquadDefs();
        unitScript.coreSquadDefs = mapPlatoon.CoreSquadDefs();
        unitScript.attachedSquadDefs = unitScript.squadDefs.Except(unitScript.coreSquadDefs).ToList();
        unitScript.hasCore = unitScript.coreSquadDefs.Count > 0;
        unitScript.hasAttachments = unitScript.attachedSquadDefs.Count > 0;

        unitScript.map = this;
        tile.unit = unitScript;

        unitScript.Initialise();
        
        //Debug.Log("Platoon should have been created");

       

        return unitScript;

    }

    Squad MakeSquad(SquadDef squadDef, Unit platoon) {
        return new Squad(squadDef, platoon);
        //return
    }
    
    private void CreatePlatoons(List<MapPlatoon> mapPlatoons, Dictionary<int, double[][]> bases)
    {
        foreach (var kvp in bases)
        {
            int team = kvp.Key;
            double[][] teamBases = kvp.Value;

            // get the first base
            double[] firstBase = teamBases[0];

            // get centre tile
            Tile baseLoc = GetNearestTileToProportionalPos(firstBase);
            // shift away from edge
            if (baseLoc.z < 2) baseLoc = GetTileFromXZ(baseLoc.x, baseLoc.z +2);
            
            // get tile options
            List<Tile> baseTiles = GetTilesInMoveRange(baseLoc, 4);
            List<Tile> freeBaseTiles = new List<Tile>(baseTiles); // copy the list
            List<Tile> platoonLocs = new List<Tile>();
            
            foreach (MapPlatoon mapPlatoon in mapPlatoons)
            {
                if (team == mapPlatoon.team)
                {
                    Tile platoonLoc = null;

                    if (mapPlatoon.positionType == "pos")
                    {
                        //Debug.Log("pos");
                        platoonLoc = GetTileFromXZ(mapPlatoon.pos[0], mapPlatoon.pos[1]);
                    }
                    else if (mapPlatoon.positionType == "base")
                    {
                        //Debug.Log("base");
                        if (mapPlatoon.pos != null) // if mapPlatoos had a location specified, use that offset
                        {
                            //Debug.Log(baseLoc.x.ToString() + "+" + mapPlatoon.pos[0].ToString() + " , " + baseLoc.z.ToString() + "+" + mapPlatoon.pos[1].ToString());
                            platoonLoc = GetTileFromXZ(baseLoc.x + mapPlatoon.pos[0], baseLoc.z + mapPlatoon.pos[1]);
                        
                        } 
                        else // otherwise select a position at random
                        {

                            if (freeBaseTiles.Count == 0)
                            {
                                Debug.Log("<color=red>Warning: no more free tiles to place platoons on");
                                continue;
                            }

                            platoonLoc = freeBaseTiles[Lib.random.Next(0, freeBaseTiles.Count)]; // random tile from the list
                            freeBaseTiles.Remove(platoonLoc); // remove from free tiles
                            platoonLocs.Add(platoonLoc); // add to the used list, dunno why
                        }
                    } else
                    {
                        Debug.Log("<color=red>Warning: No valid MapPlatoon position type. positionType: " + mapPlatoon.positionType + "</color>");
                    }
                    

                    //Color groupColor = new Color(0.2f - (float)Lib.random.NextDouble() * 0.4f, 0.2f - (float)Lib.random.NextDouble() * 0.4f, 0.2f - (float)Lib.random.NextDouble() * 0.4f);



                    List<Unit> newUnits = MakePlatoon(mapPlatoon, platoonLoc, mapPlatoon.team);
                    
                }
            }
        }
    }

    //private void CreateUnits(List<int> hexesUnits, Dictionary<int, MapUnit> units)
    //{

    //    int i = 0;
    //    for (int zBase = height - 1; zBase >= 0; zBase--)
    //    {
    //        for (int x = 0; x < realWidth; x++)
    //        {
    //            int z = (height - 1) - zBase;

    //            if ((x % 2) + (z % 2) == firstHex.x)
    //            { // use modulo x to see if x is odd, and modulo y to see if y is odd. compare to location of first hex to create a correct checkerboard

    //                if (i < hexesUnits.Count && hexesUnits[i] != 0)
    //                {
    //                    int u = hexesUnits[i];
    //                    if (units.ContainsKey(u))
    //                    {
    //                        MapUnit unit = units[u];
    //                        Tile tile = tiles[tileMap[i]];

     //                       //MakeUnit(unit, tile); // Can replace the below

     //                       GameObject thisUnitObject = Instantiate(unitObject);
     //                       Unit unitScript = (Unit)thisUnitObject.GetComponent(typeof(Unit));
     //                       unitScript.team = unit.team;
     //                       unitScript.tile = tile;
     //                       //unitScript.AssignUnitDef(unit.unitDef);
     //                       unitScript.unitDef = ObjectExtensions.Copy(unit.unitDef);
     //                       unitScript.map = this;
     //                       tile.unit = unitScript;
     //
     //                       unitScript.Init0();
      //                  } else
     //                   {
     //                       Debug.Log("<color=red>Warning: key " + u + " not found in units defs for level</color>");
     //                   }
     //               }
     //
     //               i++;
    //            }
    //        }
    //    }
    //}

    private void CreateForts(List<int> hexesForts, Dictionary<int, MapFort> fortsDefs)
    {

        int i = 0;
        for (int zBase = height - 1; zBase >= 0; zBase--)
        {
            for (int x = 0; x < realWidth; x++)
            {
                int z = (height - 1) - zBase;

                if ((x % 2) + (z % 2) == firstHex.x)
                { // use modulo x to see if x is odd, and modulo y to see if y is odd. compare to location of first hex to create a correct checkerboard

                    if (i < hexesForts.Count && hexesForts[i] != 0)
                    {
                        int f = hexesForts[i];

                        if (fortsDefs.ContainsKey(f))
                        {
                            MapFort mapFort = fortsDefs[f];
                            Tile tile = tiles[tileMap[i]];

                            Fort fort =  Forts.MakeFort(mapFort.fortDef, tile, mapFort.stage);
                            fort.gameObject.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                        }
                    }

                    i++;
                }
            }
        }
    }

    void ShowTime(string place) {
        Debug.Log("<color=red>Time ("+place+"): " + (DateTime.Now.TimeOfDay - profilingTime) + "</color>");
    }

    void MakeVirtualHexes(List<VirtualHex> vHexes) {
        foreach (VirtualHex vHex in vHexes) {

            Vector3 position = GetHexLocation(vHex.x, vHex.y, vHex.z);

            GameObject hex = Instantiate(hexObject);

            Vector3 s = hex.transform.localScale;
            s.y = 1.0f + (vHex.y * YMultiplier() / 2.0f);
            hex.transform.localScale = s;

            Hex hexScript = (Hex)(hex.GetComponent(typeof(Hex)));
            hexScript.x = vHex.x;
            hexScript.y = vHex.y;
            hexScript.z = vHex.z;
            hex.transform.position = position;

            if (true) { // if hexes belong to player team / team 1
                hexScript.SetInTeamView(true);
            }

        }
    }

    
}



public class Road
{
    public List<double[]> checkpoints;
    public Road(List<double[]> Checkpoints)
    {
        checkpoints = Checkpoints;
    }
}

public struct VirtualHex {
    public int x;
    public int y;
    public int z;
    public VirtualHex(int X, int Y, int Z) {
        x = X;
        y = Y;
        z = Z;
    }
}



public enum AiMission
{
    Attack,
    Defend,
    Objective,
}

public struct CallInUnit
{
    UnitDef def;
    public CallInUnit(UnitDef Def)
    {
        def = Def;
    }
}

public struct CallIn
{
    List<CallInUnit> units;
    double penalty;

    public CallIn(List<CallInUnit> Units, double Penalty = 0.0)
    {
        units = Units;
        penalty = Penalty;
    }

    public CallIn(CallInUnit unit1, double Penalty = 0.0)
    {
        units = new List<CallInUnit>();
        units.Add(unit1);
        penalty = Penalty;
    }

    public CallIn(CallInUnit unit1, CallInUnit unit2, double Penalty = 0.0)
    {
        units = new List<CallInUnit>();
        units.Add(unit1);
        units.Add(unit2);
        penalty = Penalty;
    }

    public CallIn(int Size, CallInUnit unit1, double Penalty = 0.0)
    {
        units = new List<CallInUnit>();
        for (int i = 1; i <= Size; i++)
        {
            units.Add(unit1);
        }
        penalty = Penalty;
    }

    public CallIn(int Size, CallInUnit unit1, CallInUnit unit2, double Penalty = 0.0)
    {
        units = new List<CallInUnit>();
        for (int i = 1; i <= Size; i++)
        {
            units.Add(unit1);
            units.Add(unit2);
        }
        penalty = Penalty;
    }

}


/*
public static class CallIns
{
    public static CallIn RU_BMP_Platoon         = new CallIn(3, new CallInUnit(Definitions.BMP_2), new CallInUnit(Definitions.RU_Motor_Inf));

    public static CallIn RU_BMP_Recon_Platoon   = new CallIn(3, new CallInUnit(Definitions.BMP_2), new CallInUnit(Definitions.RU_Motor_Inf));
    public static CallIn RU_BMP_Recon_Squad     = new CallIn(new CallInUnit(Definitions.BMP_2), new CallInUnit(Definitions.RU_Motor_Inf), 0.1);

    public static CallIn RU_BMP3_Platoon        = new CallIn(3, new CallInUnit(Definitions.BMP_3), new CallInUnit(Definitions.RU_Motor_Inf));

    public static CallIn RU_BMP_Kornet_Platoon  = new CallIn(3, new CallInUnit(Definitions.BMP_2), new CallInUnit(Definitions.RU_Kornet_Squad));
    public static CallIn RU_BMP_Kornet_Squad    = new CallIn(new CallInUnit(Definitions.BMP_2), new CallInUnit(Definitions.RU_Kornet_Squad), 0.1);

    public static CallIn RU_BMP_Konkurs_Platoon = new CallIn(new CallInUnit(Definitions.BMP_2), new CallInUnit(Definitions.RU_Konkurs_Squad));
    public static CallIn RU_BMP_Konkurs_Squad   = new CallIn(new CallInUnit(Definitions.BMP_2), new CallInUnit(Definitions.RU_Konkurs_Squad), 0.1);

    public static CallIn RU_BMP_AGL_Platoon     = new CallIn(new CallInUnit(Definitions.BMP_2), new CallInUnit(Definitions.RU_AGS30_Squad));
    public static CallIn RU_BMP_AGL_Squad       = new CallIn(new CallInUnit(Definitions.BMP_2), new CallInUnit(Definitions.RU_AGS30_Squad), 0.1);

    public static CallIn RU_NonaS_Section       = new CallIn(3, new CallInUnit(Definitions.NonaS));



    public static CallIn RU_BTR_Platoon = new CallIn(3, new CallInUnit(Definitions.BTR_80), new CallInUnit(Definitions.RU_Motor_Inf));

    public static CallIn RU_BTR_Recon_Platoon = new CallIn(3, new CallInUnit(Definitions.BMP_2), new CallInUnit(Definitions.RU_Motor_Inf));
    public static CallIn RU_BTR_Recon_Squad = new CallIn(new CallInUnit(Definitions.BMP_2), new CallInUnit(Definitions.RU_Motor_Inf), 0.1);

    public static CallIn RU_BTR_Kornet_Platoon = new CallIn(3, new CallInUnit(Definitions.BMP_2), new CallInUnit(Definitions.RU_Kornet_Squad));
    public static CallIn RU_BTR_Kornet_Squad = new CallIn(new CallInUnit(Definitions.BMP_2), new CallInUnit(Definitions.RU_Kornet_Squad), 0.1);

    public static CallIn RU_BTR_Konkurs_Platoon = new CallIn(new CallInUnit(Definitions.BMP_2), new CallInUnit(Definitions.RU_Konkurs_Squad));
    public static CallIn RU_BTR_Konkurs_Squad = new CallIn(new CallInUnit(Definitions.BMP_2), new CallInUnit(Definitions.RU_Konkurs_Squad), 0.1);

    public static CallIn RU_BTR_Metis_Squad = new CallIn(new CallInUnit(Definitions.BMP_2), new CallInUnit(Definitions.RU_Metis_Squad), 0.05);

    public static CallIn RU_BTR_AGL_Platoon = new CallIn(new CallInUnit(Definitions.BMP_2), new CallInUnit(Definitions.RU_AGS30_Squad));
    public static CallIn RU_BTR_AGL_Squad = new CallIn(new CallInUnit(Definitions.BMP_2), new CallInUnit(Definitions.RU_AGS30_Squad), 0.1);

    public static CallIn RU_NonaSVK_Section = new CallIn(3, new CallInUnit(Definitions.NonaSvk));



    public static CallIn RU_T72_Platoon         = new CallIn(3, new CallInUnit(Definitions.T_72B3));
    public static CallIn RU_T72                 = new CallIn(new CallInUnit(Definitions.T_72B3), 0.1);

    public static CallIn RU_T80_Platoon         = new CallIn(3, new CallInUnit(Definitions.T_80U));
    public static CallIn RU_T80                 = new CallIn(new CallInUnit(Definitions.T_80U), 0.1);

    public static CallIn RU_T90_Platoon         = new CallIn(3, new CallInUnit(Definitions.T_90A));
    public static CallIn RU_T90                 = new CallIn(new CallInUnit(Definitions.T_90A), 0.1);
}
*/

/* Unit Group Costs

int mtlb_cost = 3;
int bmp2_cost = 8;
MT-LB = 3p
BMP = 8p
BRM-1K = 6p
BTR = 4p

7Man Squad = 6p

7Man 2Kornet Squad = 10p
7Man 2Konkurs Squad = 9p
7Man 2Metis Squad = 8p
7Man AGL Squad = 7p

2Man Scout = 2p

Nona-S BMD 120mm = 15
Nona-SVK BTR 120mm = 15

5Man 82mm Mortar = 9p
5Man 120mm Mortar = 7p


===BMP Bat

BMP Platoon         42p

BMP AT Kornet Platoon   54p
BMP AT Kornet Squad     18+2p
BMP AT Platoon          51p
BMP AT Squad            17+2p

BMP AGL Platoon         45p
BMP AGL Squad           15+2p

BMP Recon Platoon       36p
BMP Recon Squad         14+2p

Nona-S Mortar Section(2-3) 45p (x3)
BMP 120mm Mortar Section(2-3)  51p (x3)
BMP 82mm Mortar Section(2-3)   45p (x3)


===MT-LB Bat

BTR Platoon         27p

BTR Metis Squad         11+1p

BTR AT Kornet Platoon   39p
BTR AT Kornet Squad     13+2p
BTR AT Platoon          36p
BTR AT Squad            12+2p

BTR AGL Platoon         30p
BTR AGL Squad           10+2p

BTR Recon Platoon       26p
BTR Recon Squad         9+2p

Nona-S Mortar Section(2-3)   45p
BTR 120mm Mortar Section(2-3)  32p
BTR 82mm Mortar Section(2-3)   32p


===BTR Bat

BTR Platoon         30p

BTR Metis Squad         12+1p

BTR AT Kornet Platoon   42p
BTR AT Kornet Squad     14+2p
BTR AT Platoon          39p
BTR AT Squad            13+2p

BTR AGL Platoon         33p
BTR AGL Squad           11+2p

BTR Recon Platoon       38p
BTR Recon Squad         10+2p

Nona-SVK Mortar Section(2-3)   45p
BTR 120mm Mortar Section(2-3)  35p
BTR 82mm Mortar Section(2-3)   35p


===Tank Bat

T-72 Platoon			60p
T-72				23p
T-80 Platoon			65p
T-80				25p
T-90 Platoon			70p
T-90				29p
*/


