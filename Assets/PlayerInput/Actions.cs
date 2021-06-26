using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public enum ActionType
{
    None,
    Move,
    ArialMove,
    Load,
    Unload,
    Pickup,
    Dropoff,
    Dismount,
    Shoot,
    Combo,
    Artillery
}

//public struct Action
//{
//    public Unit unit;
//    public ActionType type;
//    public Tile tile;
//    public Tile[] path;
//    public Tile importantTile;
//    public int altitude;
//    public List<int> altitudeOptions;
//    public List<Action> children;
//    public Move pathfinderMove;
//    public double cost;

//    public Action(Unit unit, ActionType type, Tile tile)
//    {
//        this.unit = unit;
//        this.type = type;
//        this.tile = tile;
//        this.path = null;
//        this.importantTile = null;
//        this.altitude = 0;
//        this.altitudeOptions = new List<int>();
//        this.children = null;
//        this.pathfinderMove = new Move();
//        this.cost = 0.0;
//    }
//    public Action(Unit unit, ActionType type, Tile tile, double cost)
//        : this(unit, type, tile)
//    {
//        /*this.unit = unit;
//        this.type = type;
//        this.tile = tile;
//        this.path = null;
//        this.importantTile = null;
//        this.altitude = 0;
//        this.altitudeOptions = new List<int>();
//        this.children = null;
//        this.pathfinderMove = new Move();*/
//        this.cost = cost;
//    }

//    public Action(Unit unit, ActionType type, Tile tile, Tile[] path)
//        : this(unit, type, tile)
//    {
//        /*this.unit = unit;
//        this.type = type;
//        this.tile = tile;*/
//        this.path = path;
//        /*this.importantTile = null;
//        this.altitude = 0;
//        this.altitudeOptions = new List<int>();
//        this.children = null;
//        this.pathfinderMove = new Move();
//        this.cost = 0.0;*/
//    }

//    public Action(Unit unit, ActionType type, Tile tile, Tile[] path, Tile importantTile)
//        : this(unit, type, tile, path)
//    {
//        /*this.unit = unit;
//        this.type = type;
//        this.tile = tile;
//        this.path = path;*/
//        this.importantTile = null;
//        /*this.altitude = 0;
//        this.altitudeOptions = new List<int>();
//        this.children = null;
//        this.pathfinderMove = new Move();
//        this.cost = 0.0;*/
//    }

//    public Action(Unit unit, ActionType type, Tile tile, Tile[] path, Tile importantTile, double cost)
//        : this(unit, type, tile, path, importantTile)
//    {
//        this.cost = cost;
//    }

//    public Action(Unit unit, ActionType type, Tile tile, Tile[] path, Move pathfinderMove)
//        : this(unit, type, tile, path)
//    {
//        /*this.unit = unit;
//        this.type = type;
//        this.tile = tile;
//        this.path = path;
//        this.importantTile = null;
//        this.altitude = 0;
//        this.altitudeOptions = new List<int>();
//        this.children = null;*/
//        this.pathfinderMove = pathfinderMove;
//        /*this.cost = pathfinderMove.cost;*/
//    }
//    public Action(Unit unit, ActionType type, Tile tile, Tile[] path, double cost)
//        : this(unit, type, tile, path)
//    {
//        /*this.unit = unit;
//        this.type = type;
//        this.tile = tile;
//        this.path = path;
//        this.importantTile = null;
//        this.altitude = 0;
//        this.altitudeOptions = new List<int>();
//        this.children = null;
//        this.pathfinderMove = new Move();*/
//        this.cost = cost;
//    }

//    public Action(Unit unit, ActionType type, Tile tile, List<int> AltitudeOptions)
//        : this(unit, type, tile)
//    {
//        if (type != ActionType.ArialMove) Debug.Log("Warning: This Action construtor is intended for arial moves");
//        /*this.unit = unit;
//        this.type = type;
//        this.tile = tile;
//        this.path = null;
//        this.importantTile = null;
//        this.altitude = 0;*/
//        this.altitudeOptions = AltitudeOptions;
//        /*this.children = null;
//        this.pathfinderMove = new Move();
//        this.cost = 0.0;*/
//    }

//    public Action(Unit unit, ActionType type, Tile tile, List<int> AltitudeOptions, Move pathfinderMove)
//        : this(unit, type, tile, AltitudeOptions)
//    {
//        /*if (type != ActionType.ArialMove) Debug.Log("Warning: This Action construtor is intended for arial moves");
//        this.unit = unit;
//        this.type = type;
//        this.tile = tile;
//        this.path = null;
//        this.importantTile = null;
//        this.altitude = 0;
//        this.altitudeOptions = AltitudeOptions;
//        this.children = null;*/
//        this.pathfinderMove = pathfinderMove;
//        /*this.cost = pathfinderMove.cost;*/
//    }

//    public Action(Unit unit, ActionType type, Tile tile, List<int> AltitudeOptions, double cost)
//        : this(unit, type, tile, AltitudeOptions)
//    {
//        /*if (type != ActionType.ArialMove) Debug.Log("Warning: This Action construtor is intended for arial moves");
//        this.unit = unit;
//        this.type = type;
//        this.tile = tile;
//        this.path = null;
//        this.importantTile = null;
//        this.altitude = 0;
//        this.altitudeOptions = AltitudeOptions;
//        this.children = null;
//        this.pathfinderMove = new Move();*/
//        this.cost = cost;
//    }

