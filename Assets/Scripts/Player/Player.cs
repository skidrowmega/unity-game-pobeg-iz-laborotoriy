using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class Player : Entity
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
    bool DodgeReady = true;
    [SerializeField] float DodgeCooldown=.7f;
    public float DodgeDistance = 15;
    public float DodgeTime = 0.2f;

    public static Player player { get; private set; }

    [SerializeField] private GameObject grenadeprefab;
    [SerializeField] private float BasicFlightTime= 1f;
    [SerializeField] private float GrenadeArcHeight = 3.5f;
    [SerializeField] private float GrenadeExplosionRadius = 3f;
    [SerializeField] private float GrenadeDamage = 50f;
    [SerializeField] private float GrenadePushStrength = 5f;
    [SerializeField] private float GrenadeStunTime = 0;

    private void Awake()
    {
        player = this;
        animator= GetComponentInChildren<Animator>();
        lastparry = -parrycooldown;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (GameManager.Instance.CurrentState == GameManager.GameState.Playing)
        {
            LookAtCursor();
            AttackMelee();
            AttemptAttackRanged();
        }
        CheckDodge();
        /*if (Input.GetMouseButtonUp(1))
        {
            PushEntity(new Vector3(5, 1, 7), 0.3f);
        }*/
        TryParry();

        TryGrenadeAttack();

    }



    void AttackMelee()
    {
        if (IsStunned) return;
        if (animator == null) return;
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Udar"))
        {
            return;
        }
        if (!Input.GetMouseButtonDown(0)) return;
        if (Input.GetMouseButtonDown(0)) 
        {
            if (animator.GetCurrentAnimatorStateInfo(1).IsName("Default")) 
            {
                animator.Play("Udar");
                StartCoroutine(MeleeAttackLoop(animator.GetCurrentAnimatorStateInfo(1).length));// сделать чтобы время когда бить стало меньше
            }
        }
        
    }


    void AttemptAttackRanged()
    {
        if (IsStunned) return;
        if (!currentgun.IsUsable) return;
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Shoot"))
        {
            return;
        }
        if (animator == null) return;
        if (!Input.GetMouseButtonUp(1)) return;
        if (Input.GetMouseButtonUp(1))
        {
            if (animator.GetCurrentAnimatorStateInfo(1).IsName("Default"))
            {
                animator.Play("Shoot");
                currentgun.Shot(transform.right);
            }
        }
    }


    void CheckDodge()
    {
        if (!DodgeReady) return;
        if (IsDodging) return;
        float MoveX = Input.GetAxis("Horizontal");
        float MoveZ = Input.GetAxis("Vertical");
        if (MoveX == 0 && MoveZ == 0) return;
        if (!Input.GetKeyDown(KeyCode.LeftShift)) return;
        if (animator == null) return;
        if (!animator.GetCurrentAnimatorStateInfo(1).IsName("Default")) return;
        //animator.Play("BOBRDODGE");
        DodgeStart(DodgeTime, new Vector2(MoveX, MoveZ));
    }


    void Move()
    {
        float MoveX = Input.GetAxis("Horizontal");
        float MoveZ = Input.GetAxis("Vertical");
        if (IsStunned) {
            animator.SetBool("isWalking", false);
            animator.SetBool("isWalkingBack", false);
            return;
        }
        
        if (MoveX != 0 || MoveZ != 0) {
            animator.SetBool("isWalking", true);
            Vector3 Direction = new Vector3(MoveX, 0, MoveZ).normalized;
            float Dot = Vector3.Dot(Direction, transform.right);
            if (Dot < 0)
            {
                animator.SetBool("isWalkingBack", true);
            }
            else
            {
                animator.SetBool("isWalkingBack", false);
            }
            bool ishitX = Physics.Raycast(transform.position, new Vector3(MoveX, 0, 0).normalized, .5f);
            bool ishitZ = Physics.Raycast(transform.position, new Vector3(0, 0, MoveZ).normalized, .5f);
            if (ishitX) MoveX = 0;
            if (ishitZ) MoveZ = 0;
            Vector3 moveDirection = new Vector3(MoveX, 0, MoveZ).normalized;
            Vector3 newpos = new Vector3(MoveX, 0, MoveZ).normalized * Speed * Time.deltaTime;
            transform.position += newpos;
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isWalkingBack", false);
        }
    }

    void TryParry()
    {
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Parry"))
        {
            return;
        }
        if (Time.time - lastparry <= parrycooldown) return;
        if (animator == null) return;
        if (!Input.GetKeyDown("f")) return;
        if (Input.GetKeyDown("f"))
        {
            if (animator.GetCurrentAnimatorStateInfo(1).IsName("Default"))
            {
                animator.Play("Parry");
                lastparry = Time.time;
            }
        }
        
    }


    void LookAtCursor()
    {
        CursorVector = Cursor.position - transform.position;
        transform.rotation = Quaternion.Euler(0, Mathf.Rad2Deg * Mathf.Atan2(-CursorVector.z, CursorVector.x), 0);
    }

    public bool isparrying()
    {
        return animator.GetCurrentAnimatorStateInfo(1).IsName("Parry");
    }

    public override void TakeDamage(float damage, Vector3 source, Entity attacker, float pushstrength, DamageType type, float StunTime=0)
    {
        if (IsDodging)
        {
            if (type==DamageType.Unparriable)
            {
                DodgeStop();
                base.TakeDamage(damage, source, attacker, pushstrength, type, StunTime);

            }
            else
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
            base.TakeDamage(damage,source,attacker, pushstrength,type, StunTime);
    }
    private void OnParry(float damage, Entity source, float pushstrength,float StunTime)
    {
        lastparry=-parrycooldown;
        source.TakeDamage(0,transform.position,this,5,DamageType.Normal,StunTime);
    }

    void OnDodge()
    {
    }
    private void TryGrenadeAttack()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (InventoryManager.instance.ContainsItem(ItemType.Grenade))
            {
                InventoryManager.instance.RemoveItem("Grenade");
                DoGrenadeThrow();
            }
            
        }
    }

    public void DoGrenadeThrow()
    {
        animator.Play("Granade");
        Invoke(nameof(SpawnGranade), 1f);
    }

    private void SpawnGranade() 
    {
        if (IsStunned) return;
        StartCoroutine(GrenadeBehavior());
    }

    private void GrenadeExplosion(Vector3 Position)
    {
        foreach (Collider item in Physics.OverlapSphere(Position, GrenadeExplosionRadius))
        {
            Entity entity = item.GetComponent<Entity>();
            if (entity != null)
            {
                entity.TakeDamage(GrenadeDamage, Position, this, GrenadePushStrength, DamageType.Unparriable, GrenadeStunTime);
            }
        }
    }
    IEnumerator GrenadeBehavior()
    {
        /*
         * всяки переменные
         * цикл полета - чем дальше летит тем дольше (арбитрарное время умножается на расстояние)
         * обозначение назначения, чем ближе - тем ярче
         * оставленная зона отчуждения после удара (там звук разбивающегося стекла)
         */

        Vector3 targetPos = Cursor.transform.position;
        Vector3 startPos = transform.position;
        startPos.y -= player.transform.localScale.y / 2;
        float starty = startPos.y;
        GameObject grenade = Instantiate(grenadeprefab, transform.position, Quaternion.identity);
        float dist = Vector3.Distance(startPos, targetPos);

        float timeforflight = BasicFlightTime + (dist / 50);
        float elapsed = 0;
        while (elapsed < timeforflight)
        {
            grenade.transform.position = MathParabola.Parabola(startPos, targetPos, GrenadeArcHeight, elapsed / timeforflight);
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();


        }
        Destroy(grenade);
        GrenadeExplosion(targetPos);

    }



        IEnumerator MeleeAttackLoop(float secondstowait)
    {
        yield return new WaitForSeconds(0.4f);
        float starttime = Time.time;
        List<Collider> checkedcolliders = new List<Collider>();
        while (Time.time - starttime < secondstowait)
        {
            foreach (Collider collider in Physics.OverlapCapsule(transform.position, transform.position + Vector3.up, 1.5f * 1.6f))
            {
                Entity potentialenemy = collider.GetComponent<Entity>();
                if (checkedcolliders.Contains(collider)) continue;
                checkedcolliders.Add(collider);
                if (potentialenemy && !(potentialenemy.GetComponent<Player>())
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
            //newdodgedistance = ((hit.point - (ray.direction * 0.5f)) - ray.origin).magnitude;
            if (hit.distance < 0.5) newdodgedistance = 0;
            else
                newdodgedistance = ((hit.point - (ray.direction*.7f)) - ray.origin).magnitude;
        }
        print(newdodgedistance);
        if (newdodgedistance == 0)
        {
            DodgeStop();
            yield break;
        }
        animator.SetFloat("dodgeSpeedMultiplier", 1/(DodgeTime*newdodgedistance/DodgeDistance));
        secondstowait = secondstowait * newdodgedistance / DodgeDistance;
        Vector3 newdir = new Vector3(Direction.x, 0, Direction.y).normalized;
        Vector3 debugprevpos=Vector3.zero;
        //print(" dodge will last " + secondstowait.ToString() + " seconds");
        while (timewasted < secondstowait)
        {
            if (!IsDodging) break;
            transform.position = Vector3.Lerp(startposition, newdir*newdodgedistance+ startposition, timewasted/secondstowait);
/*            if (debugprevpos == transform.position && timewasted > 0)
            {
                break;
            }
            debugprevpos = transform.position;*/
            timewasted += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //transform.position = startposition + new Vector3(Direction.x, 0, Direction.y) * DodgeDistance;
        EndDodge();
    }
    void ReloadDodge()
    {
        DodgeReady=true;
    }

    void DodgeStart(float secondstowait, Vector2 Direction)
    {
        animator.SetBool("isWalking", false);
        animator.Play("Dodge");
        StartCoroutine(DodgeLoop(DodgeTime, Direction));
    }

    void DodgeStop()
    {
        IsDodging = false;
        animator.Play("Bober_stand");
        StopCoroutine("DodgeLoop");
        EndDodge();
    }
    
    public void EndDodge()
    {
        IsDodging = false;
        animator.Play("Bober_stand");
        DodgeReady = false;
        Invoke(nameof(ReloadDodge), DodgeCooldown);
    }

    public override void Death()
    {
        base.Death();
        GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
    }
}
