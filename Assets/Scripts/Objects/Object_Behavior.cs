using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Contains animation and plain turning
//

public class Object_Behavior : MonoBehaviour
{

    public Transform target;
    public float objSpeed;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        objSpeed = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relativePos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);

        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * objSpeed);
    }
}
