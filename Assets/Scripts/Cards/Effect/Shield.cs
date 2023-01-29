using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Effect
{
    public override void ApplyEffect(Characteristics target, Characteristics source, int efficiency)
    {
        target.status.shield += efficiency;
        Debug.Log("Apply Shield on" + target.characteristicName);
    }
}
