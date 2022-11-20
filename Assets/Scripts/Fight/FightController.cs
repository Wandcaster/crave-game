using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class FightController : NetworkSingleton<FightController>
{
    public UnityEvent HandDraw;
    public UnityEvent PlayerTurn;
    public UnityEvent EnemyTurn;

    public UnityEvent EndTurn;
    public List<PlayerController> playerControllers;
    public FightStates FightState;

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
        HandDraw.Invoke();
    }
    public void CheckEndTurn()
    {
        foreach (var player in playerControllers)
        {
            if (player.turnEnded == false) return;
        }
        EnemyTurn.Invoke();
    }

    public void HandDrawEvent()
    {
        FightState = FightStates.HandDraw;
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
