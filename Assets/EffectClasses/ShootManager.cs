using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShootProfile
{
    public Clip clip;
    public int repetitions;
    public double aimMin = 0.0;
    public double aimMax = 0.8;
    public int burstMin = 0;
    public int burstMax = 0;
    public double burstDelayMin = 0.0;
    public double burstDelayMax = 0.0;
    public double rpm;
    public double flux;
    public int shooters = 1;
    public bool projectile = false;
    public GameObject shellObj;
    public float speed = 0f;

    public double speedExponent = 0.85;
    public float yMultiplier= 0.7f;
    
    public ShootProfile() {
        /*repetitions = 1;
        burstMin = 0;
        burstMax = 0;
        burstDelayMin = 0.0;
        burstDelayMax = 0.0;
        rpm = 0.0;
        flux = 0.0;
        shooters = 1;
        projectile = false;
        speed = 0.0f;
        speedExponent = 0.85;
        yMultiplier = 0.7f;*/
    }

    public ShootProfile(ShootProfile SP) {
        clip = SP.clip;
        repetitions = SP.repetitions;
        aimMin = SP.aimMax;
        aimMax = SP.aimMax;
        burstMin = SP.burstMin;
        burstMax = SP.burstMax;
        burstDelayMin = SP.burstDelayMin;
        burstDelayMax = SP.burstDelayMax;
        rpm = SP.rpm;
        flux = SP.flux;
        shooters = SP.shooters;
        projectile = SP.projectile;
        shellObj = SP.shellObj;
        speed = SP.speed;

        speedExponent = SP.speedExponent;
        yMultiplier= SP.yMultiplier;
    }

    public ShootProfile(Clip Clip, bool Projectile = true, GameObject ShellObj = null, float Speed = 0, double SpeedExponent = 0.85, float YMultiplier = 0.7f)
    {
        clip = Clip;
        repetitions = 1;
        burstMin = 0;
        burstMax = 0;
        burstDelayMin = 0.0;
        burstDelayMax = 0.0;
        rpm = 1.0;
        flux = 0.0;
        shooters = 1;
        projectile = Projectile;
        shellObj = ShellObj;
        speed = Speed * 100f;

        speedExponent = SpeedExponent;
        yMultiplier = YMultiplier;

    }

    public ShootProfile(Clip Clip, int Repititions, double Rpm, double Flux, int Shooters, bool Projectile = true, GameObject ShellObj = null, float Speed = 0, double SpeedExponent = 0.85, float YMultiplier = 0.7f)
    {
        clip = Clip;
        repetitions = Repititions;
        burstMin = 0;
        burstMax = 0;
        burstDelayMin = 0.0;
        burstDelayMax = 0.0;
        rpm = Rpm;
        flux = Flux;
        shooters = Shooters;
        projectile = Projectile;
        shellObj = ShellObj;
        speed = Speed * 100f;

        speedExponent = SpeedExponent;
        yMultiplier = YMultiplier;

    }

    public ShootProfile(Clip Clip, int Repititions, int BurstMin, int BurstMax, double BurstDelayMin, double BurstDelayMax, double Rpm, double Flux, int Shooters, bool Projectile = true, GameObject ShellObj = null, float Speed = 0, double SpeedExponent = 0.85, float YMultiplier = 0.7f)
    {
        clip = Clip;
        repetitions = Repititions;
        burstMin = BurstMin;
        burstMax = BurstMax;
        burstDelayMin = BurstDelayMin;
        burstDelayMax = BurstDelayMax;
        rpm = Rpm;
        flux = Flux;
        shooters = Shooters;
        projectile = Projectile;
        shellObj = ShellObj;
        speed = Speed * 100f;

        speedExponent = SpeedExponent;
        yMultiplier = YMultiplier;

    }

    public ShootProfile(SoundProfile soundProfile, bool Projectile = true, GameObject ShellObj = null, float Speed = 0, double SpeedExponent = 0.85, float YMultiplier = 0.7f)
    {
        clip = soundProfile.clip;
        repetitions = soundProfile.repetitions;
        burstMin = 0;
        burstMax = 0;
        burstDelayMin = 0.0;
        burstDelayMax = 0.0;
        rpm = soundProfile.rpm;
        flux = soundProfile.flux;
        shooters = soundProfile.shooters;
        projectile = Projectile;
        shellObj = ShellObj;
        speed = Speed * 100f;

        speedExponent = SpeedExponent;
        yMultiplier = YMultiplier;
    }

 

    /*public ShootProfile(bool ignore)
    {
        clip = null;
        repetitions = 1;
        rpm = 1.0;
        flux = 0.0;
        shooters = 1;
    }*/
}

