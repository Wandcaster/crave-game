using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        //foreach (var item in Attacks)
        //{
        //    item.act = (Effect)Activator.CreateInstance(Type.GetType(item.effectType.ToString()));

        //}
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
        effect.act.ApplyEffect(target, this, effect.strength);
    }
    private void InitData()
    {
        damage = enemyData.damage;
        characteristicName= enemyData.enemyName;
        maxHp= enemyData.maxHp;
        appearance= enemyData.appearance;
        transform.localScale = enemyData.scale;
        //transform.position = enemyData.position;
        //GetComponent<SpriteRenderer>().sprite = enemyData.appearance;
        hp.Set(maxHp);
    }
    private void OnDisable()
    {
        GameLoopController.Instance.CheckEnemysLife();
    }

}