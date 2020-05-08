using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ObstacleController : MonoBehaviour
{

    Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }

    public void Activate () {

        transform.position = transform.position - new Vector3(0, 2f, 0);
        UpdatePosition();


    }

    public void Deactivate()
    {
        transform.position = originalPosition;
        UpdatePosition();

    }

    private void UpdatePosition ()
    {
        Vector3 inputPosition = transform.position;
        Debug.Log("Activate");
        foreach (GameBehaviour gb in FindObjectsOfType<GameBehaviour>())
        {
            if (gb.isLocalPlayer)
            {
                gb.CmdUpdateCubePosition(gameObject, inputPosition);
            }
        }
    }

}
