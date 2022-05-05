using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class BuyingWeaponStation : MonoBehaviour
{
    [SerializeField]
    //this will store the weapon that can be bough from this station
    GunInformation weaponToBuy;

    [SerializeField]
    //we need to tell this script that the player is on our buying station if he enters us
    BuyWeaponManager buyWeaponManager;

    //this will store the player with my photon view
    GameObject myPlayer;

    

    //when a trigger enters
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if who entered is my player
        if (collision.CompareTag("Player") && collision.GetComponent<PhotonView>().IsMine)
        {
            //tell the code at what buystation we are
            buyWeaponManager.CurrentBuystationThePlayerIsOn(this);
            //this will activate the buy weapon button
            buyWeaponManager.ActivateButton();
            //tell the code that this is my player
            myPlayer = collision.gameObject;
        }
    }

    //when a trigger exits
    private void OnTriggerExit2D(Collider2D collision)
    {
        //if who Exited is my player
        if (collision.CompareTag("Player") && collision.GetComponent<PhotonView>().IsMine)
        {
            //tell the code at what buystation we are
            buyWeaponManager.ForgetBuystationThePlayerIsOn();
            //this will activate the buy weapon button
            buyWeaponManager.DeactivateButton();
        }
    }

    //this will buy the weapon
    public void BuyWeapon()
    {
        Debug.Log("BUYingWeapon");
        //this will get the player statics scrript
        PlayerStatsAndFunctionalities playerStatsScript;
        playerStatsScript = myPlayer.GetComponent<PlayerStatsAndFunctionalities>();

        //if we have enough money to buy a weapon
        if (playerStatsScript.playerStats.money >= weaponToBuy.gun.moneyWorth)
        {
            CharacterWeapon playerCharacterWeapon = myPlayer.GetComponent<CharacterWeapon>();
            //this will do a switch case to see wich type of weapon the user is going to buy
            switch (weaponToBuy.gun.typeOfWeapon)
            {
                case 0:
                    Debug.Log("ERROR, NO WEAPON HAS 0 HAs THE INT To Switch");
                    break;
                case 1:
                    //change the sprite of the rifle
                    playerCharacterWeapon.rifle.GetComponent<SpriteRenderer>().sprite = weaponToBuy.gun.gunImage;
                    //rescale the rifle
                    playerCharacterWeapon.rifle.gameObject.transform.localScale = weaponToBuy.gun.scaleOfWeapon;

                    //edit every parameter of the rifle (i dont do gun = gun, because it has some information like the bullet spawn that we can change just move)
                    playerCharacterWeapon.rifle.gun.damage = weaponToBuy.gun.damage;
                    playerCharacterWeapon.rifle.gun.rateOfFire = weaponToBuy.gun.rateOfFire;
                    playerCharacterWeapon.rifle.gun.bulletSpeed = weaponToBuy.gun.bulletSpeed;
                    playerCharacterWeapon.rifle.gun.reloadTime = weaponToBuy.gun.reloadTime;
                    playerCharacterWeapon.rifle.gun.moneyWorth = weaponToBuy.gun.moneyWorth;
                    playerCharacterWeapon.rifle.gun.currentBulletsOnMagazine = weaponToBuy.gun.currentBulletsOnMagazine;
                    playerCharacterWeapon.rifle.gun.bulletsPerMagazine = weaponToBuy.gun.bulletsPerMagazine;
                    playerCharacterWeapon.rifle.gun.spareBullets = weaponToBuy.gun.spareBullets;
                    playerCharacterWeapon.rifle.gun.weight = weaponToBuy.gun.weight;
                    playerCharacterWeapon.rifle.gun.bulletSpawnPos.transform.localPosition = weaponToBuy.gun.bulletSpawnPos.localPosition;
                    playerCharacterWeapon.rifle.gun.bullet = weaponToBuy.gun.bullet;

                    //switch the weapon to the one bought
                    playerCharacterWeapon.switchWeapon(1);
                    break;
                case 2:
                    //change the sprite of the pistol
                    playerCharacterWeapon.pistol.GetComponent<SpriteRenderer>().sprite = weaponToBuy.gun.gunImage;
                    //rescale the pistol
                    playerCharacterWeapon.pistol.gameObject.transform.localScale = weaponToBuy.gun.scaleOfWeapon;

                    //edit every parameter of the pistol (i dont do gun = gun, because it has some information like the bullet spawn that we can change just move)
                    playerCharacterWeapon.pistol.gun.damage = weaponToBuy.gun.damage;
                    playerCharacterWeapon.pistol.gun.rateOfFire = weaponToBuy.gun.rateOfFire;
                    playerCharacterWeapon.pistol.gun.bulletSpeed = weaponToBuy.gun.bulletSpeed;
                    playerCharacterWeapon.pistol.gun.reloadTime = weaponToBuy.gun.reloadTime;
                    playerCharacterWeapon.pistol.gun.moneyWorth = weaponToBuy.gun.moneyWorth;
                    playerCharacterWeapon.pistol.gun.currentBulletsOnMagazine = weaponToBuy.gun.currentBulletsOnMagazine;
                    playerCharacterWeapon.pistol.gun.bulletsPerMagazine = weaponToBuy.gun.bulletsPerMagazine;
                    playerCharacterWeapon.pistol.gun.spareBullets = weaponToBuy.gun.spareBullets;
                    playerCharacterWeapon.pistol.gun.weight = weaponToBuy.gun.weight;
                    playerCharacterWeapon.pistol.gun.bulletSpawnPos.transform.localPosition = weaponToBuy.gun.bulletSpawnPos.localPosition;
                    playerCharacterWeapon.pistol.gun.bullet = weaponToBuy.gun.bullet;
                    //switch the weapon to the one bought
                    playerCharacterWeapon.switchWeapon(2);
                    break;
                case 3:
                    //change the sprite of the pistol
                    playerCharacterWeapon.knife.GetComponent<SpriteRenderer>().sprite = weaponToBuy.gun.gunImage;
                    //rescale the pistol
                    playerCharacterWeapon.knife.gameObject.transform.localScale = weaponToBuy.gun.scaleOfWeapon;

                    //edit every parameter of the pistol (i dont do gun = gun, because it has some information like the bullet spawn that we can change just move)
                    playerCharacterWeapon.knife.gun.damage = weaponToBuy.gun.damage;
                    playerCharacterWeapon.knife.gun.rateOfFire = weaponToBuy.gun.rateOfFire;
                    playerCharacterWeapon.knife.gun.bulletSpeed = weaponToBuy.gun.bulletSpeed;
                    playerCharacterWeapon.knife.gun.reloadTime = weaponToBuy.gun.reloadTime;
                    playerCharacterWeapon.knife.gun.moneyWorth = weaponToBuy.gun.moneyWorth;
                    playerCharacterWeapon.knife.gun.currentBulletsOnMagazine = weaponToBuy.gun.currentBulletsOnMagazine;
                    playerCharacterWeapon.knife.gun.bulletsPerMagazine = weaponToBuy.gun.bulletsPerMagazine;
                    playerCharacterWeapon.knife.gun.spareBullets = weaponToBuy.gun.spareBullets;
                    playerCharacterWeapon.knife.gun.weight = weaponToBuy.gun.weight;
                    playerCharacterWeapon.knife.gun.bulletSpawnPos.transform.localPosition = weaponToBuy.gun.bulletSpawnPos.localPosition;
                    playerCharacterWeapon.knife.gun.bullet = weaponToBuy.gun.bullet;
                    //switch the weapon to the one bought
                    playerCharacterWeapon.switchWeapon(3);
                    break;
            }
            playerStatsScript.playerStats.money -= weaponToBuy.gun.moneyWorth;
            //update the texts
            playerStatsScript.UpdateMoneyText();
            weaponToBuy.UpdateBulletsText();
        }
    }
}
