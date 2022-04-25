using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    [SerializeField]
    //this is going to store the current character weapon
    public GameObject weapon;


    public void Shoot()
    {
        //call the function that is going to shoot
        weapon.GetComponent<GunInformation>().firingMethod();   
    }
    

    public void Reload()
    {
        //call the function that is going to reload
        weapon.GetComponent<GunInformation>().reloadMethod();
    }
}
