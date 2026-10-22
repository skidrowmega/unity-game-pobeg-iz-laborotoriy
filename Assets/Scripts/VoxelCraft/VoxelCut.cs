using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UIElements;





public class VoxelCut : MonoBehaviour
{
    // Update is called once per frame


    //[SerializeField] public DrawingPatternData pattern;

    [SerializeField] Pattern patterntomake;
    [SerializeField] GameObject voxelforprefab;
    [SerializeField] private Material materialforprefab;

    GameObject[,] voxels;
    bool[,] availablevoxels;
    int[] dimensions;
    GenerateVoxelGrid voxelGrid;
    BoxCollider boxCollider;
    int[] rootpoint = new int[2];
    GameObject rootvoxel;
    [SerializeField] float BrushSize = 3;

    [SerializeField] bool ShakeCameraOnDestroy = true;
    [SerializeField] float ShakeIntensity = 0.01f;
    [SerializeField] float ShakeMagnitude = 0.1f;
    [SerializeField] float ShakeDuration = 0.4f;
    private Vector3 ShakeoriginalPosition;

    UnityEngine.UI.Slider BrushSizeSlider;
    UnityEngine.UI.Text SubmitResultText;
    UnityEngine.UI.Text SubmitButtonText;

    public float Timer;
    public bool Submitted = false;

    //ПОТОМ УБРАТЬ НАХ*Й
    private Mesh fabricatedprefabmesh;
    
    public float FinalScore=0;

    private void Awake()
    {
        voxelGrid = GetComponent<GenerateVoxelGrid>();
        boxCollider = GetComponent<BoxCollider>();
        BrushSizeSlider = Selectable.FindAnyObjectByType<UnityEngine.UI.Slider>();
        SubmitResultText = GameObject.Find("TextScore").GetComponent<UnityEngine.UI.Text>();
        SubmitResultText.transform.parent.gameObject.SetActive(false);
        SubmitButtonText = GameObject.Find("SubmitButton").GetComponentInChildren<Text>();
        SetBrushSize();
    }
    private void Start()
    {
        voxels = voxelGrid.voxels;
        dimensions = voxelGrid.dimensions;
        boxCollider.size = new Vector3((dimensions[0]) * voxelGrid.gap*2, dimensions[1] * voxelGrid.gap*2, voxelGrid.gap);
        boxCollider.center = new Vector3((dimensions[0]) / 2 * voxelGrid.gap - voxelGrid.gap / 2, (dimensions[1]) / 2 * voxelGrid.gap - voxelGrid.gap / 2, voxelGrid.gap / 2);
        rootpoint[0] = dimensions[0] - 1;
        rootpoint[1] = dimensions[1] / 2 - 1;
        rootvoxel = voxels[rootpoint[0], rootpoint[1]];
        foreach (MeshRenderer renderer in rootvoxel.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.material.color = Color.red;
        }
        availablevoxels = new bool[dimensions[0], dimensions[1]];
        for (int i = 0; i < dimensions[0]; i++)
        {
            for (int j = 0; j < dimensions[1]; j++)
            {
                availablevoxels[i, j] = true;
            }
        }
        UpdatePatternPoints();

    }

    void Update()
    {
        if (!Submitted)
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;
                Physics.Raycast(CutoutManager.instance.cutcamera.ScreenPointToRay(Input.mousePosition), out hit, 100);
                //if (hit.collider.transform.parent==transform) Destroy(hit.collider.gameObject);
                DestroyVoxelOnPosition(hit.point);

            }
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButton(1))
        {
            RaycastHit hit;
            Physics.Raycast(CutoutManager.instance.cutcamera.ScreenPointToRay(Input.mousePosition), out hit, 100);

            RemovePatternPoint(hit.point);
        }
#if UNITY_EDITOR
        #region DEBUG
        else if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            Physics.Raycast(CutoutManager.instance.cutcamera.ScreenPointToRay(Input.mousePosition), out hit, 100);

            AddPointToPattern(hit.point);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            GetCutoutScore();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            FabricatePrefab();
        }
        #endregion
