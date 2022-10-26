using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[Serializable]
public class MyOwnEvent : UnityEvent { }
public class OnClick : MonoBehaviour,IPointerClickHandler
{
    public MyOwnEvent myOwnEvent;
    public void OnPointerClick(PointerEventData eventData)
    {
        myOwnEvent.Invoke();
    }

}
