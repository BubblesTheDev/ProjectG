using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeUI : MonoBehaviour
{
    [SerializeField] private AnimationCurve shakeAnimCurve;
    [SerializeField] private float shakeStrength;
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = gameObject.GetComponent<RectTransform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled) StartCoroutine(ShakeCutscene());
    }

    private IEnumerator ShakeCutscene()
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            float shakeCurve = shakeAnimCurve.Evaluate(timer / 1);
            if (Time.frameCount % 5 == 0) gameObject.GetComponent<RectTransform>().position = startPos + shakeCurve * shakeStrength * Random.insideUnitSphere;
            yield return null;
        }
        transform.position = startPos;
    }
}
