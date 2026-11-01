using UnityEngine;


public class AmuletSO: ScriptableObject
{
    public string amuletName;
    public string amuletDescription;
    public Sprite amuletImage;
    public float resistBuff;
    public float damageBuff;

    public virtual void OnEquip(GameObject player) { }
    public virtual void OnUnequip(GameObject player) { }

    public float ModifyDamage(float Damage)
    {
        return Damage * (1 + damageBuff);
    }

    public float ResistDamage(float incomingDamage)
    {
        return incomingDamage * (1 - resistBuff);
    }
}
