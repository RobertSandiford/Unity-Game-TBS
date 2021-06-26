using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platoon
{
    public List<Squad> squads;

    Platoon(List<Squad> Squads) {
        squads = Squads;
    }
}
