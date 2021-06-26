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

public enum ActionMode
{
    Normal,
    Dropoff,
    Dismount
}

public class PlayerInput : MonoBehaviour
{

    public Global global;
    private Map map;
    private UI ui;

    public Moves moves;
    public Artillery artillery;
    //private Moves buttons;

    //public GameObject selectedUnitObject;
    public Unit selectedUnit;

    private GameObject activeMoveTarget;
    private GameObject activeLoadTarget;
    private GameObject activeUnloadTarget;
    private GameObject activeArtilleryTarget;
    private GameObject activeActionIndicator = null;

    private GameObject pinnedDropoffIndicator = null;

    
    [SerializeField] private GameObject pathIndicatorPrefab = null;

    [SerializeField] private GameObject moveTargetObject = null;
    [SerializeField] private GameObject activeMoveTargetObject = null;

    [SerializeField] private GameObject loadTargetObject = null;
    [SerializeField] private GameObject activeLoadTargetObject = null;

    [SerializeField] private GameObject unloadTargetObject = null;
    [SerializeField] private GameObject activeUnloadIndicatorPrefab = null;

    [SerializeField] private GameObject pickUpTargetObject = null;
    [SerializeField] private GameObject activePickupTargetObject = null;

    [SerializeField] private GameObject dismountTargetObject = null;
    [SerializeField] private GameObject activeDismountTargetObject = null;

    [SerializeField] public GameObject activeArtilleryTargetObject = null;
    private bool initialised = false;
    public Material hexPathMaterial;
    public bool showingLos = false;

    public List<Tile> unloadOptions;
    private List<GameObject> unloadTargets;
    
    private Dictionary<int, List<GameObject>> pathIndicators = null;
    //private List<GameObject> actionIndicators;
    private Dictionary<int, List<GameObject>> actionIndicators;


    //public bool enableDismountChoice = false;
    public Action dropoffAction = null;
    public List<Action> dismountActions;

    public Tile hoverTile = null;

    public bool arialMoveSelect = false;
    public Action arialMoveAction;

    public int targetAltitude = 0;

    public ActionMode actionMode = ActionMode.Normal;
    public int actionTier = 1;
    
    int UILayer;

    /*
    * Start() - basic init
    * Update() - Check and do 2nd init, run  CheckLeftClick CheckRightClick HoverCheck and CheckLosToggle
    * Initialise - 2nd init
    * CheckLeftClick - check if left mouse is pressed, and select a unit clicked on if possible
    * CheckRightClick() - check if right mouse is pressed, and run RightClickedHex if it was clicked on a hex
    * RightClickedHex() - process a right click on a hex, doing moves/actions, firing, or artillery ordering
    */

    private void Awake()
    {
        global = (Global)FindObjectOfType<Global>();
        global.moves = moves = new Moves(this); 
        global.artillery = artillery = new Artillery(this);

        actionIndicators = new Dictionary<int, List<GameObject>>();
        pathIndicators = new Dictionary<int, List<GameObject>>();

        UILayer = LayerMask.NameToLayer("MainUI");
    }

    // Start is called before the first frame update
    void Start()
    {
        unloadTargets = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialised) Initialise();

