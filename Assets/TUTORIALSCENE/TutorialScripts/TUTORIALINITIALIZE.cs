using UnityEngine;

public class TUTORIALINITIALIZE : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player.player.currentweapon.BreakWeapon();
        Player.player.currentgun.BreakWeapon();
    }

}
