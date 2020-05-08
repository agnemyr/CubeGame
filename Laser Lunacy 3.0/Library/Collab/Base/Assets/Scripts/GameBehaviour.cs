using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Mirror;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
// ReSharper disable ConditionIsAlwaysTrueOrFalse

[SuppressMessage("ReSharper", "RedundantLogicalConditionalExpressionOperand")]
public class GameBehaviour : CITEPlayer
{
    private const bool EnableMusic = false;
    
    private const bool EnableGameMusic = true;
    private const bool EnablePuzzleMusic = true;

    private const bool EnableDwarf = true;
    
    public GameObject cubeprefab;
    public GameObject dwarfPrefab;

    private float _nextDwarfSpawning;
    private static readonly int[] DwarfSpawnTimeRange = {0, 0}; // {20, 40}
    private readonly int[] _xSpawnRange = {-7, 7}; // {-7, 7}
    private readonly int[] _zSpawnRange = {-18, 18}; // {-18, 18}
    private Vector3 _spawnPoint;
    private Vector3 _spawnColliderCenter;
    private Quaternion _spawnRotation;
    private readonly Vector3 _spawnCollHalfExtents = new Vector3(1.5f, 0.65f, 1.5f);

    public GameObject prefabDown;
    
    private AudioSource _gameMusic;
    private AudioSource _puzzleMusic;
    
    
    
    [SyncVar] private float _serverTime;

    public override void OnStartLocalPlayer()
    {
        Debug.Log("Local GameBehaviour started as player "+playerID);
        _gameMusic = GetComponent<AudioSource>();
        _puzzleMusic = transform.GetChild(0).transform.GetChild(0).GetComponent<AudioSource>();

        // Find camera helpers and let them know who we are
        foreach (var helper in FindObjectsOfType<CameraPositioner>()){
            helper.setView(playerID);
        }
        if (hasAuthority)
        {
          //CmdCreateClientControlledTestObject(new Vector3());
          //CmdAssignSceneCubes(playerID);
        }
        if (isServer)
           CmdSetNextDwarfSpawning();
    }
    
    [Command] public void CmdUpdateCubePosition (GameObject cube, Vector3 position)
    {
        cube.transform.position = position;
    }

    public static int GetPlayerFromCoords(float posX, float posZ)
    {
        if (posX >= 0 && posZ >= 0) return 0;
        if (posX > 0 && posZ < 0) return 1;
        if (posX < 0 && posZ > 0) return 2;
        return 3;
    }

    /**
        A client can ask the server to spawn a test object that is controlled entirely by the
        server and has the state of it broadcast to all of the clients
    */
    [Command] public void CmdCreateServerControlledTestObject(){
        Debug.Log("Creating a test object");
        var testThing = Instantiate(cubeprefab, new Vector3(0, 0, 0), Quaternion.identity);
        testThing.GetComponent<Rigidbody>().isKinematic = false; // We simulate everything on the server so only be kinematic on the clients
        testThing.GetComponent<Rigidbody>().velocity = new Vector3(5,0,5);
        
        NetworkServer.Spawn (testThing);
    }

    /**
        A client can request that the server spawns an object that the client can control directly
    */
    [Command]
    private void CmdCreateClientControlledTestObject(Vector3 position){
        Debug.Log("Creating a test object for "+connectionToClient+" to control");
        switch (playerID)
        {
            case 0:
                position = new Vector3(6, 1.0f, 17);
                break;
            case 1:
                position = new Vector3(6, 1.0f, -17);
                break;
            case 2:
                position = new Vector3(-6, 1.0f, 17);
                break;
            case 3:
                position = new Vector3(-6, 1.0f, -17);
                break;
        }
        var cube = Instantiate(cubeprefab, position, Quaternion.identity);
        NetworkServer.Spawn(cube, connectionToClient);
    }

    [Server] public void Update()
    {
        if (EnableDwarf && isLocalPlayer && _serverTime >= _nextDwarfSpawning && GameObject.FindWithTag("Enemy") == null && 
            GameObject.Find("CDTextGreen").GetComponent<CountDownTimer>().startingTime > 0){
            SpawnDwarf();
        }
        _serverTime = Time.time;
    }

    [Command] private void CmdSetNextDwarfSpawning()
    {
       _nextDwarfSpawning = _serverTime + Random.Range(DwarfSpawnTimeRange[0], DwarfSpawnTimeRange[1]);
    }

    [Server] private void SpawnDwarf()
    {
        if (SceneManager.GetActiveScene().name.Equals("Level 2"))
        {
            CmdSetNextDwarfSpawning();
            return;
        }
        var spawnMask = LayerMask.GetMask("Cubes") | 
                        LayerMask.GetMask("Obstacles") |
                        LayerMask.GetMask("Laser");
        var x = Random.Range(_xSpawnRange[0], _xSpawnRange[1]);
        var levelDepZSpawnRange = SceneManager.GetActiveScene().name == "Level 3" ? new[]{-18, 0} : _zSpawnRange;
        var z = Random.Range(levelDepZSpawnRange[0], levelDepZSpawnRange[1]);
        _spawnPoint = new Vector3(x, 0, z);
        _spawnColliderCenter = new Vector3(_spawnPoint.x, _spawnCollHalfExtents.y, _spawnPoint.z);
        _spawnRotation = Quaternion.AngleAxis(Random.Range(0, 359), Vector3.up);
        var colliders = Physics.OverlapBox(_spawnColliderCenter, _spawnCollHalfExtents, 
            Quaternion.identity, spawnMask);
        if (colliders.Length == 0 )
        {
            var dwarf = Instantiate(dwarfPrefab, _spawnPoint, _spawnRotation);
            NetworkServer.Spawn(dwarf);
            GameObject.Find("GameManager").GetComponent<GameManager>().PlaySpawnedDwarfSound();
        }
    }

    public int GetPlayerId ()
    {
        return playerID;
    }
    
    [Command] public void CmdDwarfHitByLaser(GameObject dwarf)
    {
        NetworkServer.Destroy(dwarf);
        CmdSetNextDwarfSpawning();
    }

    [Command]
    public void CmdLoadNextLevel()
    {
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex - 1;
        FindObjectOfType<CITENetworkManager>().ServerChangeScene("Level " + nextLevelIndex);
    }
    
    [Command]
    public void CmdRestartLevel()
    {
        
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            
            FindObjectOfType<CITENetworkManager>().ServerChangeScene("GameScene"); // TODO: level names
        }
        else
        {
            int nextLevelIndex = SceneManager.GetActiveScene().buildIndex - 2;
            FindObjectOfType<CITENetworkManager>().ServerChangeScene("Level " + nextLevelIndex); // TODO: level names
        }
    }

    
    public void PlayGameMusic ()
    {
        if (playerID == 0 && EnableMusic && EnableGameMusic)
        {
            if (_puzzleMusic.isPlaying)
                _puzzleMusic.Stop();
            _gameMusic.Play();
        }
       else
        {
            _gameMusic.enabled = false;
        }
    }

    public void PlayPuzzleMusic()
    {
        if (playerID == 0 && EnableMusic && EnablePuzzleMusic) 
            _puzzleMusic.Play();
    }
}

