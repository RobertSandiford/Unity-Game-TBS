using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Level
{
    public List<int> heights;
    public List<int> terrains;
    public List<int> objectives;
    public List<Objective2> objectivesRandom;
    public List<int> units;
    public List<int> forts;

    public List<Objective> objectivesDefs;
    public Dictionary<int, MapUnit> unitsDefs;
    public List<MapUnitGroup> unitsDefsRandom;
    public List<MapPlatoon> platoons;
    public Dictionary<int, MapFort> fortsDefs;

    public List<Road> roads;

    public bool randomMap = false;
    public int width;
    public int height;

    public List<VirtualHex> virtualHexes;

    public Dictionary<int, double[][]> bases;

    public AiMission aiMission;

    /*public Level ( List<int> Heights, List<int> Terrains, List<int> Objectives, List<Objective> ObjectivesDefs, List<int> Units, Dictionary<int, MapUnit> UnitsDefs)
    {
        heights = Heights;
        terrains = Terrains;

        objectives = Objectives;
        objectivesDefs = ObjectivesDefs;

        units = Units;
        unitsDefs = UnitsDefs;
    }*/

    public Level()
    {
        roads = new List<Road>();
    }
}


public struct MapUnit
{
    public int id;
    public int team;
    public SquadDef squadDef;
    public int[] location;
    
    public MapUnit(int Id, int Team, SquadDef SquadDef, int[] Location)
    {
        id = Id;
        team = Team;
        squadDef = SquadDef;
        location = Location;
    }
    public MapUnit(int Id, int Team, SquadDef SquadDef)
    {
        id = Id;
        team = Team;
        squadDef = SquadDef;
        location = null;
    }
    public MapUnit(int Team, SquadDef SquadDef)
    {
        id = -1;
        team = Team;
        squadDef = SquadDef;
        location = null;
    }
    public MapUnit(SquadDef SquadDef, int[] Location)
    {
        id = -1;
        team = -1;
        squadDef = SquadDef;
        location = Location;
    }
    public MapUnit(SquadDef SquadDef)
    {
        id = -1;
        team = -1;
        squadDef = SquadDef;
        location = null;
    }

}


   
public struct MapUnitGroup
{
    public int id;
    public int team;
    public int[] pos;
    public List<MapUnit> units;
    MapUnitGroupType type;
    
    public MapUnitGroup(int Id, int Team, int[] Pos, MapUnitGroupType Type, List<MapUnit> Units)
    {
        id = Id;
        team = Team;
        pos = Pos;
        units = Units;
        type = Type;
    }
    public MapUnitGroup(int Id, int Team, MapUnitGroupType Type, List<MapUnit> Units)
    {
        id = Id;
        team = Team;
        pos = null;
        units = Units;
        type = Type;
    }
    public MapUnitGroup(int Id, int Team, int[] Pos, List<MapUnit> Units)
    {
        id = Id;
        team = Team;
        pos = Pos;
        units = Units;
        type = MapUnitGroupType.None;
    }
    public MapUnitGroup(int Id, int Team, List<MapUnit> Units)
    {
        id = Id;
        team = Team;
        pos = null;
        units = Units;
        type = MapUnitGroupType.None;
    }

}


public enum MapUnitGroupType {
    None,
    Infantry,
    InfantrySupport,
    AT,
    Armor,
    AA,
    Arty
}


public struct MapPlatoon
{
    //public int id;
    public int team;
    public string positionType;
    public int[] pos;
    //public SquadDef squadDef;
    //public int[] location;
    
    public PlatoonGroupDef platoonGroup;
    public List<PlatoonDef> platoons;
    public List<SquadDef> squads;
    public List<PlatoonDef> platoonMounts;
    public List<SquadDef> squadMounts;

    /*public MapPlatoon()
    {
        team = 0;
        pos = null;
        platoons = new List<PlatoonDef> { };
        squads = new List<SquadDef> { };
        platoonMounts = new List<PlatoonDef> { };
        squadMounts = new List<SquadDef> { };
    }*/

    public MapPlatoon(int Team, PlatoonDef PlatoonDef)
    {
        //id = Id;
        team = Team;
        //squadDef = SquadDef;
        //location = Location;
        positionType = "base";
        pos = null;
        
        platoonGroup = null;
        platoons = new List<PlatoonDef> { PlatoonDef };
        squads = new List<SquadDef> { };
        platoonMounts = new List<PlatoonDef> { };
        squadMounts = new List<SquadDef> { };
    }

    public MapPlatoon(int Team, PlatoonGroupDef PlatoonGroupDef)
    {
        //id = Id;
        team = Team;
        //squadDef = SquadDef;
        //location = Location;
        positionType = "base";
        pos = null;
        
        platoonGroup = PlatoonGroupDef;
        platoons = new List<PlatoonDef> { PlatoonGroupDef.platoon };
        squads = new List<SquadDef> { };
        platoonMounts = new List<PlatoonDef> { PlatoonGroupDef.mount };
        squadMounts = new List<SquadDef> { };
    }
    
    public MapPlatoon(int Team, string PositionType, PlatoonDef PlatoonDef)
    {
        //id = Id;
        team = Team;
        //squadDef = SquadDef;
        //location = Location;
        positionType = PositionType;
        pos = null;
        
        platoonGroup = null;
        platoons = new List<PlatoonDef> { PlatoonDef };
        squads = new List<SquadDef> { };
        platoonMounts = new List<PlatoonDef> { };
        squadMounts = new List<SquadDef> { };
    }

    public MapPlatoon(int Team, int[] Pos, PlatoonDef PlatoonDef)
    {
        //id = Id;
        team = Team;
        //squadDef = SquadDef;
        //location = Location;
        positionType = "base";
        pos = Pos;
        
        platoonGroup = null;
        platoons = new List<PlatoonDef> { PlatoonDef };
        squads = new List<SquadDef> { };
        platoonMounts = new List<PlatoonDef> { };
        squadMounts = new List<SquadDef> { };
    }

