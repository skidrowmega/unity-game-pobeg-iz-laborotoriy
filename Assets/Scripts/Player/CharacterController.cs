using UnityEditor;
using UnityEngine;
using UnityEngine.Accessibility;

public class CharacterController : Entity
{
    public Transform Cursor;
    Vector3 CursorVector;
    Animator animator;






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
        if (!Input.GetMouseButtonDown(0)) return;
        if (animator == null) return;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("VESLOBASHLEFT")) return;
        animator.Play("VESLOBASHLEFT");
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



}
