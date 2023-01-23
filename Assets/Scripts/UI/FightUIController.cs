using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlayerManagement;
using UnityEngine;

namespace UI {
    public class FightUIController : MonoBehaviour {
        [SerializeField] private CardContainer cardsInHandContainer;
        [SerializeField] private EnemyCupboard enemyContainer;
        [SerializeField] private CardSelectionContainer selectionContainer;

        public void EndTurn() {
            if (cardsInHandContainer.isHostsTurn)
                OnEndTurn?.Invoke();
        }
        
        public event CardContainer.CardEvent OnCardPlayed {
            add => cardsInHandContainer.onCardPlayed += value;
            remove => cardsInHandContainer.onCardPlayed -= value;
        }

        public event Action OnEndTurn;
        
        public async UniTask<CardData> DrawCards(ICollection<CardData> cards) {
            return await selectionContainer.SelectCard(cards);
        }

        public IEnumerable<CardData> cardsInHand => cardsInHandContainer.cardsInHand;

        public async UniTask AddToHand(CardData card) {
            // HACK: move this elsewhere if possible
            foreach (var effectData in card.effect) {
                effectData.act = (Effect) Activator.CreateInstance(Type.GetType(effectData.effectType.ToString()) ??
                                                                      throw new InvalidOperationException(
                                                                          "Null effect type"));
            }
            cardsInHandContainer.AddCard(card);
        }

        public async UniTask RemoveCard(int index) {
            cardsInHandContainer.RemoveCard(index);
        }

        /**
         * Set the current character's turn, this isn't the host's turn but the in game character's
         * Set null for no player turn (aka enemy turn)
         * This is responsible for disabling card interactions 
         */
        public void SetTurn(PlayableCharacterType? playableCharacter) {
            cardsInHandContainer.currentTurn = playableCharacter;
        }

        /**
         * Set the character the player on this computer is using 
         */
        public void SetPlayerOnThisHost(PlayableCharacterType playableCharacter) {
            cardsInHandContainer.hostCharacter = playableCharacter;
        }

        /**
         * Creates a new EnemyController and initialises it with `data`
         */
        public void AddEnemy(EnemyData data) {
            // This is a stub
            enemyContainer.AddEnemy(data);
        }

        
        /**
         * Remove an enemy by its network object id (EnemyController.NetworkObjectId)
         */
        public void RemoveEnemy(ulong id) {
            enemyContainer.RemoveEnemy(id);
        }
    }
}
