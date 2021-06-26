using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlatoonDef
{
    public string name;
    public string shortName;
    public string veryShortName;
    public UnitClass unitClass = UnitClass.Infantry;
    public List<SquadDef> squadDefs;
    //public int soldiers;
    //public int moves;
    //public int setupTime;
    //public int packupTime;
    //public UnitWeapon[] weapons;
    //public WeaponGroup[] weaponGroups;
    public TargetType targetType = TargetType.Infantry;
    //public double hitability = 1.0;
    //public int armor;
    //public Countermeasures countermeasures;
    //public CargoDef cargoDef;
    //public int minAltitude = 0;
    //public int maxAltitude = 0;
    //public double gainAltCost = 0;
    //public double loseAltPayment = 0;
    //public FortType fortType = FortType.Infantry;
    //public FortDef fortDef = FortDefs.forts[FortType.Infantry];
    //public MovementData movementData = Pathfinder.Infantry_MovementData;
    //public MovementData movementData = Pathfinder.WW2_Infantry_MovementData;

    public PlatoonDef()
    {
    }

    public PlatoonDef(string Name)
    {
        name = Name;
        shortName = name;
    }

    public PlatoonDef(string Name, List<SquadDef> SquadDefs)
    {
        name = Name;
        shortName = name;
        squadDefs = SquadDefs;
    }

    /*public PlatoonDef(Dictionary<string, object> Values)
    {
        name = Values.ContainsKey("name") ? (string)Values["name"] : "No Name";
        shortName = Values.ContainsKey("shortName") ? (string)Values["shortName"] : name;
        //squads = Values.ContainsKey("squads") ? Values["squads"]
    }*/
}



public static class PlatoonDefs {

    public static string SQUAD = ".S";
    public static string SECTION = ".Sn";
    public static string PLATOON = ".P";
    public static string COMPANY = ".C";
    
    public static string PLATOONHQ = " PHQ";
    public static string COMPANYHQ = " CHQ";

    public static string VS_MI = "Mech Inf";
    public static string VS_MR = "Mot Rif";

    ////////////////////////////
    //// US Infantry Platoons
    ////////////////////////////
    
