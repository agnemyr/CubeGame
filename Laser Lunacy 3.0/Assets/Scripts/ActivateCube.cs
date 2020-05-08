using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCube : MonoBehaviour
{
    /*
        Aktiverar och deaktiverar pilen på kuben när den träffas av en laser
    */

    public void activateArrow(GameObject cube)
    {
        cube.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(1, 1, 1, 0));

        cube.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    }

    public void deactivateArrow(GameObject cube)
    {
        cube.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
    }
}
