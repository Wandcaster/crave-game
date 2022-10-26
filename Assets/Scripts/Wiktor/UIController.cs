using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UIController :MonoBehaviour
{
    [SerializeField]
    CanvasGroup MainPanel;
    [SerializeField]
    CanvasGroup SelectPanel;
    [SerializeField]
    NetworkManager networkManager;
    private void Start()
    {
       networkManager.OnClientConnectedCallback += ServerStart;
       networkManager.OnClientDisconnectCallback += ServerStop;
    }
    private void ServerStart(ulong id)
    {
        SetCanvasGroup(SelectPanel,true);
        SetCanvasGroup(MainPanel, false);
        Debug.Log(id);
    }
    private void ServerStop(ulong id)
    {
        SetCanvasGroup(SelectPanel, false);
        SetCanvasGroup(MainPanel, true);
        Debug.Log(id);
    }
    private void SetCanvasGroup(CanvasGroup group,bool value)
    {
        group.blocksRaycasts = value;
        group.interactable = value;
        if (value) group.alpha = 1;
        else group.alpha = 0;
    }
}
