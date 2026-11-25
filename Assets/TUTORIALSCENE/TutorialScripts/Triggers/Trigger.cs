using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trigger : MonoBehaviour
{

    [SerializeField] bool DestroyObjectOnTrigger=true;

    [SerializeField] public Stack<Action> callback;

    protected virtual void Awake()
    {
        callback= new Stack<Action>();
        
    }

    protected virtual void OnSuccess()
    {
        SendCallback();
    }

    protected virtual void SendCallback()
    {
        while (callback.Count > 0)
        {
            callback.Pop()();
        }
        if(DestroyObjectOnTrigger)Destroy(gameObject);
    }

    public virtual void AddCallback(Action action)
    {
        callback.Push(action);
    }


}
