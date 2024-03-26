using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Act1EndTranAct2 : MonoBehaviour
{
    public int SceneBuildIndex = 3;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            SceneManager.LoadScene(SceneBuildIndex, LoadSceneMode.Single);
        }
    }
}
