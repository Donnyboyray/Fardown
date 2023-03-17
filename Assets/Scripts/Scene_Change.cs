using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Find out way to keep position through scene changes
public class Scene_Change : MonoBehaviour
{
    public string sceneName;
    public GameManager gm;
    //public Transform playerSpawnPosition, playerLeavePosition;
    // Start is called before the first frame update
    void Start()
    {
       //playerSpawnPosition = GameObject.Find("Player").transform;
        //gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

   private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            //playerSpawnPosition = new Vector3(other.position.x + 2f, other.position.y, other.position.z + 2f);
        }
    }
}