    ///////////
    //// US Mech Inf Bradley Coy HQ
    ///////////
    public static PlatoonDef Platoon_US_Mech_Inf_Coy_Hq = new PlatoonDef {
        name = "Mech Inf (Bradley) Coy HQ",
        shortName = "Mech Inf (Bradley) Coy HQ",
        veryShortName = "MI(Brad)" + PLATOON,
        unitClass = UnitClass.Ifv,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.Bradley,
            Definitions.Bradley,
            Definitions.M113,
        }
    };

    ///////////
    //// US Mech Inf Bradley Plt
    ///////////
    public static PlatoonDef Platoon_US_Mech_Inf = new PlatoonDef {
        name = "Mech Inf Platoon (Bradley)",
        shortName = "Mech Inf (Bradley) Plt",
        veryShortName = "Bradley(MI)" + PLATOON,
        squadDefs = new List<SquadDef> {
            Definitions.US_Mech_Inf_Plt_Hq,
            Definitions.US_Mech_Inf_Jav,
            Definitions.US_Mech_Inf_Jav,
            Definitions.US_Mech_Inf_Jav,
        }
    };
    public static PlatoonDef Platoon_US_Bradley_Mech_Inf = new PlatoonDef {
        name = "Bradley M2A3 (Mech Inf)",
        shortName = "Bradley (Mech Inf) Plt",
        unitClass = UnitClass.Ifv,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.Bradley,
            Definitions.Bradley,
            Definitions.Bradley,
            Definitions.Bradley,
        }
    };

    ////////////////////////////
    //// US Mortars
    ////////////////////////////
    
    ////////////
    // M121 M113 Mech Mortar
    ////////////
    public static PlatoonDef Platoon_US_M121_120mm_Mortar = new PlatoonDef {
        name = "M121 120mm Mortar Platoon",
        shortName = "M121 120mm Mtr",
        unitClass = UnitClass.MechMortar,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.M121,
            Definitions.M121,
            Definitions.M121,
            Definitions.M121,
        }
    };

    ////////////////////////////
    //// US Tank Platoons
    ////////////////////////////
    
    ////////////
    // Abrams Platoon
    ////////////
    public static PlatoonDef Platoon_US_Abrams_M1A2C = new PlatoonDef {
        name = "Abrams M1A2C Platoon",
        shortName = "Abrams",
        unitClass = UnitClass.Tank,
        targetType = TargetType.Heavy_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.Abrams,
            Definitions.Abrams,
            Definitions.Abrams,
            Definitions.Abrams,
        }
    };



    /////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////
    //// RU Infantry Platoons
    ////////////////////////////

    /////////////
    //// Recon
    /////////////
    public static PlatoonDef Platoon_RU_Recon = new PlatoonDef {
        name = "Motor Rifle Recon",
        shortName = "MR Recon",
        squadDefs = new List<SquadDef> {
            Definitions.RU_Recon_Hq,
            Definitions.RU_Recon_Squad,
            Definitions.RU_Recon_Squad,
            Definitions.RU_Recon_Squad,
        }
    };
    // BTR-80
    public static PlatoonDef Platoon_RU_Btr_80_Recon = new PlatoonDef {
        name = "BTR-80 Platoon (Recon)",
        shortName = "BTR-80",
        unitClass = UnitClass.Apc,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.BTR_80,
            Definitions.BTR_80,
            Definitions.BTR_80,
        }
    };
    // BMP-2
    public static PlatoonDef Platoon_RU_Bmp_2_Recon = new PlatoonDef {
        name = "BMP-2 Platoon (Recon)",
        shortName = "BMP-2",
        unitClass = UnitClass.Ifv,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.BMP_2,
            Definitions.BMP_2,
            Definitions.BMP_2,
        }
    };
    // BMP-3
    public static PlatoonDef Platoon_RU_Bmp_3_Recon = new PlatoonDef {
        name = "BMP-3 Platoon (Recon)",
        shortName = "BMP-3",
        unitClass = UnitClass.Ifv,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.BMP_3,
            Definitions.BMP_3,
            Definitions.BMP_3,
        }
    };

    ////////////////////////
    // BTR Battalion squads
    ////////////////////////

    ///////////
    //// BTR motor inf plt
    ///////////
    public static PlatoonDef Platoon_RU_Motor_Inf_Btr = new PlatoonDef {
        name = "Motor Rifle Platoon (BMP)",
        shortName = "Mtr Rif",
        veryShortName = VS_MR + PLATOON,
        squadDefs = new List<SquadDef> {
            Definitions.RU_Motor_Inf_Hq_Bmp,
            Definitions.RU_Motor_Inf_Bmp,
            Definitions.RU_Motor_Inf_Bmp,
            Definitions.RU_Motor_Inf_Bmp,
        }
    };
    public static PlatoonDef Platoon_RU_Btr_80_Motor_Inf = new PlatoonDef {
        name = "BTR-80 Platoon (Motor Rifle)",
        shortName = "BTR-80",
        unitClass = UnitClass.Apc,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.BTR_80,
            Definitions.BTR_80,
            Definitions.BTR_80,
        }
    };

    ///////////
    //// BTR motor inf plt w/ AT
    ////////////
    public static PlatoonDef Platoon_RU_Motor_Inf_Btr_At = new PlatoonDef {
        name = "Motor Rifle Platoon (BTR)",
        shortName = "Mtr Rif Plt (BTR)",
        veryShortName = VS_MR + PLATOON,
        squadDefs = new List<SquadDef> {
            Definitions.RU_Motor_Inf_Hq_Btr,
            Definitions.RU_Motor_Inf_Btr,
            Definitions.RU_Motor_Inf_Btr,
            Definitions.RU_Motor_Inf_Btr,
            Definitions.RU_Metis_Squad,
        }
    };
    public static PlatoonDef Platoon_RU_Btr_80_Motor_Inf_At = new PlatoonDef {
        name = "BTR-80 Platoon (Motor Rifle)",
        shortName = "BTR-80 Plt (MR)",
        unitClass = UnitClass.Apc,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.BTR_80,
            Definitions.BTR_80,
            Definitions.BTR_80,
        }
    };
    
    ///////////
    //// BTR motor inf coy hq
    ///////////
    public static PlatoonDef Platoon_RU_Motor_Inf_Coy_Hq_Btr = new PlatoonDef {
        name = "Motor Rifle Coy HQ (BTR)",
        shortName = "MR Coy HQ (BTR)",
        veryShortName = VS_MR + COMPANYHQ,
        squadDefs = new List<SquadDef> {
            Definitions.RU_Motor_Inf_Coy_Full_Hq_Btr,
        }
    };
    public static PlatoonDef Platoon_RU_Btr_80_Motor_Inf_Coy_Hq = new PlatoonDef {
        name = "BTR-80 Platoon (MR Coy HQ)",
        shortName = "BTR-80 Plt (MR CHQ)",
        unitClass = UnitClass.Apc,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.BTR_80,
            Definitions.BTR_80,
        }
    };

    
    ///////////
    //// BTR AGL Plt
    ///////////
    public static PlatoonDef Platoon_RU_Ags_30_Btr = new PlatoonDef {
        name = "MR AGS-30 Platoon (BTR)",
        shortName = "AGS-30 Plt (BTR)",
        veryShortName = "AGS-30" + PLATOON,
        squadDefs = new List<SquadDef> {
            Definitions.RU_AGS30_Hq,
            Definitions.RU_AGS30_Squad,
            Definitions.RU_AGS30_Squad,
            Definitions.RU_AGS30_Squad,
        }
    };
    // BTR-80
    public static PlatoonDef Platoon_RU_Btr_80_Ags_30 = new PlatoonDef {
        name = "BTR-80 Platoon (AGS-30)",
        shortName = "BTR-80 Plt (AGS-30)",
        unitClass = UnitClass.Apc,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.BTR_80,
            Definitions.BTR_80,
            Definitions.BTR_80,
        }
    };
    
    ///////////
    //// BTR Konkurs Plt
    ///////////
    public static PlatoonDef Platoon_RU_Konkurs_Btr = new PlatoonDef {
        name = "MR Konkurs Platoon (BTR)",
        shortName = "Konkurs Plt (BTR)",
        veryShortName = "Konkurs" + PLATOON,
        squadDefs = new List<SquadDef> {
            Definitions.RU_Konkurs_Hq,
            Definitions.RU_Konkurs_Squad,
            Definitions.RU_Konkurs_Squad,
            Definitions.RU_Konkurs_Squad,
        }
    };
    // BTR-80
    public static PlatoonDef Platoon_RU_Btr_80_Konkurs = new PlatoonDef {
        name = "BTR-80 Platoon (Konkurs)",
        shortName = "BTR-80 Plt (Konkurs)",
        veryShortName = "BTR-80.P",
        unitClass = UnitClass.Apc,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.BTR_80,
            Definitions.BTR_80,
            Definitions.BTR_80,
        }
    };

    /////////////////////////
    // BMP Battalion squads
    /////////////////////////

    ///////////
    //// BMP motor inf plt
    ///////////
    public static PlatoonDef Platoon_RU_Motor_Inf_Bmp = new PlatoonDef {
        name = "Motor Rifle Platoon (BMP)",
        shortName = "Mtr Rif Plt (BMP)",
        veryShortName = VS_MR + PLATOON,
        squadDefs = new List<SquadDef> {
            Definitions.RU_Motor_Inf_Hq_Bmp,
            Definitions.RU_Motor_Inf_Bmp,
            Definitions.RU_Motor_Inf_Bmp,
            Definitions.RU_Motor_Inf_Bmp,
        }
    };
    // BMP-2
    public static PlatoonDef Platoon_RU_Bmp_2_Motor_Inf = new PlatoonDef {
        name = "BMP-2 Platoon (Motor Rifle)",
        shortName = "BMP-2 Plt (MR)",
        unitClass = UnitClass.Ifv,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.BMP_2,
            Definitions.BMP_2,
            Definitions.BMP_2,
        }
    };
    // BMP-3
    public static PlatoonDef Platoon_RU_Bmp_3_Motor_Inf = new PlatoonDef {
        name = "BMP-3 Platoon (Motor Rifle)",
        shortName = "BMP-3 Plt (MR)",
        unitClass = UnitClass.Ifv,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.BMP_3,
            Definitions.BMP_3,
            Definitions.BMP_3,
        }
    };
    
    /////////////
    //// BMP motor inf coy hq
    ////////////
    public static PlatoonDef Platoon_RU_Motor_Inf_Coy_Hq_Bmp = new PlatoonDef {
        name = "Motor Rifle Coy HQ (BMP)",
        shortName = "MR Coy HQ (BMP)",
        veryShortName = VS_MR + COMPANYHQ,
        squadDefs = new List<SquadDef> {
            Definitions.RU_Motor_Inf_Coy_Full_Hq_Bmp,
        }
    };
    public static PlatoonDef Platoon_RU_Bmp_2_Motor_Inf_Coy_Hq = new PlatoonDef {
        name = "BMP-2 Platoon (MR Coy HQ)",
        shortName = "BMP-2 Plt (MR CHQ)",
        unitClass = UnitClass.Ifv,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.BMP_2,
            Definitions.BMP_2,
        }
    };
    public static PlatoonDef Platoon_RU_Bmp_3_Motor_Inf_Coy_Hq = new PlatoonDef {
        name = "BMP-3 Platoon (MR Coy HQ)",
        shortName = "BMP-3 Plt (MR CHQ)",
        unitClass = UnitClass.Ifv,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.BMP_3,
            Definitions.BMP_3,
        }
    };
    
    ///////////
    //// BMP AGL Plt
    ///////////
    public static PlatoonDef Platoon_RU_Ags_30_Bmp = new PlatoonDef {
        name = "MR AGS-30 Platoon (BMP)",
        shortName = "AGS-30 Plt (BMP)",
        veryShortName = "AGS-30" + PLATOON,
        squadDefs = new List<SquadDef> {
            Definitions.RU_AGS30_Hq,
            Definitions.RU_AGS30_Squad,
            Definitions.RU_AGS30_Squad,
            Definitions.RU_AGS30_Squad,
        }
    };
    // BMP-2
    public static PlatoonDef Platoon_RU_Bmp_2_Ags_30 = new PlatoonDef {
        name = "BMP-2 Platoon (AGS-30)",
        shortName = "BMP-2 Plt (AGS-30)",
        unitClass = UnitClass.Ifv,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.BMP_2,
            Definitions.BMP_2,
            Definitions.BMP_2,
        }
    };
    // BMP-3
    public static PlatoonDef Platoon_RU_Bmp_3_Ags_30 = new PlatoonDef {
        name = "BMP-3 Platoon (AGS-30)",
        shortName = "BMP-3 Plt (AGS-30)",
        unitClass = UnitClass.Ifv,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.BMP_3,
            Definitions.BMP_3,
            Definitions.BMP_3,
        }
    };
    
    ///////////
    //// BMP Konkurs Plt
    ///////////
    public static PlatoonDef Platoon_RU_Konkurs_Bmp = new PlatoonDef {
        name = "MR Konkurs Platoon (BMP)",
        shortName = "Konkurs Plt (BMP)",
        veryShortName = "Konkurs" + PLATOON,
        squadDefs = new List<SquadDef> {
            Definitions.RU_Konkurs_Hq,
            Definitions.RU_Konkurs_Squad,
            Definitions.RU_Konkurs_Squad,
            Definitions.RU_Konkurs_Squad,
        }
    };
    // BMP-2
    public static PlatoonDef Platoon_RU_Bmp_2_Konkurs = new PlatoonDef {
        name = "BMP-2 Platoon (Konkurs)",
        shortName = "BMP-2 Plt (Konkurs)",
        unitClass = UnitClass.Ifv,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.BMP_2,
            Definitions.BMP_2,
            Definitions.BMP_2,
        }
    };
    // BMP-3
    public static PlatoonDef Platoon_RU_Bmp_3_Konkurs = new PlatoonDef {
        name = "BMP-3 Platoon (Konkurs)",
        shortName = "BMP-3 Plt (Konkurs)",
        unitClass = UnitClass.Ifv,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.BMP_3,
            Definitions.BMP_3,
            Definitions.BMP_3,
        }
    };

    
    ///////////////////////
    // Marksman Companies


    ///////////////////////////
    //// MR Brigade squads
    ///////////////////////////
    
    ///////////
    //// MR Brigade AT
    ///////////
    public static PlatoonDef Platoon_RU_Kornet_Mt_Lb = new PlatoonDef {
        name = "MR Kornet Platoon (MT-LB)",
        shortName = "Kornet Plt (MT-LB)",
        veryShortName = "Kornet" + PLATOON,
        squadDefs = new List<SquadDef> {
            Definitions.RU_Kornet_Hq,
            Definitions.RU_Kornet_Squad,
            Definitions.RU_Kornet_Squad,
        }
    };
    // MT-LB
    public static PlatoonDef Platoon_RU_MT_LB_Kornet = new PlatoonDef {
        name = "MT-LB Platoon (Kornet)",
        shortName = "MT-LB Plt (Kornet)",
        unitClass = UnitClass.TrackedApc,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.MT_LB,
            Definitions.MT_LB,
        }
    };
    
    ///////////////////////////
    //// Towed Mortars
    ///////////////////////////
    
    //////////////
    // 82mm Podnos
    //////////////
    public static PlatoonDef Platoon_RU_82mm_Mortar_Gaz66 = new PlatoonDef {
        name = "MR 82mm Mortar Platoon (GAZ-66)",
        shortName = "82mm Mtr Plt (GAZ-66)",
        veryShortName = "82mm Mtr" + PLATOON,
        squadDefs = new List<SquadDef> {
            Definitions.RU_82mm_Mortar_Hq,
            Definitions.RU_82mm_Mortar,
            Definitions.RU_82mm_Mortar,
            Definitions.RU_82mm_Mortar,
            Definitions.RU_82mm_Mortar,
        }
    };
    // MT-LB
    public static PlatoonDef Platoon_RU_Gaz_66_82mm_Mortar = new PlatoonDef {
        name = "GAZ-66 Platoon (82mm Mortar)",
        shortName = "GAZ-66 Plt (82mm Mtr)",
        unitClass = UnitClass.Car,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.GAZ_66,
            Definitions.GAZ_66,
            Definitions.GAZ_66,
            Definitions.GAZ_66,
        }
    };
    
    //////////////
    // 120mm Sani
    //////////////
    public static PlatoonDef Platoon_RU_120mm_Mortar_Gaz66 = new PlatoonDef {
        name = "MR 120mm Mortar Platoon (GAZ-66)",
        shortName = "120mm Mtr (GAZ-66)",
        veryShortName = "120mm Mtr" + PLATOON,
        squadDefs = new List<SquadDef> {
            Definitions.RU_120mm_Mortar_Hq,
            Definitions.RU_120mm_Mortar,
            Definitions.RU_120mm_Mortar,
            Definitions.RU_120mm_Mortar,
            Definitions.RU_120mm_Mortar,
        }
    };
    // MT-LB
    public static PlatoonDef Platoon_RU_Gaz_66_120mm_Mortar = new PlatoonDef {
        name = "GAZ-66 Platoon (120mm Mortar)",
        shortName = "GAZ-66 Plt (120mm Mtr)",
        unitClass = UnitClass.Car,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.GAZ_66,
            Definitions.GAZ_66,
            Definitions.GAZ_66,
            Definitions.GAZ_66,
        }
    };

    ///////////////////////////
    //// Self Propelled Mortars
    ///////////////////////////
    
    ////////////
    // Nona-S (BTR-D)
    ////////////
    public static PlatoonDef Platoon_NonaS = new PlatoonDef {
        name = "Nona-S Platoon",
        shortName = "Nona-S Plt",
        unitClass = UnitClass.MechMortar,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.NonaS,
            Definitions.NonaS,
            Definitions.NonaS,
            Definitions.NonaS,
        }
    };

    ///////////////////////////
    //// Self Propelled Artillery
    ///////////////////////////
    
    //////////
    // MstaS
    //////////
    
    public static PlatoonDef Platoon_RU_MstaS = new PlatoonDef {
        name = "MstaS Platoon",
        shortName = "MstaS Plt",
        unitClass = UnitClass.MechArtillery,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.MstaS,
            Definitions.MstaS,
            Definitions.MstaS,
        }
    };

    //////////
    // MLRS
    //////////

    ///////////////////////////
    //// Manpads Anti Air
    ///////////////////////////
    
    //////////
    // Igla
    //////////
    public static PlatoonDef Platoon_RU_Igla = new PlatoonDef {
        name = "MR Igla Platoon (MT-LB)",
        shortName = "Igla Plt (MT-LB)",
        veryShortName = "Igla" + PLATOON,
        squadDefs = new List<SquadDef> {
            Definitions.IglaS_Hq,
            Definitions.IglaS_Squad,
            Definitions.IglaS_Squad,
            Definitions.IglaS_Squad,
        }
    };
    // MT-LB
    public static PlatoonDef Platoon_RU_MT_LB_Igla = new PlatoonDef {
        name = "MT-LB Platoon (Igla)",
        shortName = "MT-LB Plt (Igla)",
        unitClass = UnitClass.TrackedApc,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.MT_LB,
            Definitions.MT_LB,
            Definitions.MT_LB,
        }
    };

    //////////
    // Igla Tor Coy
    //////////
    public static PlatoonDef Platoon_RU_Igla_Tor_Coy = new PlatoonDef {
        name = "MR Igla Squad (Tor Coy) (MT-LB)",
        shortName = "Igla Sq (MT-LB)",
        squadDefs = new List<SquadDef> {
            Definitions.IglaS_Squad,
        }
    };
    // MT-LB
    public static PlatoonDef Platoon_RU_MT_LB_Igla_Tor_Coy = new PlatoonDef {
        name = "MT-LB Platoon (Igla Tor Coy)",
        shortName = "MT-LB Vec (Igla)",
        veryShortName = "Igla" + SQUAD,
        unitClass = UnitClass.TrackedApc,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.MT_LB,
        }
    };

    ///////////////////////////
    //// Self Propelled Anti Air
    ///////////////////////////
    
    //////////
    // Strela-10
    //////////
    public static PlatoonDef Platoon_RU_Strela_10M3 = new PlatoonDef {
        name = "Strela-10M3 Platoon",
        shortName = "Strela-10M3",
        unitClass = UnitClass.TrackedAa,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.MT_LB_Strela_10M3,
            Definitions.MT_LB_Strela_10M3,
            Definitions.MT_LB_Strela_10M3,
        }
    };
    
    //////////
    // Shilka M4 Strelet
    //////////
    public static PlatoonDef Platoon_RU_Shilka_M4_Strelet = new PlatoonDef {
        name = "Shilka M4 Strelet Platoon",
        shortName = "Shilka M4 Str",
        unitClass = UnitClass.TrackedAa,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.Shilka_M4_Strelet,
            Definitions.Shilka_M4_Strelet,
        }
    };

    //////////
    // Tunguska
    //////////
    public static PlatoonDef Platoon_RU_Tunguska = new PlatoonDef {
        name = "Tunguska Platoon",
        shortName = "Tunguska",
        unitClass = UnitClass.TrackedAa,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.Tunguska,
            Definitions.Tunguska,
        }
    };
    
    //////////
    // Tor
    //////////
    public static PlatoonDef Platoon_RU_Tor = new PlatoonDef {
        name = "Tor Platoon",
        shortName = "Tor",
        unitClass = UnitClass.TrackedAa,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.Tor,
            Definitions.Tor,
            Definitions.Tor,
            Definitions.Tor,
        }
    };



    ///////////////////////////
    //// Tank platoons
    ////////////////////////////
    
    //////////
    // T-72B3
    //////////
    public static PlatoonDef Platoon_RU_T_72B3 = new PlatoonDef {
        name = "T-72B3 Platoon",
        shortName = "T-72B3 Plt",
        unitClass = UnitClass.Tank,
        targetType = TargetType.Heavy_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.T_72B3,
            Definitions.T_72B3,
            Definitions.T_72B3,
        }
    };



    ///////////////////////////
    //// Gun Anti Tank
    ////////////////////////////
    public static PlatoonDef Platoon_RU_SprutSD = new PlatoonDef {
        name = "Sprut-SD Platoon",
        shortName = "Sprut-SD",
        unitClass = UnitClass.Spat,
        targetType = TargetType.Light_Armor,
        squadDefs = new List<SquadDef> {
            Definitions.SprutSD,
            Definitions.SprutSD,
            Definitions.SprutSD,
        }
    };
}




