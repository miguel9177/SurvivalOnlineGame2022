using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;


[System.Serializable]
public struct Gun
{
    //initialize every variable necessary for a gun
    public float damage;
    public float rateOfFire;
    public float bulletSpeed;
    public float reloadTime;
    public float moneyWorth;
    public int currentBulletsOnMagazine;
    public int bulletsPerMagazine;
    public int spareBullets;
    public float weight;
    public Sprite gunImage;
    public Transform bulletSpawnPos;
    public NormalBullet bullet;
    public Vector2 scaleOfWeapon;
}

//this script will store the gun information, with this i can have multiple types of weapons and just sedn the information here
public abstract class GunInformation : MonoBehaviourPun
{
    //this will create a variable for the gun struct, making it have all the statics of the weapon here
    public Gun gun;

    //this will be from 3 to 1, 1 is a rifle, 2 is a pistol 3 is a knife
    public int typeOfWeapon;

    [HideInInspector]
    //this is the textbox for the bullets of the weapon, this is assigned by the uimanager
    public TextMeshProUGUI txtBulletsOfWeapon;

    [HideInInspector]
    //this is the textbox for the reload information
    public TextMeshProUGUI txtReloadInformation;

    [SerializeField]
    //this will store the character weapon script, so that i can block the switching of weapons
    public CharacterWeapon characterWeaponsScript;

    [SerializeField]
    //i need to store this script here, so that i can know wich player shot the bullet so that i can know who to give the money
    public PlayerStatsAndFunctionalities playerStats;

    public virtual void CallShootFunction()
    {

    }

    public virtual void UpdateBulletsText()
    {

    }

}
