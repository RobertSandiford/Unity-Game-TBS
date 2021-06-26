using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public struct Node
{
    public Tile tile;

    public Node(Tile Tile)
    {
        tile = Tile;
    }
}

// A step from A to adjacent B within a full Move
public struct Step
{
    public Tile tile;
    public double cost;
    public Step(Tile Tile, double Cost)
    {
        tile = Tile;
        cost = Cost;
    }
}

// A complete Move from A to X, comprising 1 or more steps
public struct Move
{
    public Tile tile;
    public double cost;
    public Tile[] path;
    public List<Step> steps;
    public Move(Tile Tile, double Cost, List<Step> Steps) {
        tile = Tile;
        cost = Cost;

        path = new Tile[Steps.Count];
        for (int i = 0; i < Steps.Count; i++)
        {
            path[i] = Steps[i].tile;
        }

        steps = Steps;
    }
    public Move(Tile Tile)
    {
        tile = Tile;
        cost = 0;
        path = null; // new Tile[] { Tile };
        steps = new List<Step>();
    }
}
public struct DropoffMove
{
    public Tile tile;
    public double cost;
    public List<Step> steps;
    public List<Move> passengerMoves;

    public DropoffMove(Tile Tile, double Cost, List<Step> Steps, List<Move> PassengerMoves)
    {
        tile = Tile;
        cost = Cost;
        steps = Steps;
        passengerMoves = PassengerMoves;
    }
}

public struct MovementData
{
    public int moves;
    public Dictionary<int, int> terrainCosts;
    public double roadCost;
    public bool flying; // defaults to false
    public int groundMaxElevationGain;
    public int groundMaxElevationLoss;
    public double groundElevationGainCost;
    public double groundElevationDropCost;
    public int groundElevationGainCostStartsAt;
    public int groundElevationLossCostStartsAt;

    /*public MovementData (int Moves, Dictionary<int, int> TerrainCosts)
    {
        moves = Moves;
        terrainCosts = TerrainCosts;
    }*/
}

public class AStarNode
{
    public Tile tile;
    public double cost;
    public List<Tile> path;
    public double minRemainingCost;
    public double minTotalCost;
    public bool expired = false;

    public AStarNode() { }
}

public class Pathfinder
{
    private Map map;
    //private Tile from;
    //private Tile to;

    //int subHexes = 0;

