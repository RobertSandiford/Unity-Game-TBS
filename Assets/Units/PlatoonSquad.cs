using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;


public class Squad //: MonoBehaviour
{

    Unit platoon;

    //GameObject _3dModel = null;

    // refs
    //public GameObject moveTarget;
    //public GameObject cargoTarget;
    //public List<GameObject> moveTargets;
    //public List<GameObject> cargoTargets;
    //public GameObject fortDiscObj;
    //public Material[] materials;
    public Map map;
    private UI ui;
    private Global global;

    // mechanics
    //private bool started = false;
    public List<Tile> visibleTiles;
    public List<Unit> visibleEnemyUnits;
    public List<Unit> enemiesWithVisionOnThis;
    //public List<Tile> availableMoves;
    //public List<Tile> loadOptions;

    // unit defs
    //public UnitType unitType;
    //public SquadDef squadDef;
    
    public SquadDef squadDef;

    // game vars
    public int playerTeam = 1;
    public int team;
    public int group;
    public Color groupColor;
    //public int x;
    //public int z;
    //public Tile tile;
    public int health = 5;
    public int actionPointsMax = 1;
    public int actionPoints = 1;
    public bool hasFired = false;
    public bool alive = true;
    public Cargo cargo;
    public Unit carrierUnit = null;
    public bool visible;
    public int altitude = 0;
    public bool buildFortAtTurnEnd = false;

    public UnitWeapon setupUWeapon;
    public int setupProgress;
    public int packupProgress;
    public bool isSettingUp = false;
    public bool isPackingUp = false;
    public bool isSetUp = false;
    public bool isPackedUp = true;

    public bool inCargo = false;

    public UnitUI unitUi;

    public List<Action> availableActions;

    public static bool alwaysShowEnemy = false;

    static double unitHeight = 2.0;

    //placeholder
    public TargetType targetType = TargetType.Infantry;
    public double hitability;
    public int armor;
    public Countermeasures countermeasures;
    public string unitName;
    public string unitShortName;
    public int moves;
    public int minAltitude;
    public int maxAltitude;

    public FortType fortType;

    //////////////////////////
    /// Initialisation
    //////////////////////////
     
    public static GameObject unitObject;

    public static Unit MakeUnit() {
        return new Unit();
    }

    public Squad(SquadDef SquadDef, Unit Platoon)
    {
        squadDef = SquadDef;
        platoon = Platoon;
        Initialise();
    }

    // Initialise is called manually when the unit is created, to initialise variables
    public void Initialise()
    {
        SetNames();

        //visibleTiles = new List<Tile>();
        //visibleEnemyUnits = new List<Unit>();
        //enemiesWithVisionOnThis = new List<Unit>();
        //availableMoves = new List<Tile>();
        cargo = new Cargo(squadDef.cargoDef);
        //unitUi = (UnitUI)gameObject.transform.Find("UnitCanvas").GetComponent<UnitUI>();
        //unitUi.Initialise();
        //UpdateCargoText();

        //if (team == 1) visible = true;

        //if (squadDef.minAltitude > 0)
        //    altitude = Math.Max(squadDef.minAltitude, map.GetMinTileAlt(tile));
        //else
        //altitude = 0; 

        global = (Global)GameObject.FindObjectOfType<Global>();
        map = (Map)GameObject.FindObjectOfType<Map>();
        ui = (UI)GameObject.FindObjectOfType<UI>();

    }

    //public void Awake()
    //{
    //    global = (Global)FindObjectOfType<Global>();
    //    map = (Map)FindObjectOfType<Map>();
    //    ui = (UI)FindObjectOfType<UI>();
    //}

    void SetNames()
    {
        unitName = squadDef.name;
        unitShortName = squadDef.shortName;
    }

    //void Setup3dModel()
    //{
    //    switch (squadDef.name) {
    //        case "BMP-2":
    //            _3dModel = Instantiate( (GameObject)Resources.Load("Models/Dummy IFV/Dummy_IFV_Prefab") );
    //            _3dModel.transform.SetParent(this.gameObject.transform);
    //            break;
    //        case "PAK40 75mm":
    //            //Debug.Log("Found a PAK");
    //            _3dModel = Instantiate( (GameObject)Resources.Load("Models/6Pounder/6Pounder_Prefab") );
    //            _3dModel.transform.SetParent(this.gameObject.transform);
   //             break;
    //     }
    //}

    //private void SetMaterial()
    //{
    //    MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
    //    switch (squadDef.unitClass)
    //    {
    //        case UnitClass.Infantry:
    //        case UnitClass.Mortar:
    //        case UnitClass.Artillery:
    //        case UnitClass.Gun:
    //            meshRenderer.material = materials[1];
    //            break;
    //        case UnitClass.Truck:
    //        case UnitClass.Apc:
    //        case UnitClass.WheeledApc:
    //        case UnitClass.ApcMortar:
    //        case UnitClass.WheeledMortar:
    //            meshRenderer.material = materials[2];
    //            break;
    //        case UnitClass.Ifv:
    //        case UnitClass.TrackedApc:
    //        case UnitClass.Carrier:
    //        case UnitClass.HalftrackOpen:
    //        case UnitClass.Halftrack:
    //        case UnitClass.MechMortar:
    //        case UnitClass.MechArtillery:
    //            meshRenderer.material = materials[3];
    //            break;
    //        case UnitClass.Tank:
    //        case UnitClass.PanzerJager:
    //            meshRenderer.material = materials[4];
    //            break;
    //        case UnitClass.Helicopter:
    //            meshRenderer.material = materials[5];
    //            break;
    //    }
        
    //}

    //private void SetTeamColor()
    //{
    //    MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
    //    Color color = new Color(0.4f, 0.7f, 1.0f, 1.0f);
    //    if (team == 1)
    //        color = new Color(0.4f, 0.7f, 1.0f, 1.0f);
    //    //meshRenderer.material.SetColor("_Color", new Color(0.2f, 0.5f, 0.9f, 1.0f));
    //    if (team == 2)
    //        color = new Color(1.0f, 0.5f, 0.25f, 1.0f);

    //    color = color + groupColor;
    //    meshRenderer.material.SetColor("_Color", color);

    //}

    public void RefillActions()
    {
        actionPoints = actionPointsMax;
        hasFired = false;
        if (team == 1)
        {
            if (isSetUp) 
                unitUi.SetIsSetup();
            else 
                unitUi.SetHasActions(true);
        }
    }

    public void EventText(string text)
    {
        unitUi.AddEventMessage(text, 1.0);
    }

    //public void AssignSquadDef(SquadDef SquadDef)
    //{
    //    squadDef = ObjectExtensions.Copy(SquadDef);
    //}
    



    public bool CanFortify()
    {
        /*switch (squadDef.unitClass)
        {
            case UnitClass.Infantry:
            case UnitClass.Mortar:
            case UnitClass.Artillery:
            case UnitClass.Gun:
                return true;
        }*/
        return false;
    }

    
    public int MaxSoldiers() // return the current number of active soldiers
    {
        Debug.Log(".MaxSoldiers() still to be implemented");
        return 0;
    }
       
    public int CurrentSoldiers() // return the current number of active soldiers
    {
        Debug.Log(".CurrentSoldiers() still to be implemented");
        return 0;
    }
    
    public int Moves()
    {
        return moves;
    }

    public int Altitude()
    {
        return altitude;
    }
    
    public int MinAltitude()
    {
        return minAltitude;
    }
    public int MaxAltitude()
    {
        return maxAltitude;
    }

    //public void PlayerMoveTo(int X, int Z, int altitude = 0)
    //{
    //    PlayerMoveTo(map.GetTileFromXZ(X, Z));
    //}
    //public void PlayerMoveTo(Tile to, int altitude = 0)
    //{
    //    //map.MarkHexesTeamVisible(visibleTiles, false);
    //    MoveTo(to, altitude);
    //    UpdateUnitActionDisplays();
    //    PlayerMoveEnd();
    //}

    //public void AiMoveTo(Tile to, int altitude = 0)
    //{
    //    MoveTo(to, altitude);
    //    RecalculateUnitVisibility(1);
    //    GetVisibility();
    //}

    //public void RecalculateUnitVisibility(int toTeam)
    //{

    //    List<Unit> teamUnits = Store.GetAliveTeamUnits(toTeam);
    //    foreach (Unit teamUnit in teamUnits)
    //    {

