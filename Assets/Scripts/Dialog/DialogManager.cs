using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//copy to Map and delete in Dialog
public class DialogManager : Singleton<DialogManager>
{
    private List<DialogData> dialogList;
    [SerializeField] DialogController dialogController;

    private void Awake()
    {
        dialogList = new List<DialogData>(Resources.LoadAll<DialogData>("DialogData"));
        //TriggerEvent();
    }

    private DialogData GetRandomEvent()
    {
        return dialogList[UnityEngine.Random.Range(0, dialogList.Count)];
    }
    public void TriggerEvent()
    {
        dialogController.InitializeDialog(GetRandomEvent());
    }
    public bool EventIsRunning()
    {
        return dialogController.panel.gameObject.activeSelf;
    }

}
