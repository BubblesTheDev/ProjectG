using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class basicRangedAI : MonoBehaviour
{

    [Header("Basic Stats")]
    [SerializeField] private bool enableDebug;
    [SerializeField] private bool canShoot = true;
    [SerializeField] private List<GameObject> firePoints;
    [SerializeField] private GameObject enemyBullet;
    [SerializeField] private Animator currentAnimator;
    [SerializeField] private float closeRangeDistance, midRangeDistance, longRangeDistance;
    [SerializeField] private float damage;

    [Header("Movement Brain")]
    [SerializeField] private float maxMoveDistance;
    [SerializeField, Range(0.01f,1f)] private float percentToMoveCloserToMid = 0.3f;
    [SerializeField] private float minOrbitTime = 0.5f, MaxOrbitTime = 1.5f;

    [Header("Short Range Shooting")]
    [SerializeField] private float fireCooldownShortRange;
    [SerializeField] private float fireRateShortRange;
    [SerializeField] private float numShotsShortRange;
    [SerializeField] private float bulletSpeedShortRange;

    [Header("Long Range Shooting")]
    [SerializeField] private float fireCooldownLongRange;
    [SerializeField] private float fireRateLongRange;
    [SerializeField] private float numShotsLongRange;
    [SerializeField] private float bulletSpeedLongRange;



    #region Assignables
    private NavMeshAgent ref_NavMeshAgent;
    private GameObject ref_PlayerObj;
    private Rigidbody ref_PlayerRB;
    private Animator ref_rangedAnimator;
    private enemyStats ref_EnemyStats;
    private float orbitTime;
    private float maxOrbitTime;
    private int orbitDir;
    #endregion


    private void Awake()
    {
        ref_PlayerObj = GameObject.Find("Player");
        ref_PlayerRB = ref_PlayerObj.GetComponent<Rigidbody>();
        ref_EnemyStats = GetComponent<enemyStats>();
        ref_NavMeshAgent = GetComponent<NavMeshAgent>();
        ref_rangedAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (canShoot)
        {
            if (Vector3.Distance(transform.position, ref_PlayerRB.transform.position) < midRangeDistance) StartCoroutine(closeRangeShot());
            else if (Vector3.Distance(transform.position, ref_PlayerRB.transform.position) > ((longRangeDistance - midRangeDistance) * percentToMoveCloserToMid) + midRangeDistance) StartCoroutine(longRangeShot());
        }
    }

    private void FixedUpdate()
    {
        rangerMoveBrain();

    }

    private void rangerMoveBrain()
    {
        //Calculates the player position but on the same height as enemy
        Vector3 verticalScaledPos = new Vector3(ref_PlayerObj.transform.position.x, transform.position.y, ref_PlayerObj.transform.position.z);

        //Rotate to face the player
        transform.LookAt(new Vector3(ref_PlayerObj.transform.position.x, transform.position.y, ref_PlayerObj.transform.position.z), Vector3.up);

        //Moves them away from the player if too close
        if (Vector3.Distance(verticalScaledPos, transform.position) < closeRangeDistance)
        {
            //print("moving away from player");
            ref_NavMeshAgent.SetDestination(transform.position + (transform.position + verticalScaledPos).normalized * maxMoveDistance);
        }
        //This orbits the enemies around the player if they are within the sweet spot
        else if(Vector3.Distance(verticalScaledPos, transform.position) > closeRangeDistance 
            && Vector3.Distance(verticalScaledPos, transform.position) < midRangeDistance)
        {
            if(orbitTime >= maxOrbitTime)
            {
                if (Random.Range(1, 101) >= 50) orbitDir = 1;
                else orbitDir = -1;
                //This determines how long until it choses its next destination around the player
                maxOrbitTime = Random.Range(minOrbitTime, maxOrbitTime);
                orbitTime = 0;
            }
            ref_NavMeshAgent.SetDestination(transform.position + (transform.right * orbitDir).normalized * maxMoveDistance/4);
            orbitTime += Time.deltaTime;
        }
        //This moves the enemy closer to the player into their sweetspot range, if they are closer to the player than their long range distance
        else if (Vector3.Distance(verticalScaledPos, transform.position) > midRangeDistance
             && Vector3.Distance(verticalScaledPos,transform.position) < ((longRangeDistance - midRangeDistance) * percentToMoveCloserToMid) + midRangeDistance)
        {
            ref_NavMeshAgent.SetDestination(transform.position + (verticalScaledPos - transform.position).normalized * maxMoveDistance);
        }
    }

    private IEnumerator closeRangeShot()
    {
        canShoot = false;
        for (int i = 0; i < numShotsShortRange; i++)
        {
            //Calculates the position to aim at
            int firePointIndex = Random.Range(0, firePoints.Count);
            Vector3 leadingDir = (ref_PlayerObj.transform.position + ref_PlayerRB.velocity * Time.deltaTime) - firePoints[firePointIndex].transform.position;

            GameObject temp = Instantiate(enemyBullet, firePoints[firePointIndex].transform.position, Quaternion.LookRotation(leadingDir.normalized), GameObject.Find("Bullet Storage").transform);
            temp.GetComponent<Rigidbody>().velocity = temp.transform.forward * bulletSpeedShortRange;

            yield return new WaitForSeconds(fireRateShortRange);
        }

        yield return new WaitForSeconds(fireCooldownShortRange);
        canShoot = true;
    }
    private IEnumerator longRangeShot()
    {
        canShoot = false;
        for (int i = 0; i < numShotsLongRange; i++)
        {
            for (int j = 0; j < firePoints.Count; j++)
            {
                GameObject temp = Instantiate(enemyBullet, firePoints[j].transform.position, Quaternion.LookRotation((ref_PlayerObj.transform.position - firePoints[j].transform.position).normalized), GameObject.Find("Bullet Storage").transform);
                temp.GetComponent<Rigidbody>().velocity = temp.transform.forward * bulletSpeedShortRange;
            }
            yield return new WaitForSeconds(fireRateLongRange);
        }


        yield return new WaitForSeconds(fireCooldownShortRange);
        canShoot = true;
    }

    #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {

            Handles.color = Color.blue;
            Handles.DrawWireDisc(transform.position, transform.up, longRangeDistance);
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(transform.position, transform.up, ((longRangeDistance-midRangeDistance)*percentToMoveCloserToMid) + midRangeDistance);
            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position, transform.up, midRangeDistance);
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, transform.up, closeRangeDistance);

            Gizmos.color = Color.white;
            if(ref_PlayerObj != null && ref_PlayerRB != null) Gizmos.DrawWireSphere((ref_PlayerObj.transform.position + ref_PlayerRB.velocity * Time.deltaTime), 1);
    }
#endif

}