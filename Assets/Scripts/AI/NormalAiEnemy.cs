using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
[System.Serializable]
struct AiEnemy
{
    //initialize every variable necessary for a AiEnemy
    public float speed;
    public float damage;
    public float hp;
    public float moneyWorth;
}

public class NormalAiEnemy : MonoBehaviourPun
{
    [SerializeField]
    TextMeshProUGUI tempHp;

    [SerializeField]
    //this will store the settings of this enemy
    AiEnemy thisEnemy;

    //this will check if the die function have already been called
    bool haveIHaverBeenCalled = false;

    //if collided with some collider
    void OnCollisionEnter2D(Collision2D col)
    {
        //this will checl of ythe bullet has a bullet information, if it has it will get damaged
        if(col.gameObject.GetComponent<BulletInformation>())
        {
            //this will decrease the ai hp
            thisEnemy.hp = thisEnemy.hp - col.gameObject.GetComponent<BulletInformation>().damage;
            
            //if the hp is less or equal to 0, call the function die
            if(thisEnemy.hp <= 0)
            {
                //if the fucntion die havent been called on this ai enemy, call the function die
                if(haveIHaverBeenCalled == false)
                    //this will call the function Die, on every pc on the server
                    photonView.RPC("Die", RpcTarget.All);

                //tell the code that the function die has already been called
                haveIHaverBeenCalled=true;
            }
        }
    }

    
    [PunRPC]
    void Die()
    {
        //destroy this object, since it died
        Destroy(this.gameObject);
    }

    private void Update()
    {
        tempHp.text = thisEnemy.hp.ToString();
    }
}