public class ShootEffect
{
    public AudioSource source;
    public Clip clip;
    public int repetitions; 
    public double aimMin;
    public double aimMax;
    public int burstMin;
    public int burstMax;
    public double burstDelayMin;
    public double burstDelayMax;
    public double rpm;
    public double flux;
    public Vector3 startPos;
    public Vector3 endPos;
    public float speed;
    public ShootProfile shootProfile;

    public double startTime;
    public double lastPlay;
    public double nextPlayTime;

    public double burstStart;
    public int burstShots;
    public int burstShotsFired;
    public double nextBurst;

    public int timesPlayed = 0;

    float volume = 0.08f;

    double fadeStart = 0.3;
    double fadeEnd = 1.7;
    float volumeFadeEnd = 0.33f;

    System.Random random;

    /*public ShootEffect(AudioSource Source, SoundProfile soundProfile)
    {
        source = Source;
        clip = soundProfile.clip;
        repitions = soundProfile.repetitions;
        rpm = soundProfile.rpm;
        flux = soundProfile.flux;

        startTime = Time.time;
        random = new System.Random();

        Play();
    }*/

    public ShootEffect(AudioSource Source, ShootProfile ShootProfile)
    {
        source = Source;
        shootProfile = ShootProfile;
        clip = shootProfile.clip;
        repetitions = shootProfile.repetitions;
        aimMin = shootProfile.aimMin;
        aimMax = shootProfile.aimMax;
        burstMin = shootProfile.burstMin;
        burstMax = shootProfile.burstMax;
        burstDelayMin = shootProfile.burstDelayMin;
        burstDelayMax = shootProfile.burstDelayMax;
        rpm = shootProfile.rpm;
        flux = shootProfile.flux;
        startPos = new Vector3();
        endPos = new Vector3();
        speed = shootProfile.speed;

        startTime = Time.time;
        random = new System.Random();

        Init();
    }

    public ShootEffect(AudioSource Source, ShootProfile ShootProfile, Vector3 StartPos, Vector3 EndPos)
    {
        source = Source;
        shootProfile = ShootProfile;
        clip = shootProfile.clip;
        repetitions = shootProfile.repetitions;
        aimMin = shootProfile.aimMin;
        aimMax = shootProfile.aimMax;
        burstMin = shootProfile.burstMin;
        burstMax = shootProfile.burstMax;
        burstDelayMin = shootProfile.burstDelayMin;
        burstDelayMax = shootProfile.burstDelayMax;
        rpm = shootProfile.rpm;
        flux = shootProfile.flux;
        startPos = StartPos;
        endPos = EndPos;
        speed = shootProfile.speed;

        startTime = Time.time;
        random = new System.Random();

        Init();
    }

    //public ShootEffect(AudioSource Source, Clip Clip, int Repitions, double Rpm, double Flux)
    //{
    //    source = Source;
    //    clip = Clip;
    //    repetitions = Repitions;
    //    rpm = Rpm;
    //    flux = Flux;

    //    startTime = Time.time;
    //    random = new System.Random();

    //    Shoot();
    //}

    void Init() {
        Debug.Log("Shoot init, burstMin: " + burstMin);
        if (burstMin > 0) {
            Debug.Log("ShootEffect - This code needs work");
            StartBurst();
        } else {
            nextPlayTime = Time.time + aimMin + ( random.NextDouble() * (aimMax - aimMin) );
            //Shoot();
        }
    }

    private void Shoot()
    {
        lastPlay = Time.time;
        PlaySound();
        if ( shootProfile.projectile ) MakeProjectile();

        timesPlayed++;

        if (burstMin > 0) {
            burstShotsFired++;
            //Debug.Log("Burst shots fired " + burstShotsFired);
        };

        if (burstMin > 0 && burstShotsFired >= burstShots) {

            PrepNextBurst();

        } else {

            double interval = (60.0 / rpm);

            if (flux != 0)
            {
                double mult = 1 + (random.NextDouble() * flux);
                //Debug.Log("<color=green>"+mult.ToString()+"</color>");
                if (random.NextDouble() > 0.5)
                {
                    interval *= mult;
                }
                else
                {
                    interval /= mult;
                }
            }

            nextPlayTime = lastPlay + interval;

        }

    }

    private void StartBurst() {
        burstShots = Lib.random.Next(burstMin, burstMax + 1);
        Debug.Log("Burst shots " + burstShots);
        burstShotsFired = 0;
        burstStart = Time.time;
        Shoot();
    }

