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
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<EnemyController>() != null && Input.GetButtonUp("Fire1"))
        {
            Debug.Log(other.name);
            PlayCard(other.GetComponent<EnemyController>());
            cardOwner.discarded.Add(this);
            cardOwner.hand.Remove(this);
            gameObject.SetActive(false);
        }
    }

    private void PlayCard(Characteristics target)
    {
        foreach (var effect in effect)
        {
            effect.effect.ApplyEffect(target);
        }
    }
    //Rozbiæ animacje skalowania oraz chowania karty na osobne
    //Dezaktywacja kart które nie zosta³y rzucone
    //
}

