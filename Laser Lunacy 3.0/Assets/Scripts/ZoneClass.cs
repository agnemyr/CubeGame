using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneClass
{

    [SerializeField] private ArrayList cubeList = new ArrayList();
    [SerializeField] private string zoneName;
 

    public ZoneClass (string name)
    {
        zoneName = name;
      
    }


    public ArrayList getCubeList()
    {
        return this.cubeList;
    }

    public void addCubeToList (GameObject cube)
    {
        cubeList.Add(cube);
    }

    public void removeCubeFromList (GameObject cube)
    {
        cubeList.Remove(cube);
    }

    public string getName ()
    {
        return zoneName;
    }
}
