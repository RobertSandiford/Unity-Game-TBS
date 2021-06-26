using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/* enum ActionType - A list of action types, e.g. Move, Load
 *
 * struct Action - A struct contain data about an action, such as type and destination tile
 *
 * class PlayerInput - Main class
 *
 */
 

/*
* class PlayerInput plan
*
*
*
*
*/

public class Artillery
{

    private Global global;
    private PlayerInput playerInput;
    
    private GameObject activeArtilleryTarget;

    //[SerializeField] private GameObject activeArtilleryTargetObject = null;

    public int targetAltitude;
    public bool artilleryTargetingMode = false;
    public bool artilleryTargeting = false;
    public int artillerySalvos;
    public ArtilleryTargetType artilleryTargetType;
    public Tile artilleryTargetingStartTile;

    public Artillery(PlayerInput PlayerInput)
    {
        playerInput = PlayerInput;
        global = playerInput.global;
    }
    
    // Do things related to artillery targetting on tile mouse in 
    public void ArtilleryTargetingMouseOver(Tile tile)
    {
        if (artilleryTargetingMode && playerInput.selectedUnit != null)
        {
            Unit unit = playerInput.selectedUnit;

            int range = global.map.GetGameRange(unit.tile, tile);
            UnitWeapon uWeapon = unit.GetIndirectWeapon();

            if (uWeapon.weapon.indirectWeapon.range >= range)
            {
                //Debug.Log("Placing Artillery Target");
                activeArtilleryTarget = GameObject.Instantiate(playerInput.activeArtilleryTargetObject);
                PositionArtilleryTargetOnTile(activeArtilleryTarget, tile);
            }
        }
    }

    // Process Artillery functions while mouse moves off a tile
    public void ArtilleryTargetingMouseOut(Tile tile)
    {
        if (artilleryTargetingMode && playerInput.selectedUnit != null)
        {
            DestroyArtilleryTarget();
            // reset hex material if needed
        }
    }

    public void ArtilleryDeselectUnit()
    {
        if (artilleryTargetingMode) EndArtilleryTargetingMode();
    }

    
    void PositionArtilleryTargetOnTile(GameObject targetObject, Tile tile)
    {
        Vector3 pos = tile.position;
        if (tile.unit != null)
        {
            pos.y += 0.2f;
        }
        targetObject.transform.position = pos;
    }

    void DestroyArtilleryTarget()
    {
        if (activeArtilleryTarget != null)
        {
            GameObject.Destroy(activeArtilleryTarget);
        }
    }

    public void EnableArtilleryTargetingMode()
    {
        artilleryTargetingMode = true;
        playerInput.DestroyActionIndicators();
        artillerySalvos = 1;
        global.ui.ShowArtilleryControlPanel(artillerySalvos);
        artilleryTargetType = ArtilleryTargetType.Point;
        global.ui.SetArtilleryTargetType(artilleryTargetType);
        //Debug.Log("Arty Targeting Mode Enabled");
    }

    public void EndArtilleryTargetingMode()
    {
        artilleryTargetingMode = false;
        artilleryTargeting = false;
        if (activeArtilleryTarget != null) GameObject.Destroy(activeArtilleryTarget);
        DestroyArtilleryTarget();
        global.ui.HideArtilleryControlPanel();
        //Debug.Log("End Arty Mode");
        //Debug.Log("Arty Targeting Mode Ended");
    }


    public void ArtilleryTargetTile(Tile targetTile)
    {
        switch (artilleryTargetType)
        {
            case ArtilleryTargetType.Point:
                ArtilleryPointTargetTile(targetTile);
                break;
            case ArtilleryTargetType.Circle:
                ArtilleryCircleTargetTile(targetTile);
                break;
            case ArtilleryTargetType.Line:
                ArtilleryLineTargetTile(targetTile);
                break;
        }
    }

    public void ArtilleryPointTargetTile(Tile targetTile)
    {
        Unit unit = playerInput.selectedUnit;

        int range = global.map.GetGameRange(unit.tile, targetTile);
        UnitWeapon uWeapon = unit.GetIndirectWeapon();
        if (range <= uWeapon.weapon.indirectWeapon.range)
        {
            targetTile.hex.SetArtyTarget(true);
            //Debug.Log("Do Arty Attack Here");
            unit.PlayerArtilleryAttack(unit.GetIndirectWeapon(), new ArtilleryTarget(targetTile), artillerySalvos);
            EndArtilleryTargetingMode();
            playerInput.PostAction(unit);
        }
    }
    
        
        
    public void ArtilleryCircleTargetTile(Tile targetTile)
    {
        Unit unit = playerInput.selectedUnit;

        int range = global.map.GetGameRange(unit.tile, targetTile);
        UnitWeapon uWeapon = unit.GetIndirectWeapon();

        if (!artilleryTargeting)
        {
            if (range <= uWeapon.weapon.indirectWeapon.range)
            {
                artilleryTargeting = true;
                artilleryTargetingStartTile = targetTile;
                targetTile.hex.SetArtyTarget(true);
            }
        }
        else
        {
            if (range <= uWeapon.weapon.indirectWeapon.range)
            {
                /////// check that all parts of circle are in range /////

                int radius = global.map.GetMoveDistance(artilleryTargetingStartTile, targetTile);
                if (radius >= 1 && radius <= 3)
                {
                    artilleryTargeting = false;
                    unit.PlayerArtilleryAttack(unit.GetIndirectWeapon(), new ArtilleryTarget(artilleryTargetingStartTile, radius), artillerySalvos);
                    EndArtilleryTargetingMode();
                    playerInput.PostAction(unit);
                }
            }
        }
    }

    public void ArtilleryLineTargetTile(Tile targetTile)
    {
        Unit unit = playerInput.selectedUnit;

        int range = global.map.GetGameRange(unit.tile, targetTile);
        UnitWeapon uWeapon = unit.GetIndirectWeapon();

        if (!artilleryTargeting)
        {
            if (range <= uWeapon.weapon.indirectWeapon.range)
            {
                artilleryTargeting = true;
                artilleryTargetingStartTile = targetTile;
                targetTile.hex.SetArtyTarget(true);
            }
        }
        else
        {
            if (range <= uWeapon.weapon.indirectWeapon.range)
            {
                int dist = global.map.GetMoveDistance(artilleryTargetingStartTile, targetTile);
                if (dist >= 1 && dist <= 5)
                {
                    if (
                        (targetTile.z == artilleryTargetingStartTile.z)
                        || 
                        (Math.Abs(targetTile.x - artilleryTargetingStartTile.x) == Math.Abs(targetTile.z - artilleryTargetingStartTile.z))
                    )
                    {
                        artilleryTargeting = false;
                        unit.PlayerArtilleryAttack(unit.GetIndirectWeapon(), new ArtilleryTarget(artilleryTargetingStartTile, targetTile), artillerySalvos);
                        EndArtilleryTargetingMode();
                        playerInput.PostAction(unit);
                    }
                }
            }
        }
    }

    public void ButtonArtillery()
    {
        Unit unit = playerInput.selectedUnit;

        if (unit)
        {
            if (unit.actionPoints > 0 && unit.HasIndirectWeapon())
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
        Unit unit = playerInput.selectedUnit;
        int maxSalvos = 5;
        if (artillerySalvos + 1 <= maxSalvos)
        {
            artillerySalvos++;
            global.ui.UpdateSalvos(unit, artillerySalvos);
        }
    }

    public void ButtonSalvosMinus()
    {
        Unit unit = playerInput.selectedUnit;
        int minSalvos = 1;
        if (artillerySalvos - 1 >= minSalvos)
        {
            artillerySalvos--;
            global.ui.UpdateSalvos(unit, artillerySalvos);
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

}

