using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserForwarding : MonoBehaviour
{

    private bool laserActivated = false;
    private LineRenderer lr;
    bool isHit;

    public void Start()
    {
        GetComponent<Laser>().enabled = false;
        lr = GetComponent<LineRenderer>();

    }

    public void ActivateLaserOnCube()
    {

        if (laserActivated == false)
        {
            GetComponent<Laser>().enabled = true;
            laserActivated = true;
            Debug.Log("Laser Activated");
            enableLinderenderer();
            GameObject.Find("GameManager").GetComponent<GameManager>().PlayCubeHitByLaserSound();
        }


    }

    public void DisableLaserOnCube()
    {

        if (laserActivated == true)
        {
            laserActivated = false;
            GetComponent<Laser>().enabled = true;
            disableLineRenderer();
            Debug.Log("Laser Deactivated");
        }


    }

    private void disableLineRenderer()
    {
        lr.enabled = false;
    }

    private void enableLinderenderer()
    {
        lr.enabled = true;
    }




    public void SetIsHit(bool hit)
    {
        isHit = hit;
    }


}
