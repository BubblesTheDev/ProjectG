using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.VFX;

public class enemyStats : MonoBehaviour
{
    [Header("Stats")]
    public float currentHP;
    public float maxHp;
    public float moveSpeed;

    [Space, Header("Feedback Variables")]
    [SerializeField] private float timeToDie = 1f;
    [SerializeField] private VisualEffect[] VFX_onHit;
    [SerializeField] private VisualEffect[] VFX_onDeath;
    [SerializeField] private GameObject enemyGFX;
    [SerializeField] private float staticToGive = 35f;
    [SerializeField] private enemyType type;

    [HideInInspector] public roomEnemySpawner spawner;
    private NavMeshAgent ref_NavMeshAgent;
    private Collider temp_enemyCollider;
    private playerHealth playerStats;
    private bool isDead = false;

    [HideInInspector] public UnityEvent enemyDamageTaken;
    [HideInInspector] public UnityEvent enemyDeath;

    private void Awake()
    {
        if(GetComponent<NavMeshAgent>()) ref_NavMeshAgent = GetComponent<NavMeshAgent>();
        playerStats = GameObject.Find("Player").GetComponent<playerHealth>();
        temp_enemyCollider = GetComponent<Collider>();
        currentHP = maxHp;
        if(ref_NavMeshAgent != null) ref_NavMeshAgent.speed = moveSpeed;
    }
    private void Update()
    {
        if (currentHP <= 0 && !isDead) StartCoroutine(die());
    }

    IEnumerator die()
    {
        isDead = true;
        if(VFX_onDeath.Length > 0)
        {
            foreach (VisualEffect deathVFX in VFX_onDeath)
            {
                if (deathVFX != null)
                {
                    deathVFX.transform.parent = null;
                    deathVFX.Play();
                }
            }
        }

        playerStats.currentStaticEnergy += staticToGive;

        temp_enemyCollider.enabled = false;
        if(ref_NavMeshAgent != null) ref_NavMeshAgent.isStopped = true;
        if(enemyGFX != null) enemyGFX.SetActive(false);
        enemyDeath?.Invoke();
        yield return new WaitForSeconds(timeToDie);
        Destroy(gameObject);
    }

    public void takeDamage(float damageToTake)
    {
        enemyDamageTaken?.Invoke();
        currentHP -= damageToTake;
        StartCoroutine(bloodVFX());

        switch (type)
        {
            case enemyType.herc:
                AudioManager.instance.PlaySFX(FMODEvents.instance.bruiserHit, transform.position);
                break;
            case enemyType.cerb:
                AudioManager.instance.PlaySFX(FMODEvents.instance.bruiserHit, transform.position);
                break;
            case enemyType.seeker:
                AudioManager.instance.PlaySFX(FMODEvents.instance.bruiserHit, transform.position);
                break;
            case enemyType.turret:
                break;
        }

    }

    IEnumerator bloodVFX()
    {
        if (VFX_onHit.Length > 0)
        {
            foreach (VisualEffect dmgVFX in VFX_onHit)
            {
                if (dmgVFX != null)
                {
                    dmgVFX.Play();
                }
            }
        }
        yield return null;
    }

}

enum enemyType
{
    none, 
    herc,
    cerb,
    seeker,
    turret
}
