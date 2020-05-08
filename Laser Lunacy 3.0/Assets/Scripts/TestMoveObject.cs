
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TestMoveObject : MonoBehaviour
{
    Vector3 dist;
    Vector3 startPos;
    float posX;
    float posZ;
    float posY;
    bool mouseDown;

    float x;
    float y;
    float z;

    void OnMouseDown()
     {

        mouseDown = true;
        // transform.position = transform.position;
        startPos = transform.position;
        dist = Camera.main.WorldToScreenPoint(transform.position);
        posX = Input.mousePosition.x - dist.x;
        posY = Input.mousePosition.y - dist.y;
        posZ = Input.mousePosition.z - dist.z;

    }
   
     void OnMouseDrag()
     {
        if (mouseDown == true)
        {
            float disX = Input.mousePosition.x - posX;
            float disY = Input.mousePosition.y - posY;
            float disZ = Input.mousePosition.z - posZ;
            Vector3 lastPos = Camera.main.ScreenToWorldPoint(new Vector3(disX, disY, disZ));
            transform.position = new Vector3(lastPos.x, startPos.y, lastPos.z);

        }
    }

//  public void OnMouseUp()
//  {
//      // Debug.Log("mouseDown in onMouseUp-function: " + mouseDown);
//      if (mouseDown == true)
//      {
//          resetPosition();
//      }
//      mouseDown = true;
//
//  }
   
     private void disableMouse ()
     {
         mouseDown = false;
         
         // Debug.Log("Mouse is disabled");
     }
   
     private void resetPosition ()
     {
         mouseDown = false;
         transform.position = startPos;
     }
   
     public Vector3 getStartPos ()
     {
         return startPos;
     }
   
   public void setNewStartPosition (Vector3 sp)
   {
       startPos = sp;
   }
   
     private void updateStartPos ()
     {
         startPos = transform.position;
     }


}


