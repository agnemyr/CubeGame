using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnPoint 
{

    [SyncVar][SerializeField] Vector3 position = new Vector3();
    [SyncVar][SerializeField] GameObject cube;
   

    public SpawnPoint (Vector3 position)
    {
        this.position = position;
        
    }

    public void SetCube (GameObject cube)
    {
        this.cube = cube;    
    }

   

    public Vector3 GetPosition ()
    {
        return position;
    }

    public GameObject GetCube ()
    {
        return cube;
    }

    public int CountCube ()
    {
        if (cube != null)
        {
            return 1;
        }
        return 0;
    }

    public string Text()
    {
        return "" + position;

    }
}
