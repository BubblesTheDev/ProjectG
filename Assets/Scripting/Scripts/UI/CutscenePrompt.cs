using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutscenePrompt : MonoBehaviour
{
    [SerializeField] private NewBehaviourScript timerScript;
    // Start is called before the first frame update
    void Awake()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if (timerScript.timerRemaining <= 0)
        {
            FadeInText();
            gameObject.GetComponent<TextMeshProUGUI>().color = Color.Lerp(Color.white, new Color(255, 255, 255, 0), Mathf.PingPong(Time.time, 1));
        }
        else gameObject.GetComponent<TextMeshProUGUI>().color = Color.clear;
    }

    private IEnumerator FadeInText()
    {
        gameObject.GetComponent<TextMeshProUGUI>().color = Color.Lerp(new Color(255, 255, 255, 0), Color.white, 0.5f);
        return null;
    }
}
