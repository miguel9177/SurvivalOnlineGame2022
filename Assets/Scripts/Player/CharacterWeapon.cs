using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{

    [SerializeField]
    //this is going to store the current knife character weapon
    public GunInformation knifeWeapon;

    [SerializeField]
    //this is going to store the current pistol character weapon
    public GunInformation pistolWeapon;

    [SerializeField]
    //this is going to store the current rifle character weapon
    public GunInformation rifleWeapon;

    [HideInInspector]
    //this will hold the current weapon being used
    public GunInformation currentUsedWeapon;

    private void Awake()
    {
        currentUsedWeapon = pistolWeapon;
    }

    //this will switch weapons of the player, its called from the character input
    public void switchWeapon(int weaponToSwitchTo)
    {
        //changed the current weapon to the weapon we just switched to 
        switch(weaponToSwitchTo)
        {
            case 1:
                currentUsedWeapon = rifleWeapon;
                break;
            case 2:
                currentUsedWeapon = pistolWeapon;
                break;
            case 3:
                currentUsedWeapon=rifleWeapon;
                break;
        }
    }

    public void Shoot()
    {
        //call the function that is going to shoot
        currentUsedWeapon.GetComponent<GunInformation>().CallShootFunction();   
    }
    

    public void Reload()
    {
        //call the function that is going to reload
        currentUsedWeapon.GetComponent<GunInformation>().reloadMethod();
    }


}
