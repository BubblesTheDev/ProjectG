using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.UIElements;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 1;
    [Range(0, 1)]
    public float musicVolume = 1;
    [Range(0, 1)]
    public float ambienceVolume = 1;
    [Range(0, 1)]
    public float SFXVolume = 1;

    private Bus masterBus;
    private Bus musicBus;
    private Bus ambienceBus;
    private Bus sfxBus;

    private List<EventInstance> eventInstances;

    private EventInstance musicEventInstance;

    FMOD.Studio.EventInstance slidingSFX;
    FMOD.Studio.EventInstance chargePistol;

    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene");
        }
        instance = this;

        eventInstances = new List<EventInstance>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(GameObject.Find("AudioManager"));
        DontDestroyOnLoad(GameObject.Find("FMODEvents"));
    }

    private void Start()
    {
        InitializeMusic(FMODEvents.instance.battleMusic);
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        ambienceBus.setVolume(ambienceVolume);
        sfxBus.setVolume(SFXVolume);

       if (SceneManager.GetActiveScene().name == "L1")
        {
            musicEventInstance.setParameterByName("Music", 1);
        } 
    }

    public void PlaySFX(EventReference SFX, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(SFX, worldPos);
    } 

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = EventInstance(musicEventReference);
        musicEventInstance.start();
    }

    public EventInstance EventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public void PlaySlideSFX()
    {
            slidingSFX = RuntimeManager.CreateInstance(FMODEvents.instance.slideSFX);
            slidingSFX.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            slidingSFX.start();
            slidingSFX.release();
    }

    public void StopSlideSFX()
    {
        slidingSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void PlaychargePistolSFX()
    {
        chargePistol = RuntimeManager.CreateInstance(FMODEvents.instance.slideSFX);
        chargePistol.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        chargePistol.start();
        chargePistol.release();
    }

    public void StopSlideSFX()
    {
        chargePistol.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void CleanUp()
    {
        //stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
