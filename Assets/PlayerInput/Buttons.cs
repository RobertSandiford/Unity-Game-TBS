using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/*
public class Buttons
{

    private Global global;
    private PlayerInput playerInput;

    public Buttons(PlayerInput PlayerInput)
    {
        playerInput = PlayerInput;
        global = playerInput.global;
    }

    
    public void ButtonArtillery()
    {
        if (selectedUnit)
        {
            if (selectedUnit.actionPoints > 0 && selectedUnit.HasIndirectWeapon())
            {
                if ( ! artilleryTargetingMode )
                {
                    EnableArtilleryTargetingMode();
                }
                else
                {
                    EndArtilleryTargetingMode();
                }
            }
        }
    }

    public void ButtonSalvosPlus()
    {
        Unit u = selectedUnit;
        int maxSalvos = 5;
        if (artillerySalvos + 1 <= maxSalvos)
        {
            artillerySalvos++;
            global.ui.UpdateSalvos(selectedUnit, artillerySalvos);
        }


    }

    public void ButtonSalvosMinus()
    {
        Unit u = selectedUnit;
        int minSalvos = 1;
        if (artillerySalvos - 1 >= minSalvos)
        {
            artillerySalvos--;
            global.ui.UpdateSalvos(selectedUnit, artillerySalvos);
        }
    }

    public void ButtonArtilleryTargetPoint()
    {
        if (!artilleryTargeting) {
            artilleryTargetType = ArtilleryTargetType.Point;
            global.ui.SetArtilleryTargetType(ArtilleryTargetType.Point);
        }
    }

    public void ButtonArtilleryTargetCircle()
    {
        if (!artilleryTargeting)
        {
            artilleryTargetType = ArtilleryTargetType.Circle;
            global.ui.SetArtilleryTargetType(ArtilleryTargetType.Circle);
        }
    }

    public void ButtonArtilleryTargetLine()
    {
        if (!artilleryTargeting)
        {
            artilleryTargetType = ArtilleryTargetType.Line;
            global.ui.SetArtilleryTargetType(ArtilleryTargetType.Line);
        }
    }

    public void ButtonSelectAltitude(Action action, int alt)
    {
        //Debug.Log("Select altitude " + action.tile.x + "," + action.tile.z + " " + alt);
        global.ui.DestroyAltitudePanel();
        moves.DoArialMoveFinal(selectedUnit, action, alt);
    }

    public void ButtonEndTurn()
    {
        if (selectedUnit) DeselectUnit();
        global.turnManager.ProgressTurn();
    }

    public void ButtonNextUnit()
    {
        //if (selectedUnit) DeselectUnit();
        //global.turnManager.ProgressTurn();
        List<Unit> units = Store.GetAliveTeamUnits(1);
        foreach (Unit unit in units) {
            if (unit.actionPoints > 0) {
                global.gameCamera.CenterCameraOnObject(unit.gameObject);
                SelectUnit(unit);
                return;
            }
        }

    }


}
*/

