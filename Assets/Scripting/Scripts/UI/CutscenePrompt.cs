using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutscenePrompt : MonoBehaviour
{
    [SerializeField] private NewBehaviourScript timerScript;
    private bool startFadeIn = false;
    // Start is called before the first frame update
    void Awake()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled && startFadeIn)
        {
            gameObject.GetComponent<TextMeshProUGUI>().color = Color.Lerp(new Color(255, 255, 255, 0), Color.white, Mathf.PingPong(Time.time, 1));
        }
        else gameObject.GetComponent<TextMeshProUGUI>().color = Color.clear;
    }

    private void OnEnable()
    {
        StartCoroutine(FadeInText());
    }

    private IEnumerator FadeInText()
    {
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<TextMeshProUGUI>().color = Color.Lerp(new Color(255, 255, 255, 0), Color.white, 0.5f);
        startFadeIn = true;
    }
}
