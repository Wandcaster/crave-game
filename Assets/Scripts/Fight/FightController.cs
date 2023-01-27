using System.Collections;
using System.Collections.Generic;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
//Dobór Kart
//Tura gracza 
//Tura przeciwnika

public class FightController : NetworkSingleton<FightController>
{
    public UnityEvent HandDraw;
    public UnityEvent PlayerTurn;
    public UnityEvent EnemyTurn;

    public UnityEvent EndTurn;
    public List<PlayerController> playerControllers;
    public FightStates FightState;

    [SerializeField] FightUIController fightUIController;
    private void Start()
    {
        EndTurn.AddListener(CheckEndTurn);
        HandDraw.AddListener(HandDrawEvent);
        PlayerTurn.AddListener(PlayerTurnEvent);
        EnemyTurn.AddListener(EnemyTurnEvent);

        StartBattle();
    }
    private void StartBattle()
    {
        fightUIController.OnEndTurn += EndTurnMethod;
        HandDraw.Invoke();
        PlayerTurn.Invoke();
    }
    public void EndTurnMethod()
    {
        EndTurn.Invoke();
    }
    public void CheckEndTurn()
    {
        foreach (var player in playerControllers)
        {
            if (player.turnEnded == false) return; //Zmieniaæ sieciowo bool na graczach
        }
        foreach (var player in playerControllers) player.status.DecreaseStatuses();
        EnemyTurn.Invoke();
    }

    public void HandDrawEvent()
    {
        //FightState = FightStates.HandDraw;
    }
    public void PlayerTurnEvent()
    {
        FightState = FightStates.PlayerTurn;
    }
    public void EnemyTurnEvent()
    {
        FightState = FightStates.EnemyTurn;
    }
}
