using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checkpointSystem : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public List<GameObject> checkPointSpawnPositions = new List<GameObject>();
    public int checkPointIndex { get; private set; }

    [Space, Header("Graphics Settings")]
    public Image checkPointImage;
    public float timeToFadeIn = .5f;
    public float timeToWait = 2f;
    public float timeToFadeOut = .25f;
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

    public IEnumerator getCheckpoint()
    {
        if (checkPointImage == null)
        {
            Debug.LogWarning("There is no image for the checkpoint graphic set for this script on object named " + this.name);
            yield break;
        }

        Color colorToEdit = new Color(255, 255, 255, 255);
        Color fullColor = new Color(255, 255, 255, 255);
        Color emptyColor = new Color(255, 255, 255, 0);
        float time = 0;

        while(time < timeToFadeIn)
        {

            colorToEdit = new Color(255, 255, 255, Mathf.Lerp(0, 255, time / timeToFadeIn));
            checkPointImage.color = colorToEdit;
            time += Time.deltaTime;
            yield return null;
        }
        checkPointImage.color = fullColor;

        yield return new WaitForSeconds(timeToWait);

        time = 0;
        while (time < timeToFadeIn)
        {
            colorToEdit = new Color(255, 255, 255, Mathf.Lerp(255, 0, time / timeToFadeIn));
            checkPointImage.color = colorToEdit;
            time += Time.deltaTime;
            yield return null;
        }
        checkPointImage.color = emptyColor;
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
