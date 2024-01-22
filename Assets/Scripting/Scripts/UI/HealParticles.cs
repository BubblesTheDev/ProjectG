using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HealParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private playerHealth stats;


    // Start is called before the first frame update
    void Start()
    {
        stats = GameObject.Find("Player").GetComponent<playerHealth>();
        // stats.healedDamage.AddListener(VignetteSequence);
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

/*    void VignetteSequence()
    {
        StartCoroutine(SpawnVignette());
    }

    private IEnumerator SpawnVignette()
    {
        v = ScriptableObject.CreateInstance<Vignette>();
        v.enabled.Override(true);
        v.intensity.Override(1f);
        ppv = PostProcessManager.instance.QuickVolume(10, 100, v);
        v.intensity.value = Mathf.Sin(Time.realtimeSinceStartup);
        yield return new WaitForSeconds(0.5f);
        RuntimeUtilities.DestroyVolume(ppv, true, false);
    }*/


}
