using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventMessage
{
    public string text;
    public double startTime;
    public double endTime;

    public EventMessage(string text, double startTime, double endTime)
    {
        this.text = text;
        this.startTime = startTime;
        this.endTime = endTime;
    }

    public bool IsOver()
    {
        return Time.time > endTime;
    }
}
public class UnitUI : MonoBehaviour
{
    public bool started = false;

    public GameObject unit;
    public Unit unitScript;
    public GameObject uiPanel;
    public GameObject unitName;
    private Vector3 unitNameInitialScale;
    public GameObject healthbar;
    public GameObject healthBlip1;
    public GameObject healthBlip2;
    public GameObject healthBlip3;
    public GameObject healthBlip4;
    public GameObject healthBlip5;
    public GameObject eventPanel;
    public GameObject eventText;
    public GameObject cargo;
    public GameObject hasActions;
    public GameObject hasNoActions;
    public GameObject hasPartActions;
    public GameObject isSetup;
    private Vector3 uiPanelInitialScale;
    private Vector3 canvasInitialScale;
    private Vector3 healthbarInitialScale;
    private Vector3 eventPanelInitialScale;
    //private bool initialised = false;

    bool eventTextActive = false;
    double eventTextUpdatedAt = 0.0;
    double eventTextDuration = 0.0;
    List<EventMessage> eventMessages;

    public void Initialise()
    {
        unit = gameObject.transform.parent.gameObject;
        unitScript = (Unit)(unit.GetComponent(typeof(Unit)));

        uiPanel = gameObject.transform.Find("UnitUiPanel").gameObject;

        unitName = uiPanel.transform.Find("UnitName").gameObject;
        unitName.GetComponent<Text>().text = unitScript.unitShortName;
        //unitName.GetComponent<Text>().fontSize = 22; // hack because font size not being respected

        unitNameInitialScale = unitName.transform.localScale;
        healthbar = uiPanel.transform.Find("Healthbar").gameObject;
        healthBlip1 = healthbar.transform.Find("Blip1").gameObject;
        healthBlip2 = healthbar.transform.Find("Blip2").gameObject;
        healthBlip3 = healthbar.transform.Find("Blip3").gameObject;
        healthBlip4 = healthbar.transform.Find("Blip4").gameObject;
        healthBlip5 = healthbar.transform.Find("Blip5").gameObject;
        eventPanel = uiPanel.transform.Find("EventPanel").gameObject;
        eventText = eventPanel.transform.Find("EventText").gameObject;
        cargo = uiPanel.transform.Find("Cargo").gameObject;

        hasActions = uiPanel.transform.Find("HasActions").gameObject;
        hasNoActions = uiPanel.transform.Find("HasNoActions").gameObject;
        hasPartActions = uiPanel.transform.Find("HasPartActions").gameObject;
        isSetup = uiPanel.transform.Find("IsSetup").gameObject;

        canvasInitialScale = gameObject.transform.localScale;
        uiPanelInitialScale = uiPanel.transform.localScale;
        healthbarInitialScale = healthbar.transform.localScale;
        eventPanelInitialScale = eventPanel.transform.localScale;
        eventMessages = new List<EventMessage>();

        eventText.GetComponent<Text>().text = "";
        SetCargoText("");

        InitHasActions();

        started = true;
        //initialised = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (!initialised)
        //{
        //    Initialise();
        //    initialised = true;
        //}

        // Every Frame
        UpdateEveryFrame();
    }
    

    // Update is called once per frame
    void UpdateEveryFrame()
    {
        PositionCanvas();
        UpdateHealthDisplay();

        CheckEventMessages();

    }

    public void PositionCanvas()
    {
        Vector3 unitScreenPos = Camera.main.WorldToScreenPoint(unit.transform.position);

        float distanceFromCamera = Vector3.Distance(Camera.main.transform.position, unit.transform.position);
        float scale = 10.0f / distanceFromCamera * 100f;


        unitScreenPos.y += 33f * scale; // shift the UI upwards

        unitScreenPos.z = 0f; // sets z to 0 distance, and far plane clips otherwise

        //if (unitScript.squadDefs[0].name == "Shilka-M4") Debug.Log(unitScreenPos);

        uiPanel.transform.position = unitScreenPos;
        uiPanel.transform.localScale = uiPanelInitialScale * scale;


    }

