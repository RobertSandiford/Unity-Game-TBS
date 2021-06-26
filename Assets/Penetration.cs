using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class Penetration {

    private static System.Random random = new System.Random();

    public static bool Resolve(double penetration, double armor)
    {
        double penChance = PenChance(penetration, armor);
        double ran = Lib.random.NextDouble();
        return (ran < penChance);
    }

    public static double PenChance(double penetration, double armor)
    {
        double penetrationFactor = penetration / armor;
        double penChance = 0.5;

        if (penetrationFactor <= 1)
        {
            penChance = penChance - (1 - penetrationFactor); // reduce penetration chance by distance under 1 linearly. Pen chance == 0 when factor = 0.5
            penChance = Math.Max(0.0, penChance);
        }

        if (penetrationFactor > 1)
        {
            double coef = 2.2;
            penChance = 1 / ( 1 + Math.Exp( (penetrationFactor -1) * -1 * coef ) );
            penChance = Math.Min(1.0, penChance);
        }

        //Debug.Log("Pen Factor:" + penetrationFactor);
        //Debug.Log("Pen Chance:" + penChance);

        return penChance;
    }
    
}
