using UnityEngine;

public class GunWeapon : WeaponRanged
{
    [SerializeField] WeaponRangedStats statsonbreak;
    [SerializeField] GameObject tempNewWeapon;
    [SerializeField] WeaponRangedStats tempstats;

    public GunWeapon()
    {
        WeaponName = "Pistol";
        Damage = 5;
        PushStrength = 0;
        ReloadingTime = 1;
        RicochetCount = 0;
    }

    


    void BreakWeapon()
    {
        tempstats = new WeaponRangedStats(this);
        SwapStats(statsonbreak);
        OnBreak();
    }
    void OnBreak()
    {
        Destroy(weaponReference);
        weaponReference = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            BreakWeapon();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            PlaceWeapon(tempNewWeapon, tempstats);
        }
    }

    GameObject PlaceWeapon(GameObject weapontoPlace, WeaponRangedStats newstats)
    {
        if (weaponReference != null) return null;
        weaponReference = Instantiate(weapontoPlace, MeleeHandReference.transform);
        SwapStats(newstats);
        return weaponReference;
    }
}
