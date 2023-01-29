using UI;
using UnityEngine;

public class Cleanse : Effect
{
    public override void ApplyEffect(Characteristics target, Characteristics source, int efficiency)
    {
        target.status.bleeding.Value -= efficiency;
        target.status.vulnerability.Value -= efficiency;
        target.status.weakness.Value -= efficiency;

        if (target.status.strength.Value < 0)
        {
            target.status.strength.Value += efficiency;
            target.status.strength.Value = target.status.strength.Value > 0 ? target.status.strength.Value = 0 : target.status.strength.Value;
        }
        target.status.bleeding.Value = target.status.bleeding.Value < 0 ? target.status.bleeding.Value = 0 : target.status.bleeding.Value;
        target.status.vulnerability.Value = target.status.vulnerability.Value < 0 ? target.status.vulnerability.Value = 0 : target.status.vulnerability.Value;
        target.status.weakness.Value = target.status.weakness.Value < 0 ? target.status.weakness.Value = 0 : target.status.weakness.Value;




        Debug.Log("Cleanse" + target);
        //Debug.Log(target+" "+source+" "+efficiency);
    }
}
