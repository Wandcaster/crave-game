using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "Weakness", menuName = "Cards/Effect/Weakness")]
public class Weakness : Effect
{
    public override void ApplyEffect(Characteristics target)
    {
        target.status.weakness += duration;
        Debug.Log("Apply Weakness on"+target.name);
    }
}