public class PlatoonGroupDef
{
    public string name;
    public string shortName;
    //public UnitType unitType;
    //public UnitClass unitClass = UnitClass.Infantry;
    //public int vehiclesNum;
    //public PlatoonDef[] subPlatoons;
    public PlatoonDef platoon;
    public PlatoonDef mount;

    public PlatoonGroupDef()
    {
    }

    public PlatoonGroupDef(string Name)
    {
        name = Name;
        shortName = name;
    }

    /*public PlatoonGroupDef(string Name, PlatoonDef[] SubPlatoons)
    {
        name = Name;
        shortName = name;
        subPlatoons = SubPlatoons;
    }*/

    //public PlatoonDef(Dictionary<string, object> Values)
    //{
    //    name = Values.ContainsKey("name") ? (string)Values["name"] : "No Name";
    //    shortName = Values.ContainsKey("shortName") ? (string)Values["shortName"] : name;
    //    //squads = Values.ContainsKey("squads") ? Values["squads"]
    //}
}


public static class PlatoonGroupDefs {
    
    
    ///////////////////////////////////////////////////////////////
    // US
    ///////////////////////////////////////////////////////////////
    
    // mi platoon
    public static PlatoonGroupDef PlatoonGroup_US_Mech_Inf = new PlatoonGroupDef {
        name = "Mech Inf Bradley Platoon",
        shortName = "Mech Inf Bradley Plt",
        /*subPlatoons = new PlatoonDef[] {
            PlatoonDefs.Platoon_US_Mech_Inf,
            PlatoonDefs.Platoon_US_Bradley_Mech_Inf
        }*/
        platoon = PlatoonDefs.Platoon_US_Mech_Inf,
        mount = PlatoonDefs.Platoon_US_Bradley_Mech_Inf
    };

