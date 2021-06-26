using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public enum FortType
{
    Infantry,
    Vehicle,
    Mortar,
    Artillery,
    MechanisedArtillery
}

public class FortDef
{
    public string name;
    public FortType type;
    public List<FortStageDef> stages;

    public FortDef()
    {
    }
}

public class FortStageDef
{
    public string name;
    public int buildPoints;
    public double cover;
    public Material material;
}

public static class Forts
{
    private static GameObject fortDisc = (GameObject)Resources.Load("GO/FortDisc");

    public static Fort MakeFort(FortDef fortDef, Tile tile, int stage = 0)
    {
        GameObject fortObj = GameObject.Instantiate(fortDisc);
        Fort fort = (Fort)fortObj.GetComponent<Fort>();
        fort.Setup(fortDef, tile, stage);
        tile.fort = fort;

        return fort;
    }
}

public class Fort : MonoBehaviour
{
    public new string name;
    public FortType type;
    public Tile tile;
    public List<FortStageDef> stages;
    public FortStageDef stage;
    public int progress;
    public bool visible;
    public GameObject fortObj;

    public Fort(FortDef fortDef)
    {
        name = fortDef.name;
        type = fortDef.type;
        stages = fortDef.stages;
        stage = stages[0];
        progress = 0;
    }
    public Fort()
    {

    }

    public void Start()
    {
    }

    public void Setup(FortDef fortDef, Tile unitTile, int stageNum = 0)
    {
        name = fortDef.name;
        type = fortDef.type;
        tile = unitTile;
        stages = fortDef.stages;
        stage = stages[stageNum];
        progress = 0;
        gameObject.GetComponent<MeshRenderer>().material = stage.material;

        fortObj = gameObject;

        SetPosition();
    }

    public void SetPosition()
    {
        GameObject go = gameObject;
        Vector3 pos = tile.position;
        go.transform.position = pos;
    }

    public bool IsComplete()
    {
        return (stage == stages.Last());
    }

    public FortStageDef NextStage()
    {
        int currentStageIndex = stages.IndexOf(stage);
        if (currentStageIndex < (stages.Count - 1))
        {
            return stages[currentStageIndex + 1];
        }
        else
        {
            return null;
        }
    }

    public void Build(int points)
    {
        progress += points;
        int currentStageIndex = stages.IndexOf(stage);
        while (currentStageIndex < (stages.Count - 1) && progress >= stages[currentStageIndex + 1].buildPoints)
        {
            // increment the stage
            stage = stages[currentStageIndex + 1];
            // update current stage index
            currentStageIndex = stages.IndexOf(stage);
            // take that stage's build point need away from the progress points
            progress -= stage.buildPoints;

            gameObject.GetComponent<MeshRenderer>().material = stage.material;
        }
    }

    public void Show()
    {
        ((MeshRenderer)fortObj.GetComponent<MeshRenderer>()).enabled = true;
        visible = true;
    }
    public void Hide()
    {
        ((MeshRenderer)fortObj.GetComponent<MeshRenderer>()).enabled = false;
        visible = false;
    }
}

public static class FortDefs
{


    public static FortDef Infantry = new FortDef
    {
        name = "Infantry Entrenchment",
        type = FortType.Infantry,
        stages = new List<FortStageDef>
        {
            new FortStageDef
            {
                name = "Preparing",
                buildPoints = 0,
                cover = 0,
                material = Resources.Load<Material>("Tex/Materials/Fort_Blank_Tex")
            },
            new FortStageDef {
                name = "Shellscrape",
                buildPoints = 2,
                cover = 0.15,
                material = Resources.Load<Material>("Tex/Materials/Fort_Infantry_Shellscrapes_Tex")
            },
            new FortStageDef {
                name = "Foxholes",
                buildPoints = 4,
                cover = 0.3,
                material = Resources.Load<Material>("Tex/Materials/Fort_Infantry_Foxholes_Tex")
            },
            new FortStageDef {
                name = "Trenches",
                buildPoints = 7,
                cover = 0.45,
                material = Resources.Load<Material>("Tex/Materials/Fort_Infantry_Trenches_Tex")
            },
        }
    };

    public static FortDef Vehicle = new FortDef
    {
        name = "Vehicle Entrenchment",
        type = FortType.Vehicle,
        stages = new List<FortStageDef>
        {
            new FortStageDef
            {
                name = "Preparing",
                buildPoints = 0,
                cover = 0,
                material = Resources.Load<Material>("Tex/Materials/Fort_Blank_Tex")
            },
            new FortStageDef {
                name = "Mound",
                buildPoints = 4,
                cover = 0.1,
                material = Resources.Load<Material>("Tex/Materials/Fort_Vehicle_Mound_Tex")
            },
            new FortStageDef {
                name = "Shallow Trench",
                buildPoints = 4,
                cover = 0.2,
                material = Resources.Load<Material>("Tex/Materials/Fort_Vehicle_Trench_Tex")
            },
            new FortStageDef {
                name = "Full Trench",
                buildPoints = 4,
                cover = 0.3,
                material = Resources.Load<Material>("Tex/Materials/Fort_Vehicle_Reinforced_Tex")
            },
        }
    };

    public static FortDef Mortar = new FortDef
    {
        name = "Mortar Entrenchment",
        type = FortType.Mortar,
        stages = new List<FortStageDef>
        {
            new FortStageDef
            {
                name = "Preparing",
                buildPoints = 0,
                cover = 0,
            },
            new FortStageDef {
                name = "Foxholes",
                buildPoints = 4,
                cover = 0.1,
            },
            new FortStageDef {
                name = "Pit",
                buildPoints = 4,
                cover = 0.2,
            },
            new FortStageDef {
                name = "Reinforced Pit",
                buildPoints = 4,
                cover = 0.3,
            },
        }
    };

    public static FortDef Artillery = new FortDef
    {
        name = "Artillery Entrenchment",
        type = FortType.Artillery,
        stages = new List<FortStageDef>
        {
            new FortStageDef
            {
                name = "Preparing",
                buildPoints = 0,
                cover = 0,
            },
            new FortStageDef {
                name = "Shallow Pit",
                buildPoints = 4,
                cover = 0.1,
            },
            new FortStageDef {
                name = "Pit",
                buildPoints = 4,
                cover = 0.2,
            },
            new FortStageDef {
                name = "Reinforced Pit",
                buildPoints = 4,
                cover = 0.3,
            },
        }
    };

    public static FortDef MechanisedArtillery = new FortDef
    {
        name = "Artillery Entrenchment",
        type = FortType.MechanisedArtillery,
        stages = new List<FortStageDef>
        {
            new FortStageDef
            {
                name = "Preparing",
                buildPoints = 0,
                cover = 0,
            },
            new FortStageDef {
                name = "Shallow Trench",
                buildPoints = 6,
                cover = 0.1,
            },
            new FortStageDef {
                name = "Full Trench",
                buildPoints = 6,
                cover = 0.2,
            },
            new FortStageDef {
                name = "Extended Trench",
                buildPoints = 6,
                cover = 0.3,
            },
        }
    };

    public static Dictionary<FortType, FortDef> forts = new Dictionary<FortType, FortDef>
    {
        { FortType.Infantry, Infantry },
        { FortType.Vehicle, Vehicle },
        { FortType.Mortar, Mortar },
        { FortType.Artillery, Artillery },
        { FortType.MechanisedArtillery, MechanisedArtillery},
    };

}
