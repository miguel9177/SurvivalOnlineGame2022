using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AiEnemy
{
    //initialize every variable necessary for a AiEnemy
    public float speed;
    public float damage;
    public float hp;
    public float moneyWorth;
    //just like in clash of clans, every enemy ocupies a certain space, so that if we have stronger enemys they occupy more space
    public int spaceOccupied;
    //i need max hp, to be able to edit the slider of the enemy ai hp, since we need to transform it to a 0-1 value
    public float maxHp;
}

public class AiEnemyInformation : MonoBehaviour
{
    [HideInInspector]
    //this will store the round system script, to be able to let him know that a enemy died
    public RoundSystem GameManagerRoundSystem;

    [SerializeField]
    //this will store the settings of this enemy
    public AiEnemy thisEnemy;

    //this wil be called by the aiEnemy scrirpt (for example the normalAiEnemy script) from this gameobject, when the ai dies
    public void Death()
    {
        //this will let the round system know that this enemy died, and will tell him how much space it occupies (like in clash of clans)
        GameManagerRoundSystem.enemyDied(thisEnemy.spaceOccupied);
    }
}
