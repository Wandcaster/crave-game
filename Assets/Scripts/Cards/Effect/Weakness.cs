using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakness : Effect
{
    public override void ApplyEffect(Characteristics target)
    {
        int tempInCelsius = 0;
        string text=tempInCelsius < 20.0 ? "Cold." : "Perfect!";
        throw new System.NotImplementedException();
    }
}
