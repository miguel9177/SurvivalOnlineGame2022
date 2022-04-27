using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;





public class NormalGun : GunInformation
{





    //this bool will be true when we are reloading
    bool reloading = false;
    //this will be true when i can shoot, this  will let me control the rate of fire
    bool canISHoot = true;

    // Start is called before the first frame update
    void Start()
    {
        
        //delegate the function callshoot to the gun information
        //gunInformation.firingMethod = CallShootFunction;
        //delegate the function reload to the gun information
        reloadMethod = Reload;
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
    public override void CallShootFunction()
    {
        //if we have bullets on the magazine, and we are not reloading, and we can shoot
        if(gun.currentBulletsOnMagazine > 0 && reloading == false && canISHoot==true)
        {
            //this will call the function shoot, on every pc on the server, and send the info about the position so that everyone gets this player position
            photonView.RPC("Shoot", RpcTarget.All, gun.bulletSpawnPos.position, gun.bulletSpeed, gun.damage);
            //call the coroutine for controlling the rate of fire
            StartCoroutine(RateOfFireController());
        }
    }

    //this courotine will controll the rate of fire
    IEnumerator RateOfFireController()
    {
        //tell the code to dont shoot
        canISHoot = false;
        yield return new WaitForSeconds(gun.rateOfFire);
        //tell the code we can shoot
        canISHoot = true;
    }

    //this is called by the character input script
    void Reload()
    {
        //if we dont have max ammo and theres more then 0 spare bullets, we can reload
        if (gun.currentBulletsOnMagazine < gun.bulletsPerMagazine && gun.spareBullets > 0)
        {
            //make the text saying the user is reloading visible
            txtReloadInformation.gameObject.SetActive(true);
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
        txtReloadInformation.gameObject.SetActive(false);
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
        GameObject projectileInstance = Instantiate(gun.bullet.gameObject, position_, Quaternion.identity);

        //this will store who shot the bullet
        projectileInstance.GetComponent<BulletInformation>().playerThatShotMe = playerStats;
        //this is going to call the function that will add force to the bullet
        projectileInstance.GetComponent<NormalBullet>().AddForceToBullet(-transform.up, bulletSpeed_, gunDamage_);

        //since we shot a bullet we remove one
        gun.currentBulletsOnMagazine -= 1;
        //call th function to update the text o fthe bullets
        UpdateBulletsText();
    }

    //this function will write the correct text on the bullets text
    public override void UpdateBulletsText()
    {
        //if this photon view is mine, edit the text of the bullets, like this i only see my bullets
        if(photonView.IsMine && txtBulletsOfWeapon != null)
            //write on the text the number of bullets
            txtBulletsOfWeapon.text = gun.currentBulletsOnMagazine.ToString() + " / " + gun.spareBullets.ToString();
    }

   
}
