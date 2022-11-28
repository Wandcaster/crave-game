using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Card : ICard
{
    [SerializeField]
    public List<EffectData> effect;
    [SerializeField]
    PlayerController cardOwner;
    private void Start()
    {
        cardOwner=FindObjectOfType<PlayerController>();
        foreach (var item in effect)
        {
            item.effect = (Effect)Activator.CreateInstance(Type.GetType(item.effectType.ToString()));
            item.effect.strength = item.strength;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log(Input.GetMouseButton(0));
        if (other.GetComponent<EnemyController>() != null && !Input.GetMouseButton(0))
        {
            Debug.Log(other.name);
            PlayCard(other.GetComponent<EnemyController>());
            cardOwner.discarded.Add(this);
            cardOwner.hand.Remove(this);
            gameObject.SetActive(false);
            cardOwner.RepositionCards();
        }
    }


    

    private void PlayCard(Characteristics target)
    {
        foreach (var effect in effect)
        {
            effect.effect.ApplyEffect(target, cardOwner);
        }
    }
    //Rozbiæ animacje skalowania oraz chowania karty na osobne
    //Dezaktywacja kart które nie zosta³y rzucone
    //
}

