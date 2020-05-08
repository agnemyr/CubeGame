using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleButtonScript : MonoBehaviour
{
    public ObstacleController movableWall;

    private void OnMouseDown()
    {
        movableWall.Activate();
        Debug.Log("Orb is pressed");
    }

    private void OnMouseDrag()
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
       

    private void OnMouseUp()
    {
        movableWall.Deactivate();
    }
}