    //        if (teamUnit.visibleTiles.Contains(tile))
    //        {
    //            // can see
    //            if (!teamUnit.visibleEnemyUnits.Contains(this)) teamUnit.Spotted(this);
    //        }
    //        else
    //        {
    //            // can't see
    //            if (teamUnit.visibleEnemyUnits.Contains(this)) teamUnit.LostSightOf(this);
    //        }

    //    }
    //}

    //public void MoveTo(int X, int Z, int altitude = 0)
    //{
    //    MoveTo( map.GetTileFromXZ(X, Z) );
    //}

    //public void MoveTo(Tile to, int newAltitude = 0)
    //{
    //    map.UnassignUnitFromTile(tile);
    //    tile = to;
    //    this.altitude = newAltitude;
    //    ConsumeActionPoint(); //Debug.Log("Consuming action point on " + squadDef.name + " for moving");
    //    MoveToCurrentTile();
    //}

    //void MoveToCurrentTile()
    //{
    //    Vector3 location = map.GetHexLocation(tile.x, tile.z);
    //    location.y = location.y + (float)0.0;
    //    location.y = location.y + altitude * map.YMultiplier();
    //    transform.position = location;

    //    map.AssignUnitToTile(this, tile);

    //    unitUi.PositionCanvas();
    //}

    //public void Show()
    //{
    //    //ShowUnit();
    //    gameObject.GetComponent<MeshRenderer>().enabled = true;
    //    if (_3dModel != null) _3dModel.GetComponent<MeshRenderer>().enabled = true;
    //    ((UnitUI)gameObject.transform.Find("UnitCanvas").GetComponent<UnitUI>()).Show();
    //    visible = true;
    //}
    //public void Hide()
    //{
    //    if ( alive && alwaysShowEnemy ) return;

    //    gameObject.GetComponent<MeshRenderer>().enabled = false;
    //    if (_3dModel != null) _3dModel.GetComponent<MeshRenderer>().enabled = false;
    //    ((UnitUI)gameObject.transform.Find("UnitCanvas").GetComponent<UnitUI>()).Hide();
    //    visible = false;
    //}
    //private void ShowUnit()
    //{
    //    gameObject.SetActive(true);
    //}
    //private void HideUnit()
    //{
    //    gameObject.SetActive(false);
    //}


    //public void PlayerLoadAt(Tile carrierTile)
    //{
    //    carrierUnit = carrierTile.unit;
    //    map.UnassignUnitFromTile(tile.x, tile.z);
    //    tile = carrierUnit.tile;
    //    LoadInto(carrierUnit);

    //    PlayerMoveEnd();
    //}

    //public void LoadInto(Unit carrierUnit)
    //{
    //    PutIntoCargoSilent(carrierUnit);
    //    ConsumeActionPoint();//Debug.Log("Consuming action point on " + squadDef.name + " for loading");
    //    carrierUnit.ConsumeActionPoint();//Debug.Log("Consuming action point on " + carrierUnit.name + " for receiving cargo");
    //}

    //public void PutIntoCargoSilent(Unit carrierUnit)
    //{
    //    inCargo = true;
    //    HideUnit();
    //    //Debug.Log(carrierUnit);
    //    carrierUnit.cargo.LoadUnit(this);
    //    carrierUnit.UpdateCargoText();
    //}

    //public void UpdateCargoText()
    //{
    //    List<string> cargoUnits = new List<string>();
    //    if (cargo.containedUnits.Count > 0) {
    //        foreach (Unit unit in cargo.containedUnits)
    //        {
    //            cargoUnits.Add(unit.unitShortName);
    //        }
    //        if (cargoUnits.Count > 0)
    //        {
    //            unitUi.SetCargoText( string.Concat("(", string.Join(", ", cargoUnits), ")") ); 
    //        } else
    //        {
    //            unitUi.SetCargoText("");
    //        }
    //    } else {
    //        unitUi.SetCargoText("");
    //    }
        
    //}

    //public void PlayerUnloadTo(Tile to)
    //{
    //    Unit dismountSquad = cargo.containedUnits[0];
    //    cargo.UnloadUnit(dismountSquad);
    //    unitUi.SetCargoText("");
    //    dismountSquad.DebusTo(to);
    //    unitUi.SetCargoText("");
    //    PlayerMoveEnd();
    //}

    //public void PlayerPickupAt(Tile to)
    //{
    //    Unit pickupUnit = to.unit;
    //    MoveTo(to);
    //    pickupUnit.LoadInto(this);
        
    //    PlayerMoveEnd();
    //}
    
    //public void PlayerDropoffTo(Action action)
    //{
    //    // move unless the vehicle does not need to to drop off -- AP should only be consumed if it moves
    //    if (action.path[action.path.Length - 2] != tile) MoveTo(action.path[action.path.Length - 2]);
        
    //    Unit dismountSquad = cargo.containedUnits[0];
    //    cargo.UnloadUnit(dismountSquad);
    //    dismountSquad.DebusTo(action.tile);
    //    UpdateCargoText();
    //    PlayerMoveEnd();
    //}

    //public void PlayerMoveEnd()
    //{
    //    Detection.ShowHideVisibleEnemies(1);
    //    map.ShowTeamVision(team);
    //}

    //public void DebusTo(Tile to)
    //{
    //    //map.UnassignUnitFromTile(tile.x, tile.z);
    //    ExitCargo(to);
    //    ConsumeActionPoint();//Debug.Log("Consuming action point on " + squadDef.name + " for exiting");
    //}

    //public void BailOut(Tile to)
    //{
    //    ExitCargo(to);
    //    EventText("Bail Out");
    //}

    //private void ExitCargo(Tile to)
    //{
    //    tile = to;
    //    carrierUnit = null;
    //    ShowUnit();
    //    MoveToCurrentTile();
    //}

    //void ShowMoveOptions()
    //{
    //    DestroyAvailableMoves();

    //    if (actionPoints == 0) return;

    //    availableMoves = map.GetMoves(new int[] { tile.x, tile.z }, moves);
    //    moveTargets = new List<GameObject>();
    //    foreach ( Tile move in availableMoves )
    //    {
    //        GameObject target = Instantiate(moveTarget);
    //        Vector3 location = map.GetHexLocation(move);
    //        location.y = location.y + (float) 0.0;
    //        target.transform.position = location;
    //        moveTargets.Add(target);
    //    }

    //}

    //void DestroyAvailableMoves()
    //{
    //    foreach (GameObject target in moveTargets)
    //    {
    //        Destroy(target);
    //        // GC?
    //    }
    //    moveTargets = new List<GameObject>();
    //    availableMoves = new List<Tile>();
    //}





    //void DestroyLoadOptions()
    //{
    //    loadOptions = new List<Tile>();
    //    DestroyCargoTargets();
    //}

    //void DestroyCargoTargets()
    //{
    //    foreach (GameObject target in cargoTargets)
    //    {
    //        Destroy(target);
    //        // GC?
    //    }
    //    cargoTargets = new List<GameObject>();
    //}


    //////////////////////////////////
    /// Actions
    //////////////////////////////////

    //public bool CanDoAction()
    //{
    //    if (actionPoints == 0) return false;
    //    return true;
    //}

    //public List<Action> GetActions(int targetAltitude = 0)
    //{
    //    if ( ! CanDoAction() ) return new List<Action>();

    //    Dictionary<string, Move> moves = GetPossibleMoves();
    //    //Debug.Log(moves.ToString());

    //    //List<Action> moves = (remainingMoves == -1) ? unit.GetMoveActions() : unit.GetMoveActions(remainingMoves, targetAltitude);
    //    List<Action> moveActions = GetMoveActions(moves);
    //    List<Action> loadActions = GetLoadActions(moves);
    //    List<Action> unloadActions = GetUnloadActions(moves);
    //    List<Action> pickUpActions = GetPickupActions(moves);

    //    Dictionary<string, Action> tempActions = new Dictionary<string, Action>();

    //    //MergeInActions(ref tempActions, moves);
    //    //MergeInActions(ref tempActions, loads);
    //    //MergeInActions(ref tempActions, unloads);
    //    //MergeInActions(ref tempActions, pickUps);

