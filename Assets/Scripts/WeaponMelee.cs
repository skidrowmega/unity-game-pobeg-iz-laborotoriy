using UnityEngine;

public abstract class WeaponMelee: MonoBehaviour
{
    public string WeaponName="Oar";
    public int Damage=10;
    public int Stamina=100;
    public int StaminaRecoveryRate=20;
    public int StamineDecreaseRate=20;
    public float PushStrength=5;
    public Entity WeaponHolder;

    public virtual void OnHit(Entity entity, Vector3 source) {

    }
    private void Awake()
    {
        WeaponHolder = transform.parent.GetComponent<Entity>();
    }
}
