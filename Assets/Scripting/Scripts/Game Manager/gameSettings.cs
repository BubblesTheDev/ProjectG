using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using UnityEngine.UI;
//using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine.EventSystems;

public class gameSettings : MonoBehaviour
{
    public static gameSettings gameSettingsReference
    {
        get
        {
            if (gameSettingsReference == null)
            {
                if (GameObject.FindObjectOfType<gameSettings>() != null)
                    GameObject.FindObjectOfType<gameSettings>();
                else
                {
                    Debug.LogError("There is no Game Settings script in the scene \n <b>Please Add One To The Scene </b>");
                    Debug.Break();
                }
            }
            return gameSettingsReference;

        }
    }

    [Header("Screen Settings")]
    public TMP_Dropdown resolutionSetting;
    private Resolution[] resolutions;
    private List<Resolution> usableResolutions;
    private double currentRefreshRate;
    private screenState fullscreenState;
    public TMP_Dropdown screenStateSetting;
    private List<string> screenStateOptions;

    [Header("Mouse Settings")]
    [Space] public bool flipHorizontalMouseSetting;
    public bool flipHorizontalDefault = false;

    public bool flipVerticalMouseSetting;
    public bool flipVerticalDefault = false;
    
    public float horizontalMouseSensitivitySetting;
    public float maxHorizontalSensDefault = 100, minHorizontalSensDefault = 1, HorizontalSensDefault = 25;
    
    public float verticalMouseSensitivitySetting;
    public float maxVerticalSensDefault = 100, minVerticalSensDefault = 1, VerticalSensDefault = 25;

    [Header("CameraSettings")]
    [Space] public float FOVSetting;
    public float maxFOVSettingDefault = 90, minFOVSettingDefault = 20, FOVSettingDefault= 60;
    
    public float screenShakeSetting;
    public float maxScreenShakeSettingDefault, minScreenShakeSettingDefault, screenShakeSettingDefault;

    public bool headbobEnableSetting;
    public bool headbobEnableDefault = true;

    public float headbobIntensitySetting;
    public float minHeadbobIntensity = 0f, maxHeadbobIntensity = 2f, headBobIntensity = 1f;

    public bool weaponBounceEnableSetting;
    public bool weaponBounceEnable = true;

    [Header("Volume Settings")]
    [Space] public float soundEffectsVolumeSetting;
    public float maxSoundEffectVolumeSettingDefault, minSoundEffectVolumeSettingDefault, soundEffectVolumeSettingDefault;
    
    public float musicVolumeSetting;
    public float maxMusicVolumeSettingDefault, minMusicVolumeSettingDefault, musicVolumeSettingDefault;
    
    public float voicesVolumeSetting;
    public float maxVoicesVolumeSettingDefault, minVoicesVolumeSettingDefault, voicesVolumeSettingDefault;


    private void Awake()
    {
        if(!PlayerPrefs.HasKey("initializedSettings"))
        {
            PlayerPrefs.SetString("initializedSettings", "true");
            setupPlayerPrefs();
            resolutionSetup();
        }
    }


    void setupPlayerPrefs()
    {
        #region Mouse Player Prefs
        if (!PlayerPrefs.HasKey("flipHoizontalMouseSetting")) PlayerPrefs.SetString("flipHoizontalMouseSetting", flipHorizontalDefault.ToString());
        if (!PlayerPrefs.HasKey("flipVerticalMouseSetting")) PlayerPrefs.SetString("flipVerticalMouseSetting", flipVerticalDefault.ToString()); ;
        if (!PlayerPrefs.HasKey("horizontalMouseSensitivitySetting")) PlayerPrefs.SetFloat("horizontalMouseSensitivitySetting", horizontalMouseSensitivitySetting);
        if (!PlayerPrefs.HasKey("verticalMouseSensitivitySetting")) PlayerPrefs.SetFloat("verticalMouseSensitivitySetting", verticalMouseSensitivitySetting);
        #endregion

        #region Camera Settings Player Prefs
        if (!PlayerPrefs.HasKey("FOVSettings")) PlayerPrefs.SetFloat("FOVSettings", FOVSetting);
        if (!PlayerPrefs.HasKey("ScreenShakeSettings")) PlayerPrefs.SetFloat("screenShakeSettings", screenShakeSetting);
        if (!PlayerPrefs.HasKey("headbobEnableSettings")) PlayerPrefs.SetString("headbobEnableSettings", headbobEnableDefault.ToString());
        if (!PlayerPrefs.HasKey("headbobIntensitySettings")) PlayerPrefs.SetFloat("headbobIntensitySettings", headBobIntensity);
        if (!PlayerPrefs.HasKey("weaponBounceEnableSetting")) PlayerPrefs.SetString("weaponBounceEnableSetting", weaponBounceEnable.ToString());

        #endregion

        #region Volume Setting Player Prefs
        if (!PlayerPrefs.HasKey("soundEffectVolumeSetting")) PlayerPrefs.SetFloat("soundEffectVolumeSetting", soundEffectVolumeSettingDefault);
        if (!PlayerPrefs.HasKey("musicVolumeSetting")) PlayerPrefs.SetFloat("musicVolumeSetting", musicVolumeSettingDefault);
        if (!PlayerPrefs.HasKey("voicesVolumeSetting")) PlayerPrefs.SetFloat("voicesVolumeSetting", voicesVolumeSettingDefault);
        #endregion
    }

    void resolutionSetup()
    {
        if (resolutionSetting != null)
        {
            resolutions = Screen.resolutions;
            usableResolutions = new List<Resolution>();

            resolutionSetting.ClearOptions();
            currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;

            List<string> resolutionOptions = new List<string>();

            for (int i = 0; i < resolutions.Length - 1; i++)
            {
                if (resolutions[i].refreshRateRatio.value == currentRefreshRate)
                {
                    usableResolutions.Add(resolutions[i]);
                    resolutionOptions.Add(resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRateRatio.value.ToString());

                }
            }

            resolutionSetting.value = usableResolutions.IndexOf(usableResolutions.Find(x => x.height == Screen.height && x.width == Screen.width));
            resolutionSetting.RefreshShownValue();

            PlayerPrefs.SetString("resolutionSetting", usableResolutions.IndexOf(usableResolutions.Find(x => x.height == Screen.height && x.width == Screen.width)).ToString());
        }
        else
        {
            Debug.LogWarning("There is no dropdown for the resolution options. \n Please create a dropdown for this and assign it in this scene");
            Debug.Break();
        }

        if (screenStateSetting != null)
        {
            for (int i = 0; i < Enum.GetNames(typeof(screenState)).Length - 1; i++)
            {
                screenStateOptions.Add(Enum.GetName(typeof(screenState), i));
            }
            screenStateSetting.AddOptions(screenStateOptions);
            screenStateSetting.value = screenStateSetting.options.FindIndex(x => x.text == Screen.fullScreenMode.ToString());

            PlayerPrefs.SetString("screenStateSetting", screenStateSetting.options.FindIndex(x => x.text == Screen.fullScreenMode.ToString()).ToString());
        }
        else
        {
            Debug.LogWarning("There is no dropdown for the screen state options. \n Please create a dropdown for this and assign it in this scene");
        }

    }

    public void setResolution(int index)
    {
        switch (fullscreenState)
        {
            case screenState.fullScreen:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                Screen.SetResolution(usableResolutions[index].width, usableResolutions[index].height, true);
                break;
            case screenState.windowed:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Screen.SetResolution(usableResolutions[index].width, usableResolutions[index].height, false);
                break;
            case screenState.borderlessWindowed:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Screen.SetResolution(usableResolutions[index].width, usableResolutions[index].height, true);
                break;
        }

        PlayerPrefs.SetString("resolutionSetting", resolutionSetting.options[resolutionSetting.value].ToString());
    }

    public void setScreenState(int index)
    {
        switch (index)
        {
            case 0:
                fullscreenState = screenState.fullScreen;
                break;
            case 1:
                fullscreenState = screenState.windowed;
                break;
            case 2:
                fullscreenState = screenState.borderlessWindowed;
                break;
        }

        PlayerPrefs.SetString("screenStateSetting", fullscreenState.ToString());
    }

    
}







