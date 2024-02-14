using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class seekerAI : MonoBehaviour
{


    [Header("Debug Things")]
    [SerializeField] private seekerAIStates currentAIState;
    [SerializeField] private bool attackMode;
    [SerializeField] private bool canUseSlash = true, canUseLeap = true, canUseDash = true;
    [SerializeField] private float slashAttackCooldown, leapAttackCooldown, dashMovmentCooldown, hitStunDuration;

    [Header("Dash Movement Stats")]
    [SerializeField] private float distanceToStartDash;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float numOfDashes;
    [SerializeField] private float timeBetweenDash;
    [SerializeField] private LayerMask enviromentLayer;
    [SerializeField] private TrailRenderer[] movementTrails;

    [Header("Slash Attack Stats")]
    [SerializeField] private int slashDamage;
    [SerializeField] private bool hitboxActive_Slash;
    [SerializeField] private float timeBeforeSlashAttack;
    [SerializeField] private float distanceToSlash;
    [SerializeField] private float slashTime;
    [SerializeField] private Collider slashAttackHitbox;
    private bool hasHitWithSlash;

    [Header("Leap Attack Stats")]
    [SerializeField] private int leapDamage;
    [SerializeField] private AnimationCurve leapCurve;
    [SerializeField] private bool hitboxActive_Leap;
    [SerializeField] private Collider leapAttackHitbox;
    [SerializeField] private float time_lineUpLeap, time_leapReaction, time_calmDown;
    [SerializeField] private bool playerIsVisable;

    #region Assignables
    private NavMeshAgent ref_NavMeshAgent;
    private GameObject ref_PlayerObj;
    private playerMovement ref_PlayerMovement;
    private playerHealth ref_PlayerStats;
    private Animator ref_seekerAnimator;
    private Collider ref_playerCollider;
    #endregion

    private void Awake()
    {
        ref_NavMeshAgent = GetComponent<NavMeshAgent>();
        ref_PlayerObj = GameObject.Find("Player");
        ref_PlayerMovement = ref_PlayerObj.GetComponent<playerMovement>();
        ref_PlayerStats = ref_PlayerObj.GetComponent<playerHealth>();
        ref_playerCollider = ref_PlayerObj.GetComponent<Collider>();
    }

    private void OnDrawGizmosSelected()
    {
        #region Dash Debug
        #endregion


    }

    private void Update()
    {
        if (currentAIState == seekerAIStates.following)
        {
            ref_NavMeshAgent.SetDestination(ref_PlayerObj.transform.position);
            //if (Mathf.Abs(ref_NavMeshAgent.velocity.magnitude) > 0) ref_seekerAnimator.Play("SeekerWalk");
            //else ref_seekerAnimator.Play("SeekerIdle");
        }
        if(Vector3.Distance(transform.position, new Vector3(ref_PlayerObj.transform.position.x, transform.position.y, ref_PlayerObj.transform.position.z)) < distanceToSlash)
        {
            StartCoroutine(action_Slash());
        }


        /*if (Vector3.Distance(transform.position, new Vector3(ref_PlayerObj.transform.position.x, transform.position.y, ref_PlayerObj.transform.position.z)) < distanceToStartDash && canUseDash)
        {
            StartCoroutine(action_Dash());
        }*/


    }

    IEnumerator action_Dash()
    {
        Vector3 playerPosition = ref_PlayerObj.transform.position;
        currentAIState = seekerAIStates.dashing;
        ref_NavMeshAgent.isStopped = true;
        ref_NavMeshAgent.velocity *= 0;
        canUseDash = false;

        
     

       

        currentAIState = seekerAIStates.following;
        ref_NavMeshAgent.isStopped = false;
        yield return new WaitForSeconds(dashMovmentCooldown);
        canUseDash = true;
    }

    IEnumerator action_Slash()
    {
        currentAIState = seekerAIStates.slashing;
        ref_NavMeshAgent.isStopped = true;
        canUseSlash = false;

        ref_seekerAnimator.Play("seekerSlash");


        float temp = 0;
        while (temp < slashTime)
        {
            if (slashAttackHitbox.bounds.Intersects(ref_playerCollider.bounds) && !hasHitWithSlash)
            {
                hasHitWithSlash = true;
                StartCoroutine(ref_PlayerStats.takeDamage(slashDamage));
            }

            temp += Time.deltaTime;
            yield return null;

        }

        ref_NavMeshAgent.isStopped = false;
        currentAIState = seekerAIStates.following;
        yield return new WaitForSeconds(slashAttackCooldown);
        canUseSlash = true;
    }

    IEnumerator action_Leap()
    {
        currentAIState = seekerAIStates.leaping;
        ref_NavMeshAgent.isStopped = true;
        canUseLeap = false;

        ref_seekerAnimator.Play("leapStartPose");
        yield return new WaitForSeconds(ref_seekerAnimator.GetCurrentAnimatorClipInfo(0).Length);

        #region Line Up Leap
        
        //Start lining up leap
        float time = 0;
        while(time < time_lineUpLeap)
        {
            transform.LookAt(new Vector3(ref_PlayerObj.transform.position.x, transform.position.y, ref_PlayerObj.transform.position.z), Vector3.up);



            time += Time.deltaTime;
            yield return null;
        }
        #endregion

        //Launch at the player
        #region launch at player
        #endregion
    }

    public void toggle_HitboxActive_Slash()
    {
        hitboxActive_Slash = !hitboxActive_Slash;
    }
    public void toggle_HitboxActive_Leap()
    {
        hitboxActive_Leap = !hitboxActive_Leap;
    }
}

[System.Serializable]
enum seekerAIStates
{
    following,
    dashing,
    slashing,
    leaping,
    hitstun
}
