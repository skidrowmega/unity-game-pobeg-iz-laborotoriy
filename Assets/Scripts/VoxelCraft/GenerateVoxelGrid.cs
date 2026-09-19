using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;

public class GenerateVoxelGrid : MonoBehaviour
{
    [SerializeField] GameObject voxel;
    [SerializeField] public int[] dimensions;
    [SerializeField] public float gap;
    [SerializeField] public GameObject[,] voxels;
    void Awake()
    {
        voxel.transform.localScale=new Vector3(gap, gap, gap);
        voxels=GenerateGrid(voxel,dimensions);
    }

    GameObject[,] GenerateGrid(GameObject voxel,int[] dimensions)
    {
        GameObject[,] gameobjectarray = new GameObject[dimensions[0], dimensions[1]];
        for (int i = 0; i < dimensions[0]; i++)
        {
            for (int j = 0; j < dimensions[1]; j++)
            {
                gameobjectarray[i, j] = Instantiate<GameObject>(voxel, transform.position + new Vector3(i, j) * gap,Quaternion.identity);
                gameobjectarray[i, j].transform.parent = transform;
            }
        }
        transform.position = transform.position - new Vector3(dimensions[0] * gap/2, dimensions[0] * gap/2, 0); 
        return gameobjectarray;
    }

}
