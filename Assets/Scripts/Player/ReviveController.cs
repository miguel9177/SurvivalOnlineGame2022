using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ReviveController : MonoBehaviour
{
    [SerializeField]
    //this will store the amount of time to be revived
    float reviveTime;

    [SerializeField]
    //this will be assign by the ui manager
    public Button reviveButton;    

    //this will be true when someone is reviving this dead player
    bool beingRevived = false;

    [SerializeField]
    //this will get the player stats and functionalities of the dead player (us)
    PlayerStatsAndFunctionalities playerStatsAndFunctionalities;

    //this will get the player stats and functionalities of the player reviving (us)
    PlayerStatsAndFunctionalities revivingPlayerStatsAndFunc;

    private void Start()
    {
        //call the courotine that is going to make this script start after everyone else 
        StartCoroutine(LateStart());
        reviveButton.gameObject.SetActive(false);
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);
        
        

        //this will disable the gameobject
        this.gameObject.SetActive(false);
    }

    //if theres some object inside the trigger
    private void OnTriggerEnter2D(Collider2D col)
    {
        //if this player isnt being revived
        if (beingRevived == false)
        {
           
            //this will get the player stats script from the object inside
            PlayerStatsAndFunctionalities colPlayerStatsFunct = col.gameObject.GetComponent<PlayerStatsAndFunctionalities>();

            //if the object that entered the trigger is a player and is mine and is not dead
            if (colPlayerStatsFunct != null && col.gameObject.GetComponent<PhotonView>().IsMine && colPlayerStatsFunct.amIDead == false)
            {
                //enable the revive button
                reviveButton.gameObject.SetActive(true);

                //this will get player stats script from the player that is going to revive us
                revivingPlayerStatsAndFunc = col.gameObject.GetComponent<PlayerStatsAndFunctionalities>();
            }
        }
    }

    //if theres some object that left the trigger
    private void OnTriggerExit2D(Collider2D col)
    {
        //if this player isnt being revived
        if (beingRevived == false)
        {
            
            //this will get the player stats script from the object inside
            PlayerStatsAndFunctionalities colPlayerStatsFunct = col.gameObject.GetComponent<PlayerStatsAndFunctionalities>();

            //if the object that entered the trigger is a player and is mine and is not dead
            if (colPlayerStatsFunct != null && col.gameObject.GetComponent<PhotonView>().IsMine && colPlayerStatsFunct.amIDead == false)
            {
                //enable the revive button
                reviveButton.gameObject.SetActive(false);

                //this will get player stats script from the player that is going to revive us
                revivingPlayerStatsAndFunc = null;
            }
        }
    }

    //this will start reviving the player and will be called by the revive button
    public void StartReviving()
    {
        //tell the code that we are reviving the player
        beingRevived = true;
        //call the coroutine that is going to revive the player 
        StartCoroutine(Reviving());
    }

    //this will start reviving the player
    IEnumerator Reviving()
    {
        Debug.Log("REVIVING");
        //block the player reviving us input, so that he doesnt move
        revivingPlayerStatsAndFunc.gameObject.GetComponent<CharacterInput>().blockPlayerInput = true;
        //this will wait a few seconds while we are reviving
        yield return new WaitForSeconds(reviveTime);

        //if the player reviving us is not dead
        if(revivingPlayerStatsAndFunc.amIDead == false)
        {
            //block the player reviving us input, so that he doesnt move
            revivingPlayerStatsAndFunc.gameObject.GetComponent<CharacterInput>().blockPlayerInput = false;
            //call the revive function on every pc
            playerStatsAndFunctionalities.CallReviveRpc();

            //this will hide the button since, it finished the reviving
            reviveButton.gameObject.SetActive(false);

            //tell the code that we are not being revived anymore
            beingRevived = false;
            //disable this component
            this.enabled = false;

        }
        else
        {
            //this will hide the button since, it finished the reviving
            reviveButton.gameObject.SetActive(false);
            //tell the code that we are not being revived since the player reviving is dead
            beingRevived =false;
        }
    }
}
