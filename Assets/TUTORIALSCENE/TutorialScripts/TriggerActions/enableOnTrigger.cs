using UnityEngine;



public class SetEnableOnTrigger : TriggerAction
{

    [SerializeField] GameObject objectToEnable;
    [SerializeField] bool ChangeTo;
    protected override void OnTriggerAction()
    {
        objectToEnable.SetActive(ChangeTo);
    }
}
