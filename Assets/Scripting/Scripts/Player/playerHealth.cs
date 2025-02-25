using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class playerHealth : MonoBehaviour
{
    [Header("Health Stats")]
    public int currentHP;
    public int maxHp;
    public float immunityTime;
    
    [Space,Header("Regen Stats")]
    public float maxStaticEnergy;
    public float currentStaticEnergy;
    [SerializeField] private float movementMulti;
    [SerializeField] private float staticEnergyRate;
    
    [Space, Header("Death Stats")]
    public int deathSceneIndex;

    private Rigidbody rb;
    public bool canTakeDamage;

    [HideInInspector] public UnityEvent tookDamage;
    [HideInInspector] public UnityEvent healedDamage;

    private void Awake()
    {
        // currentHP = maxHp;
        rb = GetComponent<Rigidbody>();
        canTakeDamage = true;
    }



    private void Update()
    {


        regenHP();
        if(currentHP <= 0) 
        {
            StartCoroutine(playerDeath());
        }
        if(currentHP > maxHp) currentHP = maxHp;
    }

    private void regenHP()
    {
        if (currentStaticEnergy >= maxStaticEnergy && currentHP < maxHp)
        {
            currentHP++;
            currentStaticEnergy = 0;
            healedDamage.Invoke();
        }

        if (rb.velocity.magnitude > 0 || rb.velocity.magnitude < 0)
        {
            if (currentHP < maxHp)
            {
                float movementStaticIncrease = Mathf.Sqrt(Mathf.Pow(rb.velocity.magnitude, 2));
                currentStaticEnergy += staticEnergyRate * (1 + (movementStaticIncrease / movementMulti)) * Time.deltaTime;
            }
        }
        if (currentHP == maxHp && currentStaticEnergy > 0) currentStaticEnergy = 0f; 

    }

    public IEnumerator playerDeath()
    {
        yield return null;
        SceneManager.LoadScene(deathSceneIndex);
        Cursor.visible = true;
    }

    public void startTakingDMG(int damage)
    {
        StartCoroutine(takeDamage(damage));
    }

    public IEnumerator takeDamage(int damage)
    {
        if (canTakeDamage)
        {
            canTakeDamage = false;
            currentHP -= damage;
            //Play health dmg sound
            //frame stutter
            tookDamage.Invoke();
            yield return new WaitForSeconds(immunityTime);
            canTakeDamage = true;
        }
    }


}
