using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.Debug;


public class Door_Behavior : MonoBehaviour
{

    private bool doorIsClosed;
    public float doorSpeed;
    private Vector3 doorOpenPosition, doorClosedPosition;


    void Awake()
    {
        doorIsClosed = false;
        doorOpenPosition = new Vector3(this.transform.position.x, this.transform.position.y + 4.50f, this.transform.position.z);
  
    }

    public void ToggleDoor() //put in two dif functions so that it doesn't just loop endlessly
    {

        if (doorIsClosed)
        {
            //Maybe move to coroutine to allow for smooth animation
            //Log("Door is open");

            this.transform.position = Vector3.Lerp(this.transform.position, doorOpenPosition, doorSpeed);

            if (this.transform.position. y >= 4.30f)
            {
                doorIsClosed = false;
            }
        }
        else
        {
            //Log("Door is closed");
            doorClosedPosition = new Vector3(this.transform.position.x, this.transform.position.y - 5.50f, this.transform.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, doorClosedPosition, doorSpeed);

            if (this.transform.position.y <= -1f)
            {
                doorIsClosed = true;
            }
        }
    }
   
}
