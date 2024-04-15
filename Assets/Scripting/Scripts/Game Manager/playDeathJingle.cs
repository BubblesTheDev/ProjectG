using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playDeathJingle : MonoBehaviour
{
    private void Awake()
    {
        //Play death jingle here
        AudioManager.instance.PlaySFX(FMODEvents.instance.deathJingle, this.transform.position);
    }
}