    void PrepNextBurst() {
        double d = burstDelayMin + (Lib.random.NextDouble() * (burstDelayMax - burstDelayMin));
        Debug.Log(d);
        nextPlayTime = lastPlay + d;
        burstShotsFired = 0;
        Debug.Log(nextPlayTime);
    }

    private void PlaySound()
    {
        source.PlayOneShot(clip.clip, clip.volume * volume /** getFadeVolume()*/);
    }

    private void MakeProjectile()
    {
        GameObject shellGo = GameObject.Instantiate(shootProfile.shellObj);
        Shell shell = (Shell)shellGo.GetComponent<Shell>();
        shell.Setup(startPos, endPos, speed, shootProfile.speedExponent, shootProfile.yMultiplier);
    }

    public bool DoFrameAndReportCompleteness()
    {
        if (IsComplete()) return true;
        else
        {
            CheckReady();
            return false;
        }
    }

    void CheckReady()
    {
        if (Time.time >= nextPlayTime)
        {
            Shoot();
        }
    }

    public bool IsComplete()
    {
        if (burstMin > 0 && burstShotsFired == 0 && repetitions - timesPlayed < burstMin) return true; // complete if a burst of min size cannot be made

        if (timesPlayed >= repetitions) return true;
        //return (timesPlayed >= repetitions);

        return false;
    }

    float getFadeVolume()
    {
        double timeElapsed = Time.time - startTime;
        double timeIntoFade = timeElapsed - fadeStart;
        if (timeIntoFade > 0.0)
        {
            double percentIntoFade = timeIntoFade / (fadeEnd - fadeStart);
            percentIntoFade = Math.Min(1, percentIntoFade);

            return 1.0f - ((1.0f - volumeFadeEnd) * (float)percentIntoFade);
        }
        else
        {
            return 1.0f;
        }

    }

}

/*public struct Clip
{
    public AudioClip clip;
    public float volume;

    public Clip(AudioClip Clip, float Volume)
    {
        clip = Clip;
        volume = Volume;
    }
}*/

/*public static class SoundLib
{
    public static Clip rifleShot = new Clip(Resources.Load<AudioClip>("Sounds/Rifle_shot_1"), 1.0f);
    public static Clip m249Shot = new Clip(Resources.Load<AudioClip>("Sounds/m249"), 1.0f);
    public static Clip bradleyShot = new Clip(Resources.Load<AudioClip>("Sounds/m242_25mm"), 1.1f);
    public static Clip towShot = new Clip(Resources.Load<AudioClip>("Sounds/tow"), 1.15f);
    public static Clip javelinShot = new Clip(Resources.Load<AudioClip>("Sounds/javelin"), 1.2f);
    public static Clip abramsShot = new Clip(Resources.Load<AudioClip>("Sounds/m256_120mm"), 0.7f);
}*/


public class ShootManager : MonoBehaviour
{
    /*public AudioClip rifleShotSound;
    public Clip rifleShot;
    public AudioClip bradleyShotSound;
    public Clip bradleyShot;
    public AudioClip abramsShotSound;
    public Clip abramsShot;*/

    public GameObject shellPrefab;
    private List<ShootEffect> shootEffects;

    private Global global;
    private bool initialised = false;

    // Start is called before the first frame update
    void Start()
    {
        shootEffects = new List<ShootEffect>();

        /*rifleShot = new Clip(rifleShotSound, 1.0f);
        bradleyShot = new Clip(bradleyShotSound, 1.1f);
        abramsShot = new Clip(abramsShotSound, 0.7f);*/
    }

    // Update is called once per frame
    void Update()
    {
        CheckShootEffects();
        if ( ! initialised )
        {
            Initialise();
        }
    }

    void Initialise()
    {
        global = (Global)FindObjectOfType<Global>();
        initialised = true;
    }

    void CheckShootEffects()
    {
        List<ShootEffect> killList = new List<ShootEffect>();

        foreach (ShootEffect shootEffect in shootEffects)
        {
            if (shootEffect.DoFrameAndReportCompleteness()) killList.Add(shootEffect);
        }

        foreach (ShootEffect shootEffect in killList)
        {
            shootEffects.Remove(shootEffect);
        }
    }

    /*public void PlayShootEffect(AudioSource source, SoundProfile soundProfile, Unit unit, Unit targetUnit)
    {
        PlayShootEffect(source, soundProfile, unit.tile, targetUnit.tile);
    }*/

    /*public void PlayShootEffect(AudioSource source, SoundProfile soundProfile, Tile fromTile, Tile targetTile)
    {
        Vector3 startPos = fromTile.position;
        startPos.y += 0.5f * global.map.YMultiplier();

        Vector3 endPos = targetTile.position;
        endPos.y += 0.0f * global.map.YMultiplier();
        
        PlayShootEffect(source, soundProfile, startPos, endPos);
    }*/

