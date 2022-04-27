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
            //tell the button buy that we on click, he will buy our weapon
            buttonToBuy.onClick.AddListener(BuyWeapon);

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
            //since we left the buy zone we remove all listeners, since we cant buy the weapon
            buttonToBuy.onClick.RemoveAllListeners();
        }
    }

    //this will buy the weapon
    public void BuyWeapon()
    {
        //this will get the player statics scrript
        PlayerStatsAndFunctionalities playerStatsScript;
        playerStatsScript = myPlayer.GetComponent<PlayerStatsAndFunctionalities>();

        //if we have enough money to buy a weapon
        if (playerStatsScript.playerStats.money >= weaponToBuy.gun.moneyWorth)
        {
            //decrease the money since we bought the weapon
            playerStatsScript.playerStats.money -= weaponToBuy.gun.moneyWorth;
            

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

            //change the bullet
            playerGunInformation.bullet = weaponToBuy.bullet;

            //change the gun image
            playerGunInformation.gun.gunImage = weaponToBuy.gun.gunImage;
            playerGunInformation.gameObject.GetComponent<SpriteRenderer>().sprite = weaponToBuy.gun.gunImage;

            //rescale to the weapon scale
            playerGunInformation.transform.localScale = weaponToBuy.gameObject.transform.localScale;

            //this will change the bullet spawn point to the obe of this gun
            playerGunInformation.gun.bulletSpawnPos.transform.localPosition = weaponToBuy.gun.bulletSpawnPos.transform.localPosition;

            //update the texts
            playerStatsScript.UpdateMoneyText();
            playerGunInformation.UpdateBulletsText();
        }
    }
}
