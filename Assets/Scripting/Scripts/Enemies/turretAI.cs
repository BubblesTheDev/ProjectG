using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class turretAI : MonoBehaviour
{
    [Header("Turret Variables")]
    [SerializeField] private int attacksPerBurst;
    [SerializeField] private float fireRate;
    [SerializeField] private float fireCooldown;
    [SerializeField] private float bulletSpeed;

    [Space, Header("Aiming variales")]
    [SerializeField] private float upperLimit;
    [SerializeField] private float LowerLimit;
    [SerializeField] private float spoolUpTime;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float minDistance;


    [Space, Header("Assignables")]
    [SerializeField] private roomEnemySpawner brain;
    [SerializeField] private List<GameObject> firePoints;
    [SerializeField] private List<ParticleSystem> muzzleFlashes;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject barrelBase;
    [SerializeField] private GameObject barrels;

    private GameObject playerObj;
    private Vector3 targetPos;
    private Vector3 smoothAimVel;
    private bool canShoot = true;

    private void Awake()
    {
        playerObj = GameObject.Find("Player");
    }

    private void Update()
    {
        if (brain != null) 
            if (brain.playerInRoom) 
                if (canShoot) StartCoroutine(fireGun());
    }

    private void FixedUpdate()
    {
        if (brain != null) if (brain.playerInRoom) smoothAim();
    }

    void smoothAim()
    {
        //this keeps the target a minimum distance away from the turret so it cant aim into itself when the player goes under it
        if (Vector3.Distance(transform.position, targetPos) < minDistance)
        {
            targetPos = Vector3.SmoothDamp(targetPos, 
                transform.position + Vector3.Scale((playerObj.transform.position - transform.position), new Vector3(1, 0, 1)).normalized * minDistance, 
                ref smoothAimVel, rotSpeed, Mathf.Infinity, Time.deltaTime);
        }
        else targetPos = Vector3.SmoothDamp(targetPos, playerObj.transform.position, ref smoothAimVel, rotSpeed, Mathf.Infinity, Time.deltaTime);

        targetPos.y = Mathf.Clamp(targetPos.y, transform.position.y + LowerLimit, transform.position.y + upperLimit);



        if (barrelBase != null) barrelBase.transform.LookAt(new Vector3(targetPos.x, barrelBase.transform.position.y, targetPos.z), transform.up);
        if (barrels != null) barrels.transform.LookAt(targetPos, transform.up);
    }

    IEnumerator fireGun()
    {
        canShoot = false;
        //Play spool up sound here
        AudioManager.instance.PlaySFX(FMODEvents.instance.turretCharge, this.transform.position);
        yield return new WaitForSeconds(spoolUpTime);


        for (int i = 1; i < attacksPerBurst+1; i++)
        {
            //play fire sound here
            AudioManager.instance.PlaySFX(FMODEvents.instance.turretShoot, this.transform.position);
            int muzzleFlashIndex = i % muzzleFlashes.Count;
            muzzleFlashes[muzzleFlashIndex].Play();


            int firePointIndex = i % firePoints.Count;
            GameObject bullet = Instantiate(bulletPrefab, firePoints[firePointIndex].transform.position, firePoints[firePointIndex].transform.rotation, GameObject.Find("Bullet Storage").transform);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
            yield return new WaitForSeconds(fireRate);
        }
        yield return new WaitForSeconds(fireCooldown);
        canShoot = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetPos, 1f);
        Gizmos.DrawWireSphere(transform.position, minDistance);
    }
}
