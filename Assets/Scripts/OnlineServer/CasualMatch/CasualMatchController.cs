using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CasualMatchController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private GameObject casualMatchPanel = null;
    [SerializeField]
    private GameObject waitingStatusPanel = null;
    [SerializeField]
    private TextMeshProUGUI waitingStatusText = null;

    [SerializeField]
    //this is going to store wich scene ti will join when finds an oponent
    string sceneToJoin;
        
    private bool isConnecting = false;

    //this will store in which version this game is, so that we dont matchmake with different versions
    private const string GameVersion = "0.1";
    //this will store the maximum allowed players in a single room
    private const int MaxPlayersPerRoom = 2;

    //this will make it that if a player changes scene everyone on the game changes scene aswell, so that if we are on a looby and the game starts we tell go to the scene, and every player will go to that scene
    private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

    //this is called from the button Find opponent on the FindOpoonent panel
    public void FindOpponent()
    {
        //tell the code that we are connecting
        isConnecting = true;

        //this will make the find opponent panel invisible, and then show the waiting status panel instead
        casualMatchPanel.SetActive(false);
        waitingStatusPanel.SetActive(true);

        //show the player that we are seacrhing for a game
        waitingStatusText.text = "Searching...";

        //if we are connected join a random room
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        //if we are not connected, connect to the photon network
        else
        {
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    //This is used when the client is connected to the master server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");

        //if we are connecting we join a random room
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    //if we desconnect from the client
    public override void OnDisconnected(DisconnectCause cause)
    {
        //hide the waiting status panel and show the findopponent panel
        waitingStatusPanel.SetActive(false);
        casualMatchPanel.SetActive(true);

        Debug.Log($"Disconnected due to: {cause}");
    }

    //if the joining failed,is because no one has a room created, so we will create a room ourselffs
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No clients are waiting for an opponent, creating a new room");

        //since no one has a created room we will create a room ourselfs, it will use the default room name, and on the room options we will leave everything to deafult, except the maximum players
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
    }

    //this will be called when the player has joined a room
    public override void OnJoinedRoom()
    {
        Debug.Log("Client successfully joined a room");
        //get the number of players in this room
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        //if the party is not full, say that we are waiting for an opponent
        if (playerCount != MaxPlayersPerRoom)
        {
            waitingStatusText.text = "Waiting For Opponent";
            Debug.Log("Client is waiting for an opponent");
        }
        //if the party is full, the match is ready to begin
        else
        {
            waitingStatusText.text = "Opponent found";
            Debug.Log("Match is ready to begin");
        }
    }

    //if another player entered the room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //if the current room is full
        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {
            //tell the photon network, that this room is not open
            PhotonNetwork.CurrentRoom.IsOpen = false;

            waitingStatusText.text = "Opponent found";
            Debug.Log("Match is ready to begin");

            //since the match is ready to begin, we load the "Scene_Main" level
            PhotonNetwork.LoadLevel(sceneToJoin);
        }
    }
}
