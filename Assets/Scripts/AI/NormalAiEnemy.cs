using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AI;

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

    //this wil be true if it can attack and false if it cant
    bool canIAttack = true;

    [SerializeField]
    //get the animator to be able to change animations
    Animator aiAnimator;

    [SerializeField]
    //this needs to have the same name has the attack parameter of the animator controller
    string attackAnimatorParameterName;

    [SerializeField]
    //this needs to have the same name has the die parameter of the animator controller
    string dieAnimatorParameterName;

    private void Start()
    {
        //store the script from this gameobject
        aiEnemyInformationScript = this.gameObject.GetComponent<AiEnemyInformation>();
        //get the enemy max hp
        aiEnemyInformationScript.thisEnemy.maxHp = aiEnemyInformationScript.thisEnemy.hp;

    }

    //if collided with some collider
    void OnCollisionEnter2D(Collision2D col)
    {
        BulletInformation bulletInformationScriptOfBullet;
        //this will checl of ythe bullet has a bullet information, if it has it will get damaged
        if(col.gameObject.GetComponent<BulletInformation>())
        {
            //since we have the script we will get the bullet script so that we can use it 
            bulletInformationScriptOfBullet = col.gameObject.GetComponent<BulletInformation>();
            
            //this will call the function that will decrease the ai hp and update the hp bar
            RemoveHpAndUpdateHpBar(bulletInformationScriptOfBullet.damage);
            
            //if the hp is less or equal to 0, call the function die
            if (aiEnemyInformationScript.thisEnemy.hp <= 0)
            {
                //if the fucntion die havent been called on this ai enemy, call the function die
                if (haveIHaverBeenCalled == false)
                {
                    //this will call the function Die, on every pc on the server
                    photonView.RPC("Die", RpcTarget.All, bulletInformationScriptOfBullet.playerThatShotMe.gameObject.GetComponent<PhotonView>().ViewID);
              
                }
                    

            }
        }

    }

    //if collided with some collider
    private void OnTriggerStay2D(Collider2D col)
    {   
        //if a player is inside our trigger, attack him
        if (col.gameObject.CompareTag("Player") && canIAttack)
        {
            //call the function to attack the player
            Attack(col.gameObject);
            //this will call the coroutine that is going to control the rate of fire
            StartCoroutine(AttackRateOfAttacksController());
        }

    }

    IEnumerator AttackRateOfAttacksController()
    {
        canIAttack = false;
        //wait a time before running the code below
        yield return new WaitForSeconds(aiEnemyInformationScript.thisEnemy.rateOfAttack);
        canIAttack = true;
    }

    //this function will attack a player if the photon view is ours
    void Attack(GameObject playerAttacked)
    {
        //if this has my photon view, its because it my player and we can attack him
        if(playerAttacked.GetComponent<PhotonView>().IsMine)
        {
            //tell the animator to attack
            aiAnimator.SetBool(attackAnimatorParameterName, true);

            //remove hp from my player
            playerAttacked.GetComponent<PlayerStatsAndFunctionalities>().RemoveHp(aiEnemyInformationScript.thisEnemy.damage);

            
        }
        //if this does not have my photon view, its because its not my player and we can not attack him
        else
        {
            //tell the animator to attack
            aiAnimator.SetBool(attackAnimatorParameterName, true);
        }
    }

    [PunRPC]
    void Die(int idOfPlayerThatKilledMe)
    {
        //get the player spawned, by finding its id
        GameObject playerSpawned = PhotonView.Find(idOfPlayerThatKilledMe).gameObject;
        //tell the player that killed me that he killed me.
        playerSpawned.GetComponent<PlayerStatsAndFunctionalities>().KilledEnemy(aiEnemyInformationScript.thisEnemy.moneyWorth);
        //tell the code that the function die has already been called ( to prevent it from being called twice since this function is online)
        haveIHaverBeenCalled = true;
        //tell the ai enemy information that it died, for him to inform the RoundSystem
        aiEnemyInformationScript.Death();
        //this will tell the animator to die
        aiAnimator.SetBool(dieAnimatorParameterName, true);

        //if im the master client i wont have a navmesh agent and a ai move path find
        if (PhotonNetwork.IsMasterClient)
        {
            //since this ai is dead i deactivate the ai move path find
            this.GetComponent<AiMovePathFind>().enabled = false;
            //this will disable the navmesh agent so that he stops following the player
            this.GetComponent<NavMeshAgent>().enabled = false;
        }

        //this will disable every collider of the ai
        foreach (Collider2D c in GetComponents<Collider2D>())
        {
            c.enabled = false;
        }
        //in the ened we disable this script aswell, the game object is destroyd on the state machine script destroyafteranim
        this.enabled = false;

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
