using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPointClearTrigger : MonoBehaviour
{
    private checkpointSystem _system;

    private void Awake()
    {
        _system = GameObject.Find("GameManager").GetComponent<checkpointSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) _system.resetCheckpoint();
    }
}
