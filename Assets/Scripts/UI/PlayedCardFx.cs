using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ElRaccoone.Tweens;
using UnityEngine;

namespace UI {
    public class PlayedCardFx : MonoBehaviour {
        private List<SpriteRenderer> renderers;
        private void Awake() {
            transform.localScale = Vector3.zero;
            gameObject.SetActive(false);
            renderers = GetComponentsInChildren<SpriteRenderer>().ToList();
        }

        public async UniTask PlayCardEffect(CardData cardData) {
            GetComponent<Card>().Initialise(cardData);
            gameObject.SetActive(true);
            foreach (var srenderer in renderers) {
                var color = srenderer.color;
                color = new Color(color.r, color.g, color.b, 1);
                srenderer.color = color;
            }
            await transform.TweenLocalScale(Vector3.one * 1.3f, 0.3f).Await();
            await UniTask.Delay(1500);
            await Task.WhenAll(renderers.Select(it => it.TweenSpriteRendererAlpha(0, 0.9f).Await()));
            transform.localScale = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
