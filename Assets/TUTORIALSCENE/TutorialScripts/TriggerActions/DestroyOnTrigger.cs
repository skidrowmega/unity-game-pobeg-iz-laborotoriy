using UnityEngine;

public class DestroyOnTrigger : TriggerAction
{
    [SerializeField] Object ObjectToDestroy;

    protected override void OnTriggerAction()
    {
        Destroy(ObjectToDestroy);
    }
}
