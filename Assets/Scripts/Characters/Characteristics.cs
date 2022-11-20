using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Characteristics : NetworkBehaviour
{
    public int hp;
    public int maxHp;
    public string name;

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
