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
           
            

            switch (weaponToBuy.gun.typeOfWeapon)
            {
                case 0:
                    Debug.Log("ERROR, NO WEAPON HAS 0 HAs THE INT To Switch");
                    break;
                case 1:
                    myPlayer.GetComponent<CharacterWeapon>().rifle = weaponToBuy;
                    break;
                case 2:
                    myPlayer.GetComponent<CharacterWeapon>().pistol = weaponToBuy;
                    break;
                case 3:
                    myPlayer.GetComponent<CharacterWeapon>().knife = weaponToBuy;
                    break;
            }

            //update the texts
            playerStatsScript.UpdateMoneyText();
            //playerGunInformation.UpdateBulletsText();
        }
    }
}
