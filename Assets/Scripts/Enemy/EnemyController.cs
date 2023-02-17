using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : EnemyBehaviour
{
    public EnemyData enemyData;
    public EnemyData bossData;
    public List<EffectData> Attacks
    {
        get { return enemyData.Attacks; }
    }
    private void Start()
    {
        if (SessionManager.Instance.bossFight.Value)InitData(bossData);
        else InitData(enemyData);
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
        Characteristics target=null;
        if (EnemyManager.Instance.targetPriority != null) target = EnemyManager.Instance.targetPriority;
        else
        {
            switch (UnityEngine.Random.Range(0,2))
            {
                case 0:
                    target = SessionManager.Instance.player0Controller;
                    break;
                case 1:
                    target= SessionManager.Instance.player1Controller;
                    break;
            }
        }
        //Debug.Log(effect);
        //Debug.Log(target.name);
        effect.act.ApplyEffect(target, this, effect.strength*enemyData.damage);
    }
    private void InitData(EnemyData enemy)
    {
        damage = enemy.damage;
        characteristicName= enemy.enemyName;
        maxHp= enemy.maxHp;
        appearance= enemy.appearance;
        transform.localScale = enemy.scale;
        //transform.position = enemyData.position;
        GetComponent<SpriteRenderer>().sprite = enemy.appearance;
        if (SessionManager.Instance.bossFight.Value) gameObject.transform.localScale = enemy.scale;

        hp = (maxHp);
    }
    private void OnDisable()
    {
        GameLoopController.Instance.CheckEnemysLife();
    }

}