    //public void PlayShootEffect(AudioSource source, SoundProfile soundProfile, Vector3 startPos, Vector3 endPos)
    //{

    //    //ShootEffect shootEffect = new ShootEffect(source, new ShootProfile(soundProfile, startPos, endPos, 10f), shellPrefab);
    //    ShootEffect shootEffect = new ShootEffect(source, new ShootProfile(soundProfile, true, shellPrefab, 10f), startPos, endPos);
    //    shootEffects.Add(shootEffect);
    //}

    /*public void PlayShootEffect(AudioSource source, ShootProfile shootProfile, Tile fromTile, Tile targetTile)
    {
        Vector3 startPos = fromTile.position;
        startPos.y += 0.5f * global.map.YMultiplier();

        Vector3 endPos = targetTile.position;
        endPos.y += 0.0f * global.map.YMultiplier();
        
        ShootEffect shootEffect = new ShootEffect(source, shootProfile, startPos, endPos);
        shootEffects.Add(shootEffect);
    }*/

    public void PlayShootEffect(AudioSource source, ShootProfile shootProfile, int number, Vector3 startPos, Vector3 endPos)
    {

        ShootEffect shootEffect = new ShootEffect(source, shootProfile, startPos, endPos);
        shootEffects.Add(shootEffect);
        
        if (number > 1)
        {
            for (int i = 2; i <= number; i++)
            {
                StartCoroutine(SpawnShootEffect(source, shootProfile, startPos, endPos, number));
            }
        }
    }

    IEnumerator SpawnShootEffect(AudioSource source, ShootProfile shootProfile, Vector3 startPos, Vector3 endPos, int number)
    {
        double ran = global.random.NextDouble() * 1.5 / ( Math.Pow((double)(number-1), 0.5) );
        //Debug.Log("Waiting for: " + ran);
        yield return new WaitForSeconds((float)ran);

        double unitDiscRadius = 0.3;
        double randomAngle = Lib.random.NextDouble() * 360.0;
        double randomDistance = Lib.random.NextDouble() * unitDiscRadius;

        //Debug.Log("Random angle: " + randomAngle);
        //Debug.Log("Random distance: " + randomDistance);

        Vector3 disp = new Vector3(0f, 0f, 1f);
        disp = Quaternion.Euler(0f, (float)randomAngle, 0f) * disp;

        //Debug.Log("displacement: " + disp);

        startPos = startPos + disp;

        ShootEffect shootEffect = new ShootEffect(source, shootProfile, startPos, endPos);
        shootEffects.Add(shootEffect);
    }

    /*public void PlayShootEffect(AudioSource source, Clip clip, int repetitions, double rpm, double flux = 0.0)
    {
        ShootEffect shootEffect = new ShootEffect(source, clip, repetitions, rpm, flux);
        shootEffects.Add(shootEffect);
    }*/

    /*public void PlayShootEffectArtillery(AudioSource source, ShootProfile shootProfile, Tile fromTile)
    {
        //Vector3 startPos = fromTile.position;
        //startPos.y += 0.5f * global.map.YMultiplier();

        //Vector3 endPos = targetTile.position;
        //endPos.y += 0.0f * global.map.YMultiplier();

        //ShootEffect shootEffect = new ShootEffect(source, new ShootProfile(soundProfile, startPos, true), shellPrefab);
        ShootEffect shootEffect = new ShootEffect(source, shootProfile);
        shootEffects.Add(shootEffect);
    }*/

    public void PlayShootEffectArtillery(AudioSource source, ShootProfile shootProfile, Vector3 startPos)
    {
        //Vector3 startPos = fromTile.position;
        //startPos.y += 0.5f * global.map.YMultiplier();

        Vector3 endPos = startPos;
        endPos.y += 15f * 100f;

        //ShootEffect shootEffect = new ShootEffect(source, new ShootProfile(soundProfile, startPos, true), shellPrefab);
        ShootEffect shootEffect = new ShootEffect(source, shootProfile, startPos, endPos);
        shootEffects.Add(shootEffect);
    }

    public void PlayImpactEffectArtillery(AudioSource source, ShootProfile impactProfile, Vector3 endPos)
    {
        Vector3 startPos = endPos;
        startPos.y += 15f * 100f;
        //startPos.y += 0.5f * global.map.YMultiplier();

        //Vector3 endPos = targetTile.position;
        //endPos.y += 0.0f * global.map.YMultiplier();

        ShootEffect shootEffect = new ShootEffect(source, impactProfile, startPos, endPos);
        shootEffects.Add(shootEffect);
    }

}
