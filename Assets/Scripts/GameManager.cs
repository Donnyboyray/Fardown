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