    ///////////////////////////////////////////////////////////////
    // Russian
    ///////////////////////////////////////////////////////////////

    /*public static PlatoonGroupDef PlatoonGroup_RU_T72B3 = new PlatoonGroupDef {
        subPlatoons = new PlatoonDef[] {
            PlatoonDefs.Platoon_RU_T_72B3
        }
    };*/


    // mr coy hq
    public static PlatoonGroupDef PlatoonGroup_RU_Motor_Rifle_Coy_Hq_Btr80 = new PlatoonGroupDef {
        name = "Motor Rifle BTR-80 Coy HQ",
        shortName = "Mot Rif BTR-80 Coy HQ",
        /*subPlatoons = new PlatoonDef[] {
            PlatoonDefs.Platoon_RU_Motor_Inf_Coy_Hq_Btr,
            PlatoonDefs.Platoon_RU_Btr_80_Motor_Inf_Coy_Hq
        }*/
        platoon = PlatoonDefs.Platoon_RU_Motor_Inf_Coy_Hq_Btr,
        mount = PlatoonDefs.Platoon_RU_Btr_80_Motor_Inf_Coy_Hq
    };
    public static PlatoonGroupDef PlatoonGroup_RU_Motor_Rifle_Coy_Hq_Bmp2 = new PlatoonGroupDef {
        name = "Motor Rifle BMP-2 Coy HQ",
        shortName = "Mot Rif BMP-2 Coy HQ",
        /*subPlatoons = new PlatoonDef[] {
            PlatoonDefs.Platoon_RU_Motor_Inf_Coy_Hq_Bmp,
            PlatoonDefs.Platoon_RU_Bmp_2_Motor_Inf_Coy_Hq
        }*/
        platoon = PlatoonDefs.Platoon_RU_Motor_Inf_Coy_Hq_Bmp,
        mount = PlatoonDefs.Platoon_RU_Bmp_2_Motor_Inf_Coy_Hq
    };
    public static PlatoonGroupDef PlatoonGroup_RU_Motor_Rifle_Coy_Hq_Bmp3 = new PlatoonGroupDef {
        name = "Motor Rifle BMP-3 Coy HQ",
        shortName = "Mot Rif BMP-3 Coy HQ",
        /*subPlatoons = new PlatoonDef[] {
            PlatoonDefs.Platoon_RU_Motor_Inf_Coy_Hq_Bmp,
            PlatoonDefs.Platoon_RU_Bmp_3_Motor_Inf_Coy_Hq
        }*/
        platoon = PlatoonDefs.Platoon_RU_Motor_Inf_Coy_Hq_Bmp,
        mount = PlatoonDefs.Platoon_RU_Bmp_3_Motor_Inf_Coy_Hq
    };

