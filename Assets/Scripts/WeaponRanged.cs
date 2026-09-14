using UnityEngine;

public abstract class WeaponRanged: MonoBehaviour
{
    public string WeaponName;
    public int Damage;
    public int PushTime;
    public int ReloadingTime;
    public int EnergyDecrease;
    public int RicochetCount;
    public Entity WeaponHolder;
    public GameObject projectileprefab;
    public virtual void Shot(Vector3 Direction)
    {

    }
    private void Awake()
    {
        WeaponHolder = transform.parent.GetComponent<Entity>();
    }
}
