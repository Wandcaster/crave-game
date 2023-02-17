using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UI;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
//TODO dodaæ dontdestroy oraz resetowaæ funkcje FightState
public class GameLoopController : NetworkSingleton<GameLoopController>
{
    public FightStates fightState;
    [SerializeField] EnemyManager enemyManager;
    public PlayerController localPlayer;
    public PlayerController onlinePlayer;
    [SerializeField] FightUIController fightUIController;
    [SerializeField] CardContainer cardContainer;
    private int cardAddCount = 5;
    public string mapSceneName;
    private void Awake()
    {
        SubscribeToUI();
    }
    private void Start()
    {
        if(IsHost)
        {
            localPlayer = SessionManager.Instance.player0Controller;
            onlinePlayer = SessionManager.Instance.player1Controller;
        }
        else
        {
            localPlayer = SessionManager.Instance.player1Controller;
            onlinePlayer = SessionManager.Instance.player0Controller;
        }
        cardContainer.hostCharacter = localPlayer.userCharacterType;
        cardContainer.currentTurn = localPlayer.userCharacterType;
         fightState = FightStates.PlayerTurn;
        FightState().Forget();
    }
    
    public void EndTurn()
    {
        localPlayer.EndTurnServerRpc(true);
        if (onlinePlayer.turnEnded.Value && localPlayer.turnEnded.Value)
        {
            fightUIController.EndTurn();
        }
    }

    private void SubscribeToUI()
    {
        //localPlayer.hp.onChange += (old, newValue) => { fightUIController.SetHp(localPlayer.userCharacterType, newValue, localPlayer.maxHp); };
        //onlinePlayer.hp.onChange += (old, newValue) => { fightUIController.SetHp(onlinePlayer.userCharacterType, newValue, onlinePlayer.maxHp); };
        //localPlayer.energy.onChange += (old, newValue) => { fightUIController.SetEnergy(localPlayer.userCharacterType, newValue); };
        //onlinePlayer.energy.onChange += (old, newValue) => { fightUIController.SetEnergy(onlinePlayer.userCharacterType, newValue); };

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
        while(localPlayer.hp >0&&onlinePlayer.hp >0)
        {
            switch (fightState)
            {
                case FightStates.PlayerTurn:
                    ResetEnergyServerRpc();
                    await KeepAddingCards();
                    await UniTask.WaitUntil(() => onlinePlayer.turnEnded.Value && localPlayer.turnEnded.Value);
                    //EndTurnStatus();
                    //new WaitForSeconds(8);//Wait to send end turn message
                    fightState = FightStates.EnemyTurn;
                    break;
                case FightStates.EnemyTurn:
                    enemyManager.EnemiesActions();
                    cardAddCount = 2;
                    await UniTask.WaitUntil(() => onlinePlayer.turnEnded.Value == localPlayer.turnEnded.Value);
                    ResetTurnStatus();
                    fightState = FightStates.PlayerTurn;
                    localPlayer.status.DecreaseStatuses();
                    break;
            }

        }
        Debug.Log("GameOver");
    }
    [ServerRpc(RequireOwnership =false)]
    private void ResetEnergyServerRpc()
    {
        localPlayer.energy = localPlayer.maxEnergy;
        onlinePlayer.energy = onlinePlayer.maxEnergy;
    }
    private void ResetTurnStatus()
    {
        localPlayer.EndTurnServerRpc(false);
        //onlinePlayer.EndTurnServerRpc(false);
    }
    private void EndTurnStatus()
    {
        localPlayer.EndTurnServerRpc(true);
        onlinePlayer.EndTurnServerRpc(true);
    }
    private async UniTask KeepAddingCards()
    {
        Debug.Log("KeepAddingCard");
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
        MapManager.Instance.SetMapActiveServerRpc(true);
        LoadMapSceneServerRpc(); 
    }
    [ServerRpc(RequireOwnership = false)]
    private void LoadMapSceneServerRpc()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(mapSceneName, LoadSceneMode.Single);
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
