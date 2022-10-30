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
                SelectServerRpc(PlayerType.P1, character);
            }
            else
            {
                SelectServerRpc(PlayerType.P2, character);
            }
        }
        [ServerRpc(RequireOwnership =false)]
        public void SelectServerRpc(PlayerType playerType, PlayableCharacterType character) 
        {
            SelectClientRpc(playerType, character);
        }
        [ClientRpc]
        public void SelectClientRpc(PlayerType playerType, PlayableCharacterType character)
        {
            selector.ChangeCharacter(playerType, character);
        }
    }
}
