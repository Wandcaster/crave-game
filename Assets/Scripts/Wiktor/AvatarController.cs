using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AvatarController : NetworkBehaviour
{
    [SerializeField]
    int avatarID;
    public void SelectAvatar()
    {
        SessionManager.Instance.SetPlayerServerRpc(NetworkManager.LocalClient.ClientId, avatarID);
    }
    
}
