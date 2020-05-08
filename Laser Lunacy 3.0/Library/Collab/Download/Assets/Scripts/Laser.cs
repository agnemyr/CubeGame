using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Laser : MonoBehaviour
{

    private LineRenderer lr;

    [SerializeField] private bool up;
    [SerializeField] private bool down;
    [SerializeField] private bool right;
    [SerializeField] private bool left;

    [SerializeField] private bool origin;

    GameObject lastHitCube;
    GameObject lastFrameLastHitCube;
    
    GameObject previousCube;


   


    [SerializeField] bool currentlyFiring;

    private Vector3 vector;

    // Use this for initialization
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        

        //Remove to go back
        if(origin)
        {
            currentlyFiring = true;
        }
        else
        {
            currentlyFiring = false;
        }

        if (left == true)
        {
            vector = transform.forward;
        }
        if (right == true)
        {
            vector = transform.forward * -1;
        }
        if (up == true)
        {
            vector = transform.right;
        }
        if (down == true)
        {
            vector = transform.right * -1;
        }
    }

    // Update is called once per frame
    void Update()
    {

        lr.SetPosition(0, transform.position);
        RaycastHit hit;

        if(!currentlyFiring && lastHitCube != null)
        {
            lastHitCube.GetComponent<LaserForwarding>().DisableLaserOnCube();
            lastHitCube.GetComponent<Laser>().SetCurrentlyFiring(false);

            lastHitCube.GetComponent<ActivateCube>().deactivateArrow(lastHitCube);
        }
        
        if (Physics.Raycast(transform.position, vector, out hit) && currentlyFiring)
        {
            Vector3 forward = vector * 10;
            Debug.DrawRay(transform.position, forward, Color.green);

        if (hit.collider && hit.collider.tag == "Goal")
        {

                hit.collider.gameObject.GetComponent<GoalBehaviour>().WinGame();
        
            if(gameObject.CompareTag("Cube"))
            {
                gameObject.GetComponent<MoveObject>().enabled = false;
                
            }
            gameObject.GetComponent<MoveObject>().SetLevelComplete(true);
            
            hit.collider.gameObject.GetComponent<GoalBehaviour>().WinGame();
        }

            if (hit.collider && hit.collider.tag == "Cube")
            {
                //Experimenting with currentlyFiring. Remove if statement to go back
                if (currentlyFiring) { 
                    lr.SetPosition(1, hit.point);

                    if (lastHitCube != null)
                    {
                        lastFrameLastHitCube = lastHitCube;
                    }

                    lastHitCube = hit.collider.gameObject;

                    if (lastHitCube != lastFrameLastHitCube && lastFrameLastHitCube != null)
                    {
                        lastFrameLastHitCube.GetComponent<Laser>().SetCurrentlyFiring(false);

                        lastFrameLastHitCube.GetComponent<LaserForwarding>().DisableLaserOnCube();

                        lastHitCube.GetComponent<ActivateCube>().deactivateArrow(lastHitCube);
                    }

                    //Remove to go back
                    lastHitCube.GetComponent<Laser>().SetCurrentlyFiring(true);

                    lastHitCube.GetComponent<LaserForwarding>().ActivateLaserOnCube();

                    // Activate arrow
                    lastHitCube.GetComponent<ActivateCube>().activateArrow(lastHitCube);

                }
                //Experimenting with currentlyFiring. Remove everything in else to go back
                else
                {
                    lastHitCube.GetComponent<Laser>().SetCurrentlyFiring(false);

                    lastHitCube.GetComponent<LaserForwarding>().DisableLaserOnCube();

                    lastHitCube.GetComponent<ActivateCube>().deactivateArrow(lastHitCube);
                }
            }
            if (hit.collider && hit.collider.tag != "Cube")
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    GameObject.FindWithTag("Enemy").GetComponent<DwarfBehaviour>().IsHitByLaser();
                }
                if (lastHitCube != null)
                {
                    lastHitCube.GetComponent<LaserForwarding>().DisableLaserOnCube();
                    
                    //Remove to go back
                    lastHitCube.GetComponent<Laser>().SetCurrentlyFiring(false);

                    lastHitCube.GetComponent<ActivateCube>().deactivateArrow(lastHitCube);

                    lastHitCube = null;
                }
                else
                {
                    lr.SetPosition(1, hit.point);
                }

            }

        }
        else lr.SetPosition(1, vector * 5000);

    }

  
    public void SetCurrentlyFiring(bool value)
    {
        currentlyFiring = value;
    }

    public void SetPreviousCube(GameObject cube)
    {
        previousCube = cube;
    }

}

