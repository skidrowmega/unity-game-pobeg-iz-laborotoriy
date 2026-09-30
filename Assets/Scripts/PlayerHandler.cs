using System.Runtime.CompilerServices;
using UnityEngine;

public static class PlayerHandler
{
    public static CharacterController GetPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }
}
