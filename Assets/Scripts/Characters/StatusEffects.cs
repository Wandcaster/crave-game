using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class StatusEffects
{
    public int weakness = 0;//-25% attack
    public int vulnerability = 0;//+50% damage taken
    public int shield = 0;//duration is always 1 turn, acts as second hp with higher priority than hp
    public int strength = 0;//duration is always 1 turn, duration is equal to how strong buff is -> duration 2 = additional 2 points of attack for every attack
    public int bleeding = 0;//-25% heal efficiency
    public static float weaknessEfficiency = 0.75f;
    public static float vulnerabilityEfficiency = 1.5f;
    public static float bleedingEfficiency = 0.75f;
    public void DecreaseStatuses()
    {
        weakness = (weakness > 0) ? weakness - 1 : 0;
        vulnerability = (vulnerability > 0) ? vulnerability - 1 : 0;
        shield = 0;
        strength = (strength > 0) ? strength - 1 : 0;
        bleeding = (bleeding > 0) ? bleeding - 1 : 0;
    }
}