    /*void PositionUnitName()
    {
        Vector3 unitScreenPos = Camera.main.WorldToScreenPoint(unit.transform.position);

        float distanceFromCamera = Vector3.Distance(Camera.main.transform.position, unit.transform.position);
        float scale = 10.0f / distanceFromCamera;
        
        unitScreenPos.y += 70f * scale;
        unitName.transform.position = unitScreenPos;

        unitName.transform.localScale = unitNameInitialScale * scale;
    }

    void PositionHealthbar()
    {
        // position the healthbar

        Vector3 unitScreenPos = Camera.main.WorldToScreenPoint(unit.transform.position);


        float distanceFromCamera = Vector3.Distance(Camera.main.transform.position, unit.transform.position);
        float scale = 10.0f / distanceFromCamera;

        //Debug.Log(unitScreenPos);
        //print("Distance from Camera: " + distanceFromCamera);

        //Debug.Log(healthbar.transform.localScale);
        //print("Scale: " + healthbar.transform.localScale);
        //print("Scale: " + scale);

        unitScreenPos.y += 50f * scale;
        healthbar.transform.position = unitScreenPos;

        healthbar.transform.localScale = healthbarInitialScale * scale;

        // Vector3 namePos = Camera.main.WorldToScrenPoint(this.transform.position);
        // nameLabel.transform.position = namePos;
    }

    void PositionEventText()
    {
        Vector3 unitScreenPos = Camera.main.WorldToScreenPoint(unit.transform.position);

        float distanceFromCamera = Vector3.Distance(Camera.main.transform.position, unit.transform.position);
        float scale = 10.0f / distanceFromCamera;

        unitScreenPos.y += 0f * scale;
        eventPanel.transform.position = unitScreenPos;
        eventPanel.transform.localScale = eventPanelInitialScale * scale;
    }*/

    void UpdateHealthDisplay() {
        if (unitScript.health < 5) healthBlip5.SetActive(false);
        if (unitScript.health < 4) healthBlip4.SetActive(false);
        if (unitScript.health < 3) healthBlip3.SetActive(false);
        if (unitScript.health < 2) healthBlip2.SetActive(false);
        if (unitScript.health < 1) healthBlip1.SetActive(false);
   }

    public void AddEventMessage(string text, double duration = 0.5)
    {
        if (started)
        {
            while (eventMessages.Count > 5) { eventMessages.Remove(eventMessages[0]); }

            eventMessages.Add(new EventMessage(text, Time.time, Time.time + duration));

            SetEventMessageText();
        }
    }

    private void CheckEventMessages()
    {
        List<EventMessage> messagesToDelete = new List<EventMessage>();
        foreach (EventMessage eventMessage in eventMessages)
        {
            if (eventMessage.IsOver()) messagesToDelete.Add(eventMessage);
        }
        if (messagesToDelete.Count > 0)
        {
            foreach (EventMessage deletingEventMessage in messagesToDelete)
            {
                eventMessages.Remove(deletingEventMessage);
            }
            SetEventMessageText();
        }


        //eventText.GetComponent<Text>().text = "av\nasf\nasfsf\nsf\nasfsf\nsf";
    }
    private void SetEventMessageText()
    {
        string text = "";
        foreach (EventMessage eventMessage in eventMessages)
        {
            text = text + eventMessage.text + "\n";
        }
        eventText.GetComponent<Text>().text = text;
        eventText.GetComponent<Text>().fontSize = 22; // hack because font size not being respected
    }

    public void SetEventText(string text, double duration = 0.5)
    {
        eventText.GetComponent<Text>().text = text;
        eventTextActive = true;
        eventTextUpdatedAt = Time.time;
        eventTextDuration = duration;
    }

    void DisableEventText()
    {
        eventTextActive = false;
        eventText.GetComponent<Text>().text = "";
    }

    void CheckEventText()
    {
        if (eventTextActive)
        {
            if (Time.time > eventTextUpdatedAt + eventTextDuration)
            {
                DisableEventText();
            }
        }
    }

    public void SetCargoText(string cargoText)
    {
        ((Text)cargo.GetComponent<Text>()).text = cargoText;
        ((Text)cargo.GetComponent<Text>()).fontSize = 22; // hack because font size not being respected
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void InitHasActions()
    {
        if (unitScript.team == 1)
        {
            SetHasActions(true);
        } else
        {
            // hide all if not player controlled
            hasActions.SetActive(false);
            hasNoActions.SetActive(false);
            hasPartActions.SetActive(false);
            isSetup.SetActive(false);
        }
    }
    public void SetHasActions(bool has = true)
    {
        hasActions.SetActive(has);
        hasNoActions.SetActive(!has);
        hasPartActions.SetActive(false);
        isSetup.SetActive(false);
    }
    public void SetHasPartActions()
    {
        hasActions.SetActive(false);
        hasNoActions.SetActive(false);
        hasPartActions.SetActive(true);
        isSetup.SetActive(false);
    }
    public void SetHasNoActions()
    {
        hasActions.SetActive(false);
        hasNoActions.SetActive(true);
        hasPartActions.SetActive(false);
        isSetup.SetActive(false);
    }
    public void SetIsSetup()
    {
        hasActions.SetActive(false);
        hasNoActions.SetActive(false);
        hasPartActions.SetActive(false);
        isSetup.SetActive(true);
    }
}
