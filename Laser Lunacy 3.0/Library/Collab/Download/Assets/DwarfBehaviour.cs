

using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;


public class DwarfBehaviour : NetworkBehaviour {

    private Transform[] _points;
    private int _destPoint = 0;
    private NavMeshAgent _agent;
    public List<GameObject> cubes;

    private const float CubeDetectDist = 3.5f;
    private const int FreezeTime = 3;
    private const int DwarfChildren = 2;
    
    private Animator _animator;
    private static readonly int NearCube = Animator.StringToHash("nearCube");
    private static readonly int HitByLaser = Animator.StringToHash("hitByLaser");
    private static readonly int FrontalHit = Animator.StringToHash("frontalHit");
    
    private AudioSource _scream;
    private bool _hitByLaser;

    private float _eventTimeStamp;

    private void Start ()
    {
        _eventTimeStamp = Time.time;
        _scream = gameObject.GetComponent<AudioSource>();

        _animator = GetComponent<Animator>();
        _animator.enabled = false;
        
        SetPoints();
        _destPoint = GetClosestPoint(_points, 1000f) - 1;
        if (_destPoint < 0) _destPoint++;

        _agent = GetComponent<NavMeshAgent>();
        _agent.autoBraking = false;
    }

    private int GetClosestPoint(IReadOnlyList<Transform> pts, float minDist)
    {
        var point = 0;
        for (var index = 0; index < pts.Count; index++)
        {
            var dist = Vector3.Distance(transform.position, pts[index].position);
            if (dist <= minDist)
            {
                minDist = dist;
                point = index;
            }
        }
        return point;
    }

    private void SetPoints()
    {
        _points = new Transform[8];
        _points[0] = GameObject.Find("Vertex0").transform;
        _points[1] = GameObject.Find("Vertex1").transform;
        _points[2] = GameObject.Find("Vertex2").transform;
        _points[3] = GameObject.Find("Vertex3").transform;
        _points[4] = GameObject.Find("Vertex4").transform;
        _points[5] = GameObject.Find("Vertex5").transform;
        _points[6] = GameObject.Find("Vertex6").transform;
        _points[7] = GameObject.Find("Vertex7").transform;
    }
    
    private void GotoNextPoint() {
        if (_points.Length == 0)
            return;
        _destPoint = (_destPoint + 1) % _points.Length;
    }

    private void Update () {
        if (Time.time - _eventTimeStamp < FreezeTime)
        {
            var alpha = (Time.time - _eventTimeStamp) / FreezeTime;
            if (_hitByLaser) alpha = 1 - alpha;
            for (var i = 0; i < DwarfChildren; i++)
            {
                MoveObject.SetAlpha(transform.GetChild(i).gameObject, alpha);
            }
            return;
        }
        if (!_animator.enabled) _animator.enabled = true;
        if (_hitByLaser)
        {
            foreach (var gb in FindObjectsOfType<GameBehaviour>())
            {
                if (gb.isLocalPlayer)
                {
                    gb.CmdDwarfHitByLaser(gameObject);
                    return;
                }
            }
        }
        if (!FindCube()) // TODO: change after proper NavMesh
        {
            if (!_agent.pathPending && _agent.remainingDistance < 0.1f)
            {
                GotoNextPoint();
            }
            _agent.destination = _points[_destPoint].position;
        }
    }

    private bool FindCube()
    {
        var thisPos = gameObject.transform.position;
        var cubePosition= thisPos;
        var minDist = 1000.0f;
        cubes.Clear();
        foreach (var cube in GameObject.FindGameObjectsWithTag("Cube"))
        {
            if (cube.GetComponent<MoveObject>().IsOnFloor())
                cubes.Add(cube);
        }
        const int layerMask = 1 << 10;
        foreach (var cube in cubes)
        {
            if (cube.name.Equals("StartCube")) continue;
            if (!cube.GetComponent<MeshRenderer>().enabled) continue;
            if (Physics.Linecast(cube.transform.position, thisPos, layerMask)) continue;
            var distance = Vector3.Distance(cube.transform.position, thisPos);
            if (distance >= minDist) continue;
            cubePosition = cube.transform.position;
            minDist = distance;
        }
        if (minDist < CubeDetectDist)
        {
            _animator.SetBool(NearCube, true);
        }
        else _animator.SetBool(NearCube, false);

        if (cubePosition != thisPos)
        {
            _agent.destination = cubePosition;
            return true;
        }
        return false;
    }

    public void IsHitByLaser()
    {
        if (_hitByLaser) return;
        _eventTimeStamp = Time.time;
        _scream.Play();
        _hitByLaser = true;
        _animator.enabled = false;
        _agent.enabled = false;
        disableColliders();
        foreach (var comp in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            comp.material.color = new Color(1, 0.5f, 1);
        }
    }

    private void disableColliders()
    {
        foreach (var coll in GetComponentsInChildren<MeshCollider>())
        {
            coll.enabled = false;
        }
    }

    public void SmashCube(GameObject cube)
    {
        cube.GetComponent<MoveObject>().Smash();
    }
}
