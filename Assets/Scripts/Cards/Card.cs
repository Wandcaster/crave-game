using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Card : ICard
{
    [SerializeField]
    public List<EffectData> effects
    {
        get { return cardData.effect; }
        //set { cardData.effect = value; }
    }
    [SerializeField]
    PlayerController cardOwner;
    private void Start()
    {
        cardOwner=FindObjectOfType<PlayerController>();
        foreach (var effect in effects)
        {
            effect.act = (Effect)Activator.CreateInstance(Type.GetType(effect.effectType.ToString()));
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log(Input.GetMouseButton(0));
        if (other.GetComponent<EnemyController>() != null && !Input.GetMouseButton(0))
        {
            Debug.Log(other.name);
            PlayCard(other.GetComponent<EnemyController>());
            cardOwner.RepositionCards();
        }
    }
    private void PlayCard(Characteristics target)
    {
        Debug.Log("Energy" + cardOwner.energy + "|:" + useCosts);
        if (cardOwner.energy - useCosts < 0) return;
        cardOwner.energy -= useCosts;

        foreach (var effect in effects)
        {
            effect.act.ApplyEffect(target, cardOwner, effect.strength);
        }
        cardOwner.discarded.Add(this);
        cardOwner.hand.Remove(this);
        gameObject.SetActive(false);
    }
    //Rozbiæ animacje skalowania oraz chowania karty na osobne
    //Dezaktywacja kart które nie zosta³y rzucone
    //
}

