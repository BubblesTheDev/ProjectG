using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject[] imageArray;
    [SerializeField]private int CurrentImage;

    float deltaTime = 0.0f;
    public float timer = 5.0f;
    public float timerRemaining = 5.0f;
    public bool timerIsRunning = false;
    public string timerText;
    public int SceneBuildIndex;

    private void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        Rect imageRect = new Rect(0, 0, Screen.width, Screen.height);
        if(CurrentImage - 1 >= 0) imageArray[CurrentImage - 1].SetActive(false);
        if (CurrentImage < imageArray.Length) imageArray[CurrentImage].SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        CurrentImage = 0;
         timerIsRunning = true;
        timerRemaining = timer;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * .1f;
        if (Input.GetKeyDown(KeyCode.Space)) {
            CurrentImage++;
            if (CurrentImage >= imageArray.Length) {
                SceneManager.LoadScene(SceneBuildIndex, LoadSceneMode.Single);
            }
        }
    }
}
