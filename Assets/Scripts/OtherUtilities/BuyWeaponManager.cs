using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this script will work as a manager for te buying, it will receive the weapon from the current weapon station that the player is
//if the player enters a weapon station, this script will receive its weapon, and if the player clicks, buy weapon, it will buy the weapon
public class BuyWeaponManager : MonoBehaviour
{
    //this will store the weapon station that the player is on
    BuyingWeaponStation weaponStationThatThePlayerIsOn;

    [SerializeField]
    //this will store the buying button
    Button buyingButton;

    private void Start()
    {
        buyingButton.gameObject.SetActive(false);
    }

    //this will be called by the buying weapon station script, and will tell this script at what station the player is on
    public void CurrentBuystationThePlayerIsOn(BuyingWeaponStation weaponStation_)
    {
        weaponStationThatThePlayerIsOn = weaponStation_;    
    }

    //if a player enters a buing weapon station that station will call this function
    public void ActivateButton()
    {
        buyingButton.gameObject.SetActive(true);
    }


    //this will be called by the buying weapon station script, and will tell this script that the player left the
    public void ForgetBuystationThePlayerIsOn()
    {
        weaponStationThatThePlayerIsOn = null;
    }

    //if a player exits a buing weapon station that station will call this function
    public void DeactivateButton()
    {
        buyingButton.gameObject.SetActive(false);
    }

    //this will be called by the button on click, from the buy weapon button
    public void BuyWeapon()
    {
        buyingButton.gameObject.SetActive(false);
        weaponStationThatThePlayerIsOn.BuyWeapon();
    }
}
