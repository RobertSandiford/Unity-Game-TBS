using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class DamageProfile
{
    public int shots;
    public double hitChance;
    public int damage;
    public int minDamage;
    public int maxDamage;
    public double damageChance;
    //public int minRange;
    public double rangeCoefficient;
    public WeaponType weaponType;

    public DamageProfile()
    {
        damageChance = 1.0;
        rangeCoefficient = 1.0;
        weaponType = WeaponType.Bullet;
    }

    public DamageProfile(int Damage)
    {
        damage = Damage;
        damageChance = 1.0;
        rangeCoefficient = 1.0;
        weaponType = WeaponType.Bullet;
    }
    public DamageProfile(int MinDamage, int MaxDamage)
    {
        minDamage = MinDamage;
        maxDamage = MaxDamage;
        damageChance = 1.0;
        rangeCoefficient = 1.0;
        weaponType = WeaponType.Bullet;
    }

    public DamageProfile(int Shots, double HitChance, int MinDamage, int MaxDamage, double DamageChance = 1.0, double RangeCoefficient = 1.0)
    {
        shots = Shots;
        hitChance = HitChance;
        minDamage = MinDamage;
        maxDamage = MaxDamage;
        damageChance = DamageChance;
        rangeCoefficient = RangeCoefficient;
        weaponType = WeaponType.Bullet;
    }

    public DamageProfile(int Shots, double HitChance, int Damage, double DamageChance = 1.0)
    {
        shots = Shots;
        hitChance = HitChance;
        damage = Damage;
        damageChance = DamageChance;
        rangeCoefficient = 1.0;
        weaponType = WeaponType.Bullet;
    }

    public DamageProfile(int Shots, double HitChance, int Damage, double DamageChance, double RangeCoefficient)
    {
        shots = Shots;
        hitChance = HitChance;
        damage = Damage;
        damageChance = DamageChance;
        rangeCoefficient = RangeCoefficient;
        weaponType = WeaponType.Bullet;
    }

    public DamageProfile(Dictionary<string, double> data)
    {
        shots = (int)data["shots"];
        hitChance = (int)data["hitChance"];
        damage = (int)data["damage"];
        damageChance = (int)data["damageChance"];
        weaponType = (WeaponType)data["weaponType"];
    }

    /*public double GetHitChanceForRange(int range, Weapon weapon, double hitability, bool moving = false)
    {
        int additionalRange = Math.Max(0, range - 1);
        //double internalCoefficient = 2.0;
        //return Math.Pow( hitChance, 1 + (additionalRange / internalCoefficient / rangeCoefficient) );

        double internalCoefficient = 1.0;

        //double rangeBaseNumber = 0.7;
        //double rangeBaseNumber = 0.8;
        double rangeBaseNumber = Math.Pow(0.8, 1.0 / (125.0 / MapDefs.hexWidth) );

        //Debug.Log("add range " + additionalRange);
        //Debug.Log("ic " + internalCoefficient);
        //Debug.Log("rc " + rangeCoefficient);
        //Debug.Log((additionalRange > 0) ? (additionalRange / internalCoefficient / rangeCoefficient) : 0);

        double movingMultiplier = (moving) ? weapon.movingAccuracy : 1.0;

        double baseAcc = (hitChance * 1.1) // fudge for distance change 
            / Math.Pow(rangeBaseNumber, 1.0 / internalCoefficient / rangeCoefficient);

        //double baseAccuracy = hitChance / rangeBaseNumber; // hitChance / rangeBaseNumber * rangeBaseNumber gives us hitChance accuracy at tile distance 1
        //return hitChance * Math.Pow(rangeBaseNumber, range / internalCoefficient / rangeCoefficient) * hitability * movingMultiplier;
        return baseAcc * Math.Pow(rangeBaseNumber, range / internalCoefficient / rangeCoefficient) * hitability * movingMultiplier;
    }*/
    
    public double GetHitChanceForRange(int range, Weapon weapon, double hitability, double portionMovementUsed = 0.0)
    {
        int additionalRange = Math.Max(0, range - 1);
        //double internalCoefficient = 2.0;
        //return Math.Pow( hitChance, 1 + (additionalRange / internalCoefficient / rangeCoefficient) );

        double internalCoefficient = 1.0;

        //double rangeBaseNumber = 0.7;
        //double rangeBaseNumber = 0.8;
        double rangeBaseNumber = Math.Pow(0.8, 1.0 / (125.0 / MapDefs.hexWidth) );

        //Debug.Log("add range " + additionalRange);
        //Debug.Log("ic " + internalCoefficient);
        //Debug.Log("rc " + rangeCoefficient);
        //Debug.Log((additionalRange > 0) ? (additionalRange / internalCoefficient / rangeCoefficient) : 0);

        //double movingMultiplier = (moving) ? weapon.movingAccuracy : 1.0;
        double movingMultiplier = GetMovedAccuracy(portionMovementUsed);

        //Debug.Log("Moving accuracy: " + movingMultiplier.ToString());

        double baseAcc = (hitChance * 1.1) /* fudge for distance change */ / Math.Pow(rangeBaseNumber, 1.0 / internalCoefficient / rangeCoefficient);

        //double baseAccuracy = hitChance / rangeBaseNumber; // hitChance / rangeBaseNumber * rangeBaseNumber gives us hitChance accuracy at tile distance 1
        //return hitChance * Math.Pow(rangeBaseNumber, range / internalCoefficient / rangeCoefficient) * hitability * movingMultiplier;
        return baseAcc * Math.Pow(rangeBaseNumber, range / internalCoefficient / rangeCoefficient) * hitability * movingMultiplier;
    }
    /*public double GetDamageChanceForRange(int range)
    {
        return Math.Pow(damageChance, range / damageRangeCoefficient);
    }*/

    public double GetMovedAccuracy(double portionMovementUsed) {
        return 0.2 + 0.8 * (1.0-portionMovementUsed);
    }
    
    public double AverageDamagePerShot()
    {
        if (maxDamage > 0)
        {
            return (minDamage + maxDamage) / 2.0;
        }
        return (double)damage;
    }

    public int GetRandomisedDamage()
    {
        if (maxDamage > 0)
        {
            Dictionary<int, int> damages = new Dictionary<int, int>();
            int sum = 0;
            for (int d = minDamage; d <= maxDamage; d++)
            {
                int likelyhood = 1 + Math.Min(d - minDamage, maxDamage - d);
                damages[d] = likelyhood;
                sum += likelyhood;
            }

            int ran = Lib.random.Next(1, sum + 1);
            int cum = 0;
            int damage = 0;
            foreach (KeyValuePair<int, int> kvp in damages)
            {
                cum += kvp.Value;
                if (ran <= cum)
                {
                    damage = kvp.Key;
                    break;
                }
            }
            Debug.Log("Random damage calculated as " + damage);
            // damageProbablities = 
            return damage;
        }
        return damage;
    }

    /*public double GetAverageDamage(int range, Weapon weapon)
    {
        return shots * GetHitChanceForRange(range, weapon) * AverageDamagePerShot() * damageChance; // countermeasures
    }*/

    public double GetAverageDamage(Unit targetUnit, UnitWeapon uWeapon, int range, Terrain terrain, Countermeasures countermeasures, double portionMovementUsed = 0.0)
    {
        return uWeapon.number * GetAverageDamagePerWeapon(targetUnit, uWeapon, range, terrain, countermeasures, portionMovementUsed);
    }

    public double GetAverageDamagePerWeapon(Unit targetUnit, UnitWeapon uWeapon, int range, Terrain terrain, Countermeasures countermeasures, double portionMovementUsed = 0.0)
    {
        Weapon weapon = uWeapon.weapon;

        double hitability = targetUnit.Hitability();
        double fortMulti = (targetUnit.tile.fort != null) ? 1.0 - targetUnit.tile.fort.stage.cover : 1.0;

        double a = shots;
        double b = GetHitChanceForRange(range, weapon, hitability, portionMovementUsed);
        double c = (1 - terrain.GetCover(targetUnit.UnitTargetType()));
        double d = AverageDamagePerShot();
        double e = damageChance;
        double f = ((targetUnit.Armor() == 0) ? 1.0 : Penetration.PenChance(uWeapon.weapon.penetration, targetUnit.Armor()));
        double g = (1 - countermeasures.GetCountermeasureStrength(weaponType));
        double h = fortMulti;

        return (
            shots
            * GetHitChanceForRange(range, weapon, hitability, portionMovementUsed)
            * (1 - terrain.GetCover( targetUnit.UnitTargetType() ))
            * AverageDamagePerShot()
            * damageChance
            * ((targetUnit.Armor() == 0) ? 1.0 : Penetration.PenChance(uWeapon.weapon.penetration, targetUnit.Armor() ))
            * (1 - countermeasures.GetCountermeasureStrength(weaponType))
            * fortMulti
        );
    }

    public double GetAverageDamage(Unit targetUnit, UnitWeapon uWeapon, int range, Terrain terrain, Countermeasures countermeasures, double portionMovementUsed, Fort fort)
    {
        return GetAverageDamage(targetUnit, uWeapon, range, terrain, countermeasures, portionMovementUsed);
        //return uWeapon.number * shots * GetHitChanceForRange(range) * (1 - terrain.GetCover(targetUnit.unitDef.targetType)) * AverageDamagePerShot() * damageChance * ((targetUnit.unitDef.armor == 0) ? 1.0 : Penetration.PenChance(uWeapon.weapon.penetration, targetUnit.unitDef.armor)) * (1 - countermeasures.GetCountermeasureStrength(weaponType)) * (1 - fort.stage.cover); // countermeasures
    }
    
    public double GetMaxDamage(UnitWeapon uWeapon)
    {
        return uWeapon.number * shots * ((maxDamage > 0) ? maxDamage : damage);
    }

    /*public List<int> CalcDamage(int range, Terrain terrain, UnitWeapon uWeapon, Unit targetUnit, bool moving = false)
    {
        return CalcDamage(range, terrain, uWeapon, uWeapon.weapon, targetUnit, moving);
    }*/
    public List<int> CalcDamage(int range, Terrain terrain, UnitWeapon uWeapon, Unit targetUnit, double portionMovementUsed = 0.0)
    {
        return CalcDamage(range, terrain, uWeapon, uWeapon.weapon, targetUnit, portionMovementUsed);
    }
    
    /*public List<int> CalcDamage(int range, Terrain terrain, UnitWeapon uWeapon, Weapon weapon, Unit targetUnit, bool moving = false)
    {
        //UnitDef targetUnitDef = targetUnit.unitDef;

        System.Random random = Lib.random;
        List<int> hits = new List<int>();
        
        //double hitChance = GetHitChanceForRange(range, weapon, targetUnitDef.hitability, moving);
        double hitChance = GetHitChanceForRange(range, weapon, targetUnit.Hitability(), moving);
        for (int shot = 0; shot < (shots * uWeapon.number); shot++)
        {
            Debug.Log("<color>Doing Shot.</color>");
            double ran = random.NextDouble();
            if (ran <= hitChance)
            {
                //if ((new WeaponType[] { WeaponType.Rocket, WeaponType.Wire, WeaponType.Radio, WeaponType.Thermal }).Contains(weapon.weaponType))
                if (true)
                {
                    double terrainCover = terrain.GetCover( targetUnit.UnitTargetType() );
                    if (terrainCover == 0.0 || Lib.random.NextDouble() >= terrainCover)
                    {
                        double countermeasureStrength = 0.0;

                        switch (weapon.weaponType)
                        {
                            case WeaponType.Rocket:
                                countermeasureStrength = targetUnit.Countermeasures().rocket;
                                break;
                            case WeaponType.Wire:
                                countermeasureStrength = targetUnit.Countermeasures().wire;
                                break;
                            case WeaponType.Radio:
                                countermeasureStrength = targetUnit.Countermeasures().radio;
                                break;
                            case WeaponType.Thermal:
                                countermeasureStrength = targetUnit.Countermeasures().thermal;
                                break;
                        }
                        double ran2 = random.NextDouble();

                        if (countermeasureStrength == 0.0 || ran2 >= countermeasureStrength)
                        {

                            double fortCover = (targetUnit.tile.fort != null)
                                ? targetUnit.tile.fort.stage.cover
                                : 0.0;
                            double ran3 = random.NextDouble();

                            if (fortCover == 0.0 || ran3 >= fortCover)
                            {
                                double ran4 = random.NextDouble();
                                if (ran4 <= damageChance)
                                {
                                    double penChance = 1.0;
                                    if (targetUnit.Armor() > 0) penChance = Penetration.PenChance(weapon.penetration, targetUnit.Armor());

                                    double ran5 = random.NextDouble();
                                    if (penChance == 1.0 || ran5 < penChance)
                                    {
                                        int hitDamage = GetRandomisedDamage();
                                        Debug.Log("<color=red>Hits for " + hitDamage.ToString() + " damage!</color>");
                                        targetUnit.EventText("Hit " + hitDamage.ToString());
                                        hits.Add(hitDamage);
                                    }
                                    else
                                    {
                                        Debug.Log("<color=red>Failed to penetrate armor!</color>");
                                        targetUnit.EventText("Bounced!");
                                    }
                                }
                                else
                                {
                                    Debug.Log("<color=red>Hit but caused no damage!</color>");
                                    targetUnit.EventText("Ineffective!");
                                }
                            }
                            else
                            {
                                Debug.Log("<color=red>Shot blocked by fortification!</color>");
                                targetUnit.EventText("Hit Cover!");
                            }
                        }
                        else
                        {
                            Debug.Log("<color=red>Shot blocked by countermeasures!</color>");
                            targetUnit.EventText("Defeated!");
                        }
                    }
                    else
                    {
                        Debug.Log("<color=red>Shot blocked by terrain!</color>");
                        targetUnit.EventText("Hit Terrain!");
                    }
                }
                //else
                //{
                //    double ran3 = random.NextDouble();
                //    if (ran3 <= damageChance)
                //    {
                //        Debug.Log("<color=red>Hits for " + damage.ToString() + " damage!</color>");
                //        targetUnit.EventText("Hit " + damage.ToString());
                //        hits.Add(damage);
                //    }
                //    else
                //    {
                //        Debug.Log("<color=red>Hit but caused no damage!</color>");
                //    }
                //}

            }
            else
            {
                Debug.Log("<color=red>Shot misses!</color>");
                //targetUnit.EventText("Miss");
            }
        }

        return hits;
    }*/
    
    public List<int> CalcDamage(int range, Terrain terrain, UnitWeapon uWeapon, Weapon weapon, Unit targetUnit, double portionMovementUsed = 0.0)
    {
        //UnitDef targetUnitDef = targetUnit.unitDef;

        System.Random random = Lib.random;
        List<int> hits = new List<int>();
        
        //double hitChance = GetHitChanceForRange(range, weapon, targetUnitDef.hitability, moving);
        double hitChance = GetHitChanceForRange(range, weapon, targetUnit.Hitability(), portionMovementUsed);
        for (int shot = 0; shot < (shots * uWeapon.number); shot++)
        {
            Debug.Log("<color>Doing Shot.</color>");
            double ran = random.NextDouble();
            if (ran <= hitChance)
            {
                //if ((new WeaponType[] { WeaponType.Rocket, WeaponType.Wire, WeaponType.Radio, WeaponType.Thermal }).Contains(weapon.weaponType))
                if (true)
                {
                    double terrainCover = terrain.GetCover( targetUnit.UnitTargetType() );
                    if (terrainCover == 0.0 || Lib.random.NextDouble() >= terrainCover)
                    {
                        double countermeasureStrength = 0.0;

                        switch (weapon.weaponType)
                        {
                            case WeaponType.Rocket:
                                countermeasureStrength = targetUnit.Countermeasures().rocket;
                                break;
                            case WeaponType.Wire:
                                countermeasureStrength = targetUnit.Countermeasures().wire;
                                break;
                            case WeaponType.Radio:
                                countermeasureStrength = targetUnit.Countermeasures().radio;
                                break;
                            case WeaponType.Thermal:
                                countermeasureStrength = targetUnit.Countermeasures().thermal;
                                break;
                        }
                        double ran2 = random.NextDouble();

                        if (countermeasureStrength == 0.0 || ran2 >= countermeasureStrength)
                        {

                            double fortCover = (targetUnit.tile.fort != null)
                                ? targetUnit.tile.fort.stage.cover
                                : 0.0;
                            double ran3 = random.NextDouble();

                            if (fortCover == 0.0 || ran3 >= fortCover)
                            {
                                double ran4 = random.NextDouble();
                                if (ran4 <= damageChance)
                                {
                                    double penChance = 1.0;
                                    if (targetUnit.Armor() > 0) penChance = Penetration.PenChance(weapon.penetration, targetUnit.Armor());

                                    double ran5 = random.NextDouble();
                                    if (penChance == 1.0 || ran5 < penChance)
                                    {
                                        int hitDamage = GetRandomisedDamage();
                                        Debug.Log("<color=red>Hits for " + hitDamage.ToString() + " damage!</color>");
                                        targetUnit.EventText("Hit " + hitDamage.ToString());
                                        hits.Add(hitDamage);
                                    }
                                    else
                                    {
                                        Debug.Log("<color=red>Failed to penetrate armor!</color>");
                                        targetUnit.EventText("Bounced!");
                                    }
                                }
                                else
                                {
                                    Debug.Log("<color=red>Hit but caused no damage!</color>");
                                    targetUnit.EventText("Ineffective!");
                                }
                            }
                            else
                            {
                                Debug.Log("<color=red>Shot blocked by fortification!</color>");
                                targetUnit.EventText("Hit Cover!");
                            }
                        }
                        else
                        {
                            Debug.Log("<color=red>Shot blocked by countermeasures!</color>");
                            targetUnit.EventText("Defeated!");
                        }
                    }
                    else
                    {
                        Debug.Log("<color=red>Shot blocked by terrain!</color>");
                        targetUnit.EventText("Hit Terrain!");
                    }
                }
                /*else
                {
                    double ran3 = random.NextDouble();
                    if (ran3 <= damageChance)
                    {
                        Debug.Log("<color=red>Hits for " + damage.ToString() + " damage!</color>");
                        targetUnit.EventText("Hit " + damage.ToString());
                        hits.Add(damage);
                    }
                    else
                    {
                        Debug.Log("<color=red>Hit but caused no damage!</color>");
                    }
                }*/

            }
            else
            {
                Debug.Log("<color=red>Shot misses!</color>");
                //targetUnit.EventText("Miss");
            }
        }

        return hits;
    }

    /*public void DoHits(UnitWeapon uWeapon, Unit targetUnit, int range, bool moving = false)
    {
        DoHits(uWeapon, uWeapon.weapon, targetUnit, range, moving);
    }*/
    public void DoHits(UnitWeapon uWeapon, Unit targetUnit, int range, double portionMovementUsed = 0.0)
    {
        DoHits(uWeapon, uWeapon.weapon, targetUnit, range, portionMovementUsed);
    }

    /*public void DoHits(UnitWeapon uWeapon, Weapon weapon, Unit targetUnit, int range, bool moving = false)
    {

        Debug.Log("<color=blue>Firing weapon " + weapon.name + " at " + targetUnit.name + "</color>");

        List<int> hits = CalcDamage(range, targetUnit.tile.terrain, uWeapon, weapon, targetUnit, moving);

        int totalDamage = 0;
        foreach (int hit in hits)
        {
            totalDamage += hit;
            //Debug.Log("<color=red>Hit: " + hit.ToString() +  " damage</color>");
        }
        Debug.Log("<color=red>Total damage " + totalDamage.ToString() + "</color>");

        targetUnit.TakeDamage(totalDamage);
    }*/

    public void DoHits(UnitWeapon uWeapon, Weapon weapon, Unit targetUnit, int range, double portionMovementUsed = 0.0)
    {

        Debug.Log("<color=blue>Firing weapon " + weapon.name + " at " + targetUnit.name + "</color>");

        List<int> hits = CalcDamage(range, targetUnit.tile.terrain, uWeapon, weapon, targetUnit, portionMovementUsed);

        int totalDamage = 0;
        foreach (int hit in hits)
        {
            totalDamage += hit;
            //Debug.Log("<color=red>Hit: " + hit.ToString() +  " damage</color>");
        }
        Debug.Log("<color=red>Total damage " + totalDamage.ToString() + "</color>");

        targetUnit.TakeDamage(totalDamage);
    }
}

