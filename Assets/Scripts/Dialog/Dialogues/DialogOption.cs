using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogOption
{
    public string playerAnswer;
    public string playerExit="Exit";//in case for custom player's exit talk
    public List<EffectData> effect;//effect[i].ApplyDialogEffect();

}
