using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

[System.Serializable]
//this sturct will store the player stats
public struct PlayerStats
{
    public int hp;
    public int maxHp;
    public int kills;
    public float money;
    public int revives;
    //this will store the icon image of the player (the image above the hpbar)
    public Sprite iconOfSkin;
}
public class PlayerStatsAndFunctionalities : MonoBehaviourPun
{
    [SerializeField]
    //create a variable for the player stats
    public PlayerStats playerStats;

    [HideInInspector]
    //this will store the textbox that is going to show the user in which round we are
    public TextMeshProUGUI moneyText;

    [HideInInspector]
    //this will store the text informing the player hp
    public TextMeshProUGUI txtPlayerHp;

    [SerializeField]
    //this will hold the player animator (on the gameobject that holds the sprites and the animator)
    public Animator playerAnimator;

    [SerializeField]
    //this will hold the player parameter to die from the animator
    private string deathParameterName;

    //this will hold the character input script, because when the player dies, i need to stop the script from taking input
    CharacterInput characterInputScript;

    //this will hold the player collider, since we need to deactivate the collider when the player dies
    Collider2D colliderOfPlayer;

    [SerializeField]
    //this will hold the revive controller script
    public ReviveController reviveControllerScript;

    //this will be true when this player dies
    public bool amIDead=false;

    //this will store the uiManager Script
    UiManager uiManagerScript;

    //this will store the index of this player
    int indexOfThisPlayer;

    private void Start()
    {
        //this will hold the character input script, because when the player dies, i need to stop the script from taking input
        characterInputScript = GetComponent<CharacterInput>();
        //this will get the players collider
        colliderOfPlayer = GetComponent<Collider2D>();

        //this will get the ui manager component
        uiManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UiManager>();
        //this will send this player stats and functionalities, to the ui manager so it can store all the players and their hp bars
        indexOfThisPlayer = uiManagerScript.PlayerSpawned(this);
    }

    //this is accesed from the buying stations
    public void UpdateMoneyText()
    {
        //i check this since this is added by the ui manager
        if(moneyText != null)
        moneyText.text = playerStats.money + "p";
    }

    //this will be called when we lose hp, for example, it will be called from the normal ai script
    public void RemoveHp(int HpToRemove)
    {
        //if this is my player
        if (photonView.IsMine)
        {
            playerStats.hp -= HpToRemove;
            txtPlayerHp.text = "Hp: " + playerStats.hp;
            //if the playter has died
            if(playerStats.hp<=0)
            {
                //this will call the function Die, on every pc on the server
                photonView.RPC("Die", RpcTarget.All);
            }
            //this will call the function updateHpOnline, on every pc on the server
            photonView.RPC("UpdateHpOnline", RpcTarget.All, playerStats.hp);
        }
    }

    [PunRPC]
    //this function will show the hp on every pc
    void UpdateHpOnline(int hpLeft)
    {
        //this will tell the ui manager to update the hp
       uiManagerScript.UpdateHpOfPlayer(indexOfThisPlayer, hpLeft);
    }

    [PunRPC]
    //this function will do the logic for dying, like blocking scripts from moving
    void Die()
    {
        //this will tell the animator that the player has died
        playerAnimator.SetBool(deathParameterName, true);
        //this will deactivate the collider of the player
        colliderOfPlayer.enabled=false;
        //block the player input, so he doesnt move and shoot
        characterInputScript.blockPlayerInput = true;
        //enable the revive gameobject
        reviveControllerScript.gameObject.SetActive(true);

        //tell the code that this player is dead
        amIDead=true;
    }

    
    //this will be called from the revive controller, and will call the revive on every pc
    public void CallReviveRpc()
    {
        //this will call the function Die, on every pc on the server
        photonView.RPC("ReviveRpc", RpcTarget.All);
    }

    //this will revive the player on every pc
    [PunRPC]
    void ReviveRpc()
    {
        //tell the code that im not dead
        amIDead = false;

        //this will restart the animator to make him go to the entry part
        playerAnimator.enabled=true;
        playerAnimator.Rebind();
        playerAnimator.Update(0f);

        //this will tell the animator that the player is not Dead
        playerAnimator.SetBool(deathParameterName, false);
        //this will activate the collider of the player
        colliderOfPlayer.enabled = true;
        //unblock the player input, so he moves and shoots
        characterInputScript.blockPlayerInput = false;
        //disable the revive gameobject
        reviveControllerScript.gameObject.SetActive(false);
        //restore the player hp
        playerStats.hp = 100;

        //if this is my photon view
        if(photonView.IsMine)
        {
            //update the hp text since we have max hp, because we have been revived
            txtPlayerHp.text = "Hp: " + playerStats.maxHp;
        }

        //this will call the function updateHpOnline, on every pc on the server
        photonView.RPC("UpdateHpOnline", RpcTarget.All, playerStats.hp);

    }

    //this will be called when an enemy is killed
    public void KilledEnemy(float moneyGained)
    {
        
        //if this is my player
        if (photonView.IsMine)
        {
            //update the money stat
            playerStats.money += moneyGained;
            //write the money the player has
            moneyText.text = playerStats.money + "p";
        }
    }
}
