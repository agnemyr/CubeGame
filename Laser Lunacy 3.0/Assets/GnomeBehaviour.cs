using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class GnomeBehaviour : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;
    public List<GameObject> cubes;
    
    private const float CubeDetectDist = 3.1f;
    private const float VertexDetectDist = 0.1f;

    private Vector3 _movement;
    private Vector3 _rotation;
    
    //private HashSet<GameObject> _activeVertices;
    //private HashSet<GameObject> _inactiveVertices = new HashSet<GameObject>();
    private GameObject[] _wayPoints;
    private int _destPoint = 0;
    
    //private GameObject _vertexPursued;
    //private bool _pursuingVertex;
    
    private AudioSource _scream;
    private bool _hitByLaser;
    private static readonly int NearCube = Animator.StringToHash("nearCube");
    private static readonly int HitByLaser = Animator.StringToHash("hitByLaser");
    private static readonly int FrontalHit = Animator.StringToHash("frontalHit");
    private Animator _animator;

    private NavMeshAgent _agent;
    

    // Start is called before the first frame update
    private void Start()
    {
        _scream = gameObject.GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        cubes = GameObject.FindGameObjectsWithTag("Cube").ToList();
        //_activeVertices = new HashSet<GameObject>(GameObject.FindGameObjectsWithTag("Vertex"));
        InitWayPoints();
        cubes.Remove(GameObject.Find("StartCube"));
        _agent = GetComponent<NavMeshAgent>();
    }

    private void InitWayPoints()
    {
        _wayPoints = GameObject.FindGameObjectsWithTag("Vertex");
        for (var index = 0; index < _wayPoints.Length; index++)
        {
            _wayPoints[index] = _wayPoints[index];
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (_scream.isPlaying) return;
        if (_hitByLaser)
        {
            Destroy(gameObject);
            return;
        }
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
            FindCubes(Time.deltaTime);
        //FindCubesByNavMesh();
        //Debug.Log("Active vertices: " + _activeVertices.Count + ". Inactive vertices: " + _inactiveVertices.Count);
        //Debug.Log("Pursuing vertex: " + (_pursuingVertex ? "true" : "false"));
    }

    private void FindCubesByNavMesh()
    {
        var agent = GetComponent<NavMeshAgent>();
        throw new NotImplementedException();
    }

    private void FindCubes(float dt)
    {
        _animator.SetBool(NearCube, false);
        var transform1 = transform;
        var thisPos = transform1.position;
        Vector3 closestPosition = thisPos;
        if (!SetClosestCube(thisPos, out var closestCube))
        {
            Debug.Log("No visible cube");
            SetClosestVertex2();
            /*if (!SetClosestVertex(thisPos, out _vertexPursued))
            {
                Debug.Log("No visible vertex");
                closestPosition = thisPos;
            } 
             else
            {
                closestPosition = _vertexPursued.transform.position;
                Debug.Log("Seeing a vertex!");
            }*/
        }
        else
        {
            closestPosition = closestCube.transform.position;
            Debug.Log("Seeing a cube!");
        }

        //SetClosestCube(thisPos, out var closestCube);
        //closestPosition = closestCube.transform.position;

        Debug.DrawLine(thisPos, closestPosition, Color.red, 1, false);
        //_agent.destination = closestPosition;
        //transform.position = Vector3.MoveTowards(thisPos, closestPosition, movementSpeed * dt);
        //transform.rotation = GetRotation(thisPos, closestPosition, dt);

    }

    private void SetClosestVertex2()
    {
        // Returns if no points have been set up
        if (_wayPoints.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        _agent.destination = _wayPoints[_destPoint].transform.position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        _destPoint = (_destPoint + 1) % _wayPoints.Length;
    }

    private bool SetClosestVertex(Vector3 thisPos, out GameObject vertex)
    {
        /*if (SetClosestObject(thisPos, out vertex, out var distance, _activeVertices))
        {
            _pursuingVertex = true;
            if (distance < VertexDetectDist)
            {
                Debug.Log("Reached vertex!");
                // Reached vertex
                _pursuingVertex = false;
                _inactiveVertices.Add(vertex);
                _activeVertices.Remove(vertex);
                _vertexPursued = null;
                return false;
            }
            return true;
        }
        return false;*/
        vertex = null;
        return false;
    }

    private Quaternion GetRotation(Vector3 thisPos, Vector3 closestPosition, float dt)
    {
        var transform1 = transform;
        var targetDirection = closestPosition - transform1.position;
        var singleStep = rotationSpeed * dt;
        Vector3 newDirection = Vector3.RotateTowards(transform1.forward, targetDirection, 
            singleStep, 0.0f);
        //Debug.DrawRay(transform.position, newDirection, Color.red);
        return Quaternion.LookRotation(newDirection);
    }

    private bool SetClosestCube(Vector3 thisPos, out GameObject closestCube)
    {
        //if (!SetClosestObject(thisPos, out closestCube, out var minDistance, cubes)) return false;
        //_pursuingVertex = false;
        /*if (minDistance < CubeDetectDist)
        {
            _animator.SetBool(NearCube, true);
            //_activeVertices.UnionWith(_inactiveVertices);
            //_inactiveVertices.Clear();
        }
        return true;*/
        closestCube = null;
        return false;
    }

    /*private bool SetClosestObject(Vector3 thisPos, out GameObject closestObject, out float minDistance,
        IReadOnlyCollection<GameObject> gameObjects)
    {
        closestObject = _pursuingVertex ? _vertexPursued : null;
        minDistance = 1000.0f;
        if (gameObjects.Count == 0)
            return false;
        const int layerMask = 1 << 10;
        //const int layerMask = 0;
        foreach (var o in gameObjects)
        {
            if (o.tag.Equals("Vertex") && _pursuingVertex && !o.Equals(_vertexPursued)) continue;
            var distance = Vector3.Distance(o.transform.position, thisPos);
            if (distance >= minDistance) continue;
            if (Physics.Linecast(o.transform.position, thisPos, layerMask)) continue;
            minDistance = distance;
            closestObject = o;
            if (o.tag.Equals("Vertex")) _vertexPursued = o;
        }
        return (Math.Abs(minDistance - 1000.0f) > double.Epsilon);
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (_hitByLaser) return;
        switch (other.tag)
        {
            case "Laser": IsHitByLaser(other);
                break;
        }
    }

    private void IsHitByLaser(Collider other)
    {
        _hitByLaser = true;
        _scream.Play();
        _animator.SetBool(HitByLaser, true);
        var transform1 = transform;
        var position = transform1.position;
        var closestLaserPoint = other.ClosestPointOnBounds(position);
        var hitDirection = position - closestLaserPoint;

        float hitAngle = Vector3.Angle(transform.forward, hitDirection);
        if (Math.Abs(hitAngle) <= 90)
            _animator.SetBool(FrontalHit, true);
        else _animator.SetBool(FrontalHit, false);
        transform.GetComponentInChildren<Material>().color = new Color(1.0f, 0, 0, 0.1f);
        //material.shader = Shader.Find("Standard");
        //material.color = new Color(1.0f, 0.5f, 0.5f, 0.1f);
    }
}
