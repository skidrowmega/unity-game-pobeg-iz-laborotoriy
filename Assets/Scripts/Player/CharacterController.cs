using UnityEditor;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.Assemblies;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class CharacterController : Entity
{
    public Transform Cursor;
    Vector3 CursorVector;
    Animator animator;
    public WeaponMelee currentweapon;





    private void Awake()
    {
        animator= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        LookAtCursor();
        Attack();
        PushPerFrame();
        if (Input.GetMouseButtonUp(1))
        {
            PushEntity(new Vector3(5, 1, 7), 0.3f);
        }
    }

    void Attack()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 2.982696f);
        if (!Input.GetMouseButtonDown(0)) return;
        if (animator == null) return;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("VESLOBASHLEFT")) return;
        animator.Play("VESLOBASHLEFT");
        //StartCoroutine(MeleeAttackLoop(.41f));
        StartCoroutine(MeleeAttackLoop(animator.GetCurrentAnimatorStateInfo(0).length));
        print("current animation length: " + animator.GetCurrentAnimatorStateInfo(0).length);
    }
    void Move()
    {
        if (IsStunned) return;
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Speed;
    }
    void LookAtCursor()
    {
        CursorVector = Cursor.position - transform.position;
        transform.rotation = Quaternion.Euler(0, Mathf.Rad2Deg * Mathf.Atan2(-CursorVector.z, CursorVector.x), 0);
    }

    IEnumerator MeleeAttackLoop(float secondstowait)
    {
        float starttime= Time.time;
        List<Collider> checkedcolliders = new List<Collider>();
        while (Time.time - starttime < secondstowait)
        {
            foreach (Collider collider in Physics.OverlapCapsule(transform.position, transform.position + Vector3.up, 2.482696f))
            {
                Entity potentialenemy = collider.GetComponent<Entity>();
                if (checkedcolliders.Contains(collider)) continue;
                checkedcolliders.Add(collider);
                if (potentialenemy && !(potentialenemy.GetComponent<CharacterController>()))
                {
                    print("HITTING " + potentialenemy.name + " NOW!!!");
                    currentweapon.OnHit(potentialenemy, transform.position);
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
    private void FixedUpdate()
    {
        
    }
}
