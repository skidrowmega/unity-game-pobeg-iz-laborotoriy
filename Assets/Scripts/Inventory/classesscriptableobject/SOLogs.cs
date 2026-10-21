using UnityEngine;

public enum LogType { WeaponMeleeLog, WeaponRangedLog, HealLog}

[CreateAssetMenu(fileName = "Log", menuName = "Roguelike/Items/Log")]

public class SOLogs: ItemScriptableObject
{
    public LogType typelog;
    public void Awake()
    {
        type=ItemType.Log;
    }

    public override void OnUse()
    {
        //Вот тут вызывается окошко с рисованием и т.д.
    }
}
