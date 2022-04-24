using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UiManager : MonoBehaviour
{
    [SerializeField]
    //this will store the text for the bullets
    public TextMeshProUGUI txtBulletsOfWeapon;
    
    [HideInInspector]
    //this will store the player gameobject, so that i can access every script from here
    public GameObject player;
   
    

    private void Start()
    {
        //call the courotine that is going to make this script start after everyone else 
        StartCoroutine(LateStart());
       
    }

    
    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);
        //this will assign the textbox to the gun
        player.GetComponent<CharacterWeapon>().weapon.GetComponent<GunInformation>().txtBulletsOfWeapon = txtBulletsOfWeapon;
    }

    
}
