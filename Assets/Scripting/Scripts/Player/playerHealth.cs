using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private int deathSceneIndex;

    private Rigidbody rb;
    private bool isDead;

    [HideInInspector] public UnityEvent tookDamage;
    [HideInInspector] public UnityEvent healedDamage;

    private void Awake()
    {
        currentHP = maxHp;
        rb = GetComponent<Rigidbody>();
    }



    private void Update()
    {


        regenHP();
        if(currentHP <= 0) 
        {
            StartCoroutine(playerDeath());
        }
    }

    private void regenHP()
    {
        if (currentStaticEnergy >= maxStaticEnergy && currentHP != maxHp)
        {
            currentHP++;
            currentStaticEnergy = 0;
            healedDamage.Invoke();
        }

        if (rb.velocity.magnitude > 0 || rb.velocity.magnitude < 0)
        {
            if (currentHP < 5)
            {
                float movementStaticIncrease = Mathf.Sqrt(Mathf.Pow(rb.velocity.magnitude, 2));
                currentStaticEnergy += staticEnergyRate * (1 + (movementStaticIncrease / movementMulti)) * Time.deltaTime;
                if (currentStaticEnergy >= maxStaticEnergy) currentStaticEnergy = maxStaticEnergy;
            }
        }
    }

    public IEnumerator playerDeath()
    {
        yield return null;
        SceneManager.LoadScene(deathSceneIndex);
    }

    public IEnumerator takeDamage(int damage)
    {
        currentHP--;
        //Play health dmg sound
        //frame stutter
        tookDamage.Invoke();

        yield return new WaitForSeconds(immunityTime);
    }


}