    // mr platoon
    public static PlatoonGroupDef PlatoonGroup_RU_Motor_Rifle_Btr80 = new PlatoonGroupDef {
        name = "Motor Rifle BTR-80 Platoon",
        shortName = "Mot Rif BTR-80 Plt",
        /*subPlatoons = new PlatoonDef[] {
            PlatoonDefs.Platoon_RU_Motor_Inf_Coy_Hq_Bmp,
            PlatoonDefs.Platoon_RU_Btr_80_Motor_Inf
        }*/
        platoon = PlatoonDefs.Platoon_RU_Motor_Inf_Coy_Hq_Bmp,
        mount = PlatoonDefs.Platoon_RU_Btr_80_Motor_Inf
    };
    public static PlatoonGroupDef PlatoonGroup_RU_Motor_Rifle_Bmp2 = new PlatoonGroupDef {
        name = "Motor Rifle BMP-2 Platoon",
        shortName = "Mot Rif BMP-2 Plt",
        /*subPlatoons = new PlatoonDef[] {
            PlatoonDefs.Platoon_RU_Motor_Inf_Bmp,
            PlatoonDefs.Platoon_RU_Bmp_2_Motor_Inf
        }*/
        platoon = PlatoonDefs.Platoon_RU_Motor_Inf_Bmp,
        mount = PlatoonDefs.Platoon_RU_Bmp_2_Motor_Inf
    };
    public static PlatoonGroupDef PlatoonGroup_RU_Motor_Rifle_Bmp3 = new PlatoonGroupDef {
        name = "Motor Rifle BMP-3 Platoon",
        shortName = "Mot Rif BMP-3 Plt",
        /*subPlatoons = new PlatoonDef[] {
            PlatoonDefs.Platoon_RU_Motor_Inf_Bmp,
            PlatoonDefs.Platoon_RU_Bmp_3_Motor_Inf
        }*/
        platoon = PlatoonDefs.Platoon_RU_Motor_Inf_Bmp,
        mount = PlatoonDefs.Platoon_RU_Bmp_3_Motor_Inf
    };

