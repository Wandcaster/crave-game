using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : ICard
{
    [SerializeField]
    List<Effect> effect;
    private void OnMouseEnter()
    {
        GetComponent<Animator>().SetBool("MouseHover",true);
    }
    private void OnMouseExit()
    {
        GetComponent<Animator>().SetBool("MouseHover",false);
    }
    private void PlayCard(Characteristics target)
    {
        foreach (var effect in effect)
        {
            effect.ApplyEffect(target);
        }
    }

}
