using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public struct PlatoonInfoPanel {
    public GameObject infoPanel;
    public GameObject textArea;
    public GameObject unitName;
    public GameObject squads;
    //public GameObject weapons;
    public GameObject weaponNums;
    public GameObject weaponNames;
    public GameObject weaponRanges;
    public GameObject weaponPens;
    public GameObject weaponAmmos;

    public PlatoonInfoPanel(GameObject InfoPanel) {
        infoPanel = InfoPanel;
        textArea = infoPanel.transform.Find("PlatoonInfoPanelText").gameObject;

        float lineHeight = 28f * 1.12f;

        float unitNameElHeight = lineHeight + 5f;
        unitName = UI.CreateUiTextElement("PlatoonUnitName", textArea, new Dictionary<string, object> {
            { "position", new Vector3(0, 0, 0) },
            { "size", new Vector2(364, unitNameElHeight) },
        });
        
        
        /*unitName2 = UI.CreateUiTextElement("PlatoonSquads2", textArea, new Dictionary<string, object> {
            //{ "position", new Vector3(0, -(squadsY+lineHeight), 0) },
            { "position", new Vector3(0, -lineHeight, 0) },
            { "size", new Vector2(364, lineHeight * 5) },
        });*/

                        // name space + 1 line
        float squadsY = lineHeight + lineHeight;
        int squadsLines = 5;
        float squadsElHeight = lineHeight * squadsLines + 5f;
        squads = UI.CreateUiTextElement("PlatoonSquads", textArea, new Dictionary<string, object> {
            { "position", new Vector3(0, -squadsY, 0) },
            { "size", new Vector2(364, squadsElHeight) },
        });


        // bottom of squads + 1 line
        float weaponsY = squadsY + squadsLines*lineHeight + lineHeight;
        int weaponsLines = 6;
        float weaponsElHeight = lineHeight * weaponsLines + 5f;
        /*weapons = UI.CreateUiTextElement("PlatoonWeapons", textArea, new Dictionary<string, object> {
            { "position", new Vector3(0, -weaponsY, 0) },
            { "size", new Vector2(364, weaponsElHeight) },
        });*/

        int weaponsSpacer = 5;
        int weaponsFontSize = 24;

        int numsX = 0;
        int numsWidth = 38;
        weaponNums = UI.CreateUiTextElement("PlatoonWeaponNums", textArea, new Dictionary<string, object> {
            { "position", new Vector3(numsX, -weaponsY, 0) },
            { "size", new Vector2(numsWidth, weaponsElHeight) },
            { "fontSize", weaponsFontSize }
        });

        int namesX = numsX + numsWidth + weaponsSpacer;
        int nameWidth = 200;
        weaponNames = UI.CreateUiTextElement("PlatoonWeaponNames", textArea, new Dictionary<string, object> {
            { "position", new Vector3(namesX, -weaponsY, 0) },
            { "size", new Vector2(nameWidth, weaponsElHeight) },
            { "fontSize", weaponsFontSize }
        });

        int rangesX = namesX + nameWidth + weaponsSpacer;
        int rangesWidth = 38;
        weaponRanges = UI.CreateUiTextElement("PlatoonWeaponRanges", textArea, new Dictionary<string, object> {
            { "position", new Vector3(rangesX, -weaponsY, 0) },
            { "size", new Vector2(rangesWidth, weaponsElHeight) },
            { "fontSize", weaponsFontSize }
        });

        int pensX = rangesX + rangesWidth + weaponsSpacer;
        int pensWidth = 38;
        weaponPens = UI.CreateUiTextElement("PlatoonWeaponPens", textArea, new Dictionary<string, object> {
            { "position", new Vector3(pensX, -weaponsY, 0) },
            { "size", new Vector2(pensWidth, weaponsElHeight) },
            { "fontSize", weaponsFontSize }
        });

        int ammosX = pensX + rangesWidth + weaponsSpacer;
        int ammoWidth = 38;
        weaponAmmos = UI.CreateUiTextElement("PlatoonWeaponAmmos", textArea, new Dictionary<string, object> {
            { "position", new Vector3(ammosX, -weaponsY, 0) },
            { "size", new Vector2(ammoWidth, weaponsElHeight) },
            { "fontSize", weaponsFontSize }
        });

    }

}

public class UI : MonoBehaviour
{

    bool initialised = false;
    Map map;
    Global global;

    public GameObject unitInfoPanel;
    public GameObject unitName;
    public GameObject soldiersNum;
    public GameObject armor;
    public GameObject weapons;

    public GameObject altitude;
    public GameObject height;

    public GameObject weapon1Num;
    public GameObject weapon1Name;
    public GameObject weapon1Range;
    public GameObject weapon1Pen;
    public GameObject weapon1Ammo;

    public GameObject weapon2Num;
    public GameObject weapon2Name;
    public GameObject weapon2Range;
    public GameObject weapon2Pen;
    public GameObject weapon2Ammo;

    public GameObject weapon3Num;
    public GameObject weapon3Name;
    public GameObject weapon3Range;
    public GameObject weapon3Pen;
    public GameObject weapon3Ammo;

    public GameObject weapon4Num;
    public GameObject weapon4Name;
    public GameObject weapon4Range;
    public GameObject weapon4Pen;
    public GameObject weapon4Ammo;

    public GameObject weapon5Num;
    public GameObject weapon5Name;
    public GameObject weapon5Range;
    public GameObject weapon5Pen;
    public GameObject weapon5Ammo;

    public GameObject weapon6Num;
    public GameObject weapon6Name;
    public GameObject weapon6Range;
    public GameObject weapon6Pen;
    public GameObject weapon6Ammo;

    private GameObject[] weaponNums;
    private GameObject[] weaponNames;
    private GameObject[] weaponRanges;
    private GameObject[] weaponDamages;
    private GameObject[] weaponPens;
    private GameObject[] weaponAmmos;

    public GameObject cargoLabel;
    public GameObject cargo;

    //public GameObject fortType;
    public GameObject fortName;
    public GameObject fortCover;

    // platoon info panel
    public PlatoonInfoPanel platoonInfoPanel;
    // GameObject platoonInfoPanelText;

    // targetting panel
    private GameObject targetingPanel;

    public GameObject targetingCrosshair;
    private Vector3 targetingCrosshiarInitialScale;
    //private GameObject targetingCrosshairTargetedUnit;
    private GameObject targetingCrosshairTargetedObject;

