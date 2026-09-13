using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
public class Entity : MonoBehaviour
{
    public int healthpoints=100;
    public float Speed = 0.005f;
    public bool IsStunned = false;
    public bool IsBeingPushed = false;
    public const float GlobalPushTime = 0.2f;
    public virtual void Death()
    {

    }
    public virtual void TakeDamage(int damage, Entity source, float pushstrength)
    {
        healthpoints -= damage;
        if (healthpoints <= 0) Death();
        if (pushstrength>0)
        {
            PushEntity(source.transform.position, pushstrength);
        }
    }
    public void PushPerFrame()
    {
        if (IsBeingPushed) {
            //if (Time.time - pushstart > PushStrength)
            //{
            //    PushStop();
            //}
            //else
            //    //transform.position = Vector3.MoveTowards(transform.position, pushsource, Time.deltaTime * GlobalPushStrength);
            //    transform.position += (transform.position- pushsource) * Time.deltaTime * GlobalPushStrength;
        }
    }
    public void PushEntity(Vector3 source, float pushstrength)
    {
        if (IsBeingPushed) return;
        IsBeingPushed= true;
        IsStunned = true;
        StartCoroutine(PushCoroutine(source, pushstrength));
    }
    private void PushStop()
    {
        IsBeingPushed = false;
        IsStunned = false;
    }

    IEnumerator PushCoroutine(Vector3 source, float pushstrength)
    {
        float timewasted = 0;
        Vector3 pushdirection = (transform.position - source).normalized;
        Vector3 startposition = transform.position;
        while (timewasted < GlobalPushTime)
        {
            transform.position=Vector3.Lerp(transform.position, pushdirection*pushstrength+startposition, timewasted );
            timewasted += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = pushdirection * pushstrength + startposition;
        PushStop();
    }

}
