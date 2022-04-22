using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAiMovement : MonoBehaviour
{
    [SerializeField]
    Transform target;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        //MANDATORY CODE FOR EVERY AI ENTETY, THIS IS TO FIX A BUG FROM THE PACKAGE
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        //MIGUEL CHANGE THIS TO NOT BE UPDATE    
        agent.SetDestination(target.position);
    }
}
