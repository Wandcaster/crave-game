using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : NetworkSingleton<EnemyManager>
{
    public List<EnemyController> enemyControllers= new List<EnemyController>();
    [SerializeField] private List<EnemyController> avalableEnemys;
    private void Start()
    {
        FightController.Instance.EnemyTurn.AddListener(EnemysActions);

    }
    public void RespawnEnemys(int count)
    {
        for (int i = 0; i < count; i++)
        {
            enemyControllers.Add(Instantiate(avalableEnemys[Random.Range(0, avalableEnemys.Count)],null));
        }
    }
    private void EnemysActions()
    {
        foreach (var enemy in enemyControllers)
        {
            enemy.DefaultAction();
        }
        FightController.Instance.HandDraw.Invoke();
        FightController.Instance.PlayerTurn.Invoke();

    }
}
//Karty
//