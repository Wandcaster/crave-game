using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : NetworkSingleton<EnemyManager>
{
    public List<EnemyController> enemyControllers= new List<EnemyController>();
    [SerializeField] private List<EnemyController> avalableEnemys; 
    public void RespawnEnemys(int count)
    {
        for (int i = 0; i < count; i++)
        {
            enemyControllers.Add(Instantiate(avalableEnemys[Random.Range(0, avalableEnemys.Count)],null));
        }
    }
}
//Przeciwnicy jako scriptableObject
//Button od koñca tury
//Efekty jako scriptableObject
//Karty
//