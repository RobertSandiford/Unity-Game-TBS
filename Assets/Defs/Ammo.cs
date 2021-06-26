using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo {
    public string key;
    public string name;

    public Ammo (string Key, string Name) {
        key = Key;
        name = Name;
    }
}

public static class AmmoDef {

    public static Ammo Stanag = new Ammo("stanag", "5.56mm Stanag");
    public static Ammo M249 = new Ammo("m249", "5.56mm M249");
    public static Ammo M240 = new Ammo("m240", "7.62mm M240");
    public static Ammo M22 = new Ammo("m2", "12.7mm M2");
    public static Ammo M203 = new Ammo("m203", "40mm M203");
    public static Ammo Mk19 = new Ammo("mk19", "40mm Mk19");
    public static Ammo AT4 = new Ammo("at4", "AT4");
    public static Ammo Jav = new Ammo("jav", "Javelin");
    public static Ammo Tow = new Ammo("tow", "Tow");

    public static Ammo Stinger = new Ammo("stinger", "Stinger");

    public static Ammo _545 = new Ammo("545", "5.45mm Mag");
    public static Ammo PK = new Ammo("pk", "7.62mm PK");
    public static Ammo RU50 = new Ammo("ru50", "12.7mm HMG");
    public static Ammo GP25 = new Ammo("gp25", "40mm GP-25");
    public static Ammo AGS = new Ammo("ags", "30mm AGS");
    public static Ammo RPG22 = new Ammo("rpg22", "RPG-22");
    public static Ammo RPG26 = new Ammo("rpg26", "RPG-26");
    public static Ammo RPG7 = new Ammo("rpg7", "RPG-7");
    public static Ammo RPOA = new Ammo("rpoa", "RPO-A");
    public static Ammo Metis = new Ammo("metis", "Metis");
    public static Ammo MetisM = new Ammo("metism", "Metis-M");
    public static Ammo Konkurs = new Ammo("konkurs", "Konkurs");
    public static Ammo KonkursM = new Ammo("konkursm", "Konkurs-M");
    public static Ammo Kornet = new Ammo("kornet", "Kornet");
    public static Ammo KornetM = new Ammo("kornetm", "Kornet-M");
    
    public static Ammo Strela3 = new Ammo("strela3", "Strela-1");
    public static Ammo Igla1 = new Ammo("igla1", "Igla-1");
    public static Ammo Igla = new Ammo("igla", "Igla");
    public static Ammo IglaS = new Ammo("iglas", "Igla-S");

}
