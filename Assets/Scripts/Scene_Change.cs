using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Change : MonoBehaviour
{
    //public Transform player;
    public string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.Find("Player").transform;
    }

   private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}
