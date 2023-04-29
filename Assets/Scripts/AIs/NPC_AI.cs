using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Random;
using static UnityEngine.Debug;

public class NPC_AI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform target;

    public LayerMask whatIsGround, whatIsPlayer, whatAreWalls;

    //Patrol Mode
    private Vector3 walkPoint; //set point
    bool walkPointSet; //check if point is set
    public float walkPointRange; //keep track of distance till point

    public float sightRange, autoWalkPointReset, lookSpeed;
    private bool playerInSightRange;
    public bool idleWalking, isTalking;

    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer); //Change to dif raycast

        if(isTalking == false )
        {
            if (!playerInSightRange && idleWalking)
            {
                Patroling();
                //isTalking = false; 
            }
            else if (playerInSightRange)
            {
                //agent.SetDestination(target.position);
                StopToTalk();
                //isTalking = true;
            }
        }
        else
        {
            StopToTalk();
        }
    }

    public void Patroling()
    {

        agent.speed = 2f;
        idleWalking = true;

        autoWalkPointReset--;
        //Make it so new walk point is set if current one is not met after a few seconds to prevent enemy from just freezing
        if (!walkPointSet || autoWalkPointReset <= 0)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {

        autoWalkPointReset = 1000f;

        float randomZ = Range(-walkPointRange, walkPointRange);
        float randomX = Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);


        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }

    }

    private void StopToTalk()
    {
        //Log("Stopped To Talk");
        float dist = Vector3.Distance(target.position, transform.position);

        if (dist <= 4f)
        {
            agent.SetDestination(transform.position);
            isTalking = true;
            idleWalking = false;
            Vector3 relativePos = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);

            Quaternion current = transform.localRotation;

            transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * lookSpeed);
        }
        else
        {
            StartCoroutine(PauseAfterTalk());
        }

    }

    private IEnumerator PauseAfterTalk()
    {
        Log("Paused after interaction");
        isTalking = false;
        idleWalking = true; //Set later on to be random
        yield return new WaitForSeconds(7);
    }
}
