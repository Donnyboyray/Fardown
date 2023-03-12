using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input : MonoBehaviour
{

    public float speed;
    public float runningSpeed;
    private float backupSpeed = 3f;
    public Transform player;
    public bool isHiding;
    public Vector3 hidingPlace;
    private Vector3 playerHidingPlace;
    public LayerMask whatIsHiding;
    public GameObject hidingPlaceObj;

    public GameManager gm; //Game manager in script
 

    // Start is called before the first frame update
    void Start()
    {
        //gm = GetComponent<GameManager>();
        hidingPlaceObj = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHiding)
        {

            //RUNNING
            if (Input.GetKey(KeyCode.W) && (Input.GetKey(KeyCode.LeftShift)))
            {
                player.transform.Translate(Vector3.forward * Time.deltaTime * runningSpeed);
                //Add timer so that player cannot run indefinitely
            }
            //FORWARD
            else if (Input.GetKey(KeyCode.W))
            {
                player.transform.Translate(Vector3.forward * Time.deltaTime * speed);

            }
            //BACKWARDS
            if (Input.GetKey(KeyCode.S))
            {
                player.transform.Translate(Vector3.back * Time.deltaTime * backupSpeed);
                //Uses a dif, private speed to go slower. Cannot be changed in game
            }
        }

            //LEFT
         if (Input.GetKey(KeyCode.A))
         {
             player.transform.Rotate(new Vector3(0, -15f, 0) * Time.deltaTime * speed);
         }
            //RIGHT
         if (Input.GetKey(KeyCode.D))
         {
                player.transform.Rotate(new Vector3(0, 15f, 0) * Time.deltaTime * speed);
         }

        if (Physics.Raycast(hidingPlace, transform.forward, 2f, whatIsHiding))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //hidingPlaceObj
                isHiding = !isHiding;
                if (isHiding == true)
                {
                    Debug.Log("Player is Hiding");
                    //playerHidingPlace = new Vector3(other.transform.position.x, player.transform.position.y, other.transform.position.z);
                   // this.transform.position = playerHidingPlace;
                }
            }
        }



    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                //Collects item
                gm.itemCount++;
                // Destroy(other.gameObject);
                other.gameObject.SetActive(false);
            }
        }

        if (other.gameObject.CompareTag("Hidable"))
        {
            // Debug.Log("Can Hide");

        }
    }

    public void OnCollisionStay(Collision other)
    {

    }
}
