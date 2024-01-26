using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class enemyStats : MonoBehaviour
{
    [Header("Stats")]
    public float currentHP;
    public float maxHp;
    public float moveSpeed;

    [Space, Header("Feedback Variables")]
    [SerializeField] private float timeToDie = 1f;
    [SerializeField] private ParticleSystem[] VFX_onHit;
    [SerializeField] private ParticleSystem[] VFX_onDeath;
    [SerializeField] private GameObject enemyGFX;

    [HideInInspector] public roomEnemySpawner spawner;
    private NavMeshAgent ref_NavMeshAgent;
    private Collider temp_enemyCollider;

    [HideInInspector] public UnityEvent enemyDamageTaken;
    [HideInInspector] public UnityEvent enemyDeath;

    private void Awake()
    {
        ref_NavMeshAgent = GetComponent<NavMeshAgent>();
        temp_enemyCollider = GetComponent<Collider>();
        currentHP = maxHp;
        ref_NavMeshAgent.speed = moveSpeed;
    }
    private void Update()
    {
        if (currentHP <= 0) StartCoroutine(die());
    }

    IEnumerator die()
    {
        temp_enemyCollider.enabled = false;
        ref_NavMeshAgent.isStopped = true;
        enemyGFX.SetActive(false);
        enemyDamageTaken?.Invoke();

        foreach (ParticleSystem deathVFX in VFX_onDeath)
        {
            if(deathVFX != null) deathVFX.Play();
            yield return null;
        }

        yield return new WaitForSeconds(timeToDie);
        Destroy(gameObject);
    }

    public void takeDamage(float damageToTake)
    {
        enemyDeath?.Invoke();
        currentHP -= damageToTake;
    }
}
