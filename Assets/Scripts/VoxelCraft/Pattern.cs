using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern", menuName = "ChiselDrawing/Pattern")]
public class Pattern : ScriptableObject
{
    Pattern()
    {
        points= new List<Vector2Int>();
        Size = 0;
    }
    [SerializeField] public List<Vector2Int> points;
    [SerializeField] public int Size;
}