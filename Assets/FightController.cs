using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class FightController : NetworkSingleton<FightController>
{
    public UnityEvent HandDraw;
    public UnityEvent EndTurn;
    public UnityEvent EnemyTurn;
    public List<PlayerController> playerControllers;

    private void Start()
    {
        EndTurn.AddListener(CheckEndTurn);
        StartBattle();

    }
    public enum FightStates
	{
		HandDraw,
		PlayerTurn,
		EnemyTurn,
	}
	public FightStates State;
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
}
