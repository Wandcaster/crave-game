using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class Transfer : Effect
{
    public override void ApplyEffect(Characteristics target, Characteristics source)
    {
        var player = (PlayerController)target;
        player.energy += efficiency;
        Debug.Log("Transfer energy on" + target.characteristicName);
    }
}
