using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weaponPowerBase : MonoBehaviour
{
    public bool canUsePower = true;
    public float powerCooldown = 1;
    public int numCharges = 1;
    public int currentCharges = 1;
    public weaponType weaponPowerIsFor;
    public Slider weaponPowerIcon;

    public virtual IEnumerator usePower()
    {
        yield return null;
    }
}

[Flags]
public enum weaponType
{
    revolver,
    shotgun,
    machineGun,
    rocketLauncher,
    sniper
}
