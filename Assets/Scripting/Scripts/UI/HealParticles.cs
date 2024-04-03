using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HealParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private playerHealth stats;
    [SerializeField] private Volume healPPR;
    [SerializeField] private float vignetteShowTime;
    [SerializeField] private ParticleSystem healingParticles;

    // Start is called before the first frame update
    void Awake()
    {
        stats = GameObject.Find("Player").GetComponent<playerHealth>();
        stats.healedDamage.AddListener(VignetteSequence);
    }

    // Update is called once per frame
    void Update()
    {
        var main = particles.main;

        if (stats.currentStaticEnergy <= stats.maxStaticEnergy * 0.25f)
        {
            main.maxParticles = 0;
        }
        else if (stats.currentStaticEnergy <= stats.maxStaticEnergy * 0.5f && stats.currentStaticEnergy >= stats.maxStaticEnergy * 0.26f)
        {
            main.maxParticles = 1;
        }
        else if (stats.currentStaticEnergy <= stats.maxStaticEnergy * 0.75f && stats.currentStaticEnergy >= stats.maxStaticEnergy * 0.51f)
        {
            main.maxParticles = 3;
        }
        else if (stats.currentStaticEnergy <= stats.maxStaticEnergy * .99f && stats.currentStaticEnergy >= stats.maxStaticEnergy * .76f)
        {
            main.maxParticles = 10;
        }

    }



    void VignetteSequence()
    {
        // healingParticles.transform.parent.position = stats.gameObject.transform.position;
        healingParticles.Play();
        StartCoroutine(HealVignette());
        
    }

    private IEnumerator HealVignette()
    {
        float timer = 0;
        while (timer < vignetteShowTime)
        {
            timer += Time.deltaTime;
            healPPR.weight = timer / vignetteShowTime;
            yield return null;
        }

        yield return new WaitForSeconds(vignetteShowTime);
        timer = 0;
        while (timer < vignetteShowTime)
        {
            timer += Time.deltaTime;
            healPPR.weight = 1 - (timer / vignetteShowTime);
            yield return null;
        }

        healPPR.weight = 0;

    }


}
