using System;
using System.Diagnostics;
using Unity.Netcode;

[Serializable]
public class StatusEffects:NetworkBehaviour
{
    public NetworkVariable <int> _weakness = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);//-25% attack
    public int weakness
    {
        get { return _weakness.Value; }
        set { SetWeaknessServerRpc(value); }
    }
    public NetworkVariable <int> _vulnerability = new NetworkVariable<int>(0);//+50% damage taken
    public int vulnerability
    {
        get { return _vulnerability.Value; }
        set { SetVulnerabilityServerRpc(value); }
    }
    public NetworkVariable <int> _shield = new NetworkVariable<int>(0);//duration is always 1 turn, acts as second hp with higher priority than hp
    public int shield
    {
        get { return _shield.Value; }
        set { SetShieldServerRpc(value); }
    }
    public NetworkVariable <int> _strength = new NetworkVariable<int>(0);//duration is always 1 turn, duration is equal to how strong buff is -> duration 2 = additional 2 points of attack for every attack
    public int strength
    {
        get { return _strength.Value; }
        set { SetStrengthServerRpc(value); }
    }
    public NetworkVariable <int> _bleeding = new NetworkVariable<int>(0);//-25% heal efficiency
    public int bleeding
    {
        get { return _bleeding.Value; }
        set { SetBleedingServerRpc(value); }
    }

    public static float weaknessEfficiency = 0.75f;
    
    public static float vulnerabilityEfficiency = 1.5f;
    
    public static float bleedingEfficiency = 0.75f;

    [ServerRpc(RequireOwnership =false)]
    private void SetWeaknessServerRpc(int newValue)
    {
        Debug.WriteLine("SetWeakness!");
        _weakness.Value = newValue;
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetVulnerabilityServerRpc(int newValue)
    {
        Debug.WriteLine("SetVulnerability!");
        _vulnerability.Value = newValue;
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetShieldServerRpc(int newValue)
    {
        _shield.Value = newValue;
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetStrengthServerRpc(int newValue)
    {
        _strength.Value = newValue;
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetBleedingServerRpc(int newValue)
    {
        _bleeding.Value = newValue;
    }

    public void DecreaseStatuses()
    {
        weakness = (weakness > 0) ? weakness - 1 : 0;
        vulnerability = (vulnerability > 0) ? vulnerability - 1 : 0;
        shield = 0;
        strength = (strength > 0) ? strength - 1 : 0;
        bleeding = (bleeding > 0) ? bleeding - 1 : 0;
    }
    //[ServerRpc(RequireOwnership =false)]
    //public void SendStatusEffectServerRpc(int typeID, int newValue)
    //{
    //    weakness.Value = newValue;
    //    vulnerability.Value = newValue;
    //    shield.Value = newValue;
    //    strength.Value = newValue;
    //    bleeding.Value = newValue;
    //}
}
