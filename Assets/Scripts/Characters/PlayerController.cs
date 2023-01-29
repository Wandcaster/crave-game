using PlayerManagement;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
using Unity.Netcode;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Characteristics
{
    [SerializeField] public NetworkVariable<int> _energy = new NetworkVariable<int>();
    public int energy
    {
        get { return _energy.Value; }
        set { SetEnergyServerRpc(value); }
    }
    [SerializeField] public int maxEnergy;
    [SerializeField] public int drawCardsInHand = 3;
    public NetworkVariable <bool> turnEnded= new NetworkVariable<bool>(false);
    [SerializeField] public List<CardData> deck;//player cards
    [SerializeField] public List<UI.Card> draw;//available to draw
    [SerializeField] public List<UI.Card> hand;//cards in hand
    [SerializeField] public List<UI.Card> discarded;//used/discarded cards
    [SerializeField] private GameObject deckFolder;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private float cardDistance = 3.0f;
    [SerializeField] private float cardPositionY = -4;//middle card position

    [ServerRpc(RequireOwnership =false)]
    public void SetEnergyServerRpc(int newValue)
    {
        _energy.Value= newValue;
    }
    private void Awake()
    {
        SubscribeToUI();
        
    }
    [ServerRpc(RequireOwnership =false)]
    public void SetOwnershipServerRpc(ulong value)
    {
        GetComponent<NetworkObject>().ChangeOwnership(value);
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        energy = (maxEnergy);
        hp = (maxHp);
        _hp.OnValueChanged.Invoke(0, maxHp);
        _energy.OnValueChanged.Invoke(0, maxEnergy);
        if (!IsHost) SetOwnershipServerRpc(1);
    }

    public void SubscribeToUI()
    {
        _hp.OnValueChanged+= (old, newValue) => { SetHp( newValue, maxHp); };
        _energy.OnValueChanged += (old, newValue) => { SetEnergy(newValue); };
    }
    public void SetHp(int currentHp, int maxHp)
    {
        var hpBar = transform.GetComponentInChildren<HealthBar>();
        hpBar.SetHp(currentHp, maxHp);
    }
    public void SetEnergy(int newValue)
    {
        var txt = transform.GetComponentInChildren<TMP_Text>();
        txt.SetText(newValue.ToString());
    }

    public void DrawCard()
    {
        turnEnded.Value = false;
        energy = (maxEnergy);
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
    [ServerRpc(RequireOwnership = false)]
    public void EndTurnServerRpc( bool newValue)
    {
        turnEnded.Value= newValue;
    }
}
