using Unity.Netcode;

public class Characteristics : NetworkBehaviour
{
    public int hp;
    public int maxHp;
    public string name;

    //status' duration in turns; pay in mind to calculate anything on target, not this.gameObject. Except CalculateDamage -> first calc damage on attacker, then take damage on defender
    public StatusEffects status;

    public Characteristics() 
    {
        status = new StatusEffects();
        maxHp = hp;
    }
    protected bool IsAlive()
    {
        if (hp > 0) return true;
        return false;
    }
    
    //returns true is target is alive, returns false if target is dead
    public bool TakeDamage(int damage)
    {
        if (status.vulnerability !=0) damage = (int)(damage * StatusEffects.vulnerabilityEfficiency);

        if (status.shield != 0)
        {
            if(status.shield > damage) { status.shield -= damage;}
            else
            {
                damage -= status.shield;
                status.shield = 0;
            }
        }
        hp -= damage;
        return IsAlive();
    }

    //damage is card damage, damageMultiplayer is for enemy additional scaling
    protected int CalculateDamage(int damage, float damageMultiplayer=1)
    {
        damage = (int) (damage*damageMultiplayer);
        damage += status.strength;
        if (status.weakness != 0) damage = (int)(damage * StatusEffects.weaknessEfficiency);

        return damage;
    }
    public void Heal(int heal)
    {
        if (status.bleeding != 0) heal = (int) (heal * StatusEffects.bleedingEfficiency);
        hp = (hp + heal > maxHp) ? maxHp : hp + heal;
    }
}
