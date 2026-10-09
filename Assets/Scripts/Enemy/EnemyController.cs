using UnityEngine;

public class EnemyController : Entity
{
    private Player player;
    void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(!IsStunned) transform.Translate((player.transform.position- transform.position) *Speed);
        if (!IsStunned) transform.position += (player.transform.position - transform.position).normalized * Speed * Time.deltaTime;
    }

}
