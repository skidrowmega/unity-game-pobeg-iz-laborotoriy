using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class Chemist: EnemyAll
{
    public Chemist()
    {
        healthpoints = 150;
        Speed = 0.005f;
        Damage = 5;
        Reload = 5;
    }

    protected override void TryAttack()//Кидается колбой (случайный урон)
    {
        base.TryAttack();
    }

    public void Heal()
    {

    }

    public void MolotovСocktail()
    {

    }
}
