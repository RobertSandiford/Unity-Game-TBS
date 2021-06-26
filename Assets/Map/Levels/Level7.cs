using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class LevelDefs
{
    

    public static Level level7 = new Level
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
            }),

            /*new MapUnitGroup (21, 2, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(Definitions.Bradley),
                new MapUnit(Definitions.US_Mech_Inf_Jav),
                new MapUnit(Definitions.Bradley),
                new MapUnit(Definitions.US_Mech_Inf_Jav),
                new MapUnit(Definitions.Bradley),
                new MapUnit(Definitions.US_Mech_Inf_Jav),
                new MapUnit(Definitions.Bradley),
            }),*/

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
