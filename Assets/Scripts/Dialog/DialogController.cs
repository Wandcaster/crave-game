using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    [SerializeField] public GameObject panel;
    [SerializeField] TMPro.TMP_Text initialTalk;
    [SerializeField] TMPro.TMP_Text[] options;
    [SerializeField] TMPro.TMP_Text exitButton;
    [SerializeField] private Characteristics defaultCharacteristic;

    private DialogData currentlyDisplayed;

    private void Start()
    {
        
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
        panel.SetActive(value);
    }
    private void ShowOptions(bool value)
    {
        initialTalk.gameObject.SetActive(value);
        for (int i = 0; i < 3; i++) options[i].transform.parent.gameObject.SetActive(value);
        exitButton.transform.parent.gameObject.SetActive(!value);
    }
    public void ClickedOptionID(int id)
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
            Debug.Log("PlayerController:"+SessionManager.Instance.player0Controller+"Default charct:" + defaultCharacteristic+"option " + option +"optioneffect "+ option.effect[i]);
            option.effect[i].act.ApplyEffect(SessionManager.Instance.player0Controller, defaultCharacteristic, option.effect[i].strength);
            option.effect[i].act.ApplyEffect(SessionManager.Instance.player1Controller, defaultCharacteristic, option.effect[i].strength);

        }


    }
}
