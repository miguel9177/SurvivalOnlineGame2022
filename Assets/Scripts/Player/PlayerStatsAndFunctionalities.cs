using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

[System.Serializable]
//this sturct will store the player stats
public struct PlayerStats
{
    public int kills;
    public float money;
    public int revives;
}
public class PlayerStatsAndFunctionalities : MonoBehaviourPun
{
    [SerializeField]
    //create a variable for the player stats
    PlayerStats playerStats;

    [HideInInspector]
    //this will store the textbox that is going to show the user in which round we are
    public TextMeshProUGUI moneyText;

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
