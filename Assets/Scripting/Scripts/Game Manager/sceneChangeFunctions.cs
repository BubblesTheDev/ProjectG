using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneChangeFunctions : MonoBehaviour
{
    [SerializeField] private int sceneIndexToLoadOnTrigger;

    public void loadScene(int sceneIndexToLoad)
    {
        if (SceneManager.GetSceneAt(sceneIndexToLoad) != null) loadScene(sceneIndexToLoad); 
        else Debug.LogWarning("There is no scene in the build settings set to index " + sceneIndexToLoad);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (SceneManager.GetSceneAt(sceneIndexToLoadOnTrigger) != null) loadScene(sceneIndexToLoadOnTrigger);
            else Debug.LogWarning("There is no scene in the build settings set to index " + sceneIndexToLoadOnTrigger);
        }
    }
}
