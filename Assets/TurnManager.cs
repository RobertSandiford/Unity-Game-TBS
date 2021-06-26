using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    public int turn = 0;
    private bool initialised = false;
    private PlayerInput playerInput;
    private Map map;
    private Global global;
    private AiManager aiManager;

    public void DoAiTurn()
    {
        aiManager.DoTurn();
    }

    public void ProgressTurn()
    {

        CheckBuildForts(1);

        //Debug.Log("reset mats");
        map.ResetAllHexMaterials();
        map.ShowTeamVision(1);

        // Do Ai turn
        DoAiTurn();

        //// CALLBACK NEEDED
        // Then advance the turn
        //ProgressTurnPart2();

    }

    public void CheckBuildForts(int team)
    {
        List<Unit> units = Store.GetAliveTeamUnits(team);
        foreach (Unit unit in units)
        {
            if (unit.tile.fort != null)
            {
                if (unit.tile.fort.type == unit.fortType)
                {
                    if ( ! unit.tile.fort.IsComplete() )
                    {
                        if (unit.actionPoints > 0)
                        {
                            unit.tile.fort.Build(1);
                        }
                        else if (unit.buildFortAtTurnEnd)
                        {
                            unit.tile.fort.Build(1);
                            unit.buildFortAtTurnEnd = false;
                        }
                    }
                }
            }
        }
    }

    public void StartNewTurn() // Start Player New Turn
    {
        //// CALLBACK NEEDED
        // Then advance the turn
        turn++;
        
        Unit.ProcessUnitsSetupPackup(1);

        RefreshActions();
        UpdateUnitVisiblities(1);


        global.artilleryManager.ProcessMissions(turn);

        // refresh selected unit
        playerInput.RefreshSelectedUnit();
        
        //Debug.Log("reset mats");
        //map.ResetAllHexMaterials();

    }

    public void RefreshActions()
    {
        List<Unit> units = Store.GetAliveTeamUnits(1);
        foreach (Unit unit in units)
        {
            unit.RefillActions();
        }
        List<Unit> unitsEnemy = Store.GetAliveTeamUnits(2);
        foreach (Unit unit in unitsEnemy)
        {
            unit.RefillActions();
        }
    }

    // quick fix to make enemies that have moved shootable.
    public void UpdateUnitVisiblities(int team)
    {
        List<Unit> units = Store.GetAliveTeamUnits(team);
        foreach (Unit unit in units)
        {
            unit.GetVisibility();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialised) Initialise();
    }

    void Initialise()
    {
        playerInput = (PlayerInput)FindObjectOfType<PlayerInput>();
        aiManager = (AiManager)FindObjectOfType<AiManager>();
        map = (Map)FindObjectOfType<Map>();
        global = (Global)FindObjectOfType<Global>();
        global.turnManager = this;
        initialised = true;
    }
}
