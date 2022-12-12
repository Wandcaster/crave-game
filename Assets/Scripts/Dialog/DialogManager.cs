using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//copy to Map and delete in Dialog
public class DialogManager : Singleton<DialogManager>
{
    private List<DialogData> dialogList;

    private void Awake()
    {
        dialogList = new List<DialogData>(Resources.LoadAll<DialogData>("DialogData"));
    }

    public DialogData GetRandomEvent()
    {
        return dialogList[UnityEngine.Random.Range(0, dialogList.Count)];
    }


}
