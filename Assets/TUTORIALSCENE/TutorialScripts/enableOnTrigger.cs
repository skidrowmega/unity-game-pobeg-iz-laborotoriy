using UnityEngine;



public class enableOnTrigger : Trigger
{

    [SerializeField] GameObject objectToEnable;

    private void Update()
    {
        CheckCollision();
    }


    protected override void OnSuccess()
    {
        objectToEnable.SetActive(true);
        SendCallback();
        Destroy(gameObject);
    }
    
}
