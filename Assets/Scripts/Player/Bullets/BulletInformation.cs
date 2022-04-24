using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is going to store the information of the bullet, either if its a normal bullet or another type, like this i can access its values from here
public class BulletInformation : MonoBehaviour
{
    [HideInInspector]
    public float damage;

    [HideInInspector]
    //this will store the player that shot this bullet, so that we can give money if we kill a ai
    public PlayerStatsAndFunctionalities playerThatShotMe;

}
