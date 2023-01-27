using PlayerManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : Characteristics
{
    [SerializeField] public Observable<int> energy=new Observable<int>(3);
    [SerializeField] public int maxEnergy;
    [SerializeField] public int drawCardsInHand = 3;
    public bool turnEnded=false;
    [SerializeField] public List<CardData> deck;//player cards
    [SerializeField] public List<UI.Card> draw;//available to draw
    [SerializeField] public List<UI.Card> hand;//cards in hand
    [SerializeField] public List<UI.Card> discarded;//used/discarded cards
    [SerializeField] private GameObject deckFolder;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private float cardDistance = 3.0f;
    [SerializeField] private float cardPositionY = -4;//middle card position

    private void Awake()
    {
        energy.Set(maxEnergy);
        hp.Set(maxHp);
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        InitForFightScene();
    }

    private void InitForFightScene()
    {
        //draw = new List<UI.Card>();
        //for (int i = 0; i < deck.Count; i++)
        //{
        //    GameObject temp = Instantiate(cardPrefab, deckFolder.transform);
        //    UI.Card tempCard= temp.GetComponentInChildren<UI.Card>();
        //    draw.Add(tempCard);
        //    draw.LastOrDefault().cardData = deck[i];
        //    //Uzupe³niæ opis kart
        //}
        FightController.Instance.HandDraw.AddListener(DrawCard);
        FightController.Instance.EndTurn.AddListener(EndTurn);
    }

    public void DrawCard()
    {
        turnEnded = false;
        energy.Set(maxEnergy);
        for (int i = 0; i < hand.Count; i++) hand[i].gameObject.SetActive(false);

        discarded.AddRange(hand);
        hand.Clear();
        for (int i = 0; i < drawCardsInHand; i++)
        {
            Draw();
        }
        //UStaw pozycje kart
    }



    private void Draw()
    {
        if (draw.Count == 0) Shuffle();
        int cardID = UnityEngine.Random.Range(0, draw.Count);
        UI.Card newCard = draw[cardID];
        hand.Add(newCard);
        draw.Remove(newCard);
        //Wyœwietl karte
    }

    private void Shuffle()
    {
        draw.AddRange(discarded);
        discarded.Clear();
        UI.Card temp;
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
    }
}
