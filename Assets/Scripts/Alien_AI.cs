using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Random;
using static UnityEngine.Debug;
using System.Security.Cryptography;

public class Alien_AI : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;

    public Transform player;
    public Rigidbody rbPlayer;

    public LayerMask whatIsGround, whatIsPlayer, whatAreWalls;

    //Patrol Mode
    private Vector3 walkPoint; //set point
    bool walkPointSet; //check if point is set
    public float walkPointRange; //keep track of distance till point

    //Attack Mode
    public float timeBetweenAttacks, timeBetweenChase;
    bool alreadyAttacked;

    public float sightRange, attackRange, autoWalkPointReset;
    private bool playerInSightRange, playerInAttackRange;

    //private float timeGuess;
    private Vector3 guessLocation;

    public GameManager gm;

    //public Hiding_Behavior hidingB;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        rbPlayer = GameObject.Find("Player").GetComponent<Rigidbody>();
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //timeGuess = 1000f;
        alreadyAttacked = false;
        agent.speed = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer); //Change to dif raycast
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


         if (gm.isHiding == false)
         {
                if (!playerInSightRange && !playerInAttackRange)
                {
                    if(gm.isStill == false)
                    {
                    Patroling();
                    agent.speed = 7f;
                }
                    else
                    {
                    Patroling();
                    agent.speed = 0.5f;
                    }
                }

                if (playerInSightRange && !playerInAttackRange)
                {
                     if (gm.isStill == false)
                    {
                    ChasePlayer();
                    agent.speed = 9f;
                }
                    else
                    {
                    ChasePlayer();
                    agent.speed = 0.5f;
                    }
                }

                if (playerInAttackRange && playerInSightRange)
                {
                    AttackPlayer();
                }
            }
            else
            {
                Patroling();
                agent.speed = 7f;
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

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);


        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }

    }

    private void ChasePlayer()
    {
        //Log("Is chasing.");
        //var position = new Vector3(player.position.x + Range(-15f, 15f), player.position.y, player.position.z + Range(-15f, 15f));
        agent.SetDestination(player.position);
        //agent.speed = 50f;
        //timeGuess = Range(800f, 1200f);

    }

    private void AttackPlayer()
    {

       // agent.SetDestination(transform.position);


        if (alreadyAttacked == false)
        {
            gm.playerHealth--;
            //player.transform.position = new Vector3(player.position.x, player.position.y, player.position.z);
            agent.SetDestination(transform.position);
            Log("Attacked");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }



    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        agent.speed = 20f;
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }*/

}
