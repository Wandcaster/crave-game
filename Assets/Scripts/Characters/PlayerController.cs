using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Characteristics
{
    [SerializeField] public int energy=3;
    [SerializeField] public int maxEnergy;
    [SerializeField] public int drawCardsInHand = 3;

    private void Start()
    {
        maxEnergy = energy;
    }


}
