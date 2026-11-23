using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionTrigger : Trigger
{
    Collider coll;


    protected override void  Awake()
    {
        base.Awake();
        coll = GetComponent<Collider>();
    }

    
    void Update()
    {
        CheckCollision();
    }
    protected virtual void CheckCollision()
    {

        List<Collider> colliders = Physics.OverlapBox(transform.position, coll.bounds.extents, transform.rotation).ToList();

        if (colliders.Exists(x => x.gameObject == Player.player.gameObject))
        {
            OnSuccess();
        }
    }
}
