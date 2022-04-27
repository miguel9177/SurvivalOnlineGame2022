using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CharacterWeapon : MonoBehaviourPun
{
    [SerializeField]
    //this will store the knife weapon 
    public GunInformation knife;
    [SerializeField]
    //this will store the pistol weapon 
    public GunInformation pistol;
    [SerializeField]
    //this will store the rifle weapon 
    public GunInformation rifle;

    //if this is true it will block the weapon switch, this is accessed by the normalGun
    public bool blockWeaponSwitch = false;

    private void Awake()
    {
        switchWeapon(2);
    }

    //this will switch weapons of the player, its called from the character input
    public void switchWeapon(int weaponToSwitchTo_int)
    {
        //this will store the bullet info of the old weapon before switching guns
        switch (weaponToSwitchTo_int)
        {
            case 0:
                Debug.Log("ERROR, NO WEAPON HAS 0 HAs THE INT To Switch");
                break;
            //if the weapon to switch to is 1 that means we want to make the knife visible and the rest invisible
            case 1:
                knife.gameObject.SetActive(true);
                pistol.gameObject.SetActive(false);
                rifle.gameObject.SetActive(false);
                break;
            //if the weapon to switch to is 2 that means we want to make the pistol visible and the rest invisible
            case 2:
                knife.gameObject.SetActive(false);
                pistol.gameObject.SetActive(true);
                rifle.gameObject.SetActive(false);
                break;
            //if the weapon to switch to is 3 that means we want to make the rifle visible and the rest invisible
            case 3:
                knife.gameObject.SetActive(false);
                pistol.gameObject.SetActive(false);
                rifle.gameObject.SetActive(true);
                break;
        }
    }

    
    private void Update()
    {
        //if the photon vie is mine, and we can switch weapons
        if(photonView.IsMine && blockWeaponSwitch == false)
        {
            //this will handle the input for weapon swicthing
            TakeInput();
        }

    }

    void TakeInput()
    {
        //if the user clicked 1, change the weapon to the rifle
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //switch weapon and let the characetr weapon script know wich number i swicthed to
            switchWeapon(1);
        }
        //if the user clicked 2, change the weapon to the pistol
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //switch weapon and let the characetr weapon script know wich number i swicthed to
            switchWeapon(2);
        }
        //if the user clicked 3, change the weapon to the knife
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //switch weapon and let the characetr weapon script know wich number i swicthed to
            switchWeapon(3);
        }
    }



}
