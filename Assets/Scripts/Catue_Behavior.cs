using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Works very similarily to NPC where they idly move around the map until they sense Reedy nearby
//Once Reedy is inside the box collider for a decent amount of time, Catue will pursue them and push them into a corner
//Reedy is forced into long rambling dialogue until Catue is satisfied
public class Catue_Behavior : MonoBehaviour
{
    public Transform targetToChase;
    public float chaseCountDown;

    private float decidingTimer; //Time it takes for NPC to make a new decision/carry out current one
    private int decidedAction;
    public bool idleWalking;
    public float idleSpeed;
    public float chasingSpeed;

    // Start is called before the first frame update
    void Start()
    {
        targetToChase = null;
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            //Debug.Log("Collission"); //Check if colliding

            decidedAction = 1; //Stops NPC in their tracks
            decidingTimer = 200f; //reset timer so no decision is made during this process

            this.transform.Rotate(new Vector3(0, -180f, 0) * idleSpeed); //spin them 180 from where they currently are
            decidedAction = 5; //Set action to 5 (Slowly backing away)
        }
    }


    public void OnTriggerStay(Collider other)
    {
        if (chaseCountDown >= 0)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                chaseCountDown--;
                targetToChase = other.transform;
                Debug.Log("See's Player");

            }
        }
        else
        {

            idleWalking = false;
            decidedAction = 6;

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            chaseCountDown = 500f;
            idleWalking = true;
            Debug.Log("Lost Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Making a new decision
        if (decidingTimer <= 0)
        {
            decidedAction = Random.Range(1, 4);
            decidingTimer = 200f;

        }
        //Only if idleWalking box is ticked to true
        if (idleWalking == true)
        {
            decidingTimer--;
            //Use Random_Range to generate a random number every few seconds. Numbers determine if npc rotates, walks or pauses.
            if (decidedAction == 1)
            {
                //Stand
                this.transform.Translate(new Vector3(0, 0, 0) * Time.deltaTime);

            }
            else if (decidedAction == 2)
            {
                //Walk
                this.transform.Translate(Vector3.forward * Time.deltaTime * idleSpeed);

            }
            else if (decidedAction == 3)
            {
                //Rotate left
                this.transform.Rotate(new Vector3(0, -15f, 0) * Time.deltaTime * idleSpeed);

            }
            else if (decidedAction == 4)
            {
                //Rotate right
                this.transform.Rotate(new Vector3(0, 15f, 0) * Time.deltaTime * idleSpeed);

            }
            else if (decidedAction == 5)
            {
                this.transform.Translate(Vector3.forward * Time.deltaTime * 1.5f);
            }
        }
    }
}

 

