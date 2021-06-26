using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{

    public int x;
    public int y;
    public int z;
    public Tile tile;
    public MeshRenderer meshRenderer;

    public bool objective = false;
    public bool road = false;

    public bool isAiActive = false;
    public bool isAiShooting = false;

    public bool isBlocking = false;
    public bool isArtyTarget = false;

    public bool hasShootableTarget = false;
    public bool inUnitViewAndWeaponRange = false;
    public bool inUnitView = false;
    public bool inTeamView = false;


    // public general highlight = false;

    public Material normalMaterial;
    public Material objectiveMaterial;
    public Material roadMaterial;

    public Material aiActiveMaterial;
    public Material aiShootingMaterial;

    public Material visionHighlightMaterial;
    public Material teamVisionHighlightMaterial;
    public Material unitVisionHighlightMaterial;
    public Material weaponRangeHighlightMaterial;

    public Material blockingMaterial;
    public Material artyTargetMaterial;

    public Material shootableTargetMaterial;

    private Material baseMaterial;

    public Material teamViewMaterial;
    public Material unitViewMaterial;
    public Material inWeaponRangeMaterial;



    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = (MeshRenderer)GetComponent<MeshRenderer>();

        LoadMaterials();
        SetMaterials();
        UpdateTexture();
    }

    void LoadMaterials()
    {
        normalMaterial = (Material)Resources.Load("Tex/Materials/HexMaterial");
        objectiveMaterial = (Material)Resources.Load("Tex/Materials/HexObjectiveMaterial");
        roadMaterial = (Material)Resources.Load("Tex/Materials/HexRoadMaterial");

        aiActiveMaterial = (Material)Resources.Load("Tex/Materials/HexActiveMaterial");
        aiShootingMaterial = (Material)Resources.Load("Tex/Materials/HexShootingMaterial");

        blockingMaterial = (Material)Resources.Load("Tex/Materials/HexBlockingMaterial");
        artyTargetMaterial = (Material)Resources.Load("Tex/Materials/HexArtyTargetMaterial");

        shootableTargetMaterial = (Material)Resources.Load("Tex/Materials/HexShootableMaterial");

        visionHighlightMaterial = (Material)Resources.Load("Tex/Materials/HexVisionHighlightMaterial");

        teamVisionHighlightMaterial = (Material)Resources.Load("Tex/Materials/HexTeamVisionHighlightMaterial");
        unitVisionHighlightMaterial = (Material)Resources.Load("Tex/Materials/HexUnitVisionHighlightMaterial");
        weaponRangeHighlightMaterial = (Material)Resources.Load("Tex/Materials/HexWeaponRangeHighlightMaterial");

        //teamViewMaterial = (Material)Resources.Load("Tex/Materials/HexTeamViewMaterial");
        //unitViewMaterial = (Material)Resources.Load("Tex/Materials/HexUnitViewMaterial");
        //inWeaponRangeMaterial = (Material)Resources.Load("Tex/Materials/HexWeaponRangeMaterial");
    }

    public void SetMaterials()
    {
        if (road)
        {
            baseMaterial = roadMaterial;
        } else
        {
            if (objective)
            {
                baseMaterial = objectiveMaterial;
            }
            else
            {
                baseMaterial = normalMaterial;
            }
        }


        Color color;
        
        teamViewMaterial = new Material(Shader.Find("Standard"));
        color = baseMaterial.color;
        color.r = (float)Math.Min(color.r * 1.4, 1.0);
        color.g = (float)Math.Min(color.g * 1.4, 1.0);
        color.b = (float)Math.Min(color.b * 1.4, 1.0);
        teamViewMaterial.color = color;

        unitViewMaterial = new Material(Shader.Find("Standard"));
        color = baseMaterial.color;
        color.r = (float)Math.Min(color.r * 1.75, 1.0);
        color.g = (float)Math.Min(color.g * 1.7, 1.0);
        color.b = (float)Math.Min(color.b * 1.45, 1.0);
        unitViewMaterial.color = color;

        inWeaponRangeMaterial = new Material(Shader.Find("Standard"));
        color = baseMaterial.color;
        color.r = (float)Math.Min(color.r * 1.7, 1.0);
        color.g = (float)Math.Min(color.g * 1.7, 1.0);
        color.b = (float)Math.Min(color.b * 1.4, 1.0);
        inWeaponRangeMaterial.color = color;
        

    }
    
    // Update is called once per frame
    //void Update()
    //{
    //}

    public void ResetFlags()
    {
        isAiActive = false;
        isAiShooting = false;

        isBlocking = false;
        isArtyTarget = false;

        hasShootableTarget = false;
        inUnitViewAndWeaponRange = false;
        inUnitView = false;
        inTeamView = false;

        UpdateTexture();
    }


    public void ResetAiFlags()
    {
        isAiActive = false;
        isAiShooting = false;

        UpdateTexture();
    }

    public void SetAiActive(bool status)
    {
        isAiActive = status;
        UpdateTexture();
    }

    public void SetAiShooting(bool status)
    {
        isAiShooting = status;
        UpdateTexture();
    }

    public void SetBlocking(bool status)
    {
        isBlocking = status;
        UpdateTexture();
    }

    public void SetArtyTarget(bool status)
    {
        isArtyTarget = status;
        UpdateTexture();
    }

    public void SetHasShootableTarget(bool status)
    {
        hasShootableTarget = status;
        UpdateTexture();
    }

    public void SetInWeaponRange(bool status)
    {
        inUnitViewAndWeaponRange = status;
        UpdateTexture();
    }

    public void SetInUnitView(bool status)
    {
        inUnitView = status;
        UpdateTexture();
    }

    public void SetInTeamView(bool status)
    {
        inTeamView = status;
        UpdateTexture();
    }


    public void UpdateTexture()
    {
        if (meshRenderer == null) return; // gaurd against early calls (which are still allowed to set flags)

        // AI
        if (isAiShooting)
        {
            meshRenderer.material = aiShootingMaterial;
            return;
        }
        if (isAiActive)
        {
            meshRenderer.material = aiActiveMaterial;
            return;
        }

        // UI
        if (isBlocking) {
            meshRenderer.material = blockingMaterial;
            return;
        }
        if (isArtyTarget)
        {
            meshRenderer.material = artyTargetMaterial;
            return;
        }

        // Vision
        if (hasShootableTarget) {
            meshRenderer.material = shootableTargetMaterial;
            return;
        }
        if (inUnitViewAndWeaponRange)
        {
            //meshRenderer.material.Lerp(baseMaterial, visionHighlightMaterial, 0.5f);
            //meshRenderer.material.Lerp(baseMaterial, visionHighlightMaterial, 0.8f);
            meshRenderer.material = inWeaponRangeMaterial;
            return;
        }
        if (inUnitView)
        {
            //meshRenderer.material.Lerp(baseMaterial, unitVisionHighlightMaterial, 0.34f);
            //meshRenderer.material.Lerp(baseMaterial, unitVisionHighlightMaterial, 0.6f);
            meshRenderer.material = unitViewMaterial;

            return;
        }
        if (inTeamView)
        {
            //meshRenderer.material.Lerp(baseMaterial, weaponRangeHighlightMaterial, 0.14f);
            //meshRenderer.material.Lerp(baseMaterial, weaponRangeHighlightMaterial, 0.38f);
            meshRenderer.material = teamViewMaterial;

            return;
        }

        // basic
        meshRenderer.material = baseMaterial;
        return;
    }

    public void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }
}
