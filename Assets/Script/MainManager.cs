using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainManager : NetworkBehaviour, INetworkRunnerCallbacks
{
    public NetworkPrefabRef _malePlayerPrefab;
    public NetworkPrefabRef _femalePlayerPrefab;

    public NetworkRunner _runner;
    public NetworkSceneManagerDefault _sceneManager;

    void Awake()
    {
        if(_runner == null)
        {
            GameObject runnerObj = new GameObject("NetworkRunner");
            _runner = runnerObj.AddComponent<NetworkRunner>();
            _runner.AddCallbacks(this);
            _sceneManager = runnerObj.AddComponent<NetworkSceneManagerDefault>();
        }
        ConnectToFusion();
    }

    async void ConnectToFusion()
    {
        Debug.Log("Connecting to Fusion Network...");
        _runner.ProvideInput = true;
        string sessionName = "MyGameSession";

        var startGameArgs = new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SceneManager = _sceneManager,
            SessionName = sessionName,
            PlayerCount = 5,
            IsVisible = true,
            IsOpen = true,
        };

        var result = await _runner.StartGame(startGameArgs);
        if(result.Ok)
        {
            Debug.Log("Connected to Fusion Network successfully");
        }   
        else
        {
            Debug.Log($"Failed to connect: {result.ShutdownReason}");
        }
        InvokeRepeating(nameof(SpawnEnemy), 2, 2);
    }


    private void Start()
    {
       
    }

    public NetworkPrefabRef[] EnemyPrefab;
    private NetworkObject _spawnedEnemy;

    public void SpawnEnemy()
    {
        var enemyPrefab = EnemyPrefab[UnityEngine.Random.Range(0, EnemyPrefab.Length)];
        var position = new Vector3(UnityEngine.Random.Range(-10, 10), 1, UnityEngine.Random.Range(-10, 10));
        var rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);

        _spawnedEnemy = _runner.Spawn(
            enemyPrefab,
            position,
            rotation,
            null,
            (r, o) =>
            {
                EnemyAI enemyAI = o.GetComponent<EnemyAI>();
                enemyAI.networkRunner = r;
            });
        Invoke(nameof(DeSpawnEnemy), 15);
    }
    void DeSpawnEnemy()
    {
        if (_spawnedEnemy != null)
        {
            _runner.Despawn(_spawnedEnemy);
        }
    }
    public void OnConnectedToServer(NetworkRunner runner)
    {
       
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
       
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
      
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
     
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
       
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("....Player joined: " + player);
        if (_runner.LocalPlayer != player) return;
        // thực hiện spawn nhân vật
        var playerClass = PlayerPrefs.GetString("PlayerClass");
        var playerName = PlayerPrefs.GetString("PlayerName","");

        var prefab = playerClass.Equals("Male") ? _malePlayerPrefab : _femalePlayerPrefab/*playerClass.Equals("") ? "":  ""*/;
        var position = new Vector3(0, 0.544f, 0);

        _runner.Spawn(
            prefab,
            position,
            Quaternion.identity,
            player,
            (r, o) =>
            {
                PlayerGun playerGun = o.GetComponent<PlayerGun>();
                playerGun.networkRunner = r;
            }
          );

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
     
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
  
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
  
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
}
