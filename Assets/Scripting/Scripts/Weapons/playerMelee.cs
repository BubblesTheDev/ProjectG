using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerMelee : MonoBehaviour
{
    [Header("Melee Variables")]
    [SerializeField] private int damage;
    [SerializeField] private float punchCooldown;
    [SerializeField] private Collider punchCollider;
    [SerializeField] private Animator punchAnimator;
    private bool canPunch = true;
    private bool hasDamagedThisPunch;

    InteractionInputActions interactionInput;
    private weaponBase weapon;

    private void Awake()
    {
        interactionInput = new InteractionInputActions();
        weapon = GetComponent<weaponBase>();
    }

    private void Update()
    {
        if (interactionInput.Combat.Melee.WasPressedThisFrame() && weapon.weaponIsEquipped) StartCoroutine(punch());

    }

    private void OnEnable()
    {
        interactionInput.Enable();
    }

    private void OnDisable()
    {
        interactionInput.Disable();
    }

    private IEnumerator punch()
    {
        if (!canPunch) yield break;
        hasDamagedThisPunch = false;
        canPunch = false;
        punchAnimator.Play("Punch", 1);
        punchCollider.enabled = true;
        yield return new WaitForSeconds(punchAnimator.GetCurrentAnimatorStateInfo(0).length);
        punchCollider.enabled = false;

        yield return new WaitForSeconds(punchCooldown);
        canPunch = true;

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && !hasDamagedThisPunch)
        {
            hasDamagedThisPunch = true;
            other.GetComponent<enemyStats>().takeDamage(damage);
        }

    }
}
