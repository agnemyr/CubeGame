using System;
using System.Linq;
using UnityEngine;
using Mirror;

public class MoveObject : NetworkBehaviour
{
    public float respawnAlpha = 0.5f;
    public int respawnTime = 10; // 10

    Vector3 dist;
    Vector3 startPos;
    float posX; float posZ; float posY;
    bool mouseDown;
    bool levelComplete = false;
    
    [SerializeField] private GameObject teleportpad;

    private GameObject telepad;

    private bool _respawning;
    private float _timeTillRespawn;
    private readonly Vector3 _respawnThrust = new Vector3(0, 2000, 0);
    private static readonly int Color = Shader.PropertyToID("_Color");
    private GameObject _progressBar;

    private Rigidbody m_Rigidbody;
    
    private void Start()
    {
        Transform transform1;
        _progressBar = (transform1 = transform).GetChild(3).gameObject;
       // _progressBar.GetComponent<ProgressBar>().AlignProgressBar(transform1.position);
        m_Rigidbody = GetComponent<Rigidbody>();
        

        //This locks the RigidBody so that it does not move or rotate in the z axis (can be seen in Inspector).
        
    }

    private void Update()
    {
        if (_timeTillRespawn > 0)
        {
            _timeTillRespawn -= Time.deltaTime;
            _progressBar.GetComponent<ProgressBar>().UpdateBar((respawnTime - _timeTillRespawn) / respawnTime);
        }
        else if (_respawning)
        {
            SetAlpha(gameObject, 1);
            SetAlpha(transform.GetChild(0).transform.gameObject, 1);
            transform.GetChild(1).GetComponent<AudioSource>().Play();
            GetComponent<Rigidbody>().AddForce(_respawnThrust, ForceMode.Impulse);
            transform.GetChild(2).transform.gameObject.SetActive(false);
            _progressBar.GetComponent<ProgressBar>().StopDisplaying();
            _respawning = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) == true)
        {
            transform.position = transform.position - new Vector3(0, 0, 1);
        }

        if (mouseDown == false)
        {
            m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY |
                RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        }
    }

    public void OnMouseDown()
    {
        if (Input.touchCount > 0) return; // Separate mouse and touch entrance
        OnInputDown(Input.mousePosition);

    }

    public void OnMouseDrag()
    {
        if (Input.touchCount > 0) return;
        OnInputDrag(Input.mousePosition);
    }

    public void OnMouseUp()
    {
        if (Input.touchCount > 0) return;
        if (levelComplete)
        {
            mouseDown = true;
            return;
        }
        OnInputUp();
        mouseDown = false;

    }

    public void OnInputDrag(Vector3 pos)
    {
        if (_timeTillRespawn > double.Epsilon) return;
        if (mouseDown)
        {
            var disX = pos.x - posX;
            var disY = pos.y - posY;
            var disZ = pos.z - posZ;
            var lastPos = Camera.main.ScreenToWorldPoint(new Vector3(disX, disY, disZ));
            UpdatePosition(lastPos, false);
        }
    }

    public void OnInputDown(Vector3 pos)
    {
        if (_timeTillRespawn > double.Epsilon) return;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        mouseDown = true;
        startPos = transform.position;
        dist = Camera.main.WorldToScreenPoint(transform.position);
        posX = pos.x - dist.x;
        posY = pos.y - dist.y;
        posZ = pos.z - dist.z;
        Debug.Log("mousedown " + mouseDown);
    }

    public void OnInputUp()
    {
        mouseDown = false;
        if (_timeTillRespawn > double.Epsilon) return;

        if (teleportpad == null)
        {
            ResetPosition();
            Debug.Log("Resetting position");
        }
        else
        {

            teleportpad.GetComponent<TeleportCube>().DoTeleport(gameObject);
        }
           
    }

    public void ResetPosition()
    {
        UpdatePosition(startPos, true);
        mouseDown = false;
    }

    public void UpdatePosition(Vector3 inputPosition, bool isTeleported)
    {
        if (isTeleported)
        {
            mouseDown = false;
            Debug.Log("mousedown " + mouseDown);
           // _progressBar.GetComponent<ProgressBar>().AlignProgressBar(inputPosition);
        }
        foreach (GameBehaviour gb in FindObjectsOfType<GameBehaviour>())
        {
            if (gb.isLocalPlayer)
            {
                gb.CmdUpdateCubePosition(gameObject, inputPosition);
            }
        }
    }

    public void Smash()
    {
        UpdatePosition(startPos, true);
        _timeTillRespawn = respawnTime;
        SetAlpha(gameObject, respawnAlpha);
        SetAlpha(transform.GetChild(0).transform.gameObject, respawnAlpha);
        var color = GetComponent<MeshRenderer>().material.color;
        GetComponent<MeshRenderer>().material.SetColor(Color,
            new Color(color.r, color.g, color.b, respawnAlpha));
        transform.GetChild(2).transform.gameObject.SetActive(true);
        _respawning = true;
    }

    public static void SetAlpha(GameObject obj, float alpha)
    {
        var comp = obj.GetComponent<MeshRenderer>();
        if (comp == null)
        {
            var comp2 = obj.GetComponent<SkinnedMeshRenderer>();
            var color2 = comp2.material.color;
            comp2.material.SetColor(Color, new Color(color2.r, color2.g, color2.b, alpha));
        }
        else
        {
            var color = comp.material.color; 
            comp.material.SetColor(Color, new Color(color.r, color.g, color.b, alpha));
        }
        
    }

    public bool IsOnFloor()
    {
        var cubeTransform = transform;
        // ReSharper disable once Unity.PreferNonAllocApi
        var position = cubeTransform.position;
        var localScale = cubeTransform.localScale;
        var colliders = Physics.OverlapBox(new Vector3(position.x, position.y - localScale.z / 2, 
                position.z), localScale, Quaternion.identity, ~0);
        foreach (var collider1 in colliders)
        {
            //if (gameObject.name.Equals("CubeRight (2)")) 
                //Debug.Log(collider1.name);
        }
        //return colliders.All(coll => coll.name.Equals("Floor"));
        return !colliders.Any(coll => coll.name.Equals("YellowZone") || 
            coll.name.Equals("GreenZone") || coll.name.Equals("RedZone") || coll.name.Equals("BlueZone"));
    }

    public bool GetMouseDown()
    {
        return mouseDown;
    }

    public void SetIsTeleporting(GameObject teleportPad)
    {
        this.teleportpad = teleportPad;
    }

    public void SetLevelComplete (bool isComplete)
    {
        levelComplete = isComplete;
    }


}


