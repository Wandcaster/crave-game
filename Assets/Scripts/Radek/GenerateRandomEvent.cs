using System;
using System.Collections;
using System.Collections.Generic;
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




    void Start()
    {
        connections = new List<GameObject>();
        RandomEvent();
    }

    private void OnMouseUpAsButton()
    {
        if (eventClicked == true) return;

            MapManager.Instance.ChangeCurrentTile(gameObject);
            StartEvent();
    
    }



    //TODO: generowanie eventow po wcisnieciu/zmiana sceny
    private void StartEvent()
    {
        Debug.Log("click"+gameObject.name);
        eventClicked = true;
    }

    private void RandomEvent()
    {

        int t = UnityEngine.Random.Range(0, enumSize - 1);
        eventType = (EventType) t;//it'll never get us boss fight -> range is: 0 to eventSize-1
        ChangeSprite((EventType)t);

    }


    //TODO: podmiana sprite'ow
    private void ChangeSprite(EventType type)
    {
        //Debug.Log(type);
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
//        spriteRenderer.sprite = newSprite;
    }


    public void AddConnection(GameObject obj)
    {
        connections.Add(obj);
    }



}
