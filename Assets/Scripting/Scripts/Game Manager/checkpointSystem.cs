using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpointSystem : MonoBehaviour
{
    public int checkPointIndex { get; private set; }
    public List<GameObject> checkPointSpawnPositions = new List<GameObject>();

    private GameObject playerObj;
    

    private void Awake()
    {
        playerObj = GameObject.Find("Player");
        if(playerObj != null)
        {
            if (checkPointSpawnPositions.Count > 0 && PlayerPrefs.GetInt("checkpointIndex") <= checkPointSpawnPositions.Count)
            {
                playerObj.transform.position = checkPointSpawnPositions[PlayerPrefs.GetInt("checkpointIndex")].transform.position;
                playerObj.transform.rotation = checkPointSpawnPositions[PlayerPrefs.GetInt("checkpointIndex")].transform.rotation;
            }
        } 
    }

    public void updateCheckpoint(int checkPointIndexToGive)
    {
        PlayerPrefs.SetInt("checkpointIndex", checkPointIndexToGive);
        checkPointIndex = checkPointIndexToGive;
    }

    public void resetCheckpoint()
    {
        checkPointIndex = 0;
        PlayerPrefs.SetInt("checkpointIndex", checkPointIndex);
    }
}
