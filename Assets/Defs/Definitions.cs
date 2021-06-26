using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

interface IUnit {

}

/*public struct DamagePackage
{

}*/

/*/public enum UnitType
{
    US_Mech_Inf,
    Stryker_Inf,
    Light_Inf,
    Bradley,
    Stryker_M2,
    Stryker_Mk19,
    Humvee_M2,
    Humvee_Mk19,
    Abrams,
    US_81mm_Mortar,

    RU_Motor_Inf,
    MT_LB,
    BTR_80,
    BMP_2,
    BMP_3,
    T_72,
    T_80,
    T_90,
    T_90_CM,
}*/

public enum UnitClass
{
    Infantry,
    Gun,
    Car,
    Truck,
    Apc,
    WheeledApc,
    Ifv,
    TrackedApc,
    Carrier,
    HalftrackOpen,
    Halftrack,
    Spat,
    Tank,
    PanzerJager,
    Mortar,
    Artillery,
    ApcMortar,
    WheeledMortar,
    MechMortar,
    MechArtillery,
    WheeledAa,
    TrackedAa,
    Helicopter,
    FixedWing,

    // PS
    Horse,
}

public enum TargetType
{
    Infantry,
    CSW,
    Gun,
    Vehicle,
    Armor,
    Light_Armor,
    Heavy_Armor,
    Helicopter,
    FixedWing,
    Plane,
    
    // PS
    Horse,

    None,
}

/*public class UnitClass
{
    public UnitClass(string Name, int Soldiers, UnitWeapon[] Weapons)
    {
        name = Name;
        soldiers = Soldiers;
        weapons = Weapons;
    }

    public string name;
    public int soldiers;
    public UnitWeapon[] weapons;
}*/





public class Countermeasures
{
    public string name;

    public double kinetic;
    public double super_kinetic;
    public double rocket;
    public double wire;
    public double laser;
    public double radio;
    public double thermal;

    public double topEffectiveness;

    public Countermeasures()
    {
        kinetic = 0.0;
        super_kinetic = 0.0;
        rocket = 0.0;
        wire = 0.0;
        laser = 0.0;
        radio = 0.0;
        thermal = 0.0;

        topEffectiveness = 1.0;
    }

    public Countermeasures(double Rocket, double Wire, double Laser, double Radio, double Thermal)
    {
        kinetic = 0.0;
        super_kinetic = 0.0;
        rocket = Rocket;
        wire = Wire;
        laser = Laser;
        radio = Radio;
        thermal = Thermal;

        topEffectiveness = 1.0;
    }

    public Countermeasures(double Kinetic, double Super_Kinetic, double Rocket, double Wire, double Laser, double Radio, double Thermal)
    {
        kinetic = Kinetic;
        super_kinetic = Super_Kinetic;
        rocket = Rocket;
        wire = Wire;
        laser = Laser;
        radio = Radio;
        thermal = Thermal;

        topEffectiveness = 1.0;
    }

    public double GetCountermeasureStrength(WeaponType weaponType)
    {
        double s = 0.0;

        switch (weaponType)
        {
            case WeaponType.LaunchedGrenade:
                s = rocket;
                break;
            case WeaponType.Rocket:
                s = rocket;
                break;
            case WeaponType.Wire:
                s = wire;
                break;
            case WeaponType.Laser:
                s = laser;
                break;
            case WeaponType.Radio:
                s = radio;
                break;
            case WeaponType.Thermal:
                s = thermal;
                break;
        };

        return s;
    }
}

public struct CargoDef
{
    public int soldiers;
    public int squads;
    public int ammo;
    public List<UnitClass> transportable;

    public CargoDef(int Soldiers, int Squads, int Ammo)
    {
        soldiers = Soldiers;
        squads = Squads;
        ammo = Ammo;
        transportable = new List<UnitClass> {
            UnitClass.Infantry,
            UnitClass.Mortar,
        };
    }
    public CargoDef(int Soldiers, int Squads)
    {
        soldiers = Soldiers;
        squads = Squads;
        ammo = 100;
        transportable = new List<UnitClass> {
            UnitClass.Infantry,
            UnitClass.Mortar,
        };
    }
    /*public CargoDef()
    {
        soldiers = 0;
        squads = 0;
        ammo = 0;
        transportable = new List<UnitClass>();
    }*/

}

public class UnitDef
{
    public List<SquadDef> squads;
    public List<SquadDef> coreSquads;
    public List<SquadDef> mounts;
    public List<SquadDef> coreMounts;
    public bool hasCore;
    public bool hasCoreMounts;

    UnitDef() {
        squads = new List<SquadDef> { };
        coreSquads = new List<SquadDef> { };
        mounts = new List<SquadDef> { };
        coreMounts = new List<SquadDef> { };
        hasCore = false;
        hasCoreMounts = false;
    }
}

public class SquadDef
{
    public string name;
    public string shortName;
    public string veryShortName;
    public string attachmentName;
    public string attachmentShortName;
    public string attachmentVeryShortName;
    //public UnitType unitType;
    public UnitClass unitClass = UnitClass.Infantry;
    public int vehiclesNum;
    public SquadDef[] vehicles;
    public int soldiers;
    public int moves;
    public int setupTime;
    public int packupTime;
    public UnitWeapon[] weapons;
    public WeaponGroup[] weaponGroups;
    public TargetType targetType;
    public double hitability = 1.0;
    public int armor;
    public Countermeasures countermeasures;
    public CargoDef cargoDef;
    public int minAltitude = 0;
    public int maxAltitude = 0;
    public double gainAltCost = 0;
    public double loseAltPayment = 0;
    public FortType fortType = FortType.Infantry;
    public FortDef fortDef = FortDefs.forts[FortType.Infantry];
    public MovementData movementData = Pathfinder.Infantry_MovementData;
    //public MovementData movementData = Pathfinder.WW2_Infantry_MovementData;

    public SquadDef()
    {
        attachmentName = shortName;
        countermeasures = new Countermeasures();
        cargoDef = new CargoDef();
    }

    public SquadDef(string Name, int Soldiers, int Moves, UnitWeapon[] Weapons, TargetType TargetType)
    {
        name = Name;
        shortName = name;
        attachmentName = name;
        attachmentShortName = shortName;
        soldiers = Soldiers;
        moves = Moves;
        weapons = Weapons;
        targetType = TargetType;
        countermeasures = new Countermeasures();
        cargoDef = new CargoDef();
    }

    public SquadDef(string Name, int Soldiers, int Moves, UnitWeapon[] Weapons, TargetType TargetType, Countermeasures Countermeasures)
    {
        name = Name;
        shortName = name;
        attachmentName = name;
        attachmentShortName = shortName;
        soldiers = Soldiers;
        moves = Moves;
        weapons = Weapons;
        targetType = TargetType;
        countermeasures = Countermeasures;
        cargoDef = new CargoDef();
    }

    public SquadDef(string Name, int Soldiers, int Moves, UnitWeapon[] Weapons, TargetType TargetType, Countermeasures Countermeasures, CargoDef Cargo)
    {
        name = Name;
        shortName = name;
        attachmentName = name;
        attachmentShortName = shortName;
        soldiers = Soldiers;
        moves = Moves;
        weapons = Weapons;
        targetType = TargetType;
        countermeasures = Countermeasures;
        cargoDef = Cargo;
    }

    public SquadDef(Dictionary<string, object> Values)
    {
        name = Values.ContainsKey("name") ? (string)Values["name"] : "No Name";
        shortName = Values.ContainsKey("shortName") ? (string)Values["shortName"] : name;
        attachmentName = Values.ContainsKey("attachmentName") ? (string)Values["attachmentName"] : shortName;
        attachmentShortName = Values.ContainsKey("attachmentShortName") ? (string)Values["attachmentShortName"] : shortName;
        unitClass = Values.ContainsKey("unitClass") ? (UnitClass)Values["unitClass"] : UnitClass.Infantry;
        soldiers = Values.ContainsKey("soldiers") ? (int)Values["soldiers"] : 0;
        moves = Values.ContainsKey("moves") ? (int)Values["moves"] : 0;
        weapons = Values.ContainsKey("weapons") ? (UnitWeapon[])Values["weapons"] : new UnitWeapon[] { };
        targetType = Values.ContainsKey("targetType") ? (TargetType)Values["targetType"] : TargetType.None;
        countermeasures = Values.ContainsKey("countermeasures") ? (Countermeasures)Values["countermeasures"] : new Countermeasures();
        cargoDef = Values.ContainsKey("cargo") ? (CargoDef)Values["cargo"] : new CargoDef();
    }
}


public static class Definitions
{
    /// <summary>
    /// US
    /// Weapons
    /// </summary>
    /// 

    static GameObject bullet_s = (GameObject)Resources.Load("GO/Bullet_S");
    static GameObject bullet_m = (GameObject)Resources.Load("GO/Bullet_M");
    static GameObject bullet_l = (GameObject)Resources.Load("GO/Bullet_L");
    static GameObject bullet_vl = (GameObject)Resources.Load("GO/Bullet_VL");
    static GameObject shell = (GameObject)Resources.Load("GO/shell");

    public static SoundProfile defaultSoundProfile = new SoundProfile(SoundLib.rifleShot, 3, 85, 0.15, 1);

    //public static SoundProfile rifleSoundProfile = new SoundProfile(SoundLib.rifleShot, 20, 145, 0.4); // more shooters?
    public static SoundProfile rifleSoundProfile = new SoundProfile(SoundLib.rifleShot, 6, 145, 0.4); // more shooters?

    //public static ShootProfile rifleShootProfile = new ShootProfile(SoundLib.rifleShot, 14, 145, 0.4, 5, true, shell, 18);

    public static ShootProfile rifleShootProfile = new ShootProfile(SoundLib.rifleShot, 5, 60, 0.5, 1, true, bullet_s, 18);

    public static SoundProfile m249SoundProfile = new SoundProfile(SoundLib.m249Shot, 9, 750, 0.05); // more shooters?
    public static ShootProfile m249ShootProfile = new ShootProfile(SoundLib.m249Shot, 26, 750, 0.05, 2, true, bullet_s, 18);
    public static SoundProfile m240SoundProfile = new SoundProfile(SoundLib.m249Shot, 8, 650, 0.05); // more shooters?
    public static ShootProfile m240ShootProfile = new ShootProfile(SoundLib.m249Shot, 12, 650, 0.05, 1, true, bullet_m, 18);
    public static ShootProfile m240TripodShootProfile = new ShootProfile(SoundLib.m249Shot, 17, 650, 0.05, 1, true, bullet_m, 18);

    public static ShootProfile m2ShootProfile = new ShootProfile(SoundLib.m249Shot, 16, 600, 0.05, 1, true, bullet_l, 18);
    public static ShootProfile m3pShootProfile = new ShootProfile(SoundLib.m249Shot, 30, 600, 0.05, 1, true, bullet_l, 18);

    public static SoundProfile rpkSoundProfile = new SoundProfile(SoundLib.m249Shot, 8, 600, 0.05); // more shooters?
    public static ShootProfile rpkShootProfile = new ShootProfile(SoundLib.m249Shot, 11, 600, 0.05, 1, true, bullet_s, 18);
    public static SoundProfile pkSoundProfile = new SoundProfile(SoundLib.m249Shot, 8, 600, 0.05); // more shooters?
    public static ShootProfile pkShootProfile = new ShootProfile(SoundLib.m249Shot, 11, 600, 0.05, 1, true, bullet_m, 18);
    public static ShootProfile pkTripodShootProfile = new ShootProfile(SoundLib.m249Shot, 16, 600, 0.05, 1, true, bullet_m, 18);

    public static ShootProfile dshkShootProfile = new ShootProfile(SoundLib.m249Shot, 16, 600, 0.05, 1, true, shell, 18);

    public static ShootProfile kpvShootProfile = new ShootProfile(SoundLib.m249Shot, 10, 600, 0.05, 1, true, bullet_vl, 20);

    public static ShootProfile m203ShootProfile = new ShootProfile(SoundLib.m249Shot, 3, 20, 0.15, 1, true, shell, 8);
    public static ShootProfile gp25ShootProfile = m203ShootProfile;

    public static SoundProfile bradleySoundProfile = new SoundProfile(SoundLib.bradleyShot, 12, 250, 0.01);
    //public static SoundProfile bradleySoundProfile = new SoundProfile(SoundLib.bradleyShot, 5, 250, 0.01);
    public static ShootProfile bradleyShootProfile = new ShootProfile(SoundLib.bradleyShot, 15, 200, 0.01, 1, true, shell, 20);

    public static SoundProfile towSoundProfile = new SoundProfile(SoundLib.towShot);
    public static SoundProfile javelinSoundProfile = new SoundProfile(SoundLib.javelinShot);

    public static ShootProfile towShootProfile = new ShootProfile(SoundLib.towShot, true, shell, 8, 1.0, 0.0f);
    public static ShootProfile javelinShootProfile = new ShootProfile(SoundLib.javelinShot, true, shell, 4, 1.28, 0.75f);


    //public static SoundProfile abramsSoundProfile = new SoundProfile(SoundLib.abramsShot, 3, 26, 0.1);
    //public static SoundProfile abramsSoundProfile = new SoundProfile(SoundLib.abramsShot, 3, 85, 0.1);
    public static SoundProfile abramsSoundProfile = new SoundProfile(SoundLib.abramsShot, 3, 25, 0.1);
    public static ShootProfile abramsShootProfile = new ShootProfile(SoundLib.abramsShot, 3, 25, 0.1, 1, true, shell, 24);

    public static ShootProfile _60mmMortarShootProfile = new ShootProfile(SoundLib.abramsShot, 5, 41, 0.1, 1, true, shell, 12);
    public static ShootProfile _60mmMortarImpactProfile = new ShootProfile(SoundLib.abramsShot, 1, 1, 0.0, 1, true, shell, 10);

    public static ShootProfile _81mmMortarShootProfile = new ShootProfile(SoundLib.abramsShot, 4, 35, 0.1, 1, true, shell, 14);
    public static ShootProfile _81mmMortarImpactProfile = new ShootProfile(SoundLib.abramsShot, 1, 1, 0.0, 1, true, shell, 12);

    public static ShootProfile _82mmMortarShootProfile = new ShootProfile(SoundLib.abramsShot, 4, 35, 0.1, 1, true, shell, 16);
    public static ShootProfile _82mmMortarImpactProfile = new ShootProfile(SoundLib.abramsShot, 1, 1, 0.0, 1, true, shell, 14);

    public static ShootProfile _120mmMortarShootProfile = new ShootProfile(SoundLib.abramsShot, 3, 26, 0.1, 1, true, shell, 16);
    public static ShootProfile _120mmMortarImpactProfile = new ShootProfile(SoundLib.abramsShot, 1, 1, 0.0, 1, true, shell, 14);

    public static ShootProfile _mstaArtyShootProfile = new ShootProfile(SoundLib.abramsShot, 3, 26, 0.1, 1, true, shell, 20);
    public static ShootProfile _mstaArtyImpactProfile = new ShootProfile(SoundLib.abramsShot, 1, 1, 0.0, 1, true, shell, 18);

    //public static SoundProfile abramsSoundProfile = new SoundProfile(SoundLib.m249Shot, 9, 750, 0.05);

    public static SoundProfile _2a42SoundProfile = new SoundProfile(SoundLib.bradleyShot, 18, 300, 0.05);
    //public static SoundProfile _2a42SoundProfile = new SoundProfile(SoundLib.bradleyShot, 4, 300, 0.05);
    public static SoundProfile _2a72SoundProfile = new SoundProfile(SoundLib.bradleyShot, 18, 300, 0.05);
    //public static SoundProfile _2a72SoundProfile = new SoundProfile(SoundLib.bradleyShot, 4, 300, 0.05);
    //public static SoundProfile _2a46mSoundProfile = new SoundProfile(SoundLib.abramsShot, 3, 26, 0.1);
    //public static SoundProfile _2a46mSoundProfile = new SoundProfile(SoundLib.abramsShot, 1, 26, 0.1);
    //public static SoundProfile _2a46mSoundProfile = new SoundProfile(SoundLib.abramsShot, 3, 85, 0.1);
    public static SoundProfile _2a46mSoundProfile = new SoundProfile(SoundLib.abramsShot, 3, 25, 0.1);
    
                                                                                        //Sh//rpm//flux//num//proj//pOjb//pSpeed
    //public static ShootProfile _2a42ShootProfile = new ShootProfile(SoundLib.bradleyShot, 18, 250, 0.01, 1, true, shell, 20);

    public static ShootProfile _2a42ShootProfile = new ShootProfile
    {
        clip = SoundLib.bradleyShot,
        repetitions = 18,
        rpm = 250,
        flux = 0.01,
        aimMin = 0.0,
        aimMax = 0.5,
        shooters = 1,
        projectile = true,
        shellObj = shell,
        speed = 20 * 100f
    };

    public static ShootProfile _2a72ShootProfile = new ShootProfile
    {
        clip = SoundLib.bradleyShot,
        repetitions = 20,
        rpm = 330,
        flux = 0.01,
        aimMin = 0.0,
        aimMax = 0.5,
        shooters = 1,
        projectile = true,
        shellObj = shell,
        speed = 20 * 100f
    };

    public static ShootProfile _gsh302kShootProfile = new ShootProfile(SoundLib.bradleyShot, 20, 330, 0.01, 1, true, shell, 20);
    public static ShootProfile _gsh302kFastShootProfile = new ShootProfile(SoundLib.bradleyShot, 50, 2300, 0.01, 1, true, shell, 20);
    public static ShootProfile _gsh301ShootProfile = new ShootProfile(SoundLib.bradleyShot, 45, 1800, 0.01, 1, true, shell, 20);
    public static ShootProfile _2a46mShootProfile = abramsShootProfile;

    public static ShootProfile _2a7ShootProfile = new ShootProfile(SoundLib.bradleyShot, 50, 900, 0.0, 1, true, shell, 20);
    public static ShootProfile _2a38ShootProfile = new ShootProfile(SoundLib.bradleyShot, 50, 2250, 0.0, 1, true, shell, 20);

    public static ShootProfile _2a70ShootProfile = new ShootProfile(SoundLib.abramsShot, 3, 22, 0.06, 1, true, shell, 10);
    public static ShootProfile _mstaShootProfile = new ShootProfile(SoundLib.abramsShot, 3, 22, 0.06, 1, true, shell, 20);

    public static ShootProfile _2a51DirectShootProfile = _2a70ShootProfile;

    public static ShootProfile _2a28ShootProfile = new ShootProfile(SoundLib.abramsShot, 2, 16, 0.15, 1, true, shell, 9);

    

    public static ShootProfile at4ShootProfile = new ShootProfile(SoundLib.abramsShot, 1, 85, 0.1, 1, true, shell, 15);

    public static SoundProfile rpg7SoundProfile = new SoundProfile(SoundLib.abramsShot, 2, 85, 0.1);
    public static ShootProfile rpg7ShootProfile = new ShootProfile(SoundLib.abramsShot, 2, 16, 0.1, 1, true, shell, 14);

    public static ShootProfile hydraShootProfile = new ShootProfile(SoundLib.abramsShot, 6, 120, 0.05, 1, true, shell, 10);
    public static ShootProfile s5ShootProfile = new ShootProfile(SoundLib.abramsShot, 7, 120, 0.05, 1, true, shell, 10);

    public static SoundProfile rpg22SoundProfile = new SoundProfile(SoundLib.abramsShot, 1, 85, 0.1);
    public static SoundProfile rpg26SoundProfile = rpg22SoundProfile;

    public static ShootProfile rpg22ShootProfile = new ShootProfile(SoundLib.abramsShot, 1, 85, 0.1, 1, true, shell, 12);
    public static ShootProfile rpg26ShootProfile = rpg22ShootProfile;

    //public static SoundProfile ags17SoundProfile = new SoundProfile(SoundLib.m249Shot, 10, 100, 0.1);
    //public static SoundProfile ags30SoundProfile = ags17SoundProfile;

    public static ShootProfile ags17ShootProfile = new ShootProfile(SoundLib.m249Shot, 12, 400, 0.03, 1, true, shell, 9);

    public static ShootProfile metisShootProfile = towShootProfile;
    public static ShootProfile malyutkaShootProfile = towShootProfile;
    public static ShootProfile konkursShootProfile = towShootProfile;
    public static ShootProfile kornetShootProfile = towShootProfile;
    public static ShootProfile khrizantemaShootProfile = kornetShootProfile;

    public static ShootProfile bastionShootProfile = towShootProfile;
    public static ShootProfile kobraShootProfile = towShootProfile;
    public static ShootProfile refleksShootProfile = towShootProfile;

    public static ShootProfile shturmShootProfile = towShootProfile;
    public static ShootProfile atakaShootProfile = towShootProfile;

    public static ShootProfile kh25ShootProfile = new ShootProfile(SoundLib.towShot, true, shell, 18, 1.0, 0.0f);
    public static ShootProfile kh38ShootProfile = kh25ShootProfile;

    public static ShootProfile strelaShootProfile = towShootProfile;
    public static ShootProfile iglaShootProfile = strelaShootProfile;
    public static ShootProfile osaShootProfile = towShootProfile;
    public static ShootProfile torShootProfile = osaShootProfile;
    public static ShootProfile bukShootProfile = osaShootProfile;

    public static ShootProfile r73ShootProfile = new ShootProfile(SoundLib.towShot, true, shell, 25, 1.0, 0.0f);

    public static double rifle_acc = 0.15;
    public static double rifle_rfac = 0.8;
    public static double rifle_vs_helo_acc = rifle_acc;
    public static double rifle_vs_helo_rfac = rifle_rfac;

    public static double lmg_acc = 0.09;
    public static double lmg_rfac = 0.8;
    public static double lmg_vs_helo_acc = lmg_acc;
    public static double lmg_vs_helo_rfac = lmg_rfac;

    public static double hlmg_acc = 0.09;
    public static double hlmg_rfac = 0.9;
    public static double hlmg_vs_helo_acc = lmg_acc;
    public static double hlmg_vs_helo_rfac = lmg_rfac;

    public static double mmg_acc = 0.1;
    public static double mmg_rfac = 1.0;
    public static double mmg_vs_helo_acc = mmg_acc;
    public static double mmg_vs_helo_rfac = mmg_rfac;

    // bipod / tripod?
    /**public static double mmg_acc = 0.05;
    public static double mmg_rfac = 1.0;
    public static double mmg_vs_helo_acc = mmg_acc;
    public static double mmg_vs_helo_rfac = mmg_rfac;*/

    public static double mmg_vec_acc = 0.13;
    public static double mmg_vec_rfac = 1.0;
    public static double mmg_vec_vs_helo_acc = 0.05;
    public static double mmg_vec_vs_rfa = mmg_vec_rfac;

    public static double hmg_acc = 0.15;
    public static double hmg_rfac = 1.1;
    public static double hmg_vs_helo_acc = 0.05;
    public static double hmg_vs_helo_rfac = hmg_rfac;

    public static double hmg_aa_acc = MultiplyAndRoundAcc(hmg_acc, 0.6);
    public static double hmg_aa_rfac = MultiplyAndRoundAcc(hmg_rfac, 0.8);
    public static double hmg_aa_vs_helo_acc = 0.3;
    public static double hmg_aa_vs_helo_rfac = 1.0;

    public static double vhmg_acc = 0.15;
    public static double vhmg_rfac = 1.1;
    public static double vhmg_vs_helo_acc = 0.05;
    public static double vhmg_vs_helo_rfac = vhmg_rfac;

    public static double gl_acc = 0.4;
    public static double gl_rfac = 0.6;

    public static double agl_acc = 0.4;
    public static double agl_rfac = 0.7;

    public static double auto_acc = 0.55;
    public static double auto_rfac = 1.7;
    public static double auto_vs_helo_acc = auto_acc - 0.3;
    public static double auto_vs_helo_rfac = auto_rfac;

    public static double aa_radar_auto_acc = DivideAndRoundAcc(auto_acc, 2.0);
    public static double aa_radar_auto_rfac = MultiplyAndRoundAcc(auto_rfac, 0.8);
    public static double aa_radar_auto_vs_helo_acc = 0.4;
    public static double aa_radar_auto_vs_helo_rfac = 1.4;

    public static double heli_auto_acc = auto_acc - 0.1;
    public static double heli_auto_rfac = auto_rfac - 0.1;
    
    public static double air_auto_acc = DivideAndRoundAcc(heli_auto_acc, 2.0);
    public static double air_auto_rfac = heli_auto_rfac - 0.4;

    public static double frag_cannon_acc = 0.6;
    public static double frag_cannon_rfac = 1.7;

    public static double cannon_acc = 0.8;
    public static double cannon_rfac = 3.0;
    public static double cannon_vs_helo_acc = 0.1;
    public static double cannon_vs_helo_rfac = 1.2;

    public static double rocket_acc = 0.55;
    public static double rocket_rfac = 0.5;
    public static double rocket_vs_helo_acc = DivideAndRoundAcc(rocket_acc, 3.0);
    public static double rocket_vs_helo_rfac = rocket_rfac;

    public static double heli_rocket_acc = rocket_acc - 0.2;
    public static double heli_rocket_rfac = rocket_rfac;

    public static double missile_acc = 0.7;
    public static double missile_rfac = 7.3;

    public static double heli_missile_acc = missile_acc;
    public static double heli_missile_rfac = missile_rfac + 2;

    public static double thermal_missile_acc = 0.75;
    public static double thermal_missile_rfac = missile_rfac + 4.5;

    public static double air_ground_missile_acc = heli_missile_acc;
    public static double air_ground_missile_rfac = heli_missile_rfac + 3;
    
    public static double aa_missile_acc = 0.7;
    public static double aa_missile_rfac = 14.0;

    public static double air_air_missile_acc = aa_missile_acc;
    public static double air_air_missile_rfac = aa_missile_rfac;

    public static double air_air_missile_vs_helo_acc = aa_missile_acc - 0.1;
    public static double air_air_missile_vs_helo_rfac = aa_missile_rfac - 1.0;

    public static double mortar_acc = 0.2;
    public static double mortar_rfac = 1;

    public static double howitzer_acc = 0.5;
    public static double howitzer_rfac = 1;

    public static double arty_cannon_acc = 0.5;
    public static double arty_cannon_rfac = 1;

    public static double DivideAndRoundAcc(double acc, double divide)
    {
        double t = acc / divide;
        t = Math.Round(t * 20.0) / 20.0;
        return t;
    }
    public static double MultiplyAndRoundAcc(double acc, double multiply)
    {
        double t = acc * multiply;
        t = Math.Round(t * 20.0) / 20.0;
        return t;
    }


    public static class Cm
    {
        public static Countermeasures Drozd = new Countermeasures
        {
            name = "Drozd",
            kinetic = 0.0,
            super_kinetic = 0.0,
            rocket = 0.5,
            wire = 0.5,
            laser = 0.5,
            radio = 0.5,
            thermal = 0.5,

            topEffectiveness = 0.2,
        };

        public static Countermeasures Shtora = new Countermeasures
        {
            name = "Shtora-1",
            kinetic = 0.0,
            super_kinetic = 0.0,
            rocket = 0.0,
            wire = 0.0,
            laser = 0.5,
            radio = 0.0,
            thermal = 0.0,

            topEffectiveness = 0.25,
        };

        public static Countermeasures Arena = new Countermeasures
        {
            name = "Arena",
            kinetic = 0.0,
            super_kinetic = 0.0,
            rocket = 0.75,
            wire = 0.75,
            laser = 0.75,
            radio = 0.75,
            thermal = 0.75,

            topEffectiveness = 0.25,
        };

        public static Countermeasures Afghanit = new Countermeasures
        {
            name = "Afghanit",
            kinetic = 0.75,
            super_kinetic = 0.5,
            rocket = 0.8,
            wire = 0.8,
            laser = 0.8,
            radio = 0.8,
            thermal = 0.8,

            topEffectiveness = 0.25,
        };

        public static Countermeasures Trophy = new Countermeasures
        {
            name = "Trophy",
            kinetic = 0.0,
            super_kinetic = 0.0,
            rocket = 0.8,
            wire = 0.8,
            laser = 0.8,
            radio = 0.8,
            thermal = 0.8,

            topEffectiveness = 1.0,
        };

    }

