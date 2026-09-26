using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public float DodgeTime = 0.2f;


    private void Awake()
    {
        animator= GetComponentInChildren<Animator>();
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
        if (IsDodging) return;
        float MoveX = Input.GetAxis("Horizontal");
        float MoveZ = Input.GetAxis("Vertical");
        if (MoveX == 0 && MoveZ == 0) return;
        if (!Input.GetKeyDown(KeyCode.LeftShift)) return;
        if (animator == null) return;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("VESLOSTILLRIGHT")) return;
        animator.Play("BOBRDODGE");
        StartCoroutine(DodgeLoop(DodgeTime, new Vector2(MoveX, MoveZ)));
    }


    void Move()
    {
        float MoveX = Input.GetAxis("Horizontal");
        float MoveZ = Input.GetAxis("Vertical");
        if (IsStunned) {
            animator.SetBool("isWalking", false);
            return;
        }
        
        if (MoveX != 0 || MoveZ != 0) {
            animator.SetBool("isWalking", true);
            bool ishitX = Physics.Raycast(transform.position, new Vector3(MoveX, 0, 0).normalized, .5f);
            bool ishitZ = Physics.Raycast(transform.position, new Vector3(0, 0, MoveZ).normalized, .5f);
            if (ishitX) MoveX = 0;
            if (ishitZ) MoveZ = 0;
            Vector3 newpos = new Vector3(MoveX, 0, MoveZ).normalized * Speed * Time.deltaTime;
            transform.position += newpos;
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
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

    public bool isparrying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("VESLOPARRY");
    }

    public override void TakeDamage(int damage, Vector3 source, Entity attacker, float pushstrength, DamageType type, float StunTime=0)
    {
        if (IsDodging)
        {
            OnDodge();
            return;
        }
        if (attacker!=null&&isparrying() && type!=DamageType.Unparriable)
        {
            if (type == DamageType.Projectile) return;
            OnParry(damage,attacker,pushstrength, StunTime);
            return;
        }
        else
        {
            base.TakeDamage(damage,source,attacker, pushstrength,type, StunTime);
        }
    }
    private void OnParry(int damage, Entity source, float pushstrength,float StunTime)
    {
        lastparry=-parrycooldown;
        source.TakeDamage(0,transform.position,this,5,DamageType.Normal,StunTime);
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
        IsStunned = false;
        IsDodging=true;
        float newdodgedistance = DodgeDistance;
        float starttime = Time.time;
        Vector3 startposition = transform.position;
        float timewasted = 0;

        Ray ray = new Ray(startposition, new Vector3(Direction.x,0,Direction.y));
        RaycastHit hit;
        bool ishit = Physics.Raycast(ray,out hit,newdodgedistance);
        
        if (ishit)
        {
            newdodgedistance = ((hit.point - (ray.direction * 0.5f)) - ray.origin).magnitude;
        }
        Vector3 newdir = new Vector3(Direction.x, 0, Direction.y).normalized;
        Vector3 debugprevpos=Vector3.zero;
        //print(" dodge will last " + secondstowait.ToString() + " seconds");
        while (timewasted < secondstowait)
        {
            transform.position = Vector3.Lerp(transform.position, newdir*newdodgedistance+ startposition, timewasted/secondstowait);
            if (debugprevpos == transform.position&&timewasted>0)
            {
                break;
            }
            debugprevpos = transform.position;
            timewasted += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        IsStunned=false;
        //transform.position = startposition + new Vector3(Direction.x, 0, Direction.y) * DodgeDistance;
        IsDodging= false;
    }

}
