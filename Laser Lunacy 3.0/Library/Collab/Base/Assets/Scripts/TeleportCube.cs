using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TeleportCube : NetworkBehaviour
{
    [SerializeField] public GameObject zone;
    [SerializeField] public GameObject targetZone;
    
    public ParticleSystem effect;

    
    private void OnTriggerStay(Collider other)
    {
        

        other.gameObject.GetComponent<MoveObject>().SetIsTeleporting(this);
     
    }

    
    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<MoveObject>().SetIsTeleporting(null);
    }

    
    public void DoTeleport (GameObject cube)
    {
     // if (cube.CompareTag("Cube") && targetZone.GetComponent<ZoneBehaviour>().listIsFull() == false)
     // {
            // cube.gameObject.GetComponent<MoveObject>().UpdatePosition(newPos, true);
            // targetZone.GetComponent<ZoneBehaviour>().SetCube(cube.gameObject, newPos);
            //zone.GetComponent<ZoneBehaviour>().freeSlot(cube.gameObject);

            foreach (GameBehaviour gb in FindObjectsOfType<GameBehaviour>())
        {
            if (gb.isLocalPlayer)
            {    
                  gb.CmdUpdateZones(cube, targetZone, zone);
            }
        }
            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            ps.Play();
            Debug.Log("Playing teleport effect");
            var em = ps.emission;
            em.enabled = true;

            
            
        }
    // else if (cube.gameObject.CompareTag("Cube") && targetZone.GetComponent<ZoneBehaviour>().listIsFull() == true)
    // {
    //     cube.gameObject.GetComponent<MoveObject>().ResetPosition();
    // }
   //}

    
    

}
