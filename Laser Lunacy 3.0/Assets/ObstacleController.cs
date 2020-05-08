using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ObstacleController : MonoBehaviour
{

    Vector3 originalPosition;
    AudioSource sfx;

    private void Start()
    {
        originalPosition = transform.position;
        sfx = GetComponent<AudioSource>();
    }

    public void Activate () {

        transform.position = transform.position - new Vector3(0, 2f, 0);
        UpdatePosition();
        Debug.Log("Activate");
        sfx.Play();


    }

    public void Deactivate()
    {
        transform.position = originalPosition;
        UpdatePosition();
        Debug.Log("DeActivate");
        sfx.Play();
    }

    private void UpdatePosition ()
    {
        Vector3 inputPosition = transform.position;
        
        foreach (GameBehaviour gb in FindObjectsOfType<GameBehaviour>())
        {
            if (gb.isLocalPlayer)
            {
                gb.CmdUpdateCubePosition(gameObject, inputPosition);
            }
        }
    }

}
