using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EnemyBehaviour
{
    public EnemyData enemyData;
    public List<EffectData> Attacks
    {
        get { return enemyData.Attacks; }
    }
    private void Start()
    {
        InitData();
        foreach (var item in Attacks)
        {
            item.effect = (Effect)Activator.CreateInstance(Type.GetType(item.effectType.ToString()));
            item.effect.efficiency = item.strength;
        }
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
        EffectData effect= Attacks[UnityEngine.Random.Range(0,Attacks.Count)];
        Characteristics target;
        if (EnemyManager.Instance.targetPriority != null) target = EnemyManager.Instance.targetPriority;
        else target = FightController.Instance.playerControllers[UnityEngine.Random.Range(0, FightController.Instance.playerControllers.Count)];

        //Debug.Log(effect);
        //Debug.Log(target.name);
        effect.effect.ApplyEffect(target, this);
    }
    private void InitData()
    {
        damage = enemyData.damage;
        characteristicName= enemyData.enemyName;
        maxHp= enemyData.maxHp;
        appearance= enemyData.appearance;
        transform.localScale = enemyData.scale;
        transform.position = enemyData.position;
        GetComponent<SpriteRenderer>().sprite = enemyData.appearance;
    }
}