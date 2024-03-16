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
    [Range(0, 1)]
    public float voiceVolume = 1;

    private Bus masterBus;
    private Bus musicBus;
    private Bus ambienceBus;
    private Bus sfxBus;
    private Bus voiceBus;

    private List<EventInstance> eventInstances;

    private EventInstance musicEventInstance;

    private EventInstance ambienceEventInstance;

    FMOD.Studio.EventInstance slidingSFX;
    FMOD.Studio.EventInstance chargePistol;

    [SerializeField] private GameObject roomSpawnerContainer;

    public List<GameObject> roomSpawners = new List<GameObject>();
    [SerializeField] private int[] enemiesPerSpawner;
    [SerializeField] private int enemies;
    private GameObject[] g;

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
        voiceBus = RuntimeManager.GetBus("bus:/Voice");

        // DontDestroyOnLoad(gameObject);
        // DontDestroyOnLoad(GameObject.Find("AudioManager"));
        // DontDestroyOnLoad(GameObject.Find("FMODEvents"));

        roomSpawnerContainer = GameObject.Find("RoomSpawners");

        foreach (Transform child in roomSpawnerContainer.transform)
        {
            roomSpawners.Add(child.gameObject);
        }
        g = roomSpawners.ToArray();
        enemiesPerSpawner = new int[roomSpawners.Count];
        for(int i = 0; i < roomSpawners.Count; i++)
        {
           enemiesPerSpawner[i] = g[i].GetComponent<roomEnemySpawner>().enemiesRemaining.Count;
        }

        enemies = 0;

        getSettings();
    }

    private void Start()
    {
        InitializeMusic(FMODEvents.instance.battleMusic);
        InitializeAmbience(FMODEvents.instance.ambience);
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        ambienceBus.setVolume(ambienceVolume);
        sfxBus.setVolume(SFXVolume);

        SetEnemyCount();

       if (SceneManager.GetActiveScene().name == "L1" && enemies > 0)
        {
            musicEventInstance.setParameterByName("Music", 3);
        }
        else if (SceneManager.GetActiveScene().name == "L1" && enemies <= 0)
        {
            musicEventInstance.setParameterByName("Music", 1);
        }

    }

    void getSettings()
    {
        if (PlayerPrefs.HasKey("MasterVolumeValue")) masterVolume = PlayerPrefs.GetFloat("MasterVolumeValue");
        if (PlayerPrefs.HasKey("SFXVolumeValue")) SFXVolume = PlayerPrefs.GetFloat("SFXVolumeValue");
        if (PlayerPrefs.HasKey("MusicVolumeValue")) musicVolume = PlayerPrefs.GetFloat("MusicVolumeValue");
        if (PlayerPrefs.HasKey("AmbienceVolumeValue")) ambienceVolume = PlayerPrefs.GetFloat("AmbienceVolumeValue");
    }

    void SetEnemyCount()
    {
        int count = 0;
        for (int i = 0; i < roomSpawners.Count; i++)
       {
            enemiesPerSpawner[i] = g[i].GetComponent<roomEnemySpawner>().enemiesRemaining.Count;
            count += enemiesPerSpawner[i];
        }
        enemies = count;
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

    private void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = EventInstance(ambienceEventReference);
        ambienceEventInstance.start();
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
        chargePistol = RuntimeManager.CreateInstance(FMODEvents.instance.chargePistolSFX);
        chargePistol.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        chargePistol.start();
        chargePistol.release();
    }

    public void StopchargePistolSFX()
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
