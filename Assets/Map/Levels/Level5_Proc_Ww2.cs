using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class LevelDefs
{
    

    
    public static Level level5 = new Level
    {
        randomMap = true,
        

         width = 52,
         height = 66,


        aiMission = AiMission.Objective,


        //objectivesDefs = new List<Objective> {
        //    new Objective(1, 2, 1)
        //},
        objectivesRandom = new List<Objective2> {
            new Objective2(
                1, // id
                //new int[] { 40, 20 },
                new double[] { 0.5, 0.3 }, // pos
                3, // width
                1 // height
            ),
            new Objective2(
                1, // id
                //new int[] { 40, 20 },
                new double[] { 0.5, 0.53 }, // pos
                3, // width
                1 // height
            ),
            new Objective2(
                1, // id
                //new int[] { 40, 20 },
                new double[] { 0.5, 0.76 }, // pos
                3, // width
                1 // height
            )
        },

        unitsDefsRandom = new List<MapUnitGroup>
        {
            /*
            // German Inf Platoon
            new MapUnitGroup (1, 1, new int[] { -7, 1 }, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(DefsWW2.Grenadier),
                new MapUnit(DefsWW2.Grenadier),
                new MapUnit(DefsWW2.Grenadier),
                new MapUnit(DefsWW2.SMG42_Team),
                new MapUnit(DefsWW2.SMG42_Team),
            }),
            
            // German Inf Platoon
            new MapUnitGroup (2, 1, new int[] { 6, 2 }, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(DefsWW2.Grenadier),
                new MapUnit(DefsWW2.Grenadier),
                new MapUnit(DefsWW2.Grenadier),
                new MapUnit(DefsWW2.SMG42_Team),
                new MapUnit(DefsWW2.SMG42_Team),
            }),

            // German Inf Platoon
            new MapUnitGroup (2, 1, new int[] { 6, -2 }, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(DefsWW2.Grenadier),
                new MapUnit(DefsWW2.Grenadier),
                new MapUnit(DefsWW2.Grenadier),
                new MapUnit(DefsWW2.SMG42_Team),
                new MapUnit(DefsWW2.SMG42_Team),
            }),
            
            // German AT Gun Section
            new MapUnitGroup (6, 1, new int[] { 0, 0 }, MapUnitGroupType.AT, new List<MapUnit> {
                new MapUnit(DefsWW2.SdKfz_11),
                new MapUnit(DefsWW2.Pak40_Gun),
                //new MapUnit(DefsWW2.SdKfz_11),
                //new MapUnit(DefsWW2.Pak40_Gun),
            }),
            
            // German Inf PanzerSchreck Section
            new MapUnitGroup (7, 1, new int[] { 0, 2 }, MapUnitGroupType.AT, new List<MapUnit> {
                new MapUnit(DefsWW2.PanzerShreck_Team),
                new MapUnit(DefsWW2.PanzerShreck_Team),
                new MapUnit(DefsWW2.PanzerShreck_Team),
                new MapUnit(DefsWW2.PanzerShreck_Team),
                new MapUnit(DefsWW2.PanzerShreck_Team),
                new MapUnit(DefsWW2.PanzerShreck_Team),
            }),
            
            // Mortar Section
            new MapUnitGroup (9, 1, new int[] { -5, -3 }, MapUnitGroupType.Arty, new List<MapUnit> {
                // transport? // If.9 handcart
                new MapUnit(DefsWW2.Grw34_Team),
                // transport? // If.9 handcart
                new MapUnit(DefsWW2.Grw34_Team),
            }),

            // German Stug III Section
            //new MapUnitGroup (11, 1, new int[] { 4, -2 }, MapUnitGroupType.Armor, new List<MapUnit> {
            //    new MapUnit(DefsWW2.Stug_3),
            //    new MapUnit(DefsWW2.Stug_3),
            //}),

            // Fusiliers
            //new MapUnitGroup (12, 1, new int[] { 4, 6 }, MapUnitGroupType.Infantry, new List<MapUnit> {
            //    new MapUnit(DefsWW2.BMP_2),
            //    new MapUnit(DefsWW2.RU_Recon_Squad),
            //}),
            */



           
            
            // German Gpzt Inf Platoon
            new MapUnitGroup (1, 1, new int[] { -8, 2 }, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(DefsWW2.Armored_Pz_Grenadier),
                new MapUnit(DefsWW2.Armored_Pz_Grenadier),
                new MapUnit(DefsWW2.Armored_Pz_Grenadier),
                new MapUnit(DefsWW2.Armored_Pz_Grenadier_Plt_HQ),
                new MapUnit(DefsWW2.SdKfz_251),
                new MapUnit(DefsWW2.SdKfz_251),
                new MapUnit(DefsWW2.SdKfz_251),
                new MapUnit(DefsWW2.SdKfz_251),
            }),

            /*
            // German Gpzt Heavy Platoon
            new MapUnitGroup (1, 1, new int[] { -7, 1 }, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(DefsWW2.Gpzt_SMG42_Team),
                new MapUnit(DefsWW2.Gpzt_SMG42_Team),
                new MapUnit(DefsWW2.Gpzt_SMG42_Team),
                //new MapUnit(DefsWW2.Gpzt_Heavy_Plt_Hq),
                new MapUnit(DefsWW2.SdKfz_251_17),
                new MapUnit(DefsWW2.SdKfz_251_17),
                new MapUnit(DefsWW2.SdKfz_251_17),
                //new MapUnit(DefsWW2.SdKfz_251),
                new MapUnit(DefsWW2.SdKfz_251_2),
                new MapUnit(DefsWW2.SdKfz_251_2),
                new MapUnit(DefsWW2.SdKfz_251_9),
                new MapUnit(DefsWW2.SdKfz_251_9),
            }),
            */

            /*
            // German Mot PzGren Platoon
            new MapUnitGroup (1, 1, new int[] { -8, 2 }, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(DefsWW2.Pz_Grenadier),
                new MapUnit(DefsWW2.Pz_Grenadier),
                new MapUnit(DefsWW2.Pz_Grenadier),
                //new MapUnit(DefsWW2.Pz_Grenadier_Plt_HQ),
                new MapUnit(DefsWW2.Opel_Blitz_2T),
                new MapUnit(DefsWW2.Opel_Blitz_2T),
                new MapUnit(DefsWW2.Opel_Blitz_2T),
                //new MapUnit(DefsWW2.Kubelwagens),
                //new MapUnit(DefsWW2.Kubelwagens),
            }),
            */
            
            // German Mot PzGren Platoon
            new MapUnitGroup (2, 1, new int[] { 10, 2 }, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(DefsWW2.Pz_Grenadier),
                new MapUnit(DefsWW2.Pz_Grenadier),
                new MapUnit(DefsWW2.Pz_Grenadier),
                //new MapUnit(DefsWW2.Mot_Plt_Hq),
                new MapUnit(DefsWW2.Opel_Blitz_2T),
                new MapUnit(DefsWW2.Opel_Blitz_2T),
                new MapUnit(DefsWW2.Opel_Blitz_2T),
                //new MapUnit(DefsWW2.Kubelwagens),
                //new MapUnit(DefsWW2.Kubelwagens),
                
            }),
            
            // German Mot Heavy Platoon
            new MapUnitGroup (4, 1, new int[] { 1, 1 }, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(DefsWW2.Mot_SMG42_Team),
                new MapUnit(DefsWW2.Mot_SMG42_Team),
                new MapUnit(DefsWW2.Mot_SMG42_Team),
                new MapUnit(DefsWW2.Mot_SMG42_Team),
                //new MapUnit(DefsWW2.Mot_Heavy_Plt_Hq),
                new MapUnit(DefsWW2.Opel_Blitz_2T),
                new MapUnit(DefsWW2.Opel_Blitz_2T),
                new MapUnit(DefsWW2.Opel_Blitz_2T),
                new MapUnit(DefsWW2.Opel_Blitz_2T),
                //new MapUnit(DefsWW2.Kubelwagens),
            }),
                new MapUnitGroup (5, 1, new int[] { -2, -2 }, MapUnitGroupType.Infantry, new List<MapUnit> {
                    new MapUnit(DefsWW2.Mot_Grw34_Team),
                    new MapUnit(DefsWW2.Mot_Grw34_Team),
                    //new MapUnit(DefsWW2.Mot_Heavy_Plt_Hq),
                    new MapUnit(DefsWW2.Opel_Blitz_2T),
                    new MapUnit(DefsWW2.Opel_Blitz_2T),
                    //new MapUnit(DefsWW2.Kubelwagens),
                }),

            // German AT Gun Section
            /*new MapUnitGroup (6, 1, new int[] { 0, 0 }, MapUnitGroupType.AT, new List<MapUnit> {
                new MapUnit(DefsWW2.SdKfz_11),
                new MapUnit(DefsWW2.Pak40_Gun),
                //new MapUnit(DefsWW2.SdKfz_11),
                //new MapUnit(DefsWW2.Pak40_Gun),
            }),*/

            // Mortar Section
            /*new MapUnitGroup (9, 1, new int[] { -5, -3 }, MapUnitGroupType.Arty, new List<MapUnit> {
                // transport? // If.9 handcart
                new MapUnit(DefsWW2.Grw34_Team),
                // transport? // If.9 handcart
                new MapUnit(DefsWW2.Grw34_Team),
            }),*/

            // Panzer IV Platoon
            new MapUnitGroup (8, 1, new int[] { -8, -2 }, MapUnitGroupType.Armor, new List<MapUnit> {
                new MapUnit(DefsWW2.Panzer_4),
                new MapUnit(DefsWW2.Panzer_4),
                new MapUnit(DefsWW2.Panzer_4),
                new MapUnit(DefsWW2.Panzer_4),
                new MapUnit(DefsWW2.Panzer_4),
            }),
            
            // Panther Platoon
            new MapUnitGroup (9, 1, new int[] { 6, -2 }, MapUnitGroupType.Armor, new List<MapUnit> {
                new MapUnit(DefsWW2.Panther),
                new MapUnit(DefsWW2.Panther),
                new MapUnit(DefsWW2.Panther),
                new MapUnit(DefsWW2.Panther),
                new MapUnit(DefsWW2.Panther),
            }),
            











            // UK Inf Platoon
            new MapUnitGroup (21, 2, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(DefsWW2.UK_Rifle),
                new MapUnit(DefsWW2.UK_Rifle),
                new MapUnit(DefsWW2.UK_Rifle),
                new MapUnit(DefsWW2.UK_Rifle_Plt_HQ),
            }),
            // UK Inf Platoon
            new MapUnitGroup (22, 2, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(DefsWW2.UK_Rifle),
                new MapUnit(DefsWW2.UK_Rifle),
                new MapUnit(DefsWW2.UK_Rifle),
                new MapUnit(DefsWW2.UK_Rifle_Plt_HQ),
            }),
            // UK Mortars
            new MapUnitGroup (24, 2, MapUnitGroupType.Arty, new List<MapUnit> {
                new MapUnit(DefsWW2.Lloyd_Carrier),
                new MapUnit(DefsWW2._3In_Mortar_Team),
                new MapUnit(DefsWW2.Lloyd_Carrier),
                new MapUnit(DefsWW2._3In_Mortar_Team),
            }),
            // UK AT Guns
            new MapUnitGroup (25, 2, MapUnitGroupType.AT, new List<MapUnit> {
                new MapUnit(DefsWW2.Lloyd_Carrier),
                new MapUnit(DefsWW2._6Pdr_Gun),
                new MapUnit(DefsWW2.Lloyd_Carrier),
                new MapUnit(DefsWW2._6Pdr_Gun),
            }),
            // UK Carrier Platoon 
            new MapUnitGroup (27, 2, MapUnitGroupType.Armor, new List<MapUnit> {
                new MapUnit(DefsWW2.Carrier),
                new MapUnit(DefsWW2.Carrier),
                new MapUnit(DefsWW2.Carrier),
            }),
            // UK Tank Sherman Platoon 
            new MapUnitGroup (27, 2, MapUnitGroupType.Armor, new List<MapUnit> {
                new MapUnit(DefsWW2.Sherman),
                new MapUnit(DefsWW2.Sherman),
                new MapUnit(DefsWW2.Sherman),
                new MapUnit(DefsWW2.Sherman5C),
            }),
            // UK Arty
            new MapUnitGroup (29, 2, MapUnitGroupType.Arty, new List<MapUnit> {
                new MapUnit(DefsWW2.Morris_C8),
                new MapUnit(DefsWW2._25Pdr_Gun),
                new MapUnit(DefsWW2.Morris_C8),
                new MapUnit(DefsWW2._25Pdr_Gun),
            }),

        },
        /*fortsDefs = new Dictionary<int, MapFort>
        {
            { 21, new MapFort(FortDefs.Infantry, 3) },
            { 23, new MapFort(FortDefs.Infantry, 3) },
            { 25, new MapFort(FortDefs.Infantry, 3) },
            { 27, new MapFort(FortDefs.Infantry, 3) },

        },*/
        bases = new Dictionary<int, double[][]>
        {
            { 1, new double[][] {
                new double[] {0.3, 0.05}
            } },
            { 2, new double[][] {
                new double[] { 0.2 + Lib.random.NextDouble() * 0.6, 0.5 + Lib.random.NextDouble() * 0.3 },
                new double[] { 0.1 + Lib.random.NextDouble() * 0.8, 0.97 }
            } }
        },
        roads = new List<Road>
        {
            new Road( new List<double[]> {
                new double[] { 0.28, 0 },
                new double[] { 0.5, 0.3 },
                new double[] { 0.5, 0.53 },
                new double[] { 0.5, 0.76 },
                new double[] { 0.7, 1.0 },
            } ),
        },
        virtualHexes = new List<VirtualHex> {
            new VirtualHex(11, 5, -4),
            new VirtualHex(13, 5, -4),
        }
    };

}
