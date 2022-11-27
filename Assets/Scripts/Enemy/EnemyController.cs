using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EnemyBehaviour
{
    public EnemyData enemyData;
    public List<Effect> Attacks
    {
        get { return enemyData.Attacks; }
    }
    private void Start()
    {
        InitData();
    }
    public override void BattleStartAction()
    {
        throw new System.NotImplementedException();
    }

    public override void DefaultAction()
    {
        RandomAttack();
    }

    public override void HpBelow50()
    {
        throw new System.NotImplementedException();
    }
    private void RandomAttack()
    {
        Effect effect= Attacks[Random.Range(0,Attacks.Count)];
        Characteristics target = FightController.Instance.playerControllers[Random.Range(0, FightController.Instance.playerControllers.Count)];
        effect.ApplyEffect(target);
    }
    private void InitData()
    {
        damage = enemyData.damage;
        name= enemyData.name;
        maxHp= enemyData.maxHp;
        appearance= enemyData.appearance;
    }
}