    //    foreach (Action action in moveActions)
    //    {
    //        string k = action.tile.x.ToString() + "-" + action.tile.z.ToString();
    //        tempActions[k] = action;
    //    }
    //    foreach (Action action in loadActions)
    //    {
    //        string k = action.tile.x.ToString() + "-" + action.tile.z.ToString();
    //        if (tempActions.ContainsKey(k))
    //        {
    //            tempActions[k] = MergeActions(tempActions[k], action);
    //        }
    //        else
    //        {
    //            tempActions[k] = action;
    //        }
    //    }
    //    foreach (Action action in unloadActions)
    //    {
    //        string k = action.tile.x.ToString() + "-" + action.tile.z.ToString();
    //        if (tempActions.ContainsKey(k))
    //        {
    //            tempActions[k] = MergeActions(tempActions[k], action);
    //        }
    //        else
    //        {
    //            tempActions[k] = action;
    //        }
    //    }
    //    foreach (Action action in pickUpActions)
    //    {
    //        string k = action.tile.x.ToString() + "-" + action.tile.z.ToString();
    //        if (tempActions.ContainsKey(k))
    //        {
    //            tempActions[k] = MergeActions(tempActions[k], action);
    //        }
    //        else
    //        {
    //            tempActions[k] = action;
    //        }
    //    }

    //    List<Action> actions = new List<Action>();
    //    foreach (KeyValuePair<string, Action> kv in tempActions)
    //    {
    //        actions.Add(kv.Value);
    //    }

    //    return actions;
    //}

    //public Dictionary<string, Move> GetPossibleMoves(int targetAltitude = 0) {
    //    MovementData md = MovementData();
    //    //Debug.Log("md");
    //    //Debug.Log(md.ToString());
        
    //    return global.pathfinder.FindAllMoves(tile, MovementData());
    //}

    //public Action MergeActions(Action action, Action newAction)
    //{
    //    if (action.type == ActionType.Combo)
    //    {
    //        action.children.Add(newAction);
    //        return action;
    //    }
    //    else
    //    {
    //        Action comboAction = new Action(action.unit, ActionType.Combo, action.tile, new List<Action> { action, newAction });
    //        return comboAction;
    //    }
    //}

    //public List<Tile> GetMoves(int limitedMoves = -1)
    //{
    //    if (actionPoints == 0) return new List<Tile>();

    //    List<Tile> moves = (limitedMoves == -1)
    //        ? map.GetMoves(tile, squadDef.moves)
    //        : map.GetMoves(tile, limitedMoves);

    //    List<Tile> ret = new List<Tile>();
    //    foreach (Tile move in moves)
    //    {
    //        ret.Add(move);
    //    }
    //    return ret;
    //}

    //public List<Action> GetMoveActions()
    //{
    //    if (maxAltitude > 0)
    //    {
    //        return GetArialMoveActions();
    //    }
    //    else
    //    {
    //        return GetGroundMoveActions();
    //    }
    //}

    //public List<Action> GetMoveActions(Dictionary<string, Move> moves)
    //{
    //    if (maxAltitude > 0)
    //    {
    //        return GetArialMoveActions(moves);
    //    }
    //    else
    //    {
    //        return GetGroundMoveActions(moves);
    //    }
    //}

    //public List<Action> GetMoveActions(int limitedMoves, int altitude)
    //{
    //    return GetMoveActions(); ///////// Placeholder

    //    //if (actionPoints == 0) return new List<Action>();

    //    //List<Action> actions = new List<Action>();
    //    //List<Tile> options = GetMoves(limitedMoves);
    //    //foreach (Tile tile in options)
    //    //{
    //    //    actions.Add(new Action(this, ActionType.Move, tile));
    //    //}
    //    //return actions;
    //}


    //public List<Action> GetGroundMoveActions()
    //{

    //    if (actionPoints == 0) return new List<Action>();

    //    List<Action> actions = new List<Action>();

    //    Dictionary<string, Move> moves = global.pathfinder.FindAllMoves(tile, MovementData());
    //    foreach (var moveKvp in moves)
    //    {
    //        string key = moveKvp.Key;
    //        Move move = moveKvp.Value;

    //        Tile[] path = new Tile[move.steps.Count];
    //        for (int i = 0; i < move.steps.Count; i++) {
    //            path[i] = move.steps[i].tile;
    //        }

    //        if (move.tile.unit == null)
    //            actions.Add(new Action(this, ActionType.Move, move.tile, path));
    //    }

    //    return actions;
    //}

    //public List<Action> GetGroundMoveActions(Dictionary<string, Move> moves)
    //{

    //    List<Action> actions = new List<Action>();
        
    //    foreach (var moveKvp in moves)
    //    {
    //        string key = moveKvp.Key;
    //        Move move = moveKvp.Value;

    //        Tile[] path = new Tile[move.steps.Count];
    //        for (int i = 0; i < move.steps.Count; i++) {
    //            path[i] = move.steps[i].tile;
    //        }

    //        if (move.tile.unit == null)
    //            actions.Add(new Action(this, ActionType.Move, move.tile, path));
    //    }

    //    return actions;
    //}
    
    //public List<Action> GetArialMoveActions()
    //{
    //    if (actionPoints == 0) return new List<Action>();

    //    int currentHeight = tile.y + altitude;

    //    double adjustedAltGainCost = squadDef.gainAltCost / map.baseHeightMultiplier;
    //    double adjustedLoseAltPayment = squadDef.loseAltPayment / map.baseHeightMultiplier;

    //    int maxPossibleHeight = currentHeight + (int)Math.Floor((double)squadDef.moves / adjustedAltGainCost);
    //    //Debug.Log("max poss height" + maxPossibleHeight);

    //    List<Action> actions = new List<Action>();
    //    List<ArialMove> options = GetArialMoves();
    //    foreach (ArialMove arialMove in options)
    //    {
    //        int dist = map.GetMoveDistance(tile, arialMove.tile);
    //        int minTileAlt = map.GetMinTileAlt(arialMove.tile);
    //        if (map.CanLandOnTile(arialMove.tile))
    //        {
    //            minTileAlt = 0;
    //        }
    //        int minHeight = arialMove.tile.y + minTileAlt;
    //        minHeight = Math.Max(currentHeight - squadDef.moves, minHeight);

    //        int maxHeight = 0;
    //        if (dist <= squadDef.moves)
    //        {
    //            maxHeight = currentHeight + (int)Math.Floor((double)(squadDef.moves - dist) / adjustedAltGainCost);
    //        } else
    //        {
    //            maxHeight = currentHeight - (int)Math.Ceiling((double)(dist - squadDef.moves) / adjustedLoseAltPayment);
    //        }
    //        maxHeight = Math.Min(maxHeight, squadDef.maxAltitude);

    //        List<int> altitudeOptions = new List<int>();

    //        //Debug.Log("min " + minHeight);
    //        //Debug.Log("max " + maxHeight);

    //        if (minHeight <= maxHeight)
    //        {
    //            for (int height = minHeight; height <= maxHeight; height++)
    //            {
    //                if (height == minHeight || height == maxHeight || height % 5 == 0 ) {
    //                    int alt = height - arialMove.tile.y;
    //                    altitudeOptions.Add(alt);
    //                }
    //                //Debug.Log(alt);
    //            }

    //            actions.Add(new Action(this, ActionType.ArialMove, arialMove.tile, altitudeOptions));

    //        } else
    //        {
    //            /// Is this to highlight that there was an error??
    //            altitudeOptions.Add(999);
    //            actions.Add(new Action(this, ActionType.ArialMove, arialMove.tile, altitudeOptions));
    //        }

    //    }
    //    return actions;
    //}
    
    //public List<Action> GetArialMoveActions(Dictionary<string, Move> moves) {
    //    Debug.Log("Arial Moves Placeholder - not written yet");
    //    return new List<Action>();
    //}

    //public List<ArialMove> GetArialMoves()
    //{
    //    if (actionPoints == 0) return new List<ArialMove>();

    //    List<ArialMove> moves = map.GetArialMoves(tile, squadDef.moves, altitude, squadDef.gainAltCost, squadDef.loseAltPayment);

    //    //List<Tile> ret = new List<Tile>();
    //    //foreach (int[] move in moves)
    //   // {
    //    //    ret.Add(map.GetTileFromXZ(move[0], move[1]));
    //    //}
    //    //return ret;

    //    //Debug.Log("Arial Moves");
    //    return moves;
    //}
    
