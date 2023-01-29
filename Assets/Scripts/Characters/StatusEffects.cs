using System;
using Unity.Netcode;

[Serializable]
public class StatusEffects:NetworkBehaviour
{
    public NetworkVariable <int> weakness = new NetworkVariable<int>(0);//-25% attack
    public NetworkVariable <int> vulnerability = new NetworkVariable<int>(0);//+50% damage taken
    public NetworkVariable <int> shield = new NetworkVariable<int>(0);//duration is always 1 turn, acts as second hp with higher priority than hp
    public NetworkVariable <int> strength = new NetworkVariable<int>(0);//duration is always 1 turn, duration is equal to how strong buff is -> duration 2 = additional 2 points of attack for every attack
    public NetworkVariable <int> bleeding = new NetworkVariable<int>(0);//-25% heal efficiency
    public static float weaknessEfficiency = 0.75f;
    public static float vulnerabilityEfficiency = 1.5f;
    public static float bleedingEfficiency = 0.75f;
    public void DecreaseStatuses()
    {
        weakness.Value = (weakness.Value > 0) ? weakness.Value - 1 : 0;
        vulnerability.Value = (vulnerability.Value > 0) ? vulnerability.Value - 1 : 0;
        shield.Value = 0;
        strength.Value = (strength.Value > 0) ? strength.Value - 1 : 0;
        bleeding.Value = (bleeding.Value > 0) ? bleeding.Value - 1 : 0;
    }
}
