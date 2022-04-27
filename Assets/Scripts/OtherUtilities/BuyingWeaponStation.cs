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
    //this will hold the buy weapon 
    Button buttonToBuy;

    //this will store the player with my photon view
    GameObject myPlayer;

    //when a trigger enters
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if who entered is my player
        if (collision.CompareTag("Player") && collision.GetComponent<PhotonView>().IsMine)
        {
            //show the buy button, since we are in buying area
            buttonToBuy.gameObject.SetActive(true);
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
            //hide the buy button, since we are in buying area
            buttonToBuy.gameObject.SetActive(false);
        }
    }

    //this will buy the weapon
    public void BuyWeapon()
    {
        PlayerStatsAndFunctionalities playerStatsScript;
        playerStatsScript = myPlayer.GetComponent<PlayerStatsAndFunctionalities>();

        if (playerStatsScript.playerStats.money >= weaponToBuy.gun.moneyWorth)
        {
            playerStatsScript.playerStats.money -= weaponToBuy.gun.moneyWorth;
            playerStatsScript.UpdateMoneyText();

            //this will get the player gun
            GunInformation playerGunInformation;
            playerGunInformation = myPlayer.GetComponent<CharacterWeapon>().currentUsedWeapon;

            //edit every gun infromation except the spawn and image
            playerGunInformation.gun.damage = weaponToBuy.gun.damage;
            playerGunInformation.gun.rateOfFire = weaponToBuy.gun.rateOfFire;
            playerGunInformation.gun.bulletSpeed = weaponToBuy.gun.bulletSpeed;
            playerGunInformation.gun.reloadTime = weaponToBuy.gun.reloadTime;
            playerGunInformation.gun.moneyWorth = weaponToBuy.gun.moneyWorth;
            playerGunInformation.gun.currentBulletsOnMagazine = weaponToBuy.gun.currentBulletsOnMagazine;
            playerGunInformation.gun.bulletsPerMagazine = weaponToBuy.gun.bulletsPerMagazine;
            playerGunInformation.gun.spareBullets = weaponToBuy.gun.spareBullets;
            playerGunInformation.gun.weight = weaponToBuy.gun.weight;

            playerGunInformation.bullet = weaponToBuy.bullet;

            //change the gun image
            playerGunInformation.gun.gunImage = weaponToBuy.gun.gunImage;
            playerGunInformation.gameObject.GetComponent<SpriteRenderer>().sprite = weaponToBuy.gun.gunImage;
        }
    }
}
