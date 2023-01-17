using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI {
    public class FightUIControllerMock : MonoBehaviour {
        [SerializeField] private List<CardData> sampleCards;

        private void Awake() {
            KeepAddingCards().Forget();
        }

        private async UniTaskVoid KeepAddingCards() {
            var cc = GetComponent<FightUIController>();
            foreach (var sampleCard in sampleCards) {
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                await cc.AddToHand(sampleCard);
            }
        }
    }
}
