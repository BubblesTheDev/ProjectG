using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checkPointBox : MonoBehaviour
{
    [SerializeField] private int checkpointIndex;
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
        }
    }
}
