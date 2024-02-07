using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunnerProjectile : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float distanceToKill;
    private Vector3 startPos;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(startPos, transform.position) > distanceToKill) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) other.gameObject.GetComponent<playerHealth>().startTakingDMG(damage);
        if(other.gameObject.layer == LayerMask.NameToLayer("Enviroment")) Destroy(gameObject);
    }

}