    public GameObject nextTurnButton;
    public GameObject fortifyButton;
    public GameObject setupButton;
    public GameObject packupButton;
    public GameObject artilleryButton;
    public GameObject dropoffButton;
    public GameObject cancelDropoffButton;

    public GameObject showingLosPanel;

    public GameObject actionsPanel;
    public Tile actionsPanelTile = null;
    public Dictionary<string, Action> actionsPanelActions = null;

    public GameObject airControlPanel;
    public GameObject airControlPanelAltitude;
    public GameObject altitudePanel;
    List<GameObject> altitudeSelectButtons;

    public GameObject artilleryControlPanel;
    public GameObject artilleryControlPanelSalvos;
    public GameObject artilleryControlPanelTargetPoint;
    public GameObject artilleryControlPanelTargetCircle;
    public GameObject artilleryControlPanelTargetLine;

    public GameObject artilleryStatusPanel;
    public GameObject artilleryStatusCancelButton;
    public GameObject artilleryStatusSalvos;
    public GameObject artilleryStatusSalvosRemaining;
    public GameObject artilleryStatusTargetType;

    public GameObject accuracyDisplayPanel;
    public GameObject accuracyDisplayText;

    


    // Start is called before the first frame update
    void Start()
    {
        weaponNums = new GameObject[] { weapon1Num, weapon2Num, weapon3Num, weapon4Num, weapon5Num, weapon6Num };
        weaponNames = new GameObject[] { weapon1Name, weapon2Name, weapon3Name, weapon4Name, weapon5Name, weapon6Name };
        weaponRanges = new GameObject[] { weapon1Range, weapon2Range, weapon3Range, weapon4Range, weapon5Range, weapon6Range };
        weaponPens = new GameObject[] { weapon1Pen, weapon2Pen, weapon3Pen, weapon4Pen, weapon5Pen, weapon6Pen, };
        weaponAmmos = new GameObject[] { weapon1Ammo, weapon2Ammo, weapon3Ammo, weapon4Ammo, weapon5Ammo, weapon6Ammo };
        // damages
        // ranges

        platoonInfoPanel = new PlatoonInfoPanel(gameObject.transform.Find("PlatoonInfoPanel").gameObject);


        targetingPanel = gameObject.transform.Find("TargetingPanel").gameObject;

        targetingCrosshair = gameObject.transform.Find("TargetingCrosshair").gameObject;
        targetingCrosshiarInitialScale = targetingCrosshair.transform.localScale;
        HideTargetingCrosshair(); // targetingCrosshair.SetActive(false);

        showingLosPanel = gameObject.transform.Find("LosIndicator").gameObject;

        HideTargetingPanel();
        HideActionsPanel();
        
        HideTargetingAccuracy();

        //HideSetupButton();
        //HidePackupButton();

        //HideArtilleryButton();

        HideUnitInfo();
        

        altitudeSelectButtons = new List<GameObject>();
        DestroyAltitudePanel();
    }
    
        
    //////////////// Unit UI
    //public void ShowUnitUI(Unit unit, SquadDef squadDef)
    public void ShowUnitUI(Unit unit)
    {
        unitInfoPanel.SetActive(true);
        platoonInfoPanel.infoPanel.SetActive(true);
        if (unit.CanFortify())
            fortifyButton.SetActive(true);
        else
            fortifyButton.SetActive(false);
        //ShowUnitTypeUI(unit, unit.squadDef);
        ShowUnitTypeUI(unit);
        UpdatePlatoonInfoPanel(unit);
        ShowHideSetupPackup(unit);
        ShowHideArtilleryButton(unit);
        ShowHideDropoffButton(unit);
        HideCancelDropoffButton();
        //ShowArtilleryControlPanel();
    }
    public void HideUnitInfo()
    {
        unitInfoPanel.SetActive(false);
        platoonInfoPanel.infoPanel.SetActive(false);
        fortifyButton.SetActive(false);
        HideSetupButton();
        HidePackupButton();
        HideArtilleryButton();
        HideDropoffButton();
        HideCancelDropoffButton();
        airControlPanel.SetActive(false);
        HideArtilleryControlPanel();
        HideArtilleryStatusPanel();
    }
    
    
    public void ShowArtilleryStatusPanel(int salvos, int salvosRemaining, ArtilleryTargetType targetType)
    {
        artilleryStatusPanel.SetActive(true);
        //if (shots remaining > 0) {
        //artilleryStatusCancelButton.SetActive(true);
        //} else {
        //artilleryStatusCancelButton.SetActive(false);
        //}
        SetText(artilleryStatusSalvos, salvos.ToString());
        SetText(artilleryStatusSalvosRemaining, salvosRemaining.ToString());

        switch (targetType)
        {
            case ArtilleryTargetType.Point: 
                SetText(artilleryStatusTargetType, "Point");
                break;
            case ArtilleryTargetType.Circle: 
                SetText(artilleryStatusTargetType, "Circle");
                break;
            case ArtilleryTargetType.Line: 
                SetText(artilleryStatusTargetType, "Line");
                break;
        }

    }
    public void HideArtilleryStatusPanel()
    {
        artilleryStatusPanel.SetActive(false);
    }

    public void ShowArtilleryControlPanel(int salvos)
    {
        artilleryControlPanel.SetActive(true);
        SetText(artilleryControlPanelSalvos, salvos.ToString());
    }
    public void HideArtilleryControlPanel()
    {
        artilleryControlPanel.SetActive(false);
    }

    public void UpdateSalvos(Unit unit, int salvos)
    {
        SetText(artilleryControlPanelSalvos, salvos.ToString());
    }

    public void SetArtilleryTargetType(ArtilleryTargetType artilleryTargetType)
    {
        Color32 white = new Color32(255, 255, 255, 255);
        Color32 grey = new Color32(127, 127, 127, 255);

        switch (artilleryTargetType)
        {
            case ArtilleryTargetType.Point:
                //Debug.Log("Point");
                artilleryControlPanelTargetPoint.GetComponent<Image>().color = white;
                artilleryControlPanelTargetCircle.GetComponent<Image>().color = grey;
                artilleryControlPanelTargetLine.GetComponent<Image>().color = grey;
                break;
            case ArtilleryTargetType.Circle:
                //Debug.Log("Circle");
                artilleryControlPanelTargetPoint.GetComponent<Image>().color = grey;
                artilleryControlPanelTargetCircle.GetComponent<Image>().color = white;
                artilleryControlPanelTargetLine.GetComponent<Image>().color = grey;
                break;
            case ArtilleryTargetType.Line:
                //Debug.Log("Line");
                artilleryControlPanelTargetPoint.GetComponent<Image>().color = grey;
                artilleryControlPanelTargetCircle.GetComponent<Image>().color = grey;
                artilleryControlPanelTargetLine.GetComponent<Image>().color = white;
                break;
        }
    }
    