public enum WeaponType
{
    Bullet,
    Shell,
    Grenade,
    LaunchedGrenade,
    Rocket,
    Mortar,
    Artillery,
    // cannon arty vs how arty?
    Wire,
    Laser,
    Radio,
    Thermal,

    //Air
    Infra,
    AirRadar,
}

public class Weapon
{
    public string id { get; set; }
    public string name { get; set; }

    private string _shortName;
    public string shortName
    { 
        get { 
            if (_shortName != "") return shortName;
            return name;
        }
        set { _shortName = value; }
    }

    public int range { get; set; }
    public int displayRange = 0;
    public int minRange = 0;
    public int shots = 0;
    public double accuracy = 0.0;
    public double rfactor = 1.0;
    public int ceiling = 0;
    public int maxLaunchAlt = 0;
    public int soldiersToFire = 1;
    public double movingAccuracy = 0.5;
    public int setupTime = 0;
    public int packupTime = 0;
    public bool direct = true;
    public bool indirect = false;
    public int impactDelay { get; set; }
    public int aimTime = 0;
    public int flightTime = 0;
    public int artyShellsPerSalvo = 1;
    public double artyDelayPerShell = 0.0;
    public Dictionary<TargetType, DamageProfile> damageProfiles { get; set; }
    public int penetration;
    public WeaponType weaponType { get; set; } = WeaponType.Bullet;
    public SoundProfile soundProfile { get; set; }
    public ShootProfile shootProfile { get; set; }
    public ShootProfile impactProfile { get; set; }
    public Weapon indirectWeapon;

