using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Detection
{
    private static Global global;

    public static void ShowHideVisibleEnemies(int team)
    {
        bool debugThis = false;

        if (debugThis) Debug.Log("showing units");
        List<Unit> enemyUnits = Store.GetAliveTeamUnits((team == 1) ? 2 : 1);

        if (debugThis) Debug.Log( "Alive enemy units: " + enemyUnits.Count.ToString() );

        List<Unit> visibleUnits = GetVisibleEnemies(team);

        if (debugThis) Debug.Log(visibleUnits.Count);

        foreach (Unit unit in enemyUnits)
        {
            if (visibleUnits.Contains(unit))
            {
                if (debugThis) Debug.Log("Trying to show a unit");
                unit.Show();
            } else
            {
                unit.Hide();
            }
        }
    }

    public static void CalculateAndShowHideVisibleEnemies(int team)
    {
        bool debugThis = false;

        if (debugThis) Debug.Log("showing units");
        List<Unit> enemyUnits = Store.GetAliveTeamUnits((team == 1) ? 2 : 1);

        if (debugThis) Debug.Log( "Alive enemy units: " + enemyUnits.Count.ToString() );

        List<Unit> visibleUnits = CalculateAndGetVisibleEnemies(team);

        if (debugThis) Debug.Log(visibleUnits.Count);

        foreach (Unit unit in enemyUnits)
        {
            if (visibleUnits.Contains(unit))
            {
                if (debugThis) Debug.Log("Trying to show a unit");
                unit.Show();
            } else
            {
                unit.Hide();
            }
        }
    }

    public static void ShowVisibleEnemies(Unit unit)
    {
        List<Unit> visibleUnits = GetVisibleEnemies(unit);

        foreach (Unit visibleUnit in visibleUnits)
        {
            visibleUnit.Show();
        }
    }

    public static List<Unit> GetVisibleEnemies(int team)
    {

        List<Unit> teamUnits = Store.GetAliveTeamUnits(team);
        List<Unit> enemyUnits = Store.GetAliveTeamUnits((team == 1) ? 2 : 1);

        List<Unit> visibleUnits = new List<Unit>();

        foreach (Unit teamUnit in teamUnits)
        {
            foreach (Unit enemyUnit in enemyUnits)
            {
                if ( ! visibleUnits.Contains(enemyUnit) )
                {
                    if (teamUnit.visibleEnemyUnits.Contains(enemyUnit))
                    //if (teamUnit.CanSee(enemyUnit))
                    {
                        visibleUnits.Add(enemyUnit);
                    }
                }
            }
        }

        return visibleUnits;
    }

    public static List<Unit> GetVisibleEnemies(Unit unit)
    {
        List<Unit> enemyUnits = Store.GetAliveTeamUnits((unit.team == 1) ? 2 : 1);

        List<Unit> visibleUnits = unit.visibleEnemyUnits;

        /*List<Unit> visibleUnits = new List<Unit>();

        foreach (Unit enemyUnit in enemyUnits)
        {
            if (unit.CanSee(enemyUnit))
            {
                visibleUnits.Add(enemyUnit);
            }
        }*/

        return visibleUnits;
    }

    public static List<Unit> CalculateAndGetVisibleEnemies(int team)
    {
        bool debugThis = false;

        List<Unit> teamUnits = Store.GetAliveTeamUnits(team);
        List<Unit> enemyUnits = Store.GetAliveTeamUnits((team == 1) ? 2 : 1);
        
        foreach (Unit teamUnit in teamUnits)
        {
            if (debugThis) Debug.Log( "Getting visibility for " + teamUnit.UnitName() );
            teamUnit.GetVisibility();
        }

        List<Unit> visibleUnits = new List<Unit>();

        foreach (Unit teamUnit in teamUnits)
        {
            foreach (Unit enemyUnit in enemyUnits)
            {
                if ( ! visibleUnits.Contains(enemyUnit) )
                {
                    //if (teamUnit.visibleEnemyUnits.Contains(enemyUnit))
                    if (teamUnit.CanSee(enemyUnit))
                    {
                        visibleUnits.Add(enemyUnit);
                    }
                }
            }
        }

        return visibleUnits;
    }

    public static List<Unit> CalculateAndGetVisibleEnemies(Unit unit)
    {
        List<Unit> enemyUnits = Store.GetAliveTeamUnits((unit.team == 1) ? 2 : 1);
        
        unit.GetVisibility();

        //List<Unit> visibleUnits = unit.visibleEnemyUnits;

        List<Unit> visibleUnits = new List<Unit>();

        foreach (Unit enemyUnit in enemyUnits)
        {
            if (unit.CanSee(enemyUnit))
            {
                visibleUnits.Add(enemyUnit);
            }
        }

        return visibleUnits;
    }

    public static void ShowUnitIfVisible(int team, Unit enemyUnit)
    {

        if (UnitIsVisibleToTeam(team, enemyUnit))
        {
            ShowUnit(enemyUnit);
        }
    }

    public static void ShowOrHideUnit(int team, Unit enemyUnit)
    {
        if (UnitIsVisibleToTeam(team, enemyUnit))
        {
            ShowUnit(enemyUnit);
        }
        else
        {
            //HideUnit(enemyUnit);
        }
    }

    public static bool UnitIsVisibleToTeam(int team, Unit enemyUnit)
    {
        List<Unit> teamUnits = Store.GetAliveTeamUnits(team);

        foreach (Unit teamUnit in teamUnits)
        {
            //if (teamUnit.CanSee(enemyUnit))
            if (teamUnit.visibleEnemyUnits.Contains(enemyUnit) || teamUnit.visibleTiles.Contains(enemyUnit.tile))
            {
                return true;
            }
        }
        return false;
    }

    public static void ShowUnit(Unit unit)
    {
        unit.Show();
    }

    public static void HideUnit(Unit unit)
    {
        unit.Hide();
    }




    /*public static List<Unit> GetVisibleForts(int team)
    {
        List<Unit> teamUnits = Store.GetAliveTeamUnits(team);

        tiles = global.map.tiles;
        foreach (Tile tile in tiles.Values)
        {

        }

        List<Unit> visibleUnits = new List<Unit>();

        foreach (Unit teamUnit in teamUnits)
        {
            foreach (Unit enemyUnit in enemyUnits)
            {
                if (!visibleUnits.Contains(enemyUnit))
                {
                    if (teamUnit.CanSee(enemyUnit))
                    {
                        visibleUnits.Add(enemyUnit);
                    }
                }
            }
        }

        return visibleUnits;
    }*/


}
