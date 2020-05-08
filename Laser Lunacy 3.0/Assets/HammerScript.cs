using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class HammerScript : NetworkBehaviour
{
    private List<GameObject> _cubes;
    private static readonly int NearCube = Animator.StringToHash("nearCube");
    
    // Start is called before the first frame update

    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {
        var dwarf = GameObject.FindWithTag("Enemy");
        _cubes = dwarf.GetComponent<DwarfBehaviour>().cubes.ToList();
        switch (other.tag)
        {
            case "Cube":
                var o = other.gameObject;
                if (!o.GetComponent<MeshRenderer>().enabled) break;
                Debug.Log("Destroyed cube: " + o.name);
                _cubes.Remove(o);
                GameObject.Find("GameManager").GetComponent<GameManager>().PlaySmashedCubeSound();
                dwarf.GetComponent<DwarfBehaviour>().SmashCube(o);
                transform.parent.gameObject.GetComponent<Animator>().SetBool(NearCube, false);
                break;
        }
    }
}
