using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class tutorialTextBoxes : MonoBehaviour
{
    [SerializeField] private GameObject tutorialTextObj;
    [SerializeField, TextArea(3, 5)] private string tutorialText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (tutorialTextObj != null)
            {
                tutorialTextObj.SetActive(true);
                tutorialTextObj.GetComponent<TextMeshPro>().text = tutorialText;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (tutorialTextObj != null)
            {
                tutorialTextObj.SetActive(false);
                tutorialTextObj.GetComponent<TextMeshPro>().text = null;
            }
        }

    }
}
