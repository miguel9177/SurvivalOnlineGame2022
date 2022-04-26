using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    //this will store the text for the bullets
    public TextMeshProUGUI txtBulletsOfWeapon;

    [SerializeField]
    //this will store the text informing the player is reloading
    public TextMeshProUGUI txtReloadInformation;

    [SerializeField]
    //this will store the text informing the player hp
    public TextMeshProUGUI txtPlayerHp;

    [HideInInspector]
    //this will store the player gameobject, so that i can access every script from here
    public GameObject player;

    [SerializeField]
    //this will hold the revive button, and will be used by the revive controller
    public Button reviveButton;

    private void Start()
    {
        //call the courotine that is going to make this script start after everyone else 
        StartCoroutine(LateStart());
        //make this text invisible since we only want to tell the user that we are reloading, when we are reloading
        txtReloadInformation.gameObject.SetActive(false);
    }

    
    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);
        //this will assign the textbox to the gun
        player.GetComponent<CharacterWeapon>().weapon.GetComponent<GunInformation>().txtBulletsOfWeapon = txtBulletsOfWeapon;
        player.GetComponent<CharacterWeapon>().weapon.GetComponent<GunInformation>().txtReloadInformation = txtReloadInformation;
        //this will assign the textbox to the player
        player.GetComponent<PlayerStatsAndFunctionalities>().txtPlayerHp = txtPlayerHp;
    }

   
    
}
