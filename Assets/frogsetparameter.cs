using Unity.VisualScripting;
using UnityEngine;

public class frogsetparameter : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Tradeback",false);
    }
}
