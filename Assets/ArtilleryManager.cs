using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArtilleryManager : MonoBehaviour
{
    List<ArtilleryMission> artilleryMissions;

    Global global;
    bool initialised = false;

    // Start is called before the first frame update
    void Start()
    {
        artilleryMissions = new List<ArtilleryMission>();    
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
        global = (Global)FindObjectOfType<Global>();
        initialised = true;
    }

    public void AddMission(ArtilleryMission mission)
    {
        artilleryMissions.Add(mission);
    }

    public void ProcessMissions(int turn)
    {
        List<ArtilleryMission> killList = new List<ArtilleryMission>();

        foreach (ArtilleryMission mission in artilleryMissions)
        {
            if (mission.Process(turn)) killList.Add(mission);
        }

        foreach (ArtilleryMission killMission in killList)
        {
            artilleryMissions.Remove(killMission);
        }
    }
}

public class ArtilleryMission
{
    public Unit unit;
    public UnitWeapon uWeapon;
    public ArtilleryTarget target;
    public int startTurn;
    public int aimTime;
    public int flightTime;
    public int shots;
    public int shotsFired = 0;
    public int indivShellsLanded = 0;
    public bool cancelled = false;
    Global global;

    public ArtilleryMission(Unit Unit, UnitWeapon UWeapon, ArtilleryTarget Target, int StartTurn, int AimTime, int FlightTime, int Shots)
    {
        unit = Unit;
        uWeapon = UWeapon;
        target = Target;
        startTurn = StartTurn;
        aimTime = AimTime;
        flightTime = FlightTime;
        shots = Shots;

        global = (Global)GameObject.FindObjectOfType<Global>();
    }

    public bool Process(int turn)
    {
        //Debug.Log("Processing an arty mission");

        if (turn < startTurn + aimTime)
        {
            unit.ConsumeActionPoint(); //Debug.Log("Consuming action point on " + unit.unitDef.name + " for arty aiming");
        }

        if (turn >= startTurn + aimTime && shotsFired < shots)
        {
            FireShots();
            unit.ConsumeActionPoint(); //Debug.Log("Consuming action point on " + unit.unitDef.name + " for arty shooting");
        }

        // if start+aim+flight = turn for first shot. start+aim+flight+shots-1 = turn for last shot
        if ( turn >= (startTurn + aimTime + flightTime) && turn <= (startTurn + aimTime + (shotsFired - 1) + flightTime ) )
        {
            Impact();
        }

        return IsComplete(turn);
    }

    bool IsComplete(int turn)
    {
        if ( ! cancelled ) return (turn == (startTurn + aimTime + (shots - 1) + flightTime ));
        else return (turn == (startTurn + aimTime + (shotsFired - 1) + flightTime ));
    }
        
    void FireShots()
    {

        // check ammo and consume ammo


        //Debug.Log("Artillery Fires");
        shotsFired++;
        global.gameCamera.CenterCameraOnObject(unit.gameObject);
        global.shootManager.PlayShootEffectArtillery(unit.GetComponent<AudioSource>(), uWeapon.weapon.indirectWeapon.shootProfile, unit.tile.position);
    }
    
    void Impact()
    {
        global.StartCoroutine(ImpactsCoroutine());
    }

    

