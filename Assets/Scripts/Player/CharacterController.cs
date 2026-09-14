using UnityEditor;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.Assemblies;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class CharacterController : Entity
{
    public Transform Cursor;
    Vector3 CursorVector;
    Animator animator;
    public WeaponMelee currentweapon;
    public WeaponRanged currentgun;
    public float parrycooldown=3;
    float lastparry;
    public float dodgecooldown = 3;
    float lastdodge;
    public bool IsDodging=false;
    public float DodgeDistance = 15;


    private void Awake()
    {
        animator= GetComponent<Animator>();
        lastparry = -parrycooldown;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        LookAtCursor();
        AttackMelee();
        AttemptAttackRanged();
        CheckDodge();
        /*if (Input.GetMouseButtonUp(1))
        {
            PushEntity(new Vector3(5, 1, 7), 0.3f);
        }*/
        Parry();
    }

    void AttackMelee()
    {
        //Debug.DrawLine(transform.position, transform.position + transform.forward * 2.982696f);
        if(IsStunned) return;
        if (!Input.GetMouseButtonDown(0)) return;
        if (animator == null) return;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("VESLOSTILLRIGHT")) return;
        animator.Play("VESLOBASHLEFT");
        //StartCoroutine(MeleeAttackLoop(.41f));
        StartCoroutine(MeleeAttackLoop(animator.GetCurrentAnimatorStateInfo(0).length));
    }


    void AttemptAttackRanged()
    {
        if (IsStunned) return;
        if (!Input.GetMouseButtonUp(1)) return;
        if (animator == null) return;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("VESLOSTILLRIGHT")) return;
        animator.Play("GunShoot");
        currentgun.Shot(transform.right);
    }


    void CheckDodge()
    {
        if (IsStunned) return;
        float MoveX = Input.GetAxis("Horizontal");
        float MoveZ = Input.GetAxis("Vertical");
        if (MoveX == 0 && MoveZ == 0) return;
        if (!Input.GetKeyDown(KeyCode.LeftShift)) return;
        if (animator == null) return;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("VESLOSTILLRIGHT")) return;
        animator.Play("BOBRDODGE");
        StartCoroutine(DodgeLoop(animator.GetCurrentAnimatorStateInfo(0).length, new Vector2(MoveX, MoveZ)));
    }


    void Move()
    {
        float MoveX = Input.GetAxis("Horizontal");
        float MoveZ = Input.GetAxis("Vertical");
        if (IsStunned) return;
        if (MoveX != 0 || MoveZ != 0)
            transform.position += new Vector3(MoveX, 0, MoveZ).normalized * Speed * Time.deltaTime;
    }

    void Parry()
    {
        if (!Input.GetKeyDown("f")) return;
        if (Time.time - lastparry <= parrycooldown) return;
        if (animator == null) return;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("VESLOSTILLRIGHT")) return;
        animator.Play("VESLOPARRY");
        lastparry= Time.time;
    }


    void LookAtCursor()
    {
        CursorVector = Cursor.position - transform.position;
        transform.rotation = Quaternion.Euler(0, Mathf.Rad2Deg * Mathf.Atan2(-CursorVector.z, CursorVector.x), 0);
    }

    private bool isparrying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("VESLOPARRY");
    }

    public override void TakeDamage(int damage, Vector3 source, Entity attacker, float pushstrength)
    {
        if (IsDodging)
        {
            OnDodge();
            return;
        }
        if (isparrying())
        {
            OnParry(damage,attacker,pushstrength);
            return;
        }
        else
        {
            base.TakeDamage(damage,source,attacker, pushstrength);
        }
    }
    private void OnParry(int damage, Entity source, float pushstrength)
    {
        lastparry=-parrycooldown;
        source.TakeDamage(0,transform.position,this,5);
    }

    void OnDodge()
    {

    }
    IEnumerator MeleeAttackLoop(float secondstowait)
    {
        yield return new WaitForSeconds(0.13f);
        float starttime = Time.time;
        List<Collider> checkedcolliders = new List<Collider>();
        while (Time.time - starttime < secondstowait)
        {
            foreach (Collider collider in Physics.OverlapCapsule(transform.position, transform.position + Vector3.up, 1.5f * 1.6f))
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
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator DodgeLoop (float secondstowait, Vector2 Direction)
    {
        IsStunned = true;
        IsDodging=true;
        float starttime = Time.time;
        Vector3 startposition = transform.position;
        float timewasted = 0;
        while (Time.time - starttime < secondstowait)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3 (Direction.x,0,Direction.y)*DodgeDistance+ startposition, timewasted);
            timewasted += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        IsStunned=false;
        transform.position = startposition + new Vector3(Direction.x, 0, Direction.y) * DodgeDistance;
        IsDodging= false;
    }

}
