using UnityEngine;

public class DestroyOnTrigger : Trigger
{
    [SerializeField] GameObject objectToDestroy;

    private void Update()
    {
        CheckCollision();
    }


    protected override void OnSuccess()
    {
        Destroy(objectToDestroy);
        SendCallback();
        Destroy(gameObject);
    }
}