//    public Action(Unit unit, ActionType type, Tile tile, List<Action> children)
//        : this(unit, type, tile)
//    {
//        /*this.unit = unit;
//        this.type = type;
//        this.tile = tile;
//        this.path = null;
//        this.importantTile = null;
//        this.altitude = 0;
//        this.altitudeOptions = new List<int>();*/
//        this.children = children;
//        /*this.pathfinderMove = new Move();
//        this.cost = 0.0;*/
//    }

//    public Action(Unit unit, ActionType type, Tile tile, List<Action> children, Move pathfinderMove)
//        : this(unit, type, tile)
//    {
//        /*this.unit = unit;
//        this.type = type;
//        this.tile = tile;
//        this.path = null;
//        this.importantTile = null;
//        this.altitude = 0;
//        this.altitudeOptions = new List<int>();
//        this.children = children;*/
//        this.pathfinderMove = pathfinderMove;
//        /*this.cost = pathfinderMove.cost;*/
//    }
//    public Action(Unit unit, ActionType type, Tile tile, List<Action> children, double cost)
//        : this(unit, type, tile)
//    {
//        /*this.unit = unit;
//        this.type = type;
//        this.tile = tile;
//        this.path = null;
//        this.importantTile = null;
//        this.altitude = 0;
//        this.altitudeOptions = new List<int>();
//        this.children = children;
//        this.pathfinderMove = new Move();*/
//        this.cost = cost;
//    }
//}
public class Action
{
    public Unit unit;
    public ActionType type;
    public Tile tile;
    public Tile[] path = null;
    public Move pathfinderMove;
    public double cost = 0.0;
    /*public Tile importantTile;
    public int altitude;
    public List<int> altitudeOptions;
    public List<Action> children;*/
    
    public Action()
    {
        type = ActionType.None;
    }

    public Action(Unit Unit, ActionType Type, Move Move)
    {
        unit = Unit;
        type = Type;
        tile = Move.tile;
        path = Move.path;
        cost = Move.cost;
        pathfinderMove = Move;
    }

    public Action(Unit unit, ActionType type, Tile tile)
    {
        this.unit = unit;
        this.type = type;
        this.tile = tile;
        this.pathfinderMove = new Move();
    }
    
    public Action(Unit unit, ActionType type, Tile tile, Tile[] path)
        : this(unit, type, tile)
    {
        this.path = path;
    }
    
    public Action(Unit Unit, ActionType Type, Tile Tile, Tile[] Path, double Cost)
        : this(Unit, Type, Tile, Path)
    {
        cost = Cost;
    }

    public Action(Unit unit, ActionType type, Tile tile, Tile[] path, Move pathfinderMove, double cost)
        : this(unit, type, tile)
    {
        this.path = path;
        this.pathfinderMove = pathfinderMove;
        this.cost = cost;
    }
}

public class ComboAction : Action
{
    public List<Action> children;

    public ComboAction (Unit Unit, Tile Tile, List<Action> ChildActions)
        : base(Unit, ActionType.Combo, Tile)
    {
        children = ChildActions;
    }
}

public class MoveAction : Action
{

    public MoveAction(Unit Unit, Tile Tile, Tile[] Path, double Cost)
        : base(Unit, ActionType.Move, Tile, Path, Cost)
    {

    }
}
public class ArialMoveAction : Action
{
    public List<int> altitudeOptions;

    public ArialMoveAction(Unit Unit, Tile Tile, List<int> AltitudeOptions)
        : base(Unit, ActionType.ArialMove, Tile)
    {
        altitudeOptions = AltitudeOptions;
    }
}

public class LoadAction : Action
{
    public LoadAction(Unit Unit, Tile Tile, Tile[] Path, double Cost)
        : base(Unit, ActionType.Load, Tile, Path, Cost)
    {
    }
}

public class DropoffAction : Action
{
    //List<DropoffDismountOption> dismountOptions;
    public List<Move> dismountOptions;

    public DropoffAction(Unit Unit, Tile Tile, Tile[] Path, double Cost, List<Move> DismountOptions)
        : base(Unit, ActionType.Dropoff, Tile, Path, Cost)
    {
        dismountOptions = DismountOptions;
    }

    public List<Action> GetDismountActions()
    {
        var actions = new List<Action>();
        foreach (Move move in dismountOptions)
        {
            actions.Add( new DismountAction(unit, move));
        }
        return actions;
    }
}
public class DismountAction : Action
{
    public DismountAction(Unit Unit, Move Move) // initialise with a Pathfinder Move
        : base(Unit, ActionType.Dismount, Move)
    {
        //
    }

    public DismountAction(Unit Unit, Tile Tile, Tile[] Path, double Cost) // initialise with individual data
        : base(Unit, ActionType.Dismount, Tile, Path, Cost)
    {
        //
    }
}

public class DropoffDismountOption
{
    public Tile tile;
    public Tile[] path;

    DropoffDismountOption(Tile Tile, Tile[] Path) {
        tile = Tile;
        path = Path;
    }
}