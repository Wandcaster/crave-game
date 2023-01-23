using System;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

[Serializable]
public class EffectData
{
    public EffectType effectType;
    /// <summary>
    /// former effect, holds class which contains what Effect does
    /// </summary>
    public Effect act;
    public int strength;
    EffectData() 
    {
        act= (Effect)Activator.CreateInstance(Type.GetType(effectType.ToString()));
    }


}
