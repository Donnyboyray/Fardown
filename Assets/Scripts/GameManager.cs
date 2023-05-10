using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //Holds inventory 
    //Currently it's going to be a number system
    public float itemCount, playerHealth, yRotation;
    public bool isHiding, isStill, isAttracting;
    public Vector3 newScenePosition;
    private static GameManager gmInstance;

    public float speed; //Walking
    public float runningSpeed;
    public float backupSpeed = 4f; //backing up 

    //Add in inventory here
    //Inven consists of array of variables that adds on when a new type of object is discovered
    //I.E., starts empty until player finishes starting scene and then add rations then so on
    //Moving through inventory is just scrolling through array 
    //Other objects cannot be in multiple and are in a special part of the inventory that unlocks as each item is discovered
    // OR objects that aren't in multiples are used if player clicks input button in correct location/on correct object

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (gmInstance == null)
        {
            gmInstance = this;
        }
        else
        {
            Object.Destroy(this.gameObject);
        }
    }

    private void Update()
    {

        if (playerHealth <= 0)
        {
            Application.Quit();
        }
    }

}
