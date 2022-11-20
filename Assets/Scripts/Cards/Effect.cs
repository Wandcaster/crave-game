using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect:ScriptableObject
{
    public int duration;
    public virtual void ApplyEffect(Characteristics target) {
        throw new System.NotImplementedException();
    }
}
