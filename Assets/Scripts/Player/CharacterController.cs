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
        /*if (Input.GetMouseButtonUp(1))
        {
            PushEntity(new Vector3(5, 1, 7), 0.3f);
        }*/
        Parry();
    }

    void Attack()
    {
        //Debug.DrawLine(transform.position, transform.position + transform.forward * 2.982696f);
        if (!Input.GetMouseButtonDown(0)) return;
        if (animator == null) return;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("VESLOSTILLRIGHT")) return;
        animator.Play("VESLOBASHLEFT");
        //StartCoroutine(MeleeAttackLoop(.41f));
        StartCoroutine(MeleeAttackLoop(animator.GetCurrentAnimatorStateInfo(0).length));
        print("current animation length: " + animator.GetCurrentAnimatorStateInfo(0).length);
    }
    void Move()
    {
        if (IsStunned) return;
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized * Speed;
    }

    void Parry()
    {
        if (!Input.GetKeyDown("f")) return;
        if (animator == null) return;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("VESLOSTILLRIGHT")) return;
        animator.Play("VESLOPARRY");
    }


    void LookAtCursor()
    {
        CursorVector = Cursor.position - transform.position;
        transform.rotation = Quaternion.Euler(0, Mathf.Rad2Deg * Mathf.Atan2(-CursorVector.z, CursorVector.x), 0);
    }

    IEnumerator MeleeAttackLoop(float secondstowait)
    {
        float starttime = Time.time;
        List<Collider> checkedcolliders = new List<Collider>();
        while (Time.time - starttime < secondstowait)
        {
            foreach (Collider collider in Physics.OverlapCapsule(transform.position, transform.position + Vector3.up, 2.482696f))
            {
                Entity potentialenemy = collider.GetComponent<Entity>();
                if (checkedcolliders.Contains(collider)) continue;
                checkedcolliders.Add(collider);
                if (potentialenemy && !(potentialenemy.GetComponent<CharacterController>())
                    && Vector3.Dot((potentialenemy.transform.position - transform.position).normalized, transform.right) >= 0.5
                    )
                {
                    print("HIT ANGLE DEVIATION: " + Mathf.Acos(Vector3.Dot((potentialenemy.transform.position - transform.position).normalized, transform.right)) * Mathf.Rad2Deg);
                    print("HITTING " + potentialenemy.name + " NOW!!!");
                    currentweapon.OnHit(potentialenemy, transform.position);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private bool isparrying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("VESLOPARRY");
    }

    public override void TakeDamage(int damage, Entity source, float pushstrength)
    {
        if (isparrying())
        {
            OnParry(damage,source,pushstrength);
            return;
        }
        else
        {
            base.TakeDamage(damage,source, pushstrength);
        }
    }
    private void OnParry(int damage, Entity source, float pushstrength)
    {
        source.PushEntity(transform.position, 5);
    }
}
