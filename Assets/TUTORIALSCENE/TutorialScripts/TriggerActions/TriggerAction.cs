using UnityEngine;

public class TriggerAction : MonoBehaviour
{
    [SerializeField] Trigger trigger;

    protected virtual void Start()
    {
        trigger.AddCallback(OnTriggerAction);
    }

    protected virtual void OnTriggerAction()
    {
        
    }

}
