using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    //this is going to be used to take damage
    private float damage;

    //this is going to add force to the bullet
    public void AddForceToBullet(Vector3 moveDirection, float speed, float damage)
    {
        //this is going to add velocity to where the spawn bullet is facing, in this case is from the spawn point
        this.GetComponent<Rigidbody2D>().AddForce(moveDirection * speed);
    }

}
