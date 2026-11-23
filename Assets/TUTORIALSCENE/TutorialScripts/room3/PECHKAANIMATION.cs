using UnityEngine;

public class PECHKAANIMATION : MonoBehaviour
{

    Animator animator;

    [SerializeField] Trigger trigger;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        trigger.AddCallback(startanimation);
    }


    void startanimation()
    {
        animator.Play("Opening");
    }

}
