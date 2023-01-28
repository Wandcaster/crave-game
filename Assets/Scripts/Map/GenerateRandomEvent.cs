using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GenerateRandomEvent : MonoBehaviour
{
    public enum EventType : int
    {
        fight = 0,
        fullyrandom = 1,
        rest = 2,
        bossfight = 3
    }
    public int enumSize = 4;
    [SerializeField] public EventType eventType;
    [SerializeField] public List<GameObject> connections;
    [SerializeField] public bool eventClicked;
    [SerializeField] public bool isBossFight = false;



    void Start()
    {
        connections = new List<GameObject>();
        if(!isBossFight)RandomEvent();
    }

    private void OnMouseUpAsButton()
    {
        if (eventClicked == true) return;
        if (DialogManager.Instance.EventIsRunning()) return;

            MapManager.Instance.ChangeCurrentTile(gameObject);
            StartEvent();
    
    }



    //TODO: generowanie eventow po wcisnieciu/zmiana sceny
    private void StartEvent()
    {
        Debug.Log("click"+gameObject.name);
        switch (eventType)
        {
            case EventType.fullyrandom:
                DialogManager.Instance.TriggerEventServerRpc();
                break;
            default:
                Debug.Log("unhandled event type:" + eventType);
                break;
        }
        eventClicked = true;
    }

    private void RandomEvent()
    {
        //if (eventClicked) return;//if eventclicked is true then do not generate another event with different sprite

        int t = UnityEngine.Random.Range(0, enumSize - 1);
        eventType = (EventType) t;//it'll never get us boss fight -> range is: 0 to eventSize-1
        ChangeSprite((EventType)t);

    }


    private void ChangeSprite(EventType type)
    {
        //Debug.Log(type);
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        switch (type)
        {
            case EventType.fight:
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Map/bitwa2");
                break;
            case EventType.fullyrandom:
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Map/znak_zapytania") as Sprite;
                break;
            case EventType.rest:
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Map/odpoczynek") as Sprite;
                break;
            case EventType.bossfight:
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Map/walka_z_bossem3") as Sprite;
                break;
            default:
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Map/start") as Sprite;
                break;
        }


//        spriteRenderer.sprite = newSprite;
    }


    public void AddConnection(GameObject obj)
    {
        connections.Add(obj);
    }



}
