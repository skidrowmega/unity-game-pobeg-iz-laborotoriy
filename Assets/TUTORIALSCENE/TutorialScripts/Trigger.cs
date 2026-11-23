using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trigger : MonoBehaviour
{

    Collider coll;

    [SerializeField] public Stack<Action> callback;

    protected virtual void Awake()
    {
        callback= new Stack<Action>();
        coll = GetComponent<Collider>();
    }

    protected virtual void CheckCollision()
    {

        List<Collider> colliders = Physics.OverlapBox(transform.position, coll.bounds.extents, transform.rotation).ToList();

        if (colliders.Exists(x => x.gameObject == Player.player.gameObject))
        {
            OnSuccess();
        }
    }


    protected virtual void OnSuccess()
    {

    }

    protected virtual void SendCallback()
    {
        while (callback.Count > 0)
        {
            callback.Pop()();
        }
    }

    public virtual void AddCallback(Action action)
    {
        callback.Push(action);
    }


}
