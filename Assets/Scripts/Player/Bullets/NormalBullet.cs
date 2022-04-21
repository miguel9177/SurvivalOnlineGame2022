using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BulletInformation))]
public class NormalBullet : MonoBehaviour
{
    [HideInInspector]
    //this is going to be used to take damage
    public float damage;

    //this is going to add force to the bullet
    public void AddForceToBullet(Vector3 moveDirection, float speed, float damage)
    {
        //this is going to add velocity to where the spawn bullet is facing, in this case is from the spawn point
        this.GetComponent<Rigidbody2D>().AddForce(moveDirection * speed);
        //this is going to tell the script that is going to store the bullet information how much damage this bullet does
        this.GetComponent<BulletInformation>().damage = damage;
    }

}
