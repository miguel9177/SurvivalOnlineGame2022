using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
public class AiMovePathFind : MonoBehaviourPun
{


    //this will store to where the ai needs to move
    [HideInInspector]
    public List<Transform> targetsOfAi = new List<Transform>();

    //this will store the index of the closest target from the array of targets
    private int indexOfCloserTarget;

    //this will the navmeshagent, in this case this one
    private NavMeshAgent agent;
    
    [SerializeField]
    //this will store how much time to wait between updating the pathfinding
    float timeToUpdatePathFind;
    //this will store the initial value of the timeToUpdatePathFind variable, since we are going to edit it when we are close to the player
    private float initialTimeToUpdatePathFind;

    //this will store a smaller value then timeToUpdatePathFind, so that we can update more times the navmesh when we are close
    [SerializeField]
    float timeToUpdatePathFindWhenClose;
    
    [Header("time it takes to check who of the targets is closer, this can probably be a high value")]
    [SerializeField]
    float timeToUpdateTarget = 5f;

    [Header("this will define the distance to start using the timeToUpdatePathFindWhenClose, instead of timeToUpdatePathFind")]
    [SerializeField]
    float distanceToStartPathFindingCloser;


    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Destroy(this);
            Destroy(this.GetComponent<NavMeshAgent>());
            return;
        }

        initialTimeToUpdatePathFind = timeToUpdatePathFind;

        //MANDATORY CODE FOR EVERY AI ENTETY, THIS IS TO FIX A BUG FROM THE PACKAGE
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        //call the couroutine that is going to update the target depending on who is closer
        StartCoroutine(UpdateTarget());

        //call the couroutine that is going to star the loop of updating the navmesh
        StartCoroutine(LoopUpdateNavMesh());

        StartCoroutine(CheckWhichTargetToFollow());

    }

    //this will do an infinite loop to update the target depending on who is closer
    IEnumerator UpdateTarget()
    {
        //wait a time before running the code below
        yield return new WaitForSeconds(timeToUpdateTarget);
        
        //this will create the variable that is going to be used on the for loop, and will store the current closest distance
        float closestDistance = 999999f;
        //do a loop trough all targets
        for (int i = 0; i < targetsOfAi.Count; i++)
        {
            //get the distance between this object and the current target
            float tempDistance = Vector2.Distance(this.transform.position, targetsOfAi[i].transform.position);
            //if the distance of the current target is smaller then the distance of the current smallest distance, store this distance instead
            if (tempDistance < closestDistance)
            {
                //store the current distance since its smaller
                closestDistance = tempDistance;
                //save the index of this target so that the navmehs agent can know wich target to follow
                indexOfCloserTarget = i;
            }
        }
        //recall this courotine since its supossed to be a infinite loop
        StartCoroutine(UpdateTarget());
    }


    //this will do an infinite loop by updating the navmesh
    IEnumerator LoopUpdateNavMesh()
    {
        //wait a time before running the code below
        yield return new WaitForSeconds(timeToUpdatePathFind);

        //this will update the navmesh agent 
        agent.SetDestination(targetsOfAi[indexOfCloserTarget].transform.position);
        
        //recall this courotine since its supossed to be a infinite loop
        StartCoroutine(LoopUpdateNavMesh());
    }

    IEnumerator CheckWhichTargetToFollow()
    {
        Debug.Log(indexOfCloserTarget + "   " + targetsOfAi.Count);
        //if we are close to the target, start updating the pathfinding faster
        if(Vector2.Distance(this.transform.position, targetsOfAi[indexOfCloserTarget].transform.position) < distanceToStartPathFindingCloser)
        {
            //make the timeToUpdatePathFind smaller, since we are closer, and with this the navmesh wil update more times
            timeToUpdatePathFind = timeToUpdatePathFindWhenClose;
        }
        else
        {
            //reset the timeToUpdatePathFind value since we are not close
            timeToUpdatePathFind = initialTimeToUpdatePathFind;
        }

        yield return new WaitForSeconds(0.5f);
        //recall this courotine since its supossed to be a infinite loop
        StartCoroutine(LoopUpdateNavMesh());
    }

    private void Update()
    {
       
    }
}