        CheckLeftClick();
        CheckRightClick();
        CheckHover();
        //UpdateActiveMoveTarget();
        //UpdateActiveLoadTarget();
        //UpdateActiveUnloadTarget();
        CheckLosToggle();
        
    }

    void Initialise()
    {
        map = (Map)FindObjectOfType<Map>();
        ui = (UI)FindObjectOfType<UI>();
        initialised = true;
    }
    

    public bool IsPointerOverUIElement()
    {
        //return IsPointerOverUIElement(GetEventSystemRaycastResults());

        List<RaycastResult> eventSystemRaycastResults = GetEventSystemRaycastResults();
        for (int index = 0; index < eventSystemRaycastResults.Count; index++)
        {
            RaycastResult curRaycastResult = eventSystemRaycastResults[index];
            if (curRaycastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }

    //Returns 'true' if we touched or hovering on Unity UI element.
    //private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaycastResults)
    //{
    //    for (int index = 0; index < eventSystemRaycastResults.Count; index++)
    //    {
    //        RaycastResult curRaycastResult = eventSystemRaycastResults[index];
    //        if (curRaycastResult.gameObject.layer == UILayer)
    //            return true;
    //    }
    //    return false;
    //}
 
 
    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }

    void CheckLeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("mouse click");

            if (IsPointerOverUIElement()) // the mouse is over the UI, so do nothing, let the UI handle it
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 999999))
            {
                //Debug.Log("hit");
                //Debug.Log("Hit object: " + hit.transform.gameObject.name);
                if ( hit.transform.gameObject.name.Contains("UnitDisc") || hit.transform.gameObject.name.Contains("UnitObject") )
                {
                    Unit unit = (Unit)(hit.transform.gameObject.GetComponent(typeof(Unit)));
                    DoLeftClickUnit( unit );
                }
                else if ( hit.transform.gameObject.name.Contains("FortDisc") )
                {
                    DoLeftClickTile( GetTileFromFortHit(hit) );
                }
                else if (hit.transform.gameObject.name.Contains("Hex"))
                {
                    DoLeftClickTile( GetTileFromHexHit(hit) );
                }
                else
                {
                    DoLeftClickNone();
                }

            }
        }
    }

    void DoLeftClickNone()
    {
        if (selectedUnit != null)
        {
            DeselectUnit();
        }
    }

    void DoLeftClickUnit(Unit unit)
    {
        if (unit != selectedUnit)
        {
            SelectUnit(unit);
        }
    }

    void DoLeftClickTile(Tile tile)
    {
        if (tile.unit != null) // a Unit clicked
        {
            DoLeftClickUnit(tile.unit);
        }
        else
        {
            DeselectUnit();
        }
    }

    void CheckRightClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //Debug.Log("right mouse click");
            if (selectedUnit != null)
            {

                Unit unitScript = selectedUnit;
                /*if (unitScript.actionPoints > 0)
                {*/
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 999999))
                    {

                        //Debug.Log("Right Click Hit object: " + hit.transform.gameObject.name);
                        if (hit.transform.gameObject.name.Contains("Hex"))
                        {
                            GameObject hex = hit.transform.gameObject;
                            Hex hexScript = (Hex)(hex.GetComponent(typeof(Hex)));
                            RightClickedHex(hexScript.tile);
                        }

                        //if (hit.transform.gameObject.name.Contains("UnitDisc")) RightClickUnitDisc(hit);
                        if ( hit.transform.gameObject.name.Contains("UnitDisc") || hit.transform.gameObject.name.Contains("UnitObject") ) RightClickedHex(GetTileFromUnitHit(hit));
                        if ( hit.transform.gameObject.name.Contains("FortDisc") ) RightClickedHex(GetTileFromFortHit(hit));

                    }
                //}
            }
        }
    }

    // RightClickedHex() - process a right click on a hex, doing moves/actions, firing, or artillery ordering
    void RightClickedHex(Tile tile)
    {
        Unit unitScript = selectedUnit;

        if ( selectedUnit && selectedUnit.BelongsToPlayer() )
        {
            if ( (actionMode == ActionMode.Normal || actionMode == ActionMode.Dropoff) && selectedUnit.CanAct() && TileInActions(tile, selectedUnit.availableActions) )
            {
                Action action = GetActionAtTile(tile, selectedUnit.availableActions);
                moves.DoAction(selectedUnit, action);
                //PostAction(selectedUnit, action);
            }
            else if ( actionMode == ActionMode.Dismount && selectedUnit.CanAct() && TileInActions(tile, dismountActions) )
            {
                Action dismountAction = GetActionAtTile(tile, dismountActions);
                moves.DoDropoffAndDismount(selectedUnit, dropoffAction, dismountAction);
            }
            else if ( selectedUnit.CanAct() && artillery.artilleryTargetingMode )
            {
                artillery.ArtilleryTargetTile(tile);
            }
            else if ( selectedUnit.CanFire() && tile.unit && unitScript.UnitIsEnemy(tile.unit) )
            {
                Unit targetUnit = tile.unit;
                if (unitScript.CanShootTarget(targetUnit))
                {
                    //int weaponId = unitScript.BestUsableWeaponAgainstTarget(targetUnit);\
                    //unitScript.PlayerShoot(targetUnit, weaponId);\
                    unitScript.PlayerShootAutoWeapons(targetUnit);
                    PostAction(selectedUnit, new Action(selectedUnit, ActionType.Shoot, tile));
                }
            }
            //// test firing
            //else if (selectedUnit.CanFire())
            //{
            //    unitScript.PlayerShootAutoWeaponsAtTile(tile);
            //}
            //// end test firing
        }
    }


    // tests whether a raycast hit a hex, and populates 'hitTile' which was passed by ref, if it was
    //public bool WasHexHit (RaycastHit hit, ref Tile hitTile)
    //{
    //    if (hit.transform.gameObject.name.Contains("Hex"))
    //    {
    //        GameObject hex = hit.transform.gameObject;
    //        Hex hexScript = (Hex)(hex.GetComponent(typeof(Hex)));
    //        hitTile = hexScript.tile;
    //        return true;
    //    }
        
    //    if ( hit.transform.gameObject.name.Contains("UnitDisc") || hit.transform.gameObject.name.Contains("UnitObject") )
    //    {
    //        hitTile = GetTileFromUnitHit(hit);
    //        return true;
    //    }

    //    if (hit.transform.gameObject.name.Contains("FortDisc"))
    //    {
    //        hitTile = GetTileFromFortHit(hit);
    //        return true;
    //    }

    //    return false;
    //}


    // Each frame, is run to check whether the mouse is over a tile
    void CheckHover()
    {
        //if (selectedUnitObject != null)
        //{
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 999999))
            {

                if ( hit.transform.gameObject.name.Contains("UnitDisc") || hit.transform.gameObject.name.Contains("UnitObject") )
                {
                    /*GameObject targetUnitObject = hit.transform.gameObject;
                    Unit targetUnit = (Unit)(targetUnitObject.GetComponent(typeof(Unit)));

                    HoverTile(targetUnit.tile);*/

                    HoverTile(GetTileFromUnitHit(hit));
                }

                if (hit.transform.gameObject.name.Contains("FortDisc"))
                {

                    /*GameObject targetFortObject = hit.transform.gameObject;
                    Unit targetFort = (Fort)(targetFortObject.GetComponent(typeof(Fort)));*/

                    HoverTile(GetTileFromFortHit(hit));
                }

                if (hit.transform.gameObject.name.Contains("Hex"))
                {
                    GameObject hex = hit.transform.gameObject;
                    Hex hexScript = (Hex)(hex.GetComponent(typeof(Hex)));

                    HoverTile(hexScript.tile);
                }

            }
        //}
    }

    // If the mouse is over a tile each frame, calls this function to check for mouseOver and mouseOut, and calls those funcs
    void HoverTile(Tile tile)
    {
        if (hoverTile != tile)
        {
            if (hoverTile != null)
            {
                MouseOut(hoverTile);
            }
            hoverTile = tile;
            MouseOver(tile);
        }
        Hover(tile);
    }

    // Runs to perform actions with the Tile being hovered over, each frame
    void Hover(Tile tile)
    {
        //ActionsHover(tile);
    }

    /*void ActionsHover(Tile tile)
    {

    }*/

    // Tile moused in
    void MouseOver(Tile tile)
    {
        CheckHoverTargetingUpdate(); // move to this on tile change
        ActionsMouseOver(tile);
        ShowVisionFromTileOnMouseOver(tile);
        artillery.ArtilleryTargetingMouseOver(tile);

    }
    
    // Tile moused out
    void MouseOut(Tile tile)
    {
        ActionsMouseOut(tile);
        artillery.ArtilleryTargetingMouseOut(tile);
    }

    // Deals with Action's (Move, Load etc) for mouse in
    void ActionsMouseOver(Tile tile)
    {
        // tile is in actions
        if ( selectedUnit != null && selectedUnit.BelongsToPlayer() )
        {

            if ( actionMode == ActionMode.Normal || actionMode == ActionMode.Dropoff ) {
                if ( TileInActions(tile, selectedUnit.availableActions) )
                {
                    Action action = GetActionAtTile(tile, selectedUnit.availableActions);
                    HighlightAction(action);

                    if ( action.type == ActionType.Dropoff ) {
                        // For dropoff actions, show dismount options
                        List<Action> dismountActions = ((DropoffAction)action).GetDismountActions();
                        ShowTheseActions(selectedUnit, dismountActions, 2);
                    }

                    ShowTargetingAccuracyOnActionHover( action, tile );

                }
                else
                {
                    ClearActionHighlights();
                }
            }

            if ( actionMode == ActionMode.Dismount )
            {
                int tier = 2;
                if ( TileInActions(tile, dismountActions) ) {
                    Action action = GetActionAtTile(tile, dismountActions);
                    HighlightAction(action, 2);
                }
                else
                {
                    ClearActionHighlights(tier);
                }
            }

        }

    }

    void HighlightAction(Action action, int tier = 1)
    {
        GameObject objType = null;
        switch (action.type) {
            case ActionType.Move:
                objType = activeMoveTargetObject;
                break;
            case ActionType.ArialMove:
                objType = activeMoveTargetObject;
                break;
            case ActionType.Load:
                objType = activeLoadTargetObject;
                break;
            case ActionType.Unload:
                objType = activeUnloadIndicatorPrefab;
                break;
            case ActionType.Pickup:
                objType = activePickupTargetObject;
                break;
            case ActionType.Dropoff:
                objType = activeUnloadIndicatorPrefab;
                break;
            case ActionType.Dismount:
                objType = activeDismountTargetObject;
                break;
            case ActionType.Combo:
                objType = activeUnloadIndicatorPrefab;
                break;
        }

        activeActionIndicator = Instantiate(objType);
        PositionActionIndicatorOnTile(activeActionIndicator, action.tile, action.type);

        if ( action.type == ActionType.Move || action.type == ActionType.Dropoff || action.type == ActionType.Dismount ) {
            ShowPath(action, tier);
        }
    }

    public void ShowPath(Action action, int tier = 1)
    {
        DestroyPathIndicators(tier);
        List<GameObject> thesePathIndicators = pathIndicators[tier] = new List<GameObject>();
        for (int i = 0; i < action.path.Length-1; i++)
        {
            Tile pathTile = action.path[i];
            GameObject targetObj = pathIndicatorPrefab;
            //if (i == special) targetObj = activePickupTargetObject;
            GameObject pathMarker = Instantiate(targetObj);
            PositionActionIndicatorOnTile(pathMarker, pathTile, action.type);
            thesePathIndicators.Add(pathMarker);
        }
    }

    void ClearActionHighlights(int tier = 1)
    {
        HideTargetingAccuracy();
        DestroyPathIndicators(tier);
    }

    public void PinDropoffIndicator(Action action)
    {
        pinnedDropoffIndicator = Instantiate(activeUnloadIndicatorPrefab);
        PositionActionIndicatorOnTile(pinnedDropoffIndicator, action.tile, action.type);
    }

    public void DeletePinnedDropoffIndicator()
    {
        Destroy(pinnedDropoffIndicator);
        pinnedDropoffIndicator = null;
    }


    // When hovering over a possible Action (move), show targeting accuracy if appropriate
    void ShowTargetingAccuracyOnActionHover(Action action, Tile tile) {

        //Debug.Log("ShowTargetingAccuracyOnActionHover");

        switch (action.type) {
            case ActionType.Move:
            case ActionType.Dropoff:
                //case ActionType.Pickup:
                //case ActionType.Dropoff:

                double cost = action.cost;
                MovementData md = selectedUnit.MovementData();
                double frac = cost / md.moves;
                //Debug.Log("Cost: " + cost + " / frac: " + frac);
                double acc = ShootingAccuracyAfterMove(action.cost, selectedUnit.MovementData());
                //Debug.Log( "Acc: " + Funcs.Percentage(acc) );

                ui.ShowTargetingAccuracyForAction( action, tile, acc );

                break;
        }

    }

    public void HideTargetingAccuracy()
    {
        global.ui.HideTargetingAccuracy();
    }

    public double ShootingAccuracyAfterMove(double cost, MovementData md) {
        double frac = cost / md.moves;
        return 1 - (0.8 * frac);
    }

    // Post action event, simply called PostAction below
    public void PostAction(Unit unit, Action action)
    {
        PostAction(unit);
    }

    // Post action event
    public void PostAction(Unit unit)
    {
        unit.availableActions = new List<Action>();
        //DestroyActionIndicators();
        DestroyAllActionIndicators();
        DestroyActiveActionIndicator();
        //DestroyPathIndicators();
        DestroyAllPathIndicators();
        global.ui.HideActionsPanel();
        global.ui.HideTargetingAccuracy();

        // show new actions????
        unit.Select();
    }


    // Destroy indicators showing the movement path for an action
    public void DestroyPathIndicators(int tier = 1) {
        if ( pathIndicators.ContainsKey(tier)) {
            List<GameObject> thesePathIndicators = pathIndicators[tier];
            foreach (GameObject pathTarget in thesePathIndicators) {
                Destroy(pathTarget);
            }
            thesePathIndicators = new List<GameObject>();
        }
    }
    
    // Destroy all indicators showing the movement path for an action
    public void DestroyAllPathIndicators() {
        for (int tier = 1; tier <= 2; tier++)
        { 
            if ( pathIndicators.ContainsKey(tier) )
            {
                DestroyPathIndicators(tier);
            }
        }
    }

    // Decide what vision to show when a tile is moused over - vision from tile, vision from select unit, or none.
    // showingLos = true - show from tile
    // tile in a possible move for select unit - show from tile
    // selected unit, none of above - show from unit
    // otherwise - none
    void ShowVisionFromTileOnMouseOver(Tile tile)
    {
        //Debug.Log("<color=orange>Mouse Over - Show Vision From Tile</color>");
        if (!arialMoveSelect) // handled seperately via mouse enter events
        {
            if ( selectedUnit != null && selectedUnit.BelongsToPlayer() )
            {
                // tile is in actions || we are always showing LOS
                if ( (actionMode == ActionMode.Normal || actionMode == ActionMode.Dropoff) && TileInActions(tile, selectedUnit.availableActions) )
                {
                    //Debug.Log(targetAltitude);
                    selectedUnit.ShowVisionAndTargets(tile, targetAltitude);
                }
                else if ( actionMode == ActionMode.Dismount && TileInActions(tile, dismountActions) )
                {
                    //Debug.Log(targetAltitude);
                    selectedUnit.ShowVisionAndTargets(tile, targetAltitude);
                }
                else if (showingLos)
                {
                    selectedUnit.ShowVisionAndTargets(tile, targetAltitude);
                }
                else
                {
                    selectedUnit.ShowVisionAndTargets();
                }
            }
            else
            {
                if (showingLos)
                {
                    //Debug.Log("Do Map Show Vision");
                    //map.ShowVision(tile);
                    map.GetVisionFor(tile, 0);
                }
            }
        }

    }




    // position the "obj" ball GameObject on the tile
    void PositionActionIndicatorOnTile(GameObject obj, Tile tile, ActionType type)
    {
        Vector3 pos = tile.position;

        float yOffset = 0.0f;
        switch(type)
        {
            case ActionType.Load:
                yOffset = 0.2f;
                break;
            case ActionType.Pickup:
                yOffset = 0.2f;
                break;
        }
        pos.y += yOffset;

        obj.transform.position = pos;
    }

    public bool TileInActions(Tile tile, List<Action> actions)
    {
        foreach (Action action in actions)
        {
            if (action.tile == tile) return true;
        }
        return false;
    }

    public Action GetActionAtTile(Tile tile, List<Action> actions)
    {
        foreach (Action action in actions)
        {
            if (action.tile == tile) return action;
        }
        return new Action();
    }

    void ActionsMouseOut(Tile tile)
    {
        //Debug.Log("Actions mouse out");
        if (activeActionIndicator != null)
        {
            DestroyActiveActionIndicator();
        }
        if (actionMode == ActionMode.Dropoff)
        {
            //Debug.Log("Destroying tier 2 action indicators");
            DestroyActionIndicators(2);
        }
    }

    public void MouseEnterAltitudeButton(/*BaseEventData data, */GameObject go)
    {
        Global global = (Global)FindObjectOfType<Global>();
        global.playerInput.MouseEnterAltitudeButton2(go);
        //Debug.Log("Mouse Enter 1");
    }

    public void MouseEnterAltitudeButton2(/*BaseEventData data, */GameObject go)
    {

        //Debug.Log("Mouse Enter");

        int height = int.Parse(go.transform.Find("Height").GetComponent<Text>().text);

        //Debug.Log(height);
        //Debug.Log(arialMoveAction.tile.x);
        //Debug.Log(selectedUnit.tile.x);


        selectedUnit.ShowVisionAndTargets(arialMoveAction.tile, height - arialMoveAction.tile.y);

        //PointerEventData pointerData = data as PointerEventData;
        //Debug.Log(pointerData);
    }

    public void DestroyActiveActionIndicator()
    {
        Destroy(activeActionIndicator);
        activeActionIndicator = null;
    }


    void CheckLosToggle()
    {
        //Debug.Log("check los button");
        if (Input.GetKeyDown("space"))
        {
            showingLos = (showingLos) ? false : true;
            if (showingLos)
            {
                map.ResetTeamVision();
                if (hoverTile != null) ShowVisionFromTileOnMouseOver(hoverTile);
            }
            else {
                if (selectedUnit != null) {
                    selectedUnit.ShowVisionAndTargets();
                }
                else {
                    map.ResetUnitVision();
                    map.ShowTeamVision(1);
                }

            }
            ui.SetShowingLos(showingLos);
        }
    }


    public Tile GetTileFromHexHit(RaycastHit hit)
    {
        return ( (Hex)hit.transform.gameObject.GetComponent<Hex>() ).tile;
    }

    public Tile GetTileFromUnitHit(RaycastHit hit)
    {
        return ( (Unit)hit.transform.gameObject.GetComponent<Unit>() ).tile;
    }

    public Tile GetTileFromFortHit(RaycastHit hit)
    {
        return ((Fort)hit.transform.gameObject.GetComponent<Fort>()).tile;
    }

    void CheckHoverTargetingUpdate()
    {
        bool debugThis = false;

        if (selectedUnit != null)
        {

            //if (debugThis) Debug.Log("targ1");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 999999))
            {

                if (debugThis) Debug.Log("Hover Hit object: " + hit.transform.gameObject.name);

                bool showTargetingPanel = false;

                Tile tile = null;
                if ( hit.transform.gameObject.name.Contains("UnitDisc") || hit.transform.gameObject.name.Contains("UnitObject") ) // unit
                {
                    tile = GetTileFromUnitHit(hit);
                }
                else if ( hit.transform.gameObject.name.Contains("Fort") ) // fort
                {
                    tile = GetTileFromFortHit(hit);
                }
                else if ( hit.transform.gameObject.name.Contains("Hex") ) // hex
                {
                    tile = GetTileFromHexHit(hit);
                }

                if ( tile != null )
                {
                    if ( tile.unit != null )
                    {
                        Unit targetUnit = tile.unit;
                        GameObject targetUnitObject = tile.unit.gameObject;

                        Unit thisUnit = selectedUnit;

                        if (debugThis) Debug.Log("targ4");

                        if ( thisUnit.UnitIsEnemy(targetUnit) )
                        {
                     

                            if (debugThis) Debug.Log("Checking vision to target on hover");
                            if ( thisUnit.CanShootTarget(targetUnit, true) )
                            {
                                List<FiringWeapon> fws = thisUnit.SelectFiringWeapons(targetUnit);

                                if (debugThis) {
                                    Debug.Log("Firing Weapons:");
                                    foreach (FiringWeapon fw in fws)
                                    {
                                        Debug.Log(fw.number + " " + fw.uWeapon.weapon.name + " (" + fw.expDamage + ")" );
                                    }
                                }

                                if (debugThis) Debug.Log("Can shoot, show target ui");
                                showTargetingPanel = true;
                                ui.UpdateTargetingPanelTarget(thisUnit, targetUnit);
                                ui.ShowTargetingPanel();
                            }
                        }
                    }
                }

                if ( ! showTargetingPanel )
                {
                    ui.HideTargetingPanel();
                }

            }
        }

    }



    
    public void SelectUnit(Unit unit) {
        //Debug.Log("<color=red>Select a Unit " + unit.name + "</color>");

        if (selectedUnit != null)
        {
            DeselectUnit();
        }
        
        //Debug.Log("Selecting");

        //selectedUnitObject = unit.gameObject;
        selectedUnit = unit;

        selectedUnit.Select();
        
        targetAltitude = selectedUnit.altitude;
        
        if ( selectedUnit.isPackedUp && ! selectedUnit.isSettingUp ) ShowActions(selectedUnit);
    }

    public void SelectUnit(GameObject unitObject)
    {
        SelectUnit( (Unit)unitObject.GetComponent(typeof(Unit)) );
    }

    public void RefreshSelectedUnit()
    {
        if (selectedUnit != null)
        {
            SelectUnit(selectedUnit);
        }
    }




    public bool TileInUnloadOptions(Tile tile)
    {
        foreach (Tile move in unloadOptions)
        {
            if (move == tile)
            {
                return true;
            }
        }
        return false;
    }

    public void ShowUnloadOptions()
    {
        DestroyUnloadOptions();
            
        foreach (Tile move in unloadOptions)
        {
            //Debug.Log("Showing unload target");
            GameObject target = Instantiate(unloadTargetObject);
            target.transform.position = move.position;
            unloadTargets.Add(target);
        }
             
    }

    public void DestroyUnloadOptions()
    {
        foreach (GameObject target in unloadTargets) {
            Destroy(target);
        }
        unloadTargets = new List<GameObject>();
    }

    public bool MoveInUnloadOptions(Tile tile)
    {
        foreach (Tile move in unloadOptions)
        {
            if (tile == move)
            {
                return true;
            }
        }
        return false;
    }

    void DeselectUnit()
    {
        if (selectedUnit)
        {
            //Debug.Log("Deselecting");

            selectedUnit.Deselect();

            DestroyAllActionIndicators();
            DestroyAllPathIndicators();
            DestroyActiveActionIndicator();

            ui.HideActionsPanel();
            ui.HideTargetingAccuracy();
            artillery.ArtilleryDeselectUnit();
            global.ui.DestroyAltitudePanel();
            arialMoveSelect = false;

            selectedUnit = null;
        }
    }

    public void ButtonActionMove()
    {
        Unit unit = selectedUnit;
        Debug.Log("Button Clicked Move");
        unit.PlayerMoveTo(ui.actionsPanelTile);
        //ui.HideActionsPanel();
        //ui.HideTargetingAccuracy();
        PostAction(unit);
    }

    public void ButtonActionUnload()
    {
        Debug.Log("Button Clicked Unload");
        Unit unit = selectedUnit;
        unit.PlayerUnloadTo(ui.actionsPanelTile);
        //ui.HideActionsPanel();
        //ui.HideTargetingAccuracy();
        PostAction(unit);
    }



    //////////////////////////////////////////
    /// Action Targets
    //////////////////////////////////////////


    public void ShowActions(Unit unit, int remainingMoves = -1)
    {
        if (unit.team == 1) {
            List<Action> actions = (remainingMoves == -1) ? unit.availableActions = GetActions(unit) : unit.availableActions = GetActions(unit, remainingMoves) ;
            ShowTheseActions(unit, actions);
        }

    }
    

    public void ShowTheseActions(Unit unit, List<Action> actions, int tier = 1)
    {
        DestroyActionIndicators(tier);

        foreach (Action action in actions)
        {
            GameObject actionIndicatorObj = null;
            //float yOffset = 0.0f;

            switch (action.type)
            {
                case ActionType.Move:
                    actionIndicatorObj = moveTargetObject;
                    break;
                case ActionType.ArialMove:
                    actionIndicatorObj = moveTargetObject;
                    break;
                case ActionType.Load:
                    actionIndicatorObj = loadTargetObject;
                    //yOffset = 0.2f;
                    break;
                case ActionType.Unload:
                    actionIndicatorObj = unloadTargetObject;
                    break;
                case ActionType.Pickup:
                    actionIndicatorObj = pickUpTargetObject;
                    break;
                case ActionType.Dropoff:
                    actionIndicatorObj = unloadTargetObject;
                    break;
                case ActionType.Dismount:
                    actionIndicatorObj = dismountTargetObject;
                    break;
                case ActionType.Combo:
                    actionIndicatorObj = unloadTargetObject; //// placeholder
                    break;
            }

            if (actionIndicatorObj != null) //// safeguard
            {
               
                GameObject indicator = Instantiate(actionIndicatorObj);
                PositionActionIndicatorOnTile(indicator, action.tile, action.type);
                actionIndicators[tier].Add(indicator);
            }
            else
            {
                Debug.Log("Error: no action target object to create");
            }
        }
    }

    void MergeInActions(ref Dictionary<string, Action> actions, List<Action> newActions)
    {
        foreach (Action action in newActions)
        {
            string k = action.tile.x.ToString() + "-" + action.tile.z.ToString();
            if (actions.ContainsKey(k))
            {
                actions[k] = MergeActions(actions[k], action);
            }
            else
            {
                actions[k] = action;
            }
        }
    }

    public List<Action> GetActions(Unit unit, int remainingMoves = -1)
    {
        return unit.GetActions();
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

    public void DestroyActionIndicators(int tier = 1)
    {
        if (actionIndicators.ContainsKey(tier))
        {
            foreach (GameObject indicator in actionIndicators[tier])
            {
                Destroy(indicator);
            }
        }
        actionIndicators[tier] = new List<GameObject>();
    }

    public void DestroyAllActionIndicators()
    {
        for (int tier = 1; tier <= 2; tier++)
        {
            if (actionIndicators.ContainsKey(tier))
            {
                foreach (GameObject indicator in actionIndicators[tier])
                {
                    Destroy(indicator);
                }
            }
            actionIndicators[tier] = new List<GameObject>();
        }
    }

    //    public void EnableArtilleryTargetingMode()
    //{
    //    artilleryTargetingMode = true;
    //    DestroyActionIndicators();
    //    artillerySalvos = 1;
    //    global.ui.ShowArtilleryControlPanel(artillerySalvos);
    //    artilleryTargetType = ArtilleryTargetType.Point;
    //    global.ui.SetArtilleryTargetType(artilleryTargetType);
    //    //Debug.Log("Arty Targeting Mode Enabled");
    //}

    //public void EndArtilleryTargetingMode()
    //{
    //    artilleryTargetingMode = false;
    //    artilleryTargeting = false;
    //    if (activeArtilleryTarget != null) Destroy(activeArtilleryTarget);
    //    DestroyArtilleryTarget();
    //    global.ui.HideArtilleryControlPanel();
    //    //Debug.Log("End Arty Mode");
    //    //Debug.Log("Arty Targeting Mode Ended");
    //}

    //void ArtilleryTargetTile(Tile targetTile)
    //{
    //    switch (artilleryTargetType)
    //    {
    //        case ArtilleryTargetType.Point:
    //            ArtilleryPointTargetTile(targetTile);
    //            break;
    //        case ArtilleryTargetType.Circle:
    //            ArtilleryCircleTargetTile(targetTile);
    //            break;
    //        case ArtilleryTargetType.Line:
    //            ArtilleryLineTargetTile(targetTile);
    //            break;
    //    }
    //}

    //void ArtilleryPointTargetTile(Tile targetTile)
    //{
    //    int range = map.GetGameRange(selectedUnit.tile, targetTile);
    //    UnitWeapon uWeapon = selectedUnit.GetIndirectWeapon();
    //    if (range <= uWeapon.weapon.indirectWeapon.range)
    //    {
    //        targetTile.hex.SetArtyTarget(true);
    //        //Debug.Log("Do Arty Attack Here");
    //        selectedUnit.PlayerArtilleryAttack(selectedUnit.GetIndirectWeapon(), new ArtilleryTarget(targetTile), artillerySalvos);
    //        EndArtilleryTargetingMode();
    //        PostAction(selectedUnit);
    //    }
    //}
    
        
        
    //void ArtilleryCircleTargetTile(Tile targetTile)
    //{
    //    int range = map.GetGameRange(selectedUnit.tile, targetTile);
    //    UnitWeapon uWeapon = selectedUnit.GetIndirectWeapon();

    //    if (!artilleryTargeting)
    //    {
    //        if (range <= uWeapon.weapon.indirectWeapon.range)
    //        {
    //            artilleryTargeting = true;
    //            artilleryTargetingStartTile = targetTile;
    //            targetTile.hex.SetArtyTarget(true);
    //        }
    //    }
    //    else
    //    {
    //        if (range <= uWeapon.weapon.indirectWeapon.range)
    //        {
    //            /////// check that all parts of circle are in range /////

    //            int radius = map.GetMoveDistance(artilleryTargetingStartTile, targetTile);
    //            if (radius >= 1 && radius <= 3)
    //            {
    //                artilleryTargeting = false;
    //                selectedUnit.PlayerArtilleryAttack(selectedUnit.GetIndirectWeapon(), new ArtilleryTarget(artilleryTargetingStartTile, radius), artillerySalvos);
    //                EndArtilleryTargetingMode();
    //                PostAction(selectedUnit);
    //            }
    //        }
    //    }
    //}

    //void ArtilleryLineTargetTile(Tile targetTile)
    //{
    //    int range = map.GetGameRange(selectedUnit.tile, targetTile);
    //    UnitWeapon uWeapon = selectedUnit.GetIndirectWeapon();

    //    if (!artilleryTargeting)
    //    {
    //        if (range <= uWeapon.weapon.indirectWeapon.range)
    //        {
    //            artilleryTargeting = true;
    //            artilleryTargetingStartTile = targetTile;
    //            targetTile.hex.SetArtyTarget(true);
    //        }
    //    }
    //    else
    //    {
    //        if (range <= uWeapon.weapon.indirectWeapon.range)
    //        {
    //            int dist = map.GetMoveDistance(artilleryTargetingStartTile, targetTile);
    //            if (dist >= 1 && dist <= 5)
    //            {
    //                if (
    //                    (targetTile.z == artilleryTargetingStartTile.z)
    //                    || 
    //                    (Math.Abs(targetTile.x - artilleryTargetingStartTile.x) == Math.Abs(targetTile.z - artilleryTargetingStartTile.z))
    //                )
    //                {
    //                    artilleryTargeting = false;
    //                    selectedUnit.PlayerArtilleryAttack(selectedUnit.GetIndirectWeapon(), new ArtilleryTarget(artilleryTargetingStartTile, targetTile), artillerySalvos);
    //                    EndArtilleryTargetingMode();
    //                    PostAction(selectedUnit);
    //                }
    //            }
    //        }
    //    }
    //}

    public void ButtonFortify()
    {
        if ( selectedUnit )
        {
            moves.Fortify(selectedUnit);
        }
    }

    public void ButtonSetup()
    {
        if (selectedUnit)
        {
            selectedUnit.Setup();
            DestroyActionIndicators();
        }
    }

    public void ButtonPackup()
    {
        if (selectedUnit)
        {
            selectedUnit.Packup();
            DestroyActionIndicators();
        }
    }

    public void ButtonAltitudePlus()
    {
        Unit u = selectedUnit;
        int maxAltitude = selectedUnit.MaxAltitude();
        if (targetAltitude + 1 <= maxAltitude)
        {
            int altGain = targetAltitude + 1 - selectedUnit.altitude;
            if (altGain <= selectedUnit.Moves() )
            {
                targetAltitude++;

                int remainingMoves = selectedUnit.GetRemainingMovesAfterAltChange(altGain);
                ShowActions(selectedUnit, remainingMoves);
            }
        }


        global.ui.UpdateAltitudeControl(selectedUnit, targetAltitude);
    }

    public void ButtonAltitudeMinus()
    {
        Unit u = selectedUnit;
        int minAltitude = selectedUnit.MinAltitude();
        if (targetAltitude - 1 >= minAltitude)
        {
            targetAltitude--;
            int altGain = targetAltitude - selectedUnit.altitude;
            int remainingMoves = selectedUnit.GetRemainingMovesAfterAltChange(altGain);
            ShowActions(selectedUnit, remainingMoves);
        }


        global.ui.UpdateAltitudeControl(selectedUnit, targetAltitude);
    }

    public void ButtonDropOff()
    {
        List<Action> dropoffActions = selectedUnit.GetDropoffActions();

        selectedUnit.availableActions = dropoffActions;
        ShowTheseActions(selectedUnit, dropoffActions);

        actionMode = ActionMode.Dropoff;
        actionTier = 1;
        
        ui.HideDropoffButton();
        ui.ShowCancelDropoffButton();
    }

    public void ButtonCancelDropOff()
    {
        // if in dropoff mode, go back to normal mode
        if (actionMode == ActionMode.Dropoff) {
            DestroyAllActionIndicators();
            moves.CancelDropoffDismountChoice();

            ShowActions(selectedUnit);

            actionMode = ActionMode.Normal;
            actionTier = 1;

            ui.HideCancelDropoffButton();
            ui.ShowDropoffButton();
        }
        // if in dismount mode, cancel this and go back to selecting dropoff
        if (actionMode == ActionMode.Dismount) {
            DestroyActionIndicators(2);
            List<Action> dropoffActions = selectedUnit.GetDropoffActions();

            moves.CancelDropoffDismountChoice();
            actionMode = ActionMode.Dropoff;
            actionTier = 1;

            selectedUnit.availableActions = dropoffActions;
            ShowTheseActions(selectedUnit, dropoffActions);
        }
    }



    
    public void ButtonArtillery()
    {
        artillery.ButtonArtillery();
    }

    public void ButtonSalvosPlus()
    {
        artillery.ButtonSalvosPlus();
    }

    public void ButtonSalvosMinus()
    {
        artillery.ButtonSalvosMinus();
    }

    public void ButtonArtilleryTargetPoint()
    {
        artillery.ButtonArtilleryTargetPoint();
    }

    public void ButtonArtilleryTargetCircle()
    {
        artillery.ButtonArtilleryTargetCircle();
    }

    public void ButtonArtilleryTargetLine()
    {
        artillery.ButtonArtilleryTargetLine();
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

