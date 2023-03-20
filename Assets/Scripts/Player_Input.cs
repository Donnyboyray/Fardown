using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEngine.Debug;

public class Player_Input : MonoBehaviour
{
    //SCRIPT REFS
    public GameManager gm; //Game manager in script
    //public Scene_Change sc;
    public Alien_AI aai;

    //MOVEMENT VARIABLES
    public float speed; //Walking
    public float runningSpeed;
    public float runningTimer, runningRecoup; //Running
    private float backupSpeed = 4f; //backing up 

    //RAYCAST
    Ray ray; //Ray that follows camera
    private float maxDistance = 5f; //Maximum limit of ray
    //private float maxView = 10f; //Max limit of ray just for Alien AI
    public LayerMask interactableLayers; //Set to Hidable, Collectable

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("gm").GetComponent<GameManager>();
        this.transform.position = gm.newScenePosition;
        Log(this.transform.position);
        gm.isHiding = false; //Set to false just in case
        gm.isStill = false;
        runningTimer = 500f;
        runningRecoup = 800f;
    }

    // Update is called once per frame
    void Update()
    {
        //Drawing Ray
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, -0.5f));
        //Inspector view of ray
        DrawRay(ray.origin, ray.direction * maxDistance, Color.cyan);

        //Checks for isHiding to be false
        //Movement is frozen if isHiding = true

        if (runningTimer <= 0)
        {
            runningRecoup--;

            if (runningRecoup < 0)
            {
                runningTimer = 500f;
                runningRecoup = 800f;
                Log("Can Run Again");
            }
        }
        //RUNNING
        if (Input.GetKey(KeyCode.W) && (Input.GetKey(KeyCode.LeftShift)))
        {
            if(runningTimer > 0f)
            {
                this.transform.Translate(Vector3.forward * Time.deltaTime * runningSpeed);
                gm.isStill = false;
                runningTimer--;
                //Log("Running");
                //Add timer so that player cannot run indefinitely
            }
            else if (runningTimer < 0f)
            {
                this.transform.Translate(Vector3.forward * Time.deltaTime * speed);
                //Log("Is Still");
                gm.isStill = false;
                //Log("Out of breath");
                //Log(gm.isStill);
            }

        }
        else
        {
            gm.isStill = true;
        }

        //FORWARDS

        //Bool doesn't work on this one specifically?
        //BACKWARDS
        if (Input.GetKey(KeyCode.S))
        {
              this.transform.Translate(Vector3.back * Time.deltaTime * backupSpeed);
            //Uses a dif, private speed to go slower. Cannot be changed in game
              gm.isStill = false;
            //Log(gm.isStill);

        }
       
        else if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward * Time.deltaTime * speed);
            //Log("Is Still");
            gm.isStill = false;
            //Log(gm.isStill);

        }
        else
        {
            gm.isStill = true;
            //Log(gm.isStill);
        }


        //Rotating is allowed in hiding mode
        //LEFT
        if (Input.GetKey(KeyCode.A))
         {
             this.transform.Rotate(new Vector3(0, -15f, 0) * Time.deltaTime * speed);
         }
         //RIGHT
         if (Input.GetKey(KeyCode.D))
         {
                this.transform.Rotate(new Vector3(0, 15f, 0) * Time.deltaTime * speed);
         }

         //RAYCAST PHYSICS
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance , interactableLayers))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Debug.Log("Interactable detected");

                //COLLECTABLE ITEMS
                if(hit.collider.CompareTag("PickUp"))
                {
                    //Collects item
                    gm.itemCount++;
                    Destroy(hit.collider.gameObject);
                    Log("Pickup interacted with");
                }
           
            }
        }

    }
    /*private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.CompareTag("Scene Change"))
        {
            sc = other.gameObject.GetComponent<Scene_Change>();
        }

    }*/




    private void OnCollisionStay(Collision other)
    {
        if(other.gameObject.CompareTag("Wall"))
        {
            speed = 2f;
            runningSpeed = 3f;

        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            speed = 5f;
            runningSpeed = 7f;

        }
    }

}

