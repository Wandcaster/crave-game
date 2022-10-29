using UnityEngine;

namespace PlayerManagement {
    public class SelectableCharacter : MonoBehaviour {
        [SerializeField] private PlayableCharacterType character;
        [SerializeField] private PlayerSelector selector;
        [SerializeField] private PlayerType currentPlayer;

        public void Select() {
            selector.ChangeCharacter(currentPlayer, character);
        }
    }
}