    public void ShowHideSetupPackup(Unit unit)
    {
        HideSetupButton();
        HidePackupButton();
        if (unit.HasSetupWeapon())
        {
            if (unit.isPackedUp && ! unit.isSettingUp)
            {
                ShowSetupButton();
            }
            if (unit.isSetUp && !unit.isPackingUp)
            {
                ShowPackupButton();
            }
        }
    }
    public void ShowSetupButton()
    {
        setupButton.SetActive(true);
    }

    public void HideSetupButton()
    {
        setupButton.SetActive(false);
    }

    public void ShowPackupButton()
    {
        packupButton.SetActive(true);
    }

    public void HidePackupButton()
    {
        packupButton.SetActive(false);
    }

    public void ShowHideArtilleryButton(Unit unit)
    {
        if (unit.HasIndirectWeapon() && unit.isSetUp && ! unit.isPackingUp)
        {
            ShowArtilleryButton();
        }
        else
        {
            HideArtilleryButton();
        }
    }
    
    public void ShowArtilleryButton()
    {
        artilleryButton.SetActive(true);
    }
    public void HideArtilleryButton()
    {
        artilleryButton.SetActive(false);
    }

    public void ShowHideDropoffButton(Unit unit)
    {
        if (unit.cargo != null && unit.cargo.containedUnits.Count > 0)
        {
            ShowDropoffButton();
        }
        else
        {
            HideDropoffButton();
        }
    }
    public void ShowDropoffButton()
    {
        dropoffButton.SetActive(true);
    }
    public void HideDropoffButton()
    {
        dropoffButton.SetActive(false);
    }

    public void ShowCancelDropoffButton()
    {
        cancelDropoffButton.SetActive(true);
    }
    public void HideCancelDropoffButton()
    {
        cancelDropoffButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if ( ! initialised)
        {
            Initialise();
        }
        UpdateTargetingCrosshair();
        UpdateActionsPanelPosition();
        UpdateAltitudePanelPosition();
    }

    void Initialise()
    {
        map = (Map)FindObjectOfType<Map>();
        global = (Global)FindObjectOfType<Global>();
        initialised = true;
    }



    /// <summary>
    /// Target Crosshair
    /// </summary>
    /// <param name="unit"></param>
    
    public void ShowTargetingCrosshair(GameObject unitObject)
    {
        targetingCrosshairTargetedObject = unitObject;
        targetingCrosshair.SetActive(true);

        UpdateTargetingCrosshair();
    }

    public void ShowTargetingCrosshair(Tile tile)
    {
        targetingCrosshairTargetedObject = tile.hexObject;
        targetingCrosshair.SetActive(true);

        UpdateTargetingCrosshair();
    }

    public void HideTargetingCrosshair()
    {
        targetingCrosshairTargetedObject = null;
        targetingCrosshair.SetActive(false);
    }
    
    private void UpdateTargetingCrosshair()
    {

        if (targetingCrosshairTargetedObject != null)
        {
            Vector3 unitScreenPos = Camera.main.WorldToScreenPoint(targetingCrosshairTargetedObject.transform.position);

            float distanceFromCamera = Vector3.Distance(Camera.main.transform.position, targetingCrosshairTargetedObject.transform.position);
            float scale = 20.0f / distanceFromCamera;

            targetingCrosshair.transform.position = unitScreenPos;

            targetingCrosshair.transform.localScale = targetingCrosshiarInitialScale * scale;
        }

    }
    



    public void UpdateAltitudeControl(Unit selectedUnit, int targetAltitude)
    {
        // change color here to show target alt is different?
        airControlPanelAltitude.GetComponent<Text>().text = targetAltitude.ToString();
    }

    //private void ShowUnitTypeUI(Unit unit, SquadDef squadDef)
    private void ShowUnitTypeUI(Unit unit)
    {

        SquadDef squadDef = unit.squadDefs[0];

        //// rewrite all this

        unitName.GetComponent<Text>().text = squadDef.name;
        soldiersNum.GetComponent<Text>().text = "Soldiers: " + squadDef.soldiers.ToString();

        if (squadDef.armor > 0)
        {
            armor.GetComponent<Text>().text = "Armor: " + squadDef.armor.ToString();
        }
        else
        {
            armor.GetComponent<Text>().text = "";
        }

        if (unit.altitude > 0) {
            altitude.GetComponent<Text>().text = "Altitude: " + ((unit.tile.y + unit.altitude) * 10) + "m";
            height.GetComponent<Text>().text = "Height: " + ((unit.altitude) * 10) + "m";
            airControlPanel.SetActive(true);
            airControlPanelAltitude.GetComponent<Text>().text = unit.altitude.ToString();
        } else {
            altitude.GetComponent<Text>().text = "";
            height.GetComponent<Text>().text = "";
            airControlPanel.SetActive(false);
        }

        if (squadDef.weapons != null) {
            int w = 0;
            int wTotal = weaponNums.Length;
            for (; w < squadDef.weapons.Length; w++)
            {
                UnitWeapon unitWeapon = squadDef.weapons[w];

                weaponNums[w].GetComponent<Text>().text = unitWeapon.number.ToString() + "x";
                weaponNames[w].GetComponent<Text>().text = unitWeapon.weapon.name;
                weaponRanges[w].GetComponent<Text>().text = (unitWeapon.weapon.displayRange > 0) ? unitWeapon.weapon.displayRange.ToString() : unitWeapon.weapon.range.ToString();
                weaponPens[w].GetComponent<Text>().text = unitWeapon.weapon.penetration.ToString();
                weaponAmmos[w].GetComponent<Text>().text = unitWeapon.ammo.ToString();

            }
            for (; w < wTotal; w++)
            {
                weaponNums[w].GetComponent<Text>().text = "";
                weaponNames[w].GetComponent<Text>().text = "";
                weaponRanges[w].GetComponent<Text>().text = "";
                weaponPens[w].GetComponent<Text>().text = "";
                weaponAmmos[w].GetComponent<Text>().text = "";

            }
        }

        if (unit.cargo.containedUnits.Count > 0)
        {
            cargoLabel.GetComponent<Text>().text = "Cargo:";
            cargo.GetComponent<Text>().text = unit.cargo.containedUnits[0].squadDefs[0].name;
        }
        else
        {
            cargoLabel.GetComponent<Text>().text = "";
            cargo.GetComponent<Text>().text = "";
        }

        if (unit.tile.fort != null)
        {
            if (unit.tile.fort.NextStage() != null)
            {
                fortName.GetComponent<Text>().text = unit.tile.fort.stage.name + " (" + unit.tile.fort.progress + "/" + unit.tile.fort.NextStage().buildPoints + ")";
            }
            else
            {
                fortName.GetComponent<Text>().text = unit.tile.fort.stage.name;
            }
            fortCover.GetComponent<Text>().text = "Cover: " + (Math.Round(unit.tile.fort.stage.cover *100).ToString() + "%");
        }
        else
        {
            fortName.GetComponent<Text>().text = "";
            fortCover.GetComponent<Text>().text = "";
        }

    }

    
        
