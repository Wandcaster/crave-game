using Unity.Netcode;
using UnityEngine;

namespace PlayerManagement {
    public class SelectableCharacter : NetworkBehaviour {
        [SerializeField] private PlayableCharacterType character;
        [SerializeField] private PlayerSelector selector;
        public PlayerType currentPlayer;
        public void Select()
        {
            if(IsHost)
            {
                SelectServerRpc(PlayerType.P1, character,IsHost);
            }
            else
            {
                SelectServerRpc(PlayerType.P2, character, IsHost);
            }
        }
        [ServerRpc(RequireOwnership =false)]
        public void SelectServerRpc(PlayerType playerType, PlayableCharacterType character, bool isHost) 
        {
            SelectClientRpc(playerType, character, isHost);
        }
        [ClientRpc]
        public void SelectClientRpc(PlayerType playerType, PlayableCharacterType character, bool isHost)
        {
            selector.ChangeCharacter(playerType, character);
            if (isHost)
            {
                if(character==PlayableCharacterType.Kuro) SessionManager.Instance.player0Controller = GameObject.Find("KuroIcon").GetComponent<PlayerController>();
                else SessionManager.Instance.player0Controller = GameObject.Find("ShiroIcon").GetComponent<PlayerController>();
            }
            else
            {
                if (character == PlayableCharacterType.Kuro) SessionManager.Instance.player1Controller = GameObject.Find("KuroIcon").GetComponent<PlayerController>();
                else SessionManager.Instance.player1Controller = GameObject.Find("ShiroIcon").GetComponent<PlayerController>();
            }
        }
    }
}
