using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    //this will store the bullet information, of the weaopns, since we change weapons, we nned to store their bullets
    struct weaponBulletInformation
    {
        public int currentBulletsOnMagazine;
        public int spareBullets;
    }

    [SerializeField]
    //this is going to store the current knife character weapon
    public GunInformation knifeWeapon;
    //this will store the knife bullet info of this knife, since we cant edit anything on the knife variable directly, or it will change the prefab
    weaponBulletInformation knifeWeaponBulletInfo;

    [SerializeField]
    //this is going to store the current pistol character weapon
    public GunInformation pistolWeapon;
    //this will store the bullet info of this pistol, since we cant edit anything on the pistol variable directly, or it will change the prefab
    weaponBulletInformation pistolWeaponBulletInfo;

    [SerializeField]
    //this is going to store the current rifle character weapon
    public GunInformation rifleWeapon;
    //this will store the bullet info of this rifle, since we cant edit anything on the rifle variable directly, or it will change the prefab
    weaponBulletInformation rifleWeaponBulletInfo;

    [SerializeField]
    [Header("this needs to be set to the gun that is a child of this object")]
    //this will hold the current weapon being used
    public GunInformation currentUsedWeapon;

    //value from 1 to 3 , 1 is a rifle, 2 is a pistol, 3 is a knife
    int currentUsedWeaponId = 2;

    private void Awake()
    {
        rifleWeaponBulletInfo.currentBulletsOnMagazine = rifleWeapon.gun.currentBulletsOnMagazine;
        rifleWeaponBulletInfo.spareBullets = rifleWeapon.gun.spareBullets;

        pistolWeaponBulletInfo.currentBulletsOnMagazine = pistolWeapon.gun.currentBulletsOnMagazine;
        pistolWeaponBulletInfo.spareBullets = pistolWeapon.gun.spareBullets;

        knifeWeaponBulletInfo.currentBulletsOnMagazine = knifeWeapon.gun.currentBulletsOnMagazine;
        knifeWeaponBulletInfo.spareBullets = knifeWeapon.gun.spareBullets;

        //currentUsedWeapon = pistolWeapon;
        switchWeapon(2);
    }

    //this will switch weapons of the player, its called from the character input
    public void switchWeapon(int weaponToSwitchTo_int)
    {
        //this will store the weapon that we are going to change to
        GunInformation weaponToSwitchTo = null;
        //this will store the weapon info of the weapon we are going to change to
        weaponBulletInformation bulletInformationOfWeaponToChangeTo = new weaponBulletInformation();
        //this will store the bullet info of the old weapon before switching guns
        switch (currentUsedWeaponId)
        {
            case 0:
                Debug.Log("ERROR, NO WEAPON HAS 0 HAs THE INT To Switch");
                break;
            case 1:
                rifleWeaponBulletInfo.currentBulletsOnMagazine = currentUsedWeapon.gun.currentBulletsOnMagazine;
                rifleWeaponBulletInfo.spareBullets = currentUsedWeapon.gun.spareBullets;
                break;
            case 2:
                pistolWeaponBulletInfo.currentBulletsOnMagazine = currentUsedWeapon.gun.currentBulletsOnMagazine;
                pistolWeaponBulletInfo.spareBullets = currentUsedWeapon.gun.spareBullets;
                break;
            case 3:
                knifeWeaponBulletInfo.currentBulletsOnMagazine = currentUsedWeapon.gun.currentBulletsOnMagazine;
                knifeWeaponBulletInfo.spareBullets = currentUsedWeapon.gun.spareBullets;
                break;
        }

        //changed the current weapon to the weapon we just switched to 
        switch (weaponToSwitchTo_int)
        {
            case 0:
                Debug.Log("ERROR, NO WEAPON HAS 0 HAs THE INT To Switch");
                break;
            case 1:
                weaponToSwitchTo = rifleWeapon; 
                currentUsedWeaponId = weaponToSwitchTo_int;
                bulletInformationOfWeaponToChangeTo = rifleWeaponBulletInfo;
                break;
            case 2:
                weaponToSwitchTo = pistolWeapon;
                currentUsedWeaponId = weaponToSwitchTo_int;
                bulletInformationOfWeaponToChangeTo = pistolWeaponBulletInfo;
                break;
            case 3:
                weaponToSwitchTo = knifeWeapon;
                currentUsedWeaponId = weaponToSwitchTo_int;
                bulletInformationOfWeaponToChangeTo = knifeWeaponBulletInfo;
                break;
        }

        //edit every gun infromation except the spawn and image
        currentUsedWeapon.gun.damage = weaponToSwitchTo.gun.damage;
        currentUsedWeapon.gun.rateOfFire = weaponToSwitchTo.gun.rateOfFire;
        currentUsedWeapon.gun.bulletSpeed = weaponToSwitchTo.gun.bulletSpeed;
        currentUsedWeapon.gun.reloadTime = weaponToSwitchTo.gun.reloadTime;
        currentUsedWeapon.gun.moneyWorth = weaponToSwitchTo.gun.moneyWorth;
        //currentUsedWeapon.gun.currentBulletsOnMagazine = weaponToSwitchTo.gun.currentBulletsOnMagazine;
        
        currentUsedWeapon.gun.bulletsPerMagazine = weaponToSwitchTo.gun.bulletsPerMagazine;

        currentUsedWeapon.gun.currentBulletsOnMagazine = bulletInformationOfWeaponToChangeTo.currentBulletsOnMagazine;
        currentUsedWeapon.gun.spareBullets = bulletInformationOfWeaponToChangeTo.spareBullets;

        currentUsedWeapon.gun.weight = weaponToSwitchTo.gun.weight;

        //change the bullet
        currentUsedWeapon.bullet = weaponToSwitchTo.bullet;

        //change the gun image
        currentUsedWeapon.gun.gunImage = weaponToSwitchTo.gun.gunImage;
        currentUsedWeapon.gameObject.GetComponent<SpriteRenderer>().sprite = weaponToSwitchTo.gun.gunImage;

        //rescale to the weapon scale
        currentUsedWeapon.transform.localScale = weaponToSwitchTo.gameObject.transform.localScale;

        //this will change the bullet spawn point to the obe of this gun
        currentUsedWeapon.gun.bulletSpawnPos.transform.localPosition = weaponToSwitchTo.gun.bulletSpawnPos.transform.localPosition;        
        

        currentUsedWeapon.UpdateBulletsText();
        
        
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