    public Weapon()
    {

    }

    public Weapon(string Id, string Name, int Range)
    {
        id = Id;
        name = Name;
        range = Range;
        damageProfiles = new Dictionary<TargetType, DamageProfile>();
        weaponType = WeaponType.Bullet;
        soundProfile = new SoundProfile(SoundLib.rifleShot, 3, 85, 0.15, 1);
    }

    public Weapon(string Id, string Name, int Range, int realRange)
    {
        id = Id;
        name = Name;
        range = Range;
        if (realRange > 0)
        {
            range = (int)Math.Round((double)((realRange / 100) / 5 * 3));
        }
        damageProfiles = new Dictionary<TargetType, DamageProfile>();
        weaponType = WeaponType.Bullet;
        soundProfile = new SoundProfile(SoundLib.rifleShot, 3, 85, 0.15, 1);
    }

    public Weapon(string Id, string Name, int Range, Dictionary<TargetType, DamageProfile> DamageProfiles)
    {
        id = Id;
        name = Name;
        range = Range;
        damageProfiles = DamageProfiles;
        weaponType = WeaponType.Bullet;
        soundProfile = new SoundProfile(SoundLib.rifleShot, 3, 85, 0.15, 1);
    }

    public Weapon(string Id, string Name, int Range, Dictionary<TargetType, DamageProfile> DamageProfiles, SoundProfile SoundProfile)
    {
        id = Id;
        name = Name;
        range = Range;
        damageProfiles = DamageProfiles;
        weaponType = WeaponType.Bullet;
        soundProfile = SoundProfile;
    }

