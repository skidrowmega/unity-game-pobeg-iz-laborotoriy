using UnityEngine;

public class InternRanged: EnemyAll
{
    public InternRanged()
    {
        healthpoints = 10;
        Speed = 0.005f;
        Damage = 10;
    }

    protected override void Punch()
    {
        base.Punch();
    }
}
