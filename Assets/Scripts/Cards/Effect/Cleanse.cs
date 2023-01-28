using UI;
using UnityEngine;

public class Cleanse : Effect
{
    public override void ApplyEffect(Characteristics target, Characteristics source, int efficiency)
    {
        target.status.bleeding = -efficiency;
        target.status.vulnerability= -efficiency;
        target.status.weakness= -efficiency;

        if (target.status.strength < 0)
        {
            target.status.strength += efficiency;
            target.status.strength = target.status.strength > 0 ? target.status.strength = 0 : target.status.strength;
        }
        target.status.bleeding = target.status.bleeding < 0 ? target.status.bleeding = 0 : target.status.bleeding;
        target.status.vulnerability = target.status.vulnerability < 0 ? target.status.vulnerability = 0 : target.status.vulnerability;
        target.status.weakness = target.status.weakness < 0 ? target.status.weakness = 0 : target.status.weakness;




        Debug.Log("Cleanse" + target);
        //Debug.Log(target+" "+source+" "+efficiency);
    }
}
