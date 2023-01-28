using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : NetworkBehaviour
{
    [SerializeField] public GameObject panel;
    [SerializeField] TMPro.TMP_Text initialTalk;
    [SerializeField] TMPro.TMP_Text[] options;
    [SerializeField] TMPro.TMP_Text exitButton;
    [SerializeField] private Characteristics defaultCharacteristic;

    private DialogData currentlyDisplayed;

    private void Start()
    {
        DisableAllButtons();
        gameObject.transform.parent.gameObject.SetActive(false);
    }
    private void DisableAllButtons()
    {
        if (IsHost) return;
        foreach (var buttons in panel.GetComponentsInChildren<Button>())
        {
            buttons.enabled= false;
        }
    }
    public void InitializeDialog(DialogData dd)
    {
        currentlyDisplayed = dd;
        initialTalk.text = dd.initialTalk;
        options[0].text = dd.option1.playerAnswer;
        options[1].text = dd.option2.playerAnswer;
        options[2].text = dd.option3.playerAnswer;
        ShowOptions(true);
        ShowDialog(true);
    }

    public void ShowDialog(bool value)
    {
        panel.transform.parent.gameObject.SetActive(value);
    }
    private void ShowOptions(bool value)
    {
        initialTalk.gameObject.SetActive(value);
        for (int i = 0; i < 3; i++) options[i].transform.parent.gameObject.SetActive(value);
        exitButton.transform.parent.gameObject.SetActive(!value);
    }
    [ServerRpc]
    public void ClickedOptionIDServerRpc(int id)
    {
        ClickedOptionIDClientRpc(id);
    }

    [ClientRpc]
    public void ClickedOptionIDClientRpc(int id)
    {
        DialogOption option=new DialogOption();
        switch (id)
        {
            case 0:
                option = currentlyDisplayed.option1;
                break;
            case 1:
                option = currentlyDisplayed.option2;
                break;
            case 2:
                option = currentlyDisplayed.option3;
                break;


        }
        exitButton.text = option.playerExit;
        ShowOptions(false);

        ApplyEffectsToPlayer(option);


    }

    private void ApplyEffectsToPlayer(DialogOption option)
    {
        //GameObject player = player.
        for(int i=0; i< option.effect.Count; i++)
        {
            option.effect[i].InitAct();
            Debug.Log("PlayerController:"+SessionManager.Instance.player0Controller+"Default charct:" + defaultCharacteristic+"option " + option +"optioneffect "+ option.effect[i]);
            option.effect[i].act.ApplyEffect(SessionManager.Instance.player0Controller, defaultCharacteristic, option.effect[i].strength);
            option.effect[i].act.ApplyEffect(SessionManager.Instance.player1Controller, defaultCharacteristic, option.effect[i].strength);

        }


    }
}
