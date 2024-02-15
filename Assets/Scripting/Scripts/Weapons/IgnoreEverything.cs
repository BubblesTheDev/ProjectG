using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreEverything : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.layer.Equals("BlackHoleParticleCollider"))
        Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
    }

}