    private void UpdatePlatoonInfoPanel(Unit unit)
    {

        SetText(platoonInfoPanel.unitName, unit.UnitName());
        //SetText(platoonInfoPanel.unitName, unit.UnitShortName());

        //SetText(platoonInfoPanel.unitName2, unit.UnitName());

        string squadsText = "";
        int numSquads = 0;
        foreach ( SquadDef sd in unit.coreSquadDefs.Concat(unit.attachedSquadDefs) ) {
            if (squadsText != "") squadsText += "\n";
            if (sd.unitClass == UnitClass.Infantry) squadsText += sd.soldiers.ToString() + "* ";
            squadsText += sd.name;
            numSquads++;
        }
        
        SetText(platoonInfoPanel.squads, squadsText);
        
        /*
        string weaponsText = "";
        int numWeapons = 0;
        List<UnitWeapon> weapons = unit.WeaponsTally();
        //SetText(platoonInfoPanel.weapons, weapons.Count.ToString() );
        foreach ( UnitWeapon uw in weapons ) {
            if (weaponsText != "") weaponsText += "\n";
            weaponsText += uw.number + "x " + uw.weapon.name;
            numWeapons++;
        }

        SetText( platoonInfoPanel.weapons, weaponsText );
        */
        
        List<UnitWeapon> weapons = unit.WeaponsTally();
        int numWeapons = weapons.Count;

        string weaponNumsText = "";
        foreach ( UnitWeapon uw in weapons ) {
            if (weaponNumsText != "") weaponNumsText += "\n";
            weaponNumsText += uw.number + "x ";
        }
        SetText( platoonInfoPanel.weaponNums, weaponNumsText );
        
        string weaponNamesText = "";
        foreach ( UnitWeapon uw in weapons ) {
            if (weaponNamesText != "") weaponNamesText += "\n";
            weaponNamesText += uw.weapon.name;
        }
        SetText( platoonInfoPanel.weaponNames, weaponNamesText );

        string weaponRangesText = "";
        foreach ( UnitWeapon uw in weapons ) {
            if (weaponRangesText != "") weaponRangesText += "\n";
            weaponRangesText += "";
        }
        SetText( platoonInfoPanel.weaponRanges, weaponRangesText );

        string weaponPensText = "";
        foreach ( UnitWeapon uw in weapons ) {
            if (weaponPensText != "") weaponPensText += "\n";
            weaponPensText += "";
        }
        SetText( platoonInfoPanel.weaponPens, weaponPensText );

        string weaponAmmosText = "";
        foreach ( UnitWeapon uw in weapons ) {
            if (weaponAmmosText != "") weaponAmmosText += "\n";
            weaponAmmosText += "";
        }
        SetText( platoonInfoPanel.weaponAmmos, weaponAmmosText );

        //create elements


        /*
        SquadDef squadDef = unit.squadDefs[0];

        //// rewrite all this

        unitName.GetComponent<Text>().text = squadDef.name;
        soldiersNum.GetComponent<Text>().text = "Soldiers: " + squadDef.soldiers.ToString();

        if (squadDef.armor > 0)
        {
            armor.GetComponent<Text>().text = "Armor: " + squadDef.armor.ToString();
        }
        else
        {
            armor.GetComponent<Text>().text = "";
        }

        if (unit.altitude > 0) {
            altitude.GetComponent<Text>().text = "Altitude: " + ((unit.tile.y + unit.altitude) * 10) + "m";
            height.GetComponent<Text>().text = "Height: " + ((unit.altitude) * 10) + "m";
            airControlPanel.SetActive(true);
            airControlPanelAltitude.GetComponent<Text>().text = unit.altitude.ToString();
        } else {
            altitude.GetComponent<Text>().text = "";
            height.GetComponent<Text>().text = "";
            airControlPanel.SetActive(false);
        }

        if (squadDef.weapons != null) {
            int w = 0;
            int wTotal = weaponNums.Length;
            for (; w < squadDef.weapons.Length; w++)
            {
                UnitWeapon unitWeapon = squadDef.weapons[w];

                weaponNums[w].GetComponent<Text>().text = unitWeapon.number.ToString() + "x";
                weaponNames[w].GetComponent<Text>().text = unitWeapon.weapon.name;
                weaponRanges[w].GetComponent<Text>().text = (unitWeapon.weapon.displayRange > 0) ? unitWeapon.weapon.displayRange.ToString() : unitWeapon.weapon.range.ToString();
                weaponPens[w].GetComponent<Text>().text = unitWeapon.weapon.penetration.ToString();
                weaponAmmos[w].GetComponent<Text>().text = unitWeapon.ammo.ToString();

            }
            for (; w < wTotal; w++)
            {
                weaponNums[w].GetComponent<Text>().text = "";
                weaponNames[w].GetComponent<Text>().text = "";
                weaponRanges[w].GetComponent<Text>().text = "";
                weaponPens[w].GetComponent<Text>().text = "";
                weaponAmmos[w].GetComponent<Text>().text = "";

            }
        }

        if (unit.cargo.containedUnits.Count > 0)
        {
            cargoLabel.GetComponent<Text>().text = "Cargo:";
            cargo.GetComponent<Text>().text = unit.cargo.containedUnits[0].squadDefs[0].name;
        }
        else
        {
            cargoLabel.GetComponent<Text>().text = "";
            cargo.GetComponent<Text>().text = "";
        }

        if (unit.tile.fort != null)
        {
            if (unit.tile.fort.NextStage() != null)
            {
                fortName.GetComponent<Text>().text = unit.tile.fort.stage.name + " (" + unit.tile.fort.progress + "/" + unit.tile.fort.NextStage().buildPoints + ")";
            }
            else
            {
                fortName.GetComponent<Text>().text = unit.tile.fort.stage.name;
            }
            fortCover.GetComponent<Text>().text = "Cover: " + (Math.Round(unit.tile.fort.stage.cover *100).ToString() + "%");
        }
        else
        {
            fortName.GetComponent<Text>().text = "";
            fortCover.GetComponent<Text>().text = "";
        }
        */
    }

