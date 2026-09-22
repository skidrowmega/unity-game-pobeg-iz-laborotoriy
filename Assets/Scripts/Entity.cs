using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public enum DamageType
{
    Normal,
    Projectile,
    Unparriable
}
public class Entity : MonoBehaviour
{
    public int healthpoints=100;
    public float Speed = 0.005f;
    public bool IsStunned = false;
    public bool IsBeingPushed = false;
    public const float GlobalPushTime = 0.2f;
    public virtual void Death()
    {
        Destroy(gameObject);
    }
    public virtual void TakeDamage(int damage, Vector3 source ,Entity attacker, float pushstrength,DamageType damageType,float StunTime=0)
    {
        healthpoints -= damage;
        if (healthpoints <= 0) Death();
        if (pushstrength>0)
        {
            PushEntity(source, pushstrength);
        }
        if (pushstrength > 0)
        {
            StunTime = GlobalPushTime;
        }
        if (StunTime > 0)
        {
            IsStunned = true;
            Invoke(nameof(UnStun), StunTime);
        }
    }
    public virtual void PushEntity(Vector3 source, float pushstrength)
    {
        if (IsBeingPushed) return;
        IsBeingPushed= true;
        IsStunned = true;
        StartCoroutine(PushCoroutine(source, pushstrength));
    }
    protected void PushStop()
    {
        IsBeingPushed = false;
        IsStunned = false;
    }

    IEnumerator PushCoroutine(Vector3 source, float pushstrength)
    {
        float timewasted = 0;
        Vector3 pushdirection = (transform.position - source).normalized;
        Vector3 startposition = transform.position;
        Vector3 destination = pushdirection * pushstrength + startposition;
        destination.y=startposition.y;
        while (timewasted < GlobalPushTime)
        {
            transform.position=Vector3.Lerp(transform.position,destination, timewasted/GlobalPushTime);
            timewasted += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        print(timewasted);
        transform.position = destination;
        PushStop();
    }
    public virtual void UnStun()
    {
        IsStunned = false;
    }
}
