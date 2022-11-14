using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characteristics : MonoBehaviour
{
    [SerializeField] public int hp=1;
    [SerializeField] public int maxHp;
    //    [SerializeField] public int energy;
    //    [SerializeField] public int maxEnergy;

    [SerializeField] public string name;

    private void Start()
    {
        maxHp = hp;
    }

    protected bool IsAlive()
    {
        if (hp > 0) return true;
        return false;
    }


}
