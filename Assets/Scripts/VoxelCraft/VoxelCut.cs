using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class VoxelCut : MonoBehaviour
{
    // Update is called once per frame
    GameObject[,] voxels;
    bool[,] availablevoxels;
    int[] dimensions;
    GenerateVoxelGrid voxelGrid;
    BoxCollider boxCollider;
    int[] rootpoint = new int[2];
    GameObject rootvoxel;
    private void Awake()
    {
        voxelGrid = GetComponent<GenerateVoxelGrid>();
        boxCollider = GetComponent<BoxCollider>();
    }
    private void Start()
    {
        voxels = voxelGrid.voxels;
        dimensions = voxelGrid.dimensions;
        boxCollider.size = new Vector3((dimensions[0]) * voxelGrid.gap, dimensions[0] * voxelGrid.gap, voxelGrid.gap);
        boxCollider.center = new Vector3((dimensions[0])/2 * voxelGrid.gap-voxelGrid.gap/2, (dimensions[0])/2*voxelGrid.gap - voxelGrid.gap / 2, voxelGrid.gap/2);
        rootpoint[0]  = dimensions[0]-1;
        rootpoint[1] = dimensions[1] / 2-1;
        rootvoxel = voxels[rootpoint[0], rootpoint[1]];
        rootvoxel.GetComponent<SpriteRenderer>().color = Color.red;
        availablevoxels = new bool[dimensions[0], dimensions[1]];
        for (int i = 0; i < dimensions[0]; i++)
        {
            for (int j = 0; j < dimensions[1]; j++)
            {
                availablevoxels[i, j] = true;
            }
        }

    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100);
            //if (hit.collider.transform.parent==transform) Destroy(hit.collider.gameObject);
            DestroyVoxelOnPosition(hit.point);

        }
    }
    bool[,] CheckCutoffRecursive(int[] position,bool[,] VoxelsChecked = null)
    {
        if (VoxelsChecked == null)
        {
            VoxelsChecked = new bool[dimensions[0], dimensions[1]];
        }
        if (VoxelsChecked[position[0], position[1]] || !availablevoxels[position[0], position[1]]) return VoxelsChecked;
        VoxelsChecked[position[0],position[1]] = true;
        int newposx;
        int newposy;
        int[] newposition = new int[2];
        for (int i = -1;i < 2; i++)
        {
            for(int j = -1;j < 2; j++)
            {
                if (!(i == 0 || j == 0)) continue;
                newposx = position[0] + i;
                newposy = position[1] + j;
                if (newposx>=0 && newposx < dimensions[0] &&
                    newposy>=0 && newposy < dimensions[1])
                {
                    newposition[0]=newposx;
                    newposition[1]=newposy;
                    CheckCutoffRecursive(newposition, VoxelsChecked);
                }
            }
        }
        return VoxelsChecked;
    }
    
    void DestroyVoxelOnPosition(Vector3 position)
    {
        position.z = transform.position.z;
        position += new Vector3(-voxelGrid.gap/2,-voxelGrid.gap/2);
        if (PointInRangeCheck(position, transform.position - new Vector3(voxelGrid.gap / 2, voxelGrid.gap / 2), transform.position + new Vector3(dimensions[0] * voxelGrid.gap, dimensions[1] * voxelGrid.gap, 0)))
        {
            Vector3 localpos = position - transform.position;
            print("X: " + localpos.x / voxelGrid.gap + " Y: " + localpos.y / voxelGrid.gap);
            //print("Targetting voxel: " + Mathf.RoundToInt(localpos.x / voxelGrid.gap) + "   " + Mathf.RoundToInt(localpos.y / voxelGrid.gap));
            print(new Vector2(Mathf.RoundToInt(localpos.x / voxelGrid.gap), Mathf.RoundToInt(localpos.y / voxelGrid.gap)));
            int localx = Mathf.RoundToInt(localpos.x / voxelGrid.gap);
            int localy = Mathf.RoundToInt(localpos.y / voxelGrid.gap);
            DestroyVoxelByIndex(localx, localy);
            DestroyCuttoffs();
        }
    }

    void DestroyVoxelByIndex(int indX,int indY)
    {
        GameObject voxel; voxel = voxels[indX, indY];
        if (voxel == rootvoxel) return;
        availablevoxels[indX, indY] = false;
        Destroy(voxel);
    }

    void DestroyCuttoffs()
    {
        bool[,] connectedvoxels = CheckCutoffRecursive(rootpoint);
        for (int i = 0; i < dimensions[0]; i++)
        {
            for (int j = 0; j < dimensions[1]; j++)
            {
                if (availablevoxels[i,j]==true && connectedvoxels[i,j]==false)
                {
                    DestroyVoxelByIndex(i,j);
                }
            }
        } 
    }

    bool PointInRangeCheck(Vector2 point,Vector2 range1, Vector2 range2) //ѕровер€ет находитьс€ ли точка( Vector2 ) в пределах диапазона
    {
        print(point);
        print(range1);
        print(range2);
        return (point.x >= range1.x && point.y >= range1.y) &&
               (point.x <= range2.x && point.y <= range2.y);
    }
}
