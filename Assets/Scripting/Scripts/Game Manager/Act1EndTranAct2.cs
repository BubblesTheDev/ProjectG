using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Act1EndTranAct2 : MonoBehaviour
{
    public int SceneBuildIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            SceneManager.LoadScene(SceneBuildIndex, LoadSceneMode.Single);
        
        }
    }
}
