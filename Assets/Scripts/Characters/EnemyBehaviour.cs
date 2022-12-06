using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : Characteristics
{
    public int damage;

    public Sprite appearance;

    public abstract void DefaultAction();
    public abstract void BattleStartAction();
    public abstract void HpBelow50();


}
