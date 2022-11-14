using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCard : ICard
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
}
