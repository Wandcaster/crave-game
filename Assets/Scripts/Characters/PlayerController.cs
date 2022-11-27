using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Characteristics
{
    [SerializeField] public int energy=3;
    [SerializeField] public int maxEnergy;
    [SerializeField] public int drawCardsInHand = 3;
    public bool turnEnded=false;
    [SerializeField] public List<ICard> draw;//available to draw
    [SerializeField] public List<ICard> hand;//cards in hand
    [SerializeField] public List<ICard> discarded;//used/discarded cards
    [SerializeField] private GameObject deckFolder;
    [SerializeField] private float cardDistance = 3.0f;
    [SerializeField] private float cardPositionY = -4;//middle card position

    private void Awake()
    {
        maxEnergy = energy;
        FightController.Instance.HandDraw.AddListener(DrawCard);
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        Debug.Log(status);
        for (int i = 0; i < draw.Count; i++)
        {
            draw[i] = Instantiate(draw[i], deckFolder.transform);
        }
    }
    public void DrawCard()
    {
        turnEnded = false;
        for (int i = 0; i < hand.Count; i++) hand[i].gameObject.SetActive(false);

        discarded.AddRange(hand);
        hand.Clear();
        for (int i = 0; i < drawCardsInHand; i++)
        {
            Draw();
        }
        RepositionCards();
        //ShowCards();
    }

    private void RepositionCards()
    {
        float numberOfSpaces = hand.Count - 1;//i needed to give here float because with int i couldn't cast int to float, number of spaces between cards (all)
        for(int i = 0; i < hand.Count; i++)
        {
            hand[i].transform.position = new Vector3(
                (-numberOfSpaces/ 2)*cardDistance + i*cardDistance,
                cardPositionY, 0);
            ShowCard(hand[i]);
        }

    }

    private void Draw()
    {
        if (draw.Count == 0) Shuffle();
        int cardID = UnityEngine.Random.Range(0, draw.Count);
        ICard newCard = draw[cardID];
        hand.Add(newCard);
        draw.Remove(newCard);
        //ShowCard(newCard);
    }
    private void ShowCard(ICard card)
    {
        card.gameObject.SetActive(true);
        //card.transform.position = CardPos[hand.Count-1].position;
    }
    private void Shuffle()
    {
        draw.AddRange(discarded);
        discarded.Clear();
        ICard temp;
        for (int i = 0; i < draw.Count; i++)
        {
            temp = draw[i];
            int randomIndex = Random.Range(i, draw.Count);
            draw[i] = draw[randomIndex];
            draw[randomIndex] = temp;
        }
    }
    public void EndTurn()
    {
        turnEnded= true;
        FightController.Instance.EndTurn.Invoke();
    }
}
