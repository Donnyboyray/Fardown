using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_AI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float setY;

    //Patrol Mode
    public Vector3 walkPoint; //set point
    bool walkPointSet; //check if point is set
    public float walkPointRange; //keep track of distance till point

    //Attack Mode
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public Player_Input playerInput;

    //public Hiding_Behavior hidingB;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        playerInput = GameObject.Find("Player").GetComponent<Player_Input>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


        if (playerInput.isHiding == false)
        {

            if (!playerInSightRange && !playerInAttackRange)
            {
                Patroling();
            }

            if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer();
            }

            if (playerInAttackRange && playerInSightRange)
            {
                AttackPlayer();
            }
        }
        else
        {
            Patroling();
        }
    }

    public void Patroling()
    {
        if (!walkPointSet)
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

        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y , transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }

    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {

        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //add specific attack code here
            Debug.Log("Attacked");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
