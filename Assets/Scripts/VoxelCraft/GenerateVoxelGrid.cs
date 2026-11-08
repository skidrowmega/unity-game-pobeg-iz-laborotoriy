using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GenerateVoxelGrid : MonoBehaviour
{
    [SerializeField] public GameObject voxel;
    [SerializeField] public int[] dimensions;
    [SerializeField] public float gap;
    [SerializeField] public GameObject MainTexture;
    [SerializeField] public Sprite MainSprite;
    //[SerializeField] Texture2D texture;
    void Awake()
    {
        voxel.transform.localScale = new Vector3(gap, gap, gap);
        VoxelBlank[] potentialchildren = gameObject.GetComponentsInChildren<VoxelBlank>();
        if(!MainSprite) MainSprite= Resources.Load("derevo/") as Sprite;
        Texture2D texture;
        Color[] pixels;
        if (potentialchildren.Length > 0)
        {
            //int count = 0;
            //for (int j = dimensions[1] - 1; j > -1; j--)
            //{
            //    for (int i = 0; i < dimensions[0]; i++)
            //    {
            //        voxels[i, j] = potentialchildren[count].gameObject;

            //        MeshRenderer[] meshRenderer = voxels[i, j].GetComponentsInChildren<MeshRenderer>();
            //        //MeshRenderer meshRenderer = voxels[i, j].GetComponent<MeshRenderer>();
            //        sprite = sprites[count];
            //        texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);

            //        pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
            //                                                (int)sprite.textureRect.y,
            //                                                (int)sprite.textureRect.width,
            //                                                (int)sprite.textureRect.height);
            //        texture.SetPixels(pixels);
            //        texture.Apply();
            //        foreach (MeshRenderer renderer in meshRenderer)
            //        {
            //            renderer.material.mainTexture = texture;
            //        }
            //        count++;
            //    }
            //}
        }
        else
            MainTexture = GenerateGrid(voxel, dimensions);
    }

    GameObject GenerateGrid(GameObject voxel,int[] dimensions)
    {
        GameObject textureobject = Instantiate<GameObject>(voxel, transform.position , Quaternion.identity);

        textureobject.transform.parent = transform;
        textureobject.transform.localScale = new Vector3(dimensions[0] * gap*2, dimensions[1] * gap*2, 1);
        textureobject.transform.position= transform.position+new Vector3(dimensions[0] * gap/2-gap/2, dimensions[1] * gap/2-gap/2,0);
        return textureobject;




        //for (int j = dimensions[1] - 1; j > -1; j--)
        //{
        //    for (int i = 0; i < dimensions[0]; i++)
        //    {
        //        textureobject[i, j] = Instantiate<GameObject>(voxel, transform.position + new Vector3(i, j) * gap, Quaternion.identity);

        //        MeshRenderer[] meshRenderer = textureobject[i, j].GetComponentsInChildren<MeshRenderer>();
        //        sprite = sprites[count];
        //        texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);

        //        pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
        //                                                (int)sprite.textureRect.y,
        //                                                (int)sprite.textureRect.width,
        //                                                (int)sprite.textureRect.height);
        //        texture.SetPixels(pixels);
        //        texture.Apply();
        //        foreach (MeshRenderer renderer in meshRenderer)
        //        {
        //            renderer.material.mainTexture = texture;
        //        }


        //        textureobject[i, j].transform.parent = transform;
        //        count++;
        //    }
        //}
        //transform.position = transform.position - new Vector3(dimensions[0] * gap/2, dimensions[0] * gap/2, 0); 

    }

}
