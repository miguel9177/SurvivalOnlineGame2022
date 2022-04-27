using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
  

    [SerializeField]
    //this is going to store the current knife character weapon
    public GunInformation knifeWeapon;
    //this will store the knife info of this knife, since we cant edit anything on the knife variable directly, or it will change the prefab
    Gun knifeWeaponInfo;

    [SerializeField]
    //this is going to store the current pistol character weapon
    public GunInformation pistolWeapon;
    //this will store the info of this pistol, since we cant edit anything on the pistol variable directly, or it will change the prefab
    Gun pistolWeaponInfo;

    [SerializeField]
    //this is going to store the current rifle character weapon
    public GunInformation rifleWeapon;
    //this will store the info of this rifle, since we cant edit anything on the rifle variable directly, or it will change the prefab
    Gun rifleWeaponInfo;

    [SerializeField]
    [Header("this needs to be set to the gun that is a child of this object")]
    //this will hold the current weapon being used
    public GunInformation currentUsedWeapon;

    //value from 1 to 3 , 1 is a rifle, 2 is a pistol, 3 is a knife
    int currentUsedWeaponId = 2;

    private void Awake()
    {
        //this will store the rifle bullet information
        rifleWeaponInfo.currentBulletsOnMagazine = rifleWeapon.gun.currentBulletsOnMagazine;
        rifleWeaponInfo.spareBullets = rifleWeapon.gun.spareBullets;

        pistolWeaponInfo.currentBulletsOnMagazine = pistolWeapon.gun.currentBulletsOnMagazine;
        pistolWeaponInfo.spareBullets = pistolWeapon.gun.spareBullets;

        knifeWeaponInfo.currentBulletsOnMagazine = knifeWeapon.gun.currentBulletsOnMagazine;
        knifeWeaponInfo.spareBullets = knifeWeapon.gun.spareBullets;

        //currentUsedWeapon = pistolWeapon;
        switchWeapon(2);
    }

    //this will switch weapons of the player, its called from the character input
    public void switchWeapon(int weaponToSwitchTo_int)
    {
        //this will store the weapon that we are going to change to
        Gun weaponToSwitchTo = new Gun();
        //this will store the weapon info of the weapon we are going to change to
        Gun bulletInformationOfWeaponToChangeTo = new Gun();
        //this will store the bullet info of the old weapon before switching guns
        switch (currentUsedWeaponId)
        {
            case 0:
                Debug.Log("ERROR, NO WEAPON HAS 0 HAs THE INT To Switch");
                break;
            case 1:
                rifleWeaponInfo.currentBulletsOnMagazine = currentUsedWeapon.gun.currentBulletsOnMagazine;
                rifleWeaponInfo.spareBullets = currentUsedWeapon.gun.spareBullets;
                break;
            case 2:
                pistolWeaponInfo.currentBulletsOnMagazine = currentUsedWeapon.gun.currentBulletsOnMagazine;
                pistolWeaponInfo.spareBullets = currentUsedWeapon.gun.spareBullets;
                break;
            case 3:
                knifeWeaponInfo.currentBulletsOnMagazine = currentUsedWeapon.gun.currentBulletsOnMagazine;
                knifeWeaponInfo.spareBullets = currentUsedWeapon.gun.spareBullets;
                break;
        }

        //changed the current weapon to the weapon we just switched to 
        switch (weaponToSwitchTo_int)
        {
            case 0:
                Debug.Log("ERROR, NO WEAPON HAS 0 HAs THE INT To Switch");
                break;
            case 1:
                weaponToSwitchTo = rifleWeapon.gun; 
                currentUsedWeaponId = weaponToSwitchTo_int;
                bulletInformationOfWeaponToChangeTo = rifleWeaponInfo;
                break;
            case 2:
                weaponToSwitchTo = pistolWeapon.gun;
                currentUsedWeaponId = weaponToSwitchTo_int;
                bulletInformationOfWeaponToChangeTo = pistolWeaponInfo;
                break;
            case 3:
                weaponToSwitchTo = knifeWeapon.gun;
                currentUsedWeaponId = weaponToSwitchTo_int;
                bulletInformationOfWeaponToChangeTo = knifeWeaponInfo;
                break;
        }

        //edit every gun infromation except the spawn and image
        currentUsedWeapon.gun.damage = weaponToSwitchTo.damage;
        currentUsedWeapon.gun.rateOfFire = weaponToSwitchTo.rateOfFire;
        currentUsedWeapon.gun.bulletSpeed = weaponToSwitchTo.bulletSpeed;
        currentUsedWeapon.gun.reloadTime = weaponToSwitchTo.reloadTime;
        currentUsedWeapon.gun.moneyWorth = weaponToSwitchTo.moneyWorth;        
        currentUsedWeapon.gun.bulletsPerMagazine = weaponToSwitchTo.bulletsPerMagazine;

        currentUsedWeapon.gun.currentBulletsOnMagazine = bulletInformationOfWeaponToChangeTo.currentBulletsOnMagazine;
        currentUsedWeapon.gun.spareBullets = bulletInformationOfWeaponToChangeTo.spareBullets;

        currentUsedWeapon.gun.weight = weaponToSwitchTo.weight;

        //change the bullet
        currentUsedWeapon.gun.bullet = weaponToSwitchTo.bullet;

        //change the gun image
        currentUsedWeapon.gun.gunImage = weaponToSwitchTo.gunImage;
        currentUsedWeapon.gameObject.GetComponent<SpriteRenderer>().sprite = weaponToSwitchTo.gunImage;

        //rescale to the weapon scale
        currentUsedWeapon.transform.localScale = weaponToSwitchTo.scaleOfWeapon;

        //this will change the bullet spawn point to the obe of this gun
        currentUsedWeapon.gun.bulletSpawnPos.transform.localPosition = weaponToSwitchTo.bulletSpawnPos.transform.localPosition;        
        

        currentUsedWeapon.UpdateBulletsText();
        
        
    }

    //this will store the infromation of the switched weapon
    void StoreInformationOfTheSwitchedWeapon()
    { 
    
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
