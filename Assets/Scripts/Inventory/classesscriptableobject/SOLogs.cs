using System.Collections;
using UnityEngine;

public enum LogType { WeaponMeleeLog, WeaponRangedLog, HealLog}

[CreateAssetMenu(fileName = "Log", menuName = "Roguelike/Items/Log")]

public class SOLogs: ItemScriptableObject
{
    public LogType typelog;
    public WeaponMeleeStats weaponMeleeStats;
    public Color voxelcolor;

    public void Awake()
    {
        type=ItemType.Log;
    }

    public override void OnUse()
    {
        CutoutManager.instance.VoxelColor = voxelcolor;
        CutoutManager.instance.StartCut(OnCutEnd);
        
        //Вот тут вызывается окошко с рисованием и т.д.
    }
    void OnCutEnd()
    {
        Player.player.currentweapon.BreakWeapon();
        Player.player.currentweapon.PlaceWeapon(CutoutManager.instance.finalCutout, weaponMeleeStats.ApplyMultiplier(CutoutManager.instance.finalScore));
    }

    

}
