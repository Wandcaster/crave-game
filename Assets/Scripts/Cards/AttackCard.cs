using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCard : ICard
{
    [SerializeField]
    List<Effect> effect;
    private void OnMouseEnter()
    {
        GetComponent<Animator>().SetTrigger("MouseEnter");
    
    }
    private void OnMouseExit()
    {
        GetComponent<Animator>().SetTrigger("MouseExit");
    }
}
