using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class weaponInventory : MonoBehaviour
{
    public List<GameObject> weapons = new List<GameObject>();
    public int currentWeaponIndex { private set; get; }
    [SerializeField] private GameObject currentWeapon;

    [SerializeField] private TextMeshProUGUI[] weaponSwapNums;
    public TextMeshProUGUI currentWeaponNumUI;

    InteractionInputActions interactionInput;

    private void Awake()
    {
        interactionInput = new InteractionInputActions();

        interactionInput.Combat.WeaponSlot1.performed += takeOutWeapon1 => takeOutWeapon(0);
        interactionInput.Combat.WeaponSlot2.performed += takeOutWeapon2 => takeOutWeapon(1);
        interactionInput.Combat.WeaponSlot3.performed += takeOutWeapon3 => takeOutWeapon(2);
        interactionInput.Combat.WeaponSlot4.performed += takeOutWeapon4 => takeOutWeapon(3);
        interactionInput.Combat.WeaponSlot5.performed += takeOutWeapon5 => takeOutWeapon(4);
        for (int i = 0; i < weaponSwapNums.Length; i++)
        {
            weaponSwapNums[i].color = new Color32(255, 255, 255, 30);
        }
        weaponSwapNums[currentWeaponIndex].color = Color.white;

    }

    private void OnEnable()
    {
        interactionInput.Enable();
    }

    private void OnDisable()
    {
        interactionInput.Disable(); 
    }



    private void Update()
    {
        handleWeapons();
    }

    void handleWeapons()
    {
        if (weapons.Count <= 0) return;

        if(interactionInput.Combat.scrollWheel.ReadValue<float>() > 0)
        {
            currentWeaponIndex++;
            if(currentWeaponIndex > weapons.Count-1) currentWeaponIndex = 0;
            takeOutWeapon(currentWeaponIndex);
        }
        else if(interactionInput.Combat.scrollWheel.ReadValue<float>() < 0)
        {
            currentWeaponIndex--;
            if(currentWeaponIndex < 0) currentWeaponIndex = weapons.Count - 1;

            takeOutWeapon(currentWeaponIndex);
        }
    }


    void takeOutWeapon(int weaponIndex)
    {
        if(weapons.Count <= 0 || weapons[weaponIndex] == null) return;

        //Play dequip animation

        for(int i = 0; i < weaponSwapNums.Length; i++)
        {
            weaponSwapNums[i].color = new Color32(255, 255, 255, 30);
        }
        foreach (GameObject weapon in weapons)
        {
            weapon.GetComponent<weaponBase>().weaponIsEquipped = false;
        }

        currentWeaponNumUI = weaponSwapNums[weaponIndex];
        currentWeapon = weapons[weaponIndex];
        weaponSwapNums[weaponIndex].color = Color.white;
        currentWeapon.GetComponent<weaponBase>().weaponIsEquipped = true;

        //Play animation for equipping weapon
    }

}
