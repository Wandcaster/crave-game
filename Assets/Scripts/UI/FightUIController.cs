using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlayerManagement;
using UnityEngine;

namespace UI {
    public class FightUIController : MonoBehaviour {
        [SerializeField] private CardContainer cardsInHandContainer;
        [SerializeField] private EnemyCupboard enemyContainer;
        
        public event CardContainer.CardEvent OnCardPlayed {
            add => cardsInHandContainer.onCardPlayed += value;
            remove => cardsInHandContainer.onCardPlayed -= value;
        }

        public event Action OnEndTurn;
        
        public async UniTaskVoid DrawCards(ICollection<CardData> cards) {
            // TODO: call ui to draw cards
            throw new NotImplementedException("pretty card selection aniamtion?");
        }

        public IEnumerable<CardData> cardsInHand => cardsInHandContainer.cardsInHand;

        public async UniTaskVoid AddToHand(CardData card) {
            // HACK: move this elsewhere if possible
            foreach (var effectData in card.effect) {
                effectData.effect = (Effect) Activator.CreateInstance(Type.GetType(effectData.effectType.ToString()) ??
                                                                      throw new InvalidOperationException(
                                                                          "Null effect type"));
            }
            cardsInHandContainer.AddCard(card);
        }

        public async UniTaskVoid RemoveCard(int index) {
            cardsInHandContainer.RemoveCard(index);
        }

        public void SetTurn(PlayableCharacterType playableCharacter) {
            
        }

        public void SetPlayerOnThisHost(PlayableCharacterType playableCharacter) {
            
        }

        public void AddEnemy() {
            // This is a stub
            enemyContainer.AddEnemy();
        }

        public void RemoveEnemy(ulong id) {
            
        }
    }
}
