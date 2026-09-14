using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

public class InternMelee : EnemyAll
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

    protected override void Punch()
    {
        base.Punch();
    }

    protected override void Move()
    {
        base.Move();
    }
}
