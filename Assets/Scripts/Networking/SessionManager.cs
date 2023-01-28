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
    private void Start()
    {
        DontDestroyOnLoad(this);
        networkManager.OnServerStarted += ServerStart;
        networkManager.OnClientDisconnectCallback += ServerStop;
        SceneManager.sceneLoaded += InitForPlayerSelectScene;
    }
    private void InitForPlayerSelectScene(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name.Equals("PlayerSelect"))
        {
            PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();
            player0Controller = playerControllers[1];
            player1Controller= playerControllers[0];
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
        SyncSeedClientRpc(Random.Range(1, 1000));
        networkManager.SceneManager.LoadScene(MapSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        
    }
    [ClientRpc]
    private void SyncSeedClientRpc(int seed)
    {
        this.seed = seed;
        Random.InitState(seed);
    }
}
