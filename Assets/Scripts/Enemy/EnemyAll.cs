using UnityEngine;

public class EnemyAll: Entity
{
    public EnemyAll()
    {
        healthpoints = 20;
        Speed = 0.006f;
    }
    public int Damage = 5;
    public int Reload = 1;

    public virtual void Punch()//во время атаки он не может двигаться пока не закончимтся анимация
    {

    }
}