    //public List<Tile> GetAvailableLoadOptions() {
    //    Dictionary<string, Move> moves = GetPossibleMoves();
    //    return GetAvailableLoadOptions(moves);
    //}
    //public List<Tile> GetAvailableLoadOptions(Dictionary<string, Move> moves)
    //{
    //    DestroyLoadOptions();

    //    // return the empty list if not a transportable type
    //    //if (squadDef.targetType != TargetType.Infantry) return loadOptions;

    //    //List<Tile> tiles = map.GetTilesInMoveRange(tile, squadDef.moves);
    //    //foreach (Tile tile in tiles)
    //    foreach (var kvp in moves) {
    //        Move move = kvp.Value;
    //        if (move.tile.unit != null)
    //        {
    //            if (move.tile.unit.team == team)
    //            {
    //                if (move.tile.unit.altitude == 0)
    //                {
    //                    if (move.tile.unit.cargo.CanLoad(this)) {

    //                        //Debug.Log("Found a transport we can get into");
    //                        loadOptions.Add(move.tile);
                            
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    return loadOptions;
    //}

    //public void ShowAvailableLoadOptions()
    //{

    //    DestroyLoadOptions();

    //    if (actionPoints == 0) return;

    //    GetAvailableLoadOptions();

    //    cargoTargets = new List<GameObject>(); // clear cargo targets
    //    foreach (Tile tile in loadOptions)
    //    {
    //        GameObject target = Instantiate(cargoTarget);

    //        //Debug.Log("getting loc");
    //        //Debug.Log(tile.x.ToString() + " " + tile.z.ToString());

    //        Vector3 location = map.GetHexLocation(tile.x, tile.z);
    //        location.y = location.y + (float)0.2;
    //        target.transform.position = location;
    //        cargoTargets.Add(target);
    //    }
    //}




    //public MovementData MovementData()
    //{
    //    return squadDef.movementData;
    //}

    //public List<Action> GetDropoffActions() {
    //    List<Action> tempActions = GetMoveActions();
    //    for (int i = 0; i < tempActions.Count; i++) {
    //        tempActions[i] = new Action(tempActions[i].unit, ActionType.Dropoff, tempActions[i].tile, tempActions[i].path);
    //    }
    //    return tempActions;
    //}


    
    //public List<Action> GetLoadActions() {
    //    Dictionary<string, Move> moves = GetPossibleMoves();
    //    return GetLoadActions(moves);
    //}

    //public List<Action> GetLoadActions(Dictionary<string, Move> moves)
    //{
    //    if (actionPoints == 0) return new List<Action>();

    //    List<Action> actions = new List<Action>();
    //    List<Tile> options = GetAvailableLoadOptions(moves);
    //    foreach (Tile tile in options)
    //    {
    //        actions.Add(new Action(this, ActionType.Load, tile));
    //    }
    //    return actions;
    //}
    
    
    //public List<Action> GetUnloadActions() {
    //    Dictionary<string, Move> moves = GetPossibleMoves();
    //    return GetUnloadActions(moves);
    //}
    
    //public List<Action> GetUnloadActions(Dictionary<string, Move> moves)
    //{
    //    if (actionPoints == 0) return new List<Action>();

    //    List<Action> actions = new List<Action>();
    //    List<Tile> options = GetUnloadOptions(moves);
    //    foreach(Tile tile in options)
    //    {
    //        actions.Add(new Action(this, ActionType.Unload, tile));
    //    }
    //    return actions;
    //}

    //public List<Tile> GetUnloadOptions(Dictionary<string, Move> moves)
    //{
    //    // return empty if cannot transport
    //    if (cargo.squads == 0) return new List<Tile>();

    //    // return empty if not landed
    //    if (altitude > 0) return new List<Tile>();

    //    // return empty if empty
    //    if (cargo.containedUnits.Count == 0) return new List<Tile>();
        

    //    // return empty if unit has no actions left
    //    if (actionPoints == 0) return new List<Tile>();

    //    int unloadDistance = 1;

    //    return map.GetMovesReturnTiles(tile, unloadDistance);
    //}


    //public bool CanDoPickup()
    //{
    //    // return false if has no actions left
    //    if (actionPoints == 0) return false;

    //    // return empty if cannot transport
    //    if (cargo.squads == 0) return false;

    //    // return empty if not landed
    //    //if (altitude == 0) return new List<Tile>();

    //    // for now, return false if it is a helo
    //    if (squadDef.maxAltitude > 0) return false;

    //    // return false if full
    //    if (cargo.containedSoldiers > cargo.soldiers) return false;

    //    return true;
    //}
    
    //public List<Action> GetPickupActions() {
    //    Dictionary<string, Move> moves = GetPossibleMoves();
    //    return GetPickupActions(moves);
    //}

    //public List<Action> GetPickupActions(Dictionary<string, Move> moves)
    //{
    //    if ( ! CanDoAction() ) return new List<Action>();

    //    if ( ! CanDoPickup() ) return new List<Action>();

    //    List<Action> actions = new List<Action>();
        
    //    //Dictionary<string, Move> moves = global.pathfinder.FindAllMoves(tile, MovementData());
    //    foreach (KeyValuePair<string, Move> moveKvp in moves)
    //    {
    //        string key = moveKvp.Key;
    //        Move move = moveKvp.Value;

    //        if (move.tile.unit != null && move.tile.unit.team == team)
    //        {
    //            if (cargo.transportable.Contains(move.tile.unit.squadDef.unitClass) && cargo.CanLoad(move.tile.unit))
    //            {
    //                actions.Add(new Action(this, ActionType.Pickup, move.tile));
    //            }
    //        }
    //    }
        
    //    return actions;
    //}


    ////////////////////////////////////
    /// Unit Selection
    /////////// ////////////////////////

    //public void Select()
    //{
    //    if (team == playerTeam)
    //    {
    //        SelectFriendly();
    //    }
    //    else
    //    {
    //        SelectEnemy();
    //    }
    //}

    //public void SelectFriendly()
    //{
    //    //Debug.Log("<color=red>Unit Selecting</color>");
    //    ui.ShowUnitUI(this);
    //    ui.UpdateTargetingPanelThisUnit(squadDef);

    //    UpdateUnitActionDisplays();
        

    //    map.ResetBlocking();
    //}

    //public void SelectEnemy()
    //{
    //    //ShowMoveOptions();

    //    ui.ShowUnitUI(this);

    //    //ShowVisibility();
    //}

    //public void Deselect()
    //{
    //    DestroyAvailableMoves();
    //    DestroyLoadOptions();

    //    //ResetVisibility();
    //}

    //public void UpdateUnitActionDisplays()
    //{
    //    ui.ShowUnitUI(this);
    //    //ShowMoveOptions();
    //    //ShowAvailableLoadOptions();
    //    ShowVisionAndTargets();
    //}
    
    //public void ShowVisionAndTargets()
    //{
    //    //Debug.Log("<color=blue>Updating Vision and Targets</color>");
    //    ShowVisibility();
    //}
    
    ////public void ShowVision()
    ////{
    ////    Debug.Log("<color=orange>Showing Vision</color>");
    ////    ShowVisibility();
    ////    ShowShootableTargets();
    ////}

    //public void ShowVisionAndTargets(Tile forTile, int forAltitude = 0)
    //{
    //    ShowVisibility(forTile, forAltitude);
    //    //List<Tiles> visibleTiles = GetVisiblity(forTile, forAltitude)
    //        //ShowVisibilityTheseTiles(forTile, forAltitude);
    //    //ShowShootableTargets(forTile, forAltitude);
    //    //ShowShootableTargetsInList(visibleTiles);
    //}

    ////public void ResetVisibility()
    ////{
    ////    //map.ResetVisibility();
    ////}

    //public void Spotted(Unit unit)
    //{
    //    visibleEnemyUnits.Add(unit);
    //    unit.SpottedBy(this);
    //}

    //public void LostSightOf(Unit unit)
    //{
    //    visibleEnemyUnits.Remove(unit);
    //    unit.LostSightOfBy(this);
    //}

    //public void SpottedBy(Unit unit) {
    //    //Debug.Log(squadDef.name + " spotted");
    //    enemiesWithVisionOnThis.Add(unit);
    //    if ( ! visible && team != 1 )
    //    {
    //        Show();
    //    }
    //}

    //public void LostSightOfBy(Unit unit)
    //{
    //    enemiesWithVisionOnThis.Remove(unit);
    //    if ( team != 1 && enemiesWithVisionOnThis.Count == 0 )
    //    {
    //        Hide();
    //    }
    //}

