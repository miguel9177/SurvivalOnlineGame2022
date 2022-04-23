using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BulletInformation))]
public class NormalBullet : MonoBehaviour
{
    [HideInInspector]
    //this is going to be used to take damage
    public float damage;

    
    private void Start()
    {
        //call coroutine that will wait 3 seconds and then destroy the bullet
        StartCoroutine(DestroyAfterSeconds(3f));
    }

    //this coroutine will wait seconds and then destrouy the bullet
    IEnumerator DestroyAfterSeconds(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        Destroy(this.gameObject);
    }

    //this is going to add force to the bullet
    public void AddForceToBullet(Vector3 moveDirection, float speed, float damage)
    {
        //this is going to add velocity to where the spawn bullet is facing, in this case is from the spawn point
        this.GetComponent<Rigidbody2D>().AddForce(moveDirection * speed);
        //this is going to tell the script that is going to store the bullet information how much damage this bullet does
        this.GetComponent<BulletInformation>().damage = damage;
    }

    //if collided with some collider
    void OnCollisionEnter2D(Collision2D col)
    {
        //destroy the bullet
        Destroy(this.gameObject);
    }


}
