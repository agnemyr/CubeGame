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

    public void Start()
    {
        var em = effect.emission;
        em.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        

        other.gameObject.GetComponent<MoveObject>().SetIsTeleporting(this);

        enableParticleEffect();
    }

    
    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<MoveObject>().SetIsTeleporting(null);

        Invoke("disableParticleEffect", 2.0f);
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

             
    }
    // else if (cube.gameObject.CompareTag("Cube") && targetZone.GetComponent<ZoneBehaviour>().listIsFull() == true)
    // {
    //     cube.gameObject.GetComponent<MoveObject>().ResetPosition();
    // }
    //}

    //Methods for enabling and disapling orbiting particle effect
    private void enableParticleEffect()
    {
        var em = effect.emission;
        if (!em.enabled)
        {
            effect.Play();
            em.enabled = true;
        }
    }

    private void disableParticleEffect()
    {
        if(effect.time < 1)
        {
            Invoke("emDisable", 1.0f);
        }
        else
        {
            emDisable();
        }
    }

    private void emDisable()
    {
        var em = effect.emission;
        em.enabled = false;
    }



}