    public Weapon(string Id, string Name, int Range, int realRange, Dictionary<TargetType, DamageProfile> DamageProfiles)
    {
        id = Id;
        name = Name;
        range = Range;
        if (realRange > 0)
        {
            range = (int)Math.Round((double)((realRange / 100) / 5 * 3));
        }
        damageProfiles = DamageProfiles;
        weaponType = WeaponType.Bullet;
        soundProfile = new SoundProfile(SoundLib.rifleShot, 3, 85, 0.15, 1);
    }

    public Weapon(string Id, string Name, int Range, int realRange, Dictionary<TargetType, DamageProfile> DamageProfiles, SoundProfile SoundProfile)
    {
        id = Id;
        name = Name;
        range = Range;
        if (realRange > 0)
        {
            range = (int)Math.Round((double)((realRange / 100) / 5 * 3));
        }
        damageProfiles = DamageProfiles;
        weaponType = WeaponType.Bullet;
        soundProfile = SoundProfile;
    }

    public Weapon(string Id, string Name, int Range, Dictionary<TargetType, DamageProfile> DamageProfiles, WeaponType WeaponType)
    {
        id = Id;
        name = Name;
        range = Range;
        damageProfiles = DamageProfiles;
        weaponType = WeaponType;
        soundProfile = new SoundProfile(SoundLib.rifleShot, 3, 85, 0.15, 1);
    }

