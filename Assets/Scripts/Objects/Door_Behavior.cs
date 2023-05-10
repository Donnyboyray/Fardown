using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.Debug;


public class Door_Behavior : MonoBehaviour
{

    public bool doorIsOpen, doorIsCooldown;
    public GameObject doorVisual;
    public float doorSpeed;
    private Vector3 doorOpenPosition, doorClosedPosition;


    void Awake()
    {
        doorIsOpen = false;
        //doorVisual = gameObject.Find("Visual");
        doorOpenPosition = new Vector3(this.transform.position.x, this.transform.position.y + 4.50f, this.transform.position.z);
        doorClosedPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
    }

    public IEnumerator ToggleDoor()
    {
        Log("Door Toggled");
        if (doorIsOpen)
        {
            doorIsCooldown = true;

            //Use WaitUntil => y value =>
            //yield return new WaitForSeconds(3);

            //Maybe put this in it's own void? Maybe not
            doorVisual.transform.position = Vector3.Lerp(this.transform.position, doorClosedPosition, Time.deltaTime);

            //doorVisual.transform.position = doorClosedPosition * doorSpeed;

            yield return new WaitForSeconds(1);
            doorIsOpen = false;
            doorIsCooldown = false;
        }
        else
        {

            doorIsCooldown = true;

            //doorVisual.transform.position = doorOpenPosition * doorSpeed;

            //yield return new WaitForSeconds(3);
            doorVisual.transform.position = Vector3.Lerp(this.transform.position, doorOpenPosition, doorSpeed);

            yield return new WaitForSeconds(1);
            doorIsOpen = true;
            doorIsCooldown = false;
        }
  
        doorIsOpen = !doorIsOpen;
    }
}

//Jitters weirdly every once and a while and not only when the player is constantly holding the button down
//Need to find a way to set the Input to GetKeyDown and this code to some sort of IEnumerator that'll play the animation independently of input
/*public void ToggleDoorOpen() //put in two dif functions so that it doesn't just loop endlessly
{
        //Maybe move to coroutine to allow for smooth animation
        //Log("Door is open");
    if(!doorIsCoolDown)
    {
        this.transform.position = Vector3.Lerp(this.transform.position, doorOpenPosition, doorSpeed);

    }

    if (this.transform.position.y >= 4.30f)
    {
        doorIsCoolDown = true;
        StartCoroutine("ToggleCooldown");
    }


}

  public void ToggleDoorClosed()
  {

      if (!doorIsCoolDown)
      {
          this.transform.position = Vector3.Lerp(this.transform.position, doorClosedPosition, doorSpeed);
      }

      if (this.transform.position.y <= 0f)
      {
          //doorIsClosed = true;
          StartCoroutine("ToggleCooldown");
      }
  }

  IEnumerator ToggleCooldown()
  {
      yield return new WaitForSeconds(1f);
      doorIsClosed = !doorIsClosed;

      yield return new WaitForSeconds(1f);
      doorIsCoolDown = false;
  }*/

