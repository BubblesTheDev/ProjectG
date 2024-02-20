using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyGameObject", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