    /// <summary>
    /// Targeting Panel
    /// </summary>
    public void ShowTargetingPanel()
    {
        targetingPanel.SetActive(true);
    }

    public void HideTargetingPanel()
    {
        targetingPanel.SetActive(false);
    }

    public void UpdateTargetingPanelThisUnit(SquadDef squadDef)
    {

        //int range = 13;

        SetText(targetingPanel.transform.Find("This Unit Name").gameObject, squadDef.name);
        //SetText(targetingPanel.transform.Find("Range").gameObject, "Range: " + range.ToString());

        //int i = 0;
        //int maxWeapons = 5;
        //for (; i < squadDef.weapons.Length && i < maxWeapons; i++)
        //{
        //    showTargetingPanelWeapon(i + 1);
        //    UpdateTargetingPanelWeapon(i + 1, squadDef.weapons[i].weapon, range, TargetType.Infantry);
        //}
        //for (; i < maxWeapons; i++)
        //{
        //    hideTargetingPanelWeapon(i + 1);
        //}
    }

    //
    //public void UpdateTargetingPanel(SquadDef squadDef)
    //{
    //    // range
        

    //    SetText(targetingPanel.transform.Find("This Unit Name").gameObject, squadDef.name);
    //    SetText(targetingPanel.transform.Find("Range").gameObject, "Range: " + range.ToString());

    //    int i = 0;
    //    int maxWeapons = 5;
    //    for ( ; i < squadDef.weapons.Length && i < maxWeapons; i++)
    ///    {
    //        showTargetingPanelWeapon(i + 1);
    //        UpdateTargetingPanelWeapon(i+1, squadDef.weapons[i], range, TargetType.Infantry);
    //    }
    //    for ( ; i < maxWeapons; i++)
    //    {
   //         hideTargetingPanelWeapon(i + 1);
    //    }
    //}
    

    public void UpdateTargetingPanelTarget(Unit thisUnit, Unit targetUnit)
    {

        SetText( targetingPanel.transform.Find("Target Name").gameObject, targetUnit.UnitName() );

        int range = map.GetGameRange(thisUnit.tile, targetUnit.tile);
        SetText(targetingPanel.transform.Find("Range").gameObject, "Range: " + range.ToString() + " (" + Math.Floor(Math.Round((double)range * MapDefs.hexWidth, 2)) + "m)" );

        UpdateTargetingPanelWeapons(thisUnit, targetUnit, range);
    }

    public void UpdateTargetingPanelWeapons(Unit unit, Unit targetUnit, int range)
    {

        SquadDef squadDef = unit.squadDefs[0];

        //SetText(targetingPanel.transform.Find("This Unit Name").gameObject, squadDef.name);
        //SetText(targetingPanel.transform.Find("Range").gameObject, "Range: " + range.ToString());

        int i = 0;
        int maxWeapons = 5;
        for (; i < squadDef.weapons.Length && i < maxWeapons; i++)
        {
            showTargetingPanelWeapon(i + 1);
            UpdateTargetingPanelWeapon(unit, i + 1, squadDef.weapons[i], range, targetUnit);
        }
        for (; i < maxWeapons; i++)
        {
            hideTargetingPanelWeapon(i + 1);
        }

        PositionTargetingWeaponBlocks();
    }

    public string doubleToPercentString(double n)
    {
        return Math.Round(n * 100.0, 2).ToString() + "%";
    }