    //public void GetVisibility()
    //{
    //    visibleTiles =  map.GetUnitVision(this);
    //    visibleEnemyUnits = new List<Unit>();
        
    //    for (int i = 0; i < visibleTiles.Count; i++)
    //    {
    //        if (visibleTiles[i].unit != null)
    //        {
    //            Unit unit = visibleTiles[i].unit;
    //            if (unit.alive && unit.team != team)
    //            {
    //                visibleEnemyUnits.Add(unit);
    //            }
    //        }
    //    }
    //}

    //public void ShowVisibility() {
    //    List<Tile> oldVisibleTile = visibleTiles;
    //    List<Unit> oldVisibleEnemyUnits = visibleEnemyUnits;

    //    GetVisibility();
        
    //    map.ResetArtilleryTargets();
    //    map.ResetUnitVision();
    //    map.MarkHexesUnitVisible(visibleTiles);


    //    List<Unit> tempVisibleEnemyUnits = new List<Unit>();

    //    //Debug.Log("<color=blue>Showing Visibility</color>");

    //    List<Tile> tempVisibleTiles = map.GetUnitVision(this);

    //    map.ResetUnitVision();
    //    map.MarkHexesUnitVisible(visibleTiles);
    //    map.ShowTeamVision(team);

    //    List<Unit> newVisibleEnemyUnits = visibleEnemyUnits.Except(oldVisibleEnemyUnits).ToList();
    //    List<Unit> lostSightOfEnemyUnits = oldVisibleEnemyUnits.Except(visibleEnemyUnits).ToList();

    //    foreach (Unit unit in newVisibleEnemyUnits) {
    //        Spotted(unit);
    //        //unit.SpottedBy(this);
    //    }
    //    foreach (Unit unit in lostSightOfEnemyUnits) {
    //        LostSightOf(unit);
    //        //unit.LostSightOfBy(this);
    //    }

    //    ShowShootableTargets();

    //}

    //public void ShowVisibility(Tile forTile, int forAltitude = 0)
    //{
    //    List<Tile> visibleTiles = GetVisibleTilesFor(forTile, forAltitude);
        
    //    map.ResetArtilleryTargets();
    //    map.ResetUnitVision();
    //    map.MarkHexesUnitVisible(visibleTiles);
        
    //    ShowShootableTargetsFrom(forTile, forAltitude, visibleTiles);
    //}
    
    //public List<Tile> GetVisibleTilesFor(Tile forTile, int forAltitude = 0) {
    //    return map.GetVisionFor(forTile, forAltitude, GetMaxWeaponRange());
    //}

    
    //public void ShowShootableTargets()
    //{

    //    List<Unit> targets = GetShootableTargets();

    //    foreach (Unit target in targets)
    //    {
    //        map.MarkHexShootable(target.tile);
    //    }
    //}

    //public void ShowShootableTargetsFrom(Tile fromTile, int altitude, List<Tile> tiles)
    //{
    //    //Debug.Log("Looking for shootable targets");
    //    List<Unit> targets = GetShootableTargetsFrom(fromTile, altitude, tiles);

    //    foreach (Unit target in targets)
    //    {
    //        map.MarkHexShootable(target.tile);
    //    }
    //}

    //public void ShowVisibilityTheseTiles(List<Tile> tiles)
    //{
    //    map.ResetUnitVision();
    //    map.MarkHexesUnitVisible(tiles);
    //}
    
    // public List<Unit> GetShootableTargets()
    //{ 
    //    List<Unit> shootable = new List<Unit>();
    //    foreach (Unit unit in visibleEnemyUnits)
    //    {
    //        if (CanShootTarget(unit)) shootable.Add(unit);
    //    }
    //    return shootable;
    //}

    //public List<Unit> GetShootableTargetsFrom(Tile fromTile, int altitude, List<Tile> tiles)
    //{ 
    //    List<Unit> shootable = new List<Unit>();
    //    foreach (Tile tile in tiles) {
    //        if (tile.unit != null && tile.unit.team != team && tile.unit.visible) {
    //            if (CanShootTargetFrom(tile.unit, fromTile, altitude)) shootable.Add(tile.unit);
    //        }
    //    }
    //    return shootable;
    //}

    public int GetMaxWeaponRange()
    {
        if (squadDef.weapons == null) return 0;

        int range = 0;
        foreach (UnitWeapon uWeapon in squadDef.weapons)
        {
            if (uWeapon.weapon.range > range) range = uWeapon.weapon.range;
        }
        return range;
    }

    //public bool moveInAvailableMoves(int[] move)
    //{
    //    //if (availableMoves.Count > 0)
    //    //{
    //        foreach (Tile availableMove in availableMoves)
    //        {
    //            if (move[0] == availableMove.x && move[1] == availableMove.z)
    //            {
    //                return true;
    //            }
    //        }
    //    //}
    //    return false;
    //}

    //public bool moveInAvailableMoves(Tile tile)
    //{
    //    return moveInAvailableMoves( new int[] { tile.x, tile.z } );
    //}

    //public bool moveInLoadOptions(Tile move)
    //{
    //    foreach (Tile cargoOption in loadOptions)
    //    {
    //        if (move.x == cargoOption.x && move.z == cargoOption.z)
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    // Update is called once per frame
    //void Update()
    //{
    //    Map map = (Map)FindObjectOfType<Map>();
    //    Vector3 location = map.getHexLocation(1, 0);

    //    //Debug.Log(location);

    //    MeshFilter meshFilter = GetComponent<MeshFilter>();

    //    location.y = location.y + 2;

    //    //gameObject.position = location;

    //    transform.position = location;

    //}


   

    //public List<Unit> GetShootableTargets(Tile forTile, int altitude = 0)
    //{

    //    List<Unit> targets = new List<Unit>();

    //    Unit[] allUnits = (Unit[])FindObjectsOfType<Unit>();

    //    for (int i = 0; i < allUnits.Length; i++)
    //    {
    //        if (allUnits[i].alive
    //            && allUnits[i].visible
    //            && team != allUnits[i].team
    //        )
    //        {
    //            if (CanShootTarget(allUnits[i], forTile, altitude))
    //            {
    //                targets.Add(allUnits[i]);
    //            }
    //        }
    //    }

    //    return targets;
    //}
    
    public bool CanSee(Tile targetTile)
    {
        //if (map.CanSee(tile, targetTile, altitude)) return true;
        //return false;
        return visibleTiles.Contains(targetTile);
    }
    
    public bool CanSee(Unit targetUnit)
    {
        //if (map.CanSee(tile, targetUnit.tile, altitude, targetUnit.altitude)) return true;
        //return false;
        return visibleEnemyUnits.Contains(targetUnit);
    }
    
    public bool CanShootTarget( Unit targetUnit, bool showBlocking = false )
    {
        if (squadDef.weapons == null) return false;

        //Debug.Log("show blocking please");
        //if ( map.CanSee(this, targetUnit, showBlocking) )
        if ( CanSee(targetUnit) )
        {
            //double range = map.GetRealDistanceBetweenHexes(tile, targetUnit.tile);
            int gameRange = map.GetGameRange(platoon.tile, targetUnit.tile);
    
            for (int i = 0; i < squadDef.weapons.Length; i++) {
    
                UnitWeapon uWeapon = squadDef.weapons[i];
    
                if (CanShootTargetWithWeaponAtRange(targetUnit, gameRange, uWeapon)) return true;
    
            }
    
        }
        else
       {
            //Debug.Log("No LOS");
        }
        return false;
    }

    public bool CanShootTargetFrom( Unit targetUnit, Tile fromTile, int fromAltitude, bool showBlocking = false )
    {
     
        //double range = map.GetRealDistanceBetweenHexes(tile, targetUnit.tile);
        int gameRange = map.GetGameRange(fromTile, targetUnit.tile);
    
        for (int i = 0; i < squadDef.weapons.Length; i++) {
    
            UnitWeapon uWeapon = squadDef.weapons[i];
    
            if (CanShootTargetWithWeaponAtRange(targetUnit, gameRange, uWeapon)) return true;
    
        }

        return false;
    }


