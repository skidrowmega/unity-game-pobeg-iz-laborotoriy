using UnityEngine;

public class InternRanged: EnemyAll
{
    void Start()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<CharacterController>();
        }
    }

    void Update()
    {
        Punch();
        Move();
    }

    public override void Punch()
    {
        base.Punch();
    }

    protected override void Move()
    {
        base.Move();
    }
}
