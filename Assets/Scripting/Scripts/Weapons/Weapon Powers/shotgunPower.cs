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

    [Space, Header("VFX Variables")]
    [SerializeField] private GameObject shockwave;
    [SerializeField] private ParticleSystem shockwaveParticles;
    [SerializeField] private Material shockwaveMat;
    [SerializeField] private float shockwaveSize;

    [Space, Header("Movement Variables")]
    [SerializeField] private float shockwaveForce;
    [SerializeField] private float airMultiplier;
    private GameObject cameraHolder;

    private float cooldown;
    private weaponBase weapon;

    #region References
    private playerMovement ref_PlayerMovement;
    private GameObject playerOrientation;
    #endregion

    private void Awake()
    {
        ref_PlayerMovement = GameObject.Find("Player").GetComponent<playerMovement>();
        playerOrientation = GameObject.Find("Orientation");
        cameraHolder = GameObject.Find("CameraHolder");
        shockwaveMat.SetFloat("_Size", 0);
        shockwave.SetActive(false);
        if (GetComponent<weaponBase>()) weapon = GetComponent<weaponBase>();
    }

    private void Update()
    {
        if (weaponPowerIcon != null && weapon.weaponIsEquipped)
        {
            weaponPowerIcon.value = cooldown / powerCooldown;
        }
        // OnDrawGizmos();

    }

    public override IEnumerator usePower()
    {
        if (!canUsePower) yield break;
        canUsePower = false;
        cooldown = 0;

        //Place particle stuff here
        StartCoroutine(EnableShockwave());
        shockwaveMat.SetFloat("_Size", shockwaveSize);
        shockwaveParticles.Play();

        //Collects all the enemies and projectiles in the bounds
        Collider[] tempObjs = Physics.OverlapBox(boundsPosition.transform.position, halfHitboxSize, gameObject.transform.rotation);
        //Deals damage to the enemies and parries projectiles
        foreach (Collider collider in tempObjs)
        {
            if(collider != null)
            {
                if (collider.gameObject.CompareTag("Enemy")) collider.gameObject.GetComponent<enemyStats>().takeDamage(damage);
                else if (collider.gameObject.CompareTag("EnemyProjectile"))
                {
                    Vector3 tempForward = playerOrientation.transform.forward;
                    if (parriedBullet != null) Instantiate(parriedBullet, collider.transform.position, Quaternion.LookRotation(tempForward), GameObject.Find("Bullet Storage").transform);
                    Destroy(collider.gameObject);
                }

            }
            yield return null;
        }

        //Does the shockwave movement
        if (ref_PlayerMovement != null) 
        {
            Vector3 tempDir = -cameraHolder.transform.forward;
            tempDir.Normalize();
            tempDir *= shockwaveForce;

            if (!ref_PlayerMovement.grounded) tempDir *= airMultiplier;

            ref_PlayerMovement.horizontal_playerVelocity += new Vector3(tempDir.x, 0, tempDir.z);
            ref_PlayerMovement.vertical_playerVelocity += new Vector3(0, tempDir.y, 0);
            
        }
        
        while(cooldown < powerCooldown)
        {
            cooldown += Time.deltaTime;
            yield return null;
        }

        if (weaponPowerIcon != null && weapon.weaponIsEquipped) weaponPowerIcon.value = 1;
        canUsePower = true;
    }

    IEnumerator EnableShockwave()
    {
        shockwave.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        shockwaveMat.SetFloat("_Size", 0);
        shockwave.SetActive(false);
    }

/*    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boundsPosition.transform.position, halfHitboxSize * 2);

    }*/

}
