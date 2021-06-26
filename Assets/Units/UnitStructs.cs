using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cargo
{
    //public Unit unit;

    public int soldiers;
    public int squads;
    public int ammo;
    public List<UnitClass> transportable;
    public int containedSoldiers;
    public List<Unit> containedUnits;
    //public List<Ammo> containedAmmo;

    public Cargo(CargoDef cargoDef)
    {
        soldiers = cargoDef.soldiers;
        squads = cargoDef.squads;
        ammo = cargoDef.ammo;
        transportable = (cargoDef.transportable != null) ? cargoDef.transportable : new List<UnitClass>();
        containedSoldiers = 0;
        containedUnits = new List<Unit>();
    }
    /*public Cargo(int Soldiers, int Squads, int Ammo)
    {
        soldiers = Soldiers;
        squads = Squads;
        ammo = Ammo;
        containedUnits = new List<Unit>();
    }*/
    /*public Cargo(int Soldiers, int Squads)
    {
        soldiers = Soldiers;
        squads = Squads;
        ammo = 100;
        containedUnits = new List<Unit>();
    }*/
    /*public Cargo()
    {
        soldiers = 0;
        squads = 0;
        ammo = 100;
        containedUnits = new List<Unit>();
    }*/

    public void LoadUnit(Unit cargoUnit)
    {
        containedUnits.Add(cargoUnit);
        UpdateContainedSoldiers();
    }
    public void UnloadUnit(Unit cargoUnit)
    {
        containedUnits.Remove(cargoUnit);
        UpdateContainedSoldiers();
    }

    public bool HasCargo()
    {
        return (containedUnits.Count > 0);
    }

    public bool CanLoad(Unit unit)
    {
        //rrr//if ( containedSoldiers + unit.unitDef.soldiers <= soldiers ) return true;
        //rrr//if ( containedSoldiers + unit.maxSoldiers() <= soldiers ) return true;
        if ( containedSoldiers + unit.CurrentSoldiers() <= soldiers ) return true;
        return false;
    }

    void UpdateContainedSoldiers()
    {
        int soldiers = 0;
        foreach (Unit unit in containedUnits)
        {
            soldiers += unit.CurrentSoldiers();
        }
        containedSoldiers = soldiers;
    }
}


public struct VisibleTile
{
    public VisibleTile(int X, int Z, bool Shootable)
    {
        x = X;
        z = Z;
        shootable = Shootable;
    }

    int x;
    int z;
    bool shootable;
}

public class FiringWeapon
{
    public UnitWeapon uWeapon;
    public double expDamage; // expected damage, per weapon, per soldier needed to fire
    public int number;

    public FiringWeapon(UnitWeapon UWeapon, double ExpDamage)
    {
        uWeapon = UWeapon;
        expDamage = ExpDamage;
        number = 0;
    }
}