using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//THIS SCRIPT ONLY RUNS ON THE MASTER CLIENT, SINCE WE HAVE ADDED THE IF IS MASTER CLIENT ON THE START VOID 
public class AiSpawner : MonoBehaviourPun
{
    #region variables to handle spawning
    [SerializeField]
    [Header("store every spawn point")]
    //this is going to store all of the spawn points
    Transform[] spawnPoints;
    //this is going to store the current index of the last unlocked spawn point, for example, if the index is 4, we spawn enemies on 1 2 3 and 4 spawn points
    private int indexOfCurrentSpawnPoint = 0;

    [SerializeField]
    [Header("Store how many spawn points it increases per door unlocked")]
    //this is going to store how many spawn points to increase when we open a door
    int[] spawnPointsToIncreasePerDoorUnlocked;
    
    //this is going to store how many doors we have unlocked
    private int indexOfDoorsUnlocked = 0;

    //this is going to store the amount of time to spawn a normal enemy
    [SerializeField]
    float timeToSpawnNormalEnemys;
    //this is going to store the amount of time to spawn a special enemy
    [SerializeField]
    float timeToSpawnSpecialEnemys;

    [SerializeField]
    //this will stop the spawning if its true
    public bool stopSpawning = false;
    #endregion

    #region variables to handle enemys
    [HideInInspector]
    //this will store every player on the scene
    public List<Transform> targets = new List<Transform>();

    [SerializeField]
    //this will store the parent gameobject of all normal enemies (for organizational purposes)
    GameObject parentOfNormalEnemies;

    [SerializeField]
    //this will store the parent gameobject of all special enemies (for organizational purposes)
    GameObject parentOfSpecialEnemies;

    [SerializeField]
    //this is going to store every normal enemy (spanws regurly)
    GameObject[] normalEnemys;

    [SerializeField]
    //this is going to store every special enemy (spanws rarely)
    GameObject[] specialEnemys;
    #endregion

    #region variables to comunicate with the gamemanager
    //this will store the round system script to be able to tell him when a enemy spawns
    RoundSystem roundSystemScript;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //if we are not the master client remove this script since we only want to handle the spawning of the ai in one pc, and the rest receive the enemy by photon view
        if (!PhotonNetwork.IsMasterClient)
        {
            //remove this script since we are not the master client
            Destroy(this);
        }

        //set the number of spawn points on the begining of the scene
        indexOfCurrentSpawnPoint += spawnPointsToIncreasePerDoorUnlocked[indexOfDoorsUnlocked];

        //this will store the round system script
        roundSystemScript = this.gameObject.GetComponent<RoundSystem>();

        //call both courootines that spawn enemys
        StartCoroutine(SpawnNormalAiEnemy());
        StartCoroutine(SpawnSpecialAiEnemy());
    }

    //this is going to spawn a enemy on a random unlocked spawn point
    IEnumerator SpawnNormalAiEnemy()
    {
        //wait a time before running the code below
        yield return new WaitForSeconds(timeToSpawnNormalEnemys);

        //get a random number between 0 and the current available last spawn point
        int indexOfSpawnPointToSpawn = Random.Range(0, indexOfCurrentSpawnPoint);

        //get a normal enemy to spawn
        int enemyToSpawn = Random.Range(0, normalEnemys.Length);

        //spawn a normal enemy at the spawn position
        GameObject enemySpawned = PhotonNetwork.Instantiate(normalEnemys[enemyToSpawn].name, spawnPoints[indexOfSpawnPointToSpawn].position, Quaternion.identity);
        //make the enemy spawn a child of the parent of all normal enemies
        enemySpawned.transform.parent = parentOfNormalEnemies.transform;
        //tell this ai what targets it has to follow (all players in scene)
        enemySpawned.GetComponent<AiMovePathFind>().targetsOfAi = targets;
        //increase the hp of this enemy depending on the round
        enemySpawned.GetComponent<AiEnemyInformation>().thisEnemy.hp += roundSystemScript.hpToIncrease;

        //tell the round system script that a player spawned, and tell him how much space it occupies
        roundSystemScript.enemySpawned(enemySpawned.GetComponent<AiEnemyInformation>().thisEnemy.spaceOccupied, enemySpawned.GetComponent<PhotonView>().ViewID);
       
        //if the stop animation bool is false, stop spawning enemies
        if (stopSpawning==false)
            //recall this function so that it always spawns enemys
            StartCoroutine(SpawnNormalAiEnemy());
    }

    //this is going to store a enemy on a random spawn point
    IEnumerator SpawnSpecialAiEnemy()
    {
        //wait a time before running the code below
        yield return new WaitForSeconds(timeToSpawnSpecialEnemys);

        //get a random number between 0 and the current available last spawn point
        int indexOfSpawnPointToSpawn = Random.Range(0, indexOfCurrentSpawnPoint);

        //get a special enemy to spawn
        int enemyToSpawn = Random.Range(0, specialEnemys.Length);

        //spawn a special enemy at the spawn position
        GameObject enemySpawned = PhotonNetwork.Instantiate(specialEnemys[enemyToSpawn].name, spawnPoints[indexOfSpawnPointToSpawn].position, Quaternion.identity);
        //make the enemy spawn a child of the parent of all special enemies
        enemySpawned.transform.parent = parentOfSpecialEnemies.transform;
        //tell this ai what targets it has to follow (all players in scene)
        enemySpawned.GetComponent<AiMovePathFind>().targetsOfAi = targets;
        //increase the hp of this enemy depending on the round
        enemySpawned.GetComponent<AiEnemyInformation>().thisEnemy.hp += roundSystemScript.hpToIncrease;

        //tell the round system script that a player spawned, and tell him how much space it occupies
        roundSystemScript.enemySpawned(enemySpawned.GetComponent<AiEnemyInformation>().thisEnemy.spaceOccupied, enemySpawned.GetComponent<PhotonView>().ViewID);


        //if the stop animation bool is false, stop spawning enemies
        if (stopSpawning == false)
            //recall this function so that it always spawns special enemys
            StartCoroutine(SpawnSpecialAiEnemy());
    }

    //if we unlock a door, we need to increase the spawn points
    public void OnDoorUnlocked()
    {
        //since we have unlocked a door, we increase the number of doors unlocked
        indexOfDoorsUnlocked += 1;
        //store the last available spawn point
        indexOfCurrentSpawnPoint += spawnPointsToIncreasePerDoorUnlocked[indexOfDoorsUnlocked];
    }

    //this will restart every courotine on this script, since they stop when the round ends
    public void RestartAllLoops()
    {
        //call both courootines that spawn enemys
        StartCoroutine(SpawnNormalAiEnemy());
        StartCoroutine(SpawnSpecialAiEnemy());
    }
}
