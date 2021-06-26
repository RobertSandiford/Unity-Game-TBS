using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public struct SoundProfile
{
    public Clip clip;
    public int repetitions;
    public double rpm;
    public double flux;
    public int shooters;

    public SoundProfile(Clip Clip, int Repititions, double Rpm, double Flux, int Shooters = 1)
    {
        clip = Clip;
        repetitions = Repititions;
        rpm = Rpm;
        flux = Flux;
        shooters = Shooters;
    }

    public SoundProfile(Clip Clip)
    {
        clip = Clip;
        repetitions = 1;
        rpm = 1;
        flux = 0.0;
        shooters = 1;
    }

    /*public SoundProfile(bool ignore)
    {
        clip = null;
        repetitions = 1;
        rpm = 1.0;
        flux = 0.0;
        shooters = 1;
    }*/
}

/*public struct SoundProfile
{
    public Clip clip;
    public int repetitions;
    public double rpm;
    public double flux;
    public int shooters;

    public SoundProfile(Clip Clip, int Repititions, double Rpm, double Flux, int Shooters = 1)
    {
        clip = Clip;
        repetitions = Repititions;
        rpm = Rpm;
        flux = Flux;
        shooters = Shooters;
    }

    public SoundProfile(Clip Clip)
    {
        clip = Clip;
        repetitions = 1;
        rpm = 1;
        flux = 0.0;
        shooters = 1;
    }
}*/

public class Sound
{
    public AudioSource source;
    public Clip clip;
    public int repetitions;
    public double rpm;
    public double flux;

    public double startTime;
    public double lastPlay;
    public double nextPlayTime;

    public int timesPlayed = 0;

    float volume = 0.08f;

    double fadeStart = 0.3;
    double fadeEnd = 1.7;
    float volumeFadeEnd = 0.33f;

    System.Random random;

    public Sound( AudioSource Source, Clip Clip, int Repitions, double Rpm, double Flux)
    {
        source = Source;
        clip = Clip;
        repetitions = Repitions;
        rpm = Rpm;
        flux = Flux;

        startTime = Time.time;

        random = new System.Random();

        Play();
    }

    private void Play()
    {
        lastPlay = Time.time;
        source.PlayOneShot(clip.clip, clip.volume * volume * getFadeVolume());
        timesPlayed++;

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
            Play();
        }
    }

    public bool IsComplete()
    {
        return (timesPlayed >= repetitions);
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

public struct Clip
{
    public AudioClip clip;
    public float volume;

    public Clip(AudioClip Clip, float Volume)
    {
        clip = Clip;
        volume = Volume;
    }
}

public static class SoundLib
{
    public static Clip rifleShot = new Clip( Resources.Load<AudioClip>("Sounds/Rifle_shot_1"), 1.0f );
    public static Clip m249Shot = new Clip(Resources.Load<AudioClip>("Sounds/m249"), 1.0f);
    public static Clip bradleyShot = new Clip(Resources.Load<AudioClip>("Sounds/m242_25mm"), 1.1f);
    public static Clip towShot = new Clip(Resources.Load<AudioClip>("Sounds/tow"), 1.15f);
    public static Clip javelinShot = new Clip(Resources.Load<AudioClip>("Sounds/javelin"), 1.2f);
    public static Clip abramsShot = new Clip(Resources.Load<AudioClip>("Sounds/m256_120mm"), 0.7f);
}


public class SoundManager : MonoBehaviour
{
    public AudioClip rifleShotSound;
    public Clip rifleShot;
    public AudioClip bradleyShotSound;
    public Clip bradleyShot;
    public AudioClip abramsShotSound;
    public Clip abramsShot;

    private List<Sound> sounds;

    // Start is called before the first frame update
    void Start()
    {
        sounds = new List<Sound>();

        rifleShot = new Clip(rifleShotSound, 1.0f);
        bradleyShot = new Clip(bradleyShotSound, 1.1f);
        abramsShot = new Clip(abramsShotSound, 0.7f);
    }

    // Update is called once per frame
    void Update()
    {
        CheckSounds();
    }

    void CheckSounds()
    {
        List<Sound> killList = new List<Sound>();

        foreach (Sound sound in sounds)
        {
            if (sound.DoFrameAndReportCompleteness()) killList.Add(sound);
        }

        foreach (Sound sound in killList)
        {
            sounds.Remove(sound);
        }
    }

    public void PlaySound(AudioSource source, SoundProfile soundProfile)
    {
        Sound sound = new Sound(source, soundProfile.clip, soundProfile.repetitions, soundProfile.rpm, soundProfile.flux);
        sounds.Add(sound);
    }
    public void PlaySound(AudioSource source, Clip clip, int repetitions, double rpm, double flux = 0.0)
    {
        Sound sound = new Sound(source, clip, repetitions, rpm, flux);
        sounds.Add(sound);
    }

}
