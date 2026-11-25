using UnityEngine;

public class FrogBehavior : MonoBehaviour
{


    Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void StartTalking()
    {
        animator.Play("Chat");
    }

    public void StopTalking()
    {
        animator.Play("Default");
    }


    public void StartTrade()
    {
        animator.Play("Trade");
    }

    public void StopTrade()
    {
        animator.SetBool("Tradeback", true);
    }



}
