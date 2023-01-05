using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class Card : MonoBehaviour {
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private TMP_Text description;
        [SerializeField] private TMP_Text cardName;
        [SerializeField] private TMP_Text cost;
        public CardData cardData { get; private set; }

        public void Initialise(CardData cardData) {
            this.cardData = cardData;
            description.text = cardData.descrition;
            cardName.text = cardData.name;
            cost.text = cardData.useCosts.ToString();
            sprite.sprite = cardData.image;
        }
        
        
    }
}
