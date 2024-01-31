using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mouseControlSettings : MonoBehaviour
{
    [Header("Mouse Setting Assignables")]
    [SerializeField] private Slider horizontalMouseSensSlider;
    [SerializeField] private Slider verticalMouseSensSlider;
    [SerializeField] private Slider FOVSlider;
    [SerializeField] private Toggle invertMouseXToggle;
    [SerializeField] private Toggle invertMouseYToggle;
    [SerializeField] private TMPro.TMP_InputField horizontalMouseSensInput;
    [SerializeField] private TMPro.TMP_InputField verticalMouseSensInput;

    [Space, Header("Debug Options")]
    [SerializeField] private bool toggleConsoleFeedback;
    

    private cameraControl cameraControlScript;

    private void Awake()
    {
        addListeners();
        setupSettings();
        if(GameObject.Find("Player").GetComponent<cameraControl>() != null) cameraControlScript = GameObject.Find("Player").GetComponent<cameraControl>();
    }

    void addListeners()
    {
        if (horizontalMouseSensSlider != null) horizontalMouseSensSlider.onValueChanged.AddListener(delegate { changeSensitivitySlider("horizontal"); });
        if (verticalMouseSensSlider != null) verticalMouseSensSlider.onValueChanged.AddListener(delegate { changeSensitivitySlider("vertical"); });

        if (invertMouseXToggle != null) invertMouseXToggle.onValueChanged.AddListener(delegate { changeToggle("horizontal"); });
        if (invertMouseYToggle != null) invertMouseYToggle.onValueChanged.AddListener(delegate { changeToggle("vertical"); });

        if (horizontalMouseSensInput != null) horizontalMouseSensInput.onValueChanged.AddListener(delegate { changeSensitivityField("horizontal"); });
        if (verticalMouseSensInput != null) verticalMouseSensInput.onValueChanged.AddListener(delegate { changeSensitivityField("vertical"); });

        if (FOVSlider != null) FOVSlider.onValueChanged.AddListener(delegate { changeFOVSlider(); });
    }

    void setupSettings()
    {
        if (PlayerPrefs.HasKey("invertMouseX") && invertMouseXToggle != null) invertMouseXToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("invertMouseX"));
        if (PlayerPrefs.HasKey("invertMouseY") && invertMouseYToggle != null) invertMouseYToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("invertMouseY"));
        if (PlayerPrefs.HasKey("fovSetting") && FOVSlider != null) FOVSlider.value = PlayerPrefs.GetFloat("fovSetting");

        if (PlayerPrefs.HasKey("mouseXSensValue"))
        {
            if (horizontalMouseSensSlider != null) horizontalMouseSensSlider.value = PlayerPrefs.GetFloat("mouseXSensValue");
            if(horizontalMouseSensInput != null) horizontalMouseSensInput.text = PlayerPrefs.GetFloat("mouseXSensValue").ToString();
            if(cameraControlScript != null) cameraControlScript.mouseSensitivityHorizontal = PlayerPrefs.GetFloat("mouseXSensValue");
        }
        if (PlayerPrefs.HasKey("mouseYSensValue"))
        {
            if(verticalMouseSensSlider != null) verticalMouseSensSlider.value = PlayerPrefs.GetFloat("mouseYSensValue");
            if(verticalMouseSensInput != null) verticalMouseSensInput.text = PlayerPrefs.GetFloat("mouseYSensValue").ToString();
            if(cameraControlScript != null) cameraControlScript.mouseSensitivityVertical = PlayerPrefs.GetFloat("mouseYSensValue");
        }
    }


    public void changeToggle(string whichToggle)
    {
        switch(whichToggle) 
        {
            case "horizontal":
                if(toggleConsoleFeedback) print(invertMouseXToggle.isOn.ToString());
                PlayerPrefs.SetInt("invertMouseX", Convert.ToInt32(invertMouseXToggle.isOn));
                PlayerPrefs.Save();
                if (cameraControlScript != null) cameraControlScript.flipHoirzontal = invertMouseXToggle.isOn;
                break;
            case "vertical":
                if (toggleConsoleFeedback) print(invertMouseYToggle.isOn.ToString());
                PlayerPrefs.SetInt("invertMouseY", Convert.ToInt32(invertMouseYToggle.isOn));
                PlayerPrefs.Save();
                if (cameraControlScript != null) cameraControlScript.flipVertical = invertMouseYToggle.isOn;
                break;
        }
    }

    public void changeSensitivitySlider(string WhichAxis)
    {
        switch (WhichAxis)
        {
            case "horizontal":
                if (toggleConsoleFeedback) print("Mouse X Sensitivity: " + MathF.Round(horizontalMouseSensSlider.value, 3));
                PlayerPrefs.SetFloat("mouseXSensValue", MathF.Round(horizontalMouseSensSlider.value, 3));
                PlayerPrefs.Save();

                if (horizontalMouseSensInput != null) horizontalMouseSensInput.text = PlayerPrefs.GetFloat("mouseXSensValue").ToString();
                break;
            case "vertical":
                if (toggleConsoleFeedback) print("Mouse Y Sensitivity: " + MathF.Round(verticalMouseSensSlider.value, 3));
                PlayerPrefs.SetFloat("mouseYSensValue", MathF.Round(verticalMouseSensSlider.value, 3));
                PlayerPrefs.Save();
                if (verticalMouseSensInput != null) verticalMouseSensInput.text = PlayerPrefs.GetFloat("mouseYSensValue").ToString();
                break;
        }
    }

    public void changeFOVSlider()
    {
        if (toggleConsoleFeedback) print("FOV Value is " + MathF.Round(FOVSlider.value, 3));
        PlayerPrefs.SetFloat("fovSetting", MathF.Round(FOVSlider.value, 3));
        PlayerPrefs.Save();
    }

    public void changeSensitivityField(string WhichAxis)
    {
        switch (WhichAxis)
        {
            case "horizontal":
                if (toggleConsoleFeedback) print("Mouse X Sensitivity: " + MathF.Round(float.Parse(horizontalMouseSensInput.text), 3));
                PlayerPrefs.SetFloat("mouseXSensValue", MathF.Round(float.Parse(horizontalMouseSensInput.text), 3));
                PlayerPrefs.Save();

                if (horizontalMouseSensSlider != null) horizontalMouseSensSlider.value = PlayerPrefs.GetFloat("mouseXSensValue");
                break;
            case "vertical":
                if (toggleConsoleFeedback) print("Mouse Y Sensitivity: " + MathF.Round(float.Parse(verticalMouseSensInput.text), 3));
                PlayerPrefs.SetFloat("mouseYSensValue", MathF.Round(float.Parse(verticalMouseSensInput.text), 3));
                PlayerPrefs.Save();
                if (verticalMouseSensSlider != null) verticalMouseSensSlider.value = PlayerPrefs.GetFloat("mouseYSensValue");
                break;
        }
    }
}
