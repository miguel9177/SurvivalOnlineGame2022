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
    public int kills;
    public float money;
    public int revives;
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

    //this will be called when we lose hp, for example, it will be called from the normal ai script
    public void RemoveHp(int HpToRemove)
    {
        //if this is my player
        if (photonView.IsMine)
        {
            playerStats.hp -= HpToRemove;
            txtPlayerHp.text = "Hp: " + playerStats.hp;
        }
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
