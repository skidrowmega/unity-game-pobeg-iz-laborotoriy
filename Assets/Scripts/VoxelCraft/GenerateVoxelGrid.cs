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
    [SerializeField] private Sprite[] sprites;
    //[SerializeField] Texture2D texture;
    void Awake()
    {
        voxel.transform.localScale=new Vector3(gap, gap, gap);
        sprites=Resources.LoadAll<Sprite>("");

/*        for (int i = 0; i < 20; i++)
        {
            print(sprites[i].name);
        }*/
        voxels = GenerateGrid(voxel, dimensions);
    }

    GameObject[,] GenerateGrid(GameObject voxel,int[] dimensions)
    {
        Sprite sprite;
        Texture2D texture;
        Color[] pixels;

        GameObject[,] gameobjectarray = new GameObject[dimensions[0], dimensions[1]];
        int count = 0;
        for (int j = dimensions[1]-1; j > -1; j--)
        {
            for (int i = 0; i < dimensions[0]; i++)
            {
                gameobjectarray[i, j] = Instantiate<GameObject>(voxel, transform.position + new Vector3(i, j) * gap, Quaternion.identity);
                
                MeshRenderer[] meshRenderer = gameobjectarray[i,j].GetComponentsInChildren<MeshRenderer>();
                sprite = sprites[count];
                texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);

                pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                        (int)sprite.textureRect.y,
                                                        (int)sprite.textureRect.width,
                                                        (int)sprite.textureRect.height);
                texture.SetPixels(pixels);
                texture.Apply();
                foreach (MeshRenderer renderer in meshRenderer)
                {
                    renderer.material.mainTexture = texture;
                }
                
                //MeshFilter filter = gameobjectarray[i,j].GetComponent<MeshFilter>();
                //Mesh mesh = filter.mesh;

                //for (int k=0; k < 24; k++)
                //{
                //    mesh.uv2[k] = new Vector2(0, 0);
                //}

                //mesh.uv2[23] = new Vector2(0, 1);
                //mesh.uv2[21] = new Vector2(1, 1);
                //mesh.uv2[20] = new Vector2(0, 0);
                //mesh.uv2[22] = new Vector2(1, 0);

                //// 2 3 0 1 Front
                //// 6 7 10 11 Back
                //// 19 17 16 18 Left
                //// 23 21 20 22 Right
                //// 4 5 8 9 Top
                //// 15 13 12 14 Bottom

                //filter.mesh = mesh;

                gameobjectarray[i, j].transform.parent = transform;
                count++;
            }
        }
        transform.position = transform.position - new Vector3(dimensions[0] * gap/2, dimensions[0] * gap/2, 0); 
        return gameobjectarray;
    }

}
