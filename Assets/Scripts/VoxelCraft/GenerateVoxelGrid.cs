using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;
using UnityEngine.Rendering;

public class GenerateVoxelGrid : MonoBehaviour
{
    [SerializeField] GameObject voxel;
    [SerializeField] public int[] dimensions;
    [SerializeField] public float gap;
    [SerializeField] public GameObject[,] voxels;
    [SerializeField] private Texture2D texture;
    void Awake()
    {
        voxel.transform.localScale=new Vector3(gap, gap, gap);
        voxels=GenerateGrid(voxel,dimensions);
    }

    GameObject[,] GenerateGrid(GameObject voxel,int[] dimensions)
    {
        Vector2 planksize = texture.Size();
        int XSizeOffset = Mathf.FloorToInt(planksize.x / dimensions[0]);
        int YSizeOffset = Mathf.FloorToInt(planksize.y / dimensions[1]);
        print(XSizeOffset + " :  " + YSizeOffset);
        Color[] pixels;
        Sprite sprite;
        Texture2D destTexture;
        int width =  XSizeOffset;
        int height = YSizeOffset;
        GameObject[,] gameobjectarray = new GameObject[dimensions[0], dimensions[1]];
        for (int i = 0; i < dimensions[0]; i++)
        {
            for (int j = 0; j < dimensions[1]; j++)
            {
                int x = i *XSizeOffset;
                int y = j*YSizeOffset;
                print(x + " :  " + y);
                pixels = texture.GetPixels(x, y, width, height, 0);
/*                foreach (Color p in pixels)
                {
                    print(p.ToString());
                }*/

                destTexture = new Texture2D(XSizeOffset, YSizeOffset);
                destTexture.SetPixels(pixels);
                destTexture.Apply();

                sprite = Sprite.Create(destTexture, new Rect(0, 0,XSizeOffset,YSizeOffset), Vector2.zero, XSizeOffset);
                gameobjectarray[i, j] = Instantiate<GameObject>(voxel, transform.position + new Vector3(i, j) * gap,Quaternion.identity);
                gameobjectarray[i, j].transform.parent = transform;
                gameobjectarray[i, j].GetComponent<SpriteRenderer>().sprite = sprite;
            }
        }
        transform.position = transform.position - new Vector3(dimensions[0] * gap/2, dimensions[0] * gap/2, 0); 
        return gameobjectarray;
    }

}
