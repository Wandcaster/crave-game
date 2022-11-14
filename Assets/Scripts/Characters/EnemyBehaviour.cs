using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : Characteristics
{
    [SerializeField] public int damage=1;

    public abstract void DefaultAction();
    public abstract void BattleStartAction();
    public abstract void HpBelow50();


}
