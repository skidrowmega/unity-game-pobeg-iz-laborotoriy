using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;
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
        voxels= new GameObject[dimensions[0], dimensions[1]];
        VoxelBlank[] potentialchildren = gameObject.GetComponentsInChildren<VoxelBlank>();
        Sprite sprite;
        Texture2D texture;
        Color[] pixels;
        if (potentialchildren.Length > 0)
        {
            int count = 0;
            for (int j = dimensions[1] - 1; j > -1; j--)
            {
                for (int i = 0; i < dimensions[0]; i++)
                {
                    voxels[i, j] = potentialchildren[count].gameObject;
                    
                    MeshRenderer[] meshRenderer = voxels[i, j].GetComponentsInChildren<MeshRenderer>();
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
                    count++;
                }
            }
        }
        else
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
                

                gameobjectarray[i, j].transform.parent = transform;
                count++;
            }
        }
        transform.position = transform.position - new Vector3(dimensions[0] * gap/2, dimensions[0] * gap/2, 0); 
        return gameobjectarray;
    }

}
