using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //Holds inventory 
    //Currently it's going to be a number system
    public float itemCount, playerHealth;
    public bool isHiding, isStill, isAttracting;


    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {

        if (playerHealth <= 0)
        {
            Application.Quit();
        }
    }

}
