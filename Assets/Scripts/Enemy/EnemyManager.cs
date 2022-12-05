using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : NetworkSingleton<EnemyManager>
{
    public List<EnemyController> enemyControllers= new List<EnemyController>();
    [SerializeField] private List<EnemyController> avalableEnemys;
    [SerializeField] public Characteristics targetPriority=null;
    private void Start()
    {
        RespawnEnemies(1);
        FightController.Instance.EnemyTurn.AddListener(EnemiesActions);

    }
    public void RespawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            enemyControllers.Add(Instantiate(avalableEnemys[Random.Range(0, avalableEnemys.Count)],null));
        }
    }
    private void EnemiesActions()
    {
        foreach (var enemy in enemyControllers)
        {
            enemy.DefaultAction();
        }
        foreach (var enemy in enemyControllers)
        {
            enemy.status.DecreaseStatuses();
        }
        targetPriority = null;
        FightController.Instance.HandDraw.Invoke();
        FightController.Instance.PlayerTurn.Invoke();

    }
}
//Karty
//