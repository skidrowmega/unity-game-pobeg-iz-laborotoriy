using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string WeaponName;
    public float Damage;
    public float PushStrength;
    public Player WeaponHolder;
    public GameObject weaponReference;
    public GameObject MeleeHandReference;
    [SerializeField] public int MaxDurability = 100;
    [SerializeField] public int Durability = 100;
    [SerializeField] public int DurabilityDecrease = 5;
    private void Start()
    {
        WeaponHolder = Player.player;
    }
}
