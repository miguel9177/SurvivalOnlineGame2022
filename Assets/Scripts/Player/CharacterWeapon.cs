using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    [SerializeField]
    //this is going to store the current character weapon
    GameObject weapon;


    public void Shoot()
    {
        //this is going to check if its a normal gun
        if (weapon.CompareTag("NormalGun"))
        {
            //call the function that is going to shoot
            weapon.GetComponent<NormalGun>().CallShootFunction();
        }
    }


}
