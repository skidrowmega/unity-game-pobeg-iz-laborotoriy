using UnityEngine;

public class MolotovLinger : MonoBehaviour
{
    [SerializeField] public int Damage = 3;
    [SerializeField] private float Radius = 1f;
    private float lasthittime = 0;
    [SerializeField] public float HitTimer = 2f;
    [SerializeField] CharacterController player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        Radius = transform.localScale.x/2;
    }
    void Update()
    {
        if (Time.time - lasthittime > HitTimer)
        {
            if ((transform.position - player.transform.position).magnitude <= Radius+player.transform.localScale.x/2)
            {
                player.TakeDamage(Damage, transform.position, null, 0, DamageType.Unparriable);
                lasthittime = Time.time;
            }
        }
    }
}
