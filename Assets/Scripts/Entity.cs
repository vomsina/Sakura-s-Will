using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float health = 100;

    public virtual void GetDamage()
    {
        health-=10;
        if (health <= 0) Die();

    }

    public virtual void Die()
    {
        Destroy(this.gameObject);
    }

}