    public Weapon(string Id, string Name, int Range, Dictionary<TargetType, DamageProfile> DamageProfiles, WeaponType WeaponType, SoundProfile SoundProfile)
    {
        id = Id;
        name = Name;
        range = Range;
        damageProfiles = DamageProfiles;
        weaponType = WeaponType;
        soundProfile = SoundProfile;
    }

    public Weapon(string Id, string Name, int Range, int realRange, Dictionary<TargetType, DamageProfile> DamageProfiles, WeaponType WeaponType)
    {
        id = Id;
        name = Name;
        range = Range;
        if (realRange > 0)
        {
            range = (int)Math.Round((double)((realRange / 100) / 5 * 3));
        }
        damageProfiles = DamageProfiles;
        weaponType = WeaponType;
        soundProfile = new SoundProfile(SoundLib.rifleShot, 3, 85, 0.15, 1);
    }

    public static int RealRange(int realRange)
    {
        //Debug.Log("Calced Range : " + ((int)Math.Round((double)realRange / 100.0 / 5.0 * 3.0)).ToString());
        //return (int)Math.Round((double)realRange / 100.0 / 5.0 * 3.0); // 166m
        return (int)Math.Round((double)realRange / MapDefs.hexWidth ); // 100m
    }
    public static int MissileCeiling(int realCeiling)
    {
        //Debug.Log("Calced Range : " + ((int)Math.Round((double)realRange / 100.0 / 5.0 * 3.0)).ToString());
        //return (int)Math.Round((double)realCeiling / 100.0 / 5.0 * 3.0 / 0.4 /* 0.4 == YMultiplier!! );
        //return (int)Math.Round((double)realCeiling / 100.0 / 5.0 * 3.0 / 0.4 /* 0.4 == YMultiplier!! *);
        return RealAltitude(realCeiling);
    }
    public static int RealAltitude(int alt)
    {
        //Debug.Log("Calced Range : " + ((int)Math.Round((double)realRange / 100.0 / 5.0 * 3.0)).ToString());
        //return (int)Math.Round((double)alt / 100.0 / 5.0 * 3.0 / 0.4 /* 0.4 == YMultiplier!! */);
        return (int)Math.Round((double)alt / MapDefs.hexHeight);
    }

}


public class UnitWeapon
{
    public int number;
    public Weapon weapon;
    public int maxAmmo;
    public int ammo;
    public int setupProgress;
    public int packupProgress;
    public bool isSettingUp = false;
    public bool isPackingUp = false;
    public bool isSetUp = false;
    public bool isPackedUp = true;

    public bool removable = false; // can be not fired when firing too many weapons
    public int soldiersToFire = 1; // how many soldiers are consumed to fire this weapon

    public UnitWeapon(int Number, Weapon Weapon, int Ammo)
    {
        number = Number;
        weapon = Weapon;
        maxAmmo = Ammo;
        ammo = Ammo;
    }

    public UnitWeapon(int Number, Weapon Weapon)
    {
        number = Number;
        weapon = Weapon;
        maxAmmo = -1;
        ammo = -1;
    }

}

public class WeaponGroup
{
    public int[] weapons;
    public WeaponGroup(int[] Weapons)
    {
        weapons = Weapons;
    }
}