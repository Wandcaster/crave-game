using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strength : Effect
{
    public override void ApplyEffect(Characteristics target, Characteristics source)
    {
        target.status.strength += strength;
        Debug.Log("Apply Strength on" + target.characteristicName);
    }
}