    public bool UpdateTargetingPanelWeapon(Unit unit, int weaponNum, UnitWeapon uWeapon, int range, Unit targetUnit)
    {
        Weapon weapon = uWeapon.weapon;
        SquadDef targetSquadDef = targetUnit.squadDefs[0];

        TargetType targetType = targetSquadDef.targetType;

        string weaponText = "Weapon" + weaponNum.ToString();

        SetText(targetingPanel.transform.Find(weaponText + " Name").gameObject, weapon.name);

        if (weapon.damageProfiles.ContainsKey(targetType))
        {
            if (range <= weapon.range)
            {

                //targetUnitDef = targetUnit.squadDefs[0];

                //bool moving = (unit.actionPoints == 0 && !unit.hasFired);
                double portionMovementUsed = unit.PortionMovementUsed();

                DamageProfile damageProfile = weapon.damageProfiles[targetType];

                SetText(targetingPanel.transform.Find(weaponText + " Range").gameObject, "Range: " + weapon.range.ToString());

                SetText(targetingPanel.transform.Find(weaponText + " Ammo").gameObject, "Ammo: " + uWeapon.ammo.ToString());


                SetText(targetingPanel.transform.Find(weaponText + " Shots").gameObject, "Shots: " + damageProfile.shots.ToString());
                double hitChanceAtRange = damageProfile.GetHitChanceForRange( range, weapon, targetSquadDef.hitability, unit.PortionMovementUsed() );

                SetText(targetingPanel.transform.Find(weaponText + " Hit Chance").gameObject, "Hit Chance: " + Math.Round(hitChanceAtRange * 100.0, 2).ToString() + "%");

                if (damageProfile.maxDamage > 0)
                {
                    SetText(targetingPanel.transform.Find(weaponText + " Damage").gameObject, "Damage: " + damageProfile.minDamage + "-" + damageProfile.maxDamage );
                }
                else
                {
                    SetText(targetingPanel.transform.Find(weaponText + " Damage").gameObject, "Damage: " + damageProfile.damage.ToString());
                }

                SetText(targetingPanel.transform.Find(weaponText + " Damage Chance").gameObject, "Dmg Chance: " + Math.Round(damageProfile.damageChance * 100.0, 2).ToString() + "%");

                if (targetUnit.tile.fort != null)
                {
                    SetText(targetingPanel.transform.Find(weaponText + " Cover").gameObject, "Fort: " + Math.Round(targetUnit.tile.fort.stage.cover * 100).ToString() + "%");
                } else
                {
                    SetText(targetingPanel.transform.Find(weaponText + " Cover").gameObject, "");
                }

                SetText(targetingPanel.transform.Find(weaponText + " Countermeasures").gameObject, "Countermeasures: " + doubleToPercentString(targetSquadDef.countermeasures.GetCountermeasureStrength(weapon.weaponType)) );


                SetText(targetingPanel.transform.Find(weaponText + " Terrain Cover").gameObject, "Terrain: " + Math.Round(targetUnit.tile.terrain.GetCover(targetSquadDef.targetType) * 100) + "%");
                

                if (targetSquadDef.armor > 0)
                {
                    SetText(targetingPanel.transform.Find(weaponText + " PenChance").gameObject, "Pen: " + Math.Round(Penetration.PenChance(weapon.penetration, targetSquadDef.armor) * 100) + "%");
                } else
                {
                    SetText(targetingPanel.transform.Find(weaponText + " PenChance").gameObject, "");
                }

                if (targetUnit.tile.fort != null)
                {
                    SetText(targetingPanel.transform.Find(weaponText + " Average Damage").gameObject, "Average Damage: " + Math.Round(damageProfile.GetAverageDamage(targetUnit, uWeapon, range, targetUnit.tile.terrain, targetSquadDef.countermeasures, portionMovementUsed, targetUnit.tile.fort), 2).ToString());
                } else
                {
                    SetText(targetingPanel.transform.Find(weaponText + " Average Damage").gameObject, "Average Damage: " + Math.Round(damageProfile.GetAverageDamage(targetUnit, uWeapon, range, targetUnit.tile.terrain, targetSquadDef.countermeasures, portionMovementUsed), 2).ToString());
                }

                SetText(targetingPanel.transform.Find(weaponText + " Max Damage").gameObject, "Max Damage: " + Math.Round(damageProfile.GetMaxDamage(uWeapon), 2).ToString());

                SetText(targetingPanel.transform.Find(weaponText + " Info").gameObject, "");
            } else
            {
                blankOutWeaponStatsB(weaponNum);

                SetText(targetingPanel.transform.Find(weaponText + " Info").gameObject, "Out of range");
            }
            
            return true;
        }
        else
        {
            SetText(targetingPanel.transform.Find(weaponText + " Range").gameObject,            "");

            SetText(targetingPanel.transform.Find(weaponText + " Ammo").gameObject,             "");

            SetText(targetingPanel.transform.Find(weaponText + " Shots").gameObject,            "");
            SetText(targetingPanel.transform.Find(weaponText + " Hit Chance").gameObject,       "");
            SetText(targetingPanel.transform.Find(weaponText + " Damage").gameObject,           "");
            SetText(targetingPanel.transform.Find(weaponText + " Damage Chance").gameObject,    "");

            SetText(targetingPanel.transform.Find(weaponText + " Cover").gameObject, "");
            SetText(targetingPanel.transform.Find(weaponText + " Countermeasures").gameObject,  "");

            SetText(targetingPanel.transform.Find(weaponText + " Terrain Cover").gameObject,    "");
            SetText(targetingPanel.transform.Find(weaponText + " PenChance").gameObject,        "");

            SetText(targetingPanel.transform.Find(weaponText + " Average Damage").gameObject,   "");
            SetText(targetingPanel.transform.Find(weaponText + " Max Damage").gameObject,       "");

            SetText(targetingPanel.transform.Find(weaponText + " Info").gameObject,             "");

            return false;
        }

    }

    public void blankOutWeaponStatsB(int weaponNum)
    {
        string weaponText = "Weapon" + weaponNum.ToString();

        //SetText(targetingPanel.transform.Find(weaponText + " Range").gameObject, "");
        //SetText(targetingPanel.transform.Find(weaponText + " Ammo").gameObject, "");

        SetText(targetingPanel.transform.Find(weaponText + " Shots").gameObject, "");
        SetText(targetingPanel.transform.Find(weaponText + " Hit Chance").gameObject, "");
        SetText(targetingPanel.transform.Find(weaponText + " Damage").gameObject, "");
        SetText(targetingPanel.transform.Find(weaponText + " Damage Chance").gameObject, "");

        SetText(targetingPanel.transform.Find(weaponText + " Cover").gameObject, "");
        SetText(targetingPanel.transform.Find(weaponText + " Countermeasures").gameObject, "");

        SetText(targetingPanel.transform.Find(weaponText + " Terrain Cover").gameObject, "");
        SetText(targetingPanel.transform.Find(weaponText + " PenChance").gameObject, "");

        SetText(targetingPanel.transform.Find(weaponText + " Average Damage").gameObject, "");
        SetText(targetingPanel.transform.Find(weaponText + " Max Damage").gameObject, "");
    }

    public void showTargetingPanelWeapon(int weaponNum)
    {
        string weaponText = "Weapon" + weaponNum.ToString();

        Color32 color = targetingPanel.transform.Find(weaponText + " Name").gameObject.GetComponent<Text>().color;
        color.a = 255;

        SetColor(targetingPanel.transform.Find(weaponText + " Name").gameObject, color);

        SetColor(targetingPanel.transform.Find(weaponText + " Range").gameObject, color);
        SetColor(targetingPanel.transform.Find(weaponText + " Ammo").gameObject, color);

        SetColor(targetingPanel.transform.Find(weaponText + " Shots").gameObject, color);
        SetColor(targetingPanel.transform.Find(weaponText + " Hit Chance").gameObject, color);
        SetColor(targetingPanel.transform.Find(weaponText + " Damage").gameObject, color);
        SetColor(targetingPanel.transform.Find(weaponText + " Damage Chance").gameObject, color);

        Color32 countermeasuresColor = targetingPanel.transform.Find(weaponText + " Countermeasures").gameObject.GetComponent<Text>().color;
        countermeasuresColor.a = 255;
        SetColor(targetingPanel.transform.Find(weaponText + " Cover").gameObject, countermeasuresColor);
        SetColor(targetingPanel.transform.Find(weaponText + " Countermeasures").gameObject, countermeasuresColor);

        SetColor(targetingPanel.transform.Find(weaponText + " Terrain Cover").gameObject, countermeasuresColor);
        SetColor(targetingPanel.transform.Find(weaponText + " PenChance").gameObject, countermeasuresColor);

        SetColor(targetingPanel.transform.Find(weaponText + " Average Damage").gameObject, color);
        SetColor(targetingPanel.transform.Find(weaponText + " Max Damage").gameObject, color);

        SetColor(targetingPanel.transform.Find(weaponText + " Info").gameObject, color);
    }

