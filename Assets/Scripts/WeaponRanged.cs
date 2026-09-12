using UnityEngine;

public abstract class WeaponRanged: MonoBehaviour
{
    public string WeaponName;
    public int Damage;
    public int PushTime;
    public int ReloadingTime;
    public int EnergyDecrease;
    public int RicochetCount;

    public virtual void Shot()
    {

    }
}
