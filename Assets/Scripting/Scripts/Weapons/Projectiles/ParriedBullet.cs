using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParriedBullet : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float distanceToKill, speed;
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
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<enemyStats>().takeDamage(damage);
            Destroy(gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Enviroment")) Destroy(gameObject);
    }


}
