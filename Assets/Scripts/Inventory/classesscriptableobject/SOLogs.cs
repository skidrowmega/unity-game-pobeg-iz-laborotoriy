using System.Collections;
using UnityEngine;

public enum LogType { WeaponMeleeLog, WeaponRangedLog, HealLog}

[CreateAssetMenu(fileName = "Log", menuName = "Roguelike/Items/Log")]

public class SOLogs: ItemScriptableObject
{
    public LogType typelog;
    public WeaponMeleeStats weaponMeleeStats;
    public WeaponRangedStats weaponRangedStats;
    public Color voxelcolor;
    public Pattern pattern;



    public void Awake()
    {
        type=ItemType.Log;
    }

    public override void OnUse()
    {
        CutoutManager.instance.VoxelColor = voxelcolor;
        CutoutManager.instance.Pattern = pattern;
        CutoutManager.instance.StartCut(OnCutEnd);
        
        //Вот тут вызывается окошко с рисованием и т.д.
    }
    void OnCutEnd()
    {
        switch (typelog)
        {
            case LogType.WeaponMeleeLog:
                {
                    Player.player.currentweapon.BreakWeapon();
                    Player.player.currentweapon.PlaceWeapon(CutoutManager.instance.finalCutout, weaponMeleeStats.ApplyMultiplier(CutoutManager.instance.finalScore));
                    break;
                }
            case LogType.WeaponRangedLog:
                {
                    Player.player.currentgun.BreakWeapon();
                    Player.player.currentgun.PlaceWeapon(CutoutManager.instance.finalCutout, weaponRangedStats.ApplyMultiplier(CutoutManager.instance.finalScore));
                    break;
                }
            case LogType.HealLog:
                {
                    break;
                }
        }
    }

    

}
