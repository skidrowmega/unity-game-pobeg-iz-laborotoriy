using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private CharacterController player;
    public float Speed;
    void Start()
    {
        player = FindAnyObjectByType<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate((player.transform.position- transform.position) *Speed);

    }

}
