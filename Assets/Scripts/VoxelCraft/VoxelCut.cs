using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;





public class Voxel
{
    public Vector2Int Position;
    public bool isRoot = false;
}


public class VoxelCut : MonoBehaviour
{
    // Update is called once per frame


    //[SerializeField] public DrawingPatternData pattern;

    [SerializeField] Pattern patterntomake;
    [SerializeField] GameObject voxelforprefab;
    [SerializeField] private Material materialforprefab;

    Voxel[,] voxels;

    GameObject MainTexture;
    bool[,] availablevoxels;
    int[] dimensions;
    GenerateVoxelGrid voxelGrid;
    BoxCollider boxCollider;
    Vector2Int rootpoint;
    GameObject rootvoxel;
    [SerializeField] float BrushSize = 3;

    [SerializeField] bool ShakeCameraOnDestroy = true;
    [SerializeField] float ShakeIntensity = 0.01f;
    [SerializeField] float ShakeMagnitude = 0.1f;
    [SerializeField] float ShakeDuration = 0.4f;
    private Vector3 ShakeoriginalPosition;

    public UnityEngine.UI.Slider BrushSizeSlider = null;
    public UnityEngine.UI.Text SubmitResultText = null;
    public UnityEngine.UI.Text SubmitButtonText = null;

    public float Timer;
    public bool Submitted = false;

    //ПОТОМ УБРАТЬ НАХ*Й
    private Mesh fabricatedprefabmesh;

    public float FinalScore = 0;

    Texture2D mainsprite;
    Texture2D workabletexture;
    MeshRenderer mainrenderer;

    public void Awake()
    {

    }
    public void Start()
    {
        
        voxelGrid = GetComponent<GenerateVoxelGrid>();
        boxCollider = GetComponent<BoxCollider>();
        BrushSizeSlider = Selectable.FindAnyObjectByType<UnityEngine.UI.Slider>();
        if (SubmitResultText == null) SubmitResultText = GameObject.Find("TextScore").GetComponent<UnityEngine.UI.Text>();
        SubmitResultText.transform.parent.gameObject.SetActive(false);
        if (SubmitButtonText == null) SubmitButtonText = GameObject.Find("SubmitButton").GetComponentInChildren<Text>();
        dimensions = voxelGrid.dimensions;
        MainTexture = voxelGrid.MainTexture;
        mainrenderer = MainTexture.GetComponent<MeshRenderer>();
        mainsprite = (mainrenderer.material.mainTexture as Texture2D);
        boxCollider.size = new Vector3((dimensions[0]) * voxelGrid.gap * 2, dimensions[1] * voxelGrid.gap * 2, voxelGrid.gap);
        boxCollider.center = new Vector3((dimensions[0]) / 2 * voxelGrid.gap - voxelGrid.gap / 2, (dimensions[1]) / 2 * voxelGrid.gap, voxelGrid.gap / 2);
        rootpoint[0] = dimensions[0] - 1;
        rootpoint[1] = dimensions[1] / 2 - 1;
        SetBrushSize();
        InitializeVoxels();
        InitializeRootVoxel();

        /*        rootvoxel = MainTexture[rootpoint[0], rootpoint[1]];
                foreach (MeshRenderer renderer in rootvoxel.GetComponentsInChildren<MeshRenderer>())
                {
                    renderer.material.color = Color.red;
                }*/

        CutoutManager.instance.cuttingboard = this;
        if (!CutoutManager.instance.Debug && !CutoutManager.IsCuttingOut) CutoutManager.instance.SwitchEnableValue(false);
        if (CutoutManager.instance.Debug)
        {
            CutoutManager.instance.StartCut();
        }
    }

    void InitializeVoxels()
    {
        voxels = new Voxel[dimensions[0], dimensions[1]];
        availablevoxels = new bool[dimensions[0], dimensions[1]];
        for (int i = 0; i < dimensions[0]; i++)
        {
            for (int j = 0; j < dimensions[1]; j++)
            {
                availablevoxels[i, j] = true;
                voxels[i, j] = new Voxel
                {
                    Position = new Vector2Int(i, j),
                    isRoot = false,
                };
            }
        }
    }

