using System.Collections.Generic;
using UI;
using Unity.Netcode;
using UnityEngine;

public class EnemyManager : NetworkSingleton<EnemyManager>
{
    private int enemyCount = 1;
    public List<EnemyController> enemyControllers= new List<EnemyController>();
    [SerializeField] private List<EnemyData> avalableEnemys;
    [SerializeField] public Characteristics targetPriority=null;
    [SerializeField] private EnemyCupboard enemyCupboard;
    private void Start()
    {
        RespawnEnemies(enemyCount);
        FightController.Instance.EnemyTurn.AddListener(EnemiesActions);

    }
    public void RespawnEnemies(int count)
    {
        if (!SessionManager.Instance.GetComponent<NetworkObject>().IsOwner) return;
        for (int i = 0; i < count; i++)
        {
            enemyControllers.Add(enemyCupboard.AddEnemy(avalableEnemys[Random.Range(0, avalableEnemys.Count)]));
        }
    }
    public void EnemiesActions()
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