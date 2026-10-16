using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEditor;

public enum DamageType
{
    Normal,
    Projectile,
    Unparriable
}
public class Entity : MonoBehaviour
{
    public float HealthPoints=100;
    public float MaxHealthpoints = 100;
    public float Speed = 0.005f;
    public bool IsStunned = false;
    public bool IsBeingPushed = false;
    public const float GlobalPushTime = 0.2f;

    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }
    public virtual void TakeDamage(float damage, Vector3 source ,Entity attacker, float pushstrength,DamageType damageType,float StunTime=0)
    {
        HealthPoints -= damage;
        if (HealthPoints <= 0) Death();
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
        if (agent)
        {
            agent.isStopped = true;
        }
        StartCoroutine(PushCoroutine(source, pushstrength));
    }
    protected void PushStop()
    {
        if (agent)
        {
            agent.isStopped = false;
        }
        IsBeingPushed = false;
        IsStunned = false;
    }

    IEnumerator PushCoroutine(Vector3 source, float pushstrength)
    {
        float timewasted = 0;

        Vector3 startposition = transform.position;

        source.y = startposition.y;

        Vector3 pushdirection = (startposition - source).normalized;
        




        Ray ray = new Ray(startposition, pushdirection);
        RaycastHit hit;
        bool ishit = Physics.Raycast(ray, out hit, pushstrength);
        Debug.DrawRay(startposition, pushdirection*pushstrength);
        
        float newpushstrength = 0;

        if (ishit)
        {
            //newdodgedistance = ((hit.point - (ray.direction * 0.5f)) - ray.origin).magnitude;
            if (hit.distance < 0.5) newpushstrength = 0;
            else
                newpushstrength = ((hit.point - (ray.direction * .7f)) - ray.origin).magnitude;
        }
        if (newpushstrength>0) pushstrength = newpushstrength;
        Vector3 destination = pushdirection * pushstrength + startposition;


        while (timewasted < GlobalPushTime)
        {
            transform.position=Vector3.Lerp(startposition,destination, timewasted/GlobalPushTime);
            timewasted += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //print(timewasted);
        //transform.position = destination;
        PushStop();
    }
    public virtual void UnStun()
    {
        IsStunned = false;
    }
}
