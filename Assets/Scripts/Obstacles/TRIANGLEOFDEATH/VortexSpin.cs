using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class VortexSpin : MonoBehaviour
{

    [SerializeField] float RotationSpeed = 1;
    [SerializeField] float damage_radius = 1;
    [SerializeField] int Damage = 1000;
    [SerializeField] int DamageToPlayer = 30;
    [SerializeField] float pushstrength;
    Player player;
    void Start()
    {
        player = Player.player;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
        transform.Rotate(Time.deltaTime * RotationSpeed, Time.deltaTime * RotationSpeed, Time.deltaTime*RotationSpeed);
    }


    void DamagePlayer()
    {
        VortexSpin[] vortices = GetVortices();
        if (vortices.Length == 0)
        {
            player.TakeDamage(DamageToPlayer, transform.position, null, 0, DamageType.Unparriable);
            Destroy(gameObject);
            return;
        }
        VortexSpin randomvortex=vortices[Random.Range(0, vortices.Length)];
        Vector3 newpos = player.transform.position;
        newpos.x = randomvortex.transform.position.x;
        newpos.z = randomvortex.transform.position.z;
        player.TakeDamage(DamageToPlayer, transform.position, null, 0, DamageType.Unparriable);
        player.transform.position = newpos;
        DestroyBothVortices(randomvortex);

    }
    void DamageEnemy(Entity target)
    {
        VortexSpin[] vortices = GetVortices();
        VortexSpin randomvortex = vortices[Random.Range(0, vortices.Length)];
        target.TakeDamage(Damage, transform.position, null, 0, DamageType.Unparriable);
        Destroy(gameObject);
    }

    void DestroyBothVortices(VortexSpin teleporttarget)
    {
        Destroy(teleporttarget.gameObject);
        Destroy(gameObject);
    }

    void CheckCollision()
    {
        foreach (Collider collider in Physics.OverlapSphere(transform.position, damage_radius))
        {
            Entity target = collider.GetComponent<Entity>();
            if (!target) return;
            if (target is Player)
            {
                DamagePlayer();
            }
            else DamageEnemy(target);
        }
        
    }

    VortexSpin[] GetVortices()
    {
        List<VortexSpin> vortices;
        vortices = FindObjectsByType<VortexSpin>().ToList();
        vortices.Remove(this);
        return vortices.ToArray();
    }


}
