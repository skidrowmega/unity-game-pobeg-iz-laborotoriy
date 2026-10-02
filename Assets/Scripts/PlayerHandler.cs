using System.Runtime.CompilerServices;
using UnityEngine;

public static class PlayerHandler
{
    public static CharacterController player
    {
        get
        {
            return GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        }
    }
}
