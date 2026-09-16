using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

public class InternMelee : EnemyAll
{  

    void Update()
    {
        Rotation();
        TryAttack();
        Move();
    }
}
