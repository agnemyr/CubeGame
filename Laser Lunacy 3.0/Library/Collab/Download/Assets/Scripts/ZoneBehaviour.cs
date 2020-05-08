using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ZoneBehaviour : NetworkBehaviour
{
    [SerializeField] private SpawnPoint[] spawnPoints = new SpawnPoint[2];

    private SpawnPoint s1;
    private SpawnPoint s2;

    int firstTime = 0;

    private void Start()
    {
        if (gameObject.tag == "GreenZone")
        {
            s1 = new SpawnPoint(new Vector3(13, 1, 6));
            s2 = new SpawnPoint(new Vector3(13, 1, 9));
        }
        if (gameObject.tag == "RedZone")
        {
            s1 = new SpawnPoint(new Vector3(13, 1, -6));
            s2 = new SpawnPoint(new Vector3(13, 1, -9));
        }
        if (gameObject.tag == "BlueZone")
        {
            s1 = new SpawnPoint(new Vector3(-13, 1, -6));
            s2 = new SpawnPoint(new Vector3(-13, 1, -9));
        }
        if (gameObject.tag == "YellowZone")
        {
            s1 = new SpawnPoint(new Vector3(-13, 1, 6));
            s2 = new SpawnPoint(new Vector3(-13, 1, 9));
        }

        spawnPoints[0] = s1;
        spawnPoints[1] = s2;

        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Cube")
        {
            firstTime++;
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (other.transform.position == spawnPoints[i].GetPosition())
                {
                    Debug.Log("Added cube to" + name);
                    spawnPoints[i].SetCube(other.gameObject);
                }
            }
        }
    }

    public void SetCube (GameObject cube, Vector3 position)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (position == spawnPoints[i].GetPosition())
            {
                Debug.Log("Added cube to" + name);
                spawnPoints[i].SetCube(cube);
            }
        }

    }


    public bool listIsFull()
    {
        Debug.Log(name + " IS LIST FULL?????????????????????????");

        int numberOfCubes = 0;
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            numberOfCubes += spawnPoints[i].CountCube();
        }
        Debug.Log(numberOfCubes);
        if (numberOfCubes >= 2)
        {
            Debug.Log(name + " LIST IS FULL");
            return true;
        }
        else
        {
            Debug.Log(name + " IS NOT FULL");
            return false;
        }
    }

    public void freeSlot (GameObject cube)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i].GetCube() != null)
            {
                if (cube.GetInstanceID() == spawnPoints[i].GetCube().GetInstanceID())
                {
                    Debug.Log("Setting free Spot");
                    spawnPoints[i].SetCube(null);
                }
            }
        }
    }

    public Vector3 GetSpawnPosition ()
    {

        Vector3 spawnPosition = new Vector3();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i].GetCube() == null)
            {
                spawnPosition = spawnPoints[i].GetPosition();
            }
        }
        Debug.Log(spawnPosition);
        return spawnPosition;
    }
}




