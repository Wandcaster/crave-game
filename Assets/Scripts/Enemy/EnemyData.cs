using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int damage = 1;
    public Sprite appearance;
    public int hp;
    public int maxHp;
    public string enemyName;
    public List<EffectData> Attacks;
    public Vector2 scale;
    public Vector2 position;

}
