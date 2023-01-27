using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoopController : Singleton<GameLoopController>
{
    public FightStates fightState;
    [SerializeField] EnemyManager enemyManager;
    public PlayerController localPlayer;
    public PlayerController onlinePlayer;
    [SerializeField] FightUIController fightUIController;
    private int cardAddCount = 5;
    public string mapSceneName;
    private void Awake()
    {
        SubscribeToUI();
    }
    private void Start()
    {
        fightState = FightStates.PlayerTurn;
        FightState().Forget();
    }

    private void SubscribeToUI()
    {
        localPlayer.hp.onChange += (old, newValue) => { fightUIController.SetHp(localPlayer.userCharacterType, newValue, localPlayer.maxHp); };
        onlinePlayer.hp.onChange += (old, newValue) => { fightUIController.SetHp(onlinePlayer.userCharacterType, newValue, onlinePlayer.maxHp); };
        localPlayer.energy.onChange += (old, newValue) => { fightUIController.SetEnergy(localPlayer.userCharacterType, newValue); };
        onlinePlayer.energy.onChange += (old, newValue) => { fightUIController.SetEnergy(onlinePlayer.userCharacterType, newValue); };

        fightUIController.OnCardPlayed += (card, target, source) =>
        {
            /*
             * This is only executed if the card is usable in the first place - if there isnt enough energy
             * or if the target isnt suitable, OnCardPLayed will not be called
             */

            card.PlayCard(target.GetComponent<Characteristics>(), source);
            Debug.Log($"Played card {card.cardData.cardName} against {target} who is {LayerMask.LayerToName(target.layer)}");
        };
    }

    public async UniTask FightState()
    {
        while(localPlayer.hp.Get()>0&&onlinePlayer.hp.Get()>0)
        {
            switch (fightState)
            {
                case FightStates.PlayerTurn:
                    await KeepAddingCards();
                    fightState = FightStates.EnemyTurn;
                    await UniTask.WaitUntil(() => onlinePlayer.turnEnded && localPlayer.turnEnded);
                    localPlayer.status.DecreaseStatuses();
                    onlinePlayer.status.DecreaseStatuses();
                    break;
                case FightStates.EnemyTurn:
                    enemyManager.EnemiesActions();
                    cardAddCount = 2;
                    fightState = FightStates.PlayerTurn;
                    break;
            }

        }
        Debug.Log("GameOver");
    }
    private async UniTask KeepAddingCards()
    {
        var random = new System.Random();
        for (int i = 0; i < cardAddCount; ++i)
        {
            var list = new List<CardData> {
                    localPlayer.deck[random.Next(localPlayer.deck.Count)],
                    localPlayer.deck[random.Next(localPlayer.deck.Count)],
                    localPlayer.deck[random.Next(localPlayer.deck.Count)]
                };
            var c = await fightUIController.DrawCards(list);
            await fightUIController.AddToHand(c);
        }
    }
    private void LoadMapScene()
    {
        SceneManager.LoadScene(mapSceneName);
    }
    public void CheckEnemysLife()
    {
        foreach (var enemy in enemyManager.enemyControllers)
        {
            if (enemy.gameObject.activeSelf) return;
        }
        LoadMapScene();
    }
    }
