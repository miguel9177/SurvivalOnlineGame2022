using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterInput : MonoBehaviourPun
{
    //this will store the characters rigidbody
    private Rigidbody2D rigidBody;

    [SerializeField]
    //this will store hopw fast the user is moving
    private float speed;

    //this will store in what direction the user is moving (it goes from 0 to 1)
    Vector2 movement;

    [SerializeField]
    //this will hold the player animator (on the gameobject that holds the sprites and the animator)
    private Animator playerAnimator;

    [SerializeField]
    //this will hold the player parameter to walk from the animator
    private string walkParameterFloatName;

    [HideInInspector]
    //this bool will block the player movement, for example, when inside menus, or when death, it will be changed by scripts outside, like player stats and functionalities
    public bool blockPlayerInput = false;

    //this will be true if we are running the game on android
    private bool android = false;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        //this will check if we are on android if we are, tell the code that
        if (Application.platform == RuntimePlatform.Android)
        {
            android = true;
        }
        
    }

    // Update is called once per frame
    private void Update()
    {
        //if this is my photon view (if this is my character and not any other players character), and im not blocking the player input call the function that thakes input
        if (photonView.IsMine && blockPlayerInput == false)
        {
            //if we are on android
            if(android)
            {
                //this will take the input if on android 
                TakeInputIfAndroid();
            }
            //if we are on windows
            else
            {
                //call the function that gets the input and moves the object
                TakeInputIfWindows();
            }
        }
        else if(blockPlayerInput == true)
        {
            //if the input is blocked, we stop the player movement
            movement = Vector2.zero;
        }
        
    }

    
    void FixedUpdate()
    {
        //i use velocity, to instantly move without acceleration
        rigidBody.velocity = movement * speed;
      
    }

    //this function runs only on my pc, not on the other clients, if we are on windows port
    void TakeInputIfWindows()
    {
        //create a vector that is going to get the movement of the character by getting the values from the arrow keys
        movement = new Vector2
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical")
        }.normalized;

        //this will get if the player is moving on a float, so that i can pass it to the animator
        float totalMovement = Mathf.Abs(movement.x)+Mathf.Abs(movement.y);
        //send the movement value to the animator
        playerAnimator.SetFloat(walkParameterFloatName, totalMovement);    

        //this will rotate the player towards the mouse position
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        //does the tangent of the y and x to get the z angle
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //rotate the object
        transform.rotation = Quaternion.Euler(0f, 0f, rotation_z);
    }

    //this function will run on android if we are android
    void TakeInputIfAndroid()
    {
        //this will get if the player is moving on a float, so that i can pass it to the animator
        float totalMovement = Mathf.Abs(movement.x) + Mathf.Abs(movement.y);
        //send the movement value to the animator
        playerAnimator.SetFloat(walkParameterFloatName, totalMovement);

        //this will rotate the player towards the mouse position
        /*Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        //does the tangent of the y and x to get the z angle
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //rotate the object
        transform.rotation = Quaternion.Euler(0f, 0f, rotation_z);*/
    }

   

    //this will receive the movement from the ui manager if we are on android because of the joystick
    public void receiveMovementFromUiManager(Vector2 movement_)
    {
        Debug.Log("IM CALLED");
        //store the movement from the joystick
        movement = movement_;
    }
}
