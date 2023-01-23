using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ElRaccoone.Tweens;
using UnityEngine;

namespace UI {
    public class CardSelectionContainer : MonoBehaviour {
        private List<Clickable> items;
        private TaskCompletionSource<CardData> cardClickedTask;

        private void Awake() {
            Debug.Log("Starting");
            items = FindObjectsOfType<Clickable>().ToList();
            Debug.Log($"Found items {items}");
            foreach (var clickable in items) {
                clickable.onClick += obj => cardClickedTask.TrySetResult(obj.GetComponent<Card>().cardData);
                clickable.transform.localScale = Vector3.zero;
            }
            gameObject.SetActive(false);
        }

        private Task<CardData> AwaitCardSelection() {
            cardClickedTask = new TaskCompletionSource<CardData>();
            return cardClickedTask.Task;
        }

        public async UniTask<CardData> SelectCard(IEnumerable<CardData> cards) {
            Debug.Log("Selecting card");
            gameObject.SetActive(true);
            using var en = cards.GetEnumerator();
            foreach (var c in items) {
                var card = c.GetComponent<Card>();
                en.MoveNext();
                card.Initialise(en.Current);
                card.TweenLocalScale(Vector3.one, 0.3f);
            }

            var selected= await AwaitCardSelection();
            var makeSmallTasks = items.Select(i => i.TweenLocalScale(Vector3.zero, 0.3f).Await());
            await Task.WhenAll(makeSmallTasks);
            gameObject.SetActive(false);
            return selected;
        }
    }
}
