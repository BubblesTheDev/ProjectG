using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    [Header("Settings")]
    public bool paused = false;
    [SerializeField] private List<GameObject> objectsToEnable = new List<GameObject>();
    [SerializeField] private List<GameObject> objectsToDisable = new List<GameObject>();

    private InteractionInputActions interactionInput;
    private void OnEnable()
    {
        interactionInput.Enable();
    }

    private void OnDisable()
    {
        interactionInput.Disable();
    }

    private void Awake()
    {
        interactionInput = new InteractionInputActions();
    }

    private void Update()
    {
        if (interactionInput.Settings.PauseGame.WasPerformedThisFrame())
        {
            if (paused == false) pauseScene();
            else if (paused == true) unpauseScene();
        }
    }

    public void pauseScene()
    {
        paused = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }

        foreach (GameObject item in objectsToDisable)
        {
            item.SetActive(false);
        }
    }

    public void unpauseScene()
    {
        paused = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(false);
        }

        foreach (GameObject item in objectsToDisable)
        {
            item.SetActive(true);
        }
    }
}
