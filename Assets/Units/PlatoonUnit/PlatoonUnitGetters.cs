using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Unit : MonoBehaviour
{

    public string UnitName() {
        return unitName;
    }
    public string UnitShortName() {
        return unitShortName;
    }
    public string UnitVeryShortName() {
        return unitVeryShortName;
    }

    public Tile Tile() {
        return tile;
    }

    public TargetType UnitTargetType() {
        return targetType;
    }

    public int MaxSoldiers() // return the current number of active soldiers
    {
        int s = 0;
        foreach (SquadDef sd in squadDefs) {
            s += sd.soldiers;
        }
        return s;
    }
       
    public int CurrentSoldiers() // return the current number of active soldiers
    {
        int s = 0;
        foreach (SquadDef sd in squadDefs) {
            s += sd.soldiers;
        }
        return s;
    }
    
    public int Moves()
    {
        return squadDefs[0].moves;
    }

    public int Altitude()
    {
        return altitude;
    }
    
    public int MinAltitude()
    {
        return squadDefs[0].minAltitude;
    }
    public int MaxAltitude()
    {
        return squadDefs[0].maxAltitude;
    }

    public Countermeasures Countermeasures()
    {
        return countermeasures;
    }

    public int Armor()
    {
        return squadDefs[0].armor;
    }

    public double Hitability()
    {
        return hitability;
    }

}