    public void hideTargetingPanelWeapon(int weaponNum)
    {
        string weaponText = "Weapon" + weaponNum.ToString();

        Color32 color = targetingPanel.transform.Find(weaponText + " Name").gameObject.GetComponent<Text>().color;
        color.a = 0;

        SetColor(targetingPanel.transform.Find(weaponText + " Name").gameObject, color);

        SetColor(targetingPanel.transform.Find(weaponText + " Range").gameObject, color);
        SetColor(targetingPanel.transform.Find(weaponText + " Ammo").gameObject, color);

        SetColor(targetingPanel.transform.Find(weaponText + " Shots").gameObject, color);
        SetColor(targetingPanel.transform.Find(weaponText + " Hit Chance").gameObject, color);
        SetColor(targetingPanel.transform.Find(weaponText + " Damage").gameObject, color);
        SetColor(targetingPanel.transform.Find(weaponText + " Damage Chance").gameObject, color);

        Color32 countermeasuresColor = targetingPanel.transform.Find(weaponText + " Countermeasures").gameObject.GetComponent<Text>().color;
        countermeasuresColor.a = 0;
        SetColor(targetingPanel.transform.Find(weaponText + " Cover").gameObject, countermeasuresColor);
        SetColor(targetingPanel.transform.Find(weaponText + " Countermeasures").gameObject, countermeasuresColor);

        SetColor(targetingPanel.transform.Find(weaponText + " Terrain Cover").gameObject, countermeasuresColor);
        SetColor(targetingPanel.transform.Find(weaponText + " PenChance").gameObject, countermeasuresColor);

        SetColor(targetingPanel.transform.Find(weaponText + " Average Damage").gameObject, color);
        SetColor(targetingPanel.transform.Find(weaponText + " Max Damage").gameObject, color);

        SetColor(targetingPanel.transform.Find(weaponText + " Info").gameObject, color);
    }

    public void PositionTargetingWeaponBlocks()
    {
        string weaponText = "";
        for (int i = 1; i <= 1; i++)
        {
            weaponText = "Weapon" + i.ToString();
            Vector3 startCoord = targetingPanel.transform.Find(weaponText + " Name").gameObject.transform.position;
            //PositionTargetingWeaponBlock( i, startCoord );
        }
    }

    public void PositionTargetingWeaponBlock(int weaponNum, Vector3 startCoord)
    {
        float ys = -25f;
        float xs = 80f;
        string weaponText = "Weapon" + weaponNum.ToString();
        Debug.Log("Position weapon block " + weaponText);
        GameObject go = targetingPanel.transform.Find(weaponText + " Name").gameObject;
        Debug.Log(go.transform.position);
        targetingPanel.transform.Find(weaponText + " Name").gameObject.transform.position = startCoord + (new Vector3(0f, 0f, 0f));
        targetingPanel.transform.Find(weaponText + " Range").gameObject.transform.position = startCoord + (new Vector3(0f, ys, 0f));
        targetingPanel.transform.Find(weaponText + " Ammo").gameObject.transform.position = startCoord + (new Vector3(0f, ys*2, 0f));

        targetingPanel.transform.Find(weaponText + " Shots").gameObject.transform.position = startCoord + (new Vector3(xs, 0f, 0f));
        targetingPanel.transform.Find(weaponText + " Damage").gameObject.transform.position = startCoord + (new Vector3(xs, ys, 0f));
        targetingPanel.transform.Find(weaponText + " Cover").gameObject.transform.position = startCoord + (new Vector3(xs, ys * 2, 0f));

        //SetColor(targetingPanel.transform.Find(weaponText + " Terrain Cover").gameObject, countermeasuresColor);
        //SetColor(targetingPanel.transform.Find(weaponText + " PenChance").gameObject, countermeasuresColor);

        targetingPanel.transform.Find(weaponText + " Hit Chance").gameObject.transform.position = startCoord + (new Vector3(xs*2, 0f, 0f));
        targetingPanel.transform.Find(weaponText + " Damage Chance").gameObject.transform.position = startCoord + (new Vector3(xs * 2, ys, 0f));
        targetingPanel.transform.Find(weaponText + " Countermeasures").gameObject.transform.position = startCoord + (new Vector3(xs * 2, ys*2, 0f));
        targetingPanel.transform.Find(weaponText + " Average Damage").gameObject.transform.position = startCoord + (new Vector3(xs * 2, ys*3, 0f));
        targetingPanel.transform.Find(weaponText + " Max Damage").gameObject.transform.position = startCoord + (new Vector3(xs * 2, ys*4, 0f));

        targetingPanel.transform.Find(weaponText + " Info").gameObject.transform.position = startCoord + (new Vector3(xs, 0f, 0f));
        
    }

    public static GameObject CreateUiElement(string name, GameObject parent)
    {
        GameObject newObj = new GameObject(name);

        //Add Components
        RectTransform rectTransform = newObj.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);

        newObj.transform.position = new Vector3(270.0f, -300.0f, 0f);
        rectTransform.sizeDelta = new Vector2(130f, 40f);

        newObj.AddComponent<CanvasRenderer>();

        Text text = AddTextNode(newObj);

        newObj.transform.SetParent(parent.transform, false);

