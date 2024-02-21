using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class pistolPower : weaponPowerBase
{
    [Header("Power Variables")]
    [SerializeField] private float maxChargeTime;
    private float currentChargeTime;
    [SerializeField] private float chargeScaleMulti;
    [SerializeField] private float chargeScaleVel;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject firePoint;
    private GameObject mostRecentBullet;

    [Header("VFX Variables")]
    [SerializeField] private VisualEffect chargeEffect;
    [SerializeField] private GameObject shakingObj;
    [SerializeField] private float shakingIntensity;
    [SerializeField] private GameObject blackhole;
    [SerializeField] private ParticleSystem blackholeParticles;
    [SerializeField] private float timer;

    private Vector3 startingPos;
    InteractionInputActions inputActions;

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Awake()
    {
        inputActions = new InteractionInputActions();
        firePoint = GetComponent<weaponBase>().firePoint;
        startingPos = shakingObj.transform.localPosition;
    }

    private void FixedUpdate()
    {
        chargeShake();
    }

    public override IEnumerator usePower()
    {
        AudioManager.instance.PlaychargePistolSFX();
        if (!canUsePower) yield break;
        if(mostRecentBullet != null) mostRecentBullet.GetComponent<implosionBullet>().canPull = false;
        canUsePower = false;

        while (inputActions.Combat.Fire2.IsPressed())
        {
            if (currentChargeTime >= maxChargeTime)
            {
                currentChargeTime = maxChargeTime;
                yield return null;
            }
            else currentChargeTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(fireSpecialBullet(currentChargeTime));

    }

    private IEnumerator fireSpecialBullet(float chargeTime)
    {
        AudioManager.instance.StopchargePistolSFX();
        AudioManager.instance.PlaySFX(FMODEvents.instance.chargePistolShot, this.transform.position);
        blackholeParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        GameObject temp = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation, GameObject.Find("Bullet Storage").transform);
        float chargePercent = currentChargeTime / maxChargeTime;
        mostRecentBullet = temp;
        shakingObj.transform.localPosition = startingPos;

        temp.transform.localScale *= 1 + (chargeScaleMulti * chargePercent);
        //temp.GetComponent<implosionBullet>().bulletEffect.SetFloat("Start Size", 1 + (chargeScaleMulti * chargePercent));
        temp.GetComponent<implosionBullet>().velocity *= 1 + (chargeScaleVel * chargePercent);
        //increase the projectile velocity based on charge percent
        currentChargeTime = 0;

        while(timer < powerCooldown)
        {
            timer += Time.deltaTime;
            blackhole.transform.localScale = new Vector3(timer / powerCooldown, timer / powerCooldown, timer / powerCooldown);
            yield return null;
        }
        timer = 0;
        canUsePower = true;
        blackhole.transform.localScale = Vector3.one;
        blackholeParticles.Play();
    }


    void chargeShake()
    {    
        float chargePercent = currentChargeTime / maxChargeTime;
        if(currentChargeTime > 0.001f) shakingObj.transform.localPosition = startingPos + (Random.insideUnitSphere * shakingIntensity * chargePercent);
    }
}
