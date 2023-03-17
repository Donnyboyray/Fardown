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

    private void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            gm.isHiding = true;
            //Log("Hiding is" + gm.isHiding);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            gm.isHiding = false;
        }
    }
}
