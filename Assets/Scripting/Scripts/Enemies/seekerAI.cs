using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class seekerAI : MonoBehaviour
{


    [Header("Debug Things")]
    [SerializeField] private seekerAIStates currentAIState;
    [SerializeField] private bool canUseSlash = true, canUseLeap = true, canUseDash = true;
    [SerializeField] private float slashAttackCooldown, leapAttackCooldown, dashMovmentCooldown, hitStunDuration;

    [Header("Dash Movement Stats")]
    [SerializeField] private float distanceToStartDash;
    [SerializeField] private float timeBeforeDash;
    [SerializeField] private int numDashes;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float timeBetweenDash;
    [SerializeField] private Collider dashCollider;
    [SerializeField] private TrailRenderer[] movementTrails;

    [Header("Slash Attack Stats")]
    [SerializeField] private int slashDamage;
    [SerializeField] private bool hitboxActive_Slash;
    [SerializeField] private float timeBeforeSlashAttack;
    [SerializeField] private float distanceToSlash;
    [SerializeField] private float slashTime;
    [SerializeField] private Collider slashAttackHitbox;
    private bool hasHitWithSlash;

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
        ref_seekerAnimator = GetComponent<Animator>();

        ref_PlayerMovement.onAction_Dash_Start.AddListener(delegate { StartCoroutine(action_Dash()); });
    }

    private void Update()
    {
        if (currentAIState == seekerAIStates.following)
        {
            ref_NavMeshAgent.SetDestination(new Vector3(ref_PlayerObj.transform.position.x, transform.position.y, ref_PlayerObj.transform.position.z));
            if (Mathf.Abs(ref_NavMeshAgent.velocity.magnitude) > 0) ref_seekerAnimator.Play("Run",0);
            else ref_seekerAnimator.Play("Idle",0);
        }
        if (Vector3.Distance(transform.position, new Vector3(ref_PlayerObj.transform.position.x, transform.position.y, ref_PlayerObj.transform.position.z)) < distanceToSlash && currentAIState == seekerAIStates.following)
        {
            StartCoroutine(action_Slash());
        }
        if (Vector3.Distance(transform.position, new Vector3(ref_PlayerObj.transform.position.x, transform.position.y, ref_PlayerObj.transform.position.z)) < distanceToStartDash) 
        {
            StartCoroutine(action_Dash());
        }


    }

    IEnumerator action_Dash()
    {
        if (!canUseDash) yield break;
        Vector3 playerPosition = ref_PlayerObj.transform.position;
        currentAIState = seekerAIStates.dashing;
        ref_NavMeshAgent.isStopped = true;
        ref_NavMeshAgent.velocity *= 0;
        canUseDash = false;
        Vector3 dashDir = Vector3.zero;

        foreach (TrailRenderer trail in movementTrails)
        {
            trail.emitting = true;
            yield return null;
        }

        ref_seekerAnimator.Play("RunToLeap", 0);

        #region dash attack

        //Play dash charge up sound
        yield return new WaitForSeconds(timeBeforeDash);


        ref_seekerAnimator.Play("Leap", 0);

        //This causes the nav mesh to dash in a direction
        dashDir = new Vector3(ref_PlayerObj.transform.position.x, transform.position.y, ref_PlayerObj.transform.position.z) - transform.position;
        float time = 0;
        while (time < dashDuration)
        {
            ref_NavMeshAgent.velocity = dashDir.normalized * dashSpeed;
            time += Time.deltaTime;

            if (dashCollider.bounds.Intersects(ref_playerCollider.bounds) && !hasHitWithSlash)
            {
                ref_PlayerStats.startTakingDMG(slashDamage);
                hasHitWithSlash = true;
                yield return null;
            }

            yield return null;
        }

        ref_NavMeshAgent.velocity *= 0;
        #endregion

        foreach (TrailRenderer trail in movementTrails)
        {
            trail.emitting = false;
            yield return null;
        }
        hasHitWithSlash = false;
        ref_NavMeshAgent.isStopped = false;

        currentAIState = seekerAIStates.following;
        yield return new WaitForSeconds(dashMovmentCooldown);
        canUseDash = true;

    }

    IEnumerator action_Slash()
    {
        if (!canUseSlash) yield break;

        currentAIState = seekerAIStates.slashing;
        ref_NavMeshAgent.isStopped = true;
        canUseSlash = false;
        hasHitWithSlash = false;

        ref_seekerAnimator.Play("Attack",0);


        float temp = 0;
        while (temp < ref_seekerAnimator.GetCurrentAnimatorStateInfo(0).length)
        {
            if (slashAttackHitbox.bounds.Intersects(ref_playerCollider.bounds) && !hasHitWithSlash)
            {
                ref_PlayerStats.startTakingDMG(slashDamage);

                hasHitWithSlash = true;
            }

            temp += Time.deltaTime;
            yield return null;

        }

        ref_NavMeshAgent.isStopped = false;
        currentAIState = seekerAIStates.following;
        yield return new WaitForSeconds(slashAttackCooldown);
        canUseSlash = true;
        hasHitWithSlash = false;
    }

    

    public void toggle_HitboxActive_Slash()
    {
        hitboxActive_Slash = true;
    }
    public void toggle_HitboxDeactive_Slash()
    {
        hitboxActive_Slash = false;
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
