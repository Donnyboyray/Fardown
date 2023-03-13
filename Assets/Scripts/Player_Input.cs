using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input : MonoBehaviour
{
    //SCRIPT REFS
    public GameManager gm; //Game manager in script

    //MOVEMENT VARIABLES
    public float speed; //Walking
    public float runningSpeed; //Running
    private float backupSpeed = 3f; //backing up 

    //HIDING VARIABLES
    public bool isHiding; //Checks if player is hiding or not
    private Vector3 hidingPlace; //Current position of hiding place
    private Vector3 playerHidingPlace;
    private Vector3 formerStandingPosition; //Position of player when interacting w/ hiding place

    //RAYCAST
    Ray ray; //Ray that follows camera
    private float maxDistance = 3f; //Maximum limit of ray
    public LayerMask interactableLayers; //Set to Hidable, Collectable


    // Start is called before the first frame update
    void Start()
    {
        isHiding = false; //Set to false just in case
    }

    // Update is called once per frame
    void Update()
    {
        //Drawing Ray
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        //Inspector view of ray
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.cyan);

        //Checks for isHiding to be false
        //Movement is frozen if isHiding = true
        if (!isHiding)
        {
            //RUNNING
            if (Input.GetKey(KeyCode.W) && (Input.GetKey(KeyCode.LeftShift)))
            {
                this.transform.Translate(Vector3.forward * Time.deltaTime * runningSpeed);
                //Add timer so that player cannot run indefinitely
            }
            //FORWARDS
            else if (Input.GetKey(KeyCode.W))
            {
                this.transform.Translate(Vector3.forward * Time.deltaTime * speed);
            }
            //BACKWARDS
            if (Input.GetKey(KeyCode.S))
            {
                this.transform.Translate(Vector3.back * Time.deltaTime * backupSpeed);
                //Uses a dif, private speed to go slower. Cannot be changed in game
            }
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
                    Debug.Log("Pickup interacted with");
                }

                //HIDING
                if(isHiding == false) //would probably work w/o this
                {
                    if (hit.collider.CompareTag("Hidable"))
                    {

                        hidingPlace = new Vector3(hit.transform.position.x, hit.transform.position.y + 1.5f, hit.transform.position.z);
                        formerStandingPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

                        Hiding();

                        Debug.Log("Hide interacted with");

                    }
                }
            }
        }

        //ESCAPE HIDING
        if (isHiding == true)
        {
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                Hiding();
                Debug.Log("Escaped Hiding");
            }
        }

    }

    //HIDE FUNCTION
    void Hiding()
    {
        isHiding = !isHiding;

        if(isHiding == true)
        {
            this.transform.position = hidingPlace;
        }
        else
        {
            this.transform.position = formerStandingPosition;
        }

    }

}

