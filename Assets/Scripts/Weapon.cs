using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public string WeaponName;
    public float Damage;
    public float PushStrength;
    public Player WeaponHolder;
    public GameObject weaponReference;
    public GameObject HandReference;
    public WeaponDurabilityBar AssociatedUI;
    [SerializeField] public float MaxDurability = 100;
    [SerializeField] public float Durability = 100;
    [SerializeField] public float DurabilityDecrease = 5;
    private void Start()
    {
        WeaponHolder = Player.player;
    }
    protected virtual void OnBreak()
    {
        if (AssociatedUI != null)
        {
            AssociatedUI.transform.parent.GetComponent<Image>().sprite = null;
        }
    }
}
