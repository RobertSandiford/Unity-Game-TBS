using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class LevelDefs
{
    
    public static Level level9 = new Level
    {
        randomMap = true,
         width = 20,
         height = 26,

        aiMission = AiMission.Objective,

        objectivesDefs = new List<Objective> {
            new Objective(1, 2, 1)
        },

        objectivesRandom = new List<Objective2> {
            new Objective2(
                1, // id
                new double[] { 0.5, 0.3 }, // pos
                3, // width
                1 // height
            ),
            new Objective2(
                1, // id
                new double[] { 0.5, 0.53 }, // pos
                3, // width
                1 // height
            ),
            new Objective2(
                1, // id
                new double[] { 0.5, 0.76 }, // pos
                3, // width
                1 // height
            )
        },

        bases = new Dictionary<int, double[][]>
        {
            { 1, new double[][] {
                new double[] {0.3, 0.05}
            } },
            { 2, new double[][] {
                new double[] { 0.2 + Lib.random.NextDouble() * 0.6, 0.6 + Lib.random.NextDouble() * 0.3 }, // 0.2-0.8, 0.6-0.9
                new double[] { 0.1 + Lib.random.NextDouble() * 0.8, 0.97 } // 0.1 - 0.9, 0.97
            } }
        },
        
        platoons = new List<MapPlatoon> {
            
            /////////////////////
            // Team 1 - Russia
            /////////////////////
            ///new MapPlatoon(1, new int[] { 5, -1 }, PlatoonDefs.Platoon_RU_T_72B3), // T72 Platoon

            new MapPlatoon(1, new int[] { 1, 1 }, PlatoonGroupDefs.PlatoonGroup_RU_Motor_Rifle_Coy_Hq_Bmp2), // MR BMP Coy Platoon Group
            new MapPlatoon(1, new int[] { 0, 2 }, PlatoonGroupDefs.PlatoonGroup_RU_Motor_Rifle_Bmp2), // MR BMP Platoon Group
            new MapPlatoon(1, new int[] { 2, 2 }, PlatoonGroupDefs.PlatoonGroup_RU_Motor_Rifle_Bmp2), // MR BMP Platoon Group
            ///new MapPlatoon(1, new int[] { 5, 1 }, PlatoonGroupDefs.PlatoonGroup_RU_Motor_Rifle_Bmp2), // MR BMP Platoon Group

            ///new MapPlatoon(1, new int[] { -2, -2 }, PlatoonGroupDefs.PlatoonGroup_RU_Motor_Rifle_82mm_Mortar), // MR Mortar Platoon Group

            ///new MapPlatoon(1, new int[] { -3, 1 }, PlatoonGroupDefs.PlatoonGroup_RU_Motor_Rifle_Kornet), // MR Regiment Konkurs AT Platoon Group

            ///new MapPlatoon(1, new int[] { 1, -1 }, PlatoonGroupDefs.PlatoonGroup_RU_Motor_Rifle_Igla), // MR Regiment Igla AA Platoon Group



            /////////////////////
            // Team 2 - US
            /////////////////////
            /*
            new MapPlatoon(2, PlatoonDefs.Platoon_US_Abrams_M1A2C), // Abrams Tank Platoon

            new MapPlatoon(2, PlatoonDefs.Platoon_US_Mech_Inf_Coy_Hq), // Bradley Mech Inf Coy HQ Platoon Group
            new MapPlatoon(2, PlatoonGroupDefs.PlatoonGroup_US_Mech_Inf), // Bradley Mech Inf Platoon Group
            new MapPlatoon(2, PlatoonGroupDefs.PlatoonGroup_US_Mech_Inf), // Bradley Mech Inf Platoon Group
            new MapPlatoon(2, PlatoonGroupDefs.PlatoonGroup_US_Mech_Inf), // Bradley Mech Inf Platoon Group

            new MapPlatoon(2, PlatoonDefs.Platoon_US_M121_120mm_Mortar) // Mortar Platoon from Battallion, Platoon
            */

        },
     
        fortsDefs = new Dictionary<int, MapFort>
        {
            { 21, new MapFort(FortDefs.Infantry, 3) },
            { 23, new MapFort(FortDefs.Infantry, 3) },
            { 25, new MapFort(FortDefs.Infantry, 3) },
            { 27, new MapFort(FortDefs.Infantry, 3) },

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
