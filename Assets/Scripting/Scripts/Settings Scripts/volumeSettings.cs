using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class volumeSettings : MonoBehaviour
{
    [Header("Volume Setting Assignables")]

    [SerializeField] private Slider MasterVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private Slider MusicVolumeSlider;
    [SerializeField] private Slider AmbienceVolumeSlider;
    [SerializeField] private Slider VoiceVolumeSlider;

    private void Awake()
    {
        setupSettings();
        addListeners();
    }

    void addListeners()
    {
        if (MasterVolumeSlider != null) MasterVolumeSlider.onValueChanged.AddListener(delegate { changeVolumeSetting(volumeType.master); });
        if (SFXVolumeSlider != null) SFXVolumeSlider.onValueChanged.AddListener(delegate { changeVolumeSetting(volumeType.sfx); });
        if (MusicVolumeSlider != null) MusicVolumeSlider.onValueChanged.AddListener(delegate { changeVolumeSetting(volumeType.music); });
        if (AmbienceVolumeSlider != null) AmbienceVolumeSlider.onValueChanged.AddListener(delegate { changeVolumeSetting(volumeType.ambience); });
        if (VoiceVolumeSlider != null) VoiceVolumeSlider.onValueChanged.AddListener(delegate { changeVolumeSetting(volumeType.voice); });
    }

    void setupSettings()
    {
        if (PlayerPrefs.HasKey("MasterVolumeValue") && MasterVolumeSlider != null) MasterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolumeValue");
        if (PlayerPrefs.HasKey("SFXVolumeValue") && SFXVolumeSlider != null) SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolumeValue");
        if (PlayerPrefs.HasKey("MusicVolumeValue") && MusicVolumeSlider != null) MusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolumeValue");
        if (PlayerPrefs.HasKey("AmbienceVolumeValue") && AmbienceVolumeSlider != null) AmbienceVolumeSlider.value = PlayerPrefs.GetFloat("AmbienceVolumeValue");
        if (PlayerPrefs.HasKey("VoiceVolumeValue") && VoiceVolumeSlider != null) VoiceVolumeSlider.value = PlayerPrefs.GetFloat("VoiceVolumeValue");
    }

    private void changeVolumeSetting(volumeType type)
    {
        switch (type)
        {
            case volumeType.master:
                AudioManager.instance.masterVolume = MasterVolumeSlider.value;
                PlayerPrefs.SetFloat("MasterVolumeValue", MasterVolumeSlider.value);
                break;
            case volumeType.sfx:
                AudioManager.instance.SFXVolume = SFXVolumeSlider.value;
                PlayerPrefs.SetFloat("SFXVolumeValue", SFXVolumeSlider.value);
                break;
            case volumeType.music: 
                AudioManager.instance.musicVolume = MusicVolumeSlider.value;
                PlayerPrefs.SetFloat("MusicVolumeValue", MusicVolumeSlider.value);
                break;
            case volumeType.ambience: 
                AudioManager.instance.ambienceVolume = AmbienceVolumeSlider.value;
                PlayerPrefs.SetFloat("AmbienceVolumeValue", AmbienceVolumeSlider.value);
                break;
            case volumeType.voice:
                AudioManager.instance.voiceVolume = VoiceVolumeSlider.value;
                PlayerPrefs.SetFloat("VoiceVolumeValue", VoiceVolumeSlider.value);
                break;
        }
    }



}

enum volumeType
{
    master,
    sfx,
    music,
    ambience,
    voice
}