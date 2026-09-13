using UnityEngine;

public class EnemyController : Entity
{
    private CharacterController player;
    void Start()
    {
        player = FindAnyObjectByType<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(!IsStunned) transform.Translate((player.transform.position- transform.position) *Speed);
        if (!IsStunned) transform.position += (player.transform.position - transform.position).normalized * Speed;
        PushPerFrame();
    }

}
