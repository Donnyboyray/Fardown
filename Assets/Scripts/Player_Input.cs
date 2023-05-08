using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEngine.Debug;


public class Player_Input : MonoBehaviour
{
    //SCRIPT REFS
    public GameManager gm; //Game manager in script
    public Rigidbody rb;
    public Door_Behavior db;
    //public Scene_Change sc;

    //MOVEMENT VARIABLES

    /*public float speed; //Walking
    public float runningSpeed;*/
    //^^ Moved to Game Manager ^^

    public float runningTimer, runningRecoup; //Running
    private float rotatespeed = 6f;

    //RAYCAST
    Ray ray; //Ray that follows camera
    private float maxDistance = 5f; //Maximum limit of ray
    public LayerMask interactableLayers; //Set to Hidable, Collectable

    // Start is called before the first frame update
    void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("gm").GetComponent<GameManager>();
        db = null;
        rb = this.GetComponent<Rigidbody>();

        gm.isHiding = false; //Set to false just in case
        gm.isStill = false;

        runningTimer = 500f;
        runningRecoup = 800f;
    }

    void Start()
    {
        this.transform.position = gm.newScenePosition;
        this.transform.rotation = Quaternion.Euler(0, gm.yRotation, 0);
    }

    // Update is called once per frame
    void Update()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY;

        //Log("Speed is" + gm.speed + "Running is" + gm.runningSpeed);
        //Freezed player when dialogue is playing
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
          Time.timeScale = 0;


        }
        else
        {
            Time.timeScale = 1;
        }

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
                this.transform.Translate(Vector3.forward * Time.deltaTime * gm.runningSpeed);
                gm.isStill = false;
                runningTimer--;
                //Log("Running");
                //Add timer so that player cannot run indefinitely
            }
            else if (runningTimer < 0f)
            {
                this.transform.Translate(Vector3.forward * Time.deltaTime * gm.speed);
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
              this.transform.Translate(Vector3.back * Time.deltaTime * gm.backupSpeed);
            //Uses a dif, private speed to go slower. Cannot be changed in game
              gm.isStill = false;
            //Log(gm.isStill);

        }
       
        else if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward * Time.deltaTime * gm.speed);
            //Log("Is Still");
            gm.isStill = false;
            //Log(gm.isStill);

        }
        else
        {
            gm.isStill = true;
            //Log(gm.isStill);
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }


        //Rotating is allowed in hiding mode
        //LEFT
        if (Input.GetKey(KeyCode.A))
        {
            //Log("Turning");
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            this.transform.Rotate(new Vector3(0, -15f, 0) * Time.deltaTime * rotatespeed);
         }
         else if (Input.GetKey(KeyCode.D))
         {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            this.transform.Rotate(new Vector3(0, 15f, 0) * Time.deltaTime * rotatespeed);
         }
        else
        {
            //Log("Not Turning");
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

         //RAYCAST PHYSICS
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance , interactableLayers))
        {
            if (Input.GetKey(KeyCode.E))
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

                if (hit.collider.CompareTag("Door"))
                {
                    db = hit.collider.gameObject.GetComponent<Door_Behavior>();
                    db.ToggleDoor();
                }


            }
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Wall"))
        {
            Log("is Colliding");
            gm.speed = 3f;
            gm.runningSpeed = 3f;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            gm.speed = 5f;
            gm.runningSpeed = 7f;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotation;;

        }
    }

}

