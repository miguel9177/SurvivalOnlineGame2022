using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class RoundSystem : MonoBehaviourPun
{
    //this will store how many ai enemies are on the scene
    private int currentnumberOfEnemiesInScene;

    //we need this variable to check if we already spawned every ai on this round
    private int enemiesSpawnedThisRound;

    //this will store in which round we are
    private int currentRound;

    //this will store the number of enemys to spawn per round
    private int currentNumberOfEnemiesToSpawnPerRound;

    [SerializeField]
    //this will store how much hp i increase per round
    int hpOfEnemiesToIncreasePerRound;

    [SerializeField]
    //this will store the number of enemies to increase per round
    int numberOfEnemiesToIncreasePerRound;

    [SerializeField]
    //this will store the limit of enemies on the scene, this will prevent fps drop due to too many enemies on the scene
    int limitOfEnemiesOnScene;

    //this will be true if im the master client, if i am i wont do most of the processing of the round system, only receive the punRpc Fucntion calls
    private bool amIMasterClient=false;

    //this will be true when we spawned every enemy, we need this so that we can start checking if we can start the next round
    private bool spawnedEveryEnemyInThisRound=false;

    //this will store the ai spawner script, so that we can stop spawning
    AiSpawner aiSpawnerScript;

    #region UI Elements
    [SerializeField]
    //this will store the textbox that is going to show the user in which round we are
    TextMeshProUGUI roundText;
    #endregion

    private void Start()
    {
        //get the ai spawner script, so that we can stop spawning
        aiSpawnerScript = GetComponent<AiSpawner>();
        
        //set the variables for the begining of the round
        currentRound = 1;
        roundText.text = currentRound.ToString();
        currentNumberOfEnemiesToSpawnPerRound = 10;

        //if we are the master client, tell the code that we are
        if (PhotonNetwork.IsMasterClient)
        {
            amIMasterClient=true;
        }
    }

    #region "template" functions, that their only objective is call the rpc function, so that it runs on every pc

    //this will be called by the AiEnemyInformation script, when its ai dies
    public void enemyDied(int spaceOccupiedByEnemy)
    {
        if(amIMasterClient)
        {
            //this will call the function enemyDiedRPC, on every pc on the server
            photonView.RPC("enemyDiedRPC", RpcTarget.All, spaceOccupiedByEnemy);
        }
    }

    //this is called by the ai spawner, and will let this script keep track of how many enemies there are on the scene
    public void enemySpawned(int spaceOccupiedByEnemy, int enemyPhotonId)
    {

        if (amIMasterClient)
        {
            //this will call the function enemyDiedRPC, on every pc on the server
            photonView.RPC("enemySpawnedRPC", RpcTarget.All, spaceOccupiedByEnemy, enemyPhotonId);
            //since we spawned an enemy we increase the value of the variable that counts the number of enemies spawned
            enemiesSpawnedThisRound = enemiesSpawnedThisRound + spaceOccupiedByEnemy;
            
            //if we already spawned every enemy, we call the function to stop spawning
            if (enemiesSpawnedThisRound >= currentNumberOfEnemiesToSpawnPerRound)
            {
                //this will call the function StopRound, on every pc on the server, since we dont wwant to spawn anymore enemies
                photonView.RPC("StopRound", RpcTarget.All);
            }
        }
            
    }

    #endregion

    #region Rpc functions, this functions are called for the functions with the same name but whitout rpc. the rpc will make this function be called on every player

    //this will be called by the AiEnemyInformation script, when its ai dies
    [PunRPC]
    public void enemyDiedRPC(int spaceOccupiedByEnemy)
    {
        //let the round system know that it lost an enemy who ocupied this space
        currentnumberOfEnemiesInScene -= spaceOccupiedByEnemy;
        
        //if we are the master client and already stoped spawning, and all enemys are dead, start the next round 
        if (amIMasterClient && spawnedEveryEnemyInThisRound && currentnumberOfEnemiesInScene <= 0)
        {
            //this will call the function Next round, on every pc on the server, since the round finished
            photonView.RPC("NextRound", RpcTarget.All);
        }
    }

    //this is called by the ai spawner, and will let this script keep track of how many enemies there are on the scene
    [PunRPC]
    public void enemySpawnedRPC(int spaceOccupiedByEnemy, int enemyPhotonId)
    {
        //get the enemy spawned, by finding its id
        GameObject enemySpawned = PhotonView.Find(enemyPhotonId).gameObject;
        //tell the enemy spawned ai enemy information script, about this script, so that it can call the function death after
        enemySpawned.GetComponent<AiEnemyInformation>().GameManagerRoundSystem = this;

        //tell the code that the number of enemies increased by the space occupied by the enemy spawned
        currentnumberOfEnemiesInScene += spaceOccupiedByEnemy;
        
    }

    [PunRPC]
    //when we stoped spawning enemies we call this function, the round can still be going on, but we wont spawn anymore
    void StopRound()
    {
        //this will tell the ai spawner script to stop spawning
        aiSpawnerScript.stopSpawning = true;

        //tell the code that we already spawned every enemy
        spawnedEveryEnemyInThisRound = true;
    }

    [PunRPC]
    //when the round has finished, we call this function to start the next round
    void NextRound()
    {

        currentRound += 1;
        roundText.text = currentRound.ToString();
    }
    #endregion

    private void Update()
    {
        Debug.Log(enemiesSpawnedThisRound);
    }
}
