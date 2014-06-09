using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;

public class AddBox : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject box;
    private GameObject cloneBox;

    //public GameObject plane;

    public Vector3 cameraLook;
    private float cameraDistance = 8.8f;
    private float zoomBase = 0;

    private int totLevel_Y = 0; //Y的層數
    //private int currLevelShow = 0;

    private string allSeparateBtnString = "全部分開";
    private string allSeparateBtnStyle = "SpAll";
    private bool isSeparateBtn_All = false;
    private bool isSeparate_All = false;
    private Vector3[] fromPos;
    private Vector3[] toPos_All;

    private bool isSeparateBtn = false;
    private bool isMergeBtn = false;
    public Texture spText;
    public Texture clText;

    private GameObject[] Cubes;
    // 分開和合併的距離
    private float separateDistance = 2.0f;
    // 上半段物件
    private List<GameObject> TopCubes = new List<GameObject>();
    private List<Vector3> TopCubesPosition = new List<Vector3>();
    // 下半段物件
    private List<GameObject> BottomCubes = new List<GameObject>();
    private List<Vector3> BottomCubesPosition = new List<Vector3>();

    // 被點到的物件
    private GameObject separateCube;
    // 分開
    private bool isSeparating = false;
    private bool isTopSeparating = false;
    private bool isBottomSeparating = false;
    // 合併
    private bool isCombining = false;
    private bool isTopCombining = false;
    private bool isBottomCombining = false;
    // 位移值
    private float currentMoveValue = 0;



    float minX = 500, minY = 500, minZ = 500;
    float maxX = 500, maxY = 500, maxZ = 500;

    // Use this for initialization
    void Start()
    {
        int count = 0;
        float posX, posY, posZ;

        //Common.matrix_size = 10;
        for (int y = 0; y < Common.matrix_size; y++)
        {
            GameObject obj = new GameObject(); //把每層分群
            obj.name = y.ToString();
            obj.transform.position = new Vector3(0, -Common.matrix_size / 2 + y, 0);
            if (Common.matrix_size % 2 == 0)
                obj.transform.position += Vector3.up * 0.5f;
            count = 0;

            for (int x = 0; x < Common.matrix_size; x++)
                for (int z = 0; z < Common.matrix_size; z++)
                {
                    if (Common.matrix[x, y, z] == 1)
                    {
                        posX = -Common.matrix_size / 2 + x;
                        posY = -Common.matrix_size / 2 + y;
                        posZ = -Common.matrix_size / 2 + z;

                        if (Common.matrix_size % 2 == 0)
                        {
                            posX += 0.5f;
                            posY += 0.5f;
                            posZ += 0.5f;
                        }

                        count++;
                        cloneBox = Instantiate(box, new Vector3(posX, posY, posZ), Quaternion.identity) as GameObject;
                        cloneBox.transform.parent = obj.transform;

                        if (minX == 500)
                        {
                            minX = posX;
                            maxX = posX;
                        }

                        if (posX < minX)
                            minX = posX;
                        if (posX > maxX)
                            maxX = posX;

                        if (minY == 500)
                        {
                            minY = posY;
                            maxY = posY;
                        }

                        if (posY < minY)
                            minY = posY;
                        if (posY > maxY)
                            maxY = posY;

                        if (minZ == 500)
                        {
                            minZ = posZ;
                            maxZ = posZ;
                        }

                        if (posZ < minZ)
                            minZ = posZ;
                        if (posZ > maxZ)
                            maxZ = posZ;

                    }
                }

            if (count == 0)
                Destroy(GameObject.Find(y.ToString()));
            else
                totLevel_Y++;
        }

        if (Common.matrix_size > 5)
            zoomBase = Common.matrix_size - 2;
        else
            zoomBase = Common.matrix_size;

        mainCamera.orthographicSize = zoomBase;
        cameraLook = new Vector3((maxX + minX) / 2f, (maxY + minY) / 2f, (maxZ + minZ) / 2f);
        mainCamera.transform.LookAt(cameraLook);


        fromPos = new Vector3[totLevel_Y];
        toPos_All = new Vector3[totLevel_Y];
        //toPos_One = new Vector3[totLevel_Y];
        //isMoved = new bool[totLevel_Y];

        //for (int i = 0; i < totLevel_Y; i++)
        //{
        //    isMoved[i] = false;
        //}

        Cubes = new GameObject[totLevel_Y];
        for (int i = 0; i < totLevel_Y; i++)
        {
            Cubes[i] = GameObject.Find(i.ToString());
        }


        for (int i = 0; i < totLevel_Y; i++)
        {
            fromPos[i] = GameObject.Find(i.ToString()).transform.position;

            if (totLevel_Y % 2 == 0)
            {
                if (i < totLevel_Y / 2)
                    toPos_All[i] = fromPos[i] + Vector3.down * (totLevel_Y / 2 - i - 0.5f);
                else
                    toPos_All[i] = fromPos[i] + Vector3.up * (i - totLevel_Y / 2 + 0.5f);
            }
            else
            {
                if (i != totLevel_Y / 2) //中間的不動
                {
                    if (i < totLevel_Y / 2)
                        toPos_All[i] = fromPos[i] + Vector3.down * (totLevel_Y / 2 - i);
                    else
                        toPos_All[i] = fromPos[i] + Vector3.up * (i - totLevel_Y / 2);
                }
            }
        }

        RenderSettings.skybox = materials[2];
    }

    public Vector3 mousepos;

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.Escape))
        //    Screen.fullScreen = false;

        if (isZoom)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomTo, Time.deltaTime * 5f);
        }


        if (isSeparateBtn_All) //展開按鈕
        {
            if (isSeparate_All) //全部展開
                MoveBox(toPos_All);
            else //全部合併
                MoveBox(fromPos);
        }

        SeprateCombine();

        GameObject obj = null;
        if ((obj = GameObject.Find("0")) != null)
            cameraLook.y = (GameObject.Find("0").transform.position.y + GameObject.Find((totLevel_Y - 1).ToString()).transform.position.y) / 2;

    }

    private void MoveBox(Vector3[] to)
    {
        GameObject obj = null;
        for (int i = 0; i < totLevel_Y; i++)
        {
            if ((obj = GameObject.Find(i.ToString())) != null)
                GameObject.Find(i.ToString()).transform.position = Vector3.Lerp(GameObject.Find(i.ToString()).transform.position, to[i], Time.smoothDeltaTime * 3.5f);                      
        }

        currentMoveValue += Time.smoothDeltaTime;
        if (currentMoveValue >= separateDistance / 2)
        {
            for (int i = 0; i < totLevel_Y; i++)
            {
                if ((obj = GameObject.Find(i.ToString())) != null)
                    GameObject.Find(i.ToString()).transform.position = to[i];
            }
            isSeparateBtn_All = false;
            currentMoveValue = 0;
        }
    }

    bool isStartTimer = false;
    int countDownTime = 1;
    void FixedUpdate()
    {
        index++;
        if (index > 1)
            index = 0;

        if (isStartTimer)
        {
            countDownTime--;
            if (countDownTime <= 0)
            {
                isHideGUI = false;
                isSaveDialog = true;
                isSaveOK = true;
                RenderSettings.skybox = materials[2]; //orange
                mainCamera.GetComponentInChildren<MeshRenderer>().enabled = true;

                StartCoroutine("LoadImage", path);

                countDownTime = 2;
                isStartTimer = false;
            }
        }
    }

    bool isHideGUI = false;

    public float rotateY = 1.225706f;
    public float rotateZ = 1.394608f;

    public Texture[] backgroundTexture;
    private int index = 0;

    public Texture titleTexture;
    public Texture rotateTexture;

    public GUISkin gSkin;
    public Texture[] btnGoBackTexture;
    public Texture[] zoomTexture;
    public Texture[] btnSettingTexture;
    public Texture renderTextureBack; //要有兩個camera才Build成功
    public RenderTexture renderTexture;
    private Rect renderTextureRect = new Rect();

    public Texture exitWindowBackTexture;
    private bool isExitDialog = false;

    public Texture saveWindowBackTexture;
    private bool isSaveDialog = false;

    //private bool isShowSetting = false;
    private string countText = "觀看數量";
    private bool isShowCount = false;
    private int count = 0;

    private int zoomPercent = 100;
    private bool isZoom = false;
    private float zoomTo = 0;

    private bool isHideLine = false;
    private string lineText = "隱藏線條";
    public Material[] materials;

    private float screen_width, screen_height;
    private float screenBlack = 0;
    float texture_width;
    float texture_height;
    float scale = 0.3f;

    void OnGUI()
    {
        if (gSkin)
            GUI.skin = gSkin;

        screen_width = screen_height * (4f / 3f);
        screen_height = Screen.height;

        screenBlack = (Screen.width - screen_height * (4f / 3f)) / 2f;

        if (screen_width < screen_height)
        {
            texture_width = screen_width;
            texture_height = screen_width;
        }
        else
        {
            texture_width = screen_height;
            texture_height = screen_height;
        }

        GUI.DrawTexture(new Rect(screenBlack + 0, 0, screen_width, screen_height), backgroundTexture[index], ScaleMode.ScaleToFit);

        //主要
        //scale = 0.73f;
        //GUI.DrawTexture(new Rect(screenBlack + screen_width * 0.47f - texture_width * scale * 0.5f, screen_height * 0.48f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), renderTextureBack);
        scale = 0.725f;
        renderTextureRect = new Rect(screenBlack + screen_width * 0.405f - texture_width * scale * 0.5f, screen_height * 0.53f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale);
        //GUI.DrawTexture(renderTextureRect, renderTextureBack);

        scale = 0.74f;
        mainCamera.rect = new Rect((screen_width * 0.405f - texture_width * scale * 0.5f) / screen_width, (screen_height * 0.53f - texture_height * scale * 0.5f) / screen_height, texture_width * scale / screen_width, texture_height * scale / screen_height);

        //旋轉
        scale = 0.73f;
        gSkin.FindStyle("horizontalsliderthumb").overflow = new RectOffset(0, 0, (int)(texture_height * scale * 0.02f), -(int)(texture_height * scale * 0.05f));
        rotateZ = GUI.HorizontalSlider(new Rect(screenBlack + screen_width * 0.4f - texture_width * scale * 0.5f, screen_height * 0.91f - texture_height * scale * 0.5f * 0.1f, texture_width * scale, texture_height * scale * 0.1f), rotateZ, 0.0001f, 2f * Mathf.PI, "horizontalslider", "horizontalsliderthumb");
        rotateY = GUI.VerticalSlider(new Rect(screenBlack + screen_width * 0.74f - texture_width * scale * 0.5f * 0.1f, screen_height * 0.47f - texture_height * scale * 0.5f, texture_width * scale * 0.03f, texture_height * scale), rotateY, Mathf.PI - 0.0001f, 0.0001f, "VerticalSlider", "VerticalSliderthumb");
        mainCamera.transform.position = new Vector3(
           cameraLook.x + cameraDistance * Mathf.Sin(rotateY) * Mathf.Cos(rotateZ),
           cameraLook.y + cameraDistance * Mathf.Cos(rotateY),
           cameraLook.z - cameraDistance * Mathf.Sin(rotateY) * Mathf.Sin(rotateZ));
        mainCamera.transform.LookAt(cameraLook);

        scale = 0.08f;
        GUI.DrawTexture(new Rect(screenBlack + screen_width * 0.66f + texture_width * scale * 0.5f, screen_height * 0.88f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), rotateTexture, ScaleMode.ScaleAndCrop);

        //縮放按鈕
        if (!isHideGUI)
        {
            scale = 0.06f;
            if (GUI.Button(new Rect(screenBlack + screen_width * 0.535f - texture_width * scale * 0.5f, screen_height * 0.8f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), zoomTexture[1], "Zoom"))
            {
                zoomTo = mainCamera.orthographicSize + 0.2f;
                if (zoomTo < 10f)
                    isZoom = true;
                else
                    zoomTo = 10f;

                if (zoomTo > zoomBase)
                    zoomPercent = 100 - (int)((zoomTo - zoomBase) / (10f - zoomBase) * 100f);
                else if (zoomTo < zoomBase)
                    zoomPercent = 100 + (int)((zoomBase - zoomTo) / (zoomBase - 0.5f) * 100f);
                else
                    zoomPercent = 100;
            }
            gSkin.FindStyle("ZoomPercent").fontSize = (int)(texture_height * scale * 0.5f);
            //gSkin.FindStyle("ZoomPercent").contentOffset = new Vector2(0, (int)(texture_height * scale * 0.01f));
            if (GUI.Button(new Rect(screenBlack + screen_width * 0.59f - texture_width * scale * 0.9f, screen_height * 0.8f - texture_height * scale * 0.5f, texture_width * scale * 1.8f, texture_height * scale), zoomPercent.ToString() + "%", "ZoomPercent"))
            {
                if (Common.matrix_size > 5)
                    zoomBase = Common.matrix_size - 2;
                else
                    zoomBase = Common.matrix_size;

                //caamera.orthographicSize = Common.matrix_size;
                zoomTo = zoomBase;
                isZoom = true;
                zoomPercent = 100;
            }
            if (GUI.Button(new Rect(screenBlack + screen_width * 0.65f - texture_width * scale * 0.5f, screen_height * 0.8f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), zoomTexture[0], "Zoom"))
            {
                zoomTo = mainCamera.orthographicSize - 0.2f;
                if (zoomTo > 0.5f)
                    isZoom = true;
                else
                    zoomTo = 0.5f;

                if (zoomTo > zoomBase)
                    zoomPercent = 100 - (int)((zoomTo - zoomBase) / (10f - zoomBase) * 100f);
                else if (zoomTo < zoomBase)
                    zoomPercent = 100 + (int)((zoomBase - zoomTo) / (zoomBase - 0.5f) * 100f);
                else
                    zoomPercent = 100;
            }
        }

        //回上頁
        scale = 0.11f;
        if (Common.lastLevel == "MatrixMenuTwo")
            if (GUI.Button(new Rect(screenBlack + screen_width * 0.05f - texture_width * scale * 0.5f, screen_height * 0.93f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), btnGoBackTexture[0], "BtnGoBack"))
            {
                Application.LoadLevel("MatrixMenuTwo"); //變化組合回到編輯, 範例回到範例選擇
            }

        scale = 0.15f;
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.05f - texture_width * scale * 0.5f, screen_height * 0.07f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), "", "BtnGoExit"))
        {
            isExitDialog = true;
            //Application.Quit(); //離開
        }

        ////設定
        //if (GUI.Button(new Rect(screen_width * 0.95f - texture_width * scale * 0.5f, screen_height * 0.93f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), btnSettingTexture[0], "BtnGoBack"))
        //{
        //    isShowSetting = !isShowSetting;
        //}

        ////顯示數量
        //if (isShowCount)
        //{
        //    scale = 0.7f;
        //    gSkin.FindStyle("Count").fontSize = (int)(texture_height * scale * 0.23f);
        //    GUI.Label(new Rect(screen_width * 0.83f - texture_width * scale * 0.5f, screen_height * 0.7f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), count + " 個", "Count");
        //}

        scale = 0.27f;
        //if (isShowSetting)
        //{
        gSkin.FindStyle("Settings").fontSize = (int)(texture_height * scale * 0.2f);
        gSkin.FindStyle("Settings").contentOffset = new Vector2(0, (int)(texture_height * scale * 0.01f));
        //設定們
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.88f - texture_width * scale * 0.5f, screen_height * 0.24f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), "另存圖片", "Settings"))
        {
            isSaveDialog = true;

            //string path = "";
            //if (Application.platform == RuntimePlatform.WindowsPlayer)
            //{
            //    //1. Copy ".\Program Files (x86)\Unity\Editor\Data\Mono\lib\mono\2.0\System.Windows.Forms.dll"
            //    //To Assets\Plugins 2. Change player setting ".NET 2.0 Subset" To ".NET 2.0"
            //    System.Windows.Forms.SaveFileDialog saveLog = new System.Windows.Forms.SaveFileDialog();
            //    saveLog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            //    saveLog.Filter = "Image Files(*.JPG)|*.jpg;*|All files (*.*)|*.*";
            //    System.Windows.Forms.DialogResult result = saveLog.ShowDialog();
            //    if (result == System.Windows.Forms.DialogResult.OK)
            //        path = saveLog.FileName;
            //}
            //else
            //    path = Application.persistentDataPath + "/SavedScreen.jpg";

            //if (path.Length != 0)
            //{
            //    RenderTexture mainRender = renderTexture[0] as RenderTexture;
            //    Texture2D myTexture2D = new Texture2D(mainRender.width, mainRender.height);
            //    RenderTexture.active = mainRender;
            //    myTexture2D.ReadPixels(new Rect(0, 0, mainRender.width, mainRender.height), 0, 0);
            //    myTexture2D.Apply();

            //    //var bytes = myTexture2D.EncodeToPNG();
            //    //var file = File.Open(Application.persistentDataPath + "/SavedScreen.jpg", FileMode.Create);
            //    //var binary = new BinaryWriter(file);
            //    //binary.Write(bytes);
            //    //file.Close();

            //    File.WriteAllBytes(path, myTexture2D.EncodeToPNG());

            //    //isShowSetting = false;
            //}

        }


        if (!isShowCount)
        {
            gSkin.FindStyle("Settings2").fontSize = (int)(texture_height * scale * 0.2f);
            gSkin.FindStyle("Settings2").contentOffset = new Vector2(0, (int)(texture_height * scale * 0.01f));
        }
        else
        {
            gSkin.FindStyle("Settings2").fontSize = (int)(texture_height * scale * 0.4f);
            gSkin.FindStyle("Settings2").contentOffset = new Vector2(0, 0);
        }
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.88f - texture_width * scale * 0.5f, screen_height * 0.453f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), countText, "Settings2"))
        {
            count = 0;
            if (!isShowCount)
            {
                for (int x = 0; x < Common.matrix_size; x++)
                    for (int y = 0; y < Common.matrix_size; y++)
                        for (int z = 0; z < Common.matrix_size; z++)
                            if (Common.matrix[x, y, z] == 1)
                                count++;

                countText = count.ToString();//"隱藏數量";
            }
            else
                countText = "觀看數量";

            //isShowSetting = false;
            isShowCount = !isShowCount;
        }
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.88f - texture_width * scale * 0.5f, screen_height * 0.666f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), lineText, "Settings"))
        {
            if (!isHideLine) //隱藏
            {
                foreach (GameObject item in GameObject.FindGameObjectsWithTag("Box"))
                    item.renderer.material = materials[1];
                mainCamera.GetComponent<EdgeDetectEffectNormals>().enabled = true;
                mainCamera.GetComponent<AntialiasingAsPostEffect>().enabled = true;

                lineText = "顯示線條";
            }
            else //顯示線條
            {
                foreach (GameObject item in GameObject.FindGameObjectsWithTag("Box"))
                    item.renderer.material = materials[0];
                mainCamera.GetComponent<EdgeDetectEffectNormals>().enabled = false;
                mainCamera.GetComponent<AntialiasingAsPostEffect>().enabled = false;

                lineText = "隱藏線條";
            }

            //isShowSetting = false;
            isHideLine = !isHideLine;
        }
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.88f - texture_width * scale * 0.5f, screen_height * 0.88f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), "回主選單", "Settings"))
        {
            Common.init();
            //isShowSetting = false;
            Application.LoadLevel("MainMenu");
        }
        //} 

        //分層按鈕
        scale = 0.15f;
        gSkin.FindStyle(allSeparateBtnStyle).fontSize = (int)(texture_height * scale * 0.2f);
        gSkin.FindStyle(allSeparateBtnStyle).contentOffset = new Vector2(0, (int)(texture_height * scale * 0.01f));
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.065f - texture_width * scale * 0.5f, screen_height * 0.35f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), allSeparateBtnString, allSeparateBtnStyle))
        {
            isSeparateBtn_All = true;
            isSeparateBtn = false;
            isMergeBtn = false;

            if (isSeparate_All)
            {
                allSeparateBtnString = "全部分開";
                allSeparateBtnStyle = "SpAll";
            }
            else
            {
                allSeparateBtnString = "全部組合";
                allSeparateBtnStyle = "CloseAll";
            }

            isSeparate_All = !isSeparate_All;
        }
        gSkin.FindStyle("SpOne").fontSize = (int)(texture_height * scale * 0.2f);
        gSkin.FindStyle("SpOne").contentOffset = new Vector2(0, (int)(texture_height * scale * 0.01f));
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.065f - texture_width * scale * 0.5f, screen_height * 0.6f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), "單層分開", "SpOne"))
        {
            //isSeparateBtn_All = false;
            isMergeBtn = false;
            isSeparateBtn = !isSeparateBtn;
        }
        gSkin.FindStyle("CloseOne").fontSize = (int)(texture_height * scale * 0.2f);
        gSkin.FindStyle("CloseOne").contentOffset = new Vector2(0, (int)(texture_height * scale * 0.01f));
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.065f - texture_width * scale * 0.5f, screen_height * 0.7f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), "單層組合", "CloseOne"))
        {
            //isSeparateBtn_All = false;
            isSeparateBtn = false;
            isMergeBtn = !isMergeBtn;
        }

        scale = 0.035f;
        if (isSeparateBtn)
            GUI.DrawTexture(new Rect(screenBlack + screen_width * 0.41f - texture_width * scale * 20f * 0.5f, screen_height * 0.05f - texture_height * scale * 0.5f, texture_width * scale * 20f, texture_height * scale), spText, ScaleMode.ScaleAndCrop);
        if (isMergeBtn)
            GUI.DrawTexture(new Rect(screenBlack + screen_width * 0.41f - texture_width * scale * 20f * 0.5f, screen_height * 0.05f - texture_height * scale * 0.5f, texture_width * scale * 20f, texture_height * scale), clText, ScaleMode.ScaleAndCrop);
        

        //標題        
        //scale = 0.4f;
        //gSkin.FindStyle("Title").fontSize = (int)(texture_height * scale * 0.2f);
        //GUI.Label(new Rect(screen_width * 0.85f - texture_width * scale * 0.5f, screen_height * 0.25f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), "數一數，\n有幾個？", "Title");
        //GUI.DrawTexture(new Rect(screen_width * 0.85f - texture_width * scale * 0.5f, screen_height * 0.23f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), titleTexture, ScaleMode.StretchToFill);
        //gSkin.FindStyle("SubTitle").fontSize = (int)(texture_height * scale * 0.18f);
        //GUI.Label(new Rect(screen_width * 0.83f - texture_width * scale * 0.5f, screen_height * 0.27f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), "有幾個？", "SubTitle");

        if (isSaveDialog)
        {
            GUI.DrawTexture(new Rect(screenBlack + 0, 0, screen_width, screen_height), saveWindowBackTexture, ScaleMode.StretchToFill);
            scale = 0.7f;
            GUI.ModalWindow(0, new Rect(screenBlack + screen_width * 0.5f - texture_width * scale * 1.2f * 0.5f, screen_height * 0.5f - texture_height * scale * 0.5f, texture_width * scale * 1.2f, texture_height * scale), SaveWindow, "", "SaveWndow");
        }

        if (isExitDialog)
        {
            GUI.DrawTexture(new Rect(screenBlack + 0, 0, screen_width, screen_height), saveWindowBackTexture, ScaleMode.StretchToFill);
            scale = 0.7f;
            GUI.ModalWindow(0, new Rect(screenBlack + screen_width * 0.5f - texture_width * scale * 1.2f * 0.5f, screen_height * 0.5f - texture_height * scale * 0.5f, texture_width * scale * 1.2f, texture_height * scale), ExitWindow, "", "ExitWindow");
        }

        //黑邊
        DrawBlack(new Rect(0, 0, screenBlack, Screen.height));
        DrawBlack(new Rect(Screen.width - screenBlack, 0, screenBlack, Screen.height));

    }

    private void DrawBlack(Rect rect) //黑邊
    {
        Texture2D blackTexture = new Texture2D(1, 1);
        blackTexture.SetPixel(0, 0, Color.black);
        blackTexture.wrapMode = TextureWrapMode.Repeat;
        blackTexture.Apply();

        GUI.DrawTexture(rect, blackTexture);
    }

    private void ExitWindow(int id) //存檔畫面
    {
        scale = 0.25f;

        if (GUI.Button(new Rect(screen_width * 0.22f - texture_width * scale * 0.5f, screen_height * 0.49f - texture_height * scale * 0.3f * 0.5f, texture_width * scale, texture_height * scale * 0.3f), "", "ExitOk"))
        {
            Application.Quit();
        }

        if (GUI.Button(new Rect(screen_width * 0.42f - texture_width * scale * 0.5f, screen_height * 0.49f - texture_height * scale * 0.3f * 0.5f, texture_width * scale, texture_height * scale * 0.3f), "", "SaveCancle"))
        {
            isExitDialog = false;
        }
    }

    public Shader shader;
    bool isSaveOK = false;
    string path = "";

    private void SaveWindow(int id) //存檔畫面
    {
        scale = 0.35f;

        if (!isSaveOK)
        {
            path = "";

            if (GUI.Button(new Rect(screen_width * 0.32f - texture_width * scale * 0.5f, screen_height * 0.23f - texture_height * scale * 0.2f * 0.5f, texture_width * scale, texture_height * scale * 0.2f), "", "SaveDeskTop"))
            {
                path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            }
            if (GUI.Button(new Rect(screen_width * 0.32f - texture_width * scale * 0.5f, screen_height * 0.33f - texture_height * scale * 0.2f * 0.5f, texture_width * scale, texture_height * scale * 0.2f), "", "SaveMyDoc"))
            {
                path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            }
            if (GUI.Button(new Rect(screen_width * 0.32f - texture_width * scale * 0.5f, screen_height * 0.43f - texture_height * scale * 0.2f * 0.5f, texture_width * scale, texture_height * scale * 0.2f), "", "SaveMyPic"))
            {
                path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures) + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            }

            if (path.Length != 0)
            {
                //if (!isHideLine)
                //{
                //    RenderTexture mainRender = renderTexture;
                //    mainCamera.targetTexture = mainRender;
                //    Texture2D myTexture2D = new Texture2D(mainRender.width, mainRender.height);
                //    mainCamera.Render();
                //    RenderTexture.active = mainRender;
                //    myTexture2D.ReadPixels(new Rect(0, 0, mainRender.width, mainRender.height), 0, 0);
                //    myTexture2D.Apply();

                //    mainCamera.targetTexture = null;
                //    RenderTexture.active = null;

                //    File.WriteAllBytes(path, myTexture2D.EncodeToPNG());
                //    isSaveOK = true;
                //}
                //else
                //{
                isHideGUI = true;
                isSaveDialog = false;
                RenderSettings.skybox = materials[1]; //white
                mainCamera.GetComponentInChildren<MeshRenderer>().enabled = false;
                Application.CaptureScreenshot(path);

                //LoadImage("C:\\Users\\huadi\\Desktop\\20140606005721.jpg");
                isStartTimer = true;
                //}
            }


            scale = 0.25f;
            if (GUI.Button(new Rect(screen_width * 0.32f - texture_width * scale * 0.5f, screen_height * 0.57f - texture_height * scale * 0.3f * 0.5f, texture_width * scale, texture_height * scale * 0.3f), "", "SaveCancle"))
            {
                isSaveDialog = false;
            }
        }

        if (isSaveOK)
        {
            scale = 0.2f;
            if (GUI.Button(new Rect(screen_width * 0.32f - texture_width * scale * 0.5f, screen_height * 0.33f - texture_height * scale * 0.15f * 0.5f, texture_width * scale, texture_height * scale * 0.15f), "", "SaveOK"))
            { }

            if (Input.anyKeyDown)
            {
                isSaveOK = false;
                isSaveDialog = false;
            }
        }
    }

    private IEnumerator LoadImage(string path) //因為renderTexture我存不了shader
    {
        WWW www = new WWW("file:///" + path);
        yield return www;
        Texture2D tmpTexture = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);
        www.LoadImageIntoTexture(tmpTexture);

        Texture2D myTexture2D = new Texture2D((int)renderTextureRect.width, (int)renderTextureRect.height, TextureFormat.ARGB32, false);
        for (int y = (int)renderTextureRect.yMin; y < (int)renderTextureRect.yMax; y++)
            for (int x = (int)renderTextureRect.xMin; x < (int)renderTextureRect.xMax; x++)
                myTexture2D.SetPixel(x - (int)renderTextureRect.xMin, y - (int)renderTextureRect.yMin, tmpTexture.GetPixel(x, y));

        //Texture2D myTexture2D = new Texture2D(tmpTexture.width, tmpTexture.height, TextureFormat.ARGB32, false);
        //for (int y = 0; y < tmpTexture.height; y++)
        //{
        //    for (int x = 0; x < tmpTexture.width; x++)
        //    {                
        //        if (renderTextureRect.xMin < x && x < renderTextureRect.xMax && renderTextureRect.yMin < y && y < renderTextureRect.yMax)
        //            myTexture2D.SetPixel(x, y, tmpTexture.GetPixel(x, y));
        //        else
        //            myTexture2D.SetPixel(x, y, Color.clear);
        //    }            
        //}

        myTexture2D.Apply();
        File.WriteAllBytes(path, myTexture2D.EncodeToPNG());
    }



    void SeprateCombine()
    {
        if (!isSeparating && !isCombining)  // 當沒有在合併或分開的時候
        {
            #region 分開
            if (Input.GetMouseButtonDown(0) && isSeparateBtn)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit))
                {
                    separateCube = GameObject.Find(hit.transform.parent.name);
                    // 找到是第幾層（從名字去找）
                    int separateNumber = int.Parse(hit.transform.parent.name);

                    TopCubes.Clear();
                    TopCubesPosition.Clear();
                    BottomCubes.Clear();
                    BottomCubesPosition.Clear();

                    // 下面一群
                    for (int i = 0; i < separateNumber; i++)
                    {
                        BottomCubes.Add(Cubes[i]);
                        BottomCubesPosition.Add(Cubes[i].transform.position);
                    }

                    // 上面一群
                    for (int i = separateNumber + 1; i < totLevel_Y; i++)
                    {
                        TopCubes.Add(Cubes[i]);
                        TopCubesPosition.Add(Cubes[i].transform.position);
                    }

                    if (TopCubes.Count > 0)
                    {
                        // 如果上層最靠近對齊物件的距離需要合併
                        if (Vector3.Distance(TopCubes[0].transform.position, separateCube.transform.position) <= separateDistance / 2)
                        {
                            isTopSeparating = true;
                        }
                    }

                    if (BottomCubes.Count > 0)
                    {
                        // 如果下層最靠近對齊物件的距離需要合併
                        if (Vector3.Distance(BottomCubes[BottomCubes.Count - 1].transform.position, separateCube.transform.position) <= separateDistance / 2)
                        {
                            isBottomSeparating = true;
                        }
                    }

                    if (isSeparateBtn)
                        isSeparating = isTopSeparating | isBottomSeparating;
                }
            }
            #endregion

            #region 合併
            if (Input.GetMouseButtonDown(0) && isMergeBtn)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit))
                {
                    separateCube = GameObject.Find(hit.transform.parent.name);
                    // 找到是第幾層（從名字去找）
                    int separateNumber = int.Parse(hit.transform.parent.name);

                    TopCubes.Clear();
                    TopCubesPosition.Clear();
                    BottomCubes.Clear();
                    BottomCubesPosition.Clear();

                    // 下面一群
                    for (int i = 0; i < separateNumber; i++)
                    {
                        BottomCubes.Add(Cubes[i]);
                        BottomCubesPosition.Add(Cubes[i].transform.position);
                    }

                    // 上面一群
                    for (int i = separateNumber + 1; i < totLevel_Y; i++)
                    {
                        TopCubes.Add(Cubes[i]);
                        TopCubesPosition.Add(Cubes[i].transform.position);
                    }

                    if (TopCubes.Count > 0)
                    {
                        // 如果上層最靠近對齊物件的距離需要分開
                        if (Vector3.Distance(TopCubes[0].transform.position, separateCube.transform.position) > separateDistance / 2)
                        {
                            isTopCombining = true;
                        }
                    }

                    if (BottomCubes.Count > 0)
                    {
                        // 如果下層最靠近對齊物件的距離需要分開
                        if (Vector3.Distance(BottomCubes[BottomCubes.Count - 1].transform.position, separateCube.transform.position) > separateDistance / 2)
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
