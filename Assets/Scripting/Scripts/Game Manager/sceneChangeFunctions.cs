using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneChangeFunctions : MonoBehaviour
{
    [SerializeField] private string scenePathToLoadOnTrigger;

    public void loadSceneFromIndex(int sceneIndexToLoad)
    {
        SceneManager.LoadScene(sceneIndexToLoad); 
        //else Debug.LogWarning("There is no scene in the build settings set to index " + sceneIndexToLoad);
    }

    public void loadSceneFromName(string sceneNameToLoad)
    {
        SceneManager.LoadScene(sceneNameToLoad);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GameObject.Find("Player").GetComponent<playerHealth>().deathSceneIndex = SceneUtility.GetBuildIndexByScenePath(scenePathToLoadOnTrigger);
            //else Debug.LogWarning("There is no scene in the build settings set to index " + sceneIndexToLoadOnTrigger);
        }
    }
}
