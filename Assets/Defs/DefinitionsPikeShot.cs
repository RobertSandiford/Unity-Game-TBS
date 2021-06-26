using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class DefinitionsPikeShot {
    
    static GameObject bullet_s = (GameObject)Resources.Load("GO/Bullet_S");
    static GameObject bullet_m = (GameObject)Resources.Load("GO/Bullet_M");
    static GameObject bullet_l = (GameObject)Resources.Load("GO/Bullet_L");
    static GameObject bullet_vl = (GameObject)Resources.Load("GO/Bullet_VL");
    static GameObject shell = (GameObject)Resources.Load("GO/shell");

    
    public static double    pike_acc = 0.15;
    public static int       pike_damage_min = 1;
    public static int       pike_damage_max = 1;

    public static double    sword_sidearm_acc = 0.15;
    public static int       sword_sidearm_damage_min = 1;
    public static int       sword_sidearm_damage_max = 1;
    
    public static double    sword_cavalry_acc = 0.2;
    public static int       sword_cavalry_damage_min = 1;
    public static int       sword_cavalry_damage_max = 1;

    public static double    sabre_acc = 0.2;
    public static int       sabre_damage_min = 1;
    public static int       sabre_damage_max = 1;
    
    public static double    musket_acc = 0.15;
    public static double    musket_rfac = 0.4;
    public static int       musket_damage_min = 1;
    public static int       musket_damage_max = 1;


    public static double    pistol_acc = 0.12;
    public static double    pistol_rfac = 0.2;
    public static int       pistol_damage_min = 1;
    public static int       pistol_damage_max = 1;



    public static double infantry_hitability = 0.5;
    public static double horse_hitability = 1.0;


    public static Weapon Pike = new Weapon
    {
        name = "Pike",
        range = Weapon.RealRange(3),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(10, pike_acc, 1, 1.0) },
            { TargetType.Horse, new DamageProfile(10, pike_acc, 1, 1.0) },
        },
        penetration = 900,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 10, 20, 0.4, 1, false)
    };
    public static Weapon Sword_Sidearm = new Weapon
    {
        name = "Sword",
        range = Weapon.RealRange(1),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(12, sword_sidearm_acc, 1, 1.0) },
            { TargetType.Horse, new DamageProfile(12, sword_sidearm_acc, 1, 1.0) },
        },
        penetration = 700,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 12, 20, 0.4, 1, false)
    };
    public static Weapon Sword_Cavalry = new Weapon
    {
        name = "Cavalry Sword",
        range = Weapon.RealRange(1),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(12, sabre_acc, 1, 1.0) },
            { TargetType.Horse, new DamageProfile(12, sabre_acc, 1, 1.0) },
        },
        penetration = 800,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 12, 20, 0.4, 1, false)
    };
    public static Weapon Sabre = new Weapon
    {
        name = "Cavalry Sabre",
        range = Weapon.RealRange(1),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(14, sword_cavalry_acc, 1, 1.0) },
            { TargetType.Horse, new DamageProfile(14, sword_cavalry_acc, 1, 1.0) },
        },
        penetration = 500,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 14, 20, 0.4, 1, false)
    };
    public static Weapon Musket = new Weapon
    {
        name = "Musket",
        range = Weapon.RealRange(200),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(2, musket_acc, 1, musket_rfac) },
            { TargetType.Horse, new DamageProfile(2, musket_acc, 1, musket_rfac) },
        },
        penetration = 3000,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 2, 2, 0.15, 1, true, bullet_m, 15)
    };
    public static Weapon Pistol = new Weapon
    {
        name = "Pistol",
        range = Weapon.RealRange(200),
        damageProfiles = new Dictionary<TargetType, DamageProfile> {
            { TargetType.Infantry, new DamageProfile(1, pistol_acc, 1, pistol_rfac) },
            { TargetType.Horse, new DamageProfile(1, pistol_acc, 1, pistol_rfac) },
        },
        penetration = 1500,
        shootProfile = new ShootProfile(SoundLib.m249Shot, 1, 1, 0.15, 1, true, bullet_m, 15)
    };
    
    public static UnitDef Pike_Block = new UnitDef
    {
        name = "Pike",
        unitClass = UnitClass.Infantry,
        soldiers = 100,
        moves = 4,
        //movementType = Tank
        movementData = Pathfinder.PS_Pike_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(50, Pike),
            new UnitWeapon(50, Sword_Sidearm),
        },
        targetType = TargetType.Infantry,
    };
    public static UnitDef Shot_Block = new UnitDef
    {
        name = "Shot",
        unitClass = UnitClass.Infantry,
        soldiers = 50,
        moves = 4,
        //movementType = Tank
        movementData = Pathfinder.PS_Shot_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(50, Musket, 20),
            new UnitWeapon(50, Sword_Sidearm),
        },
        targetType = TargetType.Infantry,
    };
    public static UnitDef Horse_Block = new UnitDef
    {
        name = "Horse",
        unitClass = UnitClass.Horse,
        soldiers = 20,
        moves = 4,
        //movementType = Tank
        movementData = Pathfinder.PS_Horse_MovementData,
        weapons = new UnitWeapon[] {
            new UnitWeapon(40, Pistol, 3),
            new UnitWeapon(20, Sword_Cavalry),
        },
        targetType = TargetType.Horse,
    };

};

*/