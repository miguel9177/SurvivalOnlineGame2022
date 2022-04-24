using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;



//its mandatory to have this script since it will be the script that comunicates with other scripts, without knowing if the enemy is a normal or other type
[RequireComponent(typeof(AiEnemyInformation))]
public class NormalAiEnemy : MonoBehaviourPun
{
    [SerializeField]
    //this will store the gameobject with the slider of the health bar
    Slider HpSlider;

    //this will check if the die function have already been called
    bool haveIHaverBeenCalled = false;

    //this will store the script so that we can call the die function
    AiEnemyInformation aiEnemyInformationScript = null;

    private void Start()
    {
        //store the script from this gameobject
        aiEnemyInformationScript = this.gameObject.GetComponent<AiEnemyInformation>();
        
        aiEnemyInformationScript.thisEnemy.maxHp = aiEnemyInformationScript.thisEnemy.hp;

    }

    //if collided with some collider
    void OnCollisionEnter2D(Collision2D col)
    {
        //this will checl of ythe bullet has a bullet information, if it has it will get damaged
        if(col.gameObject.GetComponent<BulletInformation>())
        {
            //this will call the function that will decrease the ai hp and update the hp bar
            RemoveHpAndUpdateHpBar(col.gameObject.GetComponent<BulletInformation>().damage);
            
            //if the hp is less or equal to 0, call the function die
            if (aiEnemyInformationScript.thisEnemy.hp <= 0)
            {
                //if the fucntion die havent been called on this ai enemy, call the function die
                if(haveIHaverBeenCalled == false)
                    //this will call the function Die, on every pc on the server
                    photonView.RPC("Die", RpcTarget.All);

            }
        }
    }

    [PunRPC]
    void Die()
    {
        //tell the code that the function die has already been called ( to prevent it from being called twice since this function is online)
        haveIHaverBeenCalled = true;
        //tell the ai enemy information that it died, for him to inform the RoundSystem
        aiEnemyInformationScript.Death();
        //destroy this object, since it died
        Destroy(this.gameObject);
    }

    //this function will decrease hp of the enemy, and update the slider
    public void RemoveHpAndUpdateHpBar(float hpAmmountToLose)
    {
        //decrease the hp of this ai enemy
        aiEnemyInformationScript.thisEnemy.hp = aiEnemyInformationScript.thisEnemy.hp - hpAmmountToLose;
        //update the hp bar
        HpSlider.value = aiEnemyInformationScript.thisEnemy.hp / aiEnemyInformationScript.thisEnemy.maxHp;
    }
}
