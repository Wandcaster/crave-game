using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : ICard
{
    [SerializeField]
    List<Effect> effect;
    [SerializeField]
    PlayerController cardOwner;
    private void Start()
    {
        cardOwner=FindObjectOfType<PlayerController>();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<EnemyController>() != null && Input.GetButtonUp("Fire1"))
        {
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
            effect.ApplyEffect(target);
        }
    }
    //Rozbiæ animacje skalowania oraz chowania karty na osobne
    //Dezaktywacja kart które nie zosta³y rzucone
    //
}
