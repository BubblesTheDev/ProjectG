/*using System.Collections;
using System.Collections.Generic;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Act1EndTranAct2 : MonoBehaviour
{
    public int SceneBuildIndex;
    playerHealth  playerCheck;

    private void OnTriggerEnter(Collider other)
    {

        playerCheck = GameObject.Find("Player").GetComponent<playerHealth>();

        if (other.tag == "Player" && playerCheck.currentHP <= 0) {
            SceneManager.LoadScene(SceneBuildIndex, LoadSceneMode.Single);
        
        }
    }
}
*/