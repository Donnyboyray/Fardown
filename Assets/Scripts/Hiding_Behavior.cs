using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//if player interacts w/ this object while in it's colliding field, they can hide in the object
//Enemy will then go back into parol mode
public class Hiding_Behavior : MonoBehaviour
{

    public bool isHiding;
    public Transform player;
    public Vector3 hidingPlace;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        hidingPlace.Set(this.transform.position.x, this.transform.position.y, this.transform.position.z);

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                isHiding = !isHiding;
                Debug.Log("Player is Hiding");
               // hidingPlace = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
                other.transform.position = hidingPlace;
            }

        }
    }
}
