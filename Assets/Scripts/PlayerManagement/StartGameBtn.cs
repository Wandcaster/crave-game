using PlayerManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StartGameBtn : NetworkBehaviour
{
    [SerializeField]
    private PlayerSelector playerSelector;
    private void Start()
    {
        if (!IsHost) gameObject.SetActive(false);
    }
    public void StartGame()
    {
        if (playerSelector.p1Character == PlayableCharacterType.None || playerSelector.p2Character == PlayableCharacterType.None) return;
        if (playerSelector.p1Character == playerSelector.p2Character) return;

        SessionManager.Instance.p1Character = playerSelector.p1Character;
        SessionManager.Instance.p2Character = playerSelector.p2Character;
        SessionManager.Instance.StartGame();
    }
}
