using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class VortexSpin : MonoBehaviour
{

    [SerializeField] float RotationSpeed = 1;
    [SerializeField] float damage_radius = 1;
    [SerializeField] int Damage = 1000;
    [SerializeField] float pushstrength;
    CharacterController player;

    void Awake()
    {
        player = GameObject.Find("BOBR").GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
        transform.Rotate(Time.deltaTime * RotationSpeed, Time.deltaTime * RotationSpeed, Time.deltaTime*RotationSpeed);
    }


    void DamagePlayer()
    {
        //player.TakeDamage(Damage, transform.position, null, 0, DamageType.Unparriable);
    }
    void DamageEnemy(Entity target)
    {
        target.TakeDamage(Damage, transform.position, null, 0, DamageType.Unparriable);
    }


    void CheckCollision()
    {
        foreach (Collider collider in Physics.OverlapSphere(transform.position, damage_radius))
        {
            Entity target = collider.GetComponent<Entity>();
            if (!target) return;
            if (target is CharacterController)
            {
                DamagePlayer();
            }
            else DamageEnemy(target);
        }
        
    }


}
