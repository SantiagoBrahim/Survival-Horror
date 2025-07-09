using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookjAtPlayer : MonoBehaviour
{

    public Transform target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction = target.position - transform.position;
        //direction.y = 0;
        //Quaternion rotation = Quaternion.LookRotation(direction);
        //transform.rotation = rotation;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
    }
}
