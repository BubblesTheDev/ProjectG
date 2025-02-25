using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class weaponBase : MonoBehaviour
{
    [Header("Weapon Stats")]
    public float weaponDamage = 30;
    public float maxShotDistance;
    public int numOfPellets = 1;
    public float fireRateInSeconds = 0.25f;
    [Range(0, 15)]
    public float multiPelletAngle = 0;
    public float projectileSpeed = 250f;


    [Header("Weapon Settings")]
    public bool weaponIsEquipped;
    public LayerMask layersToIgnore;
    public bool canFire = true;
    public weaponPowerBase currentWeaponPower;
    public GameObject firePoint;
    public GameObject bulletPrefab;
    public GameObject gunGFX;

    InteractionInputActions interactionInput;
    cameraControl camControl;
    weaponVFXHandler weaponVFX;

    [Header("Crosshair Hit Marker")]
    [SerializeField] private Image[] crosshairMarker = new Image[4];
    private Coroutine hitMarkerCoroutine;
    [SerializeField] private float hitMarkerFadeTime;


    [HideInInspector] public UnityEvent gunFiredEvent;
    [HideInInspector] public UnityEvent powerActivated;

    private void Awake()
    {
        interactionInput = new InteractionInputActions();
        camControl = GameObject.Find("Player").GetComponent<cameraControl>();
        if (GetComponent<weaponVFXHandler>() != null) weaponVFX = GetComponent<weaponVFXHandler>();
        if (GetComponent<weaponPowerBase>() != null) currentWeaponPower = GetComponent<weaponPowerBase>();

        layersToIgnore = ~layersToIgnore;
    }

    private void OnEnable()
    {
        interactionInput.Enable();
    }

    private void OnDisable()
    {
        interactionInput.Disable();
    }

    private void Update()
    {
        if (weaponIsEquipped)
        {
            gunGFX.SetActive(true);
            if (camControl.lookingDir.point != null) firePoint.transform.LookAt(camControl.lookingDir.point);

            if (canFire && interactionInput.Combat.Fire1.IsPressed() && !interactionInput.Combat.Fire2.IsPressed())
            {
                if (firePoint == null)
                {
                    Debug.LogWarning("You are missing a <b>Firepoint</b> object decleration");
                    return;
                }
                StartCoroutine(fireGun());
            }
            if (interactionInput.Combat.Fire2.WasPressedThisFrame() && !interactionInput.Combat.Fire1.IsPressed())
            {
                if (currentWeaponPower == null)
                {
                    Debug.LogWarning("You are missing a <b>current weapon power</b> decleration");
                    return;
                }
                if (currentWeaponPower.canUsePower)
                {

                    powerActivated.Invoke();
                    StartCoroutine(currentWeaponPower.usePower());
                }
            }

        }
        else if (!weaponIsEquipped)
        {
            gunGFX.SetActive(false);
        }
    }

    IEnumerator fireGun()
    {
        canFire = false;

        gunFiredEvent.Invoke();
        weaponVFX.playMuzzleFlash();
        weaponVFX.playFireAnimation(fireRateInSeconds);

        for (int y = 0; y < numOfPellets; y++)
        {

            //Detects if the weapon is firing a projectile by seeing if there is a prefab to fire or not
            if (bulletPrefab == null)
            {

                List<RaycastHit> hits = new List<RaycastHit>();

                //Causes the first pellet to always be straight, or if the angle is 0
                if (multiPelletAngle == 0 || y == 0)
                {
                    RaycastHit hit;
                    Physics.Raycast(camControl.CameraObj.transform.position, camControl.CameraObj.transform.forward, out hit, math.INFINITY, layersToIgnore);
                    if (hit.transform != null) hits.Add(hit);
                }
                //shoots with a random angle
                else
                {
                    RaycastHit hit;
                    Physics.Raycast(camControl.CameraObj.transform.position, Quaternion.Euler(Random.Range(-multiPelletAngle, multiPelletAngle),
                        Random.Range(-multiPelletAngle, multiPelletAngle), 0) * camControl.CameraObj.transform.forward, out hit, math.INFINITY, layersToIgnore);
                    if (hit.transform != null) hits.Add(hit);
                }

                foreach (RaycastHit hit in hits)
                {
                    StartCoroutine(weaponVFX.spawnTrail(hit));

                    if (hit.transform.CompareTag("Enemy"))
                    {
                        hit.transform.GetComponent<enemyStats>().takeDamage(weaponDamage);
                        if (hitMarkerCoroutine != null) StopCoroutine(hitMarkerCoroutine);
                        hitMarkerCoroutine = StartCoroutine(FadeHitMarker());

                    }
                    else
                    {
                        StartCoroutine(weaponVFX.spawnBulletHole(hit));
                    }
                }
            }
            else if (bulletPrefab != null)
            {
                GameObject tempBullet = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation * Quaternion.Euler(Random.Range(-multiPelletAngle, multiPelletAngle), Random.Range(-multiPelletAngle, multiPelletAngle), 0), GameObject.Find("Bullet Storage").transform);

                tempBullet.GetComponent<playerProjectileBase>().loadStats(weaponDamage/numOfPellets, projectileSpeed, maxShotDistance);
            }
        }

        yield return new WaitForSeconds(fireRateInSeconds);
        canFire = true;
    }
    IEnumerator FadeHitMarker()
    {
        float timer = 0;

        while (timer < hitMarkerFadeTime)
        {
            timer += Time.deltaTime;
            for (int i = 0; i < 4; i++)
            {

                crosshairMarker[i].color = Color.Lerp(Color.white, new Color(0, 0, 0, 0), timer / hitMarkerFadeTime);
                yield return null;
            }
        }
        crosshairMarker[0].color = new Color(0, 0, 0, 0);
        crosshairMarker[1].color = new Color(0, 0, 0, 0);
        crosshairMarker[2].color = new Color(0, 0, 0, 0);
        crosshairMarker[3].color = new Color(0, 0, 0, 0);


    }


}