    IEnumerator ImpactsCoroutine()
    {

        List<Tile> extraTiles = target.extraTiles;
        Weapon iWeapon = uWeapon.weapon.indirectWeapon;

        //List<Tile> targetTiles = target.extraTiles;
        //targetTiles.Insert(0, target.tile);

        for (int i = 1; i <= iWeapon.artyShellsPerSalvo; i++)
        {

            Tile targetTile = target.tile;

            //Debug.Log("<color=purple>" + extraTiles.Count + "</color>");
            //Debug.Log("<color=red>"+indivShellsLanded+"</color>");
            if (target.type == ArtilleryTargetType.Circle)
            {
                //Debug.Log("<color=red>" + indivShellsLanded % (extraTiles.Count + 1) + "</color>");
                if (indivShellsLanded % (extraTiles.Count +1) == 0)
                {
                    targetTile = target.tile;
                    extraTiles.Shuffle();
                } else
                {
                    targetTile = extraTiles[(indivShellsLanded % (extraTiles.Count + 1)) - 1];
                }
            }

            if (target.type == ArtilleryTargetType.Line)
            {
                double perTile = ((double)(shots * iWeapon.artyShellsPerSalvo) / (double)(extraTiles.Count + 1));
                int s = 1;
                //Debug.Log("<color=purple>" + (indivShellsLanded + 1) + "</color>");
                while ( (indivShellsLanded+1) > Math.Round((perTile * s) + 0.001) && s < (extraTiles.Count +1))
                {
                    //Debug.Log("<color=red>" + ((perTile * s) + 0.001) + "</color>");
                    //Debug.Log("<color=green>" + Math.Round((perTile * s) + 0.001) + "</color>");
                    s++;
                }
                if (s == 1) {
                    targetTile = target.tile;
                }
                else {
                    targetTile = extraTiles[s-2];
                }
                
            }

            //int r = global.random.Next(0, targetTiles.Count);

            //Tile targetTile = targetTiles[r];

            global.gameCamera.CenterCameraOnObject(targetTile.hexObject);
            global.shootManager.PlayImpactEffectArtillery(targetTile.hexObject.GetComponent<AudioSource>(), iWeapon.impactProfile, targetTile.position);
            Debug.Log("Play Impact effects");

            //target.tile
            //Debug.Log("Arty Impact on tile: " + targetTile.x.ToString() + "," + targetTile.z.ToString());
            if (targetTile.unit != null)
            {
                Unit targetUnit = targetTile.unit;
                if (iWeapon.damageProfiles.ContainsKey( targetUnit.UnitTargetType() ))
                {
                    DamageProfile damageProfile = iWeapon.damageProfiles[targetUnit.UnitTargetType()];
                    damageProfile.DoHits(uWeapon, iWeapon, targetUnit, 0); // range == 0
                }
            }
            indivShellsLanded++;
            //MonoBehaviour.StartCoroutine(ClearTargetingCrosshair());

            if (i < iWeapon.artyShellsPerSalvo)
            {
                double flux = 0.15;
                double mult = 1 + (global.random.NextDouble() * flux);
                mult = (global.random.NextDouble() < 0.5) ? mult : 1 / mult;
                yield return new WaitForSeconds((float)iWeapon.artyDelayPerShell * (float)mult);
            }
        }
    }

    IEnumerator ClearTargetingCrosshair()
    {
        yield return new WaitForSeconds(0.25f);
        global.ui.HideTargetingCrosshair();
    }
}

public enum ArtilleryTargetType
{
    Point,
    Circle,
    Line
}

public struct PointTarget
{
    public Tile tile;

    public PointTarget(Tile Tile)
    {
        tile = Tile;
    }
}
public struct CircleTarget
{
    public Tile tile;
    public int radius;

    public CircleTarget(Tile Tile, int Radius)
    {
        tile = Tile;
        radius = Radius;
    }
}
public struct LineTarget
{
    public Tile tile;
    public Tile endTile;
    public List<Tile> tiles;

    public LineTarget(Tile Tile, Tile EndTile)
    {
        tile = Tile;
        endTile = EndTile;
        tiles = new List<Tile>();
    }
}

public struct ArtilleryTarget
{
    public Tile tile;
    public List<Tile> extraTiles;
    public ArtilleryTargetType type;
    public int circle_radius;
    public Tile line_endTile;

    ////// Point

    public ArtilleryTarget(ArtilleryTargetType Type, Tile Tile)
    {
        if (Type != ArtilleryTargetType.Point) Debug.Log("Error, wrong constructor called for Artillery Target Type: " + Type.ToString());
        tile = Tile;
        extraTiles = new List<Tile>();
        type = Type;
        circle_radius = 0;
        line_endTile = new Tile();
    }
    public ArtilleryTarget(Tile Tile)
    {
        tile = Tile;
        extraTiles = new List<Tile>();
        type = ArtilleryTargetType.Point;
        circle_radius = 0;
        line_endTile = new Tile();
    }

    ////// Circle

    public ArtilleryTarget(ArtilleryTargetType Type, Tile Tile, int Radius)
    {
        if (Type != ArtilleryTargetType.Circle) Debug.Log("Error, wrong constructor called for Artillery Target Type: " + Type.ToString());
        tile = Tile;

        Global global = (Global)MonoBehaviour.FindObjectOfType<Global>();
        extraTiles = global.map.GetTilesInMoveRange(Tile, Radius);

        foreach (Tile thisTile in extraTiles) thisTile.hex.SetArtyTarget(true);

        type = Type;
        circle_radius = Radius;
        line_endTile = new Tile();
    }
    public ArtilleryTarget(Tile Tile, int Radius)
    {
        tile = Tile;

        Global global = (Global)MonoBehaviour.FindObjectOfType<Global>();
        extraTiles = global.map.GetTilesInMoveRange(Tile, Radius);
        extraTiles.Remove(Tile);

        foreach (Tile thisTile in extraTiles) thisTile.hex.SetArtyTarget(true);

        type = ArtilleryTargetType.Circle;
        circle_radius = Radius;
        line_endTile = new Tile();
    }

    ////// Line

    public ArtilleryTarget(ArtilleryTargetType Type, Tile Tile, Tile EndTile)
    {
        if (Type != ArtilleryTargetType.Line) Debug.Log("Error, wrong constructor called for Artillery Target Type: " + Type.ToString());
        tile = Tile;

        Global global = (Global)MonoBehaviour.FindObjectOfType<Global>();
        extraTiles = new List<Tile>();
        if (EndTile.z == Tile.z)
        {
            Tile nextTile = global.map.GetTileFromXZ(Tile.x + ((EndTile.x > Tile.x) ? 2 : -2), Tile.z);
            while (nextTile != EndTile)
            {
                extraTiles.Add(nextTile);
                nextTile = global.map.GetTileFromXZ(nextTile.x + ((EndTile.x > Tile.x) ? 2 : -2), nextTile.z);
            }
            extraTiles.Add(EndTile);
        }
        else
        {
            Tile nextTile = global.map.GetTileFromXZ(Tile.x + ((EndTile.x > Tile.x) ? 1 : -1), Tile.z + ((EndTile.z > Tile.z) ? 1 : -1));
            while (nextTile != EndTile)
            {
                extraTiles.Add(nextTile);
                nextTile = global.map.GetTileFromXZ(nextTile.x + ((EndTile.x > Tile.x) ? 1 : -1), nextTile.z + ((EndTile.z > Tile.z) ? 1 : -1));
            }
            extraTiles.Add(EndTile);
        }

        foreach (Tile thisTile in extraTiles) thisTile.hex.SetArtyTarget(true);

        type = Type;
        circle_radius = 0;
        line_endTile = EndTile;
    }
    public ArtilleryTarget(Tile Tile, Tile EndTile)
    {
        tile = Tile;
        
        Global global = (Global)MonoBehaviour.FindObjectOfType<Global>();
        extraTiles = new List<Tile>();
        if (EndTile.z == Tile.z)
        {
            Tile nextTile = global.map.GetTileFromXZ(Tile.x + ((EndTile.x > Tile.x) ? 2 : -2), Tile.z);
            while (nextTile != EndTile)
            {
                extraTiles.Add(nextTile);
                nextTile = global.map.GetTileFromXZ(nextTile.x + ((EndTile.x > Tile.x) ? 2 : -2), nextTile.z);
            }
            extraTiles.Add(EndTile);
        } else
        {
            Tile nextTile = global.map.GetTileFromXZ(Tile.x + ((EndTile.x > Tile.x) ? 1 : -1), Tile.z + ((EndTile.z > Tile.z) ? 1 : -1));
            while (nextTile != EndTile)
            {
                extraTiles.Add(nextTile);
                nextTile = global.map.GetTileFromXZ(nextTile.x + ((EndTile.x > Tile.x) ? 1 : -1), nextTile.z + ((EndTile.z > Tile.z) ? 1 : -1));
            }
            extraTiles.Add(EndTile);
        }

        foreach (Tile thisTile in extraTiles) thisTile.hex.SetArtyTarget(true);

        type = ArtilleryTargetType.Line;
        circle_radius = 0;
        line_endTile = EndTile;
    }

}