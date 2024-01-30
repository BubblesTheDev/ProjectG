using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class screenSettings : MonoBehaviour
{
    [Header("Screen Settings")]
    //[SerializeField] private int resolutionIndex;
    [SerializeField] private screenState fullscreenState;
    public TMP_Dropdown resolutionSetting;
    public TMP_Dropdown screenStateSetting;
    private Resolution[] resolutions;
    private List<Resolution> usableResolutions;
    [SerializeField] private double currentRefreshRate;
    [SerializeField] private List<string> screenStateOptions;

    [Space, Header("Debug Options")]
    [SerializeField] private bool toggleConsoleFeedback;

    private void Awake()
    {
        if (resolutionSetting != null) resolutionSetting.onValueChanged.AddListener(delegate { setResolution(resolutionSetting.value); });
        if (screenStateSetting != null) screenStateSetting.onValueChanged.AddListener(delegate { setScreenState(screenStateSetting.value); });
        setupSettings();
    }

    void setupSettings()
    {

        #region This sets up the available resolutions for the game
        if (resolutionSetting != null)
        {
            resolutions = Screen.resolutions;
            usableResolutions = new List<Resolution>();

            resolutionSetting.ClearOptions();
            currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;

            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].refreshRateRatio.value == currentRefreshRate) usableResolutions.Add(resolutions[i]);
            }

            List<string> resolutionOptions = new List<string>();
            for (int i = 0; i < usableResolutions.Count; i++)
            {
                string resolutionOption = usableResolutions[i].width + "x" + usableResolutions[i].height + " " + usableResolutions[i].refreshRateRatio.value + " Hz";
                resolutionOptions.Add(resolutionOption);

                if (usableResolutions[i].width == Screen.width && usableResolutions[i].height == Screen.height && !PlayerPrefs.HasKey("resolutionSetting")) 
                    PlayerPrefs.SetString("resolutionSetting", usableResolutions[i].width.ToString() + "x" + usableResolutions[i].height.ToString() + " " + currentRefreshRate + " Hz");
            }

            resolutionSetting.AddOptions(resolutionOptions);
            resolutionSetting.value = resolutionOptions.FindIndex(x => x == PlayerPrefs.GetString("resolutionSetting"));
            resolutionSetting.RefreshShownValue();
        }
        else
        {
            Debug.LogWarning("There is no dropdown for the resolution options. \n Please create a dropdown for this and assign it in this scene");
            Debug.Break();
        }
        #endregion

        #region This sets up the screen states
        if (screenStateSetting != null)
        {
            for (int i = 0; i < Enum.GetNames(typeof(screenState)).Length; i++)
            {
                screenStateOptions.Add(Enum.GetName(typeof(screenState), i));
            }
            screenStateSetting.AddOptions(screenStateOptions);
            if (!PlayerPrefs.HasKey("screenStateSetting")) PlayerPrefs.SetString("screenStateSetting", Screen.fullScreenMode.ToString());
            screenStateSetting.value = screenStateSetting.options.FindIndex(x => x.text == PlayerPrefs.GetString("screenStateSetting"));
            fullscreenState = (screenState)screenStateSetting.value;
        }
        else
        {
            Debug.LogWarning("There is no dropdown for the screen state options. \n Please create a dropdown for this and assign it in this scene");
        }
        #endregion
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

        if (toggleConsoleFeedback) print("Setting Resolution To " + resolutionSetting.options[resolutionSetting.value].text.ToString());
        PlayerPrefs.SetString("resolutionSetting", resolutionSetting.options[resolutionSetting.value].text.ToString());
    }

    public void setScreenState(int index)
    {
        switch (index)
        {
            case 0:
                fullscreenState = screenState.fullScreen;
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                Screen.SetResolution(usableResolutions[index].width, usableResolutions[index].height, true);
                break;
            case 1:
                fullscreenState = screenState.windowed;
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Screen.SetResolution(usableResolutions[index].width, usableResolutions[index].height, false);
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                fullscreenState = screenState.borderlessWindowed;
                Screen.SetResolution(usableResolutions[index].width, usableResolutions[index].height, true);
                break;
        }

        if (toggleConsoleFeedback) print("Setting Screenstate to " + fullscreenState.ToString());
        PlayerPrefs.SetString("screenStateSetting", fullscreenState.ToString());
    }
}

enum screenState
{
    fullScreen,
    windowed,
    borderlessWindowed
}