    public static MovementData Infantry_MovementData = new MovementData {
        //moves = 1,
        moves = 2,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 1 }, // forest: 1
            { 3, 1 }, // urban: 1
        },
        roadCost = 1.0,
        groundMaxElevationGain = 2,
        groundMaxElevationLoss = 2,
    };

    public static MovementData Gun_MovementData = new MovementData {
        //moves = 1,
        moves = 1,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 1 }, // forest: 1
            { 3, 1 }, // urban: 1
        },
        roadCost = 1.0,
        groundMaxElevationGain = 2,
        groundMaxElevationLoss = 2,
    };


    public static MovementData Truck_MovementData = new MovementData
    {
        //moves = 2,
        moves = 3,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 2 }, // forest: 2
            { 3, 1 }, // urban: 1
        },
        roadCost = 0.33, // 9 moves
        groundMaxElevationGain = 1,
        groundMaxElevationLoss = 1,
        groundElevationGainCost = 2.0,
        groundElevationDropCost = 1.5,
        groundElevationGainCostStartsAt = 2,
        groundElevationLossCostStartsAt = 2,
    };
    public static MovementData Car_MovementData = new MovementData
    {
        //moves = 2,
        moves = 4,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 2 }, // forest: 2
            { 3, 1 }, // urban: 1
        },
        roadCost = 0.4, // 10 moves
        groundMaxElevationGain = 1,
        groundMaxElevationLoss = 1,
        groundElevationGainCost = 2.0,
        groundElevationDropCost = 1.5,
        groundElevationGainCostStartsAt = 2,
        groundElevationLossCostStartsAt = 2,
    };
    public static MovementData Wheeled_MovementData = new MovementData
    {
        //moves = 3,
        moves = 4,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 2 }, // forest: 2
            { 3, 1 }, // urban: 1
        },
        roadCost = 0.5, // 8 moves
        groundMaxElevationGain = 1,
        groundMaxElevationLoss = 1,
        groundElevationGainCost = 2.0,
        groundElevationDropCost = 1.5,
        groundElevationGainCostStartsAt = 2,
        groundElevationLossCostStartsAt = 2,
    };
    public static MovementData WheeledSlow_MovementData = new MovementData
    {
        //moves = 3,
        moves = 3,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 2 }, // forest: 2
            { 3, 1 }, // urban: 1
        },
        roadCost = 0.5, // 6 moves
        groundMaxElevationGain = 1,
        groundMaxElevationLoss = 1,
        groundElevationGainCost = 2.0,
        groundElevationDropCost = 1.5,
        groundElevationGainCostStartsAt = 2,
        groundElevationLossCostStartsAt = 2,
    };
    public static MovementData Tracked_MovementData = new MovementData
    {
        //moves = 3,
        moves = 4,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 2 }, // forest: 2
            { 3, 1 }, // urban: 1
        },
        roadCost = 0.66, // 6 moves
        groundMaxElevationGain = 1,
        groundMaxElevationLoss = 1,
        groundElevationGainCost = 2.0,
        groundElevationDropCost = 1.5,
        groundElevationGainCostStartsAt = 2,
        groundElevationLossCostStartsAt = 2,
    };
    public static MovementData TrackedSlow_MovementData = new MovementData
    {
        //moves = 2,
        moves = 3,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 2 }, // forest: 2
            { 3, 1 }, // urban: 1
        },
        roadCost = 0.666, // 4 moves
        groundMaxElevationGain = 1,
        groundMaxElevationLoss = 1,
        groundElevationGainCost = 2.0,
        groundElevationDropCost = 1.5,
        groundElevationGainCostStartsAt = 2,
        groundElevationLossCostStartsAt = 2,
    };

    public static MovementData Helicopter_MovementData = new MovementData
    {
        //moves = 5,
        moves = 7,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 1 }, // forest: 1
            { 3, 1 }, // urban: 1
        },
        flying = true
    };


    
    public static MovementData WW2_Infantry_MovementData = new MovementData {
        //moves = 1,
        moves = 5,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 1 }, // forest: 1
            { 3, 1 }, // urban: 1
        },
        roadCost = 1.0,
        groundMaxElevationGain = 2,
        groundMaxElevationLoss = 2,
    };

    public static MovementData WW2_CswLight_MovementData = new MovementData { // MMG
        //moves = 1,
        moves = 5,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 1 }, // forest: 1
            { 3, 1 }, // urban: 1
        },
        roadCost = 1.0,
        groundMaxElevationGain = 2,
        groundMaxElevationLoss = 2,
    };

    public static MovementData WW2_CswMedium_MovementData = new MovementData { // HMG
        //moves = 1,
        moves = 4,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 1 }, // forest: 1
            { 3, 1 }, // urban: 1
        },
        roadCost = 1.0,
        groundMaxElevationGain = 2,
        groundMaxElevationLoss = 2,
    };
    public static MovementData WW2_CswHeavy_MovementData = new MovementData { // Mortar
        //moves = 1,
        moves = 3,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 1 }, // forest: 1
            { 3, 1 }, // urban: 1
        },
        roadCost = 1.0,
        groundMaxElevationGain = 2,
        groundMaxElevationLoss = 2,
    };
    public static MovementData WW2_CswVeryHeavy_MovementData = new MovementData { // Heavy Mortar
        //moves = 1,
        moves = 2,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 1 }, // forest: 1
            { 3, 1 }, // urban: 1
        },
        roadCost = 1.0,
        groundMaxElevationGain = 2,
        groundMaxElevationLoss = 2,
    };
    public static MovementData WW2_CswVeryVeryHeavy_MovementData = new MovementData { // ???
        //moves = 1,
        moves = 1,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 1 }, // forest: 1
            { 3, 1 }, // urban: 1
        },
        roadCost = 1.0,
        groundMaxElevationGain = 2,
        groundMaxElevationLoss = 2,
    };
    
    public static MovementData WW2_GunVeryLight_MovementData = new MovementData { // 37mm
        //moves = 1,
        moves = 3,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 1 }, // forest: 1
            { 3, 1 }, // urban: 1
        },
        roadCost = 1.0,
        groundMaxElevationGain = 2,
        groundMaxElevationLoss = 2,
    };
    public static MovementData WW2_GunLight_MovementData = new MovementData { // 50 (1T) / 57mm (1.2T)
        //moves = 1,
        moves = 3,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 1 }, // forest: 1
            { 3, 1 }, // urban: 1
        },
        roadCost = 1.0,
        groundMaxElevationGain = 2,
        groundMaxElevationLoss = 2,
    };
    public static MovementData WW2_GunMedium_MovementData = new MovementData { // 75mm (1.4T), 25Pdr (1.63T)
        //moves = 1,
        moves = 2,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 1 }, // forest: 1
            { 3, 1 }, // urban: 1
        },
        roadCost = 1.0,
        groundMaxElevationGain = 2,
        groundMaxElevationLoss = 2,
    };
    public static MovementData WW2_GunHeavy_MovementData = new MovementData { // 17P (3T), LeFH 18 (2T)
        //moves = 1,
        moves = 2,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 1 }, // forest: 1
            { 3, 1 }, // urban: 1
        },
        roadCost = 1.0,
        groundMaxElevationGain = 2,
        groundMaxElevationLoss = 2,
    };
    public static MovementData WW2_GunVeryHeavy_MovementData = new MovementData { // Pak 43 (3.6T)
        //moves = 1,
        moves = 1,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 1 }, // forest: 1
            { 3, 1 }, // urban: 1
        },
        roadCost = 1.0,
        groundMaxElevationGain = 2,
        groundMaxElevationLoss = 2,
    };
    public static MovementData WW2_GunVeryVeryHeavy_MovementData = new MovementData { // 3.6 Inch, Flak 88mm
        //moves = 1,
        moves = 1,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 1 }, // forest: 1
            { 3, 1 }, // urban: 1
        },
        roadCost = 1.0,
        groundMaxElevationGain = 2,
        groundMaxElevationLoss = 2,
    };
    public static MovementData WW2_GunStatic_MovementData = new MovementData {
        //moves = 1,
        moves = 1,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 1 }, // forest: 1
            { 3, 1 }, // urban: 1
        },
        roadCost = 1.0,
        groundMaxElevationGain = 2,
        groundMaxElevationLoss = 2,
    };


    public static MovementData WW2_Truck_MovementData = new MovementData
    {
        //moves = 2,
        moves = 4,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 2 }, // forest: 2
            { 3, 1 }, // urban: 1
        },
        roadCost = 0.33, // 9 moves
        groundMaxElevationGain = 1,
        groundMaxElevationLoss = 1,
        groundElevationGainCost = 2.0,
        groundElevationDropCost = 1.5,
        groundElevationGainCostStartsAt = 2,
        groundElevationLossCostStartsAt = 2,
    };
    public static MovementData WW2_Wheeled_MovementData = new MovementData
    {
        //moves = 3,
        moves = 6,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 2 }, // forest: 2
            { 3, 1 }, // urban: 1
        },
        roadCost = 0.5, // 8 moves
        groundMaxElevationGain = 1,
        groundMaxElevationLoss = 1,
        groundElevationGainCost = 2.0,
        groundElevationDropCost = 1.5,
        groundElevationGainCostStartsAt = 2,
        groundElevationLossCostStartsAt = 2,
    };
    public static MovementData WW2_Carrier_MovementData = new MovementData
    {
        //moves = 3,
        moves = 9,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 2 }, // forest: 2
            { 3, 1 }, // urban: 1
        },
        roadCost = 0.66, // 6 moves
        groundMaxElevationGain = 1,
        groundMaxElevationLoss = 1,
        groundElevationGainCost = 2.0,
        groundElevationDropCost = 1.5,
        groundElevationGainCostStartsAt = 2,
        groundElevationLossCostStartsAt = 2,
    };
    public static MovementData WW2_Halftrack_MovementData = WW2_Carrier_MovementData;
    public static MovementData WW2_TrackedVeryVerySlow_MovementData = new MovementData
    {
        //moves = 3,
        moves = 4,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 2 }, // forest: 2
            { 3, 1 }, // urban: 1
        },
        roadCost = 0.66, // 6 moves
        groundMaxElevationGain = 1,
        groundMaxElevationLoss = 1,
        groundElevationGainCost = 2.0,
        groundElevationDropCost = 1.5,
        groundElevationGainCostStartsAt = 2,
        groundElevationLossCostStartsAt = 2,
    };
    public static MovementData WW2_TrackedVerySlow_MovementData = new MovementData
    {
        //moves = 3,
        moves = 6,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 2 }, // forest: 2
            { 3, 1 }, // urban: 1
        },
        roadCost = 0.66, // 6 moves
        groundMaxElevationGain = 1,
        groundMaxElevationLoss = 1,
        groundElevationGainCost = 2.0,
        groundElevationDropCost = 1.5,
        groundElevationGainCostStartsAt = 2,
        groundElevationLossCostStartsAt = 2,
    };
    public static MovementData WW2_TrackedSlow_MovementData = new MovementData
    {
        //moves = 3,
        moves = 8,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 2 }, // forest: 2
            { 3, 1 }, // urban: 1
        },
        roadCost = 0.66, // 6 moves
        groundMaxElevationGain = 1,
        groundMaxElevationLoss = 1,
        groundElevationGainCost = 2.0,
        groundElevationDropCost = 1.5,
        groundElevationGainCostStartsAt = 2,
        groundElevationLossCostStartsAt = 2,
    };
    public static MovementData WW2_TrackedMedium_MovementData = new MovementData
    {
        //moves = 3,
        moves = 10,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 2 }, // forest: 2
            { 3, 1 }, // urban: 1
        },
        roadCost = 0.66, // 6 moves
        groundMaxElevationGain = 1,
        groundMaxElevationLoss = 1,
        groundElevationGainCost = 2.0,
        groundElevationDropCost = 1.5,
        groundElevationGainCostStartsAt = 2,
        groundElevationLossCostStartsAt = 2,
    };
    public static MovementData WW2_TrackedFast_MovementData = new MovementData
    {
        //moves = 3,
        moves = 12,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 2 }, // forest: 2
            { 3, 1 }, // urban: 1
        },
        roadCost = 0.66, // 6 moves
        groundMaxElevationGain = 1,
        groundMaxElevationLoss = 1,
        groundElevationGainCost = 2.0,
        groundElevationDropCost = 1.5,
        groundElevationGainCostStartsAt = 2,
        groundElevationLossCostStartsAt = 2,
    };
    public static MovementData WW2_TrackedVeryFast_MovementData = new MovementData
    {
        //moves = 3,
        moves = 14,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 2 }, // forest: 2
            { 3, 1 }, // urban: 1
        },
        roadCost = 0.66, // 6 moves
        groundMaxElevationGain = 1,
        groundMaxElevationLoss = 1,
        groundElevationGainCost = 2.0,
        groundElevationDropCost = 1.5,
        groundElevationGainCostStartsAt = 2,
        groundElevationLossCostStartsAt = 2,
    };
    public static MovementData WW2_TrackedVeryVeryFast_MovementData = new MovementData
    {
        //moves = 3,
        moves = 16,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 2 }, // forest: 2
            { 3, 1 }, // urban: 1
        },
        roadCost = 0.66, // 6 moves
        groundMaxElevationGain = 1,
        groundMaxElevationLoss = 1,
        groundElevationGainCost = 2.0,
        groundElevationDropCost = 1.5,
        groundElevationGainCostStartsAt = 2,
        groundElevationLossCostStartsAt = 2,
    };
    
    ////////////////// PS

        
    public static MovementData PS_Shot_MovementData = new MovementData {
        //moves = 1,
        moves = 4,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 1 }, // forest: 1
            { 3, 1 }, // urban: 1
        },
        roadCost = 1.0,
        groundMaxElevationGain = 2,
        groundMaxElevationLoss = 2,
    }; 
    public static MovementData PS_Pike_MovementData = new MovementData {
        //moves = 1,
        moves = 3,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 2 }, // forest: 1
            { 3, 1 }, // urban: 1
        },
        roadCost = 1.0,
        groundMaxElevationGain = 2,
        groundMaxElevationLoss = 2,
    }; 
    public static MovementData PS_Horse_MovementData = new MovementData {
        //moves = 1,
        moves = 8,
        terrainCosts = new Dictionary<int, int> {
            { 1, 1 }, // plain: 1
            { 2, 2 }, // forest: 1
            { 3, 1 }, // urban: 1
        },
        roadCost = 1.0,
        groundMaxElevationGain = 2,
        groundMaxElevationLoss = 2,
    };


    public Pathfinder(Map map)
    {
        this.map = map;
    }

    private double GetStepsCost(List<Step> steps)
    {
        double cost = 0;
        foreach (Step step in steps)
        {
            cost += step.cost;
        }
        return cost;
    }

    //// Find all possible moves a hypothetical unit can make from Tile 'from' with MovementData 'movementData'
    public Dictionary<string, Move> FindAllMoves(Tile from, MovementData movementData, double allowedMovePortion = 1.0)
    {
        bool debugThis = false;

        if (debugThis) Debug.Log( "Calculating moves from " + from.ToString() );

        Dictionary<string, Move> moves = new Dictionary<string, Move>();

        //int j = movementData.moves;
        //int j = 1;

        //System.TimeSpan startTime = DateTime.Now.TimeOfDay;
        //System.TimeSpan checkpoint = startTime;
        
        string startKey = from.x + "-" + from.z;

        if (debugThis) Debug.Log( "start key " + startKey );

        // store the starting tile in 'stepMoves', so that our iterative function to detect moves for the next step
        Dictionary<string, Move> stepMoves = new Dictionary<string, Move> {
            { startKey , new Move(from) }
        };

        // work out the number of moves possible in the optimum scenario
        //int maxMoves = (int)Math.Ceiling(movementData.moves / movementData.roadCost); // why was this Ceil? safe side?
        int maxMoves = (int)Math.Floor( (movementData.moves * allowedMovePortion) / movementData.roadCost );

        for (int it = 1; it <= maxMoves; it++)
        {
            stepMoves = FindAllMovesIteration(stepMoves, ref moves, movementData, allowedMovePortion * movementData.moves);
            //Debug.Log("<color=orange>Iteration " + it + " time: " + (DateTime.Now.TimeOfDay - checkpoint) + "</color>");
            //checkpoint = DateTime.Now.TimeOfDay;
        }

        moves.Remove(startKey);
        return moves;
    }
    
    // find all possible moves that are one step away from the last set of moves
    public Dictionary<string, Move> FindAllMovesIteration(Dictionary<string, Move> lastMoves, ref Dictionary<string, Move> moves, MovementData movementData, double maxCost = -1.0)
    {
        Dictionary<string, Move> newMoves = new Dictionary<string, Move>();

        foreach (var kvp in lastMoves)
        {
            Move move = kvp.Value;
            // look for next moves from this move
            Dictionary<string, Move> newMovesTemp = SearchNextMoves(move, ref moves, movementData, maxCost);
            foreach (var kvpTemp in newMovesTemp)
            {
                newMoves[kvpTemp.Key] = kvpTemp.Value;
            }
        }

        return newMoves;
    }

    private List<Step> CloneSteps(List<Step> steps)
    {
        List<Step> retSteps = new List<Step>();
        foreach (Step step in steps)
        {
            retSteps.Add(step);
        }
        return retSteps;
    }

    private Dictionary<string, Move> SearchNextMoves(Move move, ref Dictionary<string, Move> moves, MovementData movementData, double maxCost = -1.0)
    {
        bool debugThis = false;

        //System.TimeSpan startTime = DateTime.Now.TimeOfDay;

        if (maxCost == -1.0) maxCost = movementData.moves;

        if (debugThis) Debug.Log( "max cost " + maxCost.ToString());

        Dictionary<string, Move> newMoves = new Dictionary<string, Move>();

        //Debug.Log("Adjs from " + move.tile.x + "," + move.tile.z);

        //System.TimeSpan checkpoint = DateTime.Now.TimeOfDay;

        // get the adjacent tiles
        List<Tile> adjacent = map.GetTilesInMoveRange(move.tile, 1, true);

        //Debug.Log("<color=red>Tiem to get adj tiles: " + (DateTime.Now.TimeOfDay - checkpoint) + "</color>");
        //checkpoint = DateTime.Now.TimeOfDay;

        foreach (Tile adjTile in adjacent)
        {


            string key = adjTile.x + "-" + adjTile.z;

            //checkpoint = DateTime.Now.TimeOfDay;
            List<Step> steps = CloneSteps(move.steps);
            //Debug.Log("<color=red>Time to copy: " + (DateTime.Now.TimeOfDay - checkpoint) + "</color>");

            //int stepCost = movementData.terrainCosts[adjTile.terrain.id];

            double stepCost = 0.0;
            if (move.tile.road && adjTile.road)
            {
                stepCost = movementData.roadCost;
            } else
            {
                stepCost = (double)movementData.terrainCosts[adjTile.terrain.id];
            }



            ///////////////// Terrain Elevs
            /*if ( ! movementData.flying )
            {
                int elevChange = adjustTile.y - move.tile.y;

                if (elevChange > movementsCosts.groundMaxElevationGain) || elevChange < -1 * movementsCosts.groundMaxElevationGain)
                {

                }

                double elevMulti = 1.0;
                if (elevChange >= movementCosts.groundElevationGainCostStartsAt) {
                    elevMulti = (elevChange - movementCosts.groundElevationGainCostStartsAt + 1) * groundElevationGainCost;
                }
                if (elevChange <= movementCosts.groundElevationLossCostStartsAt)
                {
                    elevMulti = (elevChange - movementCosts.groundElevationLossCostStartsAt + 1) * groundElevationLossCost;
                }

                cost *= cost;
            }*/
            ////////////////////////////////////////

            //checkpoint = DateTime.Now.TimeOfDay;
            steps.Add(new Step(adjTile, stepCost));
            //Debug.Log("<color=red>Time to add steps: " + (DateTime.Now.TimeOfDay - checkpoint) + "</color>");

            //checkpoint = DateTime.Now.TimeOfDay;
            double cost = GetStepsCost(steps);
            //Debug.Log("<color=red>Time to calc costs: " + (DateTime.Now.TimeOfDay - checkpoint) + "</color>");

            //Debug.Log("Adj: " + adjTile.x + "," + adjTile.z + "  c: " + cost);

            if (moves.ContainsKey(key))
            {
                if (cost < moves[key].cost)
                {
                    //checkpoint = DateTime.Now.TimeOfDay;
                    Move newMove = new Move(adjTile, cost, steps);
                    moves[key] = newMove;
                    newMoves[key] = newMove;
                    //Debug.Log("<color=red>Time to make and add new move 1: " + (DateTime.Now.TimeOfDay - checkpoint) + "</color>");
                }
            }
            else if (cost <= maxCost)
            {
                //checkpoint = DateTime.Now.TimeOfDay;
                Move newMove = new Move(adjTile, cost, steps);
                moves[key] = newMove;
                newMoves[key] = newMove;
                //Debug.Log("<color=red>Time to make and add new move 2: " + (DateTime.Now.TimeOfDay - checkpoint) + "</color>");
            }

            //checkpoint = DateTime.Now.TimeOfDay;

        }

        return newMoves;
    }

    //// Find all possible moves a hypothetical unit can make from Tile 'from' with MovementData 'movementData'
    public Dictionary<string, Move[]> FindAllDropoffs(Tile from, MovementData transportMovementData, MovementData passengerMovementData)
    {
        bool debugThis = false;

        if (debugThis) Debug.Log( "Calculating dropoffs from " + from.ToString() );
        
        Dictionary<string, Move> transportMoves = new Dictionary<string, Move>();

        //int j = movementData.moves;
        //int j = 1;

        //System.TimeSpan startTime = DateTime.Now.TimeOfDay;
        //System.TimeSpan checkpoint = startTime;

        double minMovePortionNeededForDismount = 1.0 / passengerMovementData.moves;
        double maxMovePortionForTransit = 1.0 - minMovePortionNeededForDismount;
        
        string startKey = from.x + "-" + from.z;

        // store the starting tile in 'stepMoves', for our iterative function to use when detecting moves for the next step
        Dictionary<string, Move> stepMoves = new Dictionary<string, Move> {
            { startKey , new Move(from) }
        };

        // work out the number of moves possible in the optimum scenario
        //int maxMoves = (int)Math.Ceiling(movementData.moves / movementData.roadCost); // old, why was this Ceiling?
        int maxMoves = (int)Math.Floor( (maxMovePortionForTransit * transportMovementData.moves) / transportMovementData.roadCost );

        if (debugThis) Debug.Log( "max moves: " + maxMoves.ToString() );

        for (int it = 1; it <= maxMoves; it++)
        {
            stepMoves = FindAllMovesIteration(stepMoves, ref transportMoves, transportMovementData, maxMovePortionForTransit * transportMovementData.moves);
            //Debug.Log("<color=orange>Iteration " + it + " time: " + (DateTime.Now.TimeOfDay - checkpoint) + "</color>");
            //checkpoint = DateTime.Now.TimeOfDay;
        }

        Dictionary<string, Move[]> dropoffMoves = new Dictionary<string, Move[]> { };

        foreach (KeyValuePair<string, Move> moveKvp in transportMoves) {
            string key = moveKvp.Key;
            Move move = moveKvp.Value;

            if (debugThis) Debug.Log("Dropoff part1 " + key);

            double remainingMovePortion = 1 - (move.cost / transportMovementData.moves);

            if (debugThis) Debug.Log( "Searching for dismount move parts from " + move.tile.ToString() );
            if (debugThis) Debug.Log( "Remaining portion " + remainingMovePortion.ToString() );
            if (debugThis) Debug.Log( "passengerMovementData " + passengerMovementData.moves.ToString() );
            Dictionary<string, Move> passengerMoves = FindAllMoves(move.tile, passengerMovementData, remainingMovePortion);
            
            foreach (KeyValuePair<string, Move> passengerMoveKvp in passengerMoves) {
                dropoffMoves[key + "|" + passengerMoveKvp.Key] = new Move[] { move, passengerMoveKvp.Value };
                
                if (debugThis) Debug.Log("Full Dropoff Key " + key + "|" + passengerMoveKvp.Key );

                if (debugThis) Debug.Log("Full Dropoff " + move.tile.ToString() + " " + passengerMoveKvp.Value.tile.ToString() );
            }
        }

        return dropoffMoves;

        //moves.Remove(startKey);
        //return transportMoves;
    }
    
    //// Find all possible dropoff moves a hypothetical unit can make from Tile 'from' with MovementData 'transportMovementData'
    //// and passenger movement data 'passengerMovementData'
    //// return a set of transport moves, which each contains many passenger dismount moves to choose from
    public List<DropoffMove> FindAllDropoffsCombined(Tile from, MovementData transportMovementData, MovementData passengerMovementData)
    {
        bool debugThis = false;

        if (debugThis) Debug.Log( "Calculating dropoffs from " + from.ToString() );
        
        var dropoffMoves = new List<DropoffMove>{ };

        Dictionary<string, Move> transportMoves = new Dictionary<string, Move>();

        //System.TimeSpan startTime = DateTime.Now.TimeOfDay;
        //System.TimeSpan checkpoint = startTime;

        double minMovePortionNeededForDismount = 1.0 / passengerMovementData.moves;
        double maxMovePortionForTransit = 1.0 - minMovePortionNeededForDismount;
        
        string startKey = from.x + "-" + from.z;

        // store the starting tile in 'stepMoves', for our iterative function to use when detecting moves for the next step
        Dictionary<string, Move> stepMoves = new Dictionary<string, Move> {
            { startKey , new Move(from) }
        };

        // work out the number of moves possible in the optimum scenario
        //int maxMoves = (int)Math.Ceiling(movementData.moves / movementData.roadCost); // old, why was this Ceiling?
        int maxMoves = (int)Math.Floor( (maxMovePortionForTransit * transportMovementData.moves) / transportMovementData.roadCost );

        if (debugThis) Debug.Log( "max moves: " + maxMoves.ToString() );

        for (int it = 1; it <= maxMoves; it++)
        {
            stepMoves = FindAllMovesIteration(stepMoves, ref transportMoves, transportMovementData, maxMovePortionForTransit * transportMovementData.moves);
            //Debug.Log("<color=orange>Iteration " + it + " time: " + (DateTime.Now.TimeOfDay - checkpoint) + "</color>");
            //checkpoint = DateTime.Now.TimeOfDay;
        }

        //Dictionary<string, Move[]> dropoffMoves = new Dictionary<string, Move[]> { };

        foreach (KeyValuePair<string, Move> moveKvp in transportMoves) {
            string key = moveKvp.Key;
            Move move = moveKvp.Value;

            if (debugThis) Debug.Log("Dropoff part1 " + key);

            double remainingMovePortion = 1 - (move.cost / transportMovementData.moves);

            if (debugThis) Debug.Log( "Searching for dismount move parts from " + move.tile.ToString() );
            if (debugThis) Debug.Log( "Remaining portion " + remainingMovePortion.ToString() );
            if (debugThis) Debug.Log( "passengerMovementData " + passengerMovementData.moves.ToString() );
            Dictionary<string, Move> passengerMoves = FindAllMoves(move.tile, passengerMovementData, remainingMovePortion);

            var passengerMovesList = new List<Move>();

            foreach (KeyValuePair<string, Move> passengerMoveKvp in passengerMoves) {
                //dropoffMoves[key + "|" + passengerMoveKvp.Key] = new Move[] { move, passengerMoveKvp.Value };
                //Debug.Log("Full Dropoff Key " + key + "|" + passengerMoveKvp.Key );
                //Debug.Log("Full Dropoff " + move.tile.ToString() + " " + passengerMoveKvp.Value.tile.ToString() );

                passengerMovesList.Add(passengerMoveKvp.Value);
            }

            dropoffMoves.Add( new DropoffMove(move.tile, move.cost, move.steps, passengerMovesList) );

        }

        return dropoffMoves;

        //moves.Remove(startKey);
        //return transportMoves;
    }
    public List<Tile> Find(Tile from, Tile to)
    {
        return FindDirectPath(from, to);
    }


    /////////////////////////
    /// A*
    /////////////////////////


    /*public static AStarNode FindBestPath(Dictionary<string, MapGenTile> tiles, MapGenTile from, MapGenTile to)
    {
        MapGenTile tile = from;

        string key = tile.x + "-" + tile.z;
        string finalKey = to.x + "-" + to.z;

        double minRemainingCost = (double)DirectDistTo(map, tile, to);
        AStarNode firstNode = new AStarNode
        {
            tile = tile,
            cost = 0.0,
            path = new List<Tile> { tile },
            minRemainingCost = minRemainingCost,
            minTotalCost = minRemainingCost
        };

        // if the dest is the source, just return this 1 step Path
        if (from.x == to.x && from.z == to.z) return firstNode;

        // init the nodes list
        Dictionary<string, AStarNode> nodes = new Dictionary<string, AStarNode>();
        nodes[key] = firstNode;

        // expand nodes until we get the best route
        AStarNode promisingNode = firstNode;

        int i = 0;
        while (true)
        {
            Debug.Log("<color=blue>Best Node Is: " + promisingNode.tile.x + "," + promisingNode.tile.z + " cost: " + promisingNode.cost + " minTotalCost: " + promisingNode.minTotalCost + "</color>");
            ExpandNode(map, promisingNode, ref nodes, to);

            promisingNode = FindMostPromisingNode(map, nodes);

            i++;
            if (i > 999) { break; Debug.Log("Possible infinite loop"); }

            if (nodes.ContainsKey(finalKey) && promisingNode.minTotalCost >= nodes[finalKey].cost) break;
        }

        return nodes[finalKey];
    }*/

    public static AStarNode FindBestPath(Map map, Tile from, Tile to)
    {

        Tile tile = from;

        string key = tile.x + "-" + tile.z;
        string finalKey = to.x + "-" + to.z;

        double minRemainingCost = (double)DirectDistTo(map, tile, to);
        AStarNode firstNode = new AStarNode
        {
            tile = tile,
            cost = 0.0,
            path = new List<Tile> { tile },
            minRemainingCost = minRemainingCost,
            minTotalCost = minRemainingCost
        };

        // if the dest is the source, just return this 1 step Path
        if (from.x == to.x && from.z == to.z) return firstNode;

        // init the nodes list
        Dictionary<string, AStarNode> nodes = new Dictionary<string, AStarNode>();
        nodes[key] = firstNode;
        
        // expand nodes until we get the best route
        AStarNode promisingNode = firstNode;

        int i = 0;
        while (true)
        {
            //Debug.Log("<color=blue>Best Node Is: "+ promisingNode.tile.x + "," + promisingNode.tile.z +" cost: "+promisingNode.cost+" minTotalCost: "+ promisingNode.minTotalCost + "</color>");
            ExpandNode(map, promisingNode, ref nodes, to);

            promisingNode = FindMostPromisingNode(map, nodes, i);

            i++;
            if (i > 9999) { Debug.Log("Possible infinite loop"); break; }

            //Debug.Log("<color=green>" + nodes.ContainsKey(finalKey) + "</color>");
            //Debug.Log("<color=green>" + promisingNode.tile.x + "," + promisingNode.tile.z + " exp: " + promisingNode.expired + "</color>");
            //if (nodes.ContainsKey(finalKey)) Debug.Log("<color=green>new " + promisingNode.minTotalCost + " vs " + nodes[finalKey].cost + "</color>");
            if ( nodes.ContainsKey(finalKey) && (promisingNode.expired || promisingNode.minTotalCost >= nodes[finalKey].cost) ) break;
        }

        return nodes[finalKey];

    }
    
    static void ExpandNode(Map map, AStarNode node, ref Dictionary<string, AStarNode> nodes, Tile to)
    {
        string key = node.tile.x + "-" + node.tile.z;

        List<Tile> adjTiles = map.GetTilesInMoveRange(node.tile, 1);

        foreach (Tile adjTile in adjTiles)
        {
            double cost = 1.0;
            switch (adjTile.terrain.id)
            {
                case 1:
                    cost = 1.2;
                    break;
                case 2:
                    //cost = 2.5;
                    cost = 3.0;
                    break;
                case 3:
                    cost = 1.0;
                    break;

            }

            double heightDiff = Math.Abs(adjTile.y - node.tile.y);

            int heightDiffInt = Math.Abs(adjTile.y - node.tile.y);
            switch (heightDiffInt)
            {
                case 0:
                    cost *= 1.0;
                    break;
                /*case 1:
                    cost *= 1.0;
                    break;
                case 2:
                    cost *= 2.0;
                    break;*/
                default:
                    //cost *= 1.0;
                    cost *= Math.Pow(1.05, heightDiffInt); // 3 -> 3.75, 4 -> 5
                    break;
            }

            // Push the road towards the centre;
            cost += Math.Pow(Math.Abs((double)adjTile.x - ((double)map.width - 1.0)) / ((double)map.width - 1.0), 1.4) * 20.0;

            string adjKey = adjTile.x + "-" + adjTile.z;

            bool insert = false;

            double minRemainingCost = (double)DirectDistTo(map, adjTile, to);
            double minTotalCost = cost + minRemainingCost;

            if (nodes.ContainsKey(adjKey))
            {
                if (minTotalCost < nodes[adjKey].minTotalCost) insert = true;
            }
            else
            {
                insert = true;
            }

            if (insert)
            {
                List<Tile> path = new List<Tile>();
                foreach (Tile tile in node.path) path.Add(tile);
                path.Add(adjTile);
                nodes[adjKey] = new AStarNode
                {
                    tile = adjTile,
                    cost = node.cost + cost,
                    path = path,
                    minRemainingCost = minRemainingCost,
                    minTotalCost = cost + minRemainingCost
                };

                //Debug.Log("<color=green>New node: " + adjTile.x + "," + adjTile.z + " cost: " + nodes[adjKey].cost + " minTotalCost: " + nodes[adjKey].minTotalCost + "</color>");
            }

        }

        nodes[key].expired = true;

    }

    static AStarNode dummyAStarNode = new AStarNode
    {
        expired = true
    };

    static AStarNode FindMostPromisingNode(Map map, Dictionary<string, AStarNode> nodes, int i) {
        AStarNode promisingNode = dummyAStarNode; 

        double bestCost = 999999.0;

        foreach (KeyValuePair<string, AStarNode> kvp in nodes)
        {
            AStarNode node = kvp.Value;
            //if (i == 0) Debug.Log("<color=red>CheckingNode: " + node.tile.x + "," + node.tile.z + " cost: " + node.cost + " minTotalCost: " + node.minTotalCost + " exp " + node.expired + "</color>");
            if ( ! node.expired)
            {
                if (node.minTotalCost < bestCost)
                {
                    promisingNode = node;
                    bestCost = node.minTotalCost;
                }
                if (node.minTotalCost == bestCost)
                {
                    if (Lib.random.NextDouble() > 0.5)
                    {
                        promisingNode = node;
                    }
                }
            }
        }

        return promisingNode;
    }


    public static List<Tile> FindDirectPath(Map map, Tile from, Tile to)
    {
        List<Tile> path = new List<Tile>();

        path.Add(from);

        while (!path[path.Count - 1].Equals(to))
        {
            path.Add(GetStraightestHex(map, path[path.Count - 1], to));
        }

        return path;

        //this.from = from;
        //this.to = to;
    }

    public List<Tile> FindDirectPath(Tile from, Tile to)
    {
        List<Tile> path = new List<Tile>();

        path.Add(from);

        while (!path[path.Count - 1].Equals(to))
        {
            path.Add(GetStraightestHex(path[path.Count - 1], to));
        }

        return path;

        //this.from = from;
        //this.to = to;
    }

    public static int DirectDistTo(Map map, Tile from, Tile to)
    {
        return FindDirectPath(map, from, to).Count;
    }

    public int DirectDistTo(Tile from, Tile to)
    {
        return FindDirectPath(from, to).Count;
    }

    public static double GetAngleTo(Tile from, Tile to)
    {
        if ((to.position.z - from.position.z) == 0)
        {
            if ((to.position.x - from.position.x) > 0)
            {
                return 90.0;
            }
            if ((to.position.x - from.position.x) < 0)
            {
                return 270.0;
            }
            return 0.0; // clicked on self?
        }
        else
        {
            double angle = Math.Atan((to.position.x - from.position.x) / (to.position.z - from.position.z)) / Math.PI * 180.0;

            // normalise
            if ((to.position.z - from.position.z) < 1) { angle += 180; }
            while (angle < 0) { angle += 360; }
            while (angle > 360) { angle -= 360; }

            return angle;
        }
    }

    public static Tile GetStraightestHex(Map map, Tile from, Tile to)
    {
        double angle = GetAngleTo(from, to);

        double angle0 = 90.0;
        double dir1Start = angle0 - 30.0;
        while (angle < dir1Start)
        {
            angle += 360.0;
        }
        int dir = (int)Math.Floor((angle - dir1Start) / 60.0);

        Tile returnTile = null;

        
        //Debug.Log((from.x) + " " + (from.z));
        
        //Debug.Log((to.x) + " " + (to.z));

        switch (dir)
        {
            case 0:
                //+2, +0
                returnTile = map.GetTileFromXZ(from.x + 2, from.z + 0);
                break;
            case 1:
                //+1, -1
                returnTile = map.GetTileFromXZ(from.x + 1, from.z - 1);
                break;
            case 2:
                //-1, -1
                returnTile = map.GetTileFromXZ(from.x - 1, from.z - 1);
                break;
            case 3:
                //-2, +0
                returnTile = map.GetTileFromXZ(from.x - 2, from.z + 0);
                break;
            case 4:
                //+1, +1
                returnTile = map.GetTileFromXZ(from.x - 1, from.z + 1);
                break;
            case 5:
                //+1, +1
                //Debug.Log((from.x + 1) + " " + (from.z + 1));
                returnTile = map.GetTileFromXZ(from.x + 1, from.z + 1);
                break;
        }

        return returnTile;
    }

    public Tile GetStraightestHex(Tile from, Tile to)
    {
        double angle = GetAngleTo(from, to);

        double angle0 = 90.0;
        double dir1Start = angle0 - 30.0;
        while (angle < dir1Start)
        {
            angle += 360.0;
        }
        int dir = (int)Math.Floor((angle - dir1Start) / 60.0);

        Tile returnTile = null;



        switch (dir)
        {
            case 0:
                //+2, +0
                returnTile = map.GetTileFromXZ(from.x + 2, from.z + 0);
                break;
            case 1:
                //+1, -1
                returnTile = map.GetTileFromXZ(from.x + 1, from.z - 1);
                break;
            case 2:
                //-1, -1
                returnTile = map.GetTileFromXZ(from.x - 1, from.z - 1);
                break;
            case 3:
                //-2, +0
                returnTile = map.GetTileFromXZ(from.x - 2, from.z + 0);
                break;
            case 4:
                //+1, +1
                returnTile = map.GetTileFromXZ(from.x - 1, from.z + 1);
                break;
            case 5:
                //+1, +1
                returnTile = map.GetTileFromXZ(from.x + 1, from.z + 1);
                break;
        }

        return returnTile;
    }


}