using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference battleMusic { get; private set; }
    [field: SerializeField] public EventReference titleMusic { get; private set; }

    [field: Header("Ambience")]
    [field: SerializeField] public EventReference ambience { get; private set; }

    [field: Header("GUN SFX")]
    [field: SerializeField] public EventReference pistolShot { get; private set; }
    [field: SerializeField] public EventReference chargePistolSFX { get; private set; }
    [field: SerializeField] public EventReference chargePistolShot { get; private set; }
    [field: SerializeField] public EventReference chargePistolExplode { get; private set; }
    [field: SerializeField] public EventReference shotgunShotNoCock { get; private set; }
    [field: SerializeField] public EventReference shotgunShotCock { get; private set; }
    [field: SerializeField] public EventReference altShotgunShot { get; private set; }
    [field: SerializeField] public EventReference enemyShot { get; private set; }

    [field: Header("Gravity Switch")]
    [field: SerializeField] public EventReference gravitySwitch { get; private set; }

    [field: Header("Enemy SFX")]
    [field: SerializeField] public EventReference enemySpawn { get; private set; }
    [field: SerializeField] public EventReference bruiserHit { get; private set; }
    [field: SerializeField] public EventReference turretCharge { get; private set; }
    [field: SerializeField] public EventReference turretShoot { get; private set; }
    [field: SerializeField] public EventReference cerberusShoot { get; private set; }
    [field: SerializeField] public EventReference seekerDash { get; private set; }
    [field: SerializeField] public EventReference seekerDashStart { get; private set; }
    [field: SerializeField] public EventReference seekerSlash { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference jumpSFX { get; private set; }
    [field: SerializeField] public EventReference dashSFX { get; private set; }
    [field: SerializeField] public EventReference regenSFX { get; private set; }
    [field: SerializeField] public EventReference slideSFX { get; private set; }
    [field: SerializeField] public EventReference runSFX { get; private set; }
    [field: SerializeField] public EventReference groundSlamSFX { get; private set; }

    [field: Header("Voices")]
    [field: SerializeField] public EventReference checkpointVoice1 { get; private set; }
    [field: SerializeField] public EventReference checkpointVoice2 { get; private set; }
    [field: SerializeField] public EventReference checkpointVoice3 { get; private set; }

    public static FMODEvents instance {  get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}
