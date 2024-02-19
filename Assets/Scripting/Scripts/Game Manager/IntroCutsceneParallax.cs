using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroCutsceneParallax : MonoBehaviour
{

    [SerializeField] private float moveAmt;
    [SerializeField] private float moveTime;
    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private Vector2 startingImagePos;
    [SerializeField] private bool isEnabled;

    // Start is called before the first frame update
    void Start()
    {
        isEnabled = false;
        startingImagePos = gameObject.GetComponent<RectTransform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled && isActiveAndEnabled) isEnabled = true;
        if (isEnabled) StartCoroutine(playCutscene());
    }

    private IEnumerator playCutscene()
    {
        float timer = 0;

        while (timer < moveTime)
        {
            timer += Time.deltaTime;
            float movement = movementCurve.Evaluate(timer / moveTime);
            gameObject.GetComponent<RectTransform>().position = new Vector2(Mathf.Lerp(startingImagePos.x, startingImagePos.x + moveAmt, movement), startingImagePos.y);
            yield return null;
        }
    }

}