    // mortars
    public static PlatoonGroupDef PlatoonGroup_RU_Motor_Rifle_82mm_Mortar = new PlatoonGroupDef {
        name = "82mm Mortar GAZ-66 Platoon",
        shortName = "82mm Mtr GAZ-66 Plt",
        /*subPlatoons = new PlatoonDef[] {
            PlatoonDefs.Platoon_RU_82mm_Mortar_Gaz66,
            PlatoonDefs.Platoon_RU_Gaz_66_82mm_Mortar
        }*/
        platoon = PlatoonDefs.Platoon_RU_82mm_Mortar_Gaz66,
        mount = PlatoonDefs.Platoon_RU_Gaz_66_82mm_Mortar
    };

    // brigade at
    public static PlatoonGroupDef PlatoonGroup_RU_Motor_Rifle_Kornet = new PlatoonGroupDef {
        name = "Konkurs AT MT-LB Platoon",
        shortName = "Konkurs MT-LB Plt",
        /*subPlatoons = new PlatoonDef[] {
            PlatoonDefs.Platoon_RU_Kornet_Mt_Lb,
            PlatoonDefs.Platoon_RU_MT_LB_Kornet
        }*/
        platoon = PlatoonDefs.Platoon_RU_Kornet_Mt_Lb,
        mount = PlatoonDefs.Platoon_RU_MT_LB_Kornet
    };

    public static PlatoonGroupDef PlatoonGroup_RU_Motor_Rifle_Igla = new PlatoonGroupDef {
        name = "Igla AA MT-LB Platoon",
        shortName = "Igla MT-LB Plt",
        /*subPlatoons = new PlatoonDef[] {
            PlatoonDefs.Platoon_RU_Igla,
            PlatoonDefs.Platoon_RU_MT_LB_Igla
        }*/
        platoon = PlatoonDefs.Platoon_RU_Igla,
        mount = PlatoonDefs.Platoon_RU_MT_LB_Igla
    };

}






    

