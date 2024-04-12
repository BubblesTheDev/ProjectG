using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroCutsceneParallax : MonoBehaviour
{

    [SerializeField] private float moveAmt, moveTime, sizeChange;
    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private Vector2 startingImagePos;
    [SerializeField] private Vector3 startingSize;
    [SerializeField] private bool isEnabled, isVertical;
    public int w;

    // Start is called before the first frame update
    void Start()
    {
        isEnabled = false;
        startingImagePos = gameObject.GetComponent<RectTransform>().position;
        startingSize = gameObject.GetComponent<RectTransform>().localScale;
        w = Screen.width;
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
            if(!isVertical) gameObject.GetComponent<RectTransform>().position = new Vector2(Mathf.Lerp(startingImagePos.x, startingImagePos.x + (w / moveAmt), movement), startingImagePos.y);
            else gameObject.GetComponent<RectTransform>().position = new Vector2(startingImagePos.x, Mathf.Lerp(startingImagePos.y, startingImagePos.y + (w / moveAmt), movement));
            if (sizeChange > 0) gameObject.GetComponent<RectTransform>().localScale = Vector3.Lerp(startingSize, new Vector3(startingSize.x + (sizeChange), startingSize.y + (sizeChange), startingSize.z + (sizeChange)), movement);
            yield return null;
        }
    }

}
