using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimBandaidFix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("EnableObj", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnableObj()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
}