    public static Weapon M4 = new Weapon
    {
        id = "m4",
        name = "M4",
        range = Weapon.RealRange(500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(7, rifle_acc, 1, 1.0, rifle_rfac) },
            { TargetType.Helicopter, new DamageProfile(7, rifle_vs_helo_acc, 1, 1.0, rifle_vs_helo_rfac) }
        },
        penetration = 6,
        soundProfile = rifleSoundProfile,
        shootProfile = rifleShootProfile
    };

    /*public static Weapon M4 = new Weapon("M4", 3, new Dictionary<TargetType, DamageProfile> {
        { TargetType.Infantry, new DamageProfile(10, 0.5, 1) }
    });*/

    /*public static Weapon M249 = new Weapon("M249", 4, new Dictionary<TargetType, DamageProfile> {
        { TargetType.Infantry, new DamageProfile(20, 0.5, 1) }
    });*/

    public static Weapon M249 = new Weapon
    {
        id = "m249",
        name = "M249",
        range = Weapon.RealRange(600),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(30, lmg_acc, 1, 1.0, lmg_rfac) }
        },
        penetration = 6,
        soundProfile = m249SoundProfile,
        shootProfile = m249ShootProfile
    };


    /*public static Weapon M240 = new Weapon("M240", 6, new Dictionary<TargetType, DamageProfile> {
        { TargetType.Infantry, new DamageProfile(20, 0.6, 1) }
    });*/

    public static Weapon M240 = new Weapon
    {
        id = "m240",
        name = "M240",
        range = Weapon.RealRange(1000),
        soldiersToFire = 2,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(25, hlmg_acc, 1, 1.0, hlmg_rfac) }
        },
        penetration = 10,
        soundProfile = m240SoundProfile,
        shootProfile = m240ShootProfile,
    };

    public static Weapon M240_Tripod = new Weapon
    {
        id = "m240_tripod",
        name = "M240",
        range = Weapon.RealRange(1200),
        soldiersToFire = 2,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(25, mmg_acc, 1, 1.0, mmg_rfac) }
        },
        penetration = 10,
        shootProfile = m240TripodShootProfile,
    };

    public static Weapon M203 = new Weapon
    {
        id = "m203",
        name = "M203",
        range = Weapon.RealRange(350),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, gl_acc, 1, 1.0, gl_rfac) },
            { TargetType.Light_Armor, new DamageProfile(3, gl_acc, 1, 1.0, gl_rfac) }
        },
        penetration = 40,
        weaponType = WeaponType.LaunchedGrenade,
        shootProfile = m203ShootProfile
    };

    public static Weapon AT4 = new Weapon
    {
        id = "at4",
        name = "M136 AT",
        range = Weapon.RealRange(500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Light_Armor, new DamageProfile(1, rocket_acc + 0.1, 3, 1, rocket_rfac + 0.1) },
            { TargetType.Heavy_Armor, new DamageProfile(1, rocket_acc + 0.1, 1, 1, rocket_rfac + 0.1) }
        },
        penetration = 500,
        weaponType = WeaponType.Rocket,
        shootProfile = at4ShootProfile
    };

    public static Weapon Hydra = new Weapon
    {
        id = "hydra",
        name = "Hydra Rocket",
        range = Weapon.RealRange(1500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(6, heli_rocket_acc, 2, 1.0, heli_rocket_rfac) },
            { TargetType.Light_Armor, new DamageProfile(6, heli_rocket_acc, 2, 1.0, heli_rocket_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(6, heli_rocket_acc, 1, 1.0, heli_rocket_rfac) }
        },
        penetration = 50,
        weaponType = WeaponType.Rocket,
        soundProfile = abramsSoundProfile,
        shootProfile = hydraShootProfile
    };

    public static Weapon Javelin = new Weapon
    {
        id = "javelin",
        name = "Javelin",
        range = Weapon.RealRange(2500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Light_Armor, new DamageProfile(1, thermal_missile_acc, 2, 6, 1.0, thermal_missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, thermal_missile_acc, 2, 6, 1.0, thermal_missile_rfac) }
        },
        penetration = 1600,
        weaponType = WeaponType.Thermal,
        shootProfile = javelinShootProfile
    };


    public static Weapon AFV_M240 = new Weapon
    {
        id = "m240_afv",
        name = "M240",
        range = Weapon.RealRange(1200),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(40, mmg_vec_acc, 1) }
        },
        penetration = 10,
        soundProfile = m240SoundProfile,
        shootProfile = m240TripodShootProfile
    };

    public static Weapon M2 = new Weapon
    {
        id = "m2",
        name = "M2 .50 Cal",
        range = Weapon.RealRange(1500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(30, hmg_acc, 1, 1.0, hmg_rfac) },
            { TargetType.Light_Armor, new DamageProfile(30, hmg_acc, 1, 1.0, hmg_rfac) },
            { TargetType.Helicopter, new DamageProfile(30, hmg_vs_helo_acc, 1, 1.0, hmg_vs_helo_rfac) },
        },
        penetration = 20,
        shootProfile = m2ShootProfile
    };
    public static Weapon M3P = new Weapon
    {
        id = "m3p",
        name = "M3P .50 Cal",
        range = Weapon.RealRange(1500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(40, hmg_aa_acc, 1, 1.0, hmg_aa_rfac) },
            { TargetType.Light_Armor, new DamageProfile(40, hmg_aa_acc, 1, 1.0, hmg_aa_rfac) },
            { TargetType.Helicopter, new DamageProfile(40, hmg_aa_vs_helo_acc, 1, 1.0, hmg_aa_vs_helo_rfac) },
        },
        penetration = 20,
        shootProfile = m3pShootProfile
    };

    public static Weapon MK19 = new Weapon
    {
        id = "mk19",
        name = "MK19",
        range = Weapon.RealRange(1500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(10, agl_acc, 2, 1.0, agl_rfac) },
            { TargetType.Light_Armor, new DamageProfile(10, agl_acc, 1, 1.0, agl_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(10, agl_acc, 1, 1.0, agl_rfac) }
        },
        penetration = 40,
        soundProfile = m240SoundProfile
    };

    /*public static Weapon Bushmaster = new Weapon("Bushmaster 25mm", 15, new Dictionary<TargetType, DamageProfile> {
        { TargetType.Infantry, new DamageProfile(12, 0.65, 1) },
        { TargetType.Light_Armor, new DamageProfile(12, 0.65, 2) },
        { TargetType.Heavy_Armor, new DamageProfile(12, 0.65, 1, 0.3333) }
    }, WeaponType.Shell);*/

    public static Weapon Bushmaster = new Weapon
    {
        id = "bushmaster",
        name = "Bushmaster 25mm",
        range = Weapon.RealRange(2500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(15, auto_acc, 1, 1.0, auto_rfac) },
            { TargetType.Light_Armor, new DamageProfile(15, auto_acc, 1, 2, 1.0, auto_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(15, auto_acc, 1, 1.0, auto_rfac) },
            { TargetType.Helicopter, new DamageProfile(15, auto_vs_helo_acc, 2, 1.0, auto_vs_helo_rfac) }
        },
        penetration = 100,
        weaponType = WeaponType.Shell,
        shootProfile = bradleyShootProfile
    };

    public static Weapon M230 = new Weapon
    {
        id = "m230",
        name = "M230 30mm",
        range = Weapon.RealRange(2500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(15, heli_auto_acc, 1, 1.0, heli_auto_rfac) },
            { TargetType.Light_Armor, new DamageProfile(15, heli_auto_acc, 1, 1, 1.0, heli_auto_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(15, heli_auto_acc, 1, 1, 1.0, heli_auto_rfac) }
        },
        penetration = 60,
        weaponType = WeaponType.Shell,
        shootProfile = bradleyShootProfile
    };

    public static Weapon Bradley_TOW = new Weapon
    {
        id = "tow",
        name = "TOW",
        range = Weapon.RealRange(3749), // should be 3750
        //minRange = 2,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            //{ TargetType.Infantry, new DamageProfile(1, missile_acc, 1, 3, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 1000,
        weaponType = WeaponType.Wire,
        shootProfile = towShootProfile
    };

    public static Weapon Hellfire = new Weapon
    {
        id = "hellfire",
        name = "Hellfire",
        range = Weapon.RealRange(8000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            //{ TargetType.Infantry, new DamageProfile(1, heli_missile_acc, 2, 5, 1.0, heli_missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, heli_missile_acc, 2, 7, 1.0, heli_missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, heli_missile_acc, 2, 7, 1.0, heli_missile_rfac) }
        },
        penetration = 200 + 950,
        weaponType = WeaponType.Laser,
        shootProfile = towShootProfile
    };

    public static Weapon M256 = new Weapon
    {
        id = "m256",
        name = "M256 120mm",
        range = Weapon.RealRange(4000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, cannon_acc, 1, 5, 1.0, cannon_rfac) },
            { TargetType.Light_Armor, new DamageProfile(3, cannon_acc, 1, 5, 1.0, cannon_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(3, cannon_acc, 1, 5, 1.0, cannon_rfac) }
        },
        penetration = 900,
        weaponType = WeaponType.Shell,
        shootProfile = abramsShootProfile
    };

    public static Weapon M224_Mortar = new Weapon("m224", "M224 60mm Mtr", 0, 3490);
    public static Weapon M252_Mortar = new Weapon
    {
        id = "m252",
        name = "M252 81mm Mtr",
        //range = Weapon.RealRange(5935), // (36?)
        direct = false,
        indirect = true,
        indirectWeapon = new Weapon
        {
            range = Weapon.RealRange(5935), // (36?)
            direct = false,
            indirect = true,
            impactDelay = 1,
            aimTime = 1,
            flightTime = 1,
            //minSalvos = 1,
            //maxSalvos = 5,
            setupTime = 2,
            packupTime = 1,
            artyShellsPerSalvo = 4,
            artyDelayPerShell = 2.0,
            damageProfiles = new Dictionary<TargetType, DamageProfile> {
                { TargetType.Infantry, new DamageProfile(1, mortar_acc, 1, 3, 3.0, mortar_rfac) },
                { TargetType.Light_Armor, new DamageProfile(1, mortar_acc, 1, 3, 1.0, mortar_rfac) },
                { TargetType.Heavy_Armor, new DamageProfile(1, mortar_acc, 1, 3, 1.0, mortar_rfac) }
            },
            penetration = 150,
            weaponType = WeaponType.Mortar,
            //soundProfile = abramsSoundProfile,
            shootProfile = _81mmMortarShootProfile,
            impactProfile = _81mmMortarImpactProfile
        }
    };

    public static Weapon M120_Mortar = new Weapon
    {
        id = "m120",
        name = "M120 120mm Mtr",
        //range = Weapon.RealRange(7240), // (36?)
        direct = false,
        indirect = true,
        indirectWeapon = new Weapon
        {
            range = Weapon.RealRange(7240), // (36?)
            direct = false,
            indirect = true,
            impactDelay = 1,
            aimTime = 1,
            flightTime = 1,
            //minSalvos = 1,
            //maxSalvos = 5,
            setupTime = 3,
            packupTime = 2,
            artyShellsPerSalvo = 3,
            artyDelayPerShell = 2.0,
            damageProfiles = new Dictionary<TargetType, DamageProfile> {
                { TargetType.Infantry, new DamageProfile(1, mortar_acc, 1, 4, 3.0, mortar_rfac) },
                { TargetType.Light_Armor, new DamageProfile(1, mortar_acc, 1, 4, 1.0, mortar_rfac) },
                { TargetType.Heavy_Armor, new DamageProfile(1, mortar_acc, 1, 4, 1.0, mortar_rfac) }
            },
            penetration = 250,
            weaponType = WeaponType.Mortar,
            //soundProfile = abramsSoundProfile,
            shootProfile = _120mmMortarShootProfile,
            impactProfile = _120mmMortarImpactProfile
        }
    };

    public static Weapon M119_How = new Weapon("m119", "M119 105mm How", 0, 17500);/* = new Weapon {
        name = "M120 120mm Mtr",
        //range = Weapon.RealRange(7240), // (36?)
        direct = false,
        indirect = true,
        indirectWeapon = new Weapon
        {
            range = Weapon.RealRange(7240), // (36?)
            direct = false,
            indirect = true,
            impactDelay = 1,
            aimTime = 1,
            flightTime = 1,
            //minSalvos = 1,
            //maxSalvos = 5,
            setupTime = 3,
            packupTime = 2,
            artyShellsPerSalvo = 3,
            artyDelayPerShell = 2.0,
            damageProfiles = new Dictionary<TargetType, DamageProfile> {
                { TargetType.Infantry, new DamageProfile(1, mortar_acc, 1, 4, 3.0, mortar_rfac) },
                { TargetType.Light_Armor, new DamageProfile(1, mortar_acc, 1, 4, 1.0, mortar_rfac) },
                { TargetType.Heavy_Armor, new DamageProfile(1, mortar_acc, 1, 4, 1.0, mortar_rfac) }
            },
            penetration = 250,
            weaponType = WeaponType.Mortar,
            //soundProfile = abramsSoundProfile,
            shootProfile = _120mmMortarShootProfile,
            impactProfile = _120mmMortarImpactProfile
        }
    };*/
    public static Weapon M777_How = new Weapon("m777", "M777 155mm How", 0, 22500); /*= new Weapon {
        name = "M120 120mm Mtr",
        //range = Weapon.RealRange(7240), // (36?)
        direct = false,
        indirect = true,
        indirectWeapon = new Weapon
        {
            range = Weapon.RealRange(7240), // (36?)
            direct = false,
            indirect = true,
            impactDelay = 1,
            aimTime = 1,
            flightTime = 1,
            //minSalvos = 1,
            //maxSalvos = 5,
            setupTime = 3,
            packupTime = 2,
            artyShellsPerSalvo = 3,
            artyDelayPerShell = 2.0,
            damageProfiles = new Dictionary<TargetType, DamageProfile> {
                { TargetType.Infantry, new DamageProfile(1, mortar_acc, 1, 4, 3.0, mortar_rfac) },
                { TargetType.Light_Armor, new DamageProfile(1, mortar_acc, 1, 4, 1.0, mortar_rfac) },
                { TargetType.Heavy_Armor, new DamageProfile(1, mortar_acc, 1, 4, 1.0, mortar_rfac) }
            },
            penetration = 250,
            weaponType = WeaponType.Mortar,
            //soundProfile = abramsSoundProfile,
            shootProfile = _120mmMortarShootProfile,
            impactProfile = _120mmMortarImpactProfile
        }
    };*/
    public static Weapon M284_How = new Weapon("m284", "M284 155mm How", 0, 24000); /*= new Weapon {
        name = "M120 120mm Mtr",
        //range = Weapon.RealRange(24000), // RAP 30 000 
        direct = false,
        indirect = true,
        indirectWeapon = new Weapon
        {
            range = Weapon.RealRange(24000), // RAP 30 000 
            direct = false,
            indirect = true,
            impactDelay = 1,
            aimTime = 1,
            flightTime = 1,
            //minSalvos = 1,
            //maxSalvos = 5,
            setupTime = 3,
            packupTime = 2,
            artyShellsPerSalvo = 3,
            artyDelayPerShell = 2.0,
            damageProfiles = new Dictionary<TargetType, DamageProfile> {
                { TargetType.Infantry, new DamageProfile(1, mortar_acc, 1, 4, 3.0, mortar_rfac) },
                { TargetType.Light_Armor, new DamageProfile(1, mortar_acc, 1, 4, 1.0, mortar_rfac) },
                { TargetType.Heavy_Armor, new DamageProfile(1, mortar_acc, 1, 4, 1.0, mortar_rfac) }
            },
            penetration = 250,
            weaponType = WeaponType.Mortar,
            //soundProfile = abramsSoundProfile,
            shootProfile = _120mmMortarShootProfile,
            impactProfile = _120mmMortarImpactProfile
        }
    };*/


    
    public static Weapon Stinger = new Weapon
    {
        id = "stinger",
        name = "Stinger-F",
        range = Weapon.RealRange(4800),
        minRange = Weapon.RealRange(200),
        ceiling = Weapon.MissileCeiling(3800),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 1, 6, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = iglaShootProfile
    };


    /// <summary>
    /// Units
    /// </summary>

    // short name Mch Inf
    /*public static SquadDef US_Mech_Inf = new SquadDef("Mechanized Infantry", 9, 1, new UnitWeapon[] {
        new UnitWeapon(7, M4, 10),
        new UnitWeapon(2, M249, 10),
        new UnitWeapon(2, M203, 10),
        new UnitWeapon(2, AT4, 2),
        new UnitWeapon(1, Javelin, 2)
    }, TargetType.Infantry);*/

    public static double Infantry_Hitability = 0.6;

    public static SquadDef US_Stryker_Inf = new SquadDef
    {
        name = "Stryker Infantry",
        shortName = "Skr Inf",
        soldiers = 9,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(7, M4, 10),
            new UnitWeapon(2, M249, 10),
            new UnitWeapon(2, M203, 10),
            new UnitWeapon(4, AT4, 1),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef US_Stryker_Weapons = new SquadDef
    {
        name = "Stryker Weapons Inf",
        shortName = "Skr Wps",
        soldiers = 9,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(7, M4, 10),
            new UnitWeapon(2, M240, 20),
            new UnitWeapon(2, Javelin, 2),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef US_Stryker_Weapons_AT = new SquadDef
    {
        name = "Stryker Weapons Inf",
        shortName = "Skr Wps AT",
        soldiers = 4,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(2, M4, 10),
            new UnitWeapon(2, Javelin, 2),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef US_Stryker_Weapons_MMG = new SquadDef
    {
        name = "Stryker Weapons Inf",
        shortName = "Skr Wps MG",
        soldiers = 5,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(3, M4, 10),
            new UnitWeapon(2, M240, 20),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    
    public static SquadDef US_Mech_Inf_Plt_Hq = new SquadDef
    {
        name = "Mechanized Infantry HQ",
        shortName = "Mch Inf HQ",
        soldiers = 1,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, M4, 10),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef US_Mech_Inf = new SquadDef
    {
        name = "Mechanized Infantry",
        shortName = "Mch Inf",
        soldiers = 9,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(7, M4, 10),
            new UnitWeapon(2, M249, 10),
            new UnitWeapon(2, M203, 10),
            new UnitWeapon(4, AT4, 1),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef US_Mech_Inf_Jav = new SquadDef
    {
        name = "Mechanized Infantry",
        shortName = "Mch Inf",
        soldiers = 9,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(7, M4, 10),
            new UnitWeapon(2, M249, 10),
            new UnitWeapon(2, M203, 10),
            new UnitWeapon(2, AT4, 1),
            new UnitWeapon(1, Javelin, 2)
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef US_Mech_Inf_M240 = new SquadDef
    {
        name = "Mechanized Infantry",
        shortName = "Mch Inf",
        soldiers = 9,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(6, M4, 10),
            new UnitWeapon(2, M249, 10),
            new UnitWeapon(1, M240, 10),
            new UnitWeapon(2, M203, 10),
            new UnitWeapon(4, AT4, 1)
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef US_Mech_Inf_Hvy = new SquadDef
    {
        name = "Mechanized Infantry",
        shortName = "Mch Inf",
        soldiers = 9,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(6, M4, 10),
            new UnitWeapon(2, M249, 10),
            new UnitWeapon(1, M240, 10),
            new UnitWeapon(2, M203, 10),
            new UnitWeapon(2, AT4, 1),
            new UnitWeapon(1, Javelin, 2)
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef US_Mech_Plt_Hq = new SquadDef
    {
        name = "Mech Plt HQ",
        shortName = "Plt HQ",
        soldiers = 1,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, M4, 10),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    // short name MMG
    public static SquadDef US_MMG_Squad = new SquadDef
    {
        name = "MMG Squad",
        soldiers = 7,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(5, M4, 10),
            new UnitWeapon(2, M240, 20),
            new UnitWeapon(1, M203, 10),
            new UnitWeapon(2, AT4, 2)
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    // short name MMG A
    public static SquadDef US_MMG_Team_A = new SquadDef
    {
        name = "MMG Team (A)",
        soldiers = 4,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(3, M4, 10),
            new UnitWeapon(1, M240, 20),
            new UnitWeapon(1, M203, 10),
            new UnitWeapon(1, AT4, 2)
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    // short name MMG B
    public static SquadDef US_MMG_Team_B = new SquadDef
    {
        name = "MMG Team (B)",
        soldiers = 3,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(2, M4, 10),
            new UnitWeapon(1, M240, 20),
            new UnitWeapon(1, AT4, 2)
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    
    public static SquadDef US_Stinger_HQ = new SquadDef
    {
        name = "Stinger HQ",
        shortName = "Str HQ",
        soldiers = 4,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(4, M4, 8),
            new UnitWeapon(1, Stinger, 2)
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef US_Stinger_Pair = new SquadDef
    {
        name = "Stinger Team",
        shortName = "Str",
        soldiers = 4,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(4, M4, 8),
            new UnitWeapon(2, Stinger, 2)
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef US_Stinger_Team = new SquadDef
    {
        name = "Stinger Team",
        shortName = "Str Tm",
        soldiers = 2,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(2, M4, 8),
            new UnitWeapon(1, Stinger, 2)
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef Stryker_M2 = new SquadDef
    {
        name = "Stryker M2",
        unitClass = UnitClass.WheeledApc,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Wheeled_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Light_Armor,
        armor = 40,
    };

    public static SquadDef Stryker_MK19 = new SquadDef {
        name = "Stryker MK19",
        unitClass = UnitClass.WheeledApc,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Wheeled_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, MK19, 20)
        },
        targetType = TargetType.Light_Armor,
        armor = 40
    };

    /*
    public static SquadDef Bradley = new SquadDef("Bradley IFV", 3, 3, new UnitWeapon[] {
        new UnitWeapon(1, Bushmaster, 20),
        new UnitWeapon(1, AFV_M240, 20),
        new UnitWeapon(1, Bradley_TOW, 10)
    }, TargetType.Light_Armor, new Countermeasures(), new Cargo(9, 1));
    */

    public static SquadDef Bradley = new SquadDef
    {
        name = "Bradley IFV",
        unitClass = UnitClass.Ifv,
        soldiers = 3,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Bushmaster, 15),
            new UnitWeapon(1, AFV_M240, 30),
            new UnitWeapon(1, Bradley_TOW, 7)
        },
        targetType = TargetType.Light_Armor,
        armor = 115,
        cargoDef = new CargoDef(9, 1),
    };

    public static SquadDef Bradley_CFV = new SquadDef
    {
        name = "Bradley CFV",
        unitClass = UnitClass.Ifv,
        soldiers = 3,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Bushmaster, 25),
            new UnitWeapon(1, AFV_M240, 40),
            new UnitWeapon(1, Bradley_TOW, 12)
        },
        targetType = TargetType.Light_Armor,
        armor = 115,
        cargoDef = new CargoDef(4, 1),
    };

    public static SquadDef Bradley_Linebacker = new SquadDef
    {
        name = "Linebacker AA",
        unitClass = UnitClass.Ifv,
        soldiers = 3,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Bushmaster, 25),
            new UnitWeapon(1, AFV_M240, 40),
            new UnitWeapon(4, Stinger, 12)
        },
        targetType = TargetType.Light_Armor,
        armor = 115,
    };

    
    public static int m113_armor = 10;

    public static SquadDef M113 = new SquadDef
    {
        name = "M113",
        unitClass = UnitClass.Ifv,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, M2, 10)
        },
        targetType = TargetType.Light_Armor,
        armor = m113_armor,
        cargoDef = new CargoDef(10, 1)
    };

    public static SquadDef Avenger = new SquadDef
    {
        name = "Avenger AA",
        unitClass = UnitClass.Truck,
        soldiers = 2,
        moves = 3,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, M3P, 4),
            new UnitWeapon(2, Stinger, 4)
        },
        targetType = TargetType.Light_Armor,
        armor = 6,
    };


    /*public static SquadDef Bradley = new SquadDef( 
        name = "Bradley IFV",
        soldiers = 3,
        moves = 3,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Bushmaster, 20),
            new UnitWeapon(1, AFV_M240, 20),
            new UnitWeapon(1, Bradley_TOW, 10)
        },
        targetType = TargetType.Light_Armor,
        countermeasures = new Countermeasures(),
        cargoDef = new Cargo(9, 1)
    );*/


    /*public static SquadDef Abrams_M1A1 = new SquadDef
    {
        name = "Abrams MBT",
        unitClass = UnitClass.Tank,
        soldiers = 5,
        moves = 3,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, M256, 20),
            new UnitWeapon(1, AFV_M240, 20)
        },
        armor = 600,
        targetType = TargetType.Heavy_Armor
    };*/
    public static SquadDef Abrams = new SquadDef
    {
        name = "Abrams MBT", //// M1A2
        unitClass = UnitClass.Tank,
        soldiers = 5,
        moves = 3,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, M256, 20),
            new UnitWeapon(1, AFV_M240, 20)
        },
        armor = 700,
        targetType = TargetType.Heavy_Armor
    };




    // short name 60 Mtr
    public static SquadDef US_60mm_Mortar = new SquadDef("60mm Mortar", 3, 1, new UnitWeapon[] {
        new UnitWeapon(1, M224_Mortar, 10),
        new UnitWeapon(3, M4, 10),
    }, TargetType.Infantry);

    // short name 81 Mtr
    public static SquadDef US_81mm_Mortar = new SquadDef("81mm Mortar", 5, 1, new UnitWeapon[] {
        new UnitWeapon(1, M252_Mortar, 10),
        new UnitWeapon(5, M4, 10),
    }, TargetType.Infantry);

    // short name 120 Mtr
    public static SquadDef US_120mm_Mortar = new SquadDef("120mm Mortar", 7, 1, new UnitWeapon[] {
        new UnitWeapon(1, M120_Mortar, 10),
        new UnitWeapon(7, M4, 10),
    }, TargetType.Infantry);


    public static SquadDef US_105mm_How = new SquadDef("105mm Howitzer", 6, 1, new UnitWeapon[] {
        new UnitWeapon(1, M119_How, 10),
        new UnitWeapon(6, M4, 10),
    }, TargetType.Infantry);

    public static SquadDef US_155mm_How = new SquadDef("155mm Howitzer", 8, 1, new UnitWeapon[] {
        new UnitWeapon(1, M777_How, 10),
        new UnitWeapon(8, M4, 10),
    }, TargetType.Infantry);

    public static SquadDef Paladin = new SquadDef("Paladin", 4, 2, new UnitWeapon[] {
        new UnitWeapon(1, M284_How, 10),
        new UnitWeapon(1, M2, 10),
    }, TargetType.Light_Armor);
    
    public static SquadDef M121 = new SquadDef
    {
        name = "M121 120mm Mortar",
        unitClass = UnitClass.MechMortar,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[]
        {
            new UnitWeapon(1, M120_Mortar, 13)
        },
        armor = m113_armor, // BMD-1 based
        targetType = TargetType.Light_Armor
    };

    public static SquadDef Apache = new SquadDef
    {
        name = "Apache",
        unitClass = UnitClass.Helicopter,
        soldiers = 2,
        moves = 5,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, M230, 20),
            new UnitWeapon(1, Hellfire, 16)
        },
        targetType = TargetType.Helicopter,
        armor = 28,
        countermeasures = new Countermeasures(),
        minAltitude = 1,
        maxAltitude = 20,
        gainAltCost = 1,
        loseAltPayment = 0.5
    };

    public static SquadDef Apache_Dp = new SquadDef
    {
        name = "Apache",
        unitClass = UnitClass.Helicopter,
        soldiers = 2,
        moves = 5,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, M230, 20),
            new UnitWeapon(1, Hellfire, 8),
            new UnitWeapon(2, Hydra, 3)
        },
        targetType = TargetType.Helicopter,
        armor = 25,
        countermeasures = new Countermeasures(),
        minAltitude = 1,
        maxAltitude = 20
    };

    /// <summary>
    /// RU
    /// Weapons
    /// </summary>
    public static Weapon AK74 = new Weapon
    {
        id = "ak74",
        name = "AK74",
        range = Weapon.RealRange(500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(7, rifle_acc, 1, 1.0, rifle_rfac) },
            { TargetType.Helicopter, new DamageProfile(7, rifle_vs_helo_acc, 1, 0.2, rifle_vs_helo_rfac) }
        },
        penetration = 6,
        shootProfile = rifleShootProfile
    };

    public static Weapon RPK74 = new Weapon
    {
        id = "rpk74",
        name = "RPK74",
        range = Weapon.RealRange(600),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(25, lmg_acc, 1, 1.0, lmg_rfac) },
            { TargetType.Helicopter, new DamageProfile(25, lmg_vs_helo_acc, 1, 0.2, lmg_vs_helo_rfac) }
        },
        penetration = 6,
        soundProfile = rpkSoundProfile,
        shootProfile = rpkShootProfile
    };

    public static Weapon PKP = new Weapon
    {
        id = "pkp",
        name = "PKP",
        range = Weapon.RealRange(800),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(25, hlmg_acc, 1, 1.0, lmg_rfac) },
            { TargetType.Helicopter, new DamageProfile(25, hlmg_vs_helo_acc, 1, 0.25, hlmg_vs_helo_rfac) }
        },
        penetration = 10,
        shootProfile = pkShootProfile
    };

    public static Weapon PK = new Weapon
    {
        id = "pk",
        name = "PK",
        range = Weapon.RealRange(1000),
        soldiersToFire = 2,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(25, hlmg_acc, 1, 1.0, hlmg_rfac) },
            { TargetType.Helicopter, new DamageProfile(25, hlmg_vs_helo_acc, 1, 0.25, hlmg_vs_helo_rfac) }
        },
        penetration = 10,
        soundProfile = pkSoundProfile,
        shootProfile = pkShootProfile
    };

    public static Weapon PK_Tripod = new Weapon
    {
        id = "pk_tripod",
        name = "PK Tripod",
        range = Weapon.RealRange(1200),
        soldiersToFire = 2,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(30, mmg_acc, 1, 1.0, mmg_rfac) },
            { TargetType.Helicopter, new DamageProfile(30, mmg_vs_helo_acc, 1, 0.25, mmg_vs_helo_rfac) }
        },
        penetration = 10,
        shootProfile = pkTripodShootProfile
    };

    public static Weapon AFV_PK = new Weapon
    {
        id = "pk_afv",
        name = "PKT",
        range = Weapon.RealRange(1200),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(35, mmg_vec_acc, 1) },
            { TargetType.Helicopter, new DamageProfile(35, mmg_vec_acc - 0.15, 1, 0.25) },
        },
        penetration = 10,
        shootProfile = pkTripodShootProfile
    };
    public static Weapon DShK = new Weapon("dshk", "DShK .50 Cal", 10);
    public static Weapon Kord = new Weapon("kord", "Kord .50 Cal", 10);
    public static Weapon NSV = new Weapon("nsv", "NSV .50 Cal", 10);

    public static Weapon KPV = new Weapon
    {
        id = "kpv",
        name = "KPV 14.5mm",
        range = Weapon.RealRange(1700),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(30, vhmg_acc, 1, 1.0, vhmg_rfac) },
            { TargetType.Light_Armor, new DamageProfile(30, vhmg_acc, 1, 1.0, vhmg_rfac) },
            { TargetType.Helicopter, new DamageProfile(30, vhmg_vs_helo_acc, 1, 1.0, vhmg_vs_helo_rfac) },
        },
        penetration = 32,
        //soundProfile = pkSoundProfile
        shootProfile = kpvShootProfile
    };

    public static Weapon GP25 = new Weapon
    {
        id = "gp25",
        name = "GP-25",
        range = Weapon.RealRange(400),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, gl_acc, 2, 1.0, gl_rfac) },
            { TargetType.Light_Armor, new DamageProfile(3, gl_acc, 1, 1.0, gl_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(3, gl_acc, 1, 1.0, gl_rfac) },
            { TargetType.Helicopter, new DamageProfile(3, 0.05, 2, 1.0, 0.3) },
        },
        penetration = 25,
        shootProfile = gp25ShootProfile
    };

    public static Weapon RPG22 = new Weapon
    {
        id = "rpg22",
        name = "RPG-22",
        range = Weapon.RealRange(300),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, rocket_acc -0.05, 1, 1.0, rocket_rfac -0.05) },
            { TargetType.Light_Armor, new DamageProfile(1, rocket_acc -0.05, 2, 1.0, rocket_rfac -0.05) },
            { TargetType.Heavy_Armor, new DamageProfile(1, rocket_acc -0.05, 1, 1.0, rocket_rfac -0.05) },
            { TargetType.Helicopter, new DamageProfile(1, rocket_vs_helo_acc -0.05, 3, 1.0, rocket_vs_helo_rfac -0.05) },
        },
        penetration = 400,
        soundProfile = rpg7SoundProfile,
        shootProfile = rpg7ShootProfile
    };

    public static Weapon RPG26 = new Weapon
    {
        id = "rpg26",
        name = "RPG-26",
        range = Weapon.RealRange(300),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, rocket_acc -0.05, 1, 1.0, rocket_rfac -0.05) },
            { TargetType.Light_Armor, new DamageProfile(1, rocket_acc -0.05, 3, 1.0, rocket_rfac -0.05) },
            { TargetType.Heavy_Armor, new DamageProfile(1, rocket_acc -0.05, 2, 1.0, rocket_rfac -0.05) },
            { TargetType.Helicopter, new DamageProfile(1, rocket_vs_helo_acc -0.05, 3, 1.0, rocket_vs_helo_rfac -0.05) },
        },
        penetration = 500,
        soundProfile = rpg7SoundProfile,
        shootProfile = rpg7ShootProfile
    };

    public static Weapon RPG7 = new Weapon
    {
        id = "rpg7",
        name = "RPG-7",
        range = Weapon.RealRange(500),
        soldiersToFire = 2,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(2, rocket_acc, 1, 1.0, rocket_rfac) },
            { TargetType.Light_Armor, new DamageProfile(2, rocket_acc, 2, 1.0, rocket_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(2, rocket_acc, 1, 1.0, rocket_rfac) },
            { TargetType.Helicopter, new DamageProfile(2, rocket_vs_helo_acc, 3, 1.0, rocket_vs_helo_rfac) },
        },
        penetration = 400,
        soundProfile = rpg7SoundProfile,
        shootProfile = rpg7ShootProfile
    };

    public static Weapon S_5 = new Weapon
    {
        id = "s5",
        name = "S-7 Rockets",
        range = Weapon.RealRange(1500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(7, heli_rocket_acc, 2, 1.0, heli_rocket_rfac) },
            { TargetType.Light_Armor, new DamageProfile(7, heli_rocket_acc, 2, 1.0, heli_rocket_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(7, heli_rocket_acc, 1, 1.0, heli_rocket_rfac) }
        },
        penetration = 40,
        weaponType = WeaponType.Rocket,
        shootProfile = s5ShootProfile
    };

    public static Weapon AGS_17 = new Weapon
    {
        id = "ags17",
        name = "AGS-17",
        range = Weapon.RealRange(1500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(12, agl_acc - 0.05, 1, 2, 1.0, agl_rfac) },
            { TargetType.Light_Armor, new DamageProfile(12, agl_acc - 0.05, 1, 2, 1.0, agl_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(12, agl_acc - 0.05, 1, 2, 1.0, agl_rfac) }
        },
        penetration = 25,
        //soundProfile = ags17SoundProfile
        shootProfile = ags17ShootProfile
    };

    public static Weapon AFV_AGS_17 = new Weapon
    {
        id = "ags17_afv",
        name = "AGS-17",
        range = Weapon.RealRange(1500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(10, agl_acc - 0.05, 2, 1.0, agl_rfac) },
            { TargetType.Light_Armor, new DamageProfile(10, agl_acc - 0.05, 1, 1.0, agl_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(10, agl_acc - 0.05, 1, 1.0, agl_rfac) }
        },
        penetration = 25,
        //soundProfile = ags17SoundProfile
        shootProfile = ags17ShootProfile
    };

    public static Weapon AGS_30 = new Weapon
    {
        id = "ags30",
        name = "AGS-30",
        range = Weapon.RealRange(1500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(10, agl_acc - 0.05, 2, 1.0, agl_rfac - 0.05) },
            { TargetType.Light_Armor, new DamageProfile(10, agl_acc - 0.05, 1, 1.0, agl_rfac - 0.05) },
            { TargetType.Heavy_Armor, new DamageProfile(10, agl_acc - 0.05, 1, 1.0, agl_rfac - 0.05) }
        },
        penetration = 25,
        //soundProfile = ags30SoundProfile
        shootProfile = ags17ShootProfile
    };





    // BMP-1 73mm - no autoloader (BRM-1K)
    public static Weapon _2A28 = new Weapon
    {
        id = "2a28",
        name = "2A28 73mm",
        range = Weapon.RealRange(2000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(2, frag_cannon_acc - 0.05, 1, 3, 1.0, frag_cannon_rfac - 0.05) },
            { TargetType.Light_Armor, new DamageProfile(2, frag_cannon_acc - 0.05, 1, 4, 1.0, frag_cannon_rfac - 0.05) },
            { TargetType.Heavy_Armor, new DamageProfile(2, frag_cannon_acc - 0.05, 1, 4, 1.0, frag_cannon_rfac - 0.05) },
        },
        penetration = 380,
        shootProfile = _2a28ShootProfile
    };

    // BMP-2 etc 30mm Autocannon
    public static Weapon _2A42 = new Weapon
    {
        id = "2a42",
        name = "2A42 30mm",
        range = Weapon.RealRange(2500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(18, auto_acc, 1, 1.0, auto_rfac) },
            { TargetType.Light_Armor, new DamageProfile(18, auto_acc, 2, 1.0, auto_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(18, auto_acc, 1, 1.0, auto_rfac) },
            { TargetType.Helicopter, new DamageProfile(18, auto_vs_helo_acc, 2, 1.0, auto_vs_helo_rfac) },
        },
        penetration = 60,
        shootProfile = _2a42ShootProfile
    };

    // BMP-3 30mm Auto
    public static Weapon _2A72 = new Weapon
    {
        id = "2a72",
        name = "2A72 30mm",
        range = Weapon.RealRange(2500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(20, auto_acc, 1, 1.0, auto_rfac) },
            { TargetType.Light_Armor, new DamageProfile(20, auto_acc, 2, 1.0, auto_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(20, auto_acc, 1, 1.0, auto_rfac) },
            { TargetType.Helicopter, new DamageProfile(20, auto_vs_helo_acc, 2, 1.0, auto_vs_helo_rfac) },
        },
        penetration = 60,
        shootProfile = _2a72ShootProfile
    };

    // Hind fixed twin 30mmo
    public static Weapon Gsh_30_2k = new Weapon
    {
        id = "gsh-30-2k",
        name = "Gsh-30-2K",
        range = Weapon.RealRange(2000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(20, heli_auto_acc - 0.05, 1, 1.0, heli_auto_rfac - 0.1) },
            { TargetType.Light_Armor, new DamageProfile(20, heli_auto_acc - 0.05, 1, 1.0, heli_auto_rfac - 0.1) },
            { TargetType.Heavy_Armor, new DamageProfile(20, heli_auto_acc - 0.05, 1, 1.0, heli_auto_rfac - 0.1) },
            //{ TargetType.Helicopter, new DamageProfile(50, heli_auto_vs_helo_acc, 2, 1.0, heli_auto_vs_helo_rfac) },
        },
        penetration = 30,
        shootProfile = _gsh302kShootProfile
    };

    // Hind fixed twin 30mmo - fast
    public static Weapon Gsh_30_2k_Fast = new Weapon
    {
        id = "gsh-30-2k_fast",
        name = "Gsh-30-2K",
        range = Weapon.RealRange(2000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(50, DivideAndRoundAcc(heli_auto_acc - 0.05,2.0), 1, 1.0, heli_auto_rfac - 0.4) },
            { TargetType.Light_Armor, new DamageProfile(50, DivideAndRoundAcc(heli_auto_acc - 0.05,2.0), 1, 1.0, heli_auto_rfac - 0.4) },
            { TargetType.Heavy_Armor, new DamageProfile(50, DivideAndRoundAcc(heli_auto_acc - 0.05,2.0), 1, 1.0, heli_auto_rfac - 0.4) },
            //{ TargetType.Helicopter, new DamageProfile(50, heli_auto_vs_helo_acc, 2, 1.0, heli_auto_vs_helo_rfac) },
        },
        penetration = 30,
        shootProfile = _gsh302kFastShootProfile
    };

    // Hind fixed twin 30mmo
    public static Weapon Gsh_30_1 = new Weapon
    {
        id = "gsh-30-1",
        name = "Gsh-30-1",
        range = Weapon.RealRange(2000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(45, air_auto_acc, 1, 1.0, air_auto_rfac) },
            { TargetType.Light_Armor, new DamageProfile(45, air_auto_acc, 1, 1.0, air_auto_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(45, air_auto_acc, 1, 1.0, air_auto_rfac) },
            { TargetType.Helicopter, new DamageProfile(45, air_auto_acc, 2, 1.0, air_auto_rfac) },
        },
        penetration = 30,
        shootProfile = _gsh301ShootProfile
    };

    // AA 23mm
    public static Weapon _2A7 = new Weapon
    {
        id = "2a7",
        name = "2A7 23mm",
        range = Weapon.RealRange(2200),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(50, aa_radar_auto_acc, 1, 1.0, aa_radar_auto_rfac) },
            { TargetType.Light_Armor, new DamageProfile(50, aa_radar_auto_acc, 1, 1.0, aa_radar_auto_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(50, aa_radar_auto_acc, 1, 1.0, aa_radar_auto_rfac) },
            { TargetType.Helicopter, new DamageProfile(50, aa_radar_auto_vs_helo_acc, 2, 1.0, aa_radar_auto_vs_helo_rfac) },
        },
        penetration = 30,
        shootProfile = _2a7ShootProfile
    };

    // AA 30mm
    public static Weapon _2A38 = new Weapon
    {
        id = "2a38",
        name = "2A38 30mm",
        range = Weapon.RealRange(2200),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(50, aa_radar_auto_acc, 1, 1.0, aa_radar_auto_rfac) },
            { TargetType.Light_Armor, new DamageProfile(50, aa_radar_auto_acc, 1, 1.0, aa_radar_auto_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(50, aa_radar_auto_acc, 1, 1.0, aa_radar_auto_rfac) },
            { TargetType.Helicopter, new DamageProfile(50, aa_radar_auto_vs_helo_acc, 2, 1.0, aa_radar_auto_vs_helo_rfac) },
        },
        penetration = 30,
        shootProfile = _2a38ShootProfile
    };

    // AA 30mm
    public static Weapon _2A38M = new Weapon
    {
        id = "2a38m",
        name = "2A38M 30mm",
        range = Weapon.RealRange(2200),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(50, aa_radar_auto_acc, 1, 1.0, aa_radar_auto_rfac) },
            { TargetType.Light_Armor, new DamageProfile(50, aa_radar_auto_acc, 1, 1.0, aa_radar_auto_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(50, aa_radar_auto_acc, 1, 1.0, aa_radar_auto_rfac) },
            { TargetType.Helicopter, new DamageProfile(50, aa_radar_auto_vs_helo_acc, 2, 1.0, aa_radar_auto_vs_helo_rfac) },
        },
        penetration = 30,
        shootProfile = _2a38ShootProfile
    };

    // BMP-3 100m frag cannon
    public static Weapon _2A70 = new Weapon
    {
        id = "2a70",
        name = "2A70 100mm",
        range = Weapon.RealRange(3000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, frag_cannon_acc, 2, 4, 1.0, frag_cannon_rfac) },
            { TargetType.Light_Armor, new DamageProfile(3, frag_cannon_acc, 1, 3, 1.0, frag_cannon_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(3, frag_cannon_acc, 1, 3, 1.0, frag_cannon_rfac) }
        },
        penetration = 75,
        weaponType = WeaponType.Wire,
        shootProfile = _2a46mShootProfile
    };

    // Tank 125mm
    public static Weapon _2A46M = new Weapon
    {
        id = "2a46",
        name = "2A46M 125mm",
        range = Weapon.RealRange(4000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, cannon_acc, 1, 5, 1.0, cannon_rfac) },
            { TargetType.Light_Armor, new DamageProfile(3, cannon_acc, 1, 5, 1.0, cannon_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(3, cannon_acc, 1, 5, 1.0, cannon_rfac) },
            { TargetType.Helicopter, new DamageProfile(3, cannon_vs_helo_acc, 1, 5, cannon_vs_helo_rfac) },
        },
        penetration = 850,
        shootProfile = _2a46mShootProfile
    };

    // Sprut-SD 125mm Gun
    public static Weapon _2A75 = new Weapon
    {
        id = "2a75",
        name = "2A75 125mm",
        range = Weapon.RealRange(4000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, cannon_acc, 1, 5, 1.0, cannon_rfac) },
            { TargetType.Light_Armor, new DamageProfile(3, cannon_acc, 1, 5, 1.0, cannon_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(3, cannon_acc, 1, 5, 1.0, cannon_rfac) },
            { TargetType.Helicopter, new DamageProfile(3, cannon_vs_helo_acc, 1, 5, cannon_vs_helo_rfac) },
        },
        penetration = 850,
        shootProfile = _2a46mShootProfile
    };

    // T-12A 100mm Gun
    public static Weapon _2A29 = new Weapon
    {
        id = "2a29",
        name = "2A29 100mm",
        range = Weapon.RealRange(3000),
        setupTime = 1,
        packupTime = 1,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, cannon_acc, 1, 4, 1.0, cannon_rfac) },
            { TargetType.Light_Armor, new DamageProfile(3, cannon_acc, 1, 4, 1.0, cannon_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(3, cannon_acc, 1, 4, 1.0, cannon_rfac) },
            { TargetType.Helicopter, new DamageProfile(3, cannon_vs_helo_acc, 1, 4, cannon_vs_helo_rfac) },
        },
        penetration = 225,
        shootProfile = _2a46mShootProfile
    };



    // Armata 125mm Cannon
    public static Weapon _2A82_1M = new Weapon
    {
        id = "2a82-1m",
        name = "2A82-1M 125mm",
        range = Weapon.RealRange(4000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, cannon_acc, 1, 5, 1.0, cannon_rfac) },
            { TargetType.Light_Armor, new DamageProfile(3, cannon_acc, 1, 5, 1.0, cannon_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(3, cannon_acc, 1, 5, 1.0, cannon_rfac) },
            { TargetType.Helicopter, new DamageProfile(3, cannon_vs_helo_acc, 1, 5, cannon_vs_helo_rfac) },
        },
        penetration = 850,
        shootProfile = _2a46mShootProfile
    };

    // Armata 152mm Cannon
    public static Weapon _2A83 = new Weapon
    {
        id = "2a83",
        name = "2A83 152mm",
        range = Weapon.RealRange(4000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, cannon_acc, 1, 7, 1.0, cannon_rfac) },
            { TargetType.Light_Armor, new DamageProfile(3, cannon_acc, 1, 7, 1.0, cannon_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(3, cannon_acc, 1, 7, 1.0, cannon_rfac) },
            { TargetType.Helicopter, new DamageProfile(3, cannon_vs_helo_acc, 1, 7, cannon_vs_helo_rfac) },
        },
        penetration = 850,
        shootProfile = _2a46mShootProfile
    };



    public static Weapon Metis = new Weapon
    {
        id = "metis",
        name = "Metis ATGM",
        range = Weapon.RealRange(1000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 1, 3, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 1, 5, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 1, 5, 1.0, missile_rfac) }
        },
        penetration = 460,
        weaponType = WeaponType.Wire,
        shootProfile = metisShootProfile,
    };

    public static Weapon MetisM = new Weapon
    {
        id = "metism",
        name = "Metis-M ATGM",
        range = Weapon.RealRange(1500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 1, 3, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 1, 5, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 1, 5, 1.0, missile_rfac) }
        },
        penetration = 800,
        weaponType = WeaponType.Wire,
        shootProfile = metisShootProfile,
    };

    public static Weapon MetisM1 = new Weapon
    {
        id = "metism1",
        name = "Metis-M1 ATGM",
        range = Weapon.RealRange(2000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 1, 3, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 1, 5, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 1, 5, 1.0, missile_rfac) }
        },
        penetration = 925,
        weaponType = WeaponType.Wire,
        shootProfile = metisShootProfile,
    };

    public static Weapon AFV_Malyutka2 = new Weapon
    {
        id = "malyutka2_afv",
        name = "Malyutka-2 ATGM",
        range = Weapon.RealRange(3000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 1, 3, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc - 0.05, 1, 5, 1.0, missile_rfac - 0.05) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc - 0.05, 1, 5, 1.0, missile_rfac - 0.05) }
        },
        penetration = 800,
        weaponType = WeaponType.Wire,
        shootProfile = konkursShootProfile,
    };
    public static Weapon AFV_Malyutka2M = new Weapon
    {
        id = "malyutka2m_afv",
        name = "Malyutka-2M ATGM",
        range = Weapon.RealRange(3000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 1, 3, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 1, 5, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 1, 5, 1.0, missile_rfac) }
        },
        penetration = 200+700,
        weaponType = WeaponType.Wire,
        shootProfile = konkursShootProfile,
    };

    public static Weapon Konkurs = new Weapon
    {
        id = "konkurs",
        name = "Konkurs-M ATGM",
        range = Weapon.RealRange(4000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 1, 3, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 775,
        weaponType = WeaponType.Wire,
        shootProfile = konkursShootProfile,
    };

    public static Weapon KonkursM = new Weapon
    {
        id = "konkursm",
        name = "Konkurs-M ATGM",
        range = Weapon.RealRange(4000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 1, 3, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 200 + 775,
        weaponType = WeaponType.Wire,
        shootProfile = konkursShootProfile,
    };

    public static Weapon AFV_KonkursM = new Weapon
    {
        id = "konkursm_afv",
        name = "Konkurs-M ATGM",
        range = Weapon.RealRange(4000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 5, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 5, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 5, 1.0, missile_rfac) }
        },
        penetration = 200 + 775,
        weaponType = WeaponType.Laser,
        shootProfile = konkursShootProfile
    };

    public static Weapon KonkursM1 = new Weapon
    {
        id = "konkursm1",
        name = "Konkurs-M1 ATGM",
        range = Weapon.RealRange(4000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 1, 3, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 200 + 775,
        weaponType = WeaponType.Wire,
        shootProfile = konkursShootProfile,
    };

    public static Weapon AFV_KonkursM1 = new Weapon
    {
        id = "konkursm1_afv",
        name = "Konkurs-M1 ATGM",
        range = Weapon.RealRange(4000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 5, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 5, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 5, 1.0, missile_rfac) }
        },
        penetration = 200 + 775,
        weaponType = WeaponType.Laser,
        shootProfile = konkursShootProfile
    };

    public static Weapon KonkursM2 = new Weapon
    {
        id = "konkursm2",
        name = "Konkurs-M2 ATGM",
        range = Weapon.RealRange(4000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 1, 3, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 200 + 775,
        weaponType = WeaponType.Wire,
        shootProfile = konkursShootProfile,
    };

    public static Weapon AFV_KonkursM2 = new Weapon
    {
        id = "konkursm2_afv",
        name = "Konkurs-M2 ATGM",
        range = Weapon.RealRange(4000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 5, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 5, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 5, 1.0, missile_rfac) }
        },
        penetration = 200 + 775,
        weaponType = WeaponType.Laser,
        shootProfile = konkursShootProfile
    };

    public static Weapon Kornet = new Weapon
    {
        id = "kornet",
        name = "Kornet ATGM",
        range = Weapon.RealRange(5500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 1, 3, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 200 + 1100,
        weaponType = WeaponType.Laser,
        shootProfile = konkursShootProfile,
    };

    public static Weapon AFV_Kornet = new Weapon
    {
        id = "kornet_afv",
        name = "Kornet ATGM",
        range = Weapon.RealRange(5500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 1, 3, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 200 + 1100,
        weaponType = WeaponType.Laser,
        shootProfile = konkursShootProfile,
    };

    public static Weapon KornetM = new Weapon
    {
        id = "kornem",
        name = "Kornet-M ATGM",
        range = Weapon.RealRange(8000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 1, 3, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 200 + 1200,
        weaponType = WeaponType.Laser,
        shootProfile = konkursShootProfile,
    };

    public static Weapon AFV_KornetM = new Weapon
    {
        id = "kornetm_afv",
        name = "Kornet-M ATGM",
        range = Weapon.RealRange(8000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 1, 3, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 200 + 1200,
        weaponType = WeaponType.Laser,
        shootProfile = konkursShootProfile,
    };

    public static Weapon AFV_Khrizantema = new Weapon
    {
        id = "khrizantema_afv",
        name = "Khrizantema ATGM",
        range = Weapon.RealRange(6000),
        minRange = 2, // 333m (400)
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 1, 3, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 200 + 1175,
        weaponType = WeaponType.Laser,
        shootProfile = khrizantemaShootProfile,
    };
    public static Weapon AFV_KhrizantemaM = new Weapon
    {
        id = "khrizantemam_afv",
        name = "Khrizantema-M ATGM",
        range = Weapon.RealRange(6000),
        minRange = 2, // 333m (400)
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 1, 3, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 200 + 1175,
        weaponType = WeaponType.Laser,
        shootProfile = khrizantemaShootProfile,
    };

    public static Weapon Bastion = new Weapon
    {
        id = "bastion",
        name = "Bastion ATGM",
        range = Weapon.RealRange(4000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 6, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 200 + 550,
        weaponType = WeaponType.Laser,
        shootProfile = bastionShootProfile
    };
    public static Weapon Kan = new Weapon
    {
        id = "kan",
        name = "Kan ATGM",
        range = Weapon.RealRange(4000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 6, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 200 + 600,
        weaponType = WeaponType.Laser,
        shootProfile = bastionShootProfile
    };
    public static Weapon Arkan = new Weapon
    {
        id = "arkan",
        name = "Arkan ATGM",
        range = Weapon.RealRange(6000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 6, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 200 + 750,
        weaponType = WeaponType.Laser,
        shootProfile = bastionShootProfile
    };
    public static Weapon Arkan_BMP3 = new Weapon
    {
        id = "arkan_bmp3",
        name = "Arkan ATGM",
        range = Weapon.RealRange(5500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 6, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 200 + 750,
        weaponType = WeaponType.Laser,
        shootProfile = bastionShootProfile
    };

    public static Weapon Kobra = new Weapon
    {
        id = "kobra",
        name = "Kobra ATGM",
        range = Weapon.RealRange(4000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 5, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 5, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 5, 1.0, missile_rfac) }
        },
        penetration = 800,
        weaponType = WeaponType.Laser,
        shootProfile = kobraShootProfile
    };

    // svir T-72, refleks T-64/T80/T-90
    public static Weapon Svir = new Weapon
    {
        id = "svir",
        name = "Svir ATGM",
        range = Weapon.RealRange(4000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 6, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 200 + 800,
        weaponType = WeaponType.Laser,
        shootProfile = refleksShootProfile
    };

    public static Weapon Refleks = new Weapon
    {
        id = "refleks",
        name = "Refleks ATGM",
        range = Weapon.RealRange(4000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 6, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 200+800,
        weaponType = WeaponType.Laser,
        shootProfile = refleksShootProfile
    };

    public static Weapon Invar = new Weapon
    {
        id = "invar",
        name = "Invar ATGM",
        range = Weapon.RealRange(5000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 6, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 200 + 800,
        weaponType = WeaponType.Laser,
        shootProfile = refleksShootProfile
    };

    public static Weapon InvarM = new Weapon
    {
        id = "invarm",
        name = "Invar-M ATGM",
        range = Weapon.RealRange(5000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 6, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 200 + 900,
        weaponType = WeaponType.Laser,
        shootProfile = refleksShootProfile
    };

    public static Weapon Shturm = new Weapon
    {
        id = "shturm",
        name = "Shturm ATGM",
        range = Weapon.RealRange(5000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 6, 1.0, heli_missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, heli_missile_acc, 2, 6, 1.0, heli_missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, heli_missile_acc, 2, 6, 1.0, heli_missile_rfac) }
        },
        penetration = 560,
        weaponType = WeaponType.Radio,
        shootProfile = shturmShootProfile
    };
    public static Weapon AFV_Shturm = new Weapon
    {
        id = "shturm_afv",
        name = "Shturm ATGM",
        range = Weapon.RealRange(5000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 6, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 560,
        weaponType = WeaponType.Radio,
        shootProfile = shturmShootProfile
    };
    public static Weapon ShturmM1 = new Weapon
    {
        id = "shturm-m1",
        name = "Shturm-M1 ATGM",
        range = Weapon.RealRange(6000),
        minRange = 2, // 333m (400)
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 6, 1.0, heli_missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, heli_missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, heli_missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 625,
        weaponType = WeaponType.Radio,
        shootProfile = shturmShootProfile
    };
    public static Weapon AFV_ShturmM1 = new Weapon
    {
        id = "shturm-m1_afv",
        name = "Shturm-M1 ATGM",
        range = Weapon.RealRange(6000),
        minRange = 2, // 333m (400)
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 6, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 625,
        weaponType = WeaponType.Radio,
        shootProfile = shturmShootProfile
    };
    public static Weapon ShturmM2 = new Weapon
    {
        id = "shturm-m2",
        name = "Shturm-M2 ATGM",
        range = Weapon.RealRange(7000),
        minRange = 2, // 333m (400)
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 6, 1.0, heli_missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, heli_missile_acc, 2, 6, 1.0, heli_missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, heli_missile_acc, 2, 6, 1.0, heli_missile_rfac) }
        },
        penetration = 625,
        weaponType = WeaponType.Radio,
        shootProfile = shturmShootProfile
    };
    public static Weapon AFV_ShturmM2 = new Weapon
    {
        id = "shturm-m12_afv",
        name = "Shturm-M2 ATGM",
        range = Weapon.RealRange(7000),
        minRange = 2, // 333m (400)
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 6, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 6, 1.0, missile_rfac) }
        },
        penetration = 625,
        weaponType = WeaponType.Radio,
        shootProfile = shturmShootProfile
    };

    public static Weapon Ataka = new Weapon
    {
        id = "ataka",
        name = "Ataka ATGM",
        range = Weapon.RealRange(6000),
        minRange = 2, // 333m (400)
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 7, 1.0, heli_missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, heli_missile_acc, 2, 7, 1.0, heli_missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, heli_missile_acc, 2, 7, 1.0, heli_missile_rfac) }
        },
        penetration = 200+800,
        weaponType = WeaponType.Laser,
        shootProfile = atakaShootProfile
    };
    public static Weapon AFV_Ataka = new Weapon
    {
        id = "ataka_afv",
        name = "Ataka ATGM",
        range = Weapon.RealRange(6000),
        minRange = 2, // 333m (400)
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 7, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 7, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 7, 1.0, missile_rfac) }
        },
        penetration = 200 + 800,
        weaponType = WeaponType.Laser,
        shootProfile = atakaShootProfile
    };
    public static Weapon AtakaM = new Weapon
    {
        id = "atakam",
        name = "Ataka-M ATGM",
        range = Weapon.RealRange(8000),
        minRange = 2, // 333m (400)
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 7, 1.0, heli_missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, heli_missile_acc, 2, 7, 1.0, heli_missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, heli_missile_acc, 2, 7, 1.0, heli_missile_rfac) }
        },
        penetration = 200 + 950,
        weaponType = WeaponType.Laser,
        shootProfile = atakaShootProfile
    };
    public static Weapon AFV_AtakaM = new Weapon
    {
        id = "atakam_afv",
        name = "Ataka-M ATGM",
        range = Weapon.RealRange(8000),
        minRange = 2, // 333m (400)
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 7, 1.0, missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, missile_acc, 2, 7, 1.0, missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, missile_acc, 2, 7, 1.0, missile_rfac) }
        },
        penetration = 200 + 950,
        weaponType = WeaponType.Laser,
        shootProfile = atakaShootProfile
    };
    public static Weapon AtakaD = new Weapon
    {
        id = "atakad",
        name = "Ataka-D ATGM",
        range = Weapon.RealRange(10000),
        minRange = 2, // 333m (400)
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, 0.1, 2, 6, 1.0, heli_missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, heli_missile_acc, 2, 6, 1.0, heli_missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, heli_missile_acc, 2, 6, 1.0, heli_missile_rfac) }
        },
        penetration = 200 + 950,
        weaponType = WeaponType.Laser,
        shootProfile = atakaShootProfile
    };
    
    /////////// Fixed wing air to ground
    public static Weapon Kh_25ML = new Weapon
    {
        id = "kh25ml",
        name = "Kh-25ML A2G",
        range = Weapon.RealRange(11000),
        minRange = 2, // 333m (400)
        maxLaunchAlt = Weapon.RealAltitude(5000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, air_ground_missile_acc, 4, 9, 1.0, air_ground_missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, air_ground_missile_acc, 4, 9, 1.0, air_ground_missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, air_ground_missile_acc, 4, 9, 1.0, air_ground_missile_rfac) }
        },
        penetration = 1800,
        weaponType = WeaponType.Laser,
        shootProfile = kh38ShootProfile
    };
    public static Weapon Kh_38 = new Weapon
    {
        id = "kh38",
        name = "Kh-38 A2G",
        range = Weapon.RealRange(7000),
        minRange = 2, // 333m (400)
        maxLaunchAlt = Weapon.RealAltitude(12000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, air_ground_missile_acc, 3, 8, 1.0, air_ground_missile_rfac) },
            { TargetType.Light_Armor, new DamageProfile(1, air_ground_missile_acc, 3, 8, 1.0, air_ground_missile_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(1, air_ground_missile_acc, 3, 8, 1.0, air_ground_missile_rfac) }
        },
        penetration = 1300,
        weaponType = WeaponType.Laser,
        shootProfile = kh38ShootProfile
    };
    
    public static Weapon R27T = new Weapon
    {
        id = "r27t",
        name = "R-27T A2A",
        range = Weapon.RealRange(19000), // 2-33km h2h, 0-5.5km h2t - avg 19,000
        //minRange = 5, // 833m (00)
        ceiling = Weapon.MissileCeiling(25000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, air_air_missile_vs_helo_acc, 2, 6, 1.0, air_air_missile_vs_helo_rfac) },
            { TargetType.FixedWing, new DamageProfile(1, air_air_missile_acc, 2, 6, 1.0, air_air_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Infra,
        shootProfile = r73ShootProfile
    };
    public static Weapon R27ET = new Weapon
    {
        id = "r27et",
        name = "R-27ET A2A",
        range = Weapon.RealRange(32500), // 2-52.5km h2h, 0.7-12.5km h2t - avg 32,500
        //minRange = 5, // 833m (00)
        ceiling = Weapon.MissileCeiling(27000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, air_air_missile_vs_helo_acc, 2, 6, 1.0, air_air_missile_vs_helo_rfac) },
            { TargetType.FixedWing, new DamageProfile(1, air_air_missile_acc, 2, 6, 1.0, air_air_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Infra,
        shootProfile = r73ShootProfile
    };
    public static Weapon R73 = new Weapon
    {
        id = "r73",
        name = "R-73 A2A",
        range = Weapon.RealRange(15000), // ????
        //minRange = 5, // 833m (00)
        //ceiling = Weapon.MissileCeiling(27000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, air_air_missile_vs_helo_acc, 2, 6, 1.0, air_air_missile_vs_helo_rfac) },
            { TargetType.FixedWing, new DamageProfile(1, air_air_missile_acc, 2, 6, 1.0, air_air_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Infra,
        shootProfile = r73ShootProfile
    };
    public static Weapon R77 = new Weapon
    {
        id = "r77",
        name = "R-77 A2A",
        range = Weapon.RealRange(50000), // no idea
        //minRange = 5, // 833m (00)
        ceiling = Weapon.MissileCeiling(25000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, air_air_missile_vs_helo_acc, 2, 6, 1.0, air_air_missile_vs_helo_rfac) },
            { TargetType.FixedWing, new DamageProfile(1, air_air_missile_acc, 2, 6, 1.0, air_air_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.AirRadar,
        shootProfile = r73ShootProfile
    };


    public static Weapon Strela_1M = new Weapon
    {
        id = "strela1m",
        name = "Strela-3 AA",
        range = Weapon.RealRange(4200),
        minRange = 5, // 833m (00)
        ceiling = Weapon.MissileCeiling(3500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 1, 6, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = strelaShootProfile
    };
    public static Weapon Strela_3 = new Weapon
    {
        id = "strela3",
        name = "Strela-3 AA",
        range = Weapon.RealRange(4100),
        minRange = 5, // 833m (00)
        ceiling = Weapon.MissileCeiling(3500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 1, 6, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = strelaShootProfile
    };
    // Strela-10
    public static Weapon _9M37 = new Weapon
    {
        id = "9m37",
        name = "9M37 AA",
        range = Weapon.RealRange(5000),
        minRange = 5, // 833m (00)
        ceiling = Weapon.MissileCeiling(3500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 1, 6, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = strelaShootProfile
    };
   /* // Strela-10M3
    public static Weapon Strela_10M3 = new Weapon
    {
        name = "Strela-10M3 AA",
        range = Weapon.RealRange(5000),
        minRange = 5, // 833m (00)
        ceiling = Weapon.MissileCeiling(3500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 1, 6, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = strelaShootProfile
    };*/
    // Strela-10M3K
    public static Weapon _9M37M = new Weapon
    {
        id = "9m37m",
        name = "9M37M AA",
        range = Weapon.RealRange(5000),
        minRange = 5, // 833m (00)
        ceiling = Weapon.MissileCeiling(3500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 1, 6, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = strelaShootProfile
    };
    // Strela-10M4
    public static Weapon _9M333 = new Weapon
    {
        id = "9m333",
        name = "9M333 AA",
        range = Weapon.RealRange(5000),
        minRange = 5, // 833m (00)
        ceiling = Weapon.MissileCeiling(3500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 1, 6, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = strelaShootProfile
    };
    public static Weapon Igla1 = new Weapon
    {
        id = "igla1",
        name = "Igla-1 AA",
        range = Weapon.RealRange(4100),
        minRange = Weapon.RealRange(800),
        ceiling = Weapon.MissileCeiling(3500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 1, 6, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = iglaShootProfile
    };
    public static Weapon Igla = new Weapon
    {
        id = "igla",
        name = "Igla AA",
        range = Weapon.RealRange(5200),
        minRange = Weapon.RealRange(800),
        ceiling = Weapon.MissileCeiling(3500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 1, 6, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = iglaShootProfile
    };
    public static Weapon IglaS = new Weapon
    {
        id = "iglas",
        name = "Igla-S AA",
        range = Weapon.RealRange(5000),
        minRange = Weapon.RealRange(800),
        ceiling = Weapon.MissileCeiling(3500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 1, 6, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = iglaShootProfile
    };

    public static Weapon Strelet_Igla1 = new Weapon
    {
        id = "strelet_igla1",
        name = "Igla-1 AA",
        range = Weapon.RealRange(4100),
        minRange = Weapon.RealRange(800),
        ceiling = Weapon.MissileCeiling(3500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 1, 6, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = iglaShootProfile
    };
    public static Weapon Strelet_IglaS = new Weapon
    {
        id = "strelet_iglas",
        name = "Igla-S AA",
        range = Weapon.RealRange(5000),
        minRange = Weapon.RealRange(800),
        ceiling = Weapon.MissileCeiling(3500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 1, 6, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = iglaShootProfile
    };

    ////  Tunguska
    public static Weapon _9M311 = new Weapon
    {
        id = "9m311",
        name = "9M311 AA",
        range = Weapon.RealRange(8000),
        //minRange = Weapon.RealRange(800),
        ceiling = Weapon.MissileCeiling(3500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 1, 6, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = iglaShootProfile
    };
    ////  Tunguska
    public static Weapon _9M311_M = new Weapon
    {
        id = "9m311m",
        name = "9M311-M AA",
        range = Weapon.RealRange(8000),
        //minRange = Weapon.RealRange(800),
        ceiling = Weapon.MissileCeiling(3500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 1, 6, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = iglaShootProfile
    };
    ////  Tunguska
    public static Weapon _9M311_M1 = new Weapon
    {
        id = "9m311m1",
        name = "9M311-M1 AA",
        range = Weapon.RealRange(10000),
        //minRange = Weapon.RealRange(800),
        ceiling = Weapon.MissileCeiling(3500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 1, 6, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = iglaShootProfile
    };

    //// Osa
    public static Weapon _9M33_M3 = new Weapon
    {
        id = "9m33m3",
        name = "9M33-M3 AA",
        range = Weapon.RealRange(15000),
        minRange = Weapon.RealRange(2000),
        ceiling = Weapon.MissileCeiling(12000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 2, 6, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = bukShootProfile,
        setupTime = 4,
        packupTime = 2,
    };

    ////  Tor
    public static Weapon _9M330 = new Weapon
    {
        id = "9m330",
        name = "9M330 AA",
        range = Weapon.RealRange(12000),
        minRange = Weapon.RealRange(2000),
        ceiling = Weapon.MissileCeiling(6000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 2, 6, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = bukShootProfile
    };
    ////  Tor-M1
    public static Weapon _9M331 = new Weapon
    {
        id = "9m331",
        name = "9M331 AA",
        range = Weapon.RealRange(15000),
        minRange = Weapon.RealRange(1500),
        ceiling = Weapon.MissileCeiling(6000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 2, 6, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = bukShootProfile
    };
    ////  Tor-M2
    public static Weapon _9M332 = new Weapon
    {
        id = "9m332",
        name = "9M331 AA",
        range = Weapon.RealRange(15000),
        minRange = Weapon.RealRange(1500),
        ceiling = Weapon.MissileCeiling(6000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 2, 6, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = bukShootProfile
    };

    ////  Buk
    public static Weapon BukM3_Missile = new Weapon
    {
        id = "buk3",
        name = "Buk-M3 AA",
        range = Weapon.RealRange(45000),
        minRange = Weapon.RealRange(2500),
        ceiling = Weapon.MissileCeiling(25000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Helicopter, new DamageProfile(1, aa_missile_acc, 2, 7, 1.0, aa_missile_rfac) }
        },
        penetration = 200,
        weaponType = WeaponType.Radio,
        shootProfile = bukShootProfile
    };

    public static Weapon _2B14_Mortar = new Weapon
    {
        id = "2b14",
        name = "2B14 82mm Mtr",
        //range = 18, // 3,000m
        displayRange = Weapon.RealRange(4270), // (26)
        //damageProfiles = new Dictionary<TargetType, DamageProfile> {
        //    { TargetType.Infantry, new DamageProfile(3, frag_cannon_acc, 1, 4, 1.0, frag_cannon_rfac) },
        //    { TargetType.Light_Armor, new DamageProfile(3, frag_cannon_acc, 1, 4, 1.0, frag_cannon_rfac) },
        //    { TargetType.Heavy_Armor, new DamageProfile(3, frag_cannon_acc, 1, 4, 1.0, frag_cannon_rfac) }
        //},
        //penetration = 600,
        //weaponType = WeaponType.Shell,
        //shootProfile = _2a51DirectShootProfile,
        indirect = true,
        indirectWeapon = new Weapon
        {
            name = "2B14 82mm Mtr Ind",
            range = Weapon.RealRange(4270), // (26)
            impactDelay = 1,
            aimTime = 1,
            flightTime = 1,
            setupTime = 2,
            packupTime = 2,
            artyShellsPerSalvo = 4,
            artyDelayPerShell = 1.5,
            damageProfiles = new Dictionary<TargetType, DamageProfile> {
                { TargetType.Infantry, new DamageProfile(1, mortar_acc, 1, 3, 1.0, mortar_rfac) },
                { TargetType.Light_Armor, new DamageProfile(1, mortar_acc, 1, 3, 1.0, mortar_rfac) },
                { TargetType.Heavy_Armor, new DamageProfile(1, mortar_acc, 1, 3, 1.0, mortar_rfac) }
            },
            penetration = 150,
            weaponType = WeaponType.Mortar,
            shootProfile = _82mmMortarShootProfile,
            impactProfile = _82mmMortarImpactProfile
        }

    };
    public static Weapon _2B11_Mortar = new Weapon
    {
        id = "2b11",
        name = "2B11 120mm Mtr",
        //range = 18, // 3,000m
        displayRange = Weapon.RealRange(7180), // (43)
        //damageProfiles = new Dictionary<TargetType, DamageProfile> {
        //    { TargetType.Infantry, new DamageProfile(3, frag_cannon_acc, 1, 4, 1.0, frag_cannon_rfac) },
        //    { TargetType.Light_Armor, new DamageProfile(3, frag_cannon_acc, 1, 4, 1.0, frag_cannon_rfac) },
        //    { TargetType.Heavy_Armor, new DamageProfile(3, frag_cannon_acc, 1, 4, 1.0, frag_cannon_rfac) }
        //},
        //penetration = 600,
        //weaponType = WeaponType.Shell,
        //shootProfile = _2a51DirectShootProfile,
        indirect = true,
        indirectWeapon = new Weapon
        {
            name = "2B11 120mm Mtr Ind",
            range = Weapon.RealRange(7180), // (43)
            impactDelay = 1,
            aimTime = 1,
            flightTime = 1,
            setupTime = 3,
            packupTime = 3,
            artyShellsPerSalvo = 3,
            artyDelayPerShell = 2.0,
            damageProfiles = new Dictionary<TargetType, DamageProfile> {
                { TargetType.Infantry, new DamageProfile(1, mortar_acc, 1, 4, 1.0, mortar_rfac) },
                { TargetType.Light_Armor, new DamageProfile(1, mortar_acc, 1, 4, 1.0, mortar_rfac) },
                { TargetType.Heavy_Armor, new DamageProfile(1, mortar_acc, 1, 4, 1.0, mortar_rfac) }
            },
            penetration = 250,
            weaponType = WeaponType.Mortar,
            shootProfile = _120mmMortarShootProfile,
            impactProfile = _120mmMortarImpactProfile
        }

    };

    // 2a51 and 2a60 are the same
    public static Weapon _2A51_Mortar = new Weapon
    {
        id = "2a51",
        name = "2A51 120mm Mtr",
        range = 18, // 3,000m
        displayRange = Weapon.RealRange(8850), // (53)
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, frag_cannon_acc, 1, 4, 1.0, frag_cannon_rfac) },
            { TargetType.Light_Armor, new DamageProfile(3, frag_cannon_acc, 1, 4, 1.0, frag_cannon_rfac) },
            { TargetType.Heavy_Armor, new DamageProfile(3, frag_cannon_acc, 1, 4, 1.0, frag_cannon_rfac) }
        },
        penetration = 600,
        weaponType = WeaponType.Shell,
        shootProfile = _2a51DirectShootProfile,
        indirect = true,
        indirectWeapon = new Weapon
        {
            name = "2A51 120mm Mtr Ind",
            range = Weapon.RealRange(8850), // (53)
            impactDelay = 1,
            aimTime = 1,
            flightTime = 1,
            setupTime = 1,
            packupTime = 1,
            artyShellsPerSalvo = 3,
            artyDelayPerShell = 2.0,
            damageProfiles = new Dictionary<TargetType, DamageProfile> {
                { TargetType.Infantry, new DamageProfile(1, mortar_acc, 1, 4, 1.0, mortar_rfac) },
                { TargetType.Light_Armor, new DamageProfile(1, mortar_acc, 1, 4, 1.0, mortar_rfac) },
                { TargetType.Heavy_Armor, new DamageProfile(1, mortar_acc, 1, 4, 1.0, mortar_rfac) }
            },
            penetration = 250,
            weaponType = WeaponType.Mortar,
            shootProfile = _120mmMortarShootProfile,
            impactProfile = _120mmMortarImpactProfile
        }

    };

    // 2a51 and 2a60 are the same
    public static Weapon _2A60_Mortar = new Weapon
    {
        id = "2a60",
        name = "2A60 120mm Mtr",
        range = _2A51_Mortar.range,
        damageProfiles = _2A51_Mortar.damageProfiles,
        penetration = _2A51_Mortar.penetration,
        weaponType = _2A51_Mortar.weaponType,
        shootProfile = _2A51_Mortar.shootProfile,
        indirect = _2A51_Mortar.indirect,
        indirectWeapon = new Weapon
        {
            name = "2A60 120mm Mtr Ind",
            range = _2A51_Mortar.indirectWeapon.range,
            impactDelay = _2A51_Mortar.indirectWeapon.impactDelay,
            aimTime = _2A51_Mortar.indirectWeapon.aimTime,
            flightTime = _2A51_Mortar.indirectWeapon.flightTime,
            setupTime = _2A51_Mortar.indirectWeapon.setupTime,
            packupTime = _2A51_Mortar.indirectWeapon.packupTime,
            artyShellsPerSalvo = _2A51_Mortar.indirectWeapon.artyShellsPerSalvo,
            artyDelayPerShell = _2A51_Mortar.indirectWeapon.artyDelayPerShell,
            damageProfiles = _2A51_Mortar.indirectWeapon.damageProfiles,
            penetration = _2A51_Mortar.indirectWeapon.penetration,
            weaponType = _2A51_Mortar.indirectWeapon.weaponType,
            shootProfile = _2A51_Mortar.indirectWeapon.shootProfile,
            impactProfile = _2A51_Mortar.indirectWeapon.impactProfile
        }
        
    };

    public static Weapon D30_How = new Weapon("d30", "D30 122mm How", 0, 15400);
    public static Weapon D20_How = new Weapon("d20", "D20 152mm How", 0, 17400);
    public static Weapon D22_How = new Weapon("d22", "D22 152mm How", 0, 17400);

    public static Weapon Msta_How = new Weapon {
        id = "msta",
        name = "2A65 152mm How",
        range = Weapon.RealRange(4000),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, frag_cannon_acc - 0.05, 2, 6, 1.0, frag_cannon_rfac - 0.05) },
            { TargetType.Light_Armor, new DamageProfile(3, frag_cannon_acc - 0.05, 2, 6, 1.0, frag_cannon_rfac - 0.05) },
            { TargetType.Heavy_Armor, new DamageProfile(3, frag_cannon_acc - 0.05, 2, 6, 1.0, frag_cannon_rfac - 0.05) }
        },
        penetration = 200,
        weaponType = WeaponType.Shell,
        shootProfile = _mstaShootProfile,
        indirect = true,
        indirectWeapon = new Weapon {
            name = "2A65 152mm How Ind",
            range = Weapon.RealRange(24700), // (53) // base bleed 24700?
            minRange = Weapon.RealRange(6500), // (53) // base bleed 24700?
            impactDelay = 1,
            aimTime = 1,
            flightTime = 1,
            setupTime = 4,
            packupTime = 1,
            artyShellsPerSalvo = 3,
            artyDelayPerShell = 2.0,
            damageProfiles = new Dictionary<TargetType, DamageProfile> {
                { TargetType.Infantry, new DamageProfile(1, mortar_acc, 1, 4, 1.0, mortar_rfac) },
                { TargetType.Light_Armor, new DamageProfile(1, mortar_acc, 1, 4, 1.0, mortar_rfac) },
                { TargetType.Heavy_Armor, new DamageProfile(1, mortar_acc, 1, 4, 1.0, mortar_rfac) }
            },
            penetration = 350, // top impact
            weaponType = WeaponType.Mortar,
            shootProfile = _mstaArtyShootProfile,
            impactProfile = _mstaArtyImpactProfile
        }
    };

    public static Weapon GiatsintB_How = new Weapon("2a36", "2A36 152mm How", 0, 28400); // 33 000 RAP
    public static Weapon GiatsintS_How = new Weapon("2a37", "2A37 152mm How", 0, 28400);// 33 000 RAP


    /// <summary>
    /// Units
    /// </summary>

    // short name Mot Inf
    public static SquadDef RU_Motor_Inf = new SquadDef
    {
        name = "Motor Rifles",
        shortName = "Mtr Rif",
        soldiers = 6,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(5, AK74, 10), // R4    //// (, true) Define this as the removableWeapon, then fire multiple weapons removing this weapon so total matches soldiers
            new UnitWeapon(1, PKP, 10), // R5
            new UnitWeapon(1, GP25, 10), // R2
            new UnitWeapon(1, RPG7, 5), // R3     //// (, 2) Define this as requiring 2 soldiers to fire, if wanted
            new UnitWeapon(1, RPG26, 1) // R2
        },
        /*
        Group 1
            1 PKP  6 AK74
            1 GP25  1 PKP  5(-1) AK74
            1 RPG-7
            1 RPG-7  1 RPG-26
            1 RPG-7  1 RPG-26  1 GP25
            */
        weaponGroups = new WeaponGroup[] {

            new WeaponGroup( new int[] {
                3, // GP25
                2, // PKP
                1, // AK74
            }),
            new WeaponGroup( new int[] {
                4, // RPG-7
                5, // RPG-26
                3, // GP25
                2, // PKP
                1, // AK74
            }),

            /*new WeaponGroup( new WeaponGroupItem[] {
                new WeaponGroupItem(2, PKP),
                new WeaponGroupItem(1, AK74),
            }),
            new WeaponGroup( new WeaponGroupItem[] {
                new WeaponGroupItem(3, GP25),
                new WeaponGroupItem(2, PKP),
                new WeaponGroupItem(1, AK74, 5),
            }),
            new WeaponGroup( new WeaponGroupItem[] {
                new WeaponGroupItem(4, RPG-7),
                new WeaponGroupItem(5, RPG-26),
                new WeaponGroupItem(3, GP25),
                new WeaponGroupItem(2, PKP),
                new WeaponGroupItem(3, AK74, 3),
            }),*/
        },

        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    
    // BMP Motor Inf
    public static SquadDef RU_Motor_Inf_Hq_Bmp = new SquadDef
    {
        name = "Motor Rifle Plt HQ BMP",
        shortName = "Mtr Rif PHQ",
        soldiers = 4,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(3, AK74, 10), // R4    //// (, true) Define this as the removableWeapon, then fire multiple weapons removing this weapon so total matches soldiers
            new UnitWeapon(1, PK, 10), // R5
        },
        weaponGroups = new WeaponGroup[] {
            new WeaponGroup( new int[] {
                2, // PK
                1, // AK74
            }),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef RU_Motor_Inf_Bmp = new SquadDef
    {
        name = "Motor Rifle Squad BMP",
        shortName = "Mtr Rif",
        soldiers = 6,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(5, AK74, 10), // R4    //// (, true) Define this as the removableWeapon, then fire multiple weapons removing this weapon so total matches soldiers
            new UnitWeapon(1, PKP, 10), // R5
            new UnitWeapon(1, GP25, 10), // R2
            new UnitWeapon(1, RPG7, 5), // R3     //// (, 2) Define this as requiring 2 soldiers to fire, if wanted
            new UnitWeapon(1, RPG26, 1) // R2
        },
        weaponGroups = new WeaponGroup[] {
            new WeaponGroup( new int[] {
                3, // GP25
                2, // PKP
                1, // AK74
            }),
            new WeaponGroup( new int[] {
                4, // RPG-7
                5, // RPG-26
                3, // GP25
                2, // PKP
                1, // AK74
            }),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    
    // BTR Motor Inf
    public static SquadDef RU_Motor_Inf_Hq_Btr = new SquadDef
    {
        name = "Motor Rifle Plt HQ BTR",
        shortName = "Mtr Rif PHQ",
        soldiers = 3,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(3, AK74, 10), // R4    //// (, true) Define this as the removableWeapon, then fire multiple weapons removing this weapon so total matches soldiers
        },
        weaponGroups = new WeaponGroup[] {
            new WeaponGroup( new int[] {
                1, // AK74
            }),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef RU_Motor_Inf_Btr = new SquadDef
    {
        name = "Motor Rifle Squad BTR",
        shortName = "Mtr Rif",
        soldiers = 7,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(6, AK74, 10), // R4    //// (, true) Define this as the removableWeapon, then fire multiple weapons removing this weapon so total matches soldiers
            new UnitWeapon(1, PKP, 10), // R5
            new UnitWeapon(1, GP25, 10), // R2
            new UnitWeapon(1, RPG7, 5), // R3     //// (, 2) Define this as requiring 2 soldiers to fire, if wanted
            new UnitWeapon(1, RPG26, 1) // R2
        },
        weaponGroups = new WeaponGroup[] {
            new WeaponGroup( new int[] {
                3, // GP25
                2, // PKP
                1, // AK74
            }),
            new WeaponGroup( new int[] {
                4, // RPG-7
                5, // RPG-26
                3, // GP25
                2, // PKP
                1, // AK74
            }),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    // Russian BMP Motor Rifle Platoon

    // 3 Squads of SL(AK-74M), MG(PKP), GR(RPG-7, AK-74M), AGR(AK-74M), SR(AK-74M, GP-25), R(AK-74M, RPG2x)
    // or 3 Squads of SL(AK-74M, RPG2x), MG(PKP), AMG(AK-74M), SR(AK-74M, GP-25, RPG2x),  R(AK-74M, RPG2x), R(AK-74M, RPG2x) - 6*, 5AK, 1PKP, 4RPG26
    // PL(AK-74M), MG(PKM), AMG(AK-74M), M(AK-74M) - 4*, 3AK, 1 PKM
    // 18AK, 3PKP, 1PKM, 1SVD, 12RPG26, 3 GP-25


    // short name Mot Inf
    public static SquadDef RU_Motor_Inf_Platoon_Bmp = new SquadDef
    {
        name = "Motor Rifles (BMP)",
        shortName = "Mtr Rif",
        soldiers = 23,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(18, AK74, 10), // R4    //// (, true) Define this as the removableWeapon, then fire multiple weapons removing this weapon so total matches soldiers
            new UnitWeapon(3, PKP, 10), // R5
            new UnitWeapon(1, PK, 15), // R5
            new UnitWeapon(3, GP25, 10), // R2
            new UnitWeapon(12, RPG26, 1) // R2
        },
       

        /*weaponGroups = new WeaponGroup[] {

            new WeaponGroup( new int[] {
                5, // GP25
                4, // SVD
                3, // PKM
                2, // PKP
                1, // AK74
            }),
            new WeaponGroup( new int[] {
                6, // RPG-26
                5, // GP25
                4, // SVD
                3, // PKM
                2, // PKP
                1, // AK74
            }),
            
        },*/

        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };


    

    
    // 3 Squads of SL(AK-74M), MG(PKP), GR(RPG-7, AK-74M), AGR(AK-74M), SR(AK-74M, GP-25), R(AK-74M, RPG2x), R(AK-74M, RPG2x)
    // or 3 Squads of SL(AK-74M), MG(PKP), AMG(AK-74M), SR(AK-74M, GP-25, RPG2x),  R(AK-74M, RPG2x), R(AK-74M, RPG2x), R(AK-74M, RPG2x) - 7*, 6AK, 1PKP, 4RPG26
    // PL(AK-74M), APL(AK-74M), M(AK-74M)? - 2-3*, 2-3AK
    //
    // 21 AK, 3 PKP, 12 RPG26, 3 GP-25

    // short name Mot Inf
    public static SquadDef RU_Motor_Inf_Platoon_Btr = new SquadDef
    {
        name = "Motor Rifles (BTR)",
        shortName = "Mtr Rif",
        soldiers = 24,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(21, AK74, 10), // R4    //// (, true) Define this as the removableWeapon, then fire multiple weapons removing this weapon so total matches soldiers
            new UnitWeapon(3, PKP, 10), // R5
            new UnitWeapon(3, GP25, 10), // R2
            new UnitWeapon(12, RPG26, 1) // R2
        },

        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    // 3 Squads of SL(AK-74M), MG(PKP), GR(RPG-7, AK-74M), AGR(AK-74M), SR(AK-74M, GP-25), R(AK-74M, RPG2x), R(AK-74M, RPG2x)
    // or 3 Squads of SL(AK-74M), MG(PKP), AMG(AK-74M), SR(AK-74M, GP-25, RPG2x),  R(AK-74M, RPG2x), R(AK-74M, RPG2x), R(AK-74M, RPG2x) - 7*, 6AK, 1PKP, 4RPG26
    // AT Squad of SL(AK), 3x AT(Metis, AK), 3x ATA(AK) - 7*, 7AK, 3Metis
    // HQ of PL(AK-74M), APL(AK-74M), M(AK-74M)? - 2-3*, 2-3AK
    //
    // 21 AK, 3 PKP, 12 RPG26, 3 GP-25

    // short name Mot Inf
    public static SquadDef RU_Motor_Inf_Platoon_Btr_At = new SquadDef
    {
        name = "Motor Rifles (BTR)",
        shortName = "Mtr Rif",
        soldiers = 31,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(28, AK74, 10), // R4    //// (, true) Define this as the removableWeapon, then fire multiple weapons removing this weapon so total matches soldiers
            new UnitWeapon(3, PKP, 10), // R5
            new UnitWeapon(3, GP25, 10), // R2
            new UnitWeapon(12, RPG26, 1), // 
            new UnitWeapon(3, MetisM1, 2), // R2
        },

        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef RU_Motor_Inf_Coy_Command = new SquadDef
    {
        name = "MR Company Commander",
        shortName = "Cmp Cmd",
        soldiers = 3,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(3, AK74, 10), // R4    //// (, true) Define this as the removableWeapon, then fire multiple weapons removing this weapon so total matches soldiers
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    
    public static SquadDef RU_Motor_Inf_Coy_HQ = new SquadDef
    {
        name = "MR Company HQ",
        shortName = "Cmp HQ",
        soldiers = 3,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(3, AK74, 10), // R4    //// (, true) Define this as the removableWeapon, then fire multiple weapons removing this weapon so total matches soldiers
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef RU_Motor_Inf_Coy_Full_Hq_Bmp = new SquadDef
    {
        name = "MR Company HQ",
        shortName = "Cmp HQ",
        soldiers = 6,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(6, AK74, 10), // R4    //// (, true) Define this as the removableWeapon, then fire multiple weapons removing this weapon so total matches soldiers
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef RU_Motor_Inf_Coy_Full_Hq_Btr = new SquadDef
    {
        name = "MR Company HQ",
        shortName = "Cmp HQ",
        soldiers = 6,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(6, AK74, 10), // R4    //// (, true) Define this as the removableWeapon, then fire multiple weapons removing this weapon so total matches soldiers
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef RU_Scouts = new SquadDef
    {
        name = "Motor Scouts",
        shortName = "Scout",
        soldiers = 2,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(2, AK74, 10),
            new UnitWeapon(1, GP25, 10),
        },
        weaponGroups = new WeaponGroup[] {
            new WeaponGroup( new int[] {
                2, // GP25
                1, // AK74
            }),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef RU_Recon_Squad = new SquadDef
    {
        name = "Motor Recon",
        shortName = "Recon",
        soldiers = 6, // or 5 https://community.battlefront.com/topic/121355-modern-russian-infantry-battalion-structure/
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(5, AK74, 10), // R4    //// (, true) Define this as the removableWeapon, then fire multiple weapons removing this weapon so total matches soldiers
            new UnitWeapon(1, PKP, 10), // R5
            new UnitWeapon(1, GP25, 10), // R2
            new UnitWeapon(1, RPG7, 5), // R3     //// (, 2) Define this as requiring 2 soldiers to fire, if wanted
            new UnitWeapon(1, RPG26, 1) // R2
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef RU_Recon_Hq = new SquadDef
    {
        name = "Motor Recon HQ",
        shortName = "Recon HQ",
        soldiers = 3, // or 5 https://community.battlefront.com/topic/121355-modern-russian-infantry-battalion-structure/
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(3, AK74, 10), // R4    //// (, true) Define this as the removableWeapon, then fire multiple weapons removing this weapon so total matches soldiers
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef RU_MMG = new SquadDef {
        name = "MMG Squad",
        shortName = "MMG",
        soldiers = 7,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(5, AK74, 10),
            new UnitWeapon(2, PK_Tripod, 20),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };


    public static SquadDef VDV_MMG = new SquadDef
    {
        name = "Weapons Squad",
        shortName = "Wps",
        soldiers = 9,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(6, AK74, 10),
            new UnitWeapon(3, PK_Tripod, 15),
            new UnitWeapon(1, RPG7, 5),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef RU_Metis_Squad = new SquadDef
    {
        name = "Metis Squad",
        shortName = "Mts",
        soldiers = 7,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(7, AK74, 10),
            new UnitWeapon(2, MetisM1, 7),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef RU_Metis_Team = new SquadDef
    {
        name = "Metis Team",
        shortName = "Mts Tm",
        soldiers = 4,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(4, AK74, 10),
            new UnitWeapon(1, MetisM1, 7),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    
    public static SquadDef RU_Konkurs_Hq = new SquadDef
    {
        name = "Konkurs HQ",
        shortName = "Knk HQ",
        soldiers = 2,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(2, AK74, 10),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef RU_Konkurs_Squad = new SquadDef
    {
        name = "Konkurs Squad",
        shortName = "Knk",
        soldiers = 7,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(7, AK74, 10),
            new UnitWeapon(2, KonkursM, 5),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef RU_Konkurs_Team = new SquadDef
    {
        name = "Konkurs Team",
        shortName = "Knk Tm",
        soldiers = 4,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(4, AK74, 10),
            new UnitWeapon(1, KonkursM, 5),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    
    public static SquadDef RU_Kornet_Hq = new SquadDef
    {
        name = "Kornet HQ",
        shortName = "Knt HQ",
        soldiers = 2,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(2, AK74, 10),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef RU_Kornet_Squad = new SquadDef
    {
        name = "Kornet Squad",
        shortName = "Knt",
        soldiers = 7,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(7, AK74, 10),
            new UnitWeapon(2, Kornet, 5),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef RU_Kornet_Team = new SquadDef
    {
        name = "Kornet Team",
        shortName = "Knt Tm",
        soldiers = 4,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(4, AK74, 10),
            new UnitWeapon(1, Kornet, 5),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    
    public static SquadDef RU_AGS30_Hq = new SquadDef
    {
        name = "AGS-30 HQ",
        shortName = "AGS30 HQ",
        soldiers = 2,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(2, AK74, 10),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef RU_AGS30_Squad = new SquadDef
    {
        name = "AGS-30 Squad",
        shortName = "AGS30",
        soldiers = 7,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(7, AK74, 10),
            new UnitWeapon(2, AGS_30, 10),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef RU_AGS30_Team = new SquadDef
    {
        name = "AGS-30 Team",
        shortName = "AGS30 Tm",
        soldiers = 4,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(4, AK74, 10),
            new UnitWeapon(1, AGS_30, 10),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef RU_AGS17_Squad = new SquadDef
    {
        name = "AGS-17 Squad",
        shortName = "AGS17",
        soldiers = 7,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(7, AK74, 10),
            new UnitWeapon(2, AGS_17, 10),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    
    public static SquadDef IglaS_Hq = new SquadDef
    {
        name = "Igla-S HQ",
        shortName = "Igla-S HQ",
        soldiers = 2,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(2, AK74, 10),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef IglaS_Squad = new SquadDef
    {
        name = "Igla-S Squad",
        shortName = "Igla-S Sq",
        soldiers = 7,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(7, AK74, 10),
            new UnitWeapon(3, IglaS, 2),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef IglaS_Team = new SquadDef
    {
        name = "Igla-S Squad",
        shortName = "Igla-S Tm",
        soldiers = 2,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(2, AK74, 10),
            new UnitWeapon(1, IglaS, 2),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef GAZ_66 = new SquadDef
    {
        name = "GAZ-66",
        unitClass = UnitClass.Truck,
        soldiers = 1,
        moves = 2,
        movementData = Pathfinder.Car_MovementData,
        //weapons = new UnitWeapon[],
        targetType = TargetType.Light_Armor,
        armor = 2,
        cargoDef = new CargoDef(12, 1)
    };

    public static SquadDef BTR_80 = new SquadDef
    {
        name = "BTR-80",
        unitClass = UnitClass.WheeledApc,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Wheeled_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, KPV, 12),
            new UnitWeapon(1, AFV_PK, 30)
        },
        targetType = TargetType.Light_Armor,
        armor = 18,
        cargoDef = new CargoDef(9, 1)
    };

    public static SquadDef BTR_80A = new SquadDef
    {
        name = "BTR-80A",
        unitClass = UnitClass.WheeledApc,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Wheeled_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A72, 10),
            new UnitWeapon(1, AFV_PK, 30)
        },
        targetType = TargetType.Light_Armor,
        armor = 18,
        cargoDef = new CargoDef(2, 1)
    };
    
    public static SquadDef BTR_80_Motor_Rifle = new SquadDef
    {
        name = "BTR-80A",
        unitClass = UnitClass.WheeledApc,
        vehiclesNum = 3,
        vehicles = new SquadDef[] {
            BTR_80A, BTR_80, BTR_80
        },
        soldiers = 6,
        moves = 3,
        movementData = Pathfinder.Wheeled_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A72, 10),
            new UnitWeapon(2, KPV, 12),
            new UnitWeapon(3, AFV_PK, 30)
        },
        targetType = TargetType.Light_Armor,
        armor = 18,
        cargoDef = new CargoDef(2, 1)
    };
    
    public static SquadDef BTR_80_Motor_Rifle_At = new SquadDef
    {
        name = "BTR-80A",
        unitClass = UnitClass.WheeledApc,
        vehiclesNum = 4,
        vehicles = new SquadDef[] {
            BTR_80A, BTR_80, BTR_80, BTR_80
        },
        soldiers = 8,
        moves = 3,
        movementData = Pathfinder.Wheeled_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A72, 10),
            new UnitWeapon(3, KPV, 12),
            new UnitWeapon(4, AFV_PK, 30)
        },
        targetType = TargetType.Light_Armor,
        armor = 18,
        cargoDef = new CargoDef(2, 1)
    };

    public static int brdm_armor = 17;
    public static int brdm_2_armor = brdm_armor;

    public static SquadDef BRDM_2 = new SquadDef
    {
        name = "BRDM-2",
        unitClass = UnitClass.WheeledApc,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Wheeled_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, KPV, 12),
            new UnitWeapon(1, AFV_PK, 30)
        },
        targetType = TargetType.Light_Armor,
        armor = 17,
        cargoDef = new CargoDef(2, 1)
    };

    public static SquadDef BRDM_2_Konkurs = new SquadDef
    {
        name = "BRDM-2 Konkurs",
        unitClass = UnitClass.WheeledApc,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Wheeled_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, AFV_KonkursM, 5),
        },
        targetType = TargetType.Light_Armor,
        armor = 17,
    };

    public static int mt_lb_armor = 20;

    public static SquadDef MT_LB = new SquadDef
    {
        name = "MT-LB",
        unitClass = UnitClass.Ifv,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, AFV_PK, 10)
        },
        targetType = TargetType.Light_Armor,
        armor = mt_lb_armor,
        cargoDef = new CargoDef(10, 1)
    };

    public static SquadDef ShturmS = new SquadDef
    {
        name = "Shturm-S",
        unitClass = UnitClass.Ifv,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, AFV_ShturmM1, 12),
        },
        targetType = TargetType.Light_Armor,
        armor = mt_lb_armor,
    };

    public static SquadDef ShturmSM = new SquadDef
    {
        name = "Shturm-SM",
        unitClass = UnitClass.Ifv,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, AFV_Ataka, 12),
        },
        targetType = TargetType.Light_Armor,
        armor = mt_lb_armor,
    };

    public static int bmd_1_armor = 12;

    public static SquadDef BMD_1 = new SquadDef
    {
        name = "BMD-2",
        unitClass = UnitClass.Ifv,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A28, 16),
            new UnitWeapon(1, AFV_Malyutka2M, 3),
            new UnitWeapon(1, AFV_PK, 25),
        },
        armor = bmd_1_armor,
        targetType = TargetType.Light_Armor,
        cargoDef = new CargoDef(5, 1)
    };
    public static SquadDef BMD_2 = new SquadDef
    {
        name = "BMD-2",
        unitClass = UnitClass.Ifv,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A42, 10),
            new UnitWeapon(1, AFV_KonkursM, 3),
            new UnitWeapon(1, AFV_PK, 25),
        },
        armor = bmd_1_armor,
        targetType = TargetType.Light_Armor,
        cargoDef = new CargoDef(5, 1)
    };

    public static int bmd_3_armor = 18;

    public static SquadDef BMD_3 = new SquadDef
    {
        name = "BMD-2",
        unitClass = UnitClass.Ifv,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A42, 20),
            new UnitWeapon(1, AFV_KonkursM, 4),
            new UnitWeapon(1, AFV_PK, 25),
        },
        armor = bmd_3_armor,
        targetType = TargetType.Light_Armor,
        cargoDef = new CargoDef(6, 1)
    };
    public static SquadDef BMD_4 = new SquadDef
    {
        name = "BMD-2",
        unitClass = UnitClass.Ifv,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A72, 15),
            new UnitWeapon(1, _2A70, 7),
            new UnitWeapon(1, Arkan_BMP3, 4),
            new UnitWeapon(1, AFV_PK, 25)
        },
        armor = bmd_3_armor,
        targetType = TargetType.Light_Armor,
        cargoDef = new CargoDef(6, 1)
    };

    public static SquadDef SprutSD = new SquadDef
    {
        name = "Sprut-SD",
        unitClass = UnitClass.Ifv,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A75, 10),
            new UnitWeapon(1, Invar, 4),
            new UnitWeapon(1, AFV_PK, 20)
        },
        armor = 35,
        targetType = TargetType.Light_Armor,
    };

    // BMP Recon, Command
    public static SquadDef BRM_1K = new SquadDef
    {
        name = "BRM-1K",
        unitClass = UnitClass.Ifv,
        soldiers = 3,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A28, 10),
            new UnitWeapon(1, AFV_PK, 30),
        },
        armor = 35,
        targetType = TargetType.Light_Armor,
        cargoDef = new CargoDef(8, 1)
    };

    public static SquadDef BMP_2 = new SquadDef
    {
        name = "BMP-2",
        unitClass = UnitClass.Ifv,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A42, 20),
            new UnitWeapon(1, AFV_KonkursM, 5),
            new UnitWeapon(1, AFV_PK, 30),
        },
        armor = 35,
        targetType = TargetType.Light_Armor,
        cargoDef = new CargoDef(8, 1)
    };

    public static SquadDef BMP_2M = new SquadDef
    {
        name = "BMP-2M",
        unitClass = UnitClass.Ifv,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A42, 20),
            new UnitWeapon(1, AFV_AGS_17, 20),
            new UnitWeapon(1, AFV_Kornet, 4),
            new UnitWeapon(1, AFV_PK, 30),
        },
        armor = 35,
        targetType = TargetType.Light_Armor,
        cargoDef = new CargoDef(8, 1)
    };

    public static SquadDef BMP_2M_Motor_Rifle = new SquadDef
    {
        name = "BMP-2M",
        unitClass = UnitClass.Ifv,
        vehicles = new SquadDef[] {
            BMP_2M, BMP_2M, BMP_2M
        },
        soldiers = 6,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(3, _2A42, 20),
            new UnitWeapon(3, AFV_AGS_17, 20),
            new UnitWeapon(3, AFV_Kornet, 4),
            new UnitWeapon(3, AFV_PK, 30),
        },
        armor = 35,
        targetType = TargetType.Light_Armor,
        cargoDef = new CargoDef(8, 1)
    };

    public static SquadDef BMP_3 = new SquadDef
    {
        name = "BMP-3",
        unitClass = UnitClass.Ifv,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A72, 20),
            new UnitWeapon(1, _2A70, 10),
            new UnitWeapon(1, Arkan_BMP3, 5),
            new UnitWeapon(1, AFV_PK, 30)
        },
        armor = 60,
        targetType = TargetType.Light_Armor,
        cargoDef = new CargoDef(8, 1)
    };

    public static SquadDef BMP_3M = new SquadDef
    {
        name = "BMP-3M",
        unitClass = UnitClass.Ifv,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A72, 20),
            new UnitWeapon(1, _2A70, 10),
            new UnitWeapon(1, Arkan_BMP3, 5),
            new UnitWeapon(1, AFV_PK, 20)
        },
        armor = 60,
        targetType = TargetType.Light_Armor,
        countermeasures = Cm.Shtora,
        cargoDef = new CargoDef(8, 1)
    };

    public static SquadDef BRM_3K = new SquadDef
    {
        name = "BRM-3K",
        unitClass = UnitClass.Ifv,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A72, 20),
            new UnitWeapon(1, AFV_PK, 30)
        },
        armor = 60,
        targetType = TargetType.Light_Armor,
        cargoDef = new CargoDef(6, 1)
    };

    public static SquadDef BMP_3_Kornet_T = new SquadDef
    {
        name = "Kornet-T",
        unitClass = UnitClass.Ifv,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(2, AFV_Kornet, 8),
        },
        armor = 60,
        targetType = TargetType.Light_Armor,
    };

    public static SquadDef BMP_3_Khrizantema = new SquadDef
    {
        name = "Khrizantema-S",
        unitClass = UnitClass.Ifv,
        soldiers = 2,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(2, AFV_Khrizantema, 17),
        },
        armor = 60,
        targetType = TargetType.Light_Armor,
    };
    

    public static SquadDef T_72 = new SquadDef
    {
        name = "T-72",
        unitClass = UnitClass.Tank,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A46M, 20),
            new UnitWeapon(1, AFV_PK, 20)
        },
        targetType = TargetType.Heavy_Armor,
        armor = 400
    };

    public static SquadDef T_72A = new SquadDef
    {
        name = "T-72 A",
        unitClass = UnitClass.Tank,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A46M, 20),
            new UnitWeapon(1, AFV_PK, 20)
        },
        targetType = TargetType.Heavy_Armor,
        armor = 420
    };

    public static SquadDef T_72B = new SquadDef
    {
        name = "T-72 B", // K1
        unitClass = UnitClass.Tank,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A46M, 20),
            new UnitWeapon(1, Svir, 4),
            new UnitWeapon(1, AFV_PK, 20)
        },
        targetType = TargetType.Heavy_Armor,
        armor = 520
    };
    public static SquadDef T_72B_K5 = new SquadDef
    {
        name = "T-72 B", // K5
        unitClass = UnitClass.Tank,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A46M, 20),
            new UnitWeapon(1, Svir, 4),
            new UnitWeapon(1, AFV_PK, 20)
        },
        targetType = TargetType.Heavy_Armor,
        armor = 750
    };
    public static SquadDef T_72B3 = new SquadDef
    {
        name = "T-72 B3",
        unitClass = UnitClass.Tank,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A46M, 20),
            new UnitWeapon(1, Invar, 4),
            new UnitWeapon(1, AFV_PK, 20)
        },
        targetType = TargetType.Heavy_Armor,
        armor = 750, // K5
        countermeasures = Cm.Shtora
    };
    public static SquadDef T_72B3_2016 = new SquadDef
    {
        name = "T-72 B3 2016", // K5
        unitClass = UnitClass.Tank,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A46M, 20),
            new UnitWeapon(1, Invar, 4),
            new UnitWeapon(1, AFV_PK, 20)
        },
        targetType = TargetType.Heavy_Armor,
        armor = 1000, // Relikt
        countermeasures = Cm.Shtora
    };
    /*public static SquadDef T_72B_R = new SquadDef
    {
        name = "T-72 B R",
        unitClass = UnitClass.Tank,
        soldiers = 4,
        moves = 3,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A46M, 20),
            new UnitWeapon(1, Refleks, 4),
            new UnitWeapon(1, AFV_PK, 20)
        },
        targetType = TargetType.Heavy_Armor,
        armor = 1000
    };*/

    public static SquadDef T_80U = new SquadDef
    {
        name = "T-80 U",
        unitClass = UnitClass.Tank,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A46M, 20),
            new UnitWeapon(1, Refleks, 4),
            new UnitWeapon(1, AFV_PK, 20)
        },
        targetType = TargetType.Heavy_Armor,
        armor = 750, // K5
        countermeasures = Cm.Shtora
    };

    public static SquadDef T_80BVM = new SquadDef
    {
        name = "T-80 BVM",
        unitClass = UnitClass.Tank,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A46M, 20),
            new UnitWeapon(1, Refleks, 4),
            new UnitWeapon(1, AFV_PK, 20)
        },
        targetType = TargetType.Heavy_Armor,
        armor = 1050, // Relikt
        countermeasures = Cm.Shtora
    };

    public static SquadDef T_90 = new SquadDef
    {
        name = "T-90",
        unitClass = UnitClass.Tank,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A46M, 20),
            new UnitWeapon(1, Refleks, 4),
            new UnitWeapon(1, AFV_PK, 20)
        },
        targetType = TargetType.Heavy_Armor,
        armor = 700, // K5
        countermeasures = Cm.Shtora
    };

    public static SquadDef T_90A = new SquadDef
    {
        name = "T-90 A",
        unitClass = UnitClass.Tank,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A46M, 20),
            new UnitWeapon(1, Invar, 4),
            new UnitWeapon(1, AFV_PK, 20)
        },
        targetType = TargetType.Heavy_Armor,
        armor = 700, // K5
        countermeasures = Cm.Shtora
    };

    public static SquadDef T_90AM = new SquadDef
    {
        name = "T-90 AM",
        unitClass = UnitClass.Tank,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A46M, 20),
            new UnitWeapon(1, Invar, 4),
            new UnitWeapon(1, AFV_PK, 20)
        },
        targetType = TargetType.Heavy_Armor,
        armor = 1000, // Relikt
        countermeasures = Cm.Shtora
    };

    public static SquadDef T_90M = new SquadDef
    {
        name = "T-90 M",
        unitClass = UnitClass.Tank,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A82_1M, 20),
            new UnitWeapon(1, Invar, 4),
            new UnitWeapon(1, AFV_PK, 20)
        },
        targetType = TargetType.Heavy_Armor,
        armor = 1000, // Relikt
        countermeasures = Cm.Afghanit
    };

    public static SquadDef T_14 = new SquadDef
    {
        name = "Armata",
        unitClass = UnitClass.Tank,
        soldiers = 3,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A82_1M, 20),
            new UnitWeapon(1, Refleks, 4),
            new UnitWeapon(1, AFV_PK, 20)
        },
        targetType = TargetType.Heavy_Armor,
        armor = 900, // ???? Malachit
        countermeasures = Cm.Afghanit
    };

    public static SquadDef T_12A = new SquadDef
    {
        name = "T-12A AT Gun",
        unitClass = UnitClass.Artillery,
        soldiers = 4,
        moves = 1,
        setupTime = 2,
        packupTime = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2A29, 5),
            new UnitWeapon(1, Arkan, 4),
        },
        targetType = TargetType.Infantry,
    };

    //////////////////////////// AA

    // Shilka
    // Biryusa
    // Shilka M4
    // Shilka M5
    public static int shilka_armor = 14;

    // Biryusa
    public static SquadDef Shilka_M3 = new SquadDef
    {
        name = "Shilka-M3 (Biryusa)",
        unitClass = UnitClass.Ifv,
        soldiers = 3,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(4, _2A7, 10),
        },
        targetType = TargetType.Light_Armor,
        armor = shilka_armor,
    };

    public static SquadDef Shilka_M4 = new SquadDef
    {
        name = "Shilka-M4",
        unitClass = UnitClass.Ifv,
        soldiers = 3,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(4, _2A7, 10),
        },
        targetType = TargetType.Light_Armor,
        armor = shilka_armor,
    };
    public static SquadDef Shilka_M4_Strelet = new SquadDef
    {
        name = "Shilka-M4",
        unitClass = UnitClass.Ifv,
        soldiers = 3,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(4, _2A7, 10),
            new UnitWeapon(1, Strelet_IglaS, 4),
        },
        targetType = TargetType.Light_Armor,
        armor = shilka_armor,
    };

    public static SquadDef Shilka_M5 = new SquadDef
    {
        name = "Shilka-M5",
        unitClass = UnitClass.Ifv,
        soldiers = 3,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(4, _2A7, 10),
        },
        targetType = TargetType.Light_Armor,
        armor = shilka_armor,
    };
    public static SquadDef Shilka_M5_Strelet = new SquadDef
    {
        name = "Shilka-M5",
        unitClass = UnitClass.Ifv,
        soldiers = 3,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(4, _2A7, 10),
            new UnitWeapon(1, Strelet_IglaS, 4),
        },
        targetType = TargetType.Light_Armor,
        armor = shilka_armor,
    };


    public static int tunguska_armor = 20;

    public static SquadDef Tunguska = new SquadDef
    {
        name = "Tunguska",
        unitClass = UnitClass.Ifv,
        soldiers = 3,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(4, _2A38, 10),
            new UnitWeapon(1, _9M311, 8),
        },
        targetType = TargetType.Light_Armor,
        armor = tunguska_armor,
    };

    public static SquadDef TunguskaM = new SquadDef
    {
        name = "Tunguska-M",
        unitClass = UnitClass.Ifv,
        soldiers = 3,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(4, _2A38M, 10),
            new UnitWeapon(1, _9M311, 8),
        },
        targetType = TargetType.Light_Armor,
        armor = tunguska_armor,
    };

    public static SquadDef TunguskaM1 = new SquadDef
    {
        name = "Tunguska-M",
        unitClass = UnitClass.Ifv,
        soldiers = 3,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(4, _2A38M, 10),
            new UnitWeapon(1, _9M311_M1, 8),
        },
        targetType = TargetType.Light_Armor,
        armor = tunguska_armor,
    };

    // BRDM Gaskin
    // MT-LB Gopher

    public static SquadDef BRDM_Strela_1M = new SquadDef
    {
        name = "BRDM Strela-1M",
        unitClass = UnitClass.WheeledApc,
        soldiers = 3,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Strela_1M, 4),
        },
        targetType = TargetType.Light_Armor,
        armor = brdm_armor,
    };
    public static SquadDef MT_LB_Strela_10M3 = new SquadDef
    {
        name = "MT-LB Strela-10",
        unitClass = UnitClass.Ifv,
        soldiers = 3,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _9M37M, 12),
        },
        targetType = TargetType.Light_Armor,
        armor = mt_lb_armor,
    };
    public static SquadDef MT_LB_Strela_10M4 = new SquadDef
    {
        name = "MT-LB Strela-10",
        unitClass = UnitClass.Ifv,
        soldiers = 3,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _9M333, 12),
        },
        targetType = TargetType.Light_Armor,
        armor = mt_lb_armor,
    };

    /*public static SquadDef SosnaR = new SquadDef
    {
        name = "Sosna-R Strela-10",
        unitClass = UnitClass.Ifv,
        soldiers = 3,
        moves = 3,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _9M333, 12),
        },
        targetType = TargetType.Light_Armor,
        armor = mt_lb_armor,
    };*/

    // Strela-3
    // Osa
    // Buk
    // Tor

    public static SquadDef Osa = new SquadDef
    {
        name = "Osa-AKM",
        unitClass = UnitClass.WheeledApc,
        soldiers = 5,
        moves = 3,
        movementData = Pathfinder.WheeledSlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _9M33_M3, 6),
        },
        targetType = TargetType.Light_Armor,
        armor = 10,
    };
    public static SquadDef Tor = new SquadDef
    {
        name = "Tor",
        unitClass = UnitClass.Ifv,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _9M330, 8),
        },
        targetType = TargetType.Light_Armor,
        armor = 10,
    };
    public static SquadDef TorM1 = new SquadDef
    {
        name = "Tor-M1",
        unitClass = UnitClass.Ifv,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _9M331, 8),
        },
        targetType = TargetType.Light_Armor,
        armor = 10,
    };
    public static SquadDef TorM2 = new SquadDef
    {
        name = "Tor-M2",
        unitClass = UnitClass.Ifv,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _9M332, 8),
        },
        targetType = TargetType.Light_Armor,
        armor = 10,
    };
    /*public static SquadDef BukM2 = new SquadDef
    {
        name = "Buk-M2",
        unitClass = UnitClass.Ifv,
        soldiers = 4,
        moves = 2,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Strela_10M4, 12),
        },
        targetType = TargetType.Light_Armor,
        armor = mt_lb_armor,
    };*/
    public static int buk_armor = 10;

    public static SquadDef BukM3_Command = new SquadDef
    {
        name = "Buk-M3 Command",
        unitClass = UnitClass.Ifv,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.TrackedSlow_MovementData,
        weapons = new UnitWeapon[] {
        },
        targetType = TargetType.Light_Armor,
        armor = buk_armor,
    };
    public static SquadDef BukM3_TAR = new SquadDef
    {
        name = "Buk-M3 TAR",
        unitClass = UnitClass.Ifv,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.TrackedSlow_MovementData,
        weapons = new UnitWeapon[] {
        },
        targetType = TargetType.Light_Armor,
        armor = buk_armor,
    };
    public static SquadDef BukM3_TELAR = new SquadDef
    {
        name = "Buk-M3 TELAR",
        unitClass = UnitClass.Ifv,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.TrackedSlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, BukM3_Missile, 6),
        },
        targetType = TargetType.Light_Armor,
        armor = buk_armor,
    };
    public static SquadDef BukM3_TEL = new SquadDef
    {
        name = "Buk-M3 TEL",
        unitClass = UnitClass.Ifv,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.TrackedSlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, BukM3_Missile, 12),
        },
        targetType = TargetType.Light_Armor,
        armor = buk_armor,
    };

    public static SquadDef Hind_PN = new SquadDef
    {
        name = "Mi-24PN Hind", // name
        unitClass = UnitClass.Helicopter,
        soldiers = 2, // soldiers
        moves = 5, // moves
        movementData = Pathfinder.Helicopter_MovementData,
        weapons = new UnitWeapon[] { // weapons
            new UnitWeapon(1, Gsh_30_2k , 25),
            //new UnitWeapon(1, Gsh_30_2k_Fast , 10),
            new UnitWeapon(1, AtakaM, 8),
            //new UnitWeapon(1, Ataka, 4),
            new UnitWeapon(2, S_5, 3)
        },
        targetType = TargetType.Helicopter, // target type class,
        armor = 25,
        minAltitude = 4,
        maxAltitude = 500,
        gainAltCost = 1,
        loseAltPayment = 0.5
    };

    public static SquadDef Hip = new SquadDef
    {
        name = "Mi-17 Hip", // name
        unitClass = UnitClass.Helicopter,
        soldiers = 3, // soldiers
        moves = 5, // moves
        movementData = Pathfinder.Helicopter_MovementData,
        weapons = new UnitWeapon[] { // weapons
        },
        targetType = TargetType.Helicopter, // target type class,
        armor = 3,
        minAltitude = 1,
        maxAltitude = 20,
        gainAltCost = 1,
        loseAltPayment = 0.5,
        cargoDef = new CargoDef(24, 10)
    };


    
    public static SquadDef RU_82mm_Mortar_Hq = new SquadDef
    {
        name = "Podnos Mortar Hq",
        shortName = "82 Mtr HQ",
        soldiers = 5,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(5, AK74, 10),
        },
        targetType = TargetType.Infantry
    };

    // short name 82 Mtr
    public static SquadDef RU_82mm_Mortar = new SquadDef
    {
        name = "Podnos Mortar",
        shortName = "82 Mtr",
        unitClass = UnitClass.Mortar,
        soldiers = 5,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2B14_Mortar, 10),
            new UnitWeapon(5, AK74, 10),
        },
        targetType = TargetType.Infantry
    };
    
    public static SquadDef RU_120mm_Mortar_Hq = new SquadDef
    {
        name = "Sani Mortar Hq",
        shortName = "120 Mtr HQ",
        unitClass = UnitClass.Mortar,
        soldiers = 5,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(5, AK74, 10),
        },
        targetType = TargetType.Infantry
    };

    // short name 120 Mtr
    public static SquadDef RU_120mm_Mortar = new SquadDef
    {
        name = "Sani Mortar",
        shortName = "120 Mtr",
        unitClass = UnitClass.Mortar,
        soldiers = 5,
        moves = 1,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2B11_Mortar, 10),
            new UnitWeapon(7, AK74, 10),
        },
        targetType = TargetType.Infantry
    };



    public static SquadDef NonaS = new SquadDef
    {
        name = "Nona-S",
        unitClass = UnitClass.MechMortar,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.Tracked_MovementData,
        weapons = new UnitWeapon[]
        {
            new UnitWeapon(1, _2A60_Mortar, 13)
        },
        armor = 12, // BMD-1 based
        targetType = TargetType.Light_Armor
    };

    public static SquadDef NonaSvk = new SquadDef
    {
        name = "Nona-SVK",
        unitClass = UnitClass.WheeledMortar,
        soldiers = 4,
        moves = 3,
        movementData = Pathfinder.Wheeled_MovementData,
        weapons = new UnitWeapon[]
        {
            new UnitWeapon(1, _2A51_Mortar, 10)
        },
        armor = 18,
        targetType = TargetType.Light_Armor
    };

    public static SquadDef RU_122mm_How = new SquadDef("122mm Howitzer", 6, 1, new UnitWeapon[] {
        new UnitWeapon(1, D30_How, 10),
        new UnitWeapon(6, AK74, 10),
    }, TargetType.Infantry);

    public static SquadDef RU_152mm_How = new SquadDef("152mm Howitzer", 8, 1, new UnitWeapon[] {
        new UnitWeapon(1, Msta_How, 10),
        new UnitWeapon(8, AK74, 10),
    }, TargetType.Infantry);


    public static SquadDef Gvozdika = new SquadDef
    {
        name = "Gvozdika",
        soldiers = 4,
        moves = 2,
        movementData = Pathfinder.TrackedSlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, D30_How, 10),
        },
        targetType = TargetType.Light_Armor,
        armor = 12,
    };

    public static SquadDef Akatsiya = new SquadDef
    {
        name = "Akatsiya",
        unitClass = UnitClass.MechArtillery,
        soldiers = 4,
        moves = 2,
        movementData = Pathfinder.TrackedSlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, D22_How, 10),
            new UnitWeapon(1, AFV_PK, 10),
        },
        targetType = TargetType.Light_Armor,
        armor = 12
    };

    public static SquadDef MstaS = new SquadDef
    {
        name = "Msta-S",
        unitClass = UnitClass.MechArtillery,
        soldiers = 5,
        moves = 3,
        movementData = Pathfinder.TrackedSlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Msta_How, 10),
            new UnitWeapon(1, NSV, 10),
        },
        targetType = TargetType.Light_Armor,
        armor = 15,
        setupTime = 4,
        packupTime = 1,
    };
    public static SquadDef MstaSM1 = new SquadDef
    {
        name = "Msta-SM1",
        unitClass = UnitClass.MechArtillery,
        soldiers = 5,
        moves = 3,
        movementData = Pathfinder.TrackedSlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Msta_How, 10),
            new UnitWeapon(1, NSV, 10),
        },
        targetType = TargetType.Light_Armor,
        armor = 15,
        setupTime = 3,
        packupTime = 1,
    };
    public static SquadDef MstaSM2 = new SquadDef
    {
        name = "Msta-SM2",
        unitClass = UnitClass.MechArtillery,
        soldiers = 5,
        moves = 3,
        movementData = Pathfinder.TrackedSlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Msta_How, 10),
            new UnitWeapon(1, NSV, 10),
        },
        targetType = TargetType.Light_Armor,
        armor = 15,
        setupTime = 2,
        packupTime = 1,
    };

    public static SquadDef GiatsintS = new SquadDef
    {
        name = "Giatsint-S",
        unitClass = UnitClass.MechArtillery,
        soldiers = 5,
        moves = 2,
        movementData = Pathfinder.TrackedSlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, GiatsintS_How, 10),
            new UnitWeapon(1, AFV_PK, 10),
        },
        targetType = TargetType.Light_Armor,
        armor = 12
    };
    

    //////////////////////// Fixed Wing

    public static SquadDef SU_34_AT = new SquadDef
    {
        name = "SU-34 (AT Load)", // name
        unitClass = UnitClass.FixedWing,
        soldiers = 2, // soldiers
        moves = 5, // moves
        weapons = new UnitWeapon[] { // weapons
            new UnitWeapon(1, Gsh_30_1 , 25), // 180 rounds
            new UnitWeapon(1, AtakaM, 8),
            //new UnitWeapon(1, Ataka, 4),
            new UnitWeapon(2, S_5, 3)
        },
        targetType = TargetType.FixedWing, // target type class,
        armor = 10,
        minAltitude = 4,
        maxAltitude = 500,
        gainAltCost = 1,
        loseAltPayment = 0.5
    };



}


