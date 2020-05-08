using System;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable Unity.PreferNonAllocApi
// ReSharper disable SwitchStatementMissingSomeEnumCasesNoDefault

public class GameManager : CITEPlayer
{
    

    private readonly Dictionary<int, GameObject> _cubesTouched = new Dictionary<int, GameObject>();

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void PlaySmashedCubeSound()
    {
        transform.Find("Sounds").Find("CubeSmashed").GetComponent<AudioSource>().Play();
    }

    public void PlaySpawnedDwarfSound()
    {
        transform.Find("Sounds").Find("DwarfSpawned").GetComponent<AudioSource>().Play();
    }

    public void PlayCubeHitByLaserSound()
    {
        transform.Find("Sounds").Find("CubeHitByLaser").GetComponent<AudioSource>().Play();
    }
    
    private void FixedUpdate()
    {
        foreach (var touch in Input.touches)
        {
            if (_cubesTouched.TryGetValue(touch.fingerId, out var cube))
            {
                UpdateCubeMovement(touch, cube);
                continue;
            }
            var ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0));
            var colliders = Physics.RaycastAll(ray.origin, ray.direction);
            foreach (var hit in colliders)
            {
                var obj = hit.transform.gameObject;
                if (obj.CompareTag("Cube"))
                {
                    UpdateCubeMovement(touch, obj);
                }
            }
        }
    }

    private void UpdateCubeMovement(Touch touch, GameObject cube)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                cube.GetComponent<MoveObject>().OnInputDown(new Vector3(touch.position.x, touch.position.y, 0));
                if (!_cubesTouched.ContainsKey(touch.fingerId))
                    _cubesTouched.Add(touch.fingerId, cube);
                break;
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                cube.GetComponent<MoveObject>().OnInputUp();
                _cubesTouched.Remove(touch.fingerId);
                break;
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                cube.GetComponent<MoveObject>().OnInputDrag(new Vector3(touch.position.x, touch.position.y, 0));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
