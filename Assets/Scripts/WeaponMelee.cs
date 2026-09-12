using UnityEngine;

public abstract class WeaponMelee: MonoBehaviour
{
    public string WeaponName;
    public int Damage;
    public int Stamina;
    public int StaminaRecoveryRate;
    public int StamineDecreaseRate;
    public int PushTime;

    public virtual void Punch() 
    {
        
    }
}
