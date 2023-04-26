using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Debug;
using static UnityEngine.Random;

public class Tahto_AI : MonoBehaviour
{
    public NavMeshAgent agent;

    public float radius;
    [Range(0,360)]
    public float angle;

    public GameObject playerRef;
    public Rigidbody rbPlayer;
    public Transform target;

    public LayerMask whatisPlayer, whatareWalls, whatIsGround;
    public bool canSeePlayer, canAttackPlayer;

    public GameManager gm;

    //Patrol Mode
    private Vector3 walkPoint; //set point
    bool walkPointSet; //check if point is set
    public float walkPointRange, autoWalkPointReset; //keep track of distance till point

    //Attack Mode
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //public float chaseMemory;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rbPlayer = GameObject.Find("Player").GetComponent<Rigidbody>();
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerRef = GameObject.Find("Player"); //Find Player
        target = null;
        StartCoroutine(FOVRoutine());
    }

    void Update()
    {

        if (canAttackPlayer == false)
        {
            if (canSeePlayer == true)
            {
                ChasePlayer();
            }
            /*else if (chaseMemory >= 0)
            {
                chaseMemory--;
                Log(chaseMemory);
                ChasePlayer();
                //Log("Is Patrolling");
            }*/
            else
            {
                Patroling();
            }
        }
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    public void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, whatisPlayer); //OverlapSphere returns array

        if (rangeChecks.Length != 0)
        {
            target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2) // divide by 2 to separate cone view in half for more accurate reading...?
            {
                float distanceToTarget = Vector3.Distance(this.transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, whatareWalls))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                    //Patroling();
                }
            }
            else
            {
                canSeePlayer = false;
                //Patroling();

            }
        }
        else if (canSeePlayer == true)
        {
            canSeePlayer = false;
            //Patroling();

        }
    }


    public void Patroling()
    {
        //Log("is Patrolling");
        gm.isAttracting = false;
        autoWalkPointReset--;
        //Make it so new walk point is set if current one is not met after a few seconds to prevent enemy from just freezing
        if (!walkPointSet || autoWalkPointReset <= 0)
        {
            SearchWalkPoint();
            //Log("Walkpoint Reset");
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            //Log("Go To Point");
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
            //Log("New Walkpoint Set");
            walkPointSet = true;
        }

    }
    private void ChasePlayer()
    {;
        agent.speed = 5f;
        agent.SetDestination(target.position);
        float dist = Vector3.Distance(target.position, transform.position);

        if (dist <= 2f)
        {
            canAttackPlayer = true;
            agent.SetDestination(transform.position);


            if (alreadyAttacked == false)
            {
                AttackPlayer();
            }
        }

    }

    private void AttackPlayer()
    {


        gm.playerHealth--;
        Log("Attacked");
        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);


    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        canAttackPlayer = false;
    }
}
