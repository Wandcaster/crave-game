using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class Heal : Effect
{
    public override void ApplyEffect(Characteristics target, Characteristics source, int efficiency)
    {
        int heal = efficiency;
        if (target.status.bleeding != 0) heal = (int)(heal * StatusEffects.bleedingEfficiency);
        target.hp=((target.hp + heal > target.maxHp) ? target.maxHp : target.hp + heal);
        Debug.Log("Apply Heal on" + target.characteristicName);
    }
}
