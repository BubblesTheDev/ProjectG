using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapons : MonoBehaviour
{
    [SerializeField] private GameObject pistolPrefab, shotgunPrefab, meleePrefab, weaponHUD, m2, num1, num2;
    // Start is called before the first frame update
    void Awake()
    {
        Invoke("FixAnim", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ToggleActive();
            Destroy(gameObject);
        }
    }

    public void FixAnim()
    {
        meleePrefab.SetActive(true);
    }

    public void ToggleActive()
    {
        pistolPrefab.SetActive(true);
        shotgunPrefab.SetActive(true);
        meleePrefab.SetActive(false);
        weaponHUD.SetActive(true);
        m2.SetActive(true);
        num1.SetActive(true);
        num2.SetActive(true);
    }
}
