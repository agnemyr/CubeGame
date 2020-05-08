using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGoal : MonoBehaviour
{


    Rigidbody myRigidBody;

    public float speed = 5;
    private void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();

    }

    public void MoveDown ()
    {
        //transform.position = transform.position + new Vector3(-speed, 0, 0);
        myRigidBody.AddForce(new Vector3(-speed, 0, 0));
        
        Debug.Log(transform.position);
    }

    public void MoveUp()
    {
        //transform.position = transform.position + new Vector3(speed, 0, 0);
        myRigidBody.AddForce(new Vector3(speed, 0, 0));
        Debug.Log(transform.position);
    }

    public void MoveRight()
    {
        // transform.position = transform.position + new Vector3(0, 0, -speed);
        myRigidBody.AddForce(new Vector3(0, 0, -speed));
        Debug.Log(transform.position);
    }

    public void MoveLeft()
    {
        // transform.position = transform.position + new Vector3(0, 0, speed);
        myRigidBody.AddForce(new Vector3(0, 0, speed));
        
        Debug.Log(transform.position);
    }

    private void FixedUpdate()
    {
        UpdatePosition(transform.position);
    }

    private void UpdatePosition(Vector3 inputPosition)
    {
       
        foreach (GameBehaviour gb in FindObjectsOfType<GameBehaviour>())
        {
            if (gb.isLocalPlayer)
            {
                gb.CmdUpdateCubePosition(gameObject, inputPosition);
            }
        }
    }

}
