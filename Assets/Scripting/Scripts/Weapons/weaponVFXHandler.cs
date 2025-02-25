using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
//using UnityEngine.Rendering.HighDefinition;

public class weaponVFXHandler : MonoBehaviour
{
    [Header("General Variables")]
    public GameObject firePoint;

    [Header("Trail Variables")]
    [SerializeField] private GameObject bulletTracerPrefab;
    [SerializeField] private float trailSpeed;

    [Header("Bullet hole Variables")]
    [SerializeField] private GameObject projectorPrefab;
    [SerializeField] private float timeToFade;

    [Header("Muzzle Flash Variables")]
    [SerializeField] private List<ParticleSystem> muzzleFlashParticles = new List<ParticleSystem>();

    [Header("Weapon Animation Variables")]
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private string weaponFireAnimName;
    [SerializeField] private string weaponPowerAnimName;


    public IEnumerator spawnTrail(RaycastHit hitInfo)
    {
        GameObject tempBulletTracer = Instantiate(bulletTracerPrefab, firePoint.transform.position, Quaternion.identity, GameObject.Find("VFX Holder").transform);

        while (Vector3.Distance(tempBulletTracer.transform.position, hitInfo.point) > 0.1f)
        {
            tempBulletTracer.transform.position = Vector3.MoveTowards(tempBulletTracer.transform.position, hitInfo.point, trailSpeed * Time.deltaTime);
            yield return null;
        }


        //StartCoroutine(spawnBulletHole(hitInfo));
        Destroy(tempBulletTracer, 5f);
    }

    public IEnumerator spawnBulletHole(RaycastHit bulletHoleInfo)
    {
        GameObject tempBullethole = Instantiate(projectorPrefab, bulletHoleInfo.point, Quaternion.LookRotation(bulletHoleInfo.normal), GameObject.Find("VFX Holder").transform);
        tempBullethole.GetComponent<VisualEffect>().SetVector3("BulletHoleAngle", bulletHoleInfo.normal);

        yield return null;
        Destroy(tempBullethole, timeToFade);
    }

    public void playMuzzleFlash()
    {
        foreach (ParticleSystem muzzleFlashPeice in muzzleFlashParticles)
        {
            muzzleFlashPeice.Play();
        }
    }

    public void playFireAnimation(float fireCooldown)
    {
        //weaponAnimator.SetFloat("AnimSpeed", 1 / fireCooldown);
        weaponAnimator.Play(weaponFireAnimName,-1, 0f);
    }
}
