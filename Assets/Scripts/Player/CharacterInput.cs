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



    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        //if this is my photon view (if this is my character and not any other players character), and im not blocking the player input call the function that thakes input
        if (photonView.IsMine && blockPlayerInput == false)
        {
            //call the function that gets the input and moves the object
            TakeInput();
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

    //this function runs only on my pc, not on the other clients
    void TakeInput()
    {
        //create a vector that is going to get the movement of the character by getting the values from the arrow keys
        movement = new Vector2
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical")
        }.normalized;

        //this will get if the player is moving on a float, so that i can pass it to the animator
        float totalMovement = movement.x+movement.y;
        //send the movement value to the animator
        playerAnimator.SetFloat(walkParameterFloatName, Mathf.Abs(totalMovement));    

        //this will rotate the player towards the mouse position
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        //does the tangent of the y and x to get the z angle
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //rotate the object
        transform.rotation = Quaternion.Euler(0f, 0f, rotation_z);
    }
}
