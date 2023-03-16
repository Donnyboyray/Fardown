using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//How NPCS walk around idly and stop and turn towards you as you approach
//idle walking mode can be toggled in inspector
//all NPCS w/ this script will stop moving and focus on player OnCollision (Different from collider that detects dialogue)
public class NPC_Movement : MonoBehaviour
{
    public float npcSpeed;
    public bool idleWalking;
    public bool setToWalk;
    public Transform target;
    private float decidingTimer; //Time it takes for NPC to make a new decision/carry out current one
    private int decidedAction; //Each action has a number from 1 to 4 that's decided randomly
   // private float wallTimer; //Pause at a wall instead of accidentally going through it
    // Start is called before the first frame update
    Quaternion originalRotation;
    void Start()
    {
        decidingTimer = 300f;
        //wallTimer = 10f;
        decidedAction = 1;
        Debug.Log(decidedAction);
        target = GameObject.Find("Player").transform;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Wall"))
        {
            //Debug.Log("Collission"); //Check if colliding

            decidedAction = 1; //Stops NPC in their tracks
            decidingTimer = 200f; //reset timer so no decision is made during this process

            this.transform.Rotate(new Vector3(0, -180f, 0) * npcSpeed); //spin them 180 from where they currently are
            decidedAction = 5; //Set action to 5 (Slowly backing away)
        }
        if (other.gameObject.CompareTag("Player"))
        {
            originalRotation = transform.rotation;
            idleWalking = false;
        }

    }

    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Vector3 relativePos = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);

            Quaternion current = transform.localRotation;

            transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * npcSpeed);
        }
    }

    //Determines whether to turn idleWalking back on or not
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Very chunky currently
            //Try to smooth out the snap a bit
            transform.rotation = originalRotation; //Sets NPC back to their original rotation before speaking 


            if (setToWalk == true)
            {
                idleWalking = true;
            }
            else
            {
                idleWalking = false;
            }
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
           // Debug.Log("New action is..." + decidedAction);

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
                this.transform.Translate(Vector3.forward * Time.deltaTime * npcSpeed);
    
            }
            else if (decidedAction == 3)
            {
                //Rotate left
                this.transform.Rotate(new Vector3(0, -15f, 0) * Time.deltaTime * npcSpeed);

            }
            else if (decidedAction == 4)
            {
                //Rotate right
                this.transform.Rotate(new Vector3(0, 15f, 0) * Time.deltaTime * npcSpeed);
    
            }
            else if (decidedAction == 5)
            {
                this.transform.Translate(Vector3.forward * Time.deltaTime * 1.5f);
            }
        }
    }
}
