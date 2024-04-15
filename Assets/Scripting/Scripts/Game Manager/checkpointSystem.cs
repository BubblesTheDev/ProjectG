using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private GameObject orientationObj;

    private bool debugEnabled;

    private void Awake()
    {
        if(GameObject.Find("Player")) playerObj = GameObject.Find("Player");
        if(GameObject.Find("Orientation")) orientationObj = GameObject.Find("Orientation");
        if(playerObj != null)
        {
            if (checkPointSpawnPositions.Count > 0 && PlayerPrefs.GetInt("checkpointIndex") <= checkPointSpawnPositions.Count)
            {
                playerObj.transform.position = checkPointSpawnPositions[PlayerPrefs.GetInt("checkpointIndex")].transform.position;
                orientationObj.transform.rotation = checkPointSpawnPositions[PlayerPrefs.GetInt("checkpointIndex")].transform.rotation;
            }
        } 
    }

    void Update()
    {
        debugOptions();
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

    void debugOptions()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Time.timeScale = 0;
            debugEnabled = true;
            print("Entering Checkpoint Debug");
        }
        else if (Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            Time.timeScale = 1;
            debugEnabled = false;
            print("Exiting Checkpoint Debug");

            updateCheckpoint(checkPointIndex);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (debugEnabled)
        {
            if(Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                checkPointIndex++;
                print("You are moving to checkpoint index " + checkPointIndex);
                if (checkPointIndex == checkPointSpawnPositions.Count) checkPointIndex = 0;

                orientationObj.transform.position = checkPointSpawnPositions[checkPointIndex].transform.position;
            }
            else if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                checkPointIndex--;
                print("You are moving to checkpoint index " + checkPointIndex);
                if (checkPointIndex < 0) checkPointIndex = checkPointSpawnPositions.Count;

                orientationObj.transform.position = checkPointSpawnPositions[checkPointIndex].transform.position;
            }
        }
    }
}
