using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EnemyBehaviour
{
    public List<Effect> Attacks;
    private void Start()
    {
        FightController.Instance.EnemyTurn.AddListener(DefaultAction);
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
}