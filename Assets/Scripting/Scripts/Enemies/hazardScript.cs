using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hazardScript : MonoBehaviour
{
    [Header("Hazard Values")]
    [SerializeField] private bool isActive;
    [SerializeField] private int damageDone;
    [SerializeField] private float knockbackApplied;
    [SerializeField] private float timeToToggle;
    [SerializeField] private float cycleStartTime;
    [SerializeField] private GameObject[] objsToToggle;

    private playerMovement playerMovementScript;
    private playerHealth playerHealthScript;
    private GameObject playerObject;

    private float currentCycleTime;

    private void Awake()
    {
        playerMovementScript = GameObject.Find("Player").GetComponent<playerMovement>();
        playerHealthScript = GameObject.Find("Player").GetComponent<playerHealth>();
        playerObject = GameObject.Find("Player");
        if (timeToToggle > 0) StartCoroutine(toggleTimer());
        currentCycleTime += cycleStartTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isActive)
        {
            StartCoroutine(playerHealthScript.takeDamage(damageDone));
            Vector3 tempDir = (playerObject.transform.position - transform.position).normalized;
            tempDir *= knockbackApplied;
            playerMovementScript.horizontal_playerVelocity += new Vector3(tempDir.x, 0, tempDir.z);
            playerMovementScript.vertical_playerVelocity += new Vector3(0, tempDir.y, 0);
        }
    }

    IEnumerator toggleTimer()
    {

        while (true)
        {
            if (currentCycleTime >= timeToToggle)
            {
                currentCycleTime = 0;
                foreach (GameObject t in objsToToggle)
                {
                    if(t.activeSelf) t.SetActive(false);
                    else t.SetActive(true);
                }
            }
            currentCycleTime += Time.deltaTime;
            yield return null;
        }

    }
}
