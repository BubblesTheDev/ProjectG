using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class killBox : MonoBehaviour
{
    [SerializeField] private float timeToFade;
    [SerializeField] private float timeToReturn;
    [SerializeField] Image fadeToBlackImg;

    private checkpointSystem checkPointRef;
    private playerHealth health;
    private GameObject playerObj;
    private playerMovement movement;

    private void Awake()
    {
        checkPointRef = FindObjectOfType<checkpointSystem>();
        health = FindObjectOfType<playerHealth>();
        playerObj = GameObject.Find("Player");
        movement = playerObj.GetComponent<playerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(resetPos());
    }

    IEnumerator resetPos()
    {
        float time = 0;
        if(fadeToBlackImg != null || timeToFade == 0) 
        {
            //This fades an overlay immage to black while they fall as if blacking out
            while (time < timeToFade)
            {
                fadeToBlackImg.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, time / timeToFade);
                time += Time.deltaTime;
                yield return null;
            }
        }

        movement.vertical_playerVelocity *= 0f;

        //Resets the player gravity flip

        if (movement.current_PlayerRotationState == playerRotationState.nonFlipped)
        {
            movement.current_PlayerRotationState = playerRotationState.flipped;
        }
        else
        {
            GameObject.Find("CameraHolder").transform.eulerAngles += new Vector3(0, 0, 180);
            playerObj.transform.localEulerAngles += new Vector3(0, 0, 180);
            movement.current_PlayerRotationState = playerRotationState.nonFlipped;
        }

        //This will deal 2 damage to the player but wont kill them
        if (health.currentHP > 2 ) StartCoroutine(health.takeDamage(2));
        else if(health.currentHP > 1 ) StartCoroutine(health.takeDamage(1));
        else if (health.currentHP == 1) StartCoroutine(health.takeDamage(0));

        //This teleports the player to last checkpoint
        if(checkPointRef.checkPointSpawnPositions.Count > checkPointRef.checkPointIndex 
            && checkPointRef.checkPointSpawnPositions[checkPointRef.checkPointIndex] != null)
        {
            playerObj.transform.position = checkPointRef.checkPointSpawnPositions[checkPointRef.checkPointIndex].transform.position;
            playerObj.transform.rotation = checkPointRef.checkPointSpawnPositions[checkPointRef.checkPointIndex].transform.rotation;
        }

        if (fadeToBlackImg != null || timeToReturn == 0)
        {
            //Fades camera back in
            time = 0;
            while (time < timeToReturn)
            {
                fadeToBlackImg.color = Color.Lerp(Color.black, new Color(0, 0, 0, 0), time / timeToReturn);
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}
