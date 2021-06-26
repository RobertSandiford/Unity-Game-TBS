using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PSController
{
    //public static PSController Instance { get; private set; }

    public static PSUnit selectedUnit;

    //static RaycastHit hit;

    // Start is called before the first frame update
    public static void Start()
    {
        
    }

    // Update is called once per frame
    public static void Update()
    {
        CheckRightClick();
    }

    static void CheckRightClick()
    {
        if (Input.GetMouseButtonDown(1)) {

            if (selectedUnit != null) {
                //PSUnit unitScript = (PSUnit)(selectedUnit.GetComponent(typeof(PSUnit)));
               
                RaycastHit hit;
                if (RaycastMousePosition(out hit))
                {
                    
                    if (hit.transform.gameObject.name.Contains("Hex"))
                    {
                        GameObject hex = hit.transform.gameObject;
                        Hex hexScript = (Hex)(hex.GetComponent(typeof(Hex)));
                        //RightClickHex(hexScript.tile);

                        RightClick(hex.transform.position);
                    }
                    //if (hit.transform.gameObject.name.Contains("UnitDisc")) RightClickHex(GetTileFromUnitHit(hit));
                    //if (hit.transform.gameObject.name.Contains("FortDisc")) RightClickHex(GetTileFromFortHit(hit));

                }
            }
        }
    }

    static bool RaycastMousePosition(out RaycastHit hit) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, 999999);
    }

    static void RightClick(Vector3 position) {
        Debug.Log(position);
        
        selectedUnit.MoveTo( new Vector2(position.x, position.z) );

    }

}
