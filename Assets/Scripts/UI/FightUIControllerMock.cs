using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlayerManagement;
using UnityEngine;
using Random = System.Random;

namespace UI {
    public class FightUIControllerMock : MonoBehaviour {
        [SerializeField] private List<CardData> sampleCards;
        [SerializeField] private List<EnemyData> sampleEnemies;
        private FightUIController cc;

        private void Start() {
            cc = GetComponent<FightUIController>();
            //foreach (var enemy in sampleEnemies) {
            //    cc.AddEnemy(enemy);
            //}
            cc.SetHp(PlayableCharacterType.Kuro, 9, 10);
            cc.SetHp(PlayableCharacterType.Shiro, 9, 13);
            cc.SetEnergy(PlayableCharacterType.Kuro, 6);
            cc.SetEnergy(PlayableCharacterType.Shiro, 4);
            
            cc.OnEndTurn += () => {
                Debug.Log("Player is ending turn!");
            };
            cc.OnCardPlayed += (card, target,source) => {
                /*
                 * This is only executed if the card is usable in the first place - if there isnt enough energy
                 * or if the target isnt suitable, OnCardPLayed will not be called
                 */
                card.PlayCard(target.GetComponent<Characteristics>(),source);

                Debug.Log($"Played card {card.cardData.cardName} against {target} who is {LayerMask.LayerToName(target.layer)}");
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
            cc.SetHp(PlayableCharacterType.Kuro, 2, 10);
            await UniTask.Delay(1000);
            await cc.PlayCardEffects(sampleCards[random.Next(sampleCards.Count)]);
            await UniTask.Delay(500);
            await cc.PlayCardEffects(sampleCards[random.Next(sampleCards.Count)]);
            // foreach (var sampleCard in sampleCards) {
            //     await UniTask.Delay(TimeSpan.FromSeconds(1));
            //     await cc.AddToHand(sampleCard);
            // }
        }
    }
}
