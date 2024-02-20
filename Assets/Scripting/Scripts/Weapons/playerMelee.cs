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

    InteractionInputActions interactionInput;


    private void Awake()
    {
        interactionInput = new InteractionInputActions();
    }

    private void Update()
    {
        if (interactionInput.Combat.Melee.WasPressedThisFrame()) StartCoroutine(punch());
        
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

        canPunch = false;
        punchAnimator.Play("Punch");
        punchCollider.enabled = true;
        yield return new WaitForSeconds(punchAnimator.GetCurrentAnimatorStateInfo(0).length);
        punchCollider.enabled = false;

        yield return new WaitForSeconds(punchCooldown);
        canPunch = true;

    }

    
}
    