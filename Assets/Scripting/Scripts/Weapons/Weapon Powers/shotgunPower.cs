using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotgunPower : weaponPowerBase
{
    [Header("Combat Variables")]
    [SerializeField] private GameObject boundsPosition;
    [SerializeField] private Vector3 halfHitboxSize;
    [SerializeField] private int damage;
    [SerializeField] private GameObject parriedBullet;

    [Space, Header("Movement Variables")]
    [SerializeField] private float shockwaveForce;
    [SerializeField] private float airMultiplier;
    private GameObject cameraHolder;

    #region References
    private playerMovement ref_PlayerMovement;
    #endregion

    private void Awake()
    {
        ref_PlayerMovement = GameObject.Find("Player").GetComponent<playerMovement>();
        cameraHolder = GameObject.Find("CameraHolder");
    }

    public override IEnumerator usePower()
    {
        if (!canUsePower) yield break;
        canUsePower = false;


        //Collects all the enemies and projectiles in the bounds
        Collider[] tempObjs = Physics.OverlapBox(boundsPosition.transform.position, halfHitboxSize, transform.rotation);

        //Deals damage to the enemies and parries projectiles
        foreach (Collider collider in tempObjs)
        {
            if (collider.gameObject.CompareTag("Enemy")) collider.gameObject.GetComponent<enemyStats>().takeDamage(damage);
            else if (collider.gameObject.CompareTag("EnemyProjectile"))
            {
                Vector3 tempForward = -collider.transform.forward;
                Instantiate(parriedBullet, collider.transform.position, Quaternion.LookRotation(tempForward), GameObject.Find("BulletStorage").transform);
                Destroy(collider.gameObject);
            }
            yield return null;
        }

        //Does the shockwave movement
        if(ref_PlayerMovement != null) 
        {
            Vector3 tempDir = -cameraHolder.transform.forward;
            tempDir.Normalize();
            tempDir *= shockwaveForce;

            if (!ref_PlayerMovement.grounded) tempDir *= airMultiplier;

            ref_PlayerMovement.horizontal_playerVelocity += new Vector3(tempDir.x, 0, tempDir.z);
            ref_PlayerMovement.vertical_playerVelocity += new Vector3(0, tempDir.y, 0);
            
        }

        yield return new WaitForSeconds(powerCooldown);
        canUsePower = true;
    }
}
