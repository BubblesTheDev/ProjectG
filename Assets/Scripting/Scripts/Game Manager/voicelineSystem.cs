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
    private int voiceLineIndex;
    IEnumerator playVoiceline()
    {
        isPlaying = true;
        subtitleBox.GetComponent<TextMeshPro>().text = voiceLines[voiceLineIndex].subTitleText;
        EventReference tempReference = RuntimeManager.PathToEventReference(voiceLines[voiceLineIndex].audioKey);
        if (speakerPosition != null) RuntimeManager.PlayOneShot(tempReference, speakerPosition.transform.position);
        else RuntimeManager.PlayOneShot(tempReference);
        yield return new WaitForSeconds(voiceLines[voiceLineIndex].clipLength);
        voiceLineIndex++;
        isPlaying = false;
    }


    private void Update()
    {
        if (voiceLineIndex < voiceLines.Count && started && !isPlaying) 
        {
            StartCoroutine(playVoiceline());
        } else if(voiceLineIndex >= voiceLines.Count)
        {
            subtitleBox.SetActive(false);
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
