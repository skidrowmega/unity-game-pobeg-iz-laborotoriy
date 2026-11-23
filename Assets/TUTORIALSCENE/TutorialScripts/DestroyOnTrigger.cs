using UnityEngine;

public class DestroyOnTrigger : TriggerAction
{
    [SerializeField] GameObject ObjectToDestroy;

    protected override void OnTriggerAction()
    {
        Destroy(ObjectToDestroy);
    }
}
