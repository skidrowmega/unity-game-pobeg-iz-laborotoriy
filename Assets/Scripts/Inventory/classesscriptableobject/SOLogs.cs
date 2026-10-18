using UnityEngine;

public enum LogType { WeaponMeleeLog, WeaponRangedLog, HealLog}

[CreateAssetMenu(fileName = "Log", menuName = "Roguelike/Items/Log")]

public class SOLogs: ItemScriptableObject
{
    public LogType typelog;
}
