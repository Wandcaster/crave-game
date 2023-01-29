using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strength : Effect
{
    public override void ApplyEffect(Characteristics target, Characteristics source, int efficiency)
    {
        target.status.strength += efficiency;
        Debug.Log("Apply Strength on" + target.characteristicName);
    }
}
