using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace UI {
    public class FightUIControllerMock : MonoBehaviour {
        [SerializeField] private List<CardData> sampleCards;
        private FightUIController cc;

        private void Start() {
            cc = GetComponent<FightUIController>();
            cc.OnEndTurn += () => {
                Debug.Log("Player is ending turn!");
            };
            cc.OnCardPlayed += (card, target) => {
                Debug.Log($"Played card {card.cardName} against {target} who is {LayerMask.LayerToName(target.layer)}");
            };
            KeepAddingCards().Forget();
        }

        private async UniTaskVoid KeepAddingCards() {
            var random = new Random();
            for (int i = 0; i < 5; ++i) {
                var list = new List<CardData> {
                    sampleCards[random.Next(sampleCards.Count)],
                    sampleCards[random.Next(sampleCards.Count)],
                    sampleCards[random.Next(sampleCards.Count)]
                };
                var c = await cc.DrawCards(list);
                await cc.AddToHand(c);
            }
            // foreach (var sampleCard in sampleCards) {
            //     await UniTask.Delay(TimeSpan.FromSeconds(1));
            //     await cc.AddToHand(sampleCard);
            // }
        }
    }
}
