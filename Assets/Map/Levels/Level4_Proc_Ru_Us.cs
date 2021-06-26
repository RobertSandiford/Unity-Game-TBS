using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class LevelDefs
{
    
    public static Level level4 = new Level
    {
        randomMap = true,
        //width = 34,
        //height = 40,

        // width = 70,
        // height = 90,

         width = 40,
         height = 50,


        aiMission = AiMission.Objective,


        objectivesDefs = new List<Objective> {
            new Objective(1, 2, 1)
        },
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
            

            
            new MapUnitGroup (1, 1, new int[] { -3, 3 }, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(Definitions.BMP_2),
                new MapUnit(Definitions.RU_Motor_Inf),
                new MapUnit(Definitions.BMP_2),
                new MapUnit(Definitions.RU_Motor_Inf),
                new MapUnit(Definitions.BMP_2),
                new MapUnit(Definitions.RU_Motor_Inf),
            }),
            new MapUnitGroup (2, 1, new int[] { 7, 1 }, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(Definitions.BMP_2),
                new MapUnit(Definitions.RU_Motor_Inf),
                new MapUnit(Definitions.BMP_2),
                new MapUnit(Definitions.RU_Motor_Inf),
                new MapUnit(Definitions.BMP_2),
                new MapUnit(Definitions.RU_Motor_Inf),
            }),
            //new MapUnitGroup (4, 1, new int[] { -5, 1 }, new List<MapUnit> {
            //    new MapUnit(Definitions.BMP_2),
            //    new MapUnit(Definitions.RU_Konkurs_Squad),
            //}),
            //new MapUnitGroup (5, 1, new int[] { 5, 1 }, new List<MapUnit> {
            //    new MapUnit(Definitions.BMP_2),
            //    new MapUnit(Definitions.RU_AGS30_Squad),
            //}),

            new MapUnitGroup (6, 1, new int[] { 0, 0 }, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(Definitions.BMP_2),
                new MapUnit(Definitions.RU_Motor_Inf_Coy_Command),
                new MapUnit(Definitions.BMP_2),
                new MapUnit(Definitions.RU_Motor_Inf_Coy_HQ),
                new MapUnit(Definitions.BMP_2),
                new MapUnit(Definitions.RU_Konkurs_Squad),
                new MapUnit(Definitions.BMP_2),
                new MapUnit(Definitions.RU_AGS30_Squad),
            }),

            new MapUnitGroup (8, 1, new int[] { 3, 3 }, MapUnitGroupType.Armor, new List<MapUnit> {
                new MapUnit(Definitions.T_72B3),
                new MapUnit(Definitions.T_72B3),
                new MapUnit(Definitions.T_72B3),
            }),
            new MapUnitGroup (9, 1, new int[] { -2, -2 }, MapUnitGroupType.Arty, new List<MapUnit> {
                new MapUnit(Definitions.NonaS),
                new MapUnit(Definitions.NonaS),
            }),
            /*new MapUnitGroup (9, 1, new List<MapUnit> {
                new MapUnit(Definitions.NonaSvk),
                new MapUnit(Definitions.NonaSvk),
            }),*/
            /*new MapUnitGroup (9, 1, new List<MapUnit> {
                new MapUnit(Definitions.GAZ_66),
                new MapUnit(Definitions.RU_120mm_Mortar),
                new MapUnit(Definitions.GAZ_66),
                new MapUnit(Definitions.RU_120mm_Mortar),
            }),*/
            new MapUnitGroup (10, 1, new int[] { -4, -2 }, MapUnitGroupType.Arty, new List<MapUnit> {
                new MapUnit(Definitions.MstaS, new int[] { 11, -4 } ),
                new MapUnit(Definitions.MstaS, new int[] { 13, -4 } ),
            }),
            new MapUnitGroup (11, 1, new int[] { 4, -2 }, MapUnitGroupType.AA, new List<MapUnit> {
                new MapUnit(Definitions.BTR_80),
                new MapUnit(Definitions.IglaS_Squad),
                new MapUnit(Definitions.Shilka_M4_Strelet),
                new MapUnit(Definitions.MT_LB_Strela_10M3),
            }),
            
            /*new MapUnitGroup (12, 1, new List<MapUnit> {
                new MapUnit(Definitions.BRDM_2),
                new MapUnit(Definitions.RU_Scouts),
                new MapUnit(Definitions.BRDM_2),
                new MapUnit(Definitions.RU_Scouts),
            }),*/
            new MapUnitGroup (12, 1, new int[] { 4, 6 }, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(Definitions.BMP_2),
                new MapUnit(Definitions.RU_Recon_Squad),
            }),

            /*new MapUnitGroup (14, 1, new List<MapUnit> {
                new MapUnit(Definitions.Hind_PN),
                new MapUnit(Definitions.Hind_PN),
            }),*/
            /*new MapUnitGroup (14, 1, new List<MapUnit> {
                new MapUnit(Definitions.Hip),
                new MapUnit(Definitions.Hip),
            }),*/

            
            /*new MapUnitGroup (21, 1, new List<MapUnit> {
                new MapUnit(Definitions.Stryker_MK19),
                new MapUnit(Definitions.US_Stryker_Inf),
                new MapUnit(Definitions.Stryker_M2),
                new MapUnit(Definitions.US_Stryker_Inf),
                new MapUnit(Definitions.Stryker_MK19),
                new MapUnit(Definitions.US_Stryker_Inf),
                new MapUnit(Definitions.Stryker_M2),
                new MapUnit(Definitions.US_Stryker_Weapons),
            }),*/

            new MapUnitGroup (21, 2, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(Definitions.Bradley),
                new MapUnit(Definitions.US_Mech_Inf_Jav),
                new MapUnit(Definitions.Bradley),
                new MapUnit(Definitions.US_Mech_Inf_Jav),
                new MapUnit(Definitions.Bradley),
                new MapUnit(Definitions.US_Mech_Inf_Jav),
                new MapUnit(Definitions.Bradley),
            }),
            /*new MapUnitGroup (22, 2, new List<MapUnit> {
                new MapUnit(Definitions.Bradley),
                new MapUnit(Definitions.US_Mech_Inf_Jav),
                new MapUnit(Definitions.Bradley),
                new MapUnit(Definitions.US_Mech_Inf_Jav),
                new MapUnit(Definitions.Bradley),
                new MapUnit(Definitions.US_Mech_Inf_Jav),
                new MapUnit(Definitions.Bradley),
            }),*/
            /*new MapUnitGroup (23, 2, new List<MapUnit> {
                new MapUnit(Definitions.Abrams),
                new MapUnit(Definitions.Abrams),
            }),*/

        },

        /*
        //platoon groups
        
        // T72
        
        // MotRifCoyBMP
        // MotRifBMP
        // MotRifBMP
        // MotRifBMP

        // MotRifMtr82BMP
        
        // MotRifKornet

        // MotRifIgla

        
        */

        fortsDefs = new Dictionary<int, MapFort>
        {
            { 21, new MapFort(FortDefs.Infantry, 3) },
            { 23, new MapFort(FortDefs.Infantry, 3) },
            { 25, new MapFort(FortDefs.Infantry, 3) },
            { 27, new MapFort(FortDefs.Infantry, 3) },

        },
        bases = new Dictionary<int, double[][]>
        {
            { 1, new double[][] {
                new double[] {0.3, 0.05}
            } },
            { 2, new double[][] {
                new double[] { 0.2 + Lib.random.NextDouble() * 0.6, 0.5 + Lib.random.NextDouble() * 0.3 },
                new double[] { 0.1 + Lib.random.NextDouble() * 0.8, 0.97 }
            } }
            /*{ 1, new int[] {5, 2} },
            //{ 1, new double[] {0.1, 0.07} },
            { 2, new int[] {-7, -3} }, // why -6 not -7
            //{ 2, new double[] {0.6, 0.7} },
            //{ 2, new double[] {0.9, 0.93} },*/
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