    public MapPlatoon(int Team, string PositionType, PlatoonGroupDef PlatoonGroupDef)
    {
        //id = Id;
        team = Team;
        //squadDef = SquadDef;
        //location = Location;
        positionType = PositionType;
        pos = null;

        platoonGroup = PlatoonGroupDef;
        platoons = new List<PlatoonDef> { PlatoonGroupDef.platoon };
        squads = new List<SquadDef> { };
        platoonMounts = new List<PlatoonDef> { PlatoonGroupDef.mount };
        squadMounts = new List<SquadDef> { };
    }

    public MapPlatoon(int Team, int[] Pos, PlatoonGroupDef PlatoonGroupDef)
    {
        //id = Id;
        team = Team;
        //squadDef = SquadDef;
        //location = Location;
        positionType = "base";
        pos = Pos;

        platoonGroup = PlatoonGroupDef;
        platoons = new List<PlatoonDef> { PlatoonGroupDef.platoon };
        squads = new List<SquadDef> { };
        platoonMounts = new List<PlatoonDef> { PlatoonGroupDef.mount };
        squadMounts = new List<SquadDef> { };
    }

    public MapPlatoon(int Team, string PositionType, int[] Pos, PlatoonDef PlatoonDef)
    {
        //id = Id;
        team = Team;
        //squadDef = SquadDef;
        //location = Location;
        positionType = PositionType;
        pos = Pos;
        
        platoonGroup = null;
        platoons = new List<PlatoonDef> { PlatoonDef };
        squads = new List<SquadDef> { };
        platoonMounts = new List<PlatoonDef> { };
        squadMounts = new List<SquadDef> { };
    }

    public MapPlatoon(int Team, string PositionType, int[] Pos, PlatoonGroupDef PlatoonGroupDef)
    {
        //id = Id;
        team = Team;
        //squadDef = SquadDef;
        //location = Location;
        positionType = PositionType;
        pos = Pos;

        platoonGroup = PlatoonGroupDef;
        platoons = new List<PlatoonDef> { PlatoonGroupDef.platoon };
        squads = new List<SquadDef> { };
        platoonMounts = new List<PlatoonDef> { PlatoonGroupDef.mount };
        squadMounts = new List<SquadDef> { };
    }

    /*public MapPlatoon(int Id, int Team, SquadDef SquadDef, int[] Location)
    {
        id = Id;
        team = Team;
        squadDef = SquadDef;
        location = Location;
    }
    public MapPlatoon(int Id, int Team, SquadDef SquadDef)
    {
        id = Id;
        team = Team;
        squadDef = SquadDef;
        location = null;
    }
    public MapPlatoon(int Team, SquadDef SquadDef)
    {
        id = -1;
        team = Team;
        squadDef = SquadDef;
        location = null;
    }
    public MapPlatoon(SquadDef SquadDef, int[] Location)
    {
        id = -1;
        team = -1;
        squadDef = SquadDef;
        location = Location;
    }
    public MapPlatoon(SquadDef SquadDef)
    {
        id = -1;
        team = -1;
        squadDef = SquadDef;
        location = null;
    }*/

    public MapPlatoon MainPlatoon() {
        return new MapPlatoon {
            team = team,
            pos = pos,
            platoonGroup = null,
            platoons = platoons,
            squads = squads,
            platoonMounts = new List<PlatoonDef> { },
            squadMounts = new List<SquadDef> { },
        };
    }

    public MapPlatoon TransportPlatoon() {
        return new MapPlatoon {
            team = team,
            pos = pos,
            platoonGroup = null,
            platoons = platoonMounts,
            squads = squadMounts,
            platoonMounts = new List<PlatoonDef> { },
            squadMounts = new List<SquadDef> { },
        };
    }

    public PlatoonGroupDef PlatoonGroupDef() {
        return platoonGroup;
    }

    public List<PlatoonDef> PlatoonDefs() {
        return platoons.Concat(platoonMounts).ToList();
    }

    public List<SquadDef> SquadDefs() {

        List<SquadDef> squadDefs = new List<SquadDef> { };

        foreach (PlatoonDef platoon in platoons) {
            foreach (SquadDef squad in platoon.squadDefs) {
                squadDefs.Add(squad);
            }
        }   
        foreach (SquadDef squad in squads) {
            squadDefs.Add(squad);
        }
        foreach (PlatoonDef platoon in platoonMounts) {
            foreach (SquadDef squad in platoon.squadDefs) {
                squadDefs.Add(squad);
            }
        }
        foreach (SquadDef squad in squadMounts) {
            squadDefs.Add(squad);
        }

        return squadDefs;

    }

    public List<SquadDef> CoreSquadDefs() {

        List<SquadDef> squadDefs = new List<SquadDef> { };

        foreach (PlatoonDef platoon in platoons) {
            foreach (SquadDef squad in platoon.squadDefs) {
                squadDefs.Add(squad);
            }
        }   
        foreach (PlatoonDef platoon in platoonMounts) {
            foreach (SquadDef squad in platoon.squadDefs) {
                squadDefs.Add(squad);
            }
        }

        return squadDefs;

    }

}

public struct MapFort
{
    public int id;
    public FortDef fortDef;
    public int stage;

    /*public MapFort(int Id, FortDef FortDef, int Stage)
    {
        id = Id;
        //team = Team;
        fortDef = FortDef;
        stage = Stage;
    }*/
    public MapFort(FortDef FortDef, int Stage)
    {
        id = -1;
        fortDef = FortDef;
        stage = Stage;
    }
}