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

public class Moves
{

    private Global global;
    private PlayerInput playerInput;

    public Moves(PlayerInput PlayerInput)
    {
        playerInput = PlayerInput;
        global = playerInput.global;
    }

    public void DoAction(Unit unit, Action action)
    {
        switch (action.type)
        {
            case ActionType.Move:
                DoMove(unit, action);
                break;
            case ActionType.ArialMove:
                DoArialMove(unit, action);
                break;
            case ActionType.Load:
                DoLoad(unit, action);
                break;
            case ActionType.Unload:
                DoUnload(unit, action);
                break;
            case ActionType.Pickup:
                DoPickup(unit, action);
                break;
            case ActionType.Dropoff:
                //DoDropoff(unit, action);
                AllowDropoffDismountChoice(unit, action);
                break;
            case ActionType.Combo:
                DoCombo(unit, action);
                break;
        }
    }

    // Do a ground-based move according to the Action info provided
    public void DoMove(Unit unit, Action action)
    {
        unit.PlayerMoveTo(action, playerInput.targetAltitude);
        playerInput.PostAction(unit, action);
    }

    // Step one of an arial move, brings up a dialogue with destination altitude options
    public void DoArialMove(Unit unit, Action action)
    {
        List<ActionType> actionTypes = new List<ActionType>();
        /*foreach (Action subAction in action.children)
        {
            actionTypes.Add(subAction.type);
        }*/
        //Debug.Log("DoArialMoveACtion");
        //Debug.Log(action.tile.x);
        playerInput.arialMoveSelect = true;
        playerInput.arialMoveAction = action;
        global.ui.ShowActionsPanel(unit, action); //// placeholder
    }

    // Step two of an arial move? move according to the Action info AND separate altitude provided
    public void DoArialMoveFinal(Unit unit, Action action, int altitude)
    {
        unit.PlayerMoveTo(action, altitude);
        playerInput.PostAction(unit, action);
        playerInput.arialMoveSelect = false;
    }

    // Do a Load action acording to the Action provided
    public void DoLoad(Unit unit, Action action)
    {
        unit.PlayerLoadAt(action.tile);
        playerInput.PostAction(unit, action);
    }

    // Do an Unload action acording to the Action provided
    public void DoUnload(Unit unit, Action action)
    {
        unit.PlayerUnloadTo(action.tile);
        playerInput.PostAction(unit, action);
    }

    public void DoPickup(Unit unit, Action action)
    {
        unit.PlayerPickupAt(action.tile);
        playerInput.PostAction(unit, action);
    }
    
    public void AllowDropoffDismountChoice(Unit unit, Action action)
    {
        playerInput.dismountActions = ((DropoffAction)action).GetDismountActions();
        playerInput.dropoffAction = action;
        playerInput.actionMode = ActionMode.Dismount;
        playerInput.actionTier = 2;
        playerInput.PinDropoffIndicator(action);
        playerInput.DestroyActionIndicators(1);
        playerInput.DestroyPathIndicators(1);
        playerInput.DestroyActiveActionIndicator();
        playerInput.HideTargetingAccuracy();
    }

    public void CancelDropoffDismountChoice()
    {
        playerInput.dismountActions = null;
        playerInput.dropoffAction = null;
        playerInput.DeletePinnedDropoffIndicator();
        //playerInput.actionMode = ActionMode.Normal;
        //playerInput.actionTier = 1;
        //playerInput.ShowTheseActions();
    }

    public void DoDropoffAndDismount(Unit unit, Action dropoffAction, Action dismountAction)
    {
        CancelDropoffDismountChoice();
        
        playerInput.actionMode = ActionMode.Normal;
        playerInput.actionTier = 1;

        // do this
        unit.PlayerDropoff(dropoffAction, dismountAction);
        playerInput.PostAction(unit, dropoffAction);
    }

    public void DoCombo(Unit unit, Action action)
    {
        ComboAction comboAction = (ComboAction)action;

        List<ActionType> actionTypes = new List<ActionType>();
        foreach (Action subAction in comboAction.children)
        {
            actionTypes.Add(subAction.type);
        }
        global.ui.ShowActionsPanel(unit, action.tile, actionTypes); //// placeholder
    }
    
    public void Fortify(Unit unit)
    {
        unit.Fortify();
        playerInput.PostAction(unit);
    }



}

