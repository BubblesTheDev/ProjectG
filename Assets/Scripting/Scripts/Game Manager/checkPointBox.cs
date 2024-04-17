using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class checkPointBox : MonoBehaviour
{
    [SerializeField] private int checkpointIndex;
    [SerializeField] private Volume checkpointPPR;
    [SerializeField] private float checkpointVignetteShowTime;

    checkpointSystem brain;
    private bool hasGottenCheckpoint;
    
    private void Awake()
    {
        brain = FindObjectOfType<checkpointSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !hasGottenCheckpoint)
        {
            hasGottenCheckpoint = true;
            brain.updateCheckpoint(checkpointIndex);
            StartCoroutine(brain.getCheckpoint());
            StartCoroutine(CheckpointVignette());
        }
    }

    private IEnumerator CheckpointVignette()
    {
        float timer = 0;
        while (timer < checkpointVignetteShowTime / 3)
        {
            timer += Time.deltaTime;
            checkpointPPR.weight = timer / (checkpointVignetteShowTime / 3);
            yield return null;
        }

        yield return new WaitForSeconds(checkpointVignetteShowTime / 3);
        timer = 0;
        while (timer < checkpointVignetteShowTime / 3)
        {
            timer += Time.deltaTime;
            checkpointPPR.weight = 1 - (timer / (checkpointVignetteShowTime / 3));
            yield return null;
        }

        checkpointPPR.weight = 0;

    }
}
