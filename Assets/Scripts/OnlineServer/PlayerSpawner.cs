using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSpawner : MonoBehaviourPun
{
    //this will store wich player this script will spawn
    [SerializeField] private GameObject playerPrefab = null;
    //this will get access to the free look cinemachine camera component, so that we can say to it wich player to follow on, the void start
    [SerializeField] private CameraFollow playerCameraFollow = null;

    [SerializeField]
    //this will store the player money text to be able to send it to the player gameobject
    TextMeshProUGUI playerMoneyText;

    private void Start()
    {
        //Spawn "online" the player prefab, and assign the photon view to us, with the instatiate keyword this object will be assigned to us, and we are the ones who can move it
        //if we do the if photonView.IsMine, and will spawn it on 000 positionm and 000 rotation
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);

        //asssign the money textbox to the player statas and functionalities script
        player.GetComponent<PlayerStatsAndFunctionalities>().moneyText = playerMoneyText;

        //this will call the function StoreAllPlayersInScene, on every pc on the server
        photonView.RPC("StoreAllPlayersInScene", RpcTarget.All, player.GetComponent<PhotonView>().ViewID);

        //this will make the camera follow the player
        playerCameraFollow.objectToFollow = player;
    }

    [PunRPC]
    //this function will tell the ai spawner (script that only runs on the masterclient) which targerts it has (players playing)
    void StoreAllPlayersInScene(int playerId)
    {
        //get the player spawned, by finding its id
        GameObject playerSpawned = PhotonView.Find(playerId).gameObject;

        //if we have the ai spawner (in this case if we are the master client), tell the ai spawner about this player
        if(this.gameObject.GetComponent<AiSpawner>())
            this.gameObject.GetComponent<AiSpawner>().targets.Add(playerSpawned.transform);

        
    }
}
