using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

public class InternMelee : EnemyAll
{  
    void Update()
    {
        distanceToPlayer = (player.transform.position - transform.position).magnitude;
        Rotation();
        TryAttack();
        Move();
    }
}
