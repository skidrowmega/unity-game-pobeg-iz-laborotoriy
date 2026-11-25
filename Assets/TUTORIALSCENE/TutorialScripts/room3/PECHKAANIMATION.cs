using UnityEngine;

public class PECHKAANIMATION : TriggerAction
{

    Animator animator;
    

    [SerializeField] FrogDialog dialog;
    [TextArea(1,10)] [SerializeField] string DialogLines;

    [SerializeField] FrogBehavior frogbehavior;


    string[] parsedlines;
    private void Awake()
    {
        parsedlines = DialogLines.Split(new char[] { '\n' });
        animator = GetComponentInChildren<Animator>();
    }


    protected override void OnTriggerAction()
    {
        Player.player.IsStunned = true;
        animator.Play("Opening");
        Invoke(nameof(startfrogdialogue),2.08f);

    }

    void startfrogdialogue()
    {
        if (!DialogueBox.instance.FrogBehavior)
        {
            DialogueBox.instance.FrogBehavior = frogbehavior;
        }
        dialog.InitiateDialog(parsedlines, OnDialogueEnd);
        
    }

    void OnDialogueEnd()
    {
        Player.player.IsStunned=false;
        print("DIALOGUEENDED");
    }

}
