using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleeding : Effect
{
    public override void ApplyEffect(Characteristics target, Characteristics source)
    {
        target.status.bleeding += strength;
        Debug.Log("Apply Bleeding on" + target.characteristicName);
    }
}
