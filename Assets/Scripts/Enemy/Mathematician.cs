using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class Mathematician: EnemyAll
{
    public Mathematician()
    {
        healthpoints = 50;
        Speed = 0.005f;
        Damage = 15;
        Reload = 3;
    }

    public override void Punch()//Кидается синусоидой наверное
    {
        base.Punch();
    }

    public void DivisioByZero()
    {

    }

    public void ArithmeticProgression()
    {

    }
}
