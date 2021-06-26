using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public partial class Unit : MonoBehaviour
{

    GameObject _3dModel = null;

    public List<Squad> squads;

    // refs
    public GameObject moveTarget;
    public GameObject cargoTarget;
    public List<GameObject> moveTargets;
    public List<GameObject> cargoTargets;
    //public GameObject fortDiscObj;
    public Material[] materials;
    public Map map;
    private UI ui;
    private Global global;

    // mechanics
    private bool started = false;
    public List<Tile> visibleTiles;
    public List<Unit> visibleEnemyUnits;
    public List<Unit> enemiesWithVisionOnThis;
    public List<Move> availableMoves;
    public List<Move> loadOptions;

    // unit defs
    //public UnitType unitType;
    //public SquadDef squadDef;
    
    public PlatoonGroupDef platoonGroupDef;
    public List<PlatoonDef> platoonDefs;
    public List<SquadDef> squadDefs;
    public List<SquadDef> coreSquadDefs;
    public List<SquadDef> attachedSquadDefs;
    public bool hasCore;
    public bool hasAttachments;

    // game vars
    public int playerTeam = 1;
    public int team;
    public int group;
    public Color groupColor;
    //public int x;
    //public int z;
    public Tile tile;
    public int health = 5;
    public int actionPointsMax = 1;
    public int actionPoints = 1;
    public double moveCostUsed = 0.0;
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
    private TargetType targetType;
    private double hitability;
    private int armor;
    private Countermeasures countermeasures;
    public string unitName;
    public string unitShortName;
    public string unitVeryShortName;
    //private int moves;
    //private int minAltitude;
    //private int maxAltitude;

    public FortType fortType;

    //////////////////////////
    /// Initialisation
    //////////////////////////
     
    public static GameObject unitObject;

    public static Unit MakeUnit() {
        return new Unit();
    }

 

    public void RefillActions()
    {
        actionPoints = actionPointsMax;
        moveCostUsed = 0.0;
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

    public bool BelongsToPlayer()
    {
        return team == 1;
    }

    public void PlayerMoveTo(int X, int Z, int altitude = 0)
    {
        PlayerMoveTo(map.GetTileFromXZ(X, Z));
    }
    public void PlayerMoveTo(Move move, int altitude = 0)
    {
        moveCostUsed = move.cost;
        PlayerMoveTo(move.tile);
    }
    public void PlayerMoveTo(Action action, int altitude = 0)
    {
        moveCostUsed = action.cost;
        PlayerMoveTo(action.tile);
    }
    public void PlayerMoveTo(Tile to, int altitude = 0)
    {
        //map.MarkHexesTeamVisible(visibleTiles, false);
        MoveTo(to, altitude);
        UpdateUnitActionDisplays();
        PlayerMoveEnd();
    }

    public void AiMoveTo(Tile to, int altitude = 0)
    {
        MoveTo(to, altitude);
        RecalculateUnitVisibility(1);
        GetVisibility();
    }

    public void RecalculateUnitVisibility(int toTeam)
    {

        List<Unit> teamUnits = Store.GetAliveTeamUnits(toTeam);
        foreach (Unit teamUnit in teamUnits)
        {

            if (teamUnit.visibleTiles.Contains(tile))
            {
                // can see
                if (!teamUnit.visibleEnemyUnits.Contains(this)) teamUnit.Spotted(this);
            }
            else
            {
                // can't see
                if (teamUnit.visibleEnemyUnits.Contains(this)) teamUnit.LostSightOf(this);
            }

        }
    }

    public void MoveTo(int X, int Z, int altitude = 0)
    {
        MoveTo( map.GetTileFromXZ(X, Z) );
    }

    public void MoveTo(Tile to, int newAltitude = 0)
    {
        map.UnassignUnitFromTile(tile);
        tile = to;
        this.altitude = newAltitude;
        ConsumeActionPoint(); //Debug.Log("Consuming action point on " + squadDef.name + " for moving");
        MoveToCurrentTile();
    }

    void MoveToCurrentTile()
    {
        Vector3 location = map.GetHexLocation(tile.x, tile.z);
        location.y = location.y + (float)0.0;
        location.y = location.y + altitude * map.YMultiplier();
        transform.position = location;

        map.AssignUnitToTile(this, tile);

        unitUi.PositionCanvas();
    }

    public void Show()
    {
        //ShowUnit();
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        if (_3dModel != null) _3dModel.GetComponent<MeshRenderer>().enabled = true;
        ((UnitUI)gameObject.transform.Find("UnitCanvas").GetComponent<UnitUI>()).Show();
        visible = true;
    }
    public void Hide()
    {
        if ( alive && alwaysShowEnemy ) return;

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        if (_3dModel != null) _3dModel.GetComponent<MeshRenderer>().enabled = false;
        ((UnitUI)gameObject.transform.Find("UnitCanvas").GetComponent<UnitUI>()).Hide();
        visible = false;
    }
    private void ShowUnit()
    {
        gameObject.SetActive(true);
    }
    private void HideUnit()
    {
        gameObject.SetActive(false);
    }


    public void PlayerLoadAt(Tile carrierTile)
    {
        carrierUnit = carrierTile.unit;
        map.UnassignUnitFromTile(tile.x, tile.z);
        tile = carrierUnit.tile;
        LoadInto(carrierUnit);

        PlayerMoveEnd();
    }

    public void LoadInto(Unit carrierUnit)
    {
        PutIntoCargoSilent(carrierUnit);
        ConsumeActionPoint();//Debug.Log("Consuming action point on " + squadDef.name + " for loading");
        carrierUnit.ConsumeActionPoint();//Debug.Log("Consuming action point on " + carrierUnit.name + " for receiving cargo");
    }

    public void PutIntoCargoSilent(Unit carrierUnit)
    {
        inCargo = true;
        HideUnit();
        //Debug.Log(carrierUnit);
        carrierUnit.cargo.LoadUnit(this);
        carrierUnit.UpdateCargoText();
    }

    public void UpdateCargoText()
    {
        List<string> cargoUnits = new List<string>();
        if (cargo.containedUnits.Count > 0) {
            foreach (Unit unit in cargo.containedUnits)
            {
                cargoUnits.Add( unit.UnitVeryShortName() );
            }
            if (cargoUnits.Count > 0)
            {
                unitUi.SetCargoText( string.Concat("(", string.Join(", ", cargoUnits), ")") ); 
            } else
            {
                unitUi.SetCargoText("");
            }
        } else {
            unitUi.SetCargoText("");
        }
        
    }

    public void PlayerUnloadTo(Tile to)
    {
        Unit dismountSquad = cargo.containedUnits[0];
        cargo.UnloadUnit(dismountSquad);
        unitUi.SetCargoText("");
        dismountSquad.DebusTo(to);
        unitUi.SetCargoText("");
        PlayerMoveEnd();
    }

    public void PlayerPickupAt(Tile to)
    {
        Unit pickupUnit = to.unit;
        MoveTo(to);
        pickupUnit.LoadInto(this);
        
        PlayerMoveEnd();
    }
    
    public void PlayerDropoff(Action dropoffAction, Action dismountAction)
    {
        bool debugThis = false;

        if (debugThis) Debug.Log("Player drop off");

        if (debugThis) {
            Debug.Log("Transport Steps");
            for (int i = 0; i < dropoffAction.path.Length; i++) {
                Debug.Log( dropoffAction.path[i].ToString() );
            }
            Debug.Log("Dismount Steps");
            for (int i = 0; i < dismountAction.path.Length; i++) {
                Debug.Log( dismountAction.path[i].ToString() );
            }
        }

        // move unless the vehicle does not need to to drop off -- AP should only be consumed if it moves
        // 1st path node = start tile
        // 2nd path node = 1st move
        // etc.

        //if (action.path.Length >= 3) MoveTo(action.path[action.path.Length - 2]);
        if (dropoffAction.tile != tile) MoveTo(dropoffAction.tile);
        
        Unit dismountUnit = cargo.containedUnits[0];
        cargo.UnloadUnit(dismountUnit);
        dismountUnit.DebusTo(dismountAction.tile);
        UpdateCargoText();
        PlayerMoveEnd();
    }

    public void PlayerMoveEnd()
    {
        Detection.ShowHideVisibleEnemies(1);
        map.ShowTeamVision(team);
    }

    public void DebusTo(Tile to)
    {
        //map.UnassignUnitFromTile(tile.x, tile.z);
        ExitCargo(to);
        ConsumeActionPoint();//Debug.Log("Consuming action point on " + squadDef.name + " for exiting");
    }

    public void BailOut(Tile to)
    {
        ExitCargo(to);
        EventText("Bail Out");
    }

    private void ExitCargo(Tile to)
    {
        inCargo = false;
        tile = to;
        carrierUnit = null;
        ShowUnit();
        MoveToCurrentTile();
    }

    //void ShowMoveOptions()
    //{
    //    DestroyAvailableMoves();

    //    if (actionPoints == 0) return;

    //    availableMoves = map.GetMoves( new int[] { tile.x, tile.z }, Moves() );
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

    void DestroyAvailableMoves()
    {
        foreach (GameObject target in moveTargets)
        {
            Destroy(target);
            // GC?
        }
        moveTargets = new List<GameObject>();
        availableMoves = new List<Move>();
    }





    void DestroyLoadOptions()
    {
        loadOptions = new List<Move>();
        DestroyCargoTargets();
    }

    void DestroyCargoTargets()
    {
        foreach (GameObject target in cargoTargets)
        {
            Destroy(target);
            // GC?
        }
        cargoTargets = new List<GameObject>();
    }


    //////////////////////////////////
    /// Actions
    //////////////////////////////////
    
    public bool CanAct()
    {
        return CanDoAction();
    }

    public bool CanDoAction()
    {
        if (actionPoints == 0) return false;
        return true;
    }


    public List<Action> GetActions(int targetAltitude = 0)
    {
        if ( ! CanDoAction() ) return new List<Action>();

        Dictionary<string, Move> moves = GetPossibleMoves();

        //foreach (KeyValuePair<string, Move> kvp in moves)
        //{
        //    Move m = kvp.Value;
        //    Debug.Log("Move cost: " + m.cost.ToString());
        //}

        //Debug.Log(moves.ToString());

        //Dump.Dump(moves);

        //List<Action> moves = (remainingMoves == -1) ? unit.GetMoveActions() : unit.GetMoveActions(remainingMoves, targetAltitude);
        List<Action> moveActions = GetMoveActions(moves);
        List<Action> loadActions = GetLoadActions(moves);

        //List<Action> unloadActions = GetUnloadActions(moves); // disabling this for now
        List<Action> unloadActions = new List<Action>();        // disabling this for now

        List<Action> pickUpActions = GetPickupActions(moves);

        Dictionary<string, Action> tempActions = new Dictionary<string, Action>();

        //MergeInActions(ref tempActions, moves);
        //MergeInActions(ref tempActions, loads);
        //MergeInActions(ref tempActions, unloads);
        //MergeInActions(ref tempActions, pickUps);

        foreach (Action action in moveActions)
        {
            string k = action.tile.x.ToString() + "-" + action.tile.z.ToString();
            tempActions[k] = action;
        }
        foreach (Action action in loadActions)
        {
            string k = action.tile.x.ToString() + "-" + action.tile.z.ToString();
            if (tempActions.ContainsKey(k))
            {
                tempActions[k] = MergeActions(tempActions[k], action);
            }
            else
            {
                tempActions[k] = action;
            }
        }
        foreach (Action action in unloadActions)
        {
            string k = action.tile.x.ToString() + "-" + action.tile.z.ToString();
            if (tempActions.ContainsKey(k))
            {
                tempActions[k] = MergeActions(tempActions[k], action);
            }
            else
            {
                tempActions[k] = action;
            }
        }
        foreach (Action action in pickUpActions)
        {
            string k = action.tile.x.ToString() + "-" + action.tile.z.ToString();
            if (tempActions.ContainsKey(k))
            {
                tempActions[k] = MergeActions(tempActions[k], action);
            }
            else
            {
                tempActions[k] = action;
            }
        }

        List<Action> actions = new List<Action>();
        foreach (KeyValuePair<string, Action> kv in tempActions)
        {
            actions.Add(kv.Value);
        }

        return actions;
    }

    public Dictionary<string, Move> GetPossibleMoves(int targetAltitude = 0) {
        MovementData md = MovementData();
        //Debug.Log("md");
        //Debug.Log(md.ToString());
        
        return global.pathfinder.FindAllMoves(tile, MovementData());
    }

    public Action MergeActions(Action action, Action newAction)
    {
        if (action.type == ActionType.Combo)
        {
            ((ComboAction)action).children.Add(newAction);
            return action;
        }
        else
        {
            Action comboAction = new ComboAction(action.unit, action.tile, new List<Action> { action, newAction });
            return comboAction;
        }
    }

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

    public List<Action> GetMoveActions()
    {
        if ( MaxAltitude() > 0 )
        {
            return GetArialMoveActions();
        }
        else
        {
            return GetGroundMoveActions();
        }
    }

    public List<Action> GetMoveActions(Dictionary<string, Move> moves)
    {
        if ( MaxAltitude() > 0 )
        {
            return GetArialMoveActions(moves);
        }
        else
        {
            return GetGroundMoveActions(moves);
        }
    }

    public List<Action> GetMoveActions(int limitedMoves, int altitude)
    {
        return GetMoveActions(); ///////// Placeholder

        //if (actionPoints == 0) return new List<Action>();

        //List<Action> actions = new List<Action>();
        //List<Tile> options = GetMoves(limitedMoves);
        //foreach (Tile tile in options)
        //{
        //    actions.Add(new Action(this, ActionType.Move, tile));
        //}
        //return actions;
    }


    public List<Action> GetGroundMoveActions()
    {

        if (actionPoints == 0) return new List<Action>();

        List<Action> actions = new List<Action>();

        Dictionary<string, Move> moves = global.pathfinder.FindAllMoves(tile, MovementData());
        foreach (var moveKvp in moves)
        {
            string key = moveKvp.Key;
            Move move = moveKvp.Value;

            Tile[] path = new Tile[move.steps.Count];
            for (int i = 0; i < move.steps.Count; i++) {
                path[i] = move.steps[i].tile;
            }

            if (move.tile.unit == null)
                actions.Add(new Action(this, ActionType.Move, move.tile, path, move.cost));
        }

        return actions;
    }

    public List<Action> GetGroundMoveActions(Dictionary<string, Move> moves)
    {

        List<Action> actions = new List<Action>();
        
        foreach (var moveKvp in moves)
        {
            string key = moveKvp.Key;
            Move move = moveKvp.Value;

            // skip if there is a unit on that tile
            if (move.tile.unit != null) continue;

            Tile[] path = new Tile[move.steps.Count];
            for (int i = 0; i < move.steps.Count; i++) {
                path[i] = move.steps[i].tile;
            }

            actions.Add(new Action(this, ActionType.Move, move.tile, path, move.cost));
        }

        return actions;
    }
    
    public List<Action> GetArialMoveActions()
    {
        if (actionPoints == 0) return new List<Action>();

        int currentHeight = tile.y + altitude;

        double adjustedAltGainCost = squadDefs[0].gainAltCost / map.baseHeightMultiplier;
        double adjustedLoseAltPayment = squadDefs[0].loseAltPayment / map.baseHeightMultiplier;

        int maxPossibleHeight = currentHeight + (int)Math.Floor((double)Moves() / adjustedAltGainCost);
        //Debug.Log("max poss height" + maxPossibleHeight);

        List<Action> actions = new List<Action>();
        List<ArialMove> options = GetArialMoves();
        foreach (ArialMove arialMove in options)
        {
            int dist = map.GetMoveDistance(tile, arialMove.tile);
            int minTileAlt = map.GetMinTileAlt(arialMove.tile);
            if (map.CanLandOnTile(arialMove.tile))
            {
                minTileAlt = 0;
            }
            int minHeight = arialMove.tile.y + minTileAlt;
            minHeight = Math.Max(currentHeight - Moves(), minHeight);

            int maxHeight = 0;
            if (dist <= Moves() )
            {
                maxHeight = currentHeight + (int)Math.Floor((double)(Moves() - dist) / adjustedAltGainCost);
            } else
            {
                maxHeight = currentHeight - (int)Math.Ceiling((double)(dist - Moves()) / adjustedLoseAltPayment);
            }
            maxHeight = Math.Min(maxHeight, squadDefs[0].maxAltitude);

            List<int> altitudeOptions = new List<int>();

            //Debug.Log("min " + minHeight);
            //Debug.Log("max " + maxHeight);

            if (minHeight <= maxHeight)
            {
                for (int height = minHeight; height <= maxHeight; height++)
                {
                    if (height == minHeight || height == maxHeight || height % 5 == 0 ) {
                        int alt = height - arialMove.tile.y;
                        altitudeOptions.Add(alt);
                    }
                    //Debug.Log(alt);
                }

                actions.Add(new ArialMoveAction(this, arialMove.tile, altitudeOptions));

            } else
            {
                /// Is this to highlight that there was an error??
                altitudeOptions.Add(999);
                actions.Add(new ArialMoveAction(this, arialMove.tile, altitudeOptions));
            }

        }
        return actions;
    }
    
    public List<Action> GetArialMoveActions(Dictionary<string, Move> moves) {
        Debug.Log("Arial Moves Placeholder - not written yet");
        return new List<Action>();
    }

    public List<ArialMove> GetArialMoves()
    {
        if (actionPoints == 0) return new List<ArialMove>();

        List<ArialMove> moves = map.GetArialMoves(tile, Moves(), altitude, squadDefs[0].gainAltCost, squadDefs[0].loseAltPayment);

        //List<Tile> ret = new List<Tile>();
        //foreach (int[] move in moves)
       // {
        //    ret.Add(map.GetTileFromXZ(move[0], move[1]));
        //}
        //return ret;

        //Debug.Log("Arial Moves");
        return moves;
    }
    
    public List<Move> GetAvailableLoadOptions() {
        Dictionary<string, Move> moves = GetPossibleMoves();
        return GetAvailableLoadOptions(moves);
    }
    public List<Move> GetAvailableLoadOptions(Dictionary<string, Move> moves)
    {
        DestroyLoadOptions();

        // return the empty list if not a transportable type
        //if (squadDef.targetType != TargetType.Infantry) return loadOptions;

        //List<Tile> tiles = map.GetTilesInMoveRange(tile, squadDef.moves);
        //foreach (Tile tile in tiles)
        foreach (var kvp in moves) {
            Move move = kvp.Value;
            if (move.tile.unit != null)
            {
                if (move.tile.unit.team == team)
                {
                    if (move.tile.unit.altitude == 0)
                    {
                        if (move.tile.unit.cargo.CanLoad(this)) {

                            //Debug.Log("Found a transport we can get into");
                            loadOptions.Add(move);
                            
                        }
                    }
                }
            }
        }

        return loadOptions;
    }

    public void ShowAvailableLoadOptions()
    {

        DestroyLoadOptions();

        if (actionPoints == 0) return;

        GetAvailableLoadOptions();

        cargoTargets = new List<GameObject>(); // clear cargo targets
        foreach (Move move in loadOptions)
        {
            GameObject target = Instantiate(cargoTarget);

            //Debug.Log("getting loc");
            //Debug.Log(tile.x.ToString() + " " + tile.z.ToString());

            Vector3 location = map.GetHexLocation(move.tile.x, move.tile.z);
            location.y = location.y + (float)0.2;
            target.transform.position = location;
            cargoTargets.Add(target);
        }
    }




    public MovementData MovementData()
    {
        return squadDefs[0].movementData;
    }

    /*public List<Action> GetDropoffActions() {
        List<Action> tempActions = GetMoveActions();
        for (int i = 0; i < tempActions.Count; i++) {
            tempActions[i] = new Action(tempActions[i].unit, ActionType.Dropoff, tempActions[i].tile, tempActions[i].path, tempActions[i].cost);
        }
        return tempActions;
    }*/
    
   public Tile[] PathArryFromSteps(List<Step> steps)
   {
        Tile[] path = new Tile[steps.Count];
        for (int i = 0; i < steps.Count; i++) {
            path[i] = steps[i].tile;
        }
        return path;
   }

    //public List<Tile> GetDropoffSpots() {
    //    List<Action> tempActions = GetMoveActions();
    //    for (int i = 0; i < tempActions.Count; i++) {
    //        tempActions[i] = new Action(tempActions[i].unit, ActionType.Dropoff, tempActions[i].tile, tempActions[i].path, tempActions[i].cost);
    //    }
    //    return tempActions;
    //}

    public List<Action> GetDropoffActions() {
        
        // guards
        if (actionPoints == 0) return new List<Action>();
        if (cargo.containedUnits.Count == 0) return new List<Action>();

        List<Action> actions = new List<Action>();

        List<DropoffMove> dropoffMoves = global.pathfinder.FindAllDropoffsCombined(tile, MovementData(), cargo.containedUnits[0].MovementData());

        foreach (var dropoffMove in dropoffMoves)
        {
            //string key = dropoffMoveKvp.Key;
            //Move transportMove = moveKvp.Value[0];
            //Move passengerMove = moveKvp.Value[1];

           // guard against units on dropoff point or dest tile
            if (dropoffMove.tile.unit != null) continue;

            // check that there is space to unload at one of the dismount locations
            foreach (var dismountMove in dropoffMove.passengerMoves)
            {
                if (dismountMove.tile == tile || dismountMove.tile.unit == null)
                {
                    goto OK;
                }
            }
            // if we didn't got to OK, skip to next action
            continue;

            OK:
            
            //Debug.Log("Transport Path count " + transportMove.steps.Count.ToString() );
            //Debug.Log("Passenger Path count " + passengerMove.steps.Count.ToString() );
            //Debug.Log("Path full count " + (transportMove.steps.Count + passengerMove.steps.Count).ToString() );
            //Tile[] path = new Tile[move.steps.Count];
            
            Tile[] path = new Tile[dropoffMove.steps.Count];
            for (int i = 0; i < dropoffMove.steps.Count; i++) {
                path[i] = dropoffMove.steps[i].tile;
            }


            actions.Add(new DropoffAction(this, dropoffMove.tile, path, dropoffMove.cost, dropoffMove.passengerMoves));
        }

        return actions;
    }

    //public List<Tile> GetDropoffSpots() {
    //    List<Action> tempActions = GetMoveActions();
    //    for (int i = 0; i < tempActions.Count; i++) {
    //        tempActions[i] = new Action(tempActions[i].unit, ActionType.Dropoff, tempActions[i].tile, tempActions[i].path, tempActions[i].cost);
    //    }
    //    return tempActions;
    //}

    
    public List<Action> GetLoadActions() {
        Dictionary<string, Move> moves = GetPossibleMoves();
        return GetLoadActions(moves);
    }

    public List<Action> GetLoadActions(Dictionary<string, Move> moves)
    {
        if (actionPoints == 0) return new List<Action>();

        List<Action> actions = new List<Action>();
        List<Move> options = GetAvailableLoadOptions(moves);
        foreach (Move move in options)
        {
            actions.Add(new LoadAction(this, move.tile, move.path, move.cost));
        }
        return actions;
    }
    
    
    public List<Action> GetUnloadActions() {
        Dictionary<string, Move> moves = GetPossibleMoves();
        return GetUnloadActions(moves);
    }
    
    public List<Action> GetUnloadActions(Dictionary<string, Move> moves)
    {
        if (actionPoints == 0) return new List<Action>();

        List<Action> actions = new List<Action>();
        List<Tile> options = GetUnloadOptions(moves);
        foreach(Tile tile in options)
        {
            actions.Add(new Action(this, ActionType.Unload, tile));
        }
        return actions;
    }

    public List<Tile> GetUnloadOptions(Dictionary<string, Move> moves)
    {
        // return empty if cannot transport
        if (cargo.squads == 0) return new List<Tile>();

        // return empty if not landed
        if (altitude > 0) return new List<Tile>();

        // return empty if empty
        if (cargo.containedUnits.Count == 0) return new List<Tile>();
        

        // return empty if unit has no actions left
        if (actionPoints == 0) return new List<Tile>();

        int unloadDistance = 1;

        return map.GetMovesReturnTiles(tile, unloadDistance);
    }


    public bool CanDoPickup()
    {
        // return false if has no actions left
        if (actionPoints == 0) return false;

        // return empty if cannot transport
        if (cargo.squads == 0) return false;

        // return empty if not landed
        //if (altitude == 0) return new List<Tile>();

        // for now, return false if it is a helo
        if (squadDefs[0].maxAltitude > 0) return false;

        // return false if full
        if (cargo.containedSoldiers > cargo.soldiers) return false;

        return true;
    }
    
    public List<Action> GetPickupActions() {
        Dictionary<string, Move> moves = GetPossibleMoves();
        return GetPickupActions(moves);
    }

    public List<Action> GetPickupActions(Dictionary<string, Move> moves)
    {
        if ( ! CanDoAction() ) return new List<Action>();

        if ( ! CanDoPickup() ) return new List<Action>();

        List<Action> actions = new List<Action>();
        
        //Dictionary<string, Move> moves = global.pathfinder.FindAllMoves(tile, MovementData());
        foreach (KeyValuePair<string, Move> moveKvp in moves)
        {
            string key = moveKvp.Key;
            Move move = moveKvp.Value;

            if (move.tile.unit != null && move.tile.unit.team == team)
            {
                if (cargo.transportable.Contains(move.tile.unit.squadDefs[0].unitClass) && cargo.CanLoad(move.tile.unit))
                {
                    actions.Add(new Action(this, ActionType.Pickup, move.tile));
                }
            }
        }
        
        return actions;
    }


    ////////////////////////////////////
    /// Unit Selection
    /////////// ////////////////////////

    public void Select()
    {
        if (team == playerTeam)
        {
            SelectFriendly();
        }
        else
        {
            SelectEnemy();
        }
    }

    public void SelectFriendly()
    {
        //Debug.Log("<color=red>Unit Selecting</color>");
        ui.ShowUnitUI(this);
        ui.UpdateTargetingPanelThisUnit(squadDefs[0]);

        UpdateUnitActionDisplays();
        

        map.ResetBlocking();
    }

    public void SelectEnemy()
    {
        //ShowMoveOptions();

        ui.ShowUnitUI(this);

        //ShowVisibility();
    }

    public void Deselect()
    {
        DestroyAvailableMoves();
        DestroyLoadOptions();

        //ResetVisibility();
    }

    public void UpdateUnitActionDisplays()
    {
        ui.ShowUnitUI(this);
        //ShowMoveOptions();
        //ShowAvailableLoadOptions();
        ShowVisionAndTargets();
    }
    
    public void ShowVisionAndTargets()
    {
        //Debug.Log("<color=blue>Updating Vision and Targets</color>");
        ShowVisibility();
    }
    
    //public void ShowVision()
    //{
    //    Debug.Log("<color=orange>Showing Vision</color>");
    //    ShowVisibility();
    //    ShowShootableTargets();
    //}

    public void ShowVisionAndTargets(Tile forTile, int forAltitude = 0)
    {
        ShowVisibility(forTile, forAltitude);
        //List<Tiles> visibleTiles = GetVisiblity(forTile, forAltitude)
            //ShowVisibilityTheseTiles(forTile, forAltitude);
        //ShowShootableTargets(forTile, forAltitude);
        //ShowShootableTargetsInList(visibleTiles);
    }

    //public void ResetVisibility()
    //{
    //    //map.ResetVisibility();
    //}

    public void Spotted(Unit unit)
    {
        visibleEnemyUnits.Add(unit);
        unit.SpottedBy(this);
    }

    public void LostSightOf(Unit unit)
    {
        visibleEnemyUnits.Remove(unit);
        unit.LostSightOfBy(this);
    }

    public void SpottedBy(Unit unit) {
        //Debug.Log(squadDef.name + " spotted");
        enemiesWithVisionOnThis.Add(unit);
        if ( ! visible && team != 1 )
        {
            Show();
        }
    }

    public void LostSightOfBy(Unit unit)
    {
        enemiesWithVisionOnThis.Remove(unit);
        if ( team != 1 && enemiesWithVisionOnThis.Count == 0 )
        {
            Hide();
        }
    }

    public void GetVisibility()
    {
        visibleTiles =  map.GetUnitVision(this);
        visibleEnemyUnits = new List<Unit>();
        
        for (int i = 0; i < visibleTiles.Count; i++)
        {
            if (visibleTiles[i].unit != null)
            {
                Unit unit = visibleTiles[i].unit;
                Debug.Log(unit.platoonDefs[0].name);
                if (unit.alive && unit.team != team)
                {
                    visibleEnemyUnits.Add(unit);
                }
            }
        }
    }

    public void ShowVisibility() {
        List<Tile> oldVisibleTile = visibleTiles;
        List<Unit> oldVisibleEnemyUnits = visibleEnemyUnits;

        GetVisibility();
        
        map.ResetArtilleryTargets();
        map.ResetUnitVision();
        map.MarkHexesUnitVisible(visibleTiles);


        List<Unit> tempVisibleEnemyUnits = new List<Unit>();

        //Debug.Log("<color=blue>Showing Visibility</color>");

        List<Tile> tempVisibleTiles = map.GetUnitVision(this);

        map.ResetUnitVision();
        map.MarkHexesUnitVisible(visibleTiles);
        map.ShowTeamVision(team);

        List<Unit> newVisibleEnemyUnits = visibleEnemyUnits.Except(oldVisibleEnemyUnits).ToList();
        List<Unit> lostSightOfEnemyUnits = oldVisibleEnemyUnits.Except(visibleEnemyUnits).ToList();

        foreach (Unit unit in newVisibleEnemyUnits) {
            Spotted(unit);
            //unit.SpottedBy(this);
        }
        foreach (Unit unit in lostSightOfEnemyUnits) {
            LostSightOf(unit);
            //unit.LostSightOfBy(this);
        }

        ShowShootableTargets();

    }

    public void ShowVisibility(Tile forTile, int forAltitude = 0)
    {
        List<Tile> visibleTiles = GetVisibleTilesFor(forTile, forAltitude);
        
        map.ResetArtilleryTargets();
        map.ResetUnitVision();
        map.MarkHexesUnitVisible(visibleTiles);
        
        ShowShootableTargetsFrom(forTile, forAltitude, visibleTiles);
    }
    
    public List<Tile> GetVisibleTilesFor(Tile forTile, int forAltitude = 0) {
        return map.GetVisionFor(forTile, forAltitude, GetMaxWeaponRange());
    }

    
    public void ShowShootableTargets()
    {

        List<Unit> targets = GetShootableTargets();

        foreach (Unit target in targets)
        {
            map.MarkHexShootable(target.tile);
        }
    }

    public void ShowShootableTargetsFrom(Tile fromTile, int altitude, List<Tile> tiles)
    {
        //Debug.Log("Looking for shootable targets");
        List<Unit> targets = GetShootableTargetsFrom(fromTile, altitude, tiles);

        foreach (Unit target in targets)
        {
            map.MarkHexShootable(target.tile);
        }
    }

    public void ShowVisibilityTheseTiles(List<Tile> tiles)
    {
        map.ResetUnitVision();
        map.MarkHexesUnitVisible(tiles);
    }
    
     public List<Unit> GetShootableTargets()
    { 
        List<Unit> shootable = new List<Unit>();
        foreach (Unit unit in visibleEnemyUnits)
        {
            //Debug.Log("Can we shoot? " + unit.unitName);
            //Debug.Log(CanShootTarget(unit));
            if (CanShootTarget(unit)) shootable.Add(unit);
        }
        return shootable;
    }

    public List<Unit> GetShootableTargetsFrom(Tile fromTile, int altitude, List<Tile> tiles)
    { 
        List<Unit> shootable = new List<Unit>();
        foreach (Tile tile in tiles) {
            if (tile.unit != null && tile.unit.team != team && tile.unit.visible) {
                if (CanShootTargetFrom(tile.unit, fromTile, altitude)) shootable.Add(tile.unit);
            }
        }
        return shootable;
    }

    public int GetMaxWeaponRange()
    {
        if (squadDefs[0].weapons == null) return 0;

        int range = 0;
        foreach (UnitWeapon uWeapon in squadDefs[0].weapons)
        {
            if (uWeapon.weapon.range > range) range = uWeapon.weapon.range;
        }
        return range;
    }

    public bool moveInAvailableMoves(int[] move)
    {
        //if (availableMoves.Count > 0)
        //{
            foreach (Move availableMove in availableMoves)
            {
                if (move[0] == availableMove.tile.x && move[1] == availableMove.tile.z)
                {
                    return true;
                }
            }
        //}
        return false;
    }

    public bool moveInAvailableMoves(Tile tile)
    {
        return moveInAvailableMoves( new int[] { tile.x, tile.z } );
    }

    public bool tileInLoadOptions(Tile tile)
    {
        foreach (Move cargoOption in loadOptions)
        {
            if (tile.x == cargoOption.tile.x && tile.z == cargoOption.tile.z)
            {
                return true;
            }
        }

        return false;
    }

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
        bool debugThis = false;

        if (debugThis) Debug.Log("Test can we shoot");
        
        //if (squadDefs[0].weapons == null) return false;
        List<UnitWeapon> weapons = Weapons();

        if (weapons.Count == 0) return false;

        //Debug.Log("show blocking please");
        //if ( map.CanSee(this, targetUnit, showBlocking) )
        if ( CanSee(targetUnit) )
        {
            //double range = map.GetRealDistanceBetweenHexes(tile, targetUnit.tile);
            int gameRange = map.GetGameRange(tile, targetUnit.tile);
    
            if (debugThis) Debug.Log( "Game range " + gameRange.ToString() );

            foreach (UnitWeapon uw in weapons) {
                if (debugThis) Debug.Log( "UnitWeapon " + uw.weapon.name + " " + CanShootTargetWithWeaponAtRange(targetUnit, gameRange, uw).ToString() );
                if (CanShootTargetWithWeaponAtRange(targetUnit, gameRange, uw)) return true;
            }
            /*for (int i = 0; i < weapons.Length; i++) {
    
                UnitWeapon uWeapon = squadDefs[0].weapons[i];
    
    
            }*/
    
        }
        else
       {
            if (debugThis) Debug.Log("No LOS");
        }
        return false;
    }

    public bool CanShootTargetFrom( Unit targetUnit, Tile fromTile, int fromAltitude, bool showBlocking = false )
    {
     
        //double range = map.GetRealDistanceBetweenHexes(tile, targetUnit.tile);
        int gameRange = map.GetGameRange(fromTile, targetUnit.tile);
    
        for (int i = 0; i < squadDefs[0].weapons.Length; i++) {
    
            UnitWeapon uWeapon = squadDefs[0].weapons[i];
    
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
            double range = map.GetRealDistanceBetweenHexes(tile, targetUnit.tile);
            int gameRange = map.GetGameRange(tile, targetUnit.tile);

            UnitWeapon uWeapon = squadDefs[0].weapons[weaponId];

            if (CanShootTargetWithWeaponAtRange(targetUnit, gameRange, uWeapon)) return true;

        }
        else
        {

        }

        return false;
    }

    private bool CanShootTargetWithWeaponAtRange(Unit targetUnit, int gameRange, UnitWeapon uWeapon)
    {
        if (uWeapon.ammo >= 1)
        {
            int weaponRange = uWeapon.weapon.range;

            if (weaponRange >= gameRange)
            {
                if ( uWeapon.weapon.damageProfiles.ContainsKey(targetUnit.UnitTargetType()) )
                {
                    double expDamage = uWeapon.weapon.damageProfiles[targetUnit.UnitTargetType()].GetAverageDamage(targetUnit, uWeapon, gameRange, targetUnit.tile.terrain, targetUnit.Countermeasures(), PortionMovementUsed());

                    if (expDamage > 0.0) return true;

                    //else Debug.Log("Expected damage not greater than 0 " + targetUnit.UnitTargetType());
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
        return (actionPoints == 0);
    }
    public bool Moving()
    {
        return (actionPoints == 0);
    }

    public double MoveCostUsed() {
        return moveCostUsed;
    }

    public double PortionMovementUsed() {
        return moveCostUsed / MovementData().moves;
    }

    public List<FiringWeapon> SelectFiringWeapons(Unit targetUnit)
    {
        int gameRange = map.GetGameRange(tile, targetUnit.tile);

        var fWeps = new List<FiringWeapon>();

        List<UnitWeapon> unitWeapons = Weapons();

        for (int i = 0; i < unitWeapons.Count; i++)
        {
            UnitWeapon uWeapon = unitWeapons[i];

            if (CanShootTargetWithWeaponAtRange(targetUnit, gameRange, uWeapon))
            {
                double expDamage = uWeapon.weapon.damageProfiles[targetUnit.UnitTargetType()].GetAverageDamagePerWeapon(targetUnit, uWeapon, gameRange, targetUnit.tile.terrain, targetUnit.Countermeasures(), PortionMovementUsed());
                if (expDamage > 0)
                {
                    expDamage /= (double)uWeapon.weapon.soldiersToFire;
                    fWeps.Add( new FiringWeapon(uWeapon, expDamage) );
                }
                
            }
        }

        if (fWeps.Count > 0) {
            List<FiringWeapon> sortedWeps = fWeps.OrderByDescending(o => o.expDamage).ToList();
            int soldiersRemaining = squadDefs[0].soldiers;

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
        int gameRange = map.GetGameRange(tile, targetTile);

        var fWeps = new List<FiringWeapon>();

        List<UnitWeapon> unitWeapons = Weapons();

        for (int i = 0; i < unitWeapons.Count; i++)
        {
            UnitWeapon uWeapon = unitWeapons[i];

            if (uWeapon.weapon.range >= gameRange) {
                fWeps.Add( new FiringWeapon(uWeapon, 0.1) );
                break;
            }
        }

        if (fWeps.Count > 0) {
            //List<FiringWeapon> sortedWeps = weps.OrderByDescending(o => o.expDamage).ToList();
            //int soldiersRemaining = squadDefs[0].soldiers;

            //int i = 0;
            //foreach (FiringWeapon wep in sortedWeps)
            //{
            //    sortedWeps[i].number = Math.Min(wep.uWeapon.number, soldiersRemaining);
            //    soldiersRemaining -= sortedWeps[i].number;
            //    i++;
            //}

            //return sortedWeps;

            return fWeps;
        }

        return new List<FiringWeapon>();
    }

    public int BestUsableWeaponAgainstTarget(Unit targetUnit)
    {
        int bestWeaponId = -1;
        double bestAverageDamage = 0;
        
        int gameRange = map.GetGameRange(tile, targetUnit.tile);

        for (int i = 0; i < squadDefs[0].weapons.Length; i++)
        {

            UnitWeapon uWeapon = squadDefs[0].weapons[i];

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

    public void ConsumeActionPoint()
    {
        actionPoints = Math.Max(0, actionPoints - 1);
        if (actionPoints <= 0 && team == 1)
        {
            if ( ! hasFired)
                unitUi.SetHasPartActions();
            else
                unitUi.SetHasNoActions();
        }
    }

    public bool UnitIsEnemy(Unit otherUnit)
    {
        return team != otherUnit.team;
    }

    public bool CanFire()
    {
        return (! hasFired);
    }
    public void Fired()
    {
        hasFired = true;
        if (team == 1) unitUi.SetHasNoActions();
    }

    public void FireWeapon(Unit targetUnit, FiringWeapon fWeapon, int range)
    {
        UnitWeapon uWeapon = fWeapon.uWeapon;

        DamageProfile damageProfile = uWeapon.weapon.damageProfiles[targetUnit.UnitTargetType()];

        damageProfile.DoHits( uWeapon, targetUnit, range, PortionMovementUsed() );
        
        //double unitHeight = 2.0;

        Vector3 fromPos = tile.position;
        fromPos.y += ((float)unitHeight + (float)altitude) * global.map.YMultiplier();

        Vector3 toPos = targetUnit.tile.position;
        toPos.y += ((float)unitHeight + (float)targetUnit.altitude) * global.map.YMultiplier();


        if (uWeapon.weapon.shootProfile.repetitions >= 1)
        {
            global.shootManager.PlayShootEffect(GetComponent<AudioSource>(), uWeapon.weapon.shootProfile, fWeapon.number, fromPos, toPos);
        }
        
        ConsumeAmmo(uWeapon);
    }

    public void FireWeaponAtTile(Tile targetTile, FiringWeapon fWeapon, int range)
    {
        UnitWeapon uWeapon = fWeapon.uWeapon;

        //DamageProfile damageProfile = uWeapon.weapon.damageProfiles[targetUnit.UnitTargetType()];

        //damageProfile.DoHits(uWeapon, targetUnit, range, moved);
        
        //double unitHeight = 2.0;

        Vector3 fromPos = tile.position;
        fromPos.y += ((float)unitHeight + (float)altitude) * global.map.YMultiplier();

        Vector3 toPos = targetTile.position;
        toPos.y += ((float)unitHeight) * global.map.YMultiplier();


        if (uWeapon.weapon.shootProfile.repetitions >= 1)
        {
            global.shootManager.PlayShootEffect(GetComponent<AudioSource>(), uWeapon.weapon.shootProfile, fWeapon.number, fromPos, toPos);
        }
        
        ConsumeAmmo(uWeapon);
    }

    public void Shoot(Unit targetUnit)
    {
        //Debug.Log("Shoot - weaponid " + weaponId);
        if (CanShootTarget(targetUnit))
        {
            
            ////// platoon based
            
            //int range = map.GetGameRange(tile, targetUnit.tile);
            //bool moved = (actionPoints == 0);

            //List<FiringWeapon> weapons = SelectFiringWeapons(targetUnit);

            //foreach (FiringWeapon fWeapon in weapons)
            //{
            //    FireWeapon(targetUnit, fWeapon, range, moved);
            //}

            ////// squad based

            foreach (Squad squad in squads) {
                squad.SelectAndFireWeapons(targetUnit);
            }


            ConsumeActionPoint();
            Fired();

        }
    }

    public void Shoot(Unit targetUnit, int weaponId)
    {
        //Debug.Log("Shoot - weaponid " + weaponId);
        if ( CanShootTargetWithWeapon(targetUnit, weaponId) )
        {
            //
            UnitWeapon uWeapon = squadDefs[0].weapons[weaponId];

            if (uWeapon.weapon.damageProfiles.ContainsKey(targetUnit.UnitTargetType()))
            {


                int range = map.GetGameRange(tile, targetUnit.tile);
                DamageProfile damageProfile = uWeapon.weapon.damageProfiles[targetUnit.UnitTargetType()];

                //Debug.Log("Shooting. ap: " + actionPoints);

               //Debug.Log("Shooting - moved? " + moved);

                damageProfile.DoHits( uWeapon, targetUnit, range, PortionMovementUsed() );

                //ui.ShowTargetingCrosshair(targetUnit.gameObject);

                

                //global.soundManager.PlaySound(GetComponent<AudioSource>(), global.soundManager.rifleShot, 20, 230.0, 0.5);


                //global.soundManager.PlaySound(GetComponent<AudioSource>(), uWeapon.weapon.soundProfile.clip, 12, 250.0, 0.01);

                //global.soundManager.PlaySound(GetComponent<AudioSource>(), uWeapon.weapon.soundProfile);
                
                //double unitHeight = 2.0;

                Vector3 fromPos = tile.position;
                fromPos.y += ((float)unitHeight + (float)altitude) * global.map.YMultiplier();

                Vector3 toPos = targetUnit.tile.position;
                toPos.y += ((float)unitHeight + (float)targetUnit.altitude) * global.map.YMultiplier();

                if (uWeapon.weapon.shootProfile.repetitions >= 1)
                {
                    global.shootManager.PlayShootEffect(GetComponent<AudioSource>(), uWeapon.weapon.shootProfile, uWeapon.number, fromPos, toPos);
                }
                else
                {
                    Debug.Log("<color=red>Warning: No shoot profile found for this weapon: "+uWeapon.weapon.name+"</color>");
                }


                ConsumeActionPoint();//Debug.Log("Consuming action point on " + squadDefs[0].name + " for shooting");
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

            int range = map.GetGameRange(tile, targetTile);

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

    //public void PlayerShoot(Unit targetUnit, int weaponId)
    //{
    //    if (!hasFired)
    //    {
    //        Shoot(targetUnit, weaponId);
    //        UpdateUnitActionDisplays();
    //        PlayerMoveEnd();
    //    }
    //}

    //public void PlayerShootWithBestWeapon(Unit targetUnit)
    //{
    //    if (!hasFired)
    //    {
    //        int weaponId = BestUsableWeaponAgainstTarget(targetUnit);

    //        if (weaponId >= 0)
    //        {
    //            PlayerShoot(targetUnit, weaponId);
    //        }
    //        else
    //        {
    //            Debug.Log("No weapons available to shoot target");
    //        }
    //    }
    //}
    
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
    
    public void AiShootAutoWeapons(Unit targetUnit)
    {
        Shoot(targetUnit);
    }
     
    public List<UnitWeapon> WeaponsTally() {
        var weapons = new List<UnitWeapon>();
        var weaponsTemp = new Dictionary<string, UnitWeapon>();

        foreach (SquadDef sd in squadDefs) {
            if (sd.weapons == null) continue; // skip if this squad had no weapons

            foreach (UnitWeapon uw in sd.weapons) {
                string uwid = uw.weapon.id;

                if ( !weaponsTemp.ContainsKey(uwid) )
                {
                    weaponsTemp[uwid] = ObjectExtensions.Copy(uw);
                }
                else
                {
                    weaponsTemp[uwid].number += uw.number;
                }
            }
        }
        foreach (KeyValuePair<string, UnitWeapon> kvp in weaponsTemp) {
            weapons.Add(kvp.Value);
        }

        return weapons;
    }
     
    public List<UnitWeapon> Weapons() {
        var weapons = new List<UnitWeapon>();
        var weaponsTemp = new Dictionary<string, UnitWeapon>();

        foreach (SquadDef sd in squadDefs) {
            foreach (UnitWeapon uw in sd.weapons) {
                weapons.Add(uw);
            }
        }

        return weapons;
    }

    public int Ammo(int weaponId)
    {
        return squadDefs[0].weapons[weaponId].ammo;
    }

    public bool HasAmmo(int weaponId, int ammoRequired = 1)
    {
        return squadDefs[0].weapons[weaponId].ammo >= ammoRequired;
    }

    public void ConsumeAmmo(int weaponId, int ammoUsed = 1)
    {
        //Debug.Log("consume ammo");
        //Debug.Log(weaponId);
        squadDefs[0].weapons[weaponId].ammo = Math.Max(0, squadDefs[0].weapons[weaponId].ammo - ammoUsed);
        //Debug.Log(squadDefs[0].weapons[weaponId].ammo);
    }

    public void ConsumeAmmo(UnitWeapon uWeapon, int ammoUsed = 1)
    {
        //Debug.Log("consume ammo");
        //Debug.Log(weaponId);
        uWeapon.ammo = Math.Max(0, uWeapon.ammo - ammoUsed);
        //Debug.Log(squadDefs[0].weapons[weaponId].ammo);
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

    void RemoveSeenByThisUnit()
    {
        foreach (Unit enemyUnit in visibleEnemyUnits)
        {
            enemyUnit.LostSightOfBy(this);
        }
        visibleEnemyUnits = new List<Unit>();
    }

    public void Die()
    {
        Debug.Log("<color=green>Die!</color>");
        RemoveSeenByThisUnit();
        alive = false;
        tile.unit = null;
        gameObject.GetComponent<MeshCollider>().enabled = false;

        if (cargo.HasCargo())
        {
            int len = cargo.containedUnits.Count;
            for (int i = 0; i < len; i++)
            {
                Unit dismountSquad = cargo.containedUnits[0];
                cargo.UnloadUnit(dismountSquad);
                unitUi.SetCargoText("");
                dismountSquad.BailOut(tile);
            }
        }

        //gameObject.GetComponent<Renderer>().enabled = false;
        Hide();
    }

    // IsShootableTargetAtHex()

    // FindAllShootableTargets()

    // Fire()

    // GetsShot()
    
    //Vector3 namePos = Camera.main.WorldToScrenPoint(this.transform.position);
    //nameLabel.transform.position = namePos;
   

    public void Fortify()
    {
        if (tile.fort == null)
        {
            Forts.MakeFort(FortDefs.forts[squadDefs[0].fortType], tile);

            //GameObject fortDisc = Instantiate(fortDiscObj);
            //Fort fort = (Fort)fortDisc.GetComponent(typeof(Fort));
            //fort.Setup(FortDefs.forts[squadDefs[0].fortType], tile);
            //tile.fort = fort;
            // //tile.fort = new Fort(FortDefs.forts[squadDefs[0].fortType]);

            buildFortAtTurnEnd = true;
            ConsumeActionPoint();//Debug.Log("Consuming action point on " + squadDefs[0].name + " for fortifying");
        }
    }

    public bool HasSetupWeapon()
    {
        if (squadDefs[0].weapons == null) return false;

        foreach (UnitWeapon uWeapon in squadDefs[0].weapons)
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
        foreach (UnitWeapon uWeapon in squadDefs[0].weapons)
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
                ConsumeActionPoint();//Debug.Log("Consuming action point on " + squadDefs[0].name + " for setting up");
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
            ConsumeActionPoint();//Debug.Log("Consuming action point on " + squadDefs[0].name + " for continuing to set up");
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
                ConsumeActionPoint(); //Debug.Log("Consuming action point on " + squadDefs[0].name + " for packing up");
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
            ConsumeActionPoint(); //Debug.Log("Consuming action point on " + squadDefs[0].name + " for continuing pack up");
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
        if (squadDefs[0].weapons == null) return false;

        foreach (UnitWeapon uWeapon in squadDefs[0].weapons)
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
        foreach (UnitWeapon uWeapon in squadDefs[0].weapons)
        {
            if (uWeapon.weapon.indirect)
            {
                return uWeapon;
            }
        }
        return null;
    }

    public void ArtilleryAttack(UnitWeapon uWeapon, ArtilleryTarget target, int shots)
    {
        //global.shootManager.PlayShootEffect(GetComponent<AudioSource>(), uWeapon.weapon.soundProfile, tile, target.tile);

        //if (target.type == ArtilleryTargetType.Point) Debug.Log("Point mission");
        //if (target.type == ArtilleryTargetType.Circle) Debug.Log("Circle mission");
        //if (target.type == ArtilleryTargetType.Line) Debug.Log("Line mission");
        
        global.artilleryManager.AddMission(new ArtilleryMission(this, uWeapon, target, global.turnManager.turn, uWeapon.weapon.indirectWeapon.aimTime, uWeapon.weapon.indirectWeapon.flightTime, shots));

        ConsumeActionPoint();//Debug.Log("Consuming action point on " + squadDefs[0].name + " for arty start");
    }
    public void PlayerArtilleryAttack(UnitWeapon uWeapon, ArtilleryTarget target, int shots)
    {
        ArtilleryAttack(uWeapon, target, shots);
    }

    public int GetRemainingMovesAfterAltChange(int altChange)
    {
        return Moves() - Math.Max(0,altChange);
    }

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
