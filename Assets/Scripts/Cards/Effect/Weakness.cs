using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weakness", menuName = "Cards/Effect/Weakness")]
public class Weakness : Effect
{
    public override void ApplyEffect(Characteristics target)
    {
        Debug.Log("Apply Weakness on"+target.name);
    }
}
