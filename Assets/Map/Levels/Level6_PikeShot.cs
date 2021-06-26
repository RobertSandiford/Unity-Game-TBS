using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public static partial class LevelDefs
{
    
    
    public static Level level6 = new Level
    {
        randomMap = true,
        
         //width = 52,
         //height = 66,

         width = 22,
         height = 26,


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

           
            
            // P+S Goup
            new MapUnitGroup (1, 1, new int[] { -8, 2 }, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(DefinitionsPikeShot.Pike_Block),
                new MapUnit(DefinitionsPikeShot.Shot_Block),
                new MapUnit(DefinitionsPikeShot.Shot_Block),
            }),
            new MapUnitGroup (2, 1, new int[] { -2, 4 }, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(DefinitionsPikeShot.Pike_Block),
                new MapUnit(DefinitionsPikeShot.Shot_Block),
                new MapUnit(DefinitionsPikeShot.Shot_Block),
            }),
            new MapUnitGroup (3, 1, new int[] { 6, 4 }, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(DefinitionsPikeShot.Pike_Block),
                new MapUnit(DefinitionsPikeShot.Shot_Block),
                new MapUnit(DefinitionsPikeShot.Shot_Block),
            }),
            new MapUnitGroup (4, 1, new int[] { 10, 2 }, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(DefinitionsPikeShot.Pike_Block),
                new MapUnit(DefinitionsPikeShot.Shot_Block),
                new MapUnit(DefinitionsPikeShot.Shot_Block),
            }),

            
            //new MapUnitGroup (5, 1, new int[] { -8, 2 }, MapUnitGroupType.Infantry, new List<MapUnit> {
            //    new MapUnit(DefinitionsPikeShot.Pike_Block),
            //    new MapUnit(DefinitionsPikeShot.Shot_Block),
            //new MapUnit(DefinitionsPikeShot.Shot_Block),
            //}),
            
            

            
            // Horse Group
            new MapUnitGroup (11, 1, new int[] { -2, 0 }, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(DefinitionsPikeShot.Horse_Block),
                new MapUnit(DefinitionsPikeShot.Horse_Block),
                new MapUnit(DefinitionsPikeShot.Horse_Block),
            }),
            
  


            //// Enemy
            
            // P+S Goup
            new MapUnitGroup (21, 2, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(DefinitionsPikeShot.Pike_Block),
                new MapUnit(DefinitionsPikeShot.Shot_Block),
                new MapUnit(DefinitionsPikeShot.Shot_Block),
            }),

            
            //new MapUnitGroup (22, 2, MapUnitGroupType.Infantry, new List<MapUnit> {
            //    new MapUnit(DefinitionsPikeShot.Pike_Block),
            //    new MapUnit(DefinitionsPikeShot.Shot_Block),
            //    new MapUnit(DefinitionsPikeShot.Shot_Block),
            //}),
            
            

            
            // Horse Group
            new MapUnitGroup (31, 2, MapUnitGroupType.Infantry, new List<MapUnit> {
                new MapUnit(DefinitionsPikeShot.Horse_Block),
                new MapUnit(DefinitionsPikeShot.Horse_Block),
                new MapUnit(DefinitionsPikeShot.Horse_Block),
            }),

            

        },
        //fortsDefs = new Dictionary<int, MapFort>
        //{
        //    { 21, new MapFort(FortDefs.Infantry, 3) },
        //    { 23, new MapFort(FortDefs.Infantry, 3) },
        //    { 25, new MapFort(FortDefs.Infantry, 3) },
        //    { 27, new MapFort(FortDefs.Infantry, 3) },
        //},
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
*/