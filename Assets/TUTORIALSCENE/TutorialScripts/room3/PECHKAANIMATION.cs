using UnityEngine;

public class PECHKAANIMATION : TriggerAction
{

    Animator animator;

    [SerializeField] Trigger trigger;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    protected override void OnTriggerAction()
    {
        animator.Play("Opening");
    }

}
