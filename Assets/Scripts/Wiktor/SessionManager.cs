using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SessionManager : NetworkSingleton<SessionManager>
{
    public ulong player0 = 2;
    public ulong player1 = 2;

    [ServerRpc(RequireOwnership =false)]
    public void SetPlayerServerRpc(ulong player, int id)
    {
        if (id == 0)
        {
            if (player == player1) player1 = 2;
            player0 = player;
        }
        else
        {
            if (player == player0) player0 = 2;
            player1 = player;
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void StartGameServerRpc()
    {
        if (player0 != 2 && player1 != 2 && player0 != player1) StartGameClientRpc();
    }
    [ClientRpc]
    private void StartGameClientRpc()
    {
        Debug.LogError("Start Game");
    }
}