    void InitializeRootVoxel()
    {
        voxels[rootpoint.x, rootpoint.y].isRoot = true;
    }


    public void CutStart()
    {
        patterntomake = CutoutManager.instance.Pattern;



        Vector2 texturesize = mainsprite.Size();


        workabletexture = new Texture2D((int)texturesize.x, (int)texturesize.y, TextureFormat.RGBA32, true);
        workabletexture.SetPixels(mainsprite.GetPixels());

        mainrenderer.material.mainTexture = workabletexture;
        //availablevoxels[0, 0] = false;
        //availablevoxels[1, 1] = false;
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
                if(hit.collider != null)
                DestroyVoxelOnPosition(hit.point);

            }
        }

#if UNITY_EDITOR
        #region DEBUG
        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            Physics.Raycast(CutoutManager.instance.cutcamera.ScreenPointToRay(Input.mousePosition), out hit, 100);

            AddPointToPattern(hit.point);
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButton(1))
        {
            RaycastHit hit;
            Physics.Raycast(CutoutManager.instance.cutcamera.ScreenPointToRay(Input.mousePosition), out hit, 100);

            RemovePatternPoint(hit.point);
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
    bool[,] CheckCutoffRecursive(Vector2Int position, bool[,] VoxelsChecked = null)
    {
        if (VoxelsChecked == null)
        {
            VoxelsChecked = new bool[dimensions[0], dimensions[1]];
        }
        if (VoxelsChecked[position.x, position.y] || !availablevoxels[position.x, position.y]) return VoxelsChecked;
        VoxelsChecked[position.x, position.y] = true;
        int newposx;
        int newposy;
        Vector2Int newposition = new Vector2Int();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (!(i == 0 || j == 0)) continue;
                newposx = position.x + i;
                newposy = position.y + j;
                if (newposx >= 0 && newposx < dimensions[0] &&
                    newposy >= 0 && newposy < dimensions[1])
                {
                    newposition.x = newposx;
                    newposition.y = newposy;
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
        //position += new Vector3(-voxelGrid.gap / 2, -voxelGrid.gap / 2);

        List<Vector2Int> ChangedIndices = new List<Vector2Int>();

        Vector2Int index = GetIndexOnWorldPosition(position);

        if (index.x < 0 || index.y < 0 || index.x > dimensions[0] || index.y > dimensions[1]) return;

        if (BrushSize == 1)
        {
            DestroyedSomething = DestroyVoxelByIndex(index.x, index.y);
            ChangedIndices.Add(index);
        }
        else
        {
            DestroyedSomething = false;
            for (float i = -BrushSize / 2; i < BrushSize / 2 + 1; i++)
            {
                for (float j = -BrushSize / 2; j < BrushSize / 2 + 1; j++)
                {
                    Vector3 relativepos = position + new Vector3(i * voxelGrid.gap, j * voxelGrid.gap, 0);
                    if (Vector3.Distance(position, relativepos) > BrushSize / 2 * voxelGrid.gap) continue;
                    //print(Vector3.Distance(position, relativepos));
                    Vector2Int localpos = GetIndexOnWorldPosition(relativepos);
                    int localx = localpos.x;
                    int localy = localpos.y;
                    if (PointInRangeCheck(new Vector2(localx, localy), new Vector2(0, 0), new Vector2(dimensions[0] - 1, dimensions[1] - 1)))
                    {
                        if (DestroyVoxelByIndex(localx, localy)){
                            DestroyedSomething = true;
                            ChangedIndices.Add(new Vector2Int(localx, localy));
                        }
                    }
                }
            }
        }
        if (DestroyedSomething)
        {
            List<Vector2Int> destroyedcutoffs=DestroyCuttoffs();
            ShakeCamera();
            if (destroyedcutoffs.Count > 0) ChangedIndices = ChangedIndices.Concat(destroyedcutoffs).ToList();
            UpdatePatternPoints(ChangedIndices);
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
                UpdatePatternPoints(index);
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
                UpdatePatternPoints(index);
            }

        }
    }

    void UpdatePatternPoints(Vector2Int point)
    {
        List<Vector2Int> pointlist= new List<Vector2Int>();
        pointlist.Add(point);
        UpdatePatternPoints(pointlist);
    }

    void UpdatePatternPoints(List<Vector2Int> tocheck=null)
    {
        //ПЕРЕПИСАТЬ ТУТ БУДЕТ КАЖДОЕ ИЗМЕНЕНИЕ(ИЛИ ПОСЛЕ СОВОКУПНОСТИ ИЗМЕНЕНИЙ ВЫРЕЗАТЬСЯ ИЗ ОСНОВНОЙ ТЕКСТУРКИ КАРТИНКА
        bool changedstuff = false;

        Vector2 texturesize = workabletexture.Size();


        int count = 0;
        if(tocheck == null)
        for (int i = 0; i < availablevoxels.GetLength(0); i++)
        {
            for (int j = 0; j < availablevoxels.GetLength(1); j++)
            {
                if(CheckVoxel(i, j, texturesize)) changedstuff=true;
            }
        }
        else
        {
            for (int i = 0; i < tocheck.Count; i++)
            {
                if(CheckVoxel(tocheck[i], texturesize)) changedstuff=true;
            }
        }
        if (changedstuff)
        {
            workabletexture.Apply();
        }

    }
    bool CheckVoxel(Vector2Int index,Vector2 texturesize)
    {
        return CheckVoxel(index.x, index.y, texturesize);
    }

    bool CheckVoxel(int i, int j, Vector2 texturesize)
    {
        UnityEngine.Color emptypixelcolor = new UnityEngine.Color(0, 0, 0, 0);
        UnityEngine.Color patternpixelcolor = new UnityEngine.Color(.5f, .5f, .5f, 1);
        UnityEngine.Color rootpointcolor = new UnityEngine.Color(1, 0, 0, 1);
        bool changedstuff=false;
        if (!availablevoxels[i, j])
        {
            changedstuff = true;
            ChangeVoxel(i, j, workabletexture, texturesize, emptypixelcolor);
        }else
        if (rootpoint.x == i && rootpoint.y == j)
        {
            changedstuff = true;
            ChangeVoxel(i, j, workabletexture, texturesize, rootpointcolor);
        }else
        if (patterntomake.points.Exists((x => x == new Vector2Int(i, j) - rootpoint)))
        {
            changedstuff = true;
            ChangeVoxel(i, j, workabletexture, texturesize, patternpixelcolor);
        } 

        
        return changedstuff;
    }

    void ChangeVoxel(int i, int j, Texture2D texture, Vector2 texturesize, UnityEngine.Color color)
    {
        float voxelSize = texturesize.x / dimensions[0];

        float gridHeight = dimensions[1] * voxelSize;
        float offsetY = (texturesize.y - gridHeight) / 2f;

        int startX = Mathf.RoundToInt(i * voxelSize);
        int endX = Mathf.RoundToInt((i + 1) * voxelSize);

        int startY = Mathf.RoundToInt(j * voxelSize + offsetY);
        int endY = Mathf.RoundToInt((j + 1) * voxelSize + offsetY);

        startX = Mathf.Clamp(startX, 0, texture.width - 1);
        endX = Mathf.Clamp(endX, 0, texture.width);
        startY = Mathf.Clamp(startY, 0, texture.height - 1);
        endY = Mathf.Clamp(endY, 0, texture.height);

        for (int k = startX; k < endX; k++)
            for (int l = startY; l < endY; l++)
                texture.SetPixel(k, l, color);
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
            DisableBoard();
            finalcutout.transform.position = transform.position + new Vector3((dimensions[0]) / 2 * voxelGrid.gap - voxelGrid.gap / 2, (dimensions[1]) / 2 * voxelGrid.gap - voxelGrid.gap / 2, voxelGrid.gap / 2) - new Vector3(0, .5f, 0);
            routine = StartCoroutine(Speen(finalcutout.transform));
        }
        else
        {
            StopCoroutine(routine);
            finalcutout.transform.rotation = Quaternion.identity;
            CutoutManager.instance.finalCutout = finalcutout;
            CutoutManager.instance.finalScore = FinalScore;
            CutoutManager.instance.StopCut();
            //вот тут мы меняем сцену обратно и вместе с заработанными очками идем рабочий реальный предмет
        }
    }

    public void Cleanup()
    {
        Destroy(finalcutout);
        
        SubmitResultText.text = "";
        SubmitButtonText.text = "Готово";
    }

    [SerializeField] float SpinSpeed = 40;
    IEnumerator Speen(Transform transform)
    {
        while (true)
        {
            transform.Rotate(new Vector3(0, SpinSpeed, 0) * Time.deltaTime, Space.Self);
            yield return null;
        }
    }

    public float GetCutoutScore()
    {
        int maxscore = patterntomake.Size;
        int gainedscore = 0;
        for (int i = 0; i < patterntomake.points.Count; i++)
        {
            Vector2Int patternpoint = patterntomake.points[i];
            if (availablevoxels[patternpoint.x + rootpoint.x, patternpoint.y + rootpoint.y]) gainedscore++;
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

        return Mathf.Clamp01((float)gainedscore / (float)maxscore);
    }

    GameObject FabricatePrefab()
    {
        GameObject MainEmpty = new GameObject("MainEmpty");
        GameObject offsetempty = new GameObject("OffsetEmpty");
        GameObject meshoffsetempty = new GameObject("MeshOffsetEmpty");
        offsetempty.transform.parent = MainEmpty.transform;
        meshoffsetempty.transform.parent = offsetempty.transform;

        Transform emptytransform = meshoffsetempty.transform;

        /*        foreach (GameObject voxel in voxels)
                {
                    if (!voxel) continue;
                    //GameObject newvoxel = Instantiate(voxelforprefab, transform.position - new Vector3(dimensions[0] * voxelGrid.gap / 2, dimensions[0] * voxelGrid.gap / 2, 0), Quaternion.identity, emptytransform);
                    *//*GameObject newvoxel = Instantiate(voxelforprefab, emptytransform);
                    newvoxel.transform.localPosition = voxel.transform.localPosition;
                    newvoxel.GetComponent<MeshRenderer>().material=voxel.GetComponent<MeshRenderer>().material;*//*
                    if (voxel)
                    {
                        MeshFilter filter = voxel.GetComponent<MeshFilter>();
                        filter.mesh = patternmesh;
                        filtercollection.Add(filter);
                    }

                }*/


        //MeshFilter[] meshFilters = filtercollection.ToArray();
        //CombineInstance[] instances = new CombineInstance[meshFilters.Length];

        //for (int i = 0; i < meshFilters.Length; i++)
        //{
        //    var meshFilter = meshFilters[i];

        //    instances[i] = new CombineInstance
        //    {
        //        mesh = meshFilter.sharedMesh,
        //        transform = meshFilter.transform.localToWorldMatrix,


        //    };

        //    meshFilter.gameObject.SetActive(false);
        //}



        int count = 0;
        for (int i = 0; i < availablevoxels.GetLength(0); i++)
        {
            for (int j = 0; j < availablevoxels.GetLength(1); j++)
            {
                if (availablevoxels[i, j]) count++;
            }
        }



        GameObject actualweapon = Instantiate(voxelforprefab, emptytransform);
        actualweapon.transform.localScale = new Vector3(.5f, .5f, .5f);


        CombineInstance[] instances = new CombineInstance[count];
        Mesh CombineMesh = voxelforprefab.GetComponent<MeshFilter>().sharedMesh;
        count = 0;
        Transform tempvoxel = Instantiate(voxelforprefab).GetComponent<Transform>();
        for (int i = 0; i < availablevoxels.GetLength(0); i++)
        {
            for (int j = 0; j < availablevoxels.GetLength(1); j++)
            {
                if (availablevoxels[i, j])
                {
                    Vector3 voxelpos = new Vector3(transform.position.x + voxelGrid.gap * i, transform.position.y + voxelGrid.gap * j, transform.position.z);
                    tempvoxel.transform.position = voxelpos;

                    instances[count] = new CombineInstance
                    {
                        mesh = CombineMesh,
                        transform = tempvoxel.localToWorldMatrix

                    };
                    count++;

                }
            }
        }
        Destroy(tempvoxel.gameObject);


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
        offsetempty.transform.localPosition = new Vector3(0, combinedMesh.bounds.extents.x / 2, 0);

        emptytransform.Rotate(0, 0, 90);
        actualweapon.transform.localPosition = offset;

        actualweapon.transform.localRotation = Quaternion.identity;
        fabricatedprefabmesh = combinedMesh;
        //offsetempty.transform.localPosition = new Vector3(0, -0.5f, 0);
        return MainEmpty;
    }

    Vector2Int GetIndexOnWorldPosition(Vector3 position)
    {
        position.z = transform.position.z;

        Vector3 bottomLeft = MainTexture.transform.position - MainTexture.transform.localScale / 2;

        // 2. Вычисляем локальную позицию относительно левого нижнего угла
        Vector2 localPos = new Vector2(position.x - bottomLeft.x, position.y - bottomLeft.y);


        float voxelworldsize = MainTexture.transform.localScale.x / dimensions[0];
        int localx = Mathf.FloorToInt(localPos.x / voxelworldsize);
        int localy = Mathf.FloorToInt(localPos.y / voxelworldsize);

        //print(position);
        //print(localPos);
        //print(localx);
        //print(localy);

        //Debug.Break();

        return new Vector2Int(localx, localy);


    }


    //ПЕРЕПИСАТЬ
    Voxel GetVoxelOnIndex(Vector2Int index)
    {
        if (PointInRangeCheck(index, new Vector2(0, 0), new Vector2(dimensions[0] - 1, dimensions[1] - 1)))
            return voxels[index.x, index.y];
        return null;
    }
    Voxel GetVoxelOnIndex(int indX, int indY)
    {
        return GetVoxelOnIndex(new Vector2Int(indX, indY));
    }

    bool DestroyVoxelByIndex(int indX, int indY)
    {
        /*GameObject voxel;
        voxel = voxels[indX, indY];*/
        Voxel voxel = GetVoxelOnIndex(indX, indY);
        if (voxel.isRoot) return false;
        if (availablevoxels[indX, indY])
        {
            availablevoxels[indX, indY] = false;
            DestroyVoxel(voxel);
            return true;
        }
        return false;
    }

    void DestroyVoxel(Voxel voxel)
    {
        //voxel.SetActive(false);
        //ТУТ ТОЖЕ ПЕРЕПИСАТЬ
    }

    public void ResetBoard()
    {
        InitializeVoxels();
        UpdatePatternPoints();
    }

    void DisableBoard()
    {
        for (int i = 0; i < availablevoxels.GetLength(0); i++)
        {
            for (int j = 0; j < availablevoxels.GetLength(1); j++)
            {
                availablevoxels[i, j] = false;
            }
        }
        UpdatePatternPoints();
    }


    List<Vector2Int> DestroyCuttoffs()
    {
        List<Vector2Int> destroyedvoxels= new List<Vector2Int>();
        bool[,] connectedvoxels = CheckCutoffRecursive(rootpoint);
        for (int i = 0; i < dimensions[0]; i++)
        {
            for (int j = 0; j < dimensions[1]; j++)
            {
                if (availablevoxels[i, j] == true && connectedvoxels[i, j] == false)
                {
                    DestroyVoxelByIndex(i, j);
                    destroyedvoxels.Add(new Vector2Int(i, j));
                }
            }
        }
        return destroyedvoxels;
    }

    bool PointInRangeCheck(Vector2 point, Vector2 range1, Vector2 range2) //Проверяет находиться ли точка( Vector2 ) в пределах диапазона
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
        InvokeRepeating("DoShake", 0, ShakeIntensity);
        Invoke("StopShake", ShakeDuration);
    }

    private void DoShake()
    {
        float offsetX = UnityEngine.Random.Range(-ShakeMagnitude, ShakeMagnitude);
        float offsetY = UnityEngine.Random.Range(-ShakeMagnitude, ShakeMagnitude);
        transform.localPosition = ShakeoriginalPosition + new Vector3(offsetX, offsetY, 0);
    }

    private void StopShake()
    {
        CancelInvoke("DoShake");
        if (ShakeoriginalPosition != Vector3.zero)
            transform.localPosition = ShakeoriginalPosition;
    }
}
