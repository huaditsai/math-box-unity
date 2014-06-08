using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeprateBox : MonoBehaviour
{
    public Camera mainCamera;

    private GameObject[] Cubes = new GameObject[10];

    // 分開和合併的距離
    private float separateDistance = 2.0f;

    // 上半段物件
    private List<GameObject> TopCubes = new List<GameObject>();
    private List<Vector3> TopCubesPosition = new List<Vector3>();

    // 下半段物件
    private List<GameObject> BottomCubes = new List<GameObject>();
    private List<Vector3> BottomCubesPosition = new List<Vector3>();

    // 物件的層數
    private const int CUBES_NUMBER = 10;

    // 被點到的物件
    private GameObject separateCube;


    // 分開
    private bool isSeparating;

    private bool isTopSeparating;

    private bool isBottomSeparating;


    // 合併
    private bool isCombining;

    private bool isTopCombining;

    private bool isBottomCombining;


    // 位移值
    private float currentMoveValue;

    // Use this for initialization
    void Start()
    {
        // 找尋每一層
        for (int i = 0; i < CUBES_NUMBER; i++)
        {
            Cubes[i] = GameObject.Find("Cube_" + (i + 1).ToString());
        }

        // 初始化
        separateCube = null;

        isSeparating = false;

        isTopSeparating = false;

        isBottomSeparating = false;

        isCombining = false;

        isTopCombining = false;

        isBottomCombining = false;

        currentMoveValue = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSeparating && !isCombining)  // 當沒有在合併或分開的時候
        {

            #region 分開
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit))
                {
                    separateCube = hit.collider.gameObject;

                    // 找到是第幾層（從名字去找）
                    int separateNumber = int.Parse(separateCube.name.Split('_')[1]);

                    TopCubes.Clear();
                    TopCubesPosition.Clear();
                    BottomCubes.Clear();
                    BottomCubesPosition.Clear();

                    // 上面一群
                    for (int i = 0; i < separateNumber - 1; i++)
                    {
                        TopCubes.Add(Cubes[i]);
                        TopCubesPosition.Add(Cubes[i].transform.position);
                    }

                    // 下面一群
                    for (int i = separateNumber; i < CUBES_NUMBER; i++)
                    {
                        BottomCubes.Add(Cubes[i]);
                        BottomCubesPosition.Add(Cubes[i].transform.position);
                    }

                    if (TopCubes.Count > 0)
                    {
                        // 如果上層最靠近對齊物件的距離需要合併
                        if (Vector3.Distance(TopCubes[TopCubes.Count - 1].transform.position, separateCube.transform.position) <= separateDistance / 2)
                        {
                            isTopSeparating = true;
                        } 
                    }

                    if (BottomCubes.Count > 0)
                    {
                        // 如果下層最靠近對齊物件的距離需要合併
                        if (Vector3.Distance(BottomCubes[0].transform.position, separateCube.transform.position) <= separateDistance / 2)
                        {
                            isBottomSeparating = true;
                        } 
                    }

                    isSeparating = isTopSeparating | isBottomSeparating;
                }
            }
            #endregion

            #region 合併
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit))
                {
                    separateCube = hit.collider.gameObject;

                    // 找到是第幾層（從名字去找）
                    int separateNumber = int.Parse(separateCube.name.Split('_')[1]);

                    TopCubes.Clear();
                    TopCubesPosition.Clear();
                    BottomCubes.Clear();
                    BottomCubesPosition.Clear();

                    // 上面一群
                    for (int i = 0; i < separateNumber - 1; i++)
                    {
                        TopCubes.Add(Cubes[i]);
                        TopCubesPosition.Add(Cubes[i].transform.position);
                    }

                    // 下面一群
                    for (int i = separateNumber; i < CUBES_NUMBER; i++)
                    {
                        BottomCubes.Add(Cubes[i]);
                        BottomCubesPosition.Add(Cubes[i].transform.position);
                    }

                    if (TopCubes.Count > 0)
                    {
                        // 如果上層最靠近對齊物件的距離需要分開
                        if (Vector3.Distance(TopCubes[TopCubes.Count - 1].transform.position, separateCube.transform.position) > separateDistance / 2)
                        {
                            isTopCombining = true;
                        } 
                    }

                    if (BottomCubes.Count > 0)
                    {
                        // 如果下層最靠近對齊物件的距離需要分開
                        if (Vector3.Distance(BottomCubes[0].transform.position, separateCube.transform.position) > separateDistance / 2)
                        {
                            isBottomCombining = true;
                        } 
                    }

                    isCombining = isTopCombining | isBottomCombining;
                }
            }
            #endregion
        }
        else if (isCombining)   // 當進行合併的時候
        {
            
            if (isTopCombining)
            {
                for (int i = 0; i < TopCubes.Count; i++)
                {
                    TopCubes[i].transform.position = TopCubesPosition[i] - new Vector3(0, currentMoveValue, 0);
                }
            }

            if (isBottomCombining)
            {
                for (int i = 0; i < BottomCubes.Count; i++)
                {
                    BottomCubes[i].transform.position = BottomCubesPosition[i] + new Vector3(0, currentMoveValue, 0);
                }
            }

            currentMoveValue += Time.smoothDeltaTime;

            if (currentMoveValue >= separateDistance / 2)
            {
                if (isTopCombining)
                {
                    for (int i = 0; i < TopCubes.Count; i++)
                    {
                        TopCubes[i].transform.position = TopCubesPosition[i] - new Vector3(0, 1, 0);
                    } 
                }

                if (isBottomCombining)
                {
                    for (int i = 0; i < BottomCubes.Count; i++)
                    {
                        BottomCubes[i].transform.position = BottomCubesPosition[i] + new Vector3(0, 1, 0);
                    } 
                }

                isTopCombining = false;
                isBottomCombining = false;
                isCombining = isTopCombining | isBottomCombining;
                currentMoveValue = 0.0f;
            }
            
        }
        else if (isSeparating)  // 當進行分開
        {
            if (isTopSeparating)
            {
                for (int i = 0; i < TopCubes.Count; i++)
                {
                    TopCubes[i].transform.position = TopCubesPosition[i] + new Vector3(0, currentMoveValue, 0);
                }
            }

            if (isBottomSeparating)
            {
                for (int i = 0; i < BottomCubes.Count; i++)
                {
                    BottomCubes[i].transform.position = BottomCubesPosition[i] - new Vector3(0, currentMoveValue, 0);
                }
            }

            currentMoveValue += Time.smoothDeltaTime;

            if (currentMoveValue >= separateDistance / 2)
            {
                if (isTopSeparating)
                {
                    for (int i = 0; i < TopCubes.Count; i++)
                    {
                        TopCubes[i].transform.position = TopCubesPosition[i] + new Vector3(0, 1, 0);
                    } 
                }

                if (isBottomSeparating)
                {
                    for (int i = 0; i < BottomCubes.Count; i++)
                    {
                        BottomCubes[i].transform.position = BottomCubesPosition[i] - new Vector3(0, 1, 0);
                    } 
                }

                isTopSeparating = false;
                isBottomSeparating = false;
                isSeparating = isTopSeparating | isBottomSeparating;
                currentMoveValue = 0.0f;
            }
        }

    }
}
