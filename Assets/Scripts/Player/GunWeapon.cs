using UnityEngine;

public class GunWeapon : WeaponRanged
{
    /*    public int Speed = 1;
        public const int LifeTime = 10;
        public int damage = 1;
        public Vector3 direction;
        public BulletType type;
    */


    public GunWeapon()
    {
        WeaponName = "Pistol";
        Damage = 5;
        PushStrength = 0;
        ReloadingTime = 1;
        RicochetCount = 0;
    }





    // Update is called once per frame
    void Update()
    {

    }




    public override void Shot(Vector3 Direction)
    {
        base.Shot(Direction);
    }
}
