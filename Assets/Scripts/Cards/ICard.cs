using UnityEngine;

public abstract class ICard:MonoBehaviour
{
    public CardData cardData;
    public string cardName
    {
        get
        {
            return cardData.cardName;
        }
        set
        {
            cardData.cardName = value;
        }
    }
    public string descrition
    {
        get
        {
            return cardData.descrition;
        }
        set
        {
            cardData.descrition = value;
        }
    }
    public int useCosts
    {
        get
        {
            return cardData.useCosts;
        }
        set
        {
            cardData.useCosts = value;
        }
    }
    public Sprite image
    {
        get
        {
            return cardData.image;
        }
        set
        {
            cardData.image = value;
        }
    }
}
