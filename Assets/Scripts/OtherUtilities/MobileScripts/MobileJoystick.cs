using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileJoystick : MonoBehaviour
{
    //this will store the initial posityion of the joystick, so it returns to its position when we dont click the screen
    private Vector3 initPosOfJoystick;
    //this will store the transform of joystick
    public RectTransform joystick;

    [SerializeField]
    //this will store the canvas 
    Canvas canvas;

    [SerializeField]
    //this will store the limit of the movement of the joystick
    float limitOfJoystick;

    private void Start()
    {
        //get the initial position of the joystick
        initPosOfJoystick = joystick.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if the mouse is clicking
        if(Input.GetMouseButton(0))
        {
            //this will store the movepos of the mouse
            Vector2 movePos;
            //get the mouse position on the canvas
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition, canvas.worldCamera,
                out movePos);

            

            //if the distance between the mouse and the 
            if (Vector3.Distance(canvas.transform.TransformPoint(movePos), initPosOfJoystick) <limitOfJoystick)
                //move the joystick to where the mouse is 
                joystick.position = canvas.transform.TransformPoint(movePos); 
            //if we are not inside the joystick space, we need to move the joystick to where the mouse is pointing within the limit of the joystick movement
            else
            {
                //this will get the offset from the mouse position and the initial joystick position
                Vector3 offset = canvas.transform.TransformPoint(movePos) - initPosOfJoystick;
                //this will move the mouse within the limitis of the joystick space
                joystick.position = initPosOfJoystick + Vector3.ClampMagnitude(offset, limitOfJoystick);
            }
                
        }
        //if we are not clicking the mouse / touchscreen e move the joystick to its initial position
        else
            //move the joystick to its init pos
            joystick.position = initPosOfJoystick;  
        
    }
   
}
