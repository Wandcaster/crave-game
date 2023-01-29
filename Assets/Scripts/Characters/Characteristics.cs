using PlayerManagement;
using Unity.Netcode;

public class Characteristics : NetworkBehaviour
{
    public PlayableCharacterType userCharacterType;
    public NetworkVariable<int> _hp= new NetworkVariable<int>();
    public int hp 
    {
        get { return _hp.Value; }
        set { SetHPServerRpc(value); }
    }
    [ServerRpc(RequireOwnership =false)]
    private void SetHPServerRpc(int newHpValue)
    {
        _hp.Value= newHpValue;
    }
    public int maxHp;
    public string characteristicName;

    //status' duration in turns; pay in mind to calculate anything on target, not this.gameObject. Except CalculateDamage -> first calc damage on attacker, then take damage on defender
    public StatusEffects status;

    public Characteristics() 
    {
        maxHp = hp;
    }
    public bool IsAlive() {
        return hp> 0;
    }
    
    //returns true is target is alive, returns false if target is dead
    public bool TakeDamage(int damage)
    {
        if (status.vulnerability.Value != 0) damage = (int)(damage * StatusEffects.vulnerabilityEfficiency);

        if (status.shield.Value != 0)
        {
            if(status.shield.Value > damage) { status.shield.Value -= damage;}
            else
            {
                damage -= status.shield.Value;
                status.shield.Value = 0;
            }
        }
        hp =(hp - damage);
        return IsAlive();
    }

    //damage is card damage, damageMultiplayer is for enemy additional scaling
    protected int CalculateDamage(int damage, float damageMultiplayer=1)
    {
        damage = (int) (damage*damageMultiplayer);
        damage += status.strength.Value;
        if (status.weakness.Value != 0) damage = (int)(damage * StatusEffects.weaknessEfficiency);

        return damage;
    }
}
