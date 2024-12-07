//Andrew Legg
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundPlayer : MonoBehaviour
{
    //Use SoundLoop for music tracks, ambience, and persistent sounds.
    [System.Serializable]
    public struct SoundLoop
    {
        //Properties
        public string name;
        public FMODUnity.EventReference fmod_event;

        //We need to keep track of an instance that gets released when the loop stops.

        //Constructor
        //SoundOneShot(string n, FMODUnity.EventReference fmod_e)
        //{
        //    name = n;
        //    fmod_event = fmod_e;
        //}
    }

    //Use SoundOneShot for events/sfx.
    [System.Serializable]
    public struct SoundOneShot
    {
        //Properties
        public string name;
        public FMODUnity.EventReference fmod_event;


        //Constructor
        //SoundOneShot(string n, FMODUnity.EventReference fmod_e)
        //{
        //    name = n;
        //    fmod_event = fmod_e;
        //}
    }

    //Area for sound events to be put into arrays.
    [Header("Event Sounds List")]
    public SoundLoop[] musicList;
    public SoundLoop[] soundLoopList;
    public SoundOneShot[] soundOneShotList;

    //NOTE: FOR SIMPLICITY, ONLY ONE INSTANCE OF A LOOP IS ACTIVE AT A TIME!!!
    private Dictionary<string, FMOD.Studio.EventInstance> fmodEventInstances;

    public static SoundPlayer instance;

    //Gets called when the object awakes
    public void Awake()
    {
        //Do singleton (ew)
        if (instance != null)
        {
            Debug.LogError("MapUIController.Start(): More than one script present in scene! This is not desired and will cause errors.");
            return;
        }
        instance = this;
        fmodEventInstances = new Dictionary<string, FMOD.Studio.EventInstance>();
    }

    //Check if sound is already playing
    //public bool IsPlaying()
    //{
    //    return source.isPlaying;
    //}

    //Stops playing sounds
    //public void StopPlaying()
    //{
    //    source.Stop();
    //}

    //Creates, Plays, and Releases an FMOD event.
    public void PlayOneShot(string soundName)
    {
        //Look for sound and play it
        SoundOneShot? sound = FindSoundOneShot(soundName);
        if (sound.HasValue)
        {
            FMODUnity.RuntimeManager.PlayOneShot(sound.Value.fmod_event);
        }
    }

    //This will create, play, and release a one shot FMOD event with the parameters given. 
    public void PlayOneShotWithParams(string soundName, params (string name, float value)[] parameters)
    {
        SoundOneShot? sound = FindSoundOneShot(soundName);
        if (sound.HasValue)
        {
            FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(sound.Value.fmod_event);

            foreach (var (name, value) in parameters)
            {
                Debug.Log($"Setting Parameter: {name} to value {value}");
                instance.setParameterByName(name, value);
            }

            instance.start();
            instance.release();
        }
    }

    //Return the event instance of the sound played.
    public void PlaySoundLoop(string soundName)
    {
        SoundLoop? sound = FindSoundLoop(soundName);
        if (sound.HasValue)
        {
            CreateSoundInstance(sound.Value);
        }
    }

    public void StopSoundLoop(string soundName)
    {
        DeleteSoundInstance(soundName);
    }

    public void StopAllSounds()
    {
        foreach (FMOD.Studio.EventInstance instance in fmodEventInstances.Values)
        {
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instance.release();
        }
    }

    public void SetLocalParameter(string soundName, string paramName, float paramValue)
    {
        if (fmodEventInstances.ContainsKey(soundName))
        {
            fmodEventInstances[soundName].setParameterByName(paramName, paramValue);
        } else
        {
            Debug.LogWarning($"Did not set parameter: {paramName} for sound {soundName} since it has not been instanced.");
        }
    }

    public bool IsPlaying(string musicName)
    {
        return fmodEventInstances.ContainsKey(musicName);
    }

    public void PlayMusic(string musicName, bool stopRunningMusic = true)
    {
        bool foundMusic = false;
        for (int i = 0; i < musicList.Length; i++)
        {
            if (musicList[i].name == musicName)
            {
                foundMusic = true;
                CreateSoundInstance(musicList[i]);
            } else if (stopRunningMusic && fmodEventInstances.ContainsKey(musicList[i].name))
            {
                StopMusic(musicList[i].name);
            }
        }

        //Inform that there is no music with name
        if (!foundMusic)
        {
            Debug.LogWarning("Music track not found: \"" + musicName + "\"!");
        }
    }

    public void StopMusic(string trackName)
    {
        DeleteSoundInstance(trackName);
    }

    private SoundOneShot? FindSoundOneShot(string soundName)
    {
        for (int i = 0; i < soundOneShotList.Length; i++)
        {
            //Check if sound, and play sound if found
            if (soundOneShotList[i].name == soundName)
            {
                return soundOneShotList[i];
                //End the loop
            }
        }

        Debug.LogWarning("Sound not found: \"" + soundName + "\"!");
        return null;
    }

    private SoundLoop? FindSoundLoop(string soundName)
    {
        for (int i = 0; i < soundLoopList.Length; i++)
        {
            //Check if sound, and play sound if found
            if (soundLoopList[i].name == soundName)
            {
                return soundLoopList[i];
                //End the loop
            }
        }

        Debug.LogWarning("Sound not found: \"" + soundName + "\"!");
        return null;
    }

    private void CreateSoundInstance(SoundLoop sound)
    {
        //Check for event already runnning and stop it.
        if (fmodEventInstances.ContainsKey(sound.name))
        {
            fmodEventInstances[sound.name].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            fmodEventInstances[sound.name].release();
        }

        //Create a new instance to play the sound loop.
        fmodEventInstances[sound.name] = FMODUnity.RuntimeManager.CreateInstance(sound.fmod_event);
        fmodEventInstances[sound.name].start();
    }

    private void DeleteSoundInstance(string soundName)
    {
        if (!fmodEventInstances.ContainsKey(soundName))
        {
            Debug.LogWarning($"Sound is not playing: {soundName}");
            return;
        }

        fmodEventInstances[soundName].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        fmodEventInstances[soundName].release();
        fmodEventInstances.Remove(soundName);
    }
}