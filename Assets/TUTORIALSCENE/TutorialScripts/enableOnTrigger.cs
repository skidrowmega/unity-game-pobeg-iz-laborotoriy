using UnityEngine;



public class enableOnTrigger : TriggerAction
{

    [SerializeField] GameObject objectToEnable;

    protected override void OnTriggerAction()
    {
        objectToEnable.SetActive(true);
    }
}