    // old
    //public bool CanShootTarget(Unit targetUnit, Tile fromTile, int altitiude = 0, bool showBlocking = false)
    //{
    //    if (map.CanSee(fromTile, targetUnit.tile, altitude, targetUnit.altitude, showBlocking))
    //    {
    //        //Debug.Log("<color=yellow>Can See</color>");
    //        //double range = map.GetRealDistanceBetweenHexes(tile, targetUnit.tile);
    //        int gameRange = map.GetGameRange(fromTile, targetUnit.tile);

    //        for (int i = 0; i < squadDef.weapons.Length; i++)
    //        {

     //           UnitWeapon uWeapon = squadDef.weapons[i];

     //           if (CanShootTargetWithWeaponAtRange(targetUnit, gameRange, uWeapon))
    //            {
     //               //Debug.Log("<color=green>Can Shoot</color>");
     //               return true;
     //           }


     //       }

     //   }
     //   else
    //    {
            //Debug.Log("No LOS");
    //    }
    //    return false;
    //}
    
    /// Old
    public bool CanShootTargetWithWeapon(Unit targetUnit, int weaponId, bool showBlocking = false)
    {
        //if (map.CanSee(this, targetUnit, showBlocking))
        if ( CanSee(targetUnit) )
        {
            double range = map.GetRealDistanceBetweenHexes(platoon.tile, targetUnit.tile);
            int gameRange = map.GetGameRange(platoon.tile, targetUnit.tile);

            UnitWeapon uWeapon = squadDef.weapons[weaponId];

            if (CanShootTargetWithWeaponAtRange(targetUnit, gameRange, uWeapon)) return true;

        }
        else
        {

        }

        return false;
    }

    private double PortionMovementUsed() {
        return platoon.PortionMovementUsed();
    }

    private bool CanShootTargetWithWeaponAtRange(Unit targetUnit, int gameRange, UnitWeapon uWeapon)
    {
        if (uWeapon.ammo >= 1)
        {
            int weaponRange = uWeapon.weapon.range;

            if (weaponRange >= gameRange)
            {
                if (uWeapon.weapon.damageProfiles.ContainsKey( targetUnit.UnitTargetType() ))
                {
                    double expDamage = uWeapon.weapon.damageProfiles[targetUnit.UnitTargetType()].GetAverageDamage(targetUnit, uWeapon, gameRange, targetUnit.tile.terrain, targetUnit.Countermeasures(), PortionMovementUsed());

                    if (expDamage > 0.0) return true;
                } else
                {
                    //Debug.Log("Can't shoot with weapon " + uWeapon.weapon.name);
                    //Debug.Log("No damage profile (weapon cannot be used on this target");
                }
            } else
            {
                //Debug.Log("Can't shoot with weapon " + uWeapon.weapon.name);
                //Debug.Log("Out of range - " + gameRange + " vs " + weaponRange);
            }
        }
        else
        {
            //Debug.Log("Can't shoot with weapon " + uWeapon.weapon.name);
            //Debug.Log("No Ammo");
        }
        return false;
    }

    public bool Moved()
    {
        return (platoon.actionPoints == 0);
    }

    public bool Moving()
    {
        return (platoon.actionPoints == 0);
    }

    public Tile Tile()
    {
        return platoon.tile;
    }

    private Vector3 GetRandomSquadDisplacement()
    {
        double unitDiscRadius = 0.3 * 200;
        double randomAngle = Lib.random.NextDouble() * 360.0;
        double randomDistance = Math.Pow(Lib.random.NextDouble(), 0.5) * unitDiscRadius;

        //Vector3 disp = new Vector3(0f, 0f, (float)randomDistance);
        Vector3 disp = new Vector3(0f, 0f, (float)unitDiscRadius);

        disp = Quaternion.Euler(0f, (float)randomAngle, 0f) * disp;
        return disp;
    }

    public void SelectAndFireWeapons(Unit targetUnit)
    {
        int range = map.GetGameRange( platoon.Tile(), targetUnit.Tile() );
        List<FiringWeapon> fws = SelectFiringWeapons(targetUnit);

        Vector3 squadFiringPos = GetRandomSquadDisplacement();
        Debug.Log( "squadFiringPos " + squadFiringPos.x.ToString() + "," + squadFiringPos.y.ToString() + "," + squadFiringPos.z.ToString() );

        foreach (FiringWeapon fw in fws)
        {
            FireWeapon(targetUnit, fw, range, squadFiringPos);
        }

    }

    public List<FiringWeapon> SelectFiringWeapons(Unit targetUnit)
    {
        int gameRange = map.GetGameRange( platoon.Tile(), targetUnit.Tile() );

        var weps = new List<FiringWeapon>();

        for (int i = 0; i < squadDef.weapons.Length; i++)
        {
            UnitWeapon uWeapon = squadDef.weapons[i];

            if (CanShootTargetWithWeaponAtRange(targetUnit, gameRange, uWeapon))
            {
                double expDamage = uWeapon.weapon.damageProfiles[targetUnit.UnitTargetType()].GetAverageDamagePerWeapon(targetUnit, uWeapon, gameRange, targetUnit.tile.terrain, targetUnit.Countermeasures(), PortionMovementUsed());
                if (expDamage > 0)
                {
                    expDamage /= (double)uWeapon.weapon.soldiersToFire;
                    weps.Add( new FiringWeapon(uWeapon, expDamage) );
                }
                
            }
        }

        if (weps.Count > 0) {
            List<FiringWeapon> sortedWeps = weps.OrderByDescending(o => o.expDamage).ToList();
            int soldiersRemaining = squadDef.soldiers;

            int i = 0;
            foreach (FiringWeapon wep in sortedWeps)
            {
                sortedWeps[i].number = Math.Min(wep.uWeapon.number, soldiersRemaining);
                soldiersRemaining -= sortedWeps[i].number;
                i++;
            }

            return sortedWeps;
        }

        return new List<FiringWeapon>();
    }

    public List<FiringWeapon> SelectFiringWeaponsForTile(Tile targetTile)
    {
        int gameRange = map.GetGameRange(Tile(), targetTile);

        var weps = new List<FiringWeapon>();

        for (int i = 0; i < squadDef.weapons.Length; i++)
        {
            UnitWeapon uWeapon = squadDef.weapons[i];

            if (uWeapon.weapon.range >= gameRange) {
                weps.Add( new FiringWeapon(uWeapon, 0.1) );
                break;
            }
        }

        if (weps.Count > 0) {
            //List<FiringWeapon> sortedWeps = weps.OrderByDescending(o => o.expDamage).ToList();
            //int soldiersRemaining = squadDef.soldiers;

            //int i = 0;
            //foreach (FiringWeapon wep in sortedWeps)
            //{
            //    sortedWeps[i].number = Math.Min(wep.uWeapon.number, soldiersRemaining);
            //    soldiersRemaining -= sortedWeps[i].number;
            //    i++;
            //}

            //return sortedWeps;

            return weps;
        }

        return new List<FiringWeapon>();
    }

    public int BestUsableWeaponAgainstTarget(Unit targetUnit)
    {
        int bestWeaponId = -1;
        double bestAverageDamage = 0;
        
        int gameRange = map.GetGameRange(Tile(), targetUnit.tile);

        for (int i = 0; i < squadDef.weapons.Length; i++)
        {

            UnitWeapon uWeapon = squadDef.weapons[i];

            if (CanShootTargetWithWeaponAtRange(targetUnit, gameRange, uWeapon))
            {
                double expDamage = uWeapon.weapon.damageProfiles[targetUnit.UnitTargetType()].GetAverageDamage(targetUnit, uWeapon, gameRange, targetUnit.tile.terrain, targetUnit.Countermeasures(), PortionMovementUsed());

                if (expDamage > bestAverageDamage)
                {
                    bestAverageDamage = expDamage;
                    bestWeaponId = i;
                }
            }

        }

        return bestWeaponId;
    }

    //public void ConsumeActionPoint()
    //{
    //    actionPoints = Math.Max(0, actionPoints - 1);
    //    if (actionPoints <= 0 && team == 1)
    //    {
    //        if ( ! hasFired)
    //            unitUi.SetHasPartActions();
    //        else
    //            unitUi.SetHasNoActions();
    //    }
    //}

    public bool CanFire()
    {
        return (! hasFired);
    }
    public void Fired()
    {
        hasFired = true;
        if (team == 1) unitUi.SetHasNoActions();
    }

