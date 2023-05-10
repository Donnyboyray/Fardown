using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Debug;

public class SetNewPosition : MonoBehaviour
{
    public Vector3 newPosition;
    private Vector3 currentPosition;
    public bool usingObject;
    // Start is called before the first frame update
    void Awake()
    {
        usingObject = false;
    }
  
    //not very good performance and doesn't work
    //Streamline these values into the Game Manager that calls them up when player's Raycast hits an object of this kind
    //Create new Tag + Layer for these objects and we should be good
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Log("Is Colliding with Ladder");

            if(Input.GetKeyDown(KeyCode.E))
            {
                Log("Used Ladder");
                if (!usingObject)
                {
                    currentPosition = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
                    other.transform.position = newPosition;
                }
                else
                {
                    other.transform.position = currentPosition;
                }

            }
        }
    }
}
