using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class enemyStats : MonoBehaviour
{
    public float currentHP;
    public float maxHp;
    public float moveSpeed;
    public float ragDollTime;

    public roomEnemySpawner spawner;

    private void Update()
    {
        if (currentHP <= 0) die();
    }

    void die()
    {
    }

    public void takeDamage(float damageToTake)
    {
        currentHP -= damageToTake;
    }
}
