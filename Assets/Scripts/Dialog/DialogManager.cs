using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

//copy to Map and delete in Dialog
public class DialogManager : NetworkSingleton<DialogManager>
{
    [SerializeField]
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
    [ServerRpc]
    public void TriggerEventServerRpc()
    {
        TriggerEventClientRPC();
    }
    [ClientRpc]
    public void TriggerEventClientRPC()
    {
        dialogController.InitializeDialog(GetRandomEvent());
    }
    public bool EventIsRunning()
    {
        return dialogController.panel.transform.parent.gameObject.activeSelf;
    }

}
