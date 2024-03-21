using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using TMPro;
using UnityEngine.UIElements;

public class voicelineSystem : MonoBehaviour
{
    [SerializeField] private GameObject subtitleBox;
    [SerializeField] private GameObject speakerPosition;
    [Space]
    [SerializeField] private List<voiceLine> voiceLines;
    private bool isPlaying;
    private bool started;
    private bool finished;
    private int voiceLineIndex = 0;
    IEnumerator playVoiceline()
    {
        isPlaying = true;
        subtitleBox.GetComponent<TextMeshProUGUI>().text = voiceLines[voiceLineIndex].subTitleText;
        AudioManager.instance.playVoiceline(voiceLines[voiceLineIndex].audioClipIndex);
        yield return new WaitForSeconds(voiceLines[voiceLineIndex].clipLength);
        voiceLineIndex++;
        isPlaying = false;
    }


    private void Update()
    {
        if (voiceLineIndex < voiceLines.Count && started && !isPlaying) 
        {
            StartCoroutine(playVoiceline());
        } else if(voiceLineIndex >= voiceLines.Count && !finished)
        {
            subtitleBox.SetActive(false);
            finished = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!started && voiceLineIndex < voiceLines.Count)
            {
                started = true;
                subtitleBox.SetActive(true);
            }
        }
    }
}

[System.Serializable]
public struct voiceLine
{
    public int audioClipIndex;
    public float clipLength;
    [TextArea(3,5)] public string subTitleText;
}
