using UnityEngine;

public abstract class WeaponMelee: MonoBehaviour
{
    public string WeaponName="Oar";
    public int Damage=1;
    public int Stamina=100;
    public int StaminaRecoveryRate=20;
    public int StamineDecreaseRate=20;
    public int PushTime=0;

    public virtual void Punch() {
    
    }
}
