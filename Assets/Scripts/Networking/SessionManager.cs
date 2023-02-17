using PlayerManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionManager : NetworkSingleton<SessionManager>
{
    [Header("Config")]
    [SerializeField]
    NetworkManager networkManager;
    [SerializeField]
    string characterSelectSceneName;
    [SerializeField]
    string MapSceneName;
    [SerializeField]
    string mainMenuSceneName;
    [Header("Data")]
    public string joinCode;
    public int seed;
    public PlayerController player0Controller; //Local Player
    public PlayerController player1Controller;
    public NetworkVariable<bool> bossFight = new NetworkVariable<bool>(false);


    private void Start()
    {
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(this);
        networkManager.OnServerStarted += ServerStart;
        networkManager.OnClientDisconnectCallback += ServerStop;
        SceneManager.sceneLoaded += InitForPlayerSelectScene;
    }
    private void InitForPlayerSelectScene(Scene arg0, LoadSceneMode arg1)
    {
        SyncSeedClientRpc();
        if (arg0.name.Equals("PlayerSelect"))
        {
            PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();
            player0Controller = playerControllers[0];
            player1Controller= playerControllers[1];
        }
    }
    private void ServerStart()
    {
        networkManager.SceneManager.LoadScene(characterSelectSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    private void ServerStop(ulong id)
    {
        if(IsHost) networkManager.SceneManager.LoadScene(mainMenuSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    public void StartGame()
    {
        SyncSeedClientRpc(System.DateTime.Now.Millisecond);
        networkManager.SceneManager.LoadScene(MapSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        
    }
    [ClientRpc]
    private void SyncSeedClientRpc(int seed)
    {
        this.seed = seed;
        Random.InitState(seed);
    }
    [ClientRpc]
    private void SyncSeedClientRpc()
    {
        Random.InitState(seed);
    }
}
