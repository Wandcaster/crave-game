using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCardDescription : MonoBehaviour
{
    [SerializeField] public SpriteRenderer image;
    [SerializeField] public TMPro.TextMeshProUGUI cardName;
    [SerializeField] public TMPro.TextMeshProUGUI description ;
    [SerializeField] public TMPro.TextMeshProUGUI cost ;


    // Start is called before the first frame update
    void Start()
    {
        UpdateDescription();
    }
    public void UpdateDescription()
    {
        ICard card = GetComponent<ICard>();
        image.sprite = card.image;
        cardName.text = card.cardName;
        description.text = card.descrition;
        cost.text = card.useCosts.ToString();
    }


}
