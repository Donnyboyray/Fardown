using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEngine.Debug;

//if player interacts w/ this object while in it's colliding field, they can hide in the object
//Enemy will then go back into parol mode
public class Hiding_Behavior : MonoBehaviour
{

    public GameManager gm;
    public Vector3 hidingPosition, outsidePosition;
    public float hidingRotation;

    private void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
           if (Input.GetKeyDown(KeyCode.E))
           {
                if(gm.isHiding == false)
                {
                    gm.speed = 0f;
                    gm.runningSpeed = 0f;
                    gm.backupSpeed = 0f;
                    //prevPosition = other.transform.position; //Gets previous standing position
                    other.transform.position = hidingPosition; //Sets position to inside of the object
                    other.transform.rotation = Quaternion.Euler(0, hidingRotation, 0); //Turns player around so they're not staring at a wall
                    gm.isHiding = true; //Turn on isHiding so enemies go back to Patrol mode
                    Log("Hiding is" + gm.isHiding);
                }
                else
                {
                    gm.speed = 5f;
                    gm.runningSpeed = 7f;
                    gm.backupSpeed = 4f;
                    other.transform.position = outsidePosition; //Put player back in front of the object
                    gm.isHiding = false; //Turn off isHiding to be detected by enemies again
                }
           }
        }

    }
}

    

