using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;

public static class PlayerHandler
{
    public static CharacterController subplayer;
    public static CharacterController player
    {
        get
        {
            if (subplayer == null)
            {
                CharacterController subsubplayer= GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
                if (subsubplayer != null)
                {
                    subplayer = subsubplayer;
                    return subsubplayer;
                }
                return null;
            }
            else
            {
                return subplayer;
            }
        }
    }
}
