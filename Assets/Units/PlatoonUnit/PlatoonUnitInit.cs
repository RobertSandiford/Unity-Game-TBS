using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Unit : MonoBehaviour
{
       public Unit()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        
        //Debug.Log("Unit start");

    }

    public void Awake()
    {
    
        global = (Global)FindObjectOfType<Global>();
        map = (Map)FindObjectOfType<Map>();
        ui = (UI)FindObjectOfType<UI>();
    }

    void Update()
    {
        if (!started) GameStart();
    }

    void GameStart()
    {
        SetMaterial();
        SetTeamColor();

        Setup3dModel();

        if ( ! inCargo ) MoveToCurrentTile();
        //ShowMoveOptions();

        started = true;
    }

    public void Initialise()
    {
        SetNames();

        visibleTiles = new List<Tile>();
        visibleEnemyUnits = new List<Unit>();
        enemiesWithVisionOnThis = new List<Unit>();
        availableMoves = new List<Move>();
        cargo = new Cargo(squadDefs[0].cargoDef);
        unitUi = (UnitUI)gameObject.transform.Find("UnitCanvas").GetComponent<UnitUI>();
        unitUi.Initialise();
        UpdateCargoText();


        if (team == 1) visible = true;

        //if (squadDef.minAltitude > 0)
        //    altitude = Math.Max(squadDef.minAltitude, map.GetMinTileAlt(tile));
        //else
        altitude = 0;

        squads = new List<Squad>();
        foreach (SquadDef sd in squadDefs) {
            var squad = new Squad(sd, this); // make the squad object
            squads.Add(squad); // add squad to squads lists in unit
        }

        SetTargetType();
        SetCountermeasures();
        SetHitability();

    }

    void SetNames()
    {
        if (platoonGroupDef != null)
        {
            unitName = platoonGroupDef.name;
            unitShortName = platoonGroupDef.shortName;
            unitVeryShortName = ""; // not needed unless we use combined platoons, then maybe not even
        }
        else
        {
            if (platoonDefs.Count > 0)
            {
                unitName = platoonDefs[0].name;
                unitShortName = platoonDefs[0].shortName;
                unitVeryShortName = platoonDefs[0].veryShortName;
            }
            foreach (SquadDef sd in attachedSquadDefs)
            {
                if (unitName != "") unitName += " + ";
                unitName += sd.attachmentName;
                unitShortName += sd.attachmentShortName;
                unitVeryShortName = sd.attachmentVeryShortName;
            }

        }

        //Debug.Log("Unit Name: " + unitName);
        //Debug.Log("Unit Short Name: " + unitShortName);
        //Debug.Log("Unit Very Short Name: " + unitVeryShortName);
    }

    void SetTargetType()
    {
        // improve this
        targetType = platoonDefs[0].targetType;
    }

    void SetCountermeasures()
    {
        countermeasures = squadDefs[0].countermeasures;
    }

    void SetHitability()
    {
        hitability = squadDefs[0].hitability;
    }

    void Setup3dModel()
    {
        /*switch (squadDef.name) {
            case "BMP-2":
                _3dModel = Instantiate( (GameObject)Resources.Load("Models/Dummy IFV/Dummy_IFV_Prefab") );
                _3dModel.transform.SetParent(this.gameObject.transform);
                break;
            case "PAK40 75mm":
                //Debug.Log("Found a PAK");
                _3dModel = Instantiate( (GameObject)Resources.Load("Models/6Pounder/6Pounder_Prefab") );
                _3dModel.transform.SetParent(this.gameObject.transform);
                break;
        }*/
    }

    private void SetMaterial()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        switch (squadDefs[0].unitClass)
        {
            case UnitClass.Infantry:
            case UnitClass.Mortar:
            case UnitClass.Artillery:
            case UnitClass.Gun:
                meshRenderer.material = materials[1];
                break;
            case UnitClass.Car:
            case UnitClass.Truck:
            case UnitClass.Apc:
            case UnitClass.WheeledApc:
            case UnitClass.ApcMortar:
            case UnitClass.WheeledMortar:
            case UnitClass.WheeledAa:
                meshRenderer.material = materials[2];
                break;
            case UnitClass.Ifv:
            case UnitClass.TrackedApc:
            case UnitClass.Carrier:
            case UnitClass.HalftrackOpen:
            case UnitClass.Halftrack:
            case UnitClass.MechMortar:
            case UnitClass.MechArtillery:
            case UnitClass.TrackedAa:
                meshRenderer.material = materials[3];
                break;
            case UnitClass.Tank:
            case UnitClass.PanzerJager:
            case UnitClass.Spat:
                meshRenderer.material = materials[4];
                break;
            case UnitClass.Helicopter:
                meshRenderer.material = materials[5];
                break;
        }
        
    }

    private void SetTeamColor()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Color color = new Color(0.4f, 0.7f, 1.0f, 1.0f);
        if (team == 1)
            color = new Color(0.4f, 0.7f, 1.0f, 1.0f);
        //meshRenderer.material.SetColor("_Color", new Color(0.2f, 0.5f, 0.9f, 1.0f));
        if (team == 2)
            color = new Color(1.0f, 0.5f, 0.25f, 1.0f);

        color = color + groupColor;
        meshRenderer.material.SetColor("_Color", color);

    }

}
