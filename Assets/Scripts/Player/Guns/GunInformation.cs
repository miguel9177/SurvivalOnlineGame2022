using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

//this script will store the gun information, with this i can have multiple types of weapons and just sedn the information here
public abstract class GunInformation : MonoBehaviourPun
{

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
    }

    public Gun gun;

    //this will be from 3 to 1, 1 is a rifle, 2 is a pistol 3 is a knife
    public int typeOfWeapon;

    [SerializeField]
    public NormalBullet bullet;

    //this will store the function to shoot, i use this so that i dont have to know wich type of weapon the user is using 
    //public delegate void ShootFunctionToCall();
    //public ShootFunctionToCall firingMethod;

    //this will store the function to shoot, i use this so that i dont have to know wich type of weapon the user is using 
    public delegate void ReloadFunctionToCall();
    public ReloadFunctionToCall reloadMethod;

    [HideInInspector]
    //this is the textbox for the bullets of the weapon, this is assigned by the uimanager
    public TextMeshProUGUI txtBulletsOfWeapon;

    [HideInInspector]
    //this is the textbox for the reload information
    public TextMeshProUGUI txtReloadInformation;

    
    public virtual void CallShootFunction()
    {

    }


}
