using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct AiEnemy
{
    //initialize every variable necessary for a AiEnemy
    public float speed;
    public float damage;
    public float hp;
    public float moneyWorth;
}

public class NormalAiEnemy : MonoBehaviour
{
    [SerializeField]
    //this will store the settings of this enemy
    AiEnemy thisEnemy;

    void OnCollisionEnter2D(Collision2D col)
    {
        //this will checl of ythe bullet has a bullet information, if it has it will get damaged
        if(col.gameObject.GetComponent<BulletInformation>())
        {
            thisEnemy.hp = thisEnemy.hp - col.gameObject.GetComponent<BulletInformation>().damage;
        }
    }
}