    public void FireWeapon(Unit targetUnit, FiringWeapon fWeapon, int range, Vector3 squadPosition)
    {
        UnitWeapon uWeapon = fWeapon.uWeapon;

        DamageProfile damageProfile = uWeapon.weapon.damageProfiles[targetUnit.UnitTargetType()];

        damageProfile.DoHits( uWeapon, targetUnit, range, platoon.PortionMovementUsed() );
        
        //double unitHeight = 2.0;

        Vector3 fromPos = Tile().position;
        fromPos.y += ((float)unitHeight + (float)altitude) * global.map.YMultiplier();

        fromPos += squadPosition;

        Debug.Log("<color=green>FireWeapon fromPos: " + fromPos.ToString() + "</color>");

        Vector3 toPos = targetUnit.Tile().position;
        toPos.y += ((float)unitHeight + (float)targetUnit.altitude) * global.map.YMultiplier();


        if (uWeapon.weapon.shootProfile.repetitions >= 1)
        {
            global.shootManager.PlayShootEffect(platoon.GetComponent<AudioSource>(), uWeapon.weapon.shootProfile, fWeapon.number, fromPos, toPos);
        }
        
        ConsumeAmmo(uWeapon);
    }

    public void FireWeaponAtTile(Tile targetTile, FiringWeapon fWeapon, int range)
    {
        UnitWeapon uWeapon = fWeapon.uWeapon;

        //DamageProfile damageProfile = uWeapon.weapon.damageProfiles[targetUnit.squadDef.targetType];

        //damageProfile.DoHits( uWeapon, targetUnit, range, platoon.PortionMovementUsed() );
        
        //double unitHeight = 2.0;

        Vector3 fromPos = Tile().position;
        fromPos.y += ((float)unitHeight + (float)altitude) * global.map.YMultiplier();

        Vector3 toPos = targetTile.position;
        toPos.y += ((float)unitHeight) * global.map.YMultiplier();


        if (uWeapon.weapon.shootProfile.repetitions >= 1)
        {
            global.shootManager.PlayShootEffect(platoon.GetComponent<AudioSource>(), uWeapon.weapon.shootProfile, fWeapon.number, fromPos, toPos);
        }
        
        ConsumeAmmo(uWeapon);
    }

    public void Shoot(Unit targetUnit)
    {

        SelectAndFireWeapons(targetUnit);

        ////Debug.Log("Shoot - weaponid " + weaponId);
        //if (CanShootTarget(targetUnit))
        //{
        //    //

        //    int range = map.GetGameRange(Tile(), targetUnit.tile);
        //    bool moved = (actionPoints == 0);

        //    List<FiringWeapon> weapons = SelectFiringWeapons(targetUnit);

        //    foreach (FiringWeapon fWeapon in weapons)
        //    {
        //        FireWeapon(targetUnit, fWeapon, range, moved);
        //    }

        //    //ConsumeActionPoint();
        //    Fired();

        //}

    }

    public void Shoot(Unit targetUnit, int weaponId)
    {
        //Debug.Log("Shoot - weaponid " + weaponId);
        if ( CanShootTargetWithWeapon(targetUnit, weaponId) )
        {
            //
            UnitWeapon uWeapon = squadDef.weapons[weaponId];

            if ( uWeapon.weapon.damageProfiles.ContainsKey( targetUnit.UnitTargetType() ) )
            {


                int range = map.GetGameRange(Tile(), targetUnit.tile);
                DamageProfile damageProfile = uWeapon.weapon.damageProfiles[targetUnit.UnitTargetType()];

                //Debug.Log("Shooting. ap: " + actionPoints);
                bool moved = (actionPoints == 0);

               //Debug.Log("Shooting - moved? " + moved);

                damageProfile.DoHits( uWeapon, targetUnit, range, platoon.PortionMovementUsed() );

                //ui.ShowTargetingCrosshair(targetUnit.gameObject);

                

                //global.soundManager.PlaySound(GetComponent<AudioSource>(), global.soundManager.rifleShot, 20, 230.0, 0.5);


                //global.soundManager.PlaySound(GetComponent<AudioSource>(), uWeapon.weapon.soundProfile.clip, 12, 250.0, 0.01);

                //global.soundManager.PlaySound(GetComponent<AudioSource>(), uWeapon.weapon.soundProfile);
                
                //double unitHeight = 2.0;

                Vector3 fromPos = Tile().position;
                fromPos.y += ((float)unitHeight + (float)altitude) * global.map.YMultiplier();

                Vector3 toPos = targetUnit.tile.position;
                toPos.y += ((float)unitHeight + (float)targetUnit.altitude) * global.map.YMultiplier();

                if (uWeapon.weapon.shootProfile.repetitions >= 1)
                {
                    global.shootManager.PlayShootEffect(platoon.GetComponent<AudioSource>(), uWeapon.weapon.shootProfile, uWeapon.number, fromPos, toPos);
                }
                else
                {
                    Debug.Log("<color=red>Warning: No shoot profile found for this weapon: "+uWeapon.weapon.name+"</color>");
                }


                //ConsumeActionPoint();//Debug.Log("Consuming action point on " + squadDef.name + " for shooting");
                Fired();
                ConsumeAmmo(weaponId);

            }
        }
    }

    public void ShootAtTile(Tile targetTile)
    {
        //Debug.Log("Shoot - weaponid " + weaponId);
        //if (CanShootTarget(targetUnit)) {
        if (visibleTiles.Contains(targetTile)) {
            //

            int range = map.GetGameRange(Tile(), targetTile);
            //bool moved = (actionPoints == 0);

            List<FiringWeapon> weapons = SelectFiringWeaponsForTile(targetTile);

            foreach (FiringWeapon fWeapon in weapons)
            {
                FireWeaponAtTile(targetTile, fWeapon, range);
            }

            //ConsumeActionPoint();
            //Fired();

        }
    }

    public void ShootWithBestWeapon(Unit targetUnit)
    {
        int weaponId = BestUsableWeaponAgainstTarget(targetUnit);

        if (weaponId >= 0)
        {
            Shoot(targetUnit, weaponId);
        }
        else
        {
            Debug.Log("No weapons available to shoot target");
        }
    }

    public void PlayerShoot(Unit targetUnit, int weaponId)
    {
        if (!hasFired)
        {
            Shoot(targetUnit, weaponId);
            //UpdateUnitActionDisplays();
            //PlayerMoveEnd();
        }
    }

    public void PlayerShootWithBestWeapon(Unit targetUnit)
    {
        if (!hasFired)
        {
            int weaponId = BestUsableWeaponAgainstTarget(targetUnit);

            if (weaponId >= 0)
            {
                PlayerShoot(targetUnit, weaponId);
            }
            else
            {
                Debug.Log("No weapons available to shoot target");
            }
        }
    }
    
    public void PlayerShootAutoWeapons(Unit targetUnit)
    {
        if ( ! hasFired)
        {
            Shoot(targetUnit);
        }
    }

    public void PlayerShootAutoWeaponsAtTile(Tile targetTile)
    {
        if ( ! hasFired)
        {
            ShootAtTile(targetTile);
        }
    }

    public void AiShoot(Unit targetUnit, int weaponId)
    {
        Shoot(targetUnit, weaponId);
    }

    public void AiShootWithBestWeapon(Unit targetUnit)
    {
        int weaponId = BestUsableWeaponAgainstTarget(targetUnit);

        Shoot(targetUnit, weaponId);
    }
     
    public List<UnitWeapon> Weapons() {
        return squadDef.weapons.ToList();
    }

    public int Ammo(int weaponId)
    {
        return squadDef.weapons[weaponId].ammo;
    }

    public bool HasAmmo(int weaponId, int ammoRequired = 1)
    {
        return squadDef.weapons[weaponId].ammo >= ammoRequired;
    }

    public void ConsumeAmmo(int weaponId, int ammoUsed = 1)
    {
        //Debug.Log("consume ammo");
        //Debug.Log(weaponId);
        squadDef.weapons[weaponId].ammo = Math.Max(0, squadDef.weapons[weaponId].ammo - ammoUsed);
        //Debug.Log(squadDef.weapons[weaponId].ammo);
    }

    public void ConsumeAmmo(UnitWeapon uWeapon, int ammoUsed = 1)
    {
        //Debug.Log("consume ammo");
        //Debug.Log(weaponId);
        uWeapon.ammo = Math.Max(0, uWeapon.ammo - ammoUsed);
        //Debug.Log(squadDef.weapons[weaponId].ammo);
    }

