using System.Runtime.CompilerServices;
using UnityEngine;

public static class PlayerHandler
{

    public static CharacterController player= GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
}
