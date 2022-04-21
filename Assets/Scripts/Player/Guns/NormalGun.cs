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
    public float currentBulletsOnMagazine;
    public float bulletsPerMagazine;
    public float totalBullets;
    public float weight;
    public Sprite gunImage;
    public Vector3 bulletSpawnOffset;
}

[RequireComponent(typeof(GunInformation))]
public class NormalGun : MonoBehaviourPun
{
    [SerializeField]
    //this will store the settings of this gun
    Gun gun;

    [SerializeField]
    NormalBullet bullet;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<GunInformation>().firingMethod = CallShootFunction; 
    }

    //this is called by the gun information script
    public void CallShootFunction()
    {
        //this will call the function shoot, on every pc on the server, and send the info about the position so that everyone gets this player position
        photonView.RPC("Shoot", RpcTarget.All, transform.position + gun.bulletSpawnOffset, gun.bulletSpeed, gun.damage);
    }

    [PunRPC]
    void Shoot(Vector3 position_, float bulletSpeed_, float gunDamage_)
    {
        //this is going to spawn the bullet on every computer 
        GameObject projectileInstance = Instantiate(bullet.gameObject, position_, Quaternion.identity);
        //this is going to call the function that will add force to the bullet
        projectileInstance.GetComponent<NormalBullet>().AddForceToBullet(transform.right, bulletSpeed_, gunDamage_);
    }


    void Reload()
    {

    }
}