using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoundSystem : MonoBehaviourPun
{
    //this will store how many ai enemies are on the scene
    private int currentnumberOfEnemiesInScene;

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

    private void Start()
    {
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
            //this will call the function enemyDiedRPC, on every pc on the server
            photonView.RPC("enemyDiedRPC", RpcTarget.All, spaceOccupiedByEnemy);
    }

    //this is called by the ai spawner, and will let this script keep track of how many enemies there are on the scene
    public void enemySpawned(int spaceOccupiedByEnemy, int enemyPhotonId)
    {

        if (amIMasterClient)
            //this will call the function enemyDiedRPC, on every pc on the server
            photonView.RPC("enemySpawnedRPC", RpcTarget.All, spaceOccupiedByEnemy, enemyPhotonId);
    }

    #endregion

    #region Rpc functions, this functions are called for the functions with the same name but whitout rpc. the rpc will make this function be called on every player

    //this will be called by the AiEnemyInformation script, when its ai dies
    [PunRPC]
    public void enemyDiedRPC(int spaceOccupiedByEnemy)
    {
        //let the round system know that it lost an enemy who ocupied this space
        currentnumberOfEnemiesInScene -= spaceOccupiedByEnemy;
    }

    //this is called by the ai spawner, and will let this script keep track of how many enemies there are on the scene
    [PunRPC]
    public void enemySpawnedRPC(int spaceOccupiedByEnemy, int enemyPhotonId)
    {
        //get the player spawned, by finding its id
        GameObject enemySpawned = PhotonView.Find(enemyPhotonId).gameObject;
        enemySpawned.GetComponent<AiEnemyInformation>().GameManagerRoundSystem = this;

        //tell the code that the number of enemies increased by the space occupied by the enemy spawned
        currentnumberOfEnemiesInScene += spaceOccupiedByEnemy;
    }

    #endregion

    private void Update()
    {
        Debug.LogWarning(currentnumberOfEnemiesInScene);
    }
}
