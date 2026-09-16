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
        Rotation();
        TryAttack();
        Move();
    }
}
