using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
struct Gun
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

[RequireComponent(typeof(GunInformation))]
public class NormalGun : MonoBehaviourPun
{
    [SerializeField]
    //this will store the settings of this gun
    Gun gun;

    [SerializeField]
    NormalBullet bullet;

    [SerializeField]
    //i need to store this script here, so that i can know wich player shot the bullet so that i can know who to give the money
    PlayerStatsAndFunctionalities playerStats;

    //this will store the gun information script, so that it can assign the shoot function and this script receive the text for the bullets
    GunInformation gunInformation;

    //this bool will be true when we are reloading
    bool reloading = false;

    // Start is called before the first frame update
    void Start()
    {
        //get the script
        gunInformation = this.gameObject.GetComponent<GunInformation>();
        //delegate the function callshoot to the gun information
        gunInformation.firingMethod = CallShootFunction;
        //delegate the function reload to the gun information
        gunInformation.reloadMethod = Reload;
        //call the late start courotine, so that we can update the bullets texzt, it needs to be called after every start, so that we have everything setted before, and like this we wont receive errors
        StartCoroutine(LateStart());   
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);
        //this will assign the textbox to the gun
        UpdateBulletsText();
    }

    #region delegated functions, they are delegated to the gun information, and the character inpu calls them
    //this is called by the Character input script
    public void CallShootFunction()
    {
        //if we have bullets on the magazine
        if(gun.currentBulletsOnMagazine > 0 && reloading == false)
            //this will call the function shoot, on every pc on the server, and send the info about the position so that everyone gets this player position
            photonView.RPC("Shoot", RpcTarget.All, gun.bulletSpawnPos.position, gun.bulletSpeed, gun.damage);
    }

    //this is called by the character input script
    void Reload()
    {
        //if we dont have max ammo and theres more then 0 spare bullets, we can reload
        if (gun.currentBulletsOnMagazine < gun.bulletsPerMagazine && gun.spareBullets > 0)
        {
            //make the text saying the user is reloading visible
            gunInformation.txtReloadInformation.gameObject.SetActive(true);
            //tell the code that im reloading
            reloading = true;
            //call the coroutine that will make the player stop reloading
            StartCoroutine(Reloading(gun.reloadTime));
        }
    }
    #endregion

    //this will be called by the relad function, and will wait for the reloading to endd, when it does, it will do the rest of the code
    IEnumerator Reloading(float timeToReload)
    {
        yield return new WaitForSeconds(timeToReload);
        //make the text saying the gun is reloading visible
        gunInformation.txtReloadInformation.gameObject.SetActive(false);
        //tell the code that we stopped reloading
        reloading = false;
        
        //if i have enough bullets for a full reload
        if(gun.spareBullets > (gun.bulletsPerMagazine - gun.currentBulletsOnMagazine))
        {
            int bulletsMissing = gun.bulletsPerMagazine - gun.currentBulletsOnMagazine;
            gun.currentBulletsOnMagazine += bulletsMissing;
            gun.spareBullets -= bulletsMissing;
        }
        else
        {
            gun.currentBulletsOnMagazine += gun.spareBullets;
            gun.spareBullets = 0;
        }
        UpdateBulletsText();
    }

    [PunRPC]
    void Shoot(Vector3 position_, float bulletSpeed_, float gunDamage_)
    {
        //this is going to spawn the bullet on every computer 
        GameObject projectileInstance = Instantiate(bullet.gameObject, position_, Quaternion.identity);

        //this will store who shot the bullet
        projectileInstance.GetComponent<BulletInformation>().playerThatShotMe = playerStats;
        //this is going to call the function that will add force to the bullet
        projectileInstance.GetComponent<NormalBullet>().AddForceToBullet(-transform.right, bulletSpeed_, gunDamage_);

        //since we shot a bullet we remove one
        gun.currentBulletsOnMagazine -= 1;
        //call th function to update the text o fthe bullets
        UpdateBulletsText();
    }

    //this function will write the correct text on the bullets text
    void UpdateBulletsText()
    {
        //if this photon view is mine, edit the text of the bullets, like this i only see my bullets
        if(photonView.IsMine)
            //write on the text the number of bullets
            gunInformation.txtBulletsOfWeapon.text = gun.currentBulletsOnMagazine.ToString() + " / " + gun.spareBullets.ToString();
    }

   
}
