using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class Transfer : Effect
{
    public override void ApplyEffect(Characteristics target, Characteristics source, int efficiency)
    {
        PlayerController player = (PlayerController)target;
        player.energy =(player.energy + efficiency);
        FightUIController.instance.SetEnergy(player.userCharacterType, player.energy);

        Debug.Log("Transfer energy on" + target.characteristicName);
    }
}
