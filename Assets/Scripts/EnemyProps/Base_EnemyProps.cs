using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_EnemyProps : MonoBehaviour
{
    protected int damage;
    protected int elementDamage;

    public virtual void Initialie(int dmg, int elementDmg)
    {
        damage = dmg;
        elementDamage = elementDmg;
    }
}
