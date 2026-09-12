using UnityEngine;

public class Entity
{
    public int healthpoints=1;
    public float Speed = 0.005f;
    public virtual void Death()
    {

    }
    public TakeDamage(int damage)
    {
        healthpoints -= damage;
        if (healthpoints <= 0) Death();
    }
}
