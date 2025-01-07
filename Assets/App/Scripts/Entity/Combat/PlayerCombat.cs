using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform weaponsContent;

    [SerializeField] List<WeaponTemplate> weapons = new();
    [SerializeField] int currentWeaponIndex = 0;
    [SerializeField] Transform cam;

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    //[Header("Input")]
    //[Header("Output")]

    private void Start()
    {
        weapons[currentWeaponIndex].Reload();
    }

    private void Update()
    {
        int newWeaponIndex = (currentWeaponIndex + (int)Input.mouseScrollDelta.y) % weapons.Count;
        if(newWeaponIndex < 0) newWeaponIndex = weapons.Count - 1;

        if(newWeaponIndex != currentWeaponIndex)
        {
            weapons[currentWeaponIndex].gameObject.SetActive(false);
            weapons[newWeaponIndex].gameObject.SetActive(true);
            currentWeaponIndex = newWeaponIndex;
        }

        if (Input.GetKey(KeyCode.Mouse0) && weapons[currentWeaponIndex].isSemiAuto || Input.GetKeyDown(KeyCode.Mouse0))
        {
            weapons[currentWeaponIndex].OnAttack(cam.position, cam.forward);
        }

        if (Input.GetKey(KeyCode.R))
        {
            weapons[currentWeaponIndex].Reload();
        }
    }

    void AddWeapon(WeaponTemplate newWeaponPrefab)
    {
        weapons.Add(Instantiate(newWeaponPrefab, weaponsContent));

        int newWeaponIndex = weapons.Count - 1;
        weapons[currentWeaponIndex].gameObject.SetActive(false);
        weapons[newWeaponIndex].gameObject.SetActive(true);
        currentWeaponIndex = newWeaponIndex;
    }
}