#endif

        /*        foreach (GameObject voxel in availablevoxels)
                {
                    //GameObject voxel = GetVoxelOnIndex(point.x + rootpoint[0], point.y + rootpoint[1]);
                    if (voxel) voxel.GetComponent<MeshRenderer>().material.color = Color.black;

                }*/
    }
    bool[,] CheckCutoffRecursive(int[] position, bool[,] VoxelsChecked = null)
    {
        if (VoxelsChecked == null)
        {
            VoxelsChecked = new bool[dimensions[0], dimensions[1]];
        }
        if (VoxelsChecked[position[0], position[1]] || !availablevoxels[position[0], position[1]]) return VoxelsChecked;
        VoxelsChecked[position[0], position[1]] = true;
        int newposx;
        int newposy;
        int[] newposition = new int[2];
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (!(i == 0 || j == 0)) continue;
                newposx = position[0] + i;
                newposy = position[1] + j;
                if (newposx >= 0 && newposx < dimensions[0] &&
                    newposy >= 0 && newposy < dimensions[1])
                {
                    newposition[0] = newposx;
                    newposition[1] = newposy;
                    CheckCutoffRecursive(newposition, VoxelsChecked);
                }
            }
        }
        return VoxelsChecked;
    }


    public void SetBrushSize()
    {
        BrushSize = BrushSizeSlider.value;
    }

    void DestroyVoxelOnPosition(Vector3 position)
    {
        position.z = transform.position.z;
        bool DestroyedSomething;
        //position += new Vector3(-voxelGrid.gap/2,-voxelGrid.gap/2);
        if (BrushSize == 1 && PointInRangeCheck(position, transform.position - new Vector3(voxelGrid.gap / 2, voxelGrid.gap / 2), transform.position + new Vector3(dimensions[0] * voxelGrid.gap, dimensions[1] * voxelGrid.gap, 0)))
        {
            /*            Vector3 localpos = position - transform.position;
                        //print("X: " + localpos.x / voxelGrid.gap + " Y: " + localpos.y / voxelGrid.gap);
                        //print("Targetting voxel: " + Mathf.RoundToInt(localpos.x / voxelGrid.gap) + "   " + Mathf.RoundToInt(localpos.y / voxelGrid.gap));
                        //print(new Vector2(Mathf.RoundToInt(localpos.x / voxelGrid.gap), Mathf.RoundToInt(localpos.y / voxelGrid.gap)));
                        int localx = Mathf.RoundToInt(localpos.x / voxelGrid.gap);
                        int localy = Mathf.RoundToInt(localpos.y / voxelGrid.gap);
            */
            Vector2Int index = GetIndexOnWorldPosition(position);
            DestroyedSomething = DestroyVoxelByIndex(index.x, index.y);
        }
        else
        {
            DestroyedSomething = false;
            for (float i = -BrushSize / 2; i < BrushSize / 2 + 1; i++)
            {
                for (float j = -BrushSize / 2; j < BrushSize / 2 + 1; j++)
                {
                    Vector3 relativepos = position + new Vector3(i * voxelGrid.gap, j * voxelGrid.gap, 0);
                    if (Vector3.Distance(position, relativepos)>BrushSize/2*voxelGrid.gap) continue;
                    print(Vector3.Distance(position, relativepos));
                    Vector2Int localpos = GetIndexOnWorldPosition(relativepos);
                    int localx = localpos.x;
                    int localy = localpos.y;
                    if (PointInRangeCheck(new Vector2(localx, localy), new Vector2(0, 0), new Vector2(dimensions[0] - 1, dimensions[1] - 1)))
                    {
                        if (DestroyVoxelByIndex(localx, localy)) DestroyedSomething = true;
                    }
                }
            }
        }
        if (DestroyedSomething) {
            DestroyCuttoffs();
            ShakeCamera();
        }
    }

    void AddPointToPattern(Vector3 position)
    {
        position.z = transform.position.z;
        if (PointInRangeCheck(position, transform.position - new Vector3(voxelGrid.gap / 2, voxelGrid.gap / 2), transform.position + new Vector3(dimensions[0] * voxelGrid.gap, dimensions[1] * voxelGrid.gap, 0)))
        {
            Vector2Int index = GetIndexOnWorldPosition(position);
            if (!patterntomake.points.Contains(new Vector2Int(index.x - rootpoint[0], index.y - rootpoint[1])))
            {
                patterntomake.points.Add(new Vector2Int(index.x - rootpoint[0], index.y - rootpoint[1]));
                UpdatePatternPoints();
            }
        }
    }

    void RemovePatternPoint(Vector3 position)
    {
        position.z = transform.position.z;
        if (PointInRangeCheck(position, transform.position - new Vector3(voxelGrid.gap / 2, voxelGrid.gap / 2), transform.position + new Vector3(dimensions[0] * voxelGrid.gap, dimensions[1] * voxelGrid.gap, 0)))
        {
            Vector2Int index = GetIndexOnWorldPosition(position);
            if (patterntomake.points.Contains(new Vector2Int(index.x - rootpoint[0], index.y - rootpoint[1])))
            {
                patterntomake.points.Remove(new Vector2Int(index.x - rootpoint[0], index.y - rootpoint[1]));
                UpdatePatternPoints();
            }
            
        }
    }

    void UpdatePatternPoints()
    {
        for (int x = 0; x < dimensions[0]; x++)
        {
            for (int y = 0; y < dimensions[1]; y++)
            {
                if (availablevoxels[x, y])
                {
                    Vector2Int index = new Vector2Int(x, y);
                    Vector2Int patternindex = new Vector2Int(x - rootpoint[0], y - rootpoint[1]);
                    GameObject voxel = GetVoxelOnIndex(index);
                    if (patterntomake.points.Contains(patternindex))
                    {
                       if(voxel)voxel.GetComponent<MeshRenderer>().material.color = Color.black;
                    }
                    else if(voxel)voxel.GetComponent<MeshRenderer>().material.color = Color.white;

                }
            }
        }
        foreach (MeshRenderer renderer in rootvoxel.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.material.color = Color.red;
        }
    }
    Coroutine routine;
    GameObject finalcutout;
    public void SubmitButtonPressed()
    {
        
        if (!Submitted)
        {
            FinalScore = GetCutoutScore();
            SubmitResultText.transform.parent.gameObject.SetActive(true);
            SubmitResultText.text = (FinalScore * 100).ConvertTo(typeof(int)).ToString() + "%";
            Submitted = true;
            SubmitButtonText.text = "Продолжить";
            // Тут типа создается объект из нарисованного кринжа и начинает крутится
            finalcutout = FabricatePrefab();
            finalcutout.transform.position = transform.position + new Vector3((dimensions[0]) / 2 * voxelGrid.gap - voxelGrid.gap / 2, (dimensions[1]) / 2 * voxelGrid.gap - voxelGrid.gap / 2, voxelGrid.gap / 2) - new Vector3(0, .5f, 0);
            routine = StartCoroutine(Speen(finalcutout.transform));
        }
        else
        {
            StopCoroutine(routine);
            finalcutout.transform.rotation = Quaternion.identity;
            CutoutManager.instance.finalCutout= finalcutout;
            CutoutManager.instance.finalScore= FinalScore;
            CutoutManager.instance.StopCut();
            //вот тут мы меняем сцену обратно и вместе с заработанными очками идем рабочий реальный предмет
        }
    }

    [SerializeField] float SpinSpeed = 40;
    IEnumerator Speen(Transform transform)
    {
        while (true)
        {
            transform.Rotate(new Vector3(0, SpinSpeed , 0) * Time.deltaTime, Space.Self);
            yield return null;
        }
    }

    public float GetCutoutScore()
    {
        int maxscore = patterntomake.Size;
        int gainedscore = 0;
        for (int i = 0; i < patterntomake.points.Count; i++)
        {
            Vector2Int patternpoint=patterntomake.points[i];
            if (availablevoxels[patternpoint.x + rootpoint[0], patternpoint.y + rootpoint[1]]) gainedscore++;
        }
        for (int x = 0; x < dimensions[0]; x++)
        {
            for (int y = 0; y < dimensions[1]; y++)
            {
                if (availablevoxels[x, y] && !patterntomake.points.Contains(new Vector2Int(x - rootpoint[0], y - rootpoint[1])))
                {
                    gainedscore--;
                }
            }
        }
        print($"TotalScore: {gainedscore} MaxScore: {maxscore}");

        return Mathf.Clamp01( (float)gainedscore / (float)maxscore);
    }

    GameObject FabricatePrefab()
    {
        GameObject MainEmpty = new GameObject("MainEmpty");
        GameObject offsetempty = new GameObject("OffsetEmpty");
        GameObject meshoffsetempty = new GameObject("MeshOffsetEmpty");
        offsetempty.transform.parent = MainEmpty.transform;
        meshoffsetempty.transform.parent= offsetempty.transform;
        
        Transform emptytransform=meshoffsetempty.transform;
        GameObject actualweapon = Instantiate(voxelforprefab, emptytransform);
        actualweapon.transform.localScale = new Vector3(.5f,.5f,.5f);
        Mesh patternmesh = voxelforprefab.GetComponent<MeshFilter>().sharedMesh;
        List<MeshFilter> filtercollection = new List<MeshFilter> ();
        foreach (GameObject voxel in voxels)
        {
            if (!voxel) continue;
            //GameObject newvoxel = Instantiate(voxelforprefab, transform.position - new Vector3(dimensions[0] * voxelGrid.gap / 2, dimensions[0] * voxelGrid.gap / 2, 0), Quaternion.identity, emptytransform);
            /*GameObject newvoxel = Instantiate(voxelforprefab, emptytransform);
            newvoxel.transform.localPosition = voxel.transform.localPosition;
            newvoxel.GetComponent<MeshRenderer>().material=voxel.GetComponent<MeshRenderer>().material;*/
            if (voxel)
            {
                MeshFilter filter = voxel.GetComponent<MeshFilter>();
                filter.mesh = patternmesh;
                filtercollection.Add(filter);
            }
            
        }


        MeshFilter[] meshFilters = filtercollection.ToArray();
        CombineInstance[] instances = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            var meshFilter = meshFilters[i];

            instances[i] = new CombineInstance
            {
                mesh = meshFilter.sharedMesh,
                transform = meshFilter.transform.localToWorldMatrix,
                
                
            };

            meshFilter.gameObject.SetActive(false);
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.indexFormat = IndexFormat.UInt32;
        combinedMesh.CombineMeshes(instances);
        combinedMesh.RecalculateBounds();
        combinedMesh.OptimizeIndexBuffers();
        actualweapon.GetComponent<MeshFilter>().sharedMesh = combinedMesh;
        materialforprefab.color = CutoutManager.instance.VoxelColor;
        actualweapon.GetComponent<MeshRenderer>().material = materialforprefab;
        actualweapon.SetActive(true);
        Vector3[] objectVerts = actualweapon.GetComponent<MeshFilter>().sharedMesh.vertices;
        Vector3 offset = Vector3.zero;

        offset = actualweapon.transform.position - actualweapon.transform.TransformPoint(combinedMesh.bounds.center);

        actualweapon.transform.position = Vector3.zero;
        print(offset);
        offsetempty.transform.Rotate(0, 0, 180);
        offsetempty.transform.localPosition = new Vector3(0, combinedMesh.bounds.extents.x/2, 0);

        emptytransform.Rotate(0, 0, 90);
        actualweapon.transform.localPosition = offset;
        
        actualweapon.transform.localRotation= Quaternion.identity;
        fabricatedprefabmesh = combinedMesh;
        //offsetempty.transform.localPosition = new Vector3(0, -0.5f, 0);
        return MainEmpty;
    }

    Vector2Int GetIndexOnWorldPosition(Vector3 position)
    {
        position.z = transform.position.z;
        Vector3 localpos = position - transform.position;
        int localx = Mathf.RoundToInt(localpos.x / voxelGrid.gap);
        int localy = Mathf.RoundToInt(localpos.y / voxelGrid.gap);
        return new Vector2Int(localx, localy);
    }


    GameObject GetVoxelOnIndex(Vector2Int index)
    {
        if(PointInRangeCheck(index, new Vector2(0, 0), new Vector2(dimensions[0] - 1, dimensions[1] - 1)))
        return voxels[index.x, index.y];
        return null;
    }
    GameObject GetVoxelOnIndex(int indX,int indY)
    {
        return GetVoxelOnIndex(new  Vector2Int(indX,indY));
    }

    bool DestroyVoxelByIndex(int indX,int indY)
    {
        /*GameObject voxel;
        voxel = voxels[indX, indY];*/
        GameObject voxel = GetVoxelOnIndex(indX, indY);
        if (voxel == rootvoxel) return false;
        if (availablevoxels[indX, indY])
        {
            availablevoxels[indX, indY] = false;
            Destroy(voxel);
            return true;
        }
        return false;
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

    bool PointInRangeCheck(Vector2 point,Vector2 range1, Vector2 range2) //Проверяет находиться ли точка( Vector2 ) в пределах диапазона
    {
        //print(point);
        //print(range1);
        //print(range2);
        return (point.x >= range1.x && point.y >= range1.y) &&
               (point.x <= range2.x && point.y <= range2.y);
    }
    

    public void ShakeCamera()
    {
        StopShake();
        ShakeoriginalPosition = transform.localPosition;
        InvokeRepeating("DoShake", 0,ShakeIntensity);
        Invoke("StopShake", ShakeDuration);
    }

    private void DoShake()
    {
        float offsetX = UnityEngine.Random.Range(-ShakeMagnitude,ShakeMagnitude);
        float offsetY = UnityEngine.Random.Range(-ShakeMagnitude, ShakeMagnitude);
        transform.localPosition = ShakeoriginalPosition + new Vector3(offsetX, offsetY, 0);
    }

    private void StopShake()
    {
        CancelInvoke("DoShake");
        if(ShakeoriginalPosition!=Vector3.zero)
        transform.localPosition = ShakeoriginalPosition;
    }
}
