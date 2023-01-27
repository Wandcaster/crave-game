using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class Card : MonoBehaviour {
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private TMP_Text description;
        [SerializeField] private TMP_Text cardName;
        [SerializeField] private TMP_Text cost;

        public PlayerController cardOwner;
        public CardData cardData { get; set; }

        public void Initialise(CardData cardData) {
            this.cardData = cardData;
            description.text = cardData.descrition;
            cardName.text = cardData.name;
            cost.text = cardData.useCosts.ToString();
            sprite.sprite = cardData.image;
            cardOwner = SessionManager.Instance.player0Controller;
        }
        public void PlayCard(Characteristics target, PlayerController source)
        {
            Debug.Log("Energy" + source.energy + "|:" + cardData.useCosts);
            if (source.energy.Get() - cardData.useCosts < 0) return;
            source.energy.Set(source.energy.Get()- cardData.useCosts);

            foreach (var effect in cardData.effect)
            {
                effect.act.ApplyEffect(target, source, effect.strength);
            }
            source.discarded.Add(this);
            source.hand.Remove(this);
            gameObject.SetActive(false);
        }


    }
}
