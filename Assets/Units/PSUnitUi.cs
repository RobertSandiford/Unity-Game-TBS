using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSUnitUi : MonoBehaviour
{
    public PSUnit psUnit;
    public Vector3 unitPos;
    GameObject uiPanel;
    GameObject white;

    // Start is called before the first frame update
    void Start()
    {
        uiPanel = gameObject.transform.Find("UnitUiPanel").gameObject;
        white = uiPanel.transform.Find("White").gameObject;
        white.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SetUiPosition();
    }

    public void SetUiPosition() {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(unitPos);
        screenPos.z = 0f; // it sets z to distance, as far plane clips otherwise

        float distanceFromCamera = Vector3.Distance(Camera.main.transform.position, unitPos);
        float scale = 1000.0f / distanceFromCamera;
        
        uiPanel.transform.position = screenPos;
        uiPanel.transform.localScale = new Vector3(scale, scale, scale);

        //Debug.Log(screenPos);
    }

    public void Click() {
        white.SetActive(true);
        Debug.Log("Click");

        PSController.selectedUnit = psUnit;

        //psUnit.MoveTo( new Vector2(2200, 2000) );
    }

    public void Deselect() {
        white.SetActive(false);
    }

}
