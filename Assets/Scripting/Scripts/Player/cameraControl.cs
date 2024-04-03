using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
//using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
//using Palmmedia.ReportGenerator.Core.Reporting.Builders;

public class cameraControl : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] private GameObject Orientation;
    public GameObject CameraObj;
    CameraInputActions controls;
    public RaycastHit lookingDir;
    public LayerMask layersToIgnoreForAimingDir;

    [Space, Header("Stats")]
    public float mouseSensitivityHorizontal;
    public float mouseSensitivityVertical;
    public bool flipHoirzontal;
    public bool flipVertical;
    public float maxAngle;
    public float minAngle;

    [HideInInspector] public float mouseX;
    [HideInInspector] public float mouseY;
    private float xRot;
    private float yRot;
    [SerializeField] private float startingYRot;

    private playerMovement playerMovementScript;

    private void Awake()
    {
        getSettings();
        Cursor.lockState = CursorLockMode.Locked;
        controls = new CameraInputActions();
        playerMovementScript = GetComponent<playerMovement>();

        layersToIgnoreForAimingDir = ~layersToIgnoreForAimingDir;
    }

    private void Start()
    {
        startingYRot = Orientation.transform.localEulerAngles.y;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        if (playerMovementScript.canAffectRotation)
        {
            calculateRotation();
            Orientation.transform.localEulerAngles = new Vector3(0, xRot + startingYRot, 0);
            if (playerMovementScript.current_PlayerRotationState == playerRotationState.nonFlipped)
            {
                CameraObj.transform.localEulerAngles = new Vector3(yRot, 0, 0);
                Orientation.transform.localEulerAngles = new Vector3(0, xRot + startingYRot, 0);
            }
            else
            {
                CameraObj.transform.localEulerAngles = new Vector3(yRot, 0, 180);
                Orientation.transform.localEulerAngles = new Vector3(0, xRot + startingYRot, 0);
            }
        }
        Physics.Raycast(CameraObj.transform.position, CameraObj.transform.forward, out lookingDir, Mathf.Infinity, layersToIgnoreForAimingDir);
    }

    void getSettings()
    {
        if(PlayerPrefs.HasKey("mouseXSensValue")) mouseSensitivityHorizontal = PlayerPrefs.GetFloat("mouseXSensValue");
        if (PlayerPrefs.HasKey("mouseYSensValue")) mouseSensitivityVertical = PlayerPrefs.GetFloat("mouseYSensValue");
        if (Convert.ToBoolean(PlayerPrefs.GetInt("invertMouseX")) == true) flipHoirzontal = true;
        else flipHoirzontal = false;
        if (Convert.ToBoolean(PlayerPrefs.GetInt("invertMouseY")) == true) flipVertical = true;
        else flipVertical = false;

        //Camera.main.fieldOfView = PlayerPrefs.GetFloat("fovSetting");
    }

    private void calculateRotation()
    {
        if (playerMovementScript.current_PlayerRotationState == playerRotationState.nonFlipped)
        {
            mouseX = controls.CameraControls.CameraRotation.ReadValue<Vector2>().x;
            mouseY = controls.CameraControls.CameraRotation.ReadValue<Vector2>().y;
        }
        else
        {
            mouseX = controls.CameraControls.CameraRotation.ReadValue<Vector2>().x * -1;
            mouseY = controls.CameraControls.CameraRotation.ReadValue<Vector2>().y * -1;
        }


        if (flipHoirzontal) xRot -= mouseX * mouseSensitivityHorizontal * Time.deltaTime;
        else xRot += mouseX * mouseSensitivityHorizontal * Time.deltaTime;
        if (flipVertical) yRot += mouseY * mouseSensitivityVertical * Time.deltaTime;
        else yRot -= mouseY * mouseSensitivityVertical * Time.deltaTime;

        yRot = Mathf.Clamp(yRot, minAngle, maxAngle);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Vector3 tempDir = CameraObj.transform.forward * 100f;
        Gizmos.DrawRay(CameraObj.transform.position, tempDir);
    }
}
