using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            brain.updateCheckpoint(checkpointIndex);
        }
    }
}
