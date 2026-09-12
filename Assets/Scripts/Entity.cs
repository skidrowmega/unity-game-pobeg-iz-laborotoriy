using JetBrains.Annotations;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int healthpoints=100;
    public float Speed = 0.005f;
    public bool IsStunned = false;
    public bool IsBeingPushed = false;
    public float PushTime = 0;
    public Vector3 pushsource = Vector3.zero;
    public const int GlobalPushStrength = 3;
    public float pushstart=0;
    public virtual void Death()
    {

    }
    public void TakeDamage(int damage, Vector3 source, float pushtime)
    {
        healthpoints -= damage;
        if (healthpoints <= 0) Death();
        if (pushtime>0)
        {
            PushEntity(source, pushtime);
        }
    }
    public void PushPerFrame()
    {
        print("HI");
        if (IsBeingPushed) {
            if (Time.time - pushstart > PushTime)
            {
                PushStop();
            }
            else
                //transform.position = Vector3.MoveTowards(transform.position, pushsource, Time.deltaTime * GlobalPushStrength);
                transform.position += (transform.position- pushsource) * Time.deltaTime * GlobalPushStrength;
        }
    }
    public void PushEntity(Vector3 source, float pushtime)
    {
        if (IsBeingPushed) return;
        IsBeingPushed= true;
        pushsource= source;
        pushsource.y=transform.position.y;
        PushTime= pushtime;
        IsStunned= true;
        pushstart = Time.time;
    }
    private void PushStop()
    {
        IsBeingPushed = false;
        pushsource = Vector3.zero;
        PushTime = 0;
        IsStunned = false;
        pushstart = 0;
    }



}
