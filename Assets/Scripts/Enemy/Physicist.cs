using UnityEngine;

public class Physicist: EnemyAll
{
    public Physicist()
    {
        healthpoints = 100;
        Speed = 0.01f;
        Damage = 10;
        Reload = 3;
    }

    protected override void TryAttack()
    {
        base.TryAttack();
    }

    public void Inertia()
    {

    }

    public void GravitationalPush()
    {

    }

    public void Stan()
    {

    }
}
