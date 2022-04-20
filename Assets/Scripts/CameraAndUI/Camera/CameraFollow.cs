using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [HideInInspector]
    //this value is set on the player spawner
    public GameObject objectToFollow;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        //make this camera follow the object
        this.transform.position = objectToFollow.transform.position;
    }
}
