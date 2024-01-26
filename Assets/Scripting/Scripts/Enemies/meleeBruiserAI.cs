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

    [Header("Punch Attack Stats")]
    [SerializeField] private float hitboxActiveTime;
    [SerializeField] private string punchAnimName;
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
    }

    private void Update()
    {
        ref_NavMeshAgent.SetDestination(new Vector3(ref_PlayerObj.transform.position.x, transform.position.y, ref_PlayerObj.transform.position.z));

        //if (ref_NavMeshAgent.velocity.magnitude > walkingAnimThreshold) ref_meleeAnimator.Play("BasicMeleeWalk", 0);
        if(Vector3.Distance(ref_PlayerObj.transform.position, transform.position) < distanceToPunchPlayer && canPunch)
        {
            StartCoroutine(attackPlayer());
        }
    }

    public void enablePunchHitbox()
    {
        punchHitbox.enabled = true;
        //print("enabled hitbox for " + name);
    }

    public void disablePunchHitbox()
    {
        punchHitbox.enabled = false;
        //print("disabled hitbox for " + name);
    }

    IEnumerator attackPlayer()
    {
        canPunch = false;
        ref_meleeAnimator.Play(punchAnimName,0);
        
        //print(transform.name + " is punching");

        float temp = 0;
        while(temp < hitboxActiveTime)
        {
            if(punchHitbox.bounds.Intersects(ref_playerCollider.bounds) && !hasHitPlayerThisAttack) 
            {
                print("hit player");
                hasHitPlayerThisAttack = true;
                StartCoroutine(ref_PlayerStats.takeDamage(damage));
            }

            temp += Time.deltaTime;
            yield return null;

        }

        //print("finished punch");

        disablePunchHitbox();
        yield return new WaitForSeconds(punchAttackCooldown);
        canPunch = true;
        hasHitPlayerThisAttack = false;
    }

    
}
