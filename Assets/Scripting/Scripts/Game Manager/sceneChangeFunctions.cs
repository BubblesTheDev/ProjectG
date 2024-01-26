using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneChangeFunctions : MonoBehaviour
{
    [SerializeField] private int sceneIndexToLoadOnTrigger;

    public void loadSceneFromIndex(int sceneIndexToLoad)
    {
        SceneManager.LoadScene(sceneIndexToLoad); 
        //else Debug.LogWarning("There is no scene in the build settings set to index " + sceneIndexToLoad);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GameObject.Find("Player").GetComponent<playerHealth>().deathSceneIndex = sceneIndexToLoadOnTrigger;
            //else Debug.LogWarning("There is no scene in the build settings set to index " + sceneIndexToLoadOnTrigger);
        }
    }
}