public static class DefsWW2 {
    
    static GameObject bullet_s = (GameObject)Resources.Load("GO/Bullet_S");
    static GameObject bullet_m = (GameObject)Resources.Load("GO/Bullet_M");
    static GameObject bullet_l = (GameObject)Resources.Load("GO/Bullet_L");
    static GameObject bullet_vl = (GameObject)Resources.Load("GO/Bullet_VL");
    static GameObject shell = (GameObject)Resources.Load("GO/shell");


    public static double smg_acc = 0.15;
    public static double smg_rfac = 0.8;
    public static double smg_vs_plane_acc = smg_acc;
    public static double smg_vs_plane_rfac = smg_rfac;

    public static Weapon Sten = new Weapon
    {
        name = "STEN",
        range = Weapon.RealRange(200),
        shots = 20,
        accuracy = smg_acc,
        rfactor = smg_rfac,
        
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            /*{ TargetType.Infantry, new DamageProfile(20, smg_acc, 1, smg_rfac) },
            { TargetType.Gun, new DamageProfile(20, smg_acc, 1, smg_rfac) },
            { TargetType.Vehicle, new DamageProfile(20, smg_acc, 1, smg_rfac) },
            { TargetType.Armor, new DamageProfile(20, smg_acc, 1, smg_rfac) },*/
            { TargetType.Infantry, new DamageProfile(1) },
            { TargetType.Gun, new DamageProfile(1) },
            { TargetType.Vehicle, new DamageProfile(1) },
            { TargetType.Armor, new DamageProfile(1) },
        },
        penetration = 2,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 20, 3, 7, 0.5, 1.8, 600, 0.0, 1, true, bullet_s, 10)
    };

    public static Weapon MP40 = new Weapon
    {
        name = "MP40",
        range = Weapon.RealRange(200),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(20, smg_acc, 1, smg_rfac) },
            { TargetType.Gun, new DamageProfile(20, smg_acc, 1, smg_rfac) },
            { TargetType.Vehicle, new DamageProfile(20, smg_acc, 1, smg_rfac) },
            { TargetType.Armor, new DamageProfile(20, smg_acc, 1, smg_rfac) },
        },
        penetration = 3,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 20, 3, 7, 0.5, 1.8, 600, 0.0, 1, true, bullet_s, 8)
    };
    
    public static double bolt_rifle_acc = 0.15;
    public static double bolt_rifle_rfac = 0.8;
    public static double bolt_rifle_vs_plane_acc = bolt_rifle_acc;
    public static double bolt_rifle_vs_plane_rfac = bolt_rifle_rfac;

    public static ShootProfile boltRifleShootProfile = new ShootProfile(SoundLib.m249Shot, 4, 60, 0.35, 1, true, bullet_s, 20);

    public static Weapon SMLE = new Weapon
    {
        name = "Lee-Enfield",
        range = Weapon.RealRange(600),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(4, bolt_rifle_acc, 1, bolt_rifle_rfac) },
            { TargetType.Gun, new DamageProfile(4, bolt_rifle_acc, 1, bolt_rifle_rfac) },
            { TargetType.Vehicle, new DamageProfile(4, bolt_rifle_acc, 1, bolt_rifle_rfac) },
            { TargetType.Armor, new DamageProfile(4, bolt_rifle_acc, 1, bolt_rifle_rfac) },
        },
        penetration = 8,
        shootProfile = boltRifleShootProfile
    };

    public static Weapon Kar98k = new Weapon
    {
        name = "Karabiner 98k",
        range = Weapon.RealRange(600),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(5, bolt_rifle_acc, 1, bolt_rifle_rfac) },
            { TargetType.Gun, new DamageProfile(5, bolt_rifle_acc, 1, bolt_rifle_rfac) },
            { TargetType.Vehicle, new DamageProfile(5, bolt_rifle_acc, 1, bolt_rifle_rfac) },
            { TargetType.Armor, new DamageProfile(5, bolt_rifle_acc, 1, bolt_rifle_rfac) },
        },
        penetration = 8,
        shootProfile = boltRifleShootProfile
    };
    
    public static double lmg_acc = 0.09;
    public static double lmg_rfac = 0.8;
    public static double lmg_vs_plane_acc = lmg_acc;
    public static double lmg_vs_plane_rfac = lmg_rfac;

    public static ShootProfile brenShootProfile = new ShootProfile(SoundLib.m249Shot, 20, 3, 8, 0.9, 2.4, 600, 0.0, 1, true, bullet_s, 20);
    
    public static Weapon Bren = new Weapon
    {
        name = "BREN",
        range = Weapon.RealRange(700),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(20, lmg_acc, 1, lmg_rfac) },
            { TargetType.Gun, new DamageProfile(20, lmg_acc, 1, lmg_rfac) },
            { TargetType.Vehicle, new DamageProfile(20, lmg_acc, 1, lmg_rfac) },
            { TargetType.Armor, new DamageProfile(20, lmg_acc, 1, lmg_rfac) },
        },
        penetration = 8,
        shootProfile = brenShootProfile
    };

    public static ShootProfile mg34ShootProfile = new ShootProfile(SoundLib.m249Shot, 25, 5, 9, 0.9, 2.4, 900, 0.0, 1, true, bullet_s, 20);
    public static ShootProfile mg42ShootProfile = new ShootProfile(SoundLib.m249Shot, 30, 6, 10, 0.9, 2.4, 1200, 0.0, 1, true, bullet_s, 20);

    public static Weapon MG34 = new Weapon
    {
        name = "MG34",
        range = Weapon.RealRange(700),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(25, lmg_acc - 0.05, 1, lmg_rfac - 0.05) },
            { TargetType.Gun, new DamageProfile(25, lmg_acc - 0.05, 1, lmg_rfac - 0.05) },
            { TargetType.Vehicle, new DamageProfile(25, lmg_acc - 0.05, 1, lmg_rfac - 0.05) },
            { TargetType.Armor, new DamageProfile(25, lmg_acc - 0.05, 1, lmg_rfac - 0.05) },
        },
        penetration = 8,
        shootProfile = mg34ShootProfile
    };

    public static Weapon MG42 = new Weapon
    {
        name = "MG42",
        range = Weapon.RealRange(700),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(30, lmg_acc - 0.05, 1, lmg_rfac - 0.05) },
            { TargetType.Gun, new DamageProfile(30, lmg_acc - 0.05, 1, lmg_rfac - 0.05) },
            { TargetType.Vehicle, new DamageProfile(30, lmg_acc - 0.05, 1, lmg_rfac - 0.05) },
            { TargetType.Armor, new DamageProfile(30, lmg_acc - 0.05, 1, lmg_rfac - 0.05) },
        },
        penetration = 8,
        shootProfile = mg42ShootProfile
    };

    public static double hmg_acc = 0.1;
    public static double hmg_rfac = 1.0;
    public static double hmg_vs_plane_acc = hmg_acc;
    public static double hmg_vs_plane_rfac = hmg_rfac;

    public static double hmg_vec_acc = 0.1;
    public static double hmg_vec_rfac = 1.0;
    public static double hmg_vec_vs_plane_acc = hmg_vec_acc;
    public static double hmg_vec_vs_plane_rfac = hmg_vec_rfac;

    public static double hmg_hull_acc = 0.1;
    public static double hmg_hull_rfac = 1.0;
    public static double hmg_hull_vs_plane_acc = hmg_hull_acc;
    public static double hmg_hull_vs_plane_rfac = hmg_hull_rfac;

    public static Weapon Vickers = new Weapon {
        name = "Vickers",
        range = Weapon.RealRange(1100),
        //shots = 30,
        //accuracy = hmg_acc,
        //rfac = hmg_rfac,
        //damage = 1,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(30, hmg_acc, 1, hmg_rfac) },
            { TargetType.Gun, new DamageProfile(30, hmg_acc, 1, hmg_rfac) },
            { TargetType.Vehicle, new DamageProfile(30, hmg_acc, 1, hmg_rfac) },
            { TargetType.Armor, new DamageProfile(30, hmg_acc, 1, hmg_rfac) },
        },
        penetration = 8,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 30, 600, 0.0, 1, true, bullet_s, 20)
    };
    
    public static Weapon Bren_Vec = new Weapon
    {
        name = "BREN",
        range = Weapon.RealRange(1000),
        //shots = 18,
        //accuracy = hmg_vec_acc,
        //rfac = hmg_vec_rfac,
        //damage = 1,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(18, hmg_vec_acc, 1, hmg_vec_rfac) },
            { TargetType.Gun, new DamageProfile(18, hmg_vec_acc, 1, hmg_vec_rfac) },
            { TargetType.Vehicle, new DamageProfile(18, hmg_vec_acc, 1, hmg_vec_rfac) },
            { TargetType.Armor, new DamageProfile(18, hmg_vec_acc, 1, hmg_vec_rfac) },
        },
        penetration = 8,
        shootProfile = brenShootProfile
    };

    public static Weapon Besa = new Weapon
    {
        name = "Besa Coax",
        range = Weapon.RealRange(1100),
        //shots = 30,
        //accuracy = hmg_vec_acc,
        //rfac = hmg_vec_rfac,
        //damage = 1,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(30, hmg_vec_acc, 1, hmg_vec_rfac) },
            { TargetType.Gun, new DamageProfile(30, hmg_vec_acc, 1, hmg_vec_rfac) },
            { TargetType.Vehicle, new DamageProfile(30, hmg_vec_acc, 1, hmg_vec_rfac) },
            { TargetType.Armor, new DamageProfile(30, hmg_vec_acc, 1, hmg_vec_rfac) },
        },
        penetration = 8,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 30, 600, 0.0, 1, true, bullet_s, 20)
    };

    public static Weapon Besa_Hull = new Weapon
    {
        name = "Besa Hull",
        range = Weapon.RealRange(1100),
        //shots = 30,
        //accuracy = hmg_hull_acc,
        //rfac = hmg_hull_rfac,
        //damage = 1,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(30, hmg_hull_acc, 1, hmg_hull_rfac) },
            { TargetType.Gun, new DamageProfile(30, hmg_hull_acc, 1, hmg_hull_rfac) },
            { TargetType.Vehicle, new DamageProfile(30, hmg_hull_acc, 1, hmg_hull_rfac) },
            { TargetType.Armor, new DamageProfile(30, hmg_hull_acc, 1, hmg_hull_rfac) },
        },
        penetration = 8,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 30, 600, 0.0, 1, true, bullet_s, 20)
    };

    public static Weapon M1919_Vec = new Weapon
    {
        name = "M1919A4 Coax",
        range = Weapon.RealRange(1000),
        //shots = 30,
        //accuracy = hmg_vec_acc,
        //rfac = hmg_vec_rfac,
        //damage = 1,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(30, hmg_vec_acc, 1, hmg_vec_rfac) },
            { TargetType.Gun, new DamageProfile(30, hmg_vec_acc, 1, hmg_vec_rfac) },
            { TargetType.Vehicle, new DamageProfile(30, hmg_vec_acc, 1, hmg_vec_rfac) },
            { TargetType.Armor, new DamageProfile(30, hmg_vec_acc, 1, hmg_vec_rfac) },
        },
        penetration = 8,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 30, 600, 0.0, 1, true, bullet_s, 20)
    };
    public static Weapon M1919_Vec_Hull = new Weapon
    {
        name = "M1919A4 Hull",
        range = Weapon.RealRange(1000),
        //shots = 30,
        //accuracy = hmg_hull_acc,
        //rfac = hmg_hull_rfac,
        //damage = 1,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(30, hmg_hull_acc, 1, hmg_hull_rfac) },
            { TargetType.Gun, new DamageProfile(30, hmg_hull_acc, 1, hmg_hull_rfac) },
            { TargetType.Vehicle, new DamageProfile(30, hmg_hull_acc, 1, hmg_hull_rfac) },
            { TargetType.Armor, new DamageProfile(30, hmg_hull_acc, 1, hmg_hull_rfac) },
        },
        penetration = 8,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 30, 600, 0.0, 1, true, bullet_s, 20)
    };
    
    public static ShootProfile smg34ShootProfile = new ShootProfile(SoundLib.m249Shot, 38, 7, 11, 0.9, 2.4, 900, 0.0, 1, true, bullet_s, 20);
    public static ShootProfile smg42ShootProfile = new ShootProfile(SoundLib.m249Shot, 46, 8, 12, 0.9, 2.4, 1200, 0.0, 1, true, bullet_s, 20);

    public static Weapon SMG34 = new Weapon
    {
        name = "MG34",
        range = Weapon.RealRange(1100),
        //shots = 38,
        //accuracy = hmg_acc - 0.05,
        //rfac = hmg_rfac - 0.05,
        //damage = 1,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(37, hmg_acc - 0.05, 1, hmg_rfac - 0.05) },
            { TargetType.Gun, new DamageProfile(37, hmg_acc - 0.05, 1, hmg_rfac - 0.05) },
            { TargetType.Vehicle, new DamageProfile(37, hmg_acc - 0.05, 1, hmg_rfac - 0.05) },
            { TargetType.Armor, new DamageProfile(37, hmg_acc - 0.05, 1, hmg_rfac - 0.05) },
        },
        penetration = 8,
        shootProfile = smg34ShootProfile
    };

    public static Weapon SMG42 = new Weapon
    {
        name = "MG42",
        range = Weapon.RealRange(1100),
        //shots = 46,
        //accuracy = hmg_acc - 0.05,
        //rfac = hmg_rfac - 0.05,
        //damage = 1,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(44, hmg_acc - 0.05, 1, hmg_rfac - 0.05) },
            { TargetType.Gun, new DamageProfile(44, hmg_acc - 0.05, 1, hmg_rfac - 0.05) },
            { TargetType.Vehicle, new DamageProfile(44, hmg_acc - 0.05, 1, hmg_rfac - 0.05) },
            { TargetType.Armor, new DamageProfile(44, hmg_acc - 0.05, 1, hmg_rfac - 0.05) },
        },
        penetration = 8,
        shootProfile = smg42ShootProfile
    };

    public static Weapon MG34_Vec = new Weapon
    {
        name = "MG34",
        range = Weapon.RealRange(1000),
        //shots = 38,
        //accuracy = hmg_acc - 0.05,
        //rfac = hmg_rfac - 0.05,
        //damage = 1,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(37, hmg_vec_acc - 0.05, 1, hmg_vec_rfac - 0.05) },
            { TargetType.Gun, new DamageProfile(37, hmg_vec_acc - 0.05, 1, hmg_vec_rfac - 0.05) },
            { TargetType.Vehicle, new DamageProfile(37, hmg_vec_acc - 0.05, 1, hmg_vec_rfac - 0.05) },
            { TargetType.Armor, new DamageProfile(37, hmg_vec_acc - 0.05, 1, hmg_vec_rfac - 0.05) },
        },
        penetration = 8,
        shootProfile = mg34ShootProfile
    };
    public static Weapon MG34_Vec_Roof = new Weapon
    {
        name = "MG34",
        range = Weapon.RealRange(1000),
        //shots = 46,
        //accuracy = hmg_acc - 0.05,
        //rfac = hmg_rfac - 0.05,
        //damage = 1,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(38, hmg_vec_acc - 0.05, 1, hmg_vec_rfac - 0.05) },
            { TargetType.Gun, new DamageProfile(38, hmg_vec_acc - 0.05, 1, hmg_vec_rfac - 0.05) },
            { TargetType.Vehicle, new DamageProfile(38, hmg_vec_acc - 0.05, 1, hmg_vec_rfac - 0.05) },
            { TargetType.Armor, new DamageProfile(38, hmg_vec_acc - 0.05, 1, hmg_vec_rfac - 0.05) },
        },
        penetration = 8,
        shootProfile = mg34ShootProfile
    };
    public static Weapon MG34_Vec_Hull = new Weapon
    {
        name = "MG34",
        range = Weapon.RealRange(1000),
        //shots = 38,
        //accuracy = hmg_acc - 0.05,
        //rfac = hmg_rfac - 0.05,
        //damage = 1,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(38, hmg_hull_acc - 0.05, 1, hmg_hull_rfac - 0.05) },
            { TargetType.Gun, new DamageProfile(38, hmg_hull_acc - 0.05, 1, hmg_hull_rfac - 0.05) },
            { TargetType.Vehicle, new DamageProfile(38, hmg_hull_acc - 0.05, 1, hmg_hull_rfac - 0.05) },
            { TargetType.Armor, new DamageProfile(38, hmg_hull_acc - 0.05, 1, hmg_hull_rfac - 0.05) },
        },
        penetration = 8,
        shootProfile = mg34ShootProfile
    };
    public static Weapon MG34_Vec_AA = new Weapon
    {
        name = "MG34",
        range = Weapon.RealRange(1000),
        //shots = 38,
        //accuracy = hmg_acc - 0.05,
        //rfac = hmg_rfac - 0.05,
        //damage = 1,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(37, hmg_vec_acc - 0.05, 1, hmg_vec_rfac - 0.05) },
            { TargetType.Gun, new DamageProfile(37, hmg_vec_acc - 0.05, 1, hmg_vec_rfac - 0.05) },
            { TargetType.Vehicle, new DamageProfile(37, hmg_vec_acc - 0.05, 1, hmg_vec_rfac - 0.05) },
            { TargetType.Armor, new DamageProfile(37, hmg_vec_acc - 0.05, 1, hmg_vec_rfac - 0.05) },
        },
        penetration = 8,
        shootProfile = mg34ShootProfile
    };
    public static Weapon MG42_Pintle = new Weapon
    {
        name = "MG42",
        range = Weapon.RealRange(1000),
        //shots = 46,
        //accuracy = hmg_acc - 0.05,
        //rfac = hmg_rfac - 0.05,
        //damage = 1,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(44, hmg_vec_acc - 0.05, 1, hmg_vec_rfac - 0.05) },
            { TargetType.Gun, new DamageProfile(44, hmg_vec_acc - 0.05, 1, hmg_vec_rfac - 0.05) },
            { TargetType.Vehicle, new DamageProfile(44, hmg_vec_acc - 0.05, 1, hmg_vec_rfac - 0.05) },
            { TargetType.Armor, new DamageProfile(44, hmg_vec_acc - 0.05, 1, hmg_vec_rfac - 0.05) },
        },
        penetration = 8,
        shootProfile = mg42ShootProfile
    };
    
    public static double vhmg_acc = 0.1;
    public static double vhmg_rfac = 1.0;
    public static double vhmg_vs_plane_acc = vhmg_acc;
    public static double vhmg_vs_plane_rfac = vhmg_rfac;
    
    public static Weapon M2 = new Weapon
    {
        name = "M2 .50 Cal",
        range = Weapon.RealRange(1200),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(30, vhmg_acc, 1, vhmg_rfac) },
            { TargetType.Gun, new DamageProfile(30, vhmg_acc, 1, vhmg_rfac) },
            { TargetType.Vehicle, new DamageProfile(30, vhmg_acc, 1, vhmg_rfac) },
            { TargetType.Armor, new DamageProfile(30, vhmg_acc, 1, vhmg_rfac) },
        },
        penetration = 22,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 30, 600, 0.0, 1, true, bullet_s, 20)
    };
    public static Weapon M2_Rear = new Weapon
    {
        name = "M2 .50 Rear-facing",
        range = Weapon.RealRange(1200),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(30, vhmg_acc, 1, vhmg_rfac) },
            { TargetType.Gun, new DamageProfile(30, vhmg_acc, 1, vhmg_rfac) },
            { TargetType.Vehicle, new DamageProfile(30, vhmg_acc, 1, vhmg_rfac) },
            { TargetType.Armor, new DamageProfile(30, vhmg_acc, 1, vhmg_rfac) },
        },
        penetration = 22,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 30, 600, 0.0, 1, true, bullet_s, 20)
    };

    public static double cannon_acc = 0.8;
    public static double cannon_rfac = 2.0;
    

    public static Weapon _2Pdr = new Weapon
    {
        name = "2 Pounder",
        range = Weapon.RealRange(1500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(6, cannon_acc, 1, cannon_rfac - 0.15) },
            { TargetType.Gun, new DamageProfile(6, cannon_acc, 1, cannon_rfac - 0.15) },
            { TargetType.Vehicle, new DamageProfile(6, cannon_acc, 1, cannon_rfac - 0.15) },
            { TargetType.Armor, new DamageProfile(6, cannon_acc, 1, cannon_rfac - 0.15) },
        },
        penetration = 60,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 6, 20, 0.2, 1, true, shell, 20)
    };
    public static Weapon _2Pdr_Vec = new Weapon
    {
        name = "2 Pounder",
        range = Weapon.RealRange(1500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(6, cannon_acc, 1, cannon_rfac - 0.15) },
            { TargetType.Gun, new DamageProfile(6, cannon_acc, 1, cannon_rfac - 0.15) },
            { TargetType.Vehicle, new DamageProfile(6, cannon_acc, 1, cannon_rfac - 0.15) },
            { TargetType.Armor, new DamageProfile(6, cannon_acc, 1, cannon_rfac - 0.15) },
        },
        penetration = 60,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 6, 17, 0.2, 1, true, shell, 20)
    };
    public static Weapon _6Pdr = new Weapon
    {
        name = "6 Pounder",
        range = Weapon.RealRange(1900),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(5, cannon_acc, 1, cannon_rfac - 0.05) },
            { TargetType.Gun, new DamageProfile(5, cannon_acc, 1, cannon_rfac - 0.05) },
            { TargetType.Vehicle, new DamageProfile(5, cannon_acc, 1, cannon_rfac - 0.05) },
            { TargetType.Armor, new DamageProfile(5, cannon_acc, 1, cannon_rfac - 0.05) },
        },
        penetration = 80,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 5, 15, 0.2, 1, true, shell, 20)
    };
    public static Weapon _6Pdr_Vec = new Weapon
    {
        name = "6 Pounder",
        range = Weapon.RealRange(1900),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(5, cannon_acc, 1, cannon_rfac - 0.05) },
            { TargetType.Gun, new DamageProfile(5, cannon_acc, 1, cannon_rfac - 0.05) },
            { TargetType.Vehicle, new DamageProfile(5, cannon_acc, 1, cannon_rfac - 0.05) },
            { TargetType.Armor, new DamageProfile(5, cannon_acc, 1, cannon_rfac - 0.05) },
        },
        penetration = 80,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 5, 13, 0.2, 1, true, shell, 20)
    };
    public static Weapon _17Pdr = new Weapon
    {
        name = "17 Pounder",
        range = Weapon.RealRange(2300),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, cannon_acc, 1, cannon_rfac + 0.05) },
            { TargetType.Gun, new DamageProfile(3, cannon_acc, 1, cannon_rfac + 0.05) },
            { TargetType.Vehicle, new DamageProfile(3, cannon_acc, 1, cannon_rfac + 0.05) },
            { TargetType.Armor, new DamageProfile(3, cannon_acc, 1, cannon_rfac + 0.05) },
        },
        penetration = 163,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 3, 10, 0.2, 1, true, shell, 20)
    };
    public static Weapon _17Pdr_Challenger = new Weapon
    {
        name = "17 Pounder",
        range = Weapon.RealRange(2300),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, cannon_acc, 1, cannon_rfac + 0.05) },
            { TargetType.Gun, new DamageProfile(3, cannon_acc, 1, cannon_rfac + 0.05) },
            { TargetType.Vehicle, new DamageProfile(3, cannon_acc, 1, cannon_rfac + 0.05) },
            { TargetType.Armor, new DamageProfile(3, cannon_acc, 1, cannon_rfac + 0.05) },
        },
        penetration = 163,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 3, 10, 0.2, 1, true, shell, 20)
    };
    public static Weapon _17Pdr_M10 = new Weapon
    {
        name = "17 Pounder",
        range = Weapon.RealRange(2300),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.05) },
            { TargetType.Gun, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.05) },
            { TargetType.Vehicle, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.05) },
            { TargetType.Armor, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.05) }
        },
        penetration = 163,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 2, 10, 0.2, 1, true, shell, 20)
    };
    public static Weapon _17Pdr_Sherman = new Weapon
    {
        name = "17 Pounder",
        range = Weapon.RealRange(2300),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.05) },
            { TargetType.Gun, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.05) },
            { TargetType.Vehicle, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.05) },
            { TargetType.Armor, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.05) },
        },
        penetration = 163,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 2, 10, 0.2, 1, true, shell, 20)
    };
    public static Weapon _M7_3In = new Weapon
    {
        name = "M7 3Inch Gun",
        range = Weapon.RealRange(2200),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(4, cannon_acc, 1, cannon_rfac - 0.05) },
            { TargetType.Gun, new DamageProfile(4, cannon_acc, 1, cannon_rfac - 0.05) },
            { TargetType.Vehicle, new DamageProfile(4, cannon_acc, 1, cannon_rfac - 0.05) },
            { TargetType.Armor, new DamageProfile(4, cannon_acc, 1, cannon_rfac - 0.05) },
        },
        penetration = 110,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 4, 20, 0.2, 1, true, shell, 20)
    };
    
    public static double med_cannon_acc = 0.8;
    public static double med_cannon_rfac = 1.7;

    public static Weapon QF_75mm = new Weapon
    {
        name = "QF 75mm",
        range = Weapon.RealRange(1900),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(4, cannon_acc, 1, cannon_rfac - 0.05) },
            { TargetType.Gun, new DamageProfile(4, cannon_acc, 1, cannon_rfac - 0.05) },
            { TargetType.Vehicle, new DamageProfile(4, cannon_acc, 1, cannon_rfac - 0.05) },
            { TargetType.Armor, new DamageProfile(4, cannon_acc, 1, cannon_rfac - 0.05) },
        },
        penetration = 90,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 4, 20, 0.2, 1, true, shell, 20)
    };

    public static double frag_cannon_acc = 0.6;
    public static double frag_cannon_rfac = 1.3;

    public static Weapon QF_95mm = new Weapon
    {
        name = "QF 95mm",
        range = Weapon.RealRange(1900),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, frag_cannon_acc, 1, frag_cannon_rfac) },
            { TargetType.Gun, new DamageProfile(3, frag_cannon_acc, 1, frag_cannon_rfac) },
            { TargetType.Vehicle, new DamageProfile(3, frag_cannon_acc, 1, frag_cannon_rfac) },
            { TargetType.Armor, new DamageProfile(3, frag_cannon_acc, 1, frag_cannon_rfac) },
        },
        penetration = 50,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 3, 18, 0.2, 1, true, shell, 20)
    };
    
    public static Weapon KwK30 = new Weapon {
        name = "KwK 30 20mm",
        range = Weapon.RealRange(1800),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(20, cannon_acc, 1, cannon_rfac) },
            { TargetType.Gun, new DamageProfile(20, cannon_acc, 1, cannon_rfac) },
            { TargetType.Vehicle, new DamageProfile(20, cannon_acc, 1, cannon_rfac) },
            { TargetType.Armor, new DamageProfile(20, cannon_acc, 1, cannon_rfac) },
        },
        penetration = 20,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 20, 280, 3, 7, 0.4, 2.0, 0.2, 1, true, shell, 10)
    };
    
    public static Weapon KwK38 = new Weapon {
        name = "KwK 38 20mm",
        range = Weapon.RealRange(1800),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(30, cannon_acc, 1, cannon_rfac) },
            { TargetType.Gun, new DamageProfile(30, cannon_acc, 1, cannon_rfac) },
            { TargetType.Vehicle, new DamageProfile(30, cannon_acc, 1, cannon_rfac) },
            { TargetType.Armor, new DamageProfile(30, cannon_acc, 1, cannon_rfac) },
        },
        penetration = 20,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 30, 450, 4, 9, 0.4, 2.0, 0.2, 1, true, shell, 10)
    };

    public static Weapon Pak35 = new Weapon {
        name = "PAK 35 37mm",
        range = Weapon.RealRange(1500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(4, cannon_acc, 1, cannon_rfac - 0.15) },
            { TargetType.Gun, new DamageProfile(4, cannon_acc, 1, cannon_rfac - 0.15) },
            { TargetType.Vehicle, new DamageProfile(4, cannon_acc, 1, cannon_rfac - 0.15) },
            { TargetType.Armor, new DamageProfile(4, cannon_acc, 1, cannon_rfac - 0.15) },
        },
        penetration = 60,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 4, 450, 0.2, 1, true, shell, 20)
    };

    public static Weapon Pak38 = new Weapon {
        name = "PAK 38 50mm",
        range = Weapon.RealRange(1800),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, cannon_acc, 1, cannon_rfac - 0.10) },
            { TargetType.Gun, new DamageProfile(3, cannon_acc, 1, cannon_rfac - 0.10) },
            { TargetType.Vehicle, new DamageProfile(3, cannon_acc, 1, cannon_rfac - 0.10) },
            { TargetType.Armor, new DamageProfile(3, cannon_acc, 1, cannon_rfac - 0.10) },
        },
        penetration = 70,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 3, 15, 0.2, 1, true, shell, 20)
    };
    
    public static Weapon KwK37 = new Weapon {
        name = "KwK 37 75mm/24",
        range = Weapon.RealRange(1500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(4, cannon_acc, 1, cannon_rfac) },
            { TargetType.Gun, new DamageProfile(4, cannon_acc, 1, cannon_rfac) },
            { TargetType.Vehicle, new DamageProfile(4, cannon_acc, 1, cannon_rfac) },
            { TargetType.Armor, new DamageProfile(4, cannon_acc, 1, cannon_rfac) },
        },
        penetration = 65,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 4, 16, 0.2, 1, true, shell, 10)
    };
    public static Weapon Stuk37 = new Weapon {
        name = "StuK 37 75mm/24",
        range = Weapon.RealRange(1500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(4, cannon_acc, 1, cannon_rfac) },
            { TargetType.Gun, new DamageProfile(4, cannon_acc, 1, cannon_rfac) },
            { TargetType.Vehicle, new DamageProfile(4, cannon_acc, 1, cannon_rfac) },
            { TargetType.Armor, new DamageProfile(4, cannon_acc, 1, cannon_rfac) },
        },
        penetration = 65,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 4, 16, 0.2, 1, true, shell, 10)
    };

    public static Weapon Pak40 = new Weapon {
        name = "PAK 40 75mm",
        range = Weapon.RealRange(2200),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Gun, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Vehicle, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Armor, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
        },
        penetration = 110,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 3, 12, 0.2, 1, true, shell, 20)
    };
    public static Weapon Pak39 = new Weapon {
        name = "PAK 39 75mm",
        range = Weapon.RealRange(2200),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Gun, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Vehicle, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Armor, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
        },
        penetration = 110,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 3, 12, 0.2, 1, true, shell, 20)
    };
    public static Weapon Stuk40 = new Weapon {
        name = "StuK 40 75mm",
        range = Weapon.RealRange(2200),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Gun, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Vehicle, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Armor, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
        },
        penetration = 110,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 3, 12, 0.2, 1, true, shell, 20)
    };
    public static Weapon Kwk40 = new Weapon
    {
        name = "KwK40 75mm",
        range = Weapon.RealRange(2200),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, cannon_acc, 1, cannon_rfac ) },
            { TargetType.Gun, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Vehicle, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Armor, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
        },
        penetration = 110,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 4, 20, 0.2, 1, true, shell, 20)
    };

    public static Weapon Pak42 = new Weapon
    {
        name = "PAK42 75mm",
        range = Weapon.RealRange(2300),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Gun, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Vehicle, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Armor, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
        },
        penetration = 140,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 3, 20, 0.2, 1, true, shell, 20)
    };
    public static Weapon Kwk42 = new Weapon
    {
        name = "KwK42 75mm",
        range = Weapon.RealRange(2300),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Gun, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Vehicle, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Armor, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
        },
        penetration = 140,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 3, 20, 0.2, 1, true, shell, 20)
    };

    public static Weapon Kwk36 = new Weapon
    {
        name = "KwK36 88mm",
        range = Weapon.RealRange(2300),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Gun, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Vehicle, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
            { TargetType.Armor, new DamageProfile(3, cannon_acc, 1, cannon_rfac) },
        },
        penetration = 70,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 3, 15, 0.2, 1, true, shell, 20)
    };

    public static Weapon Pak43 = new Weapon {
        name = "PAK 43 88m",
        range = Weapon.RealRange(2500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.1) },
            { TargetType.Gun, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.1) },
            { TargetType.Vehicle, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.1) },
            { TargetType.Armor, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.1) },
        },
        penetration = 180,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 2, 8, 0.2, 1, true, shell, 20)
    };
    public static Weapon Pak43_Vec = new Weapon {
        name = "PAK 43 88m",
        range = Weapon.RealRange(2500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.1) },
            { TargetType.Gun, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.1) },
            { TargetType.Vehicle, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.1) },
            { TargetType.Armor, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.1) },
        },
        penetration = 180,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 2, 8, 0.2, 1, true, shell, 20)
    };
    public static Weapon Kwk43 = new Weapon {
        name = "KwK 43 88m",
        range = Weapon.RealRange(2500),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.1) },
            { TargetType.Gun, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.1) },
            { TargetType.Vehicle, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.1) },
            { TargetType.Armor, new DamageProfile(2, cannon_acc, 1, cannon_rfac + 0.1) },
        },
        penetration = 180,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 2, 8, 0.2, 1, true, shell, 20)
    };
    
    public static Weapon Stuk105 = new Weapon {
        name = "StuK 105mm",
        range = Weapon.RealRange(2200),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(2, frag_cannon_acc, 1, frag_cannon_rfac) },
            { TargetType.Gun, new DamageProfile(2, frag_cannon_acc, 1, frag_cannon_rfac) },
            { TargetType.Vehicle, new DamageProfile(2, frag_cannon_acc, 1, frag_cannon_rfac) },
            { TargetType.Armor, new DamageProfile(2, frag_cannon_acc, 1, frag_cannon_rfac) },
        },
        penetration = 110,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 2, 8, 0.2, 1, true, shell, 13)
    };
    
    public static double aa_cannon_acc = 0.5;
    public static double aa_cannon_rfac = 1.4;
    public static double aa_cannon_vs_plane_acc = 0.1;
    public static double aa_cannon_vs_plane_rfac = 0.7;
    
    public static Weapon _3_7In = new Weapon
    {
        name = "3.7 Inch AA",
        range = Weapon.RealRange(2200),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, aa_cannon_acc, 1, 4, aa_cannon_rfac) }, // ??
            { TargetType.Gun, new DamageProfile(3, aa_cannon_acc, 1, 4, aa_cannon_rfac) }, // ??
            { TargetType.Vehicle, new DamageProfile(3, aa_cannon_acc, 1, 4, aa_cannon_rfac) }, // ??
            { TargetType.Armor, new DamageProfile(3, aa_cannon_acc, 1, 5, aa_cannon_rfac) },
            { TargetType.Plane, new DamageProfile(3, aa_cannon_acc, 1, 5, aa_cannon_rfac) },
        },
        penetration = 80,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 3, 6, 0.1, 1, true, shell, 20) // velo?
    };

    public static Weapon Flak36 = new Weapon
    {
        name = "FlAK 36 88mm AA",
        range = Weapon.RealRange(2200),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, aa_cannon_acc, 1, 4, aa_cannon_rfac) }, // ??
            { TargetType.Gun, new DamageProfile(3, aa_cannon_acc, 1, 4, aa_cannon_rfac) }, // ??
            { TargetType.Vehicle, new DamageProfile(3, aa_cannon_acc, 1, 4, aa_cannon_rfac) }, // ??
            { TargetType.Armor, new DamageProfile(3, aa_cannon_acc, 1, 5, aa_cannon_rfac) },
            { TargetType.Plane, new DamageProfile(3, aa_cannon_acc, 1, 5, aa_cannon_rfac) },
        },
        penetration = 80,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 3, 6, 0.1, 1, true, shell, 20) // velo?
    };
    public static Weapon Flak41 = new Weapon
    {
        name = "FlAK 41 88mm AA",
        range = Weapon.RealRange(2400),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(3, aa_cannon_acc, 1, 4, aa_cannon_rfac) }, // ??
            { TargetType.Gun, new DamageProfile(3, aa_cannon_acc, 1, 4, aa_cannon_rfac) }, // ??
            { TargetType.Vehicle, new DamageProfile(3, aa_cannon_acc, 1, 4, aa_cannon_rfac) }, // ??
            { TargetType.Armor, new DamageProfile(3, aa_cannon_acc, 1, 5, aa_cannon_rfac) },
            { TargetType.Plane, new DamageProfile(3, aa_cannon_acc, 1, 5, aa_cannon_rfac) },
        },
        penetration = 100,
        shootProfile = new ShootProfile(SoundLib.abramsShot, 3, 6, 0.1, 1, true, shell, 22) // velo?
    };

    public static double rg_acc = 0.4;
    public static double rg_rfac = 0.6;
    
    public static Weapon UK_RG = new Weapon
    {
        name = "Rifle Grenade",
        range = Weapon.RealRange(300),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(2, rg_acc, 1, rg_rfac) },
            { TargetType.Gun, new DamageProfile(2, rg_acc, 1, rg_rfac) },
            { TargetType.Vehicle, new DamageProfile(2, rg_acc, 1, rg_rfac) },
            { TargetType.Armor, new DamageProfile(2, rg_acc, 1, rg_rfac) },
        },
        penetration = 50,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 2, 5, 0.15, 1, true, shell, 8)
    };

    public static Weapon Sb = new Weapon
    {
        name = "Rifle Grenade",
        range = Weapon.RealRange(300),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(2, rg_acc, 1, rg_rfac) },
            { TargetType.Gun, new DamageProfile(2, rg_acc, 1, rg_rfac) },
            { TargetType.Vehicle, new DamageProfile(2, rg_acc, 1, rg_rfac) },
            { TargetType.Armor, new DamageProfile(2, rg_acc, 1, rg_rfac) },
        },
        penetration = 50,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 2, 5, 0.15, 1, true, shell, 8)
    };
    
    public static double rocket_acc = 0.55;
    public static double rocket_rfac = 0.5;
    
    public static Weapon PIAT = new Weapon
    {
        name = "PIAT",
        soldiersToFire = 2,
        range = Weapon.RealRange(200),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(2, rocket_acc, 1, 2, rocket_rfac) },
            { TargetType.Gun, new DamageProfile(2, rocket_acc, 1, 2, rocket_rfac) },
            { TargetType.Vehicle, new DamageProfile(2, rocket_acc, 1, 2, rocket_rfac) },
            { TargetType.Armor, new DamageProfile(2, rocket_acc, 1, 5, rocket_rfac) },
        },
        penetration = 100,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 2, 6, 0.15, 1, true, shell, 8)
    };

    public static Weapon PzF30 = new Weapon
    {
        name = "PanzerFaust 30",
        range = Weapon.RealRange(30),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, rocket_acc, 1, 2, rocket_rfac - 0.3) },
            { TargetType.Gun, new DamageProfile(2, rocket_acc, 1, 2, rocket_rfac - 0.3) },
            { TargetType.Vehicle, new DamageProfile(2, rocket_acc, 1, 2, rocket_rfac - 0.3) },
            { TargetType.Armor, new DamageProfile(1, rocket_acc, 1, 5, rocket_rfac - 0.3) },
        },
        penetration = 190,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 1, 5, 0.15, 1, true, shell, 3)
    };
    public static Weapon PzF60 = new Weapon
    {
        name = "PanzerFaust 60",
        range = Weapon.RealRange(60),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, rocket_acc, 1, 2, rocket_rfac - 0.2) },
            { TargetType.Gun, new DamageProfile(2, rocket_acc, 1, 2, rocket_rfac - 0.2) },
            { TargetType.Vehicle, new DamageProfile(2, rocket_acc, 1, 2, rocket_rfac - 0.2) },
            { TargetType.Armor, new DamageProfile(1, rocket_acc, 1, 5, rocket_rfac - 0.2) },
        },
        penetration = 190,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 1, 5, 0.15, 1, true, shell, 4)
    };
    public static Weapon PzF100 = new Weapon
    {
        name = "PanzerFaust 60",
        range = Weapon.RealRange(100),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, rocket_acc, 1, 2, rocket_rfac - 0.1) },
            { TargetType.Gun, new DamageProfile(2, rocket_acc, 1, 2, rocket_rfac - 0.1) },
            { TargetType.Vehicle, new DamageProfile(2, rocket_acc, 1, 2, rocket_rfac - 0.1) },
            { TargetType.Armor, new DamageProfile(1, rocket_acc, 1, 5, rocket_rfac - 0.1) },
        },
        penetration = 190,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 1, 5, 0.15, 1, true, shell, 5)
    };
    public static Weapon PzSck = new Weapon
    {
        name = "PanzerScreck",
        range = Weapon.RealRange(250),
        soldiersToFire = 2,
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(2, rocket_acc, 1, 2, rocket_rfac - 0.05) },
            { TargetType.Gun, new DamageProfile(2, rocket_acc, 1, 2, rocket_rfac - 0.05) },
            { TargetType.Vehicle, new DamageProfile(2, rocket_acc, 1, 2, rocket_rfac - 0.05) },
            { TargetType.Armor, new DamageProfile(2, rocket_acc, 1, 6, rocket_rfac - 0.05) },
        },
        penetration = 190,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 2, 6, 0.15, 1, true, shell, 8)
    };
    
    public static double mortar_acc = 0.3;
    public static double mortar_rfac = 9999.0;

    public static Weapon _2In_Mortar = new Weapon {
        name = "2 Inch Mortar",
        displayRange = Weapon.RealRange(500),

        direct = false,
        indirect = true,
        indirectWeapon =  new Weapon
        {
            name = "2 Inch Mortar Ind.",
            range = Weapon.RealRange(500),
            
            aimTime = 1,
            flightTime = 1,

            setupTime = 3,
            packupTime = 2,

            artyShellsPerSalvo = 5,
            artyDelayPerShell = 3.333,
            impactDelay = 1,

            damageProfiles = new Dictionary<TargetType, DamageProfile> {
                { TargetType.Infantry, new DamageProfile(5, mortar_acc, 1, 2, mortar_rfac) },
                { TargetType.Gun, new DamageProfile(5, mortar_acc, 1, 2, mortar_rfac) },
                { TargetType.Vehicle, new DamageProfile(5, mortar_acc, 1, 2, mortar_rfac) },
                { TargetType.Armor, new DamageProfile(5, mortar_acc, 1, 2, mortar_rfac) },
            },
            penetration = 20,
            shootProfile = new ShootProfile(SoundLib.m249Shot, 5, 18, 0.2, 1, true, shell, 8)
        }
    };
    public static Weapon _3In_Mortar = new Weapon {
        name = "3 Inch Mortar",
        displayRange = Weapon.RealRange(3000),

        direct = false,
        indirect = true,
        indirectWeapon =  new Weapon
        {
            name = "3 Inch Mortar Ind.",
            range = Weapon.RealRange(3000),

            aimTime = 1,
            flightTime = 1,

            setupTime = 3,
            packupTime = 2,

            artyShellsPerSalvo = 4,
            artyDelayPerShell = 4.286,
            impactDelay = 1,

            damageProfiles = new Dictionary<TargetType, DamageProfile> {
                { TargetType.Infantry, new DamageProfile(4, mortar_acc, 1, 4, mortar_rfac) },
                { TargetType.Gun, new DamageProfile(4, mortar_acc, 1, 4, mortar_rfac) },
                { TargetType.Vehicle, new DamageProfile(4, mortar_acc, 1, 4, mortar_rfac) },
                { TargetType.Armor, new DamageProfile(4, mortar_acc, 1, 4, mortar_rfac) },
            },
            penetration = 35,
            shootProfile = new ShootProfile(SoundLib.m249Shot, 4, 14, 0.2, 1, true, shell, 11)
        }
    };
    public static Weapon _4_2In_Mortar = new Weapon {
        name = "4.2 Inch Mortar",
        displayRange = Weapon.RealRange(6000),

        direct = false,
        indirect = true,
        indirectWeapon =  new Weapon
        {
            name = "4.2 Inch Mortar Ind.",
            range = Weapon.RealRange(6000),

            aimTime = 1,
            flightTime = 1,

            setupTime = 3,
            packupTime = 2,

            artyShellsPerSalvo = 3,
            artyDelayPerShell = 6.0,
            impactDelay = 1,

            damageProfiles = new Dictionary<TargetType, DamageProfile> {
                { TargetType.Infantry, new DamageProfile(3, mortar_acc, 1, 6, mortar_rfac) },
                { TargetType.Gun, new DamageProfile(3, mortar_acc, 1, 6, mortar_rfac) },
                { TargetType.Vehicle, new DamageProfile(3, mortar_acc, 1, 6, mortar_rfac) },
                { TargetType.Armor, new DamageProfile(3, mortar_acc, 1, 6, mortar_rfac) },
            },
            penetration = 50,
            shootProfile = new ShootProfile(SoundLib.m249Shot, 3, 10, 0.2, 1, true, shell, 13)
        }
    };

    public static Weapon Grw34 = new Weapon {
        name = "Grw 34 8cm Mortar",
        displayRange = Weapon.RealRange(3000),

        direct = false,
        indirect = true,
        indirectWeapon =  new Weapon
        {
            name = "Grw 34 8cm Mortar Ind.",
            range = Weapon.RealRange(3000),

            aimTime = 1,
            flightTime = 1,

            setupTime = 3,
            packupTime = 2,

            artyShellsPerSalvo = 4,
            artyDelayPerShell = 4.286,
            impactDelay = 1,

            damageProfiles = new Dictionary<TargetType, DamageProfile> {
                { TargetType.Infantry, new DamageProfile(4, mortar_acc, 1, 4, mortar_rfac) },
                { TargetType.Gun, new DamageProfile(4, mortar_acc, 1, 4, mortar_rfac) },
                { TargetType.Vehicle, new DamageProfile(4, mortar_acc, 1, 4, mortar_rfac) },
                { TargetType.Armor, new DamageProfile(4, mortar_acc, 1, 4, mortar_rfac) },
            },
            penetration = 35,
            shootProfile = new ShootProfile(SoundLib.m249Shot, 4, 14, 0.2, 1, true, shell, 11)
        }
    };
    public static Weapon Grw34_Halftrack = new Weapon {
        name = "Grw 34 8cm Mortar",
        displayRange = Weapon.RealRange(3000),

        direct = false,
        indirect = true,
        indirectWeapon =  new Weapon
        {
            name = "Grw 34 8cm Mortar Ind.",
            range = Weapon.RealRange(3000),

            aimTime = 1,
            flightTime = 1,

            setupTime = 3,
            packupTime = 2,

            artyShellsPerSalvo = 4,
            artyDelayPerShell = 4.286,
            impactDelay = 1,

            damageProfiles = new Dictionary<TargetType, DamageProfile> {
                { TargetType.Infantry, new DamageProfile(4, mortar_acc, 1, 4, mortar_rfac) },
                { TargetType.Gun, new DamageProfile(4, mortar_acc, 1, 4, mortar_rfac) },
                { TargetType.Vehicle, new DamageProfile(4, mortar_acc, 1, 4, mortar_rfac) },
                { TargetType.Armor, new DamageProfile(4, mortar_acc, 1, 4, mortar_rfac) },
            },
            penetration = 35,
            shootProfile = new ShootProfile(SoundLib.m249Shot, 4, 14, 0.2, 1, true, shell, 11)
        }
    };
    public static Weapon Grw42 = new Weapon {
        name = "Grw 42 12cm Mortar",
        displayRange = Weapon.RealRange(6000),

        direct = false,
        indirect = true,
        indirectWeapon =  new Weapon
        {
            name = "Grw 42 12cm Mortar Ind.",
            range = Weapon.RealRange(6000),

            aimTime = 1,
            flightTime = 1,

            setupTime = 3,
            packupTime = 2,

            artyShellsPerSalvo = 2,
            artyDelayPerShell = 7.5,
            impactDelay = 1,

            damageProfiles = new Dictionary<TargetType, DamageProfile> {
                { TargetType.Infantry, new DamageProfile(2, mortar_acc, 1, 7, mortar_rfac) },
                { TargetType.Gun, new DamageProfile(2, mortar_acc, 1, 7, mortar_rfac) },
                { TargetType.Vehicle, new DamageProfile(2, mortar_acc, 1, 7, mortar_rfac) },
                { TargetType.Armor, new DamageProfile(2, mortar_acc, 1, 7, mortar_rfac) },
            },
            penetration = 60,
            shootProfile = new ShootProfile(SoundLib.m249Shot, 2, 8, 0.2, 1, true, shell, 13)
        }
    };
    
    
    public static double how_acc = 0.3;
    public static double how_rfac = 9999.0;

    public static Weapon _25Pdr = new Weapon {
        name = "25 Pounder Howitzer",
        displayRange = Weapon.RealRange(12000),

        direct = false,
        indirect = true,
        indirectWeapon =  new Weapon
        {
            name = "25 Pounder Howitzer Ind.",
            range = Weapon.RealRange(12000),

            aimTime = 1,
            flightTime = 1,

            setupTime = 3,
            packupTime = 2,

            artyShellsPerSalvo = 3,
            artyDelayPerShell = 8.571,
            impactDelay = 1,

            damageProfiles = new Dictionary<TargetType, DamageProfile> {
                { TargetType.Infantry, new DamageProfile(3, how_acc, 1, 4, how_rfac) },
                { TargetType.Gun, new DamageProfile(3, how_acc, 1, 4, how_rfac) },
                { TargetType.Vehicle, new DamageProfile(3, how_acc, 1, 4, how_rfac) },
                { TargetType.Armor, new DamageProfile(3, how_acc, 1, 4, how_rfac) },
            },
            penetration = 40,
            shootProfile = new ShootProfile(SoundLib.m249Shot, 3, 7, 0.2, 1, true, shell, 17)
        }
    };

    public static Weapon LeFH_18 = new Weapon {
        name = "LeFH 18 Howitzer",
        displayRange = Weapon.RealRange(11000),

        direct = false,
        indirect = true,
        indirectWeapon =  new Weapon
        {
            name = "LeFH 18 Howitzer Ind.",
            range = Weapon.RealRange(11000),

            aimTime = 1,
            flightTime = 1,

            setupTime = 3,
            packupTime = 2,

            artyShellsPerSalvo = 3,
            artyDelayPerShell = 8.571,
            impactDelay = 1,

            damageProfiles = new Dictionary<TargetType, DamageProfile> {
                { TargetType.Infantry, new DamageProfile(3, how_acc, 1, 5, how_rfac) },
                { TargetType.Gun, new DamageProfile(3, how_acc, 1, 5, how_rfac) },
                { TargetType.Vehicle, new DamageProfile(3, how_acc, 1, 5, how_rfac) },
                { TargetType.Armor, new DamageProfile(3, how_acc, 1, 5, how_rfac) },
            },
            penetration = 45,
            shootProfile = new ShootProfile(SoundLib.m249Shot, 3, 7, 0.2, 1, true, shell, 16)
        }
    };


    public static double Infantry_Hitability = 0.5;

    public static double CannonSmall_Hitability = 0.7;
    public static double Cannon_Hitability = 0.75;
    public static double CannonLarge_Hitability = 0.8;
    public static double CannonVeryLarge_Hitability = 0.85;

    public static SquadDef UK_Rifle = new SquadDef
    {
        name = "Rifle Section",
        shortName = "Rifle",
        soldiers = 10,
        //moves = 1,
        //movementType = Infantry
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Sten, 10),
            new UnitWeapon(1, Bren, 20),
            new UnitWeapon(8, SMLE, 10),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef UK_Rifle_Plt_HQ = new SquadDef
    {
        name = "Rifle Plt HQ",
        shortName = "Rif HQ",
        soldiers = 6,
        //moves = 1,
        //movementType = Infantry
        weapons = new UnitWeapon[] {
            new UnitWeapon(2, Sten, 10),
            new UnitWeapon(4, SMLE, 20),
            new UnitWeapon(1, PIAT, 4),
            new UnitWeapon(1, _2In_Mortar, 10),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    
    public static SquadDef UK_Motor_Rifle = new SquadDef
    {
        name = "Motor Rifle Sec",
        shortName = "Mot Rif",
        soldiers = 7,
        //moves = 1,
        //movementType = Infantry
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Sten, 10),
            new UnitWeapon(1, Bren, 20),
            new UnitWeapon(5, SMLE, 10),
            new UnitWeapon(1, PIAT, 4),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef UK_Motor_Rifle_Plt_HQ = new SquadDef
    {
        name = "Rifle Plt HQ",
        shortName = "Mot Rif HQ",
        soldiers = 5,
        //moves = 1,
        //movementType = Infantry
        weapons = new UnitWeapon[] {
            new UnitWeapon(2, Sten, 10),
            new UnitWeapon(4, SMLE, 20),
            new UnitWeapon(1, _2In_Mortar, 10),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef PIAT_Team = new SquadDef
    {
        name = "PIAT",
        shortName = "PIAT",
        soldiers = 2,
        //moves = 1,
        //movementType = Infantry
        weapons = new UnitWeapon[] {
            new UnitWeapon(2, SMLE, 10),
            new UnitWeapon(1, PIAT, 3),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef Vickers_Team = new SquadDef
    {
        name = "Vickers",
        shortName = "Vickers",
        soldiers = 6,
        //moves = 1,
        //movementType = CSW
        movementData = Pathfinder.WW2_CswMedium_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Vickers, 26),
            new UnitWeapon(5, SMLE, 8),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef _2Pdr_Gun = new SquadDef {
        name = "2 Pounder",
        shortName = "2Pdr",
        soldiers = 3,
        //moves = 1,
        //movementType = CannonFast
        movementData = Pathfinder.WW2_GunVeryLight_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _2Pdr, 10),
            new UnitWeapon(1, Sten, 10),
            new UnitWeapon(2, SMLE, 20),
        },
        targetType = TargetType.Gun,
        hitability = CannonSmall_Hitability
    };

    public static SquadDef _6Pdr_Gun = new SquadDef {
        name = "6 Pounder",
        shortName = "6Pdr",
        unitClass = UnitClass.Gun,
        soldiers = 4,
        //moves = 1,
        //movementType = Cannon
        movementData = Pathfinder.WW2_GunLight_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _6Pdr, 10),
            new UnitWeapon(1, Sten, 10),
            new UnitWeapon(3, SMLE, 20),
        },
        targetType = TargetType.Gun,
        hitability = Cannon_Hitability
    };

    public static SquadDef _17Pdr_Gun = new SquadDef {
        name = "6 Pounder",
        shortName = "6Pdr",
        unitClass = UnitClass.Gun,
        soldiers = 4,
        //moves = 1,
        //movementType = CannonVerySlow
        movementData = Pathfinder.WW2_GunHeavy_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _17Pdr, 10),
            new UnitWeapon(1, Sten, 10),
            new UnitWeapon(4, SMLE, 20),
        },
        targetType = TargetType.Gun,
        hitability = CannonLarge_Hitability
    };

    public static SquadDef _25Pdr_Gun = new SquadDef {
        name = "25 Pounder",
        shortName = "25Pdr",
        unitClass = UnitClass.Gun,
        soldiers = 5,
        //moves = 1,
        //movementType = CannonSlow
        movementData = Pathfinder.WW2_GunMedium_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _25Pdr, 10),
            new UnitWeapon(1, Sten, 10),
            new UnitWeapon(4, SMLE, 20),
        },
        targetType = TargetType.Gun,
        hitability = CannonLarge_Hitability
    };

    public static SquadDef _3_7In_Gun = new SquadDef {
        name = "3.7 Inch AA",
        shortName = "3.7In AA",
        unitClass = UnitClass.Gun,
        soldiers = 6,
        //moves = 1,
        //movementType = CannonVeryVerySlow
        movementData = Pathfinder.WW2_GunVeryVeryHeavy_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _3_7In, 10),
            new UnitWeapon(1, Sten, 10),
            new UnitWeapon(5, SMLE, 20),
        },
        targetType = TargetType.Gun,
        hitability = CannonVeryLarge_Hitability
    };
    
    
    public static SquadDef _3In_Mortar_Team = new SquadDef {
        name = "3In Mortar",
        shortName = "3In Mtr",
        soldiers = 4,
        //moves = 1,
        //movementType = CSW
        movementData = Pathfinder.WW2_CswHeavy_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _3In_Mortar, 10),
            new UnitWeapon(1, Sten, 10),
            new UnitWeapon(3, SMLE, 10),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef _4_2In_Mortar_Team = new SquadDef {
        name = "4.2In Mortar",
        shortName = "4.2In Mtr",
        soldiers = 6,
        //moves = 1,
        //movementType = CSW
        movementData = Pathfinder.WW2_CswVeryHeavy_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _4_2In_Mortar, 10),
            new UnitWeapon(1, Sten, 10),
            new UnitWeapon(5, SMLE, 10),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    
    // Trucks
    public static SquadDef Morris_C8 = new SquadDef
    {
        name = "Morris C8",
        unitClass = UnitClass.Truck,
        soldiers = 2,
        moves = 4,
        //movementType = TankFast
        movementData = Pathfinder.WW2_Truck_MovementData,
        targetType = TargetType.Vehicle,
        armor = 2,
        cargoDef = new CargoDef(5, 1),
    };
    
    public static SquadDef Opel_Blitz_1T = new SquadDef
    {
        name = "Opel Blitz 1T",
        unitClass = UnitClass.Truck,
        soldiers = 2,
        moves = 4,
        //movementType = TankFast
        movementData = Pathfinder.WW2_Truck_MovementData,
        targetType = TargetType.Vehicle,
        armor = 2,
        cargoDef = new CargoDef(11, 1),
    };
    public static SquadDef Opel_Blitz_2T = new SquadDef
    {
        name = "Opel Blitz 2T",
        unitClass = UnitClass.Truck,
        soldiers = 2,
        moves = 4,
        //movementType = TankFast
        movementData = Pathfinder.WW2_Truck_MovementData,
        targetType = TargetType.Vehicle,
        armor = 2,
        cargoDef = new CargoDef(11, 1),
    };
    public static SquadDef Opel_Blitz_2_5T = new SquadDef
    {
        name = "Opel Blitz 2.5T",
        unitClass = UnitClass.Truck,
        soldiers = 2,
        moves = 4,
        //movementType = TankFast
        movementData = Pathfinder.WW2_Truck_MovementData,
        targetType = TargetType.Vehicle,
        armor = 2,
        cargoDef = new CargoDef(11, 1),
    };
    public static SquadDef Opel_Blitz_3_6T = new SquadDef
    {
        name = "Opel Blitz 3.6T",
        unitClass = UnitClass.Truck,
        soldiers = 2,
        moves = 4,
        //movementType = TankFast
        movementData = Pathfinder.WW2_Truck_MovementData,
        targetType = TargetType.Vehicle,
        armor = 2,
        cargoDef = new CargoDef(11, 1),
    };

    // UK Carriers
    public static SquadDef Carrier = new SquadDef
    {
        name = "Universal Carrier",
        unitClass = UnitClass.Carrier,
        soldiers = 2,
        moves = 4,
        //movementType = TankFast
        movementData = Pathfinder.WW2_Carrier_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Bren_Vec, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 10,
        cargoDef = new CargoDef(4, 1),
    };
    public static SquadDef Lloyd_Carrier = new SquadDef
    {
        name = "Lloyd Carrier",
        unitClass = UnitClass.Carrier,
        soldiers = 2,
        moves = 4,
        //movementType = TankFast
        movementData = Pathfinder.WW2_Carrier_MovementData,
        targetType = TargetType.Armor,
        armor = 10,
        cargoDef = new CargoDef(4, 1),
    };
    
    // UK Halftracks
    public static SquadDef M5_HT = new SquadDef
    {
        name = "M5 Halftrack",
        unitClass = UnitClass.Halftrack,
        soldiers = 1,
        moves = 4,
        //movementType = TankFast
        movementData = Pathfinder.WW2_Halftrack_MovementData,
        weapons = new UnitWeapon[] {
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 10,
        cargoDef = new CargoDef(10, 1),
    };

    // GE Halftracks
    public static SquadDef RSO = new SquadDef // 250 base, without armored enclosure
    {
        name = "Raupenschlepper Ost",
        unitClass = UnitClass.HalftrackOpen,
        soldiers = 1,
        moves = 4,
        //movementType = TankFast
        movementData = Pathfinder.WW2_Halftrack_MovementData,
        weapons = new UnitWeapon[] {
        },
        targetType = TargetType.Armor,
        armor = 2,
        cargoDef = new CargoDef(8, 1),
    };
    public static SquadDef SdKfz_10 = new SquadDef // 250 base, without armored enclosure
    {
        name = "SdKfz 250 Halftrack",
        unitClass = UnitClass.HalftrackOpen,
        soldiers = 1,
        moves = 4,
        //movementType = TankFast
        movementData = Pathfinder.WW2_Halftrack_MovementData,
        weapons = new UnitWeapon[] {
        },
        targetType = TargetType.Armor,
        armor = 2,
        cargoDef = new CargoDef(8, 1),
    };
    public static SquadDef SdKfz_250 = new SquadDef
    {
        name = "SdKfz 250 Halftrack",
        unitClass = UnitClass.Halftrack,
        soldiers = 1,
        moves = 4,
        //movementType = TankFast
        movementData = Pathfinder.WW2_Halftrack_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, MG42_Pintle, 20)
        },
        targetType = TargetType.Armor,
        armor = 10,
        cargoDef = new CargoDef(6, 1),
    };
    public static SquadDef SdKfz_11 = new SquadDef // 251 base, without armored enclosure
    {
        name = "SdKfz 250 Halftrack",
        unitClass = UnitClass.HalftrackOpen,
        soldiers = 1,
        moves = 4,
        //movementType = TankFast
        movementData = Pathfinder.WW2_Halftrack_MovementData,
        weapons = new UnitWeapon[] {
        },
        targetType = TargetType.Armor,
        armor = 0,
        cargoDef = new CargoDef(12, 1),
    };
    public static SquadDef SdKfz_251 = new SquadDef
    {
        name = "SdKfz 250 Halftrack",
        unitClass = UnitClass.Halftrack,
        soldiers = 1,
        moves = 4,
        //movementType = TankFast
        movementData = Pathfinder.WW2_Halftrack_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, MG42_Pintle, 20)
        },
        targetType = TargetType.Armor,
        armor = 10,
        cargoDef = new CargoDef(10, 1),
    };
    // 2 - Mortar
    public static SquadDef SdKfz_251_2 = new SquadDef
    {
        name = "SdKfz 250 Halftrack",
        unitClass = UnitClass.Halftrack,
        soldiers = 1,
        moves = 4,
        //movementType = TankFast
        movementData = Pathfinder.WW2_Halftrack_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Grw34_Halftrack, 20)
        },
        targetType = TargetType.Armor,
        armor = 10,
        cargoDef = new CargoDef(10, 1),
    };
    // 9 - Stummel
    public static SquadDef SdKfz_251_9 = new SquadDef
    {
        name = "SdKfz 250 Halftrack",
        unitClass = UnitClass.Halftrack,
        soldiers = 1,
        moves = 4,
        //movementType = TankFast
        movementData = Pathfinder.WW2_Halftrack_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Stuk37, 20)
        },
        targetType = TargetType.Armor,
        armor = 10,
        cargoDef = new CargoDef(10, 1),
    };
    // 17 - 2cm Flak
    public static SquadDef SdKfz_251_17 = new SquadDef
    {
        name = "SdKfz 250 Halftrack",
        unitClass = UnitClass.Halftrack,
        soldiers = 1,
        moves = 4,
        //movementType = TankFast
        movementData = Pathfinder.WW2_Halftrack_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, KwK38, 10)
        },
        targetType = TargetType.Armor,
        armor = 10,
        cargoDef = new CargoDef(10, 1),
    };


    // Crom 4 & 5
    public static SquadDef Cromwell = new SquadDef
    {
        name = "Cromwell",
        unitClass = UnitClass.Tank,
        soldiers = 5,
        moves = 4,
        //movementType = TankFast
        movementData = Pathfinder.WW2_TrackedVeryFast_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, QF_75mm, 25),
            new UnitWeapon(1, Besa, 20),
            new UnitWeapon(1, Besa_Hull, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 64,
    };
    
    public static SquadDef Cromwell_6 = new SquadDef
    {
        name = "Cromwell VI CS",
        unitClass = UnitClass.Tank,
        soldiers = 5,
        moves = 4,
        //movementType = Tank
        movementData = Pathfinder.WW2_TrackedVeryFast_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, QF_95mm, 25),
            new UnitWeapon(1, Besa, 20),
            new UnitWeapon(1, Besa_Hull, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 64,
    };

    public static SquadDef Centaur_4 = new SquadDef
    {
        name = "Centaur IV CS",
        unitClass = UnitClass.Tank,
        soldiers = 5,
        moves = 4,
        //movementType = Tank
        movementData = Pathfinder.WW2_TrackedFast_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, QF_95mm, 25),
            new UnitWeapon(1, Besa, 20),
            new UnitWeapon(1, Besa_Hull, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 64,
    };

    public static SquadDef Challenger = new SquadDef
    {
        name = "Challenger",
        unitClass = UnitClass.Tank,
        soldiers = 5,
        moves = 4,
        //movementType = Tank
        movementData = Pathfinder.WW2_TrackedVeryFast_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _17Pdr_Challenger, 25),
            new UnitWeapon(1, Besa, 20),
        },
        targetType = TargetType.Armor,
        armor = 62,
    };

    public static SquadDef Challenger_UpArmor = new SquadDef
    {
        name = "Challenger",
        unitClass = UnitClass.Tank,
        soldiers = 5,
        moves = 4,
        //movementType = Tank
        movementData = Pathfinder.WW2_TrackedVeryFast_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _17Pdr_Challenger, 25),
            new UnitWeapon(1, Besa, 20),
        },
        targetType = TargetType.Armor,
        armor = 87,
    };


    public static SquadDef Sherman = new SquadDef
    {
        name = "Sherman V",
        unitClass = UnitClass.Tank,
        soldiers = 5,
        moves = 3,
        //movementType = Tank
        movementData = Pathfinder.WW2_TrackedMedium_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, QF_75mm, 25),
            new UnitWeapon(1, M1919_Vec, 20),
            new UnitWeapon(1, M1919_Vec_Hull, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 90,
    };
    public static SquadDef Sherman5C = new SquadDef
    {
        name = "Sherman VC Firefly",
        unitClass = UnitClass.Tank,
        soldiers = 4,
        moves = 3,
        //movementType = Tank
        movementData = Pathfinder.WW2_TrackedMedium_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _17Pdr_Sherman, 20),
            new UnitWeapon(1, M1919_Vec, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 90,
    };

    public static SquadDef Churchill = new SquadDef
    {
        name = "Churchill",
        unitClass = UnitClass.Tank,
        soldiers = 5,
        //moves = 2,
        //movementType = TankSlow
        movementData = Pathfinder.WW2_TrackedVeryVerySlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, QF_75mm, 20),
            new UnitWeapon(1, Besa, 20),
            new UnitWeapon(1, Besa_Hull, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 102,
    };
    public static SquadDef Churchill_6Pdr = new SquadDef
    {
        name = "Churchill",
        unitClass = UnitClass.Tank,
        soldiers = 5,
        //moves = 2,
        //movementType = TankSlow
        movementData = Pathfinder.WW2_TrackedVeryVerySlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _6Pdr_Vec, 20),
            new UnitWeapon(1, Besa, 20),
            new UnitWeapon(1, Besa_Hull, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 102,
    };
    public static SquadDef Churchill_5 = new SquadDef
    {
        name = "Churchill V CS",
        unitClass = UnitClass.Tank,
        soldiers = 5,
        //moves = 2,
        //movementType = TankSlow
        movementData = Pathfinder.WW2_TrackedVeryVerySlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, QF_95mm, 20),
            new UnitWeapon(1, Besa, 20),
            new UnitWeapon(1, Besa_Hull, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 102,
    };

    public static SquadDef Churchill_7 = new SquadDef
    {
        name = "Churchill VII",
        unitClass = UnitClass.Tank,
        soldiers = 5,
        //moves = 2,
        //movementType = TankSlow
        movementData = Pathfinder.WW2_TrackedVeryVerySlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, QF_75mm, 20),
            new UnitWeapon(1, Besa, 20),
            new UnitWeapon(1, Besa_Hull, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 152,
    };

    public static SquadDef Churchill_8 = new SquadDef
    {
        name = "Churchill VIII CS",
        unitClass = UnitClass.Tank,
        soldiers = 5,
        //moves = 2,
        //movementType = TankSlow
        movementData = Pathfinder.WW2_TrackedVeryVerySlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, QF_95mm, 20),
            new UnitWeapon(1, Besa, 20),
            new UnitWeapon(1, Besa_Hull, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 152,
    };

    public static SquadDef M10_UK = new SquadDef
    {
        name = "M10",
        unitClass = UnitClass.Tank,
        soldiers = 5,
        moves = 3,
        //movementType = Tank
        movementData = Pathfinder.WW2_TrackedMedium_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _M7_3In, 20),
            new UnitWeapon(1, M2_Rear, 20)
        },
        targetType = TargetType.Armor,
        armor = 80,
    };
    public static SquadDef M10_UK_C = new SquadDef
    {
        name = "SP 17Pdr",
        unitClass = UnitClass.Tank,
        soldiers = 5,
        moves = 3,
        //movementType = Tank
        movementData = Pathfinder.WW2_TrackedMedium_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _17Pdr_M10, 20),
            new UnitWeapon(1, M2_Rear, 20)
        },
        targetType = TargetType.Armor,
        armor = 80,
    };



    public static SquadDef Grenadier = new SquadDef
    {
        name = "Grenadier",
        shortName = "Gren",
        soldiers = 9,
        //moves = 1,
        //movementType = Infantry
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, MP40, 10),
            new UnitWeapon(1, MG42, 20),
            new UnitWeapon(7, Kar98k, 10),
            new UnitWeapon(1, Sb, 4),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef Grenadier_Faust = new SquadDef
    {
        name = "Grenadier",
        shortName = "Gren",
        soldiers = 9,
        //moves = 1,
        //movementType = Infantry
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, MP40, 10),
            new UnitWeapon(1, MG42, 20),
            new UnitWeapon(7, Kar98k, 10),
            new UnitWeapon(1, Sb, 4),
            new UnitWeapon(3, PzF30, 1),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    
    public static SquadDef Pz_Grenadier = new SquadDef
    {
        name = "Panzer Grenadier",
        shortName = "PzGren",
        soldiers = 11,
        //moves = 1,
        //movementType = Infantry
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, MP40, 10),
            new UnitWeapon(2, MG42, 20),
            new UnitWeapon(9, Kar98k, 10),
            new UnitWeapon(1, Sb, 4),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef Pz_Grenadier_Faust = new SquadDef
    {
        name = "Panzer Grenadier",
        shortName = "PzGren",
        soldiers = 11,
        //moves = 1,
        //movementType = Infantry
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, MP40, 10),
            new UnitWeapon(2, MG42, 20),
            new UnitWeapon(9, Kar98k, 10),
            new UnitWeapon(1, Sb, 4),
            new UnitWeapon(3, PzF30, 1),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    
    public static SquadDef Armored_Pz_Grenadier = new SquadDef
    {
        name = "Panzer Grenadier (Gpzt)",
        shortName = "Gp PzGren",
        soldiers = 8,
        //moves = 1,
        //movementType = Infantry
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, MP40, 10),
            new UnitWeapon(2, MG42, 20),
            new UnitWeapon(5, Kar98k, 10),
            //new UnitWeapon(1, Sb, 4),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef Armored_Pz_Grenadier_Faust = new SquadDef
    {
        name = "Panzer Grenadier (Gpzt)",
        shortName = "Gp PzGren",
        soldiers = 8,
        //moves = 1,
        //movementType = Infantry
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, MP40, 10),
            new UnitWeapon(2, MG42, 20),
            new UnitWeapon(5, Kar98k, 10),
            //new UnitWeapon(1, Sb, 4),
            new UnitWeapon(3, PzF30, 1),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef Armored_Pz_Grenadier_Plt_HQ = new SquadDef
    {
        name = "Panzer Grenadier (Gpzt) HQ",
        shortName = "Gp PzGren HQ",
        soldiers = 4,
        //moves = 1,
        //movementType = Infantry
        weapons = new UnitWeapon[] {
            new UnitWeapon(2, MP40, 10),
            new UnitWeapon(2, Kar98k, 10),
            //new UnitWeapon(1, Sb, 4),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    
    public static SquadDef SMG34_Team = new SquadDef
    {
        name = "sMG34",
        shortName = "sMG34",
        soldiers = 6,
        //moves = 1,
        //movementType = CSW
        movementData = Pathfinder.WW2_CswMedium_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, SMG34, 20),
            new UnitWeapon(5, Kar98k, 8),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef Mot_SMG34_Team = new SquadDef
    {
        name = "sMG34",
        shortName = "sMG34",
        soldiers = 6,
        //moves = 1,
        //movementType = CSW
        movementData = Pathfinder.WW2_CswMedium_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, SMG34, 20),
            new UnitWeapon(5, Kar98k, 8),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef Gpzt_SMG34_Team = new SquadDef
    {
        name = "sMG34",
        shortName = "sMG34",
        soldiers = 6,
        //moves = 1,
        //movementType = CSW
        movementData = Pathfinder.WW2_CswMedium_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, SMG34, 20),
            new UnitWeapon(5, Kar98k, 8),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef SMG42_Team = new SquadDef
    {
        name = "sMG34",
        shortName = "sMG34",
        soldiers = 6,
        //moves = 1,
        //movementType = CSW
        movementData = Pathfinder.WW2_CswMedium_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, SMG42, 20),
            new UnitWeapon(5, Kar98k, 8),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef Mot_SMG42_Team = new SquadDef
    {
        name = "sMG34",
        shortName = "sMG34",
        soldiers = 6,
        //moves = 1,
        //movementType = CSW
        movementData = Pathfinder.WW2_CswMedium_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, SMG42, 20),
            new UnitWeapon(5, Kar98k, 8),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef Gpzt_SMG42_Team = new SquadDef
    {
        name = "sMG34",
        shortName = "sMG34",
        soldiers = 6,
        //moves = 1,
        //movementType = CSW
        movementData = Pathfinder.WW2_CswMedium_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, SMG42, 20),
            new UnitWeapon(5, Kar98k, 8),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef PanzerShreck_Team = new SquadDef
    {
        name = "PanzerShreck Team",
        shortName = "PzS Tm",
        soldiers = 2,
        //moves = 1,
        //movementType = Infantry
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, MP40, 10),
            new UnitWeapon(1, Kar98k, 10),
            new UnitWeapon(1, PzSck, 5),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    public static SquadDef Pak35_Gun = new SquadDef {
        name = "PAK35 37mm",
        shortName = "PAK 37mm",
        unitClass = UnitClass.Gun,
        soldiers = 3,
        //moves = 1,
        //movementType = CannonFast
        movementData = Pathfinder.WW2_GunVeryLight_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Pak35, 10),
            new UnitWeapon(3, Kar98k, 10),
        },
        targetType = TargetType.Gun,
        hitability = CannonSmall_Hitability
    };

    public static SquadDef Pak38_Gun = new SquadDef {
        name = "PAK38 50mm",
        shortName = "PAK 50mm",
        unitClass = UnitClass.Gun,
        soldiers = 4,
        //moves = 1,
        //movementType = Cannon
        movementData = Pathfinder.WW2_GunLight_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Pak38, 10),
            new UnitWeapon(4, Kar98k, 10),
        },
        targetType = TargetType.Gun,
        hitability = Cannon_Hitability
    };
    
    public static SquadDef Pak40_Gun = new SquadDef {
        name = "PAK40 75mm",
        shortName = "PAK 75mm",
        unitClass = UnitClass.Gun,
        soldiers = 4,
        //moves = 1,
        //movementType = CannonVerySlow
        movementData = Pathfinder.WW2_GunMedium_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Pak40, 10),
            new UnitWeapon(4, Kar98k, 10),
        },
        targetType = TargetType.Gun,
        hitability = Cannon_Hitability
    };

    public static SquadDef Pak43_Gun = new SquadDef {
        name = "PAK43 88mm",
        shortName = "PAK 88mm",
        unitClass = UnitClass.Gun,
        soldiers = 5,
        //moves = 1,
        //movementType = CannonVeryVerySlow
        movementData = Pathfinder.WW2_GunVeryHeavy_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _17Pdr, 10),
            new UnitWeapon(5, Kar98k, 10),
        },
        targetType = TargetType.Gun,
        hitability = CannonLarge_Hitability
    };

    public static SquadDef LeFH_18_Gun = new SquadDef {
        name = "LeFH 18 105mm",
        shortName = "LeFH18",
        unitClass = UnitClass.Gun,
        soldiers = 5,
        //moves = 1,
        //movementType = CannonVerySlow
        movementData = Pathfinder.WW2_GunHeavy_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, LeFH_18, 10),
            new UnitWeapon(5, Kar98k, 10),
        },
        targetType = TargetType.Gun,
        hitability = CannonLarge_Hitability
    };

    public static SquadDef Flak_36 = new SquadDef {
        name = "FlAK36 88mm",
        shortName = "FlAK36 88mm",
        unitClass = UnitClass.Gun,
        soldiers = 6,
        //moves = 1,
        //movementType = CannonVeryVerySlow
        movementData = Pathfinder.WW2_GunVeryVeryHeavy_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _3_7In, 10),
            new UnitWeapon(6, Kar98k, 10),
        },
        targetType = TargetType.Gun,
        hitability = CannonVeryLarge_Hitability
    };
    public static SquadDef Flak_41 = new SquadDef {
        name = "FlAK41 88mm",
        shortName = "FlAK41 88mm",
        unitClass = UnitClass.Gun,
        soldiers = 6,
        //moves = 1,
        //movementType = CannonVeryVerySlow
        movementData = Pathfinder.WW2_GunVeryVeryHeavy_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, _3_7In, 10),
            new UnitWeapon(6, Kar98k, 10),
        },
        targetType = TargetType.Gun,
        hitability = CannonVeryLarge_Hitability
    };
    
    
    public static SquadDef Grw34_Team = new SquadDef {
        name = "GrW34 8cm Mtr",
        shortName = "GrW34 Mtr",
        soldiers = 4,
        //moves = 1,
        //movementType = CSW
        movementData = Pathfinder.WW2_CswHeavy_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Grw34, 10),
            new UnitWeapon(4, Kar98k, 10),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef Mot_Grw34_Team = new SquadDef {
        name = "GrW34 8cm Mtr",
        shortName = "GrW34 Mtr",
        soldiers = 4,
        //moves = 1,
        //movementType = CSW
        movementData = Pathfinder.WW2_CswHeavy_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Grw34, 10),
            new UnitWeapon(4, Kar98k, 10),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };
    public static SquadDef Grw42_Team = new SquadDef {
        name = "GrW42 12cm Mtr",
        shortName = "GrW42 Mtr",
        soldiers = 6,
        //moves = 1,
        //movementType = CSW
        movementData = Pathfinder.WW2_CswVeryHeavy_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Grw42, 10),
            new UnitWeapon(6, Kar98k, 10),
        },
        targetType = TargetType.Infantry,
        hitability = Infantry_Hitability
    };

    // Crom 4 & 5
    public static SquadDef Panzer_4 = new SquadDef
    {
        name = "Panzer_IV",
        unitClass = UnitClass.Tank,
        soldiers = 5,
        moves = 4,
        //movementType = TankSlow
        movementData = Pathfinder.WW2_TrackedSlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Kwk40, 25),
            new UnitWeapon(1, MG34_Vec, 20),
            new UnitWeapon(1, MG34_Vec_Hull, 20),
            new UnitWeapon(1, MG34_Vec_AA, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 80,
    };
    public static SquadDef Panther = new SquadDef
    {
        name = "Panther",
        unitClass = UnitClass.Tank,
        soldiers = 5,
        moves = 4,
        //movementType = TankFast
        movementData = Pathfinder.WW2_TrackedFast_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Kwk42, 25),
            new UnitWeapon(1, MG34_Vec, 20),
            new UnitWeapon(1, MG34_Vec_Hull, 20),
            new UnitWeapon(1, MG34_Vec_AA, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 120,
    };
    public static SquadDef Tiger = new SquadDef
    {
        name = "Tiger",
        unitClass = UnitClass.Tank,
        soldiers = 5,
        moves = 4,
        //movementType = TankFast
        movementData = Pathfinder.WW2_TrackedMedium_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Kwk36, 25),
            new UnitWeapon(1, MG34_Vec, 20),
            new UnitWeapon(1, MG34_Vec_Hull, 20),
            new UnitWeapon(1, MG34_Vec_AA, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 107,
    };

    public static SquadDef KingsTiger_P = new SquadDef
    {
        name = "King's Tiger (P)",
        unitClass = UnitClass.Tank,
        soldiers = 5,
        moves = 4,
        //movementType = TankSlow
        movementData = Pathfinder.WW2_TrackedSlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Kwk43, 25),
            new UnitWeapon(1, MG34_Vec, 20),
            new UnitWeapon(1, MG34_Vec_Hull, 20),
            new UnitWeapon(1, MG34_Vec_AA, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 190,
    };

    public static SquadDef KingsTiger_H = new SquadDef
    {
        name = "King's Tiger (H)",
        unitClass = UnitClass.Tank,
        soldiers = 5,
        moves = 4,
        //movementType = TankSlow
        movementData = Pathfinder.WW2_TrackedSlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Kwk43, 25),
            new UnitWeapon(1, MG34_Vec, 20),
            new UnitWeapon(1, MG34_Vec_Hull, 20),
            new UnitWeapon(1, MG34_Vec_AA, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 200,
    };
    
    public static SquadDef Hetzer = new SquadDef
    {
        name = "JagdPanzer 38",
        unitClass = UnitClass.PanzerJager,
        soldiers = 4,
        moves = 4,
        //movementType = Tank
        movementData = Pathfinder.WW2_TrackedMedium_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Pak39, 25),
            new UnitWeapon(1, MG34_Vec_Roof, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 100,
    };
    public static SquadDef Stug_3 = new SquadDef
    {
        name = "StuG III",
        unitClass = UnitClass.PanzerJager,
        soldiers = 4,
        moves = 4,
        //movementType = Tank
        movementData = Pathfinder.WW2_TrackedSlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Stuk40, 25),
            new UnitWeapon(1, MG34_Vec_Roof, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 100,
    };
    public static SquadDef Stuh_42 = new SquadDef
    {
        name = "StuH 42",
        unitClass = UnitClass.PanzerJager,
        soldiers = 4,
        moves = 4,
        //movementType = Tank
        movementData = Pathfinder.WW2_TrackedSlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Stuk105, 15),
            new UnitWeapon(1, MG34_Vec_Roof, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 100,
    };
    public static SquadDef Stug_4 = new SquadDef
    {
        name = "StuG IV",
        unitClass = UnitClass.PanzerJager,
        soldiers = 4,
        moves = 4,
        //movementType = Tank
        movementData = Pathfinder.WW2_TrackedSlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Stuk40, 25),
            new UnitWeapon(1, MG34_Vec_Roof, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 100,
    };
    public static SquadDef JP_4 = new SquadDef
    {
        name = "JagdPanzer IV",
        unitClass = UnitClass.PanzerJager,
        soldiers = 4,
        moves = 4,
        //movementType = Tank
        movementData = Pathfinder.WW2_TrackedSlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Pak39, 25),
            new UnitWeapon(1, MG34_Vec, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 100,
    };
    public static SquadDef JP_4_L70 = new SquadDef
    {
        name = "JagdPanzer IV L/70",
        unitClass = UnitClass.PanzerJager,
        soldiers = 4,
        moves = 4,
        //movementType = Tank
        movementData = Pathfinder.WW2_TrackedSlow_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Pak42, 25),
            new UnitWeapon(1, MG34_Vec, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 120,
    };
    public static SquadDef JagdPanther = new SquadDef
    {
        name = "JagdPanther",
        unitClass = UnitClass.PanzerJager,
        soldiers = 5,
        moves = 4,
        //movementType = Tank
        movementData = Pathfinder.WW2_TrackedFast_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(1, Pak43_Vec, 25),
            new UnitWeapon(1, MG34_Vec, 20),
            //new UnitWeapon(1, M2, 20)
        },
        targetType = TargetType.Armor,
        armor = 120,
    };
    

}