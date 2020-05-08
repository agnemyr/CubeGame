using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleButtonScript : MonoBehaviour
{
    public ObstacleController movableWall;

    private AudioSource sfx;


    //Change me to change the touch phase used.

    private void Start()
    {
        sfx = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (Input.touchCount > 0) return;
        OnInputDown();
    }

    private void OnMouseDrag()
    {
        if (Input.touchCount > 0) return;
        OnInputDrag();
    }


    private void OnMouseUp()
    {
        OnInputUp();
    }

    public void OnInputDown()
    {

        sfx.Play();
        movableWall.Activate();
    }

    public void OnInputDrag()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag != "TempleButton")
            {
                movableWall.Deactivate();
            }
        }

    }

    public void OnInputUp()
    {
        movableWall.Deactivate();

    }






    //   void Update()
    //   {
    //   
    //   if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
    //   {
    //       
    //       Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
    //       RaycastHit raycastHit;
    //       if (Physics.Raycast(raycast, out raycastHit))
    //       {
    //           Debug.Log("Raycast");
    //
    //           if (raycastHit.collider.CompareTag("TempleButton"))
    //           {
    //                  movableWall.Activate();
    //           }
    //       }
    //   }
    //      if ((Input.touchCount >= 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
    //      {
    //
    //          Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
    //          RaycastHit raycastHit;
    //          if (Physics.Raycast(raycast, out raycastHit))
    //          {
    //              Debug.Log("Raycast");
    //
    //              if (raycastHit.collider.CompareTag("TempleButton"))
    //              {
    //                  movableWall.Deactivate();
    //              }
    //          }
    //      }
    //
    //
    //  }

    public void ActivateWall()
    {
        movableWall.Activate();
    }

    public void DeactivateWall()
    {
        movableWall.Deactivate();
    }



}

