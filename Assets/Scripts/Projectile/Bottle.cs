using TMPro;
using UnityEditor;
using UnityEngine;

public class Bottle: Projectile
{
    private Vector3 playerPosition;
    protected CharacterController player;
    protected override void Awake()
    {
        base.Awake();
        playerPosition = player.transform.position;
    }
    protected override void MoveBullet()//летит на позицию игрока(где он был в момент выстрела) по параболе, чем ближе был игрок тем меньше высота броска и следовательно время полёта
    {
        
    }
}
