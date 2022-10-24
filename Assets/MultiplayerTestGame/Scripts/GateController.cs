using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GateController : NetworkBehaviour
{
    [SerializeField]
    TextMeshProUGUI winText;
    [SerializeField]
    GameObject ball;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) return;

        if (collision.CompareTag("Ball"))
        {
            ResetGame();
            if (name == "redGate") winText.text = "Blue Win";
            else winText.text = "Red Win";
        }
    }
    private void ResetObject(NetworkObject networkObject,Vector3 position)
    {
        networkObject.transform.position =position;
        networkObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
    private void Update()
    {
        if (!IsServer) return;
        if (Input.GetKeyDown(KeyCode.Space)) ResetGame();
    }
    private void ResetGame()
    {
        IReadOnlyList<NetworkClient> playerList = NetworkManager.Singleton.ConnectedClientsList;
        for (int i = 0; i < playerList.Count; i++)
        {
            if (i % 2 == 1) ResetObject(playerList[i].PlayerObject, new Vector3(-6.5f, 0, 0));
            else ResetObject(playerList[i].PlayerObject, new Vector3(6.5f, 0, 0));
        }
        ResetObject(ball.GetComponent<NetworkObject>(), Vector3.zero);
    }
}
