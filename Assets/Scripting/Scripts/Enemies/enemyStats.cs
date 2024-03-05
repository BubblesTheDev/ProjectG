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

    [HideInInspector] public roomEnemySpawner spawner;
    private NavMeshAgent ref_NavMeshAgent;
    private Collider temp_enemyCollider;

    [HideInInspector] public UnityEvent enemyDamageTaken;
    [HideInInspector] public UnityEvent enemyDeath;

    private void Awake()
    {
        if(GetComponent<NavMeshAgent>()) ref_NavMeshAgent = GetComponent<NavMeshAgent>();
        temp_enemyCollider = GetComponent<Collider>();
        currentHP = maxHp;
        if(ref_NavMeshAgent != null) ref_NavMeshAgent.speed = moveSpeed;
    }
    private void Update()
    {
        if (currentHP <= 0) StartCoroutine(die());
    }

    IEnumerator die()
    {
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
        AudioManager.instance.PlaySFX(FMODEvents.instance.bruiserHit, this.transform.position);
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
