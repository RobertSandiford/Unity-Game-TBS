using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{

    bool initialised = false;
    List<Unit> aiUnits;
    Map map;
    Pathfinder pathfinder;
    UI ui;
    Global global;
    FlyCamera gameCamera;

    bool executing = false;
    int aiUnitIndex;

    public void DoTurn()
    {

        if ( ! executing )
        {

            executing = true;
            PopulateAiUnits(); // may not need to be done each turn.

            StartCoroutine(DoTurnCoroutine());

            //Debug.Log("Number of AI units found: " + aiUnits.Count.ToString());


            /*foreach (Unit unit in aiUnits)
            {
                DoTurnForUnit(unit);

            }*/

            //DoTurnForUnit(aiUnits[0]);

        } else
        {
            Debug.Log("already executing ai turn");
        }
    }

    IEnumerator DoTurnCoroutine()
    {
        gameCamera.LockCamera();
        gameCamera.SaveCameraPosition();

        if (global.map.level.aiMission == AiMission.Attack) yield return StartCoroutine(DoAttackPlan());
        if (global.map.level.aiMission == AiMission.Objective) yield return StartCoroutine(DoAttackPlan());

        if (global.map.level.aiMission == AiMission.Defend) yield return StartCoroutine(DoDefendPlan());

        global.map.ResetAiHexModifiers();

        yield return new WaitForSeconds(0.3f);
        executing = false;

        gameCamera.ResetCameraPosition();
        gameCamera.UnlockCamera();

        global.turnManager.StartNewTurn();

    }

    IEnumerator DoAttackPlan()
    {
        bool first = true;
        foreach (Unit unit in aiUnits)
        {
            if (first)
                first = false;
            else
                yield return new WaitForSeconds(0.1f);

            if (unit.alive)
            {

                unit.GetVisibility();

                Tile startTile = unit.tile;

                if (unit.visible) gameCamera.CenterCameraOnObject(unit.gameObject);
                if (unit.visible) map.MarkHexAiActive(startTile);
                if (unit.visible) yield return new WaitForSeconds(0.3f);
                
                Tile objectiveTile = map.GetNearestTileToProportionalPos(new double[] { 0.5, 0.3 });
                //Tile objectiveTile = map.GetNearestTileToProportionalPos( map.level.objectivesRandom[0].centerPos );

                //Tile objectiveTile = map.GetTileFromXZ(40, 19); // map.level.objectives[0] etc.. etc..
                List<Unit> targets = unit.GetShootableTargets();

                if (targets.Count > 0)
                {
                    map.MarkHexAiShooting(startTile);
                    yield return new WaitForSeconds(0.25f);

                    ui.ShowTargetingCrosshair(targets[0].gameObject);
                    gameCamera.CenterCameraOnObject(targets[0].gameObject);

                    yield return new WaitForSeconds(0.5f);

                    //unit.ShootWithBestWeapon(targets[0]);
                    unit.AiShootAutoWeapons(targets[0]);
                    ui.HideTargetingCrosshair();

                    yield return new WaitForSeconds(0.5f);
                    map.MarkHexAiNormal(startTile);
                }
                else
                {
                    if (unit.visible) map.MarkHexAiNormal(startTile);

                    MoveTowardsTile(unit, objectiveTile);
                    if (unit.visible) map.MarkHexAiActive(unit.tile);

                    if (unit.visible) yield return new WaitForSeconds(0.15f);
                    if (unit.visible) map.MarkHexAiNormal(unit.tile);


                    targets = unit.GetShootableTargets();

                    if (targets.Count > 0)
                    {
                        map.MarkHexAiShooting(unit.tile);
                        yield return new WaitForSeconds(0.25f);

                        ui.ShowTargetingCrosshair(targets[0].gameObject);
                        gameCamera.CenterCameraOnObject(targets[0].gameObject);

                        yield return new WaitForSeconds(0.5f);

                        //unit.ShootWithBestWeapon(targets[0]);
                        unit.AiShootAutoWeapons(targets[0]);
                        ui.HideTargetingCrosshair();

                        yield return new WaitForSeconds(0.5f);
                        map.MarkHexAiNormal(unit.tile);
                    }

                }

                //map.MarkHexAiNormal(startTile);
            }

        }
    }

    IEnumerator DoDefendPlan()
    {
        bool first = true;
        foreach (Unit unit in aiUnits)
        {
            if (first)
                first = false;
            else
                yield return new WaitForSeconds(0.1f);

            if (unit.alive)
            {

                unit.GetVisibility();

                Tile startTile = unit.tile;

                if (unit.visible) gameCamera.CenterCameraOnObject(unit.gameObject);
                if (unit.visible) map.MarkHexAiActive(startTile);
                if (unit.visible) yield return new WaitForSeconds(0.3f);

                Tile objectiveTile = map.GetTileFromXZ(17, 4);
                List<Unit> targets = unit.GetShootableTargets();

                if (targets.Count > 0)
                {
                    map.MarkHexAiShooting(startTile);
                    yield return new WaitForSeconds(0.25f);

                    ui.ShowTargetingCrosshair(targets[0].gameObject);
                    gameCamera.CenterCameraOnObject(targets[0].gameObject);

                    yield return new WaitForSeconds(0.5f);

                    ////unit.ShootWithBestWeapon(targets[0]);
                    unit.AiShootAutoWeapons(targets[0]);
                    ui.HideTargetingCrosshair();

                    yield return new WaitForSeconds(0.5f);
                    map.MarkHexAiNormal(startTile);
                }
                /*else
                {
                    if (unit.visible) map.MarkHexAiNormal(startTile);

                    MoveTowardsTile(unit, objectiveTile);
                    Detection.ShowOrHideUnit(1, unit);
                    if (unit.visible) map.MarkHexActive(unit.tile);

                    if (unit.visible) yield return new WaitForSeconds(0.15f);
                    if (unit.visible) map.MarkHexAiNormal(unit.tile);
                }*/

                //map.MarkHexAiNormal(startTile);
            }

        }
    }

    /*IEnumerator DoTurnForUnit(Unit unit)
    {
        
    }*/

    void MoveTowardsTile(Unit unit, Tile destTile)
    {
        //Debug.Log("mtt");

        //Debug.Log("starting pos");
        //Debug.Log(unit.tile.x);
        //Debug.Log(unit.tile.z);
        //Debug.Log(destTile.x);
        //Debug.Log(destTile.z);
        int distFromCurrentTile = pathfinder.DirectDistTo(unit.tile, destTile);

        Tile bestMove = null;
        int bestDist = distFromCurrentTile;
        //bool moveFound = false;

        List<Tile> moves = map.GetMovesReturnTiles(unit.tile);
        foreach (Tile move in moves)
        {
            if (move.unit == null)
            {
                int dist = pathfinder.DirectDistTo(move, destTile);
                if ( dist < bestDist )
                {
                    bestDist = dist;
                    bestMove = move;
                    //moveFound = true;
                }
            } else
            {
                Debug.Log( "A unit is blocking this move to tile " + move.x.ToString() + "," + move.z.ToString() );
            }
        }
        if (bestMove != null)
        {
            //Debug.Log("bestMove :");
            //Debug.Log(bestMove);
            //Debug.Log(((Tile)bestMove).x);
            //Debug.Log(((Tile)bestMove).z);

            Tile bestMoveTile = (Tile)bestMove;
            unit.AiMoveTo(bestMoveTile);

        }
    }


    void PopulateAiUnits()
    {
        //Debug.Log("pau");
        int aiTeam = 2;

        aiUnits = new List<Unit>();

        Unit[] allUnits = (Unit[])FindObjectsOfType<Unit>();
        for (int i = 0; i < allUnits.Length; i++)
        {
            if (allUnits[i].team ==aiTeam)
            {
                aiUnits.Add(allUnits[i]);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        aiUnits = new List<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialised) Initialise();
    }

    // Update is called once per frame
    void Initialise()
    {
        map = (Map)FindObjectOfType<Map>();
        pathfinder = new Pathfinder(map);
        ui = (UI)FindObjectOfType<UI>();
        gameCamera = (FlyCamera)FindObjectOfType<FlyCamera>();
        global = (Global)FindObjectOfType<Global>();
        initialised = true;
    }
}

