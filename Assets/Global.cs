using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.ArrayExtensions;

public static class Ext {
    private static System.Random rng = new System.Random();
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public static class Lib
{
    public static System.Random random = new System.Random();
}

public class Global : MonoBehaviour
{
    public Map map;
    public PlayerInput playerInput;
    public Artillery artillery;
    public Moves moves;
    public TurnManager turnManager;
    public AiManager aiManager;
    public FlyCamera gameCamera;
    public GameObject cameraObj;
    public UI ui;
    public Pathfinder pathfinder;
    public SoundManager soundManager;
    public ShootManager shootManager;
    public ArtilleryManager artilleryManager;
    public MapMesh mapMesh;

    Dictionary<int, List<Unit>> units;
    public System.Random random;

    bool initialised = false;

    void Awake() {
        mapMesh = (MapMesh)FindObjectOfType<MapMesh>();
        gameCamera = (FlyCamera)FindObjectOfType<FlyCamera>();
        map = (Map)FindObjectOfType<Map>();
        playerInput = (PlayerInput)FindObjectOfType<PlayerInput>();
        aiManager = (AiManager)FindObjectOfType<AiManager>();
        ui = (UI)FindObjectOfType<UI>();
        soundManager = (SoundManager)FindObjectOfType<SoundManager>();
        shootManager = (ShootManager)FindObjectOfType<ShootManager>();
        artilleryManager = (ArtilleryManager)FindObjectOfType<ArtilleryManager>();
        pathfinder = new Pathfinder(map);
    }

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialised) Initialise();
    }

    void Initialise()
    {

        initialised = true;
    }

  
}

public static class Store
{
    private static Dictionary<int, List<Unit>> units;

    public static void PopulateUnits()
    {
        units = new Dictionary<int, List<Unit>>();

        units[1] = new List<Unit>();
        units[2] = new List<Unit>();

        Unit[] allUnits = (Unit[])GameObject.FindObjectsOfType<Unit>();

        //Debug.Log("num units " + allUnits.Length);
        for (int i = 0; i < allUnits.Length; i++)
        {
            Unit unit = allUnits[i];
            //Debug.Log("key " + unit.team.ToString());
            units[unit.team].Add(unit);
        }
    }

    public static List<Unit> GetAliveTeamUnits(int team)
    {
        List<Unit> teamUnits = new List<Unit>();

        Unit[] allUnits = (Unit[])GameObject.FindObjectsOfType<Unit>();
        for (int i = 0; i < allUnits.Length; i++)
        {
            if (allUnits[i].team == team && allUnits[i].alive)
            {
                teamUnits.Add(allUnits[i]);
            }
        }

        return teamUnits;

        // or return units[team] where alive //
    }
}