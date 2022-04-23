using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiEnemyInformation : MonoBehaviour
{
    [HideInInspector]
    //this will store the round system script, to be able to let him know that a enemy died
    public RoundSystem GameManagerRoundSystem;

    [HideInInspector]
    //this will store how much space is occupied by this ai
    public int spaceOccupiedByAi;

    //this wil be called by the aiEnemy scrirpt (for example the normalAiEnemy script) from this gameobject, when the ai dies
    public void Death()
    {
        //this will let the round system know that this enemy died, and will tell him how much space it occupies (like in clash of clans)
        GameManagerRoundSystem.enemyDied(spaceOccupiedByAi);
    }
}
