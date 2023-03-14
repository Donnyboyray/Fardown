using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Random;
using static UnityEngine.Debug;
using System.Security.Cryptography;

public class Enemy_AI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer, whatAreWalls;

    //Patrol Mode
    private Vector3 walkPoint; //set point
    bool walkPointSet; //check if point is set
    public float walkPointRange; //keep track of distance till point

    //Attack Mode
    public float timeBetweenAttacks, timeEscapeReset;
    bool alreadyAttacked, currentlyAttacking;

    public float sightRange, attackRange, autoWalkPointReset;
    private bool playerInSightRange, playerInAttackRange;

    private float timeGuess;
    private Vector3 guessLocation;

    public Player_Input playerInput;

    //public Hiding_Behavior hidingB;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        playerInput = GameObject.Find("Player").GetComponent<Player_Input>();
        agent = GetComponent<NavMeshAgent>();
        timeGuess = 500f;
    }

    void Update()
    {
        //OnDrawGizmosSelected();
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer); //Change to dif raycast
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInput.isHiding == false)
        {

            if (!playerInSightRange && !playerInAttackRange)
            {
                Patroling();
            }

            if (playerInSightRange && !playerInAttackRange)
            {
                timeGuess--;
                Log(timeGuess);

                if(timeGuess <= 0)
                {
                    LocatePlayer();
                }
            }

            if (playerInAttackRange && playerInSightRange)
            {
                ChasePlayer();
            }
        }
        else
        {
            //Debug.Log("Lost Sight of Player");
            Patroling();
        }
    }

    public void Patroling()
    {
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

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y , transform.position.z + randomZ);


        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }

    }

    private void LocatePlayer()
    {
        Log("Is chasing.");
        var position = new Vector3(player.position.x + Range(-5f, 5f), player.position.y, player.position.z + Range(-5f, 5f));
        agent.SetDestination(position);
        timeGuess = Range(100f, 500f);

    }

    private void ChasePlayer()
    {
        if (currentlyAttacking == false)
        {
            agent.SetDestination(player.position);
            //transform.LookAt(player);
        }
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            //add specific attack code here
            Log("Attacked");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        currentlyAttacking = false;
    }

   private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Log("Collided");
            currentlyAttacking = true;
            AttackPlayer();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
