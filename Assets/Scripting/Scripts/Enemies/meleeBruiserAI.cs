using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.InputSystem.iOS;

public class meleeBruiserAI : MonoBehaviour
{
    [Header("Debug Things")]
    [SerializeField] private bool canPunch;
    [SerializeField] private float punchAttackCooldown, hitStunDuration;
    [SerializeField] private float hitstunTime;

    [Header("Punch Attack Stats")]
    [SerializeField] private float hitboxActiveTime;
    [SerializeField] private int damage;
    [SerializeField] private float distanceToPunchPlayer;
    [SerializeField] private Collider punchHitbox;
    [SerializeField] private float walkingAnimThreshold;
    private bool hasHitPlayerThisAttack = false;


    #region Assignables
    private NavMeshAgent ref_NavMeshAgent;
    private GameObject ref_PlayerObj;
    private playerHealth ref_PlayerStats;
    private Animator ref_meleeAnimator;
    private enemyStats ref_EnemyStats;
    private Collider ref_playerCollider;
    #endregion

    private void Awake()
    {
        ref_NavMeshAgent = GetComponent<NavMeshAgent>();
        ref_PlayerObj = GameObject.Find("Player");
        ref_PlayerStats = ref_PlayerObj.GetComponent<playerHealth>();
        ref_meleeAnimator = GetComponent<Animator>();
        ref_EnemyStats = GetComponent<enemyStats>();
        ref_playerCollider = ref_PlayerObj.GetComponent<Collider>();
        ref_EnemyStats.enemyDamageTaken.AddListener(delegate { hitstunStart(); });
    }

    private void Update()
    {
        ref_NavMeshAgent.SetDestination(new Vector3(ref_PlayerObj.transform.position.x, transform.position.y, ref_PlayerObj.transform.position.z));

        if (ref_NavMeshAgent.velocity.magnitude > walkingAnimThreshold) ref_meleeAnimator.Play("HercRunning", 0);
        else ref_meleeAnimator.Play("HercIdle", 0);
        if (Vector3.Distance(ref_PlayerObj.transform.position, transform.position) < distanceToPunchPlayer && canPunch)
        {
            StartCoroutine(attackPlayer());
        }
    }

    public void enablePunchHitbox()
    {
        punchHitbox.enabled = true;
    }

    public void disablePunchHitbox()
    {
        punchHitbox.enabled = false;
    }

    IEnumerator attackPlayer()
    {
        canPunch = false;
        ref_meleeAnimator.SetLayerWeight(1, 1);
        ref_meleeAnimator.Play("HercPunch",1);
        

        float temp = 0;
        while(temp < hitboxActiveTime)
        {
            if(punchHitbox.bounds.Intersects(ref_playerCollider.bounds) && !hasHitPlayerThisAttack) 
            {
                hasHitPlayerThisAttack = true;
                StartCoroutine(ref_PlayerStats.takeDamage(damage));
            }

            temp += Time.deltaTime;
            yield return null;

        }

        ref_meleeAnimator.SetLayerWeight(1, 0);

        disablePunchHitbox();
        yield return new WaitForSeconds(punchAttackCooldown);
        canPunch = true;
        hasHitPlayerThisAttack = false;
    }

    void hitstunStart()
    {
        StartCoroutine(hitstun());
    }

    IEnumerator hitstun()
    {
        ref_NavMeshAgent.isStopped = true;
        ref_NavMeshAgent.velocity = Vector3.zero;
        ref_meleeAnimator.SetLayerWeight(2,1);
        ref_meleeAnimator.Play("HercHitStunned", 2);
        float time = 0;
        while (time < hitstunTime)
        {
            canPunch = false;
            time += Time.deltaTime;
            yield return null;
        }
        ref_NavMeshAgent.isStopped = false;
        ref_meleeAnimator.SetLayerWeight(2, 0);

    }


}
