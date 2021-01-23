using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    //variables for the camera script
    private Transform target;
    private float trackSpeed = 10;


    private void Start()
    {
        //gets the gameobject with the tag 'player'
        target = GameObject.FindGameObjectWithTag("Player").transform;   
    }

    // Set target
    public void SetTarget(Transform playerTransform)
    {
        //sets the targets position to the playerposition
        target = playerTransform;
    }

    // Track target
    void LateUpdate()
    {
        if (target)
        {
            //camera follows the player 
            var v = transform.position;
            v.x = target.position.x;
            transform.position = Vector3.MoveTowards(transform.position, v, trackSpeed * Time.deltaTime);
        }
    }
}
