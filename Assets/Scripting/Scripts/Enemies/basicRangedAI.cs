using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class basicRangedAI : MonoBehaviour
{

    [Header("Basic Stats")]
    [SerializeField] private bool canShoot = true;
    [SerializeField] private List<GameObject> firePoints;
    [SerializeField] private GameObject enemyBullet;
    [SerializeField] private Animator currentAnimator;
    [SerializeField] private float closeRange, midRange, longRange;
    [SerializeField] private float damage;

    [Header("Short Range Shooting")]
    [SerializeField] private float fireCooldownShortRange;
    [SerializeField] private float fireRateShortRange;
    [SerializeField] private float numShotsShortRange;

    [Header("Long Range Shooting")]
    [SerializeField] private float fireCooldownLongRange;
    [SerializeField] private float fireRateLongRange;
    [SerializeField] private float numShotsLongRange;



    #region Assignables
    private NavMeshAgent ref_NavMeshAgent;
    private GameObject ref_PlayerObj;
    private Animator ref_rangedAnimator;
    private enemyStats ref_EnemyStats;
    #endregion


    private void Awake()
    {
        ref_PlayerObj = GameObject.Find("Player");
        ref_EnemyStats = GetComponent<enemyStats>();
        ref_NavMeshAgent = GetComponent<NavMeshAgent>();
        ref_rangedAnimator = GetComponent<Animator>();
    }

    private void Update()
    {

    }
}