using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogData", menuName = "Dialogs/DialogData")]
public class DialogData : ScriptableObject
{
    public string initialTalk;//start dialog with this
    public DialogOption option1;
    public DialogOption option2;
    public DialogOption option3;
}