        return newObj;
    }

    public static GameObject CreateUiTextElement(string name, GameObject parent, Dictionary<string, object> Params)
    {
        
        Vector2 size = Params.ContainsKey("size") ? (Vector2)Params["size"] : new Vector2(0, 0);
        Vector3 position = Params.ContainsKey("position") ? (Vector3)Params["position"] : new Vector3(0, 0, 0);



        GameObject newObj = new GameObject(name);

        //Add Components
        RectTransform rectTransform = newObj.AddComponent<RectTransform>();

        // anchor top left (0 x, 1 y)
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);

        // position the box from the top left corner (0 x, 1 y)
        rectTransform.pivot = new Vector2(0, 1);
        
        // size the box
        rectTransform.sizeDelta = size; // new Vector2(130f, 40f);

        // position the box
        newObj.transform.position = position;

        newObj.AddComponent<CanvasRenderer>();

        Text text = AddTextNode(newObj, Params);

        newObj.transform.SetParent(parent.transform, false);

        return newObj;
    }

    public static Text AddTextNode(GameObject gObject)
    {
        Text text = gObject.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.color = new Color32(0x32, 0x32, 0x32, 0xFF);
        text.fontSize = 22;
        text.alignment = TextAnchor.UpperRight;
        return text;
    }

    public static Text AddTextNode(GameObject gObject, Dictionary<string, object> Params)
    {
        TextAnchor alignment = Params.ContainsKey("alignment") ? (TextAnchor) Params["alignment"] : TextAnchor.UpperLeft;
        int fontSize = Params.ContainsKey("fontSize") ? (int) Params["fontSize"] : 28;
        Color32 color = Params.ContainsKey("color") ? (Color32) Params["color"] : new Color32(0x32, 0x32, 0x32, 0xFF);

        Text textNode = gObject.AddComponent<Text>();
        textNode.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        textNode.alignment = TextAnchor.UpperLeft;
        textNode.fontSize = fontSize;
        textNode.color = color;

        return textNode;
    }

    /*public static GetParam(Dictionary<string, object> Params, string key, object default) {

    }*/

    private void SetText(GameObject gObject, string text)
    {
        gObject.GetComponent<Text>().text = text;
    }

    private void SetColor(GameObject gObject, Color32 color)
    {
        gObject.GetComponent<Text>().color = color;
    }
    
    public void SetShowingLos(bool show)
    {
        if (show)
        {
            showingLosPanel.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        else
        {
            showingLosPanel.GetComponent<Image>().color = new Color32(127, 127, 127, 255);
        }
    }
    
    //public void ShowActionsPanel(Unit unit, Tile tile, List<string> actions)
    //{
    //    Vector3 pos = GetTileScreenPos(tile);
    //    actionsPanelTile = tile;
    //    actionsPanel.transform.position = pos;
    //    actionsPanel.SetActive(true);
    //}

    public void ShowActionsPanel(Unit unit, Tile tile, List<ActionType> actionTypes)
    {
        Vector3 pos = GetTileScreenPos(tile);
        pos.z = 0; // stop z-clipping
        actionsPanelTile = tile;
        actionsPanel.transform.position = pos;
        actionsPanel.SetActive(true);
    }

    public void ShowActionsPanel(Unit unit, Action action)
    {
        switch (action.type)
        {
            case ActionType.ArialMove:
                actionsPanelTile = action.tile;
                Vector3 pos = GetTileScreenPos(action.tile);
                pos.z = 0; // stop z-clipping
                actionsPanel.transform.position = pos;
                CreateAltitudePanel( (ArialMoveAction)action );
                break;
        }

    }
    
    
    public void CreateAltitudePanel(ArialMoveAction action)
    {
        DestroyAltitudePanel();
        altitudePanel.SetActive(true);
        altitudeSelectButtons = new List<GameObject>();
        int i = 0;
        foreach (int o in action.altitudeOptions)
        {
            //Debug.Log("alt opt " + o);
            GameObject button = (GameObject)Instantiate(Resources.Load("GO/AltitudeButton"));
            button.transform.SetParent(altitudePanel.transform);
            if (o != 0)
            {
                //button.transform.Find("Text").GetComponent<Text>().text = ("Alt " + o.ToString());
                //button.transform.Find("Text").GetComponent<Text>().text = ("Height " + (o + action.tile.y) + "(" + o + ")");
                button.transform.Find("Text").GetComponent<Text>().text = ("" + ((o+action.tile.y) *10) + "m (" + (o*10)+ "m)");
            } else
            {
                button.transform.Find("Text").GetComponent<Text>().text = ("Land (0m)");
            }
            button.transform.Find("Height").GetComponent<Text>().text = (o + action.tile.y).ToString(); ;
            button.transform.localScale = new Vector3(1f, 1f , 1f);
            button.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f + ((float)(i - 1) * 60f), 0f);

            //button.GetComponent<Button>().onClick.AddListener(global.playerInput.ButtonSelectAltitude);
            button.GetComponent<Button>().onClick.AddListener(delegate { global.playerInput.ButtonSelectAltitude(action, o); });

            //button.GetComponent<EventTrigger>().onClick.AddListener(delegate { global.playerInput.ButtonSelectAltitude(action, o); });



            //button.transform.position = 
            altitudeSelectButtons.Add(button);

            i++;
        }
    }

    public void DestroyAltitudePanel()
    {
        altitudePanel.SetActive(false);
        foreach (GameObject b in altitudeSelectButtons) {
            Destroy(b);
        }
    }
    
    public void HideActionsPanel()
    {
        actionsPanel.SetActive(false);
        actionsPanelTile = null;
    }

    public void UpdateActionsPanelPosition()
    {
        if (actionsPanelTile != null)
        {
            actionsPanel.transform.position = GetTileScreenPos(actionsPanelTile);
        }
    }
    public void UpdateAltitudePanelPosition()
    {
        if (actionsPanelTile != null)
        {
            altitudePanel.transform.position = GetTileScreenPos(actionsPanelTile);
        }
    }

    public Vector3 GetTileScreenPos(Tile tile)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(tile.position);
        pos.z = 0; // stop z-clipping
        return pos;

        //float distanceFromCamera = Vector3.Distance(Camera.main.transform.position, unit.transform.position);
        //float scale = 10.0f / distanceFromCamera;
    }

    public void ShowTargetingAccuracy() {
        accuracyDisplayPanel.SetActive(true);
        //Debug.Log("showing targeting accuracy panel");
    }
    public void HideTargetingAccuracy() {
        accuracyDisplayPanel.SetActive(false);
        //Debug.Log("hiding targeting accuracy panel");
    }

    public void ShowTargetingAccuracyForAction(Action action, Tile tile, double accuracy)
    {
        bool debugThis = false;

        Vector3 tilePos = tile.position;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(tilePos);

        float distanceFromCamera = Vector3.Distance(Camera.main.transform.position, tilePos);
        float scale = 10.0f / distanceFromCamera * 100f;


        screenPos.y -= 33f * scale; // shift the panel downwards

        screenPos.z = 0f; // sets z to 0 distance, as far plane clips otherwise

        if (debugThis) Debug.Log("accuracy panel xyz " + screenPos.x + "-" + screenPos.y + "-" + screenPos.z);

        //if (unitScript.squadDefs[0].name == "Shilka-M4") Debug.Log(unitScreenPos);

        accuracyDisplayPanel.transform.position = screenPos;
        //accuracyDisplayPanel.transform.localScale = screenPos * scale;

        accuracyDisplayText.GetComponent<Text>().text = Funcs.Percentage(accuracy);

        ShowTargetingAccuracy();

    }

    

}