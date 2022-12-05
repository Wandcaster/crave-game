using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class Aggro : Effect
{
    public override void ApplyEffect(Characteristics target, Characteristics source)
    {
        EnemyManager.Instance.targetPriority = source;

        Debug.Log("Apply aggro on" + target.characteristicName);
    }
}