    //public void AiShootWithBestWeapon(Unit targetUnit)
    //{
    //    StartCoroutine(DoTurnCoroutine(targetUnit));
    //}

    //IEnumerator AiShootWithBestWeaponCoRoutine(Unit targetUnit)
    //{
    //    int weaponId = BestUsableWeaponAgainstTarget(targetUnit);
    //    ui.ShowTargetingCrosshair(targetUnit.gameObject);
    //    yield return new WaitForSeconds(0.4f);

    //    Shoot(targetUnit, weaponId);
    //    ui.HideTargetingCrosshair(targetUnit.gameObject);
    //}

    public void TakeDamage(int damage)
    {
        health = Math.Max(0, health - damage);
        //Debug.Log("health: " + health.ToString());

        DamageCargo(damage);

        if (health <= 0) Die();
    }

    public void DamageCargo(int damage)
    {

        if (cargo.HasCargo())
        {
            foreach (Unit unit in cargo.containedUnits)
            {
                for ( int i = 1; i <= damage * 2; i++)
                {
                    if (Lib.random.NextDouble() < 0.3) unit.TakeDamage(1);
                }
            }
        }

    }

    //void RemoveSeenByThisUnit()
    //{
    //    foreach (Unit enemyUnit in visibleEnemyUnits)
    //    {
    //        enemyUnit.LostSightOfBy(this);
    //    }
    //    visibleEnemyUnits = new List<Unit>();
    //}

    public void Die()
    {
        Debug.Log("<color=green>Die!</color>");
        //RemoveSeenByThisUnit();
        alive = false;
        //Tile().unit = null;
        //gameObject.GetComponent<MeshCollider>().enabled = false;

        if (cargo.HasCargo())
        {
            int len = cargo.containedUnits.Count;
            for (int i = 0; i < len; i++)
            {
                Unit dismountSquad = cargo.containedUnits[0];
                cargo.UnloadUnit(dismountSquad);
                unitUi.SetCargoText("");
                dismountSquad.BailOut(Tile());
            }
        }

        //gameObject.GetComponent<Renderer>().enabled = false;
        //Hide();
    }

    // IsShootableTargetAtHex()

    // FindAllShootableTargets()

    // Fire()

    // GetsShot()
    
    //Vector3 namePos = Camera.main.WorldToScrenPoint(this.transform.position);
    //nameLabel.transform.position = namePos;
   

    public void Fortify()
    {
        if (Tile().fort == null)
        {
            Forts.MakeFort(FortDefs.forts[squadDef.fortType], Tile());

            //GameObject fortDisc = Instantiate(fortDiscObj);
            //Fort fort = (Fort)fortDisc.GetComponent(typeof(Fort));
            //fort.Setup(FortDefs.forts[squadDef.fortType], tile);
            //tile.fort = fort;
            // //tile.fort = new Fort(FortDefs.forts[squadDef.fortType]);

            buildFortAtTurnEnd = true;
            //ConsumeActionPoint();//Debug.Log("Consuming action point on " + squadDef.name + " for fortifying");
        }
    }

    public bool HasSetupWeapon()
    {
        if (squadDef.weapons == null) return false;

        foreach (UnitWeapon uWeapon in squadDef.weapons)
        {
            if (uWeapon.weapon.setupTime > 0)
            {
                return true;
            }
            if (uWeapon.weapon.indirectWeapon != null && uWeapon.weapon.indirectWeapon.setupTime > 0)
            {
                return true;
            }
        }
        return false;
    }

    public UnitWeapon GetSetupWeapon()
    {
        foreach (UnitWeapon uWeapon in squadDef.weapons)
        {
            if (uWeapon.weapon.setupTime > 0)
            {
                return uWeapon;
            }
            if (uWeapon.weapon.indirectWeapon != null && uWeapon.weapon.indirectWeapon.setupTime > 0)
            {
                return uWeapon;
            }
        }
        return null;
    }

    public void Setup()
    {
        if (HasSetupWeapon())
        {
            if ( isPackedUp && ! isSettingUp )
            {
                setupUWeapon = GetSetupWeapon();
                isSettingUp = true;
                setupProgress = 1;
                //ConsumeActionPoint();//Debug.Log("Consuming action point on " + squadDef.name + " for setting up");
                global.ui.HideSetupButton();
            }
        }
    }

    public void ProgressSetup()
    {
        //Debug.Log("PS");
        if (setupProgress >= setupUWeapon.weapon.setupTime)
        {
            CompleteSetup();
        }
        else
        {
            setupProgress += 1;
            //ConsumeActionPoint();//Debug.Log("Consuming action point on " + squadDef.name + " for continuing to set up");
            //Debug.Log("  " + setupProgress);
        }
        
    }

    public void CompleteSetup()
    {
        //Debug.Log("Completing setup");
        isSettingUp = false;
        isPackedUp = false;
        isSetUp = true;
    }

    public void Packup()
    {
        //Debug.Log(isSetUp);
        //Debug.Log(isPackedUp);
        //Debug.Log(isSettingUp);
        //Debug.Log(isPackingUp);
        if (HasSetupWeapon())
        {
            if (isSetUp && !isPackingUp)
            {
                setupUWeapon = GetSetupWeapon();
                isPackingUp = true;
                packupProgress = 1;
                //ConsumeActionPoint(); //Debug.Log("Consuming action point on " + squadDef.name + " for packing up");
                global.ui.HidePackupButton();
                global.ui.HideArtilleryButton();
                global.artillery.EndArtilleryTargetingMode();
            }
        }
    }

    public void ProgressPackup()
    {
        if (packupProgress >= setupUWeapon.weapon.packupTime)
        {
            CompletePackup();
        }
        else
        {
            packupProgress += 1;
            //Debug.Log(packupProgress);
            //ConsumeActionPoint(); //Debug.Log("Consuming action point on " + squadDef.name + " for continuing pack up");
        }
    }

    public void CompletePackup()
    {
        isPackingUp = false;
        isSetUp = false;
        isPackedUp = true;

    }

    public void CheckSetupPackup()
    {
        //Debug.Log("Processing Setup and Packup");
        if (isSettingUp)
        {
            ProgressSetup();
        }
        if (isPackingUp)
        {
            ProgressPackup();
        }
    }

    public bool HasIndirectWeapon()
    {
        if (squadDef.weapons == null) return false;

        foreach (UnitWeapon uWeapon in squadDef.weapons)
        {
            if (uWeapon.weapon.indirect)
            {
                return true;
            }
        }
        return false;
    }

    public UnitWeapon GetIndirectWeapon()
    {
        foreach (UnitWeapon uWeapon in squadDef.weapons)
        {
            if (uWeapon.weapon.indirect)
            {
                return uWeapon;
            }
        }
        return null;
    }

    //public void ArtilleryAttack(UnitWeapon uWeapon, ArtilleryTarget target, int shots)
    //{
    //    //global.shootManager.PlayShootEffect(GetComponent<AudioSource>(), uWeapon.weapon.soundProfile, tile, target.tile);

    //    //if (target.type == ArtilleryTargetType.Point) Debug.Log("Point mission");
    //    //if (target.type == ArtilleryTargetType.Circle) Debug.Log("Circle mission");
    //    //if (target.type == ArtilleryTargetType.Line) Debug.Log("Line mission");
        
    //    global.artilleryManager.AddMission(new ArtilleryMission(this, uWeapon, target, global.turnManager.turn, uWeapon.weapon.indirectWeapon.aimTime, uWeapon.weapon.indirectWeapon.flightTime, shots));

    //    ConsumeActionPoint();//Debug.Log("Consuming action point on " + squadDef.name + " for arty start");
    //}
    //public void PlayerArtilleryAttack(UnitWeapon uWeapon, ArtilleryTarget target, int shots)
    //{
    //    ArtilleryAttack(uWeapon, target, shots);
    //}

    //public int GetRemainingMovesAfterAltChange(int altChange)
    //{
    //    return squadDef.moves - Math.Max(0,altChange);
    //}

    public static void ProcessUnitsSetupPackup(int team)
    {
        //Debug.Log("Process all setup packup");
        List<Unit> units = Store.GetAliveTeamUnits(team);
        foreach (Unit unit in units)
        {
            unit.CheckSetupPackup();
        }
    }

    public void SkipTurn() {
        actionPoints = 0;
    }
}
