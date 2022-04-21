using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script will store the gun information, with this i can have multiple types of weapons and just sedn the information here
public class GunInformation : MonoBehaviour
{
    //this will store the function to shoot, i use this so that i dont have to know wich type of weapon the user is using 
    public delegate void ShootFunctionToCall();
    public ShootFunctionToCall firingMethod;
}
