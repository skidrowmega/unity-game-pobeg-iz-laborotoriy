using UnityEngine;

public class TriggerOnPress : CollisionTrigger
{
    [SerializeField] KeyCode key;

    private void Update()
    {
        CheckCollision();
    }

    protected override void OnSuccess()
    {
        if (Input.GetKeyDown(key))
        {
            print("HELLO");
            SendCallback();
        }
    }

}
