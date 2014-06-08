using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System;

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
    private bool isSeparate_One = false;
    private int separate_name = -1;
    private Vector3[] toPos_One;
    private bool[] isMoved;

    private bool isMergeBtn = false;

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
        toPos_One = new Vector3[totLevel_Y];
        isMoved = new bool[totLevel_Y];

        for (int i = 0; i < totLevel_Y; i++)
        {
            isMoved[i] = false;
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

        if (isSeparateBtn || isMergeBtn)
        {
            //Get the point on the plane
            mousepos = Input.mousePosition;
            //mousepos.y = Screen.height - mousepos.y;
            //mousepos = mousepos - new Vector3(renderTextureRect.xMin, renderTextureRect.yMin, 0);

            ////Convert the coordinate to the mapCam's resolutiion
            //mousepos.x *= camera.pixelWidth / renderTextureRect.width;
            //mousepos.y *= camera.pixelHeight / renderTextureRect.height;
            //mousepos.y = camera.pixelHeight - mousepos.y;

            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(mainCamera.ScreenPointToRay(mousepos), out hit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //print(hit.transform.parent.name);
                    separate_name = int.Parse(hit.transform.parent.name);

                    //因為Group是擺好後加上的，所以座標一樣時就是黏住                    
                    for (int i = separate_name - 1; i <= separate_name + 1; i++)
                    {
                        if (0 <= i && i < totLevel_Y)
                        {
                            if (isSeparateBtn && !isMoved[i])
                            {
                                toPos_One[i] = toPos_All[i];
                                isMoved[i] = true;
                            }
                            else if (isMergeBtn)
                            {
                                toPos_One[i] = fromPos[i];
                                isMoved[i] = false;
                            }
                        }
                    }
                    for (int i = separate_name + 2; i < totLevel_Y; i++) //上
                    {
                        if (isSeparateBtn && !isMoved[i])
                            toPos_One[i] = toPos_One[i - 1];
                        else if (isMergeBtn && isMoved[i])
                            toPos_One[i] = GameObject.Find(i.ToString()).transform.position + fromPos[separate_name + 1] - toPos_All[separate_name + 1];
                    }
                    for (int i = separate_name - 2; i >= 0; i--) //下 
                    {
                        if (isSeparateBtn && !isMoved[i])
                            toPos_One[i] = toPos_One[i + 1];
                        else if (isMergeBtn && isMoved[i])
                            toPos_One[i] = GameObject.Find(i.ToString()).transform.position + fromPos[separate_name - 1] - toPos_All[separate_name - 1];
                    }

                    isSeparate_One = true;
                }
            }
        }

        if ((isMergeBtn || isSeparateBtn) && isSeparate_One)
        {
            MoveBox(toPos_One);
        }
    }

    private void MoveBox(Vector3[] to)
    {
        GameObject obj = null;
        for (int i = 0; i < totLevel_Y; i++)
        {
            if ((obj = GameObject.Find(i.ToString())) != null)
                GameObject.Find(i.ToString()).transform.position = Vector3.Lerp(GameObject.Find(i.ToString()).transform.position, to[i], Time.deltaTime * 5f);
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

                StartCoroutine("LoadImage", path);

                countDownTime = 2;
                isStartTimer = false;
            }
        }
    }

    bool isHideGUI = false;

    private float rotateY = Mathf.PI / 3;
    private float rotateZ = 2 * Mathf.PI / 3;

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
        scale = 0.70f;
        renderTextureRect = new Rect(screenBlack + screen_width * 0.475f - texture_width * scale * 0.5f, screen_height * 0.525f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale);
        //GUI.DrawTexture(renderTextureRect, renderTextureBack);

        scale = 0.73f;
        mainCamera.rect = new Rect((screen_width * 0.47f - texture_width * scale * 0.5f) / screen_width, (screen_height * 0.52f - texture_height * scale * 0.5f) / screen_height, texture_width * scale / screen_width, texture_height * scale / screen_height);

        //旋轉
        scale = 0.71f;
        gSkin.FindStyle("horizontalsliderthumb").overflow = new RectOffset(0, 0, (int)(texture_height * scale * 0.02f), -(int)(texture_height * scale * 0.05f));
        rotateZ = GUI.HorizontalSlider(new Rect(screenBlack + screen_width * 0.46f - texture_width * scale * 0.5f, screen_height * 0.91f - texture_height * scale * 0.5f * 0.1f, texture_width * scale, texture_height * scale * 0.1f), rotateZ, 0.0001f, 2f * Mathf.PI, "horizontalslider", "horizontalsliderthumb");
        rotateY = GUI.VerticalSlider(new Rect(screenBlack + screen_width * 0.79f - texture_width * scale * 0.5f * 0.1f, screen_height * 0.47f - texture_height * scale * 0.5f, texture_width * scale * 0.03f, texture_height * scale), rotateY, Mathf.PI - 0.0001f, 0.0001f, "VerticalSlider", "VerticalSliderthumb");
        mainCamera.transform.position = new Vector3(
           cameraLook.x + cameraDistance * Mathf.Sin(rotateY) * Mathf.Cos(rotateZ),
           cameraLook.y + cameraDistance * Mathf.Cos(rotateY),
           cameraLook.z - cameraDistance * Mathf.Sin(rotateY) * Mathf.Sin(rotateZ));
        mainCamera.transform.LookAt(cameraLook);

        scale = 0.08f;
        GUI.DrawTexture(new Rect(screenBlack + screen_width * 0.71f + texture_width * scale * 0.5f, screen_height * 0.88f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), rotateTexture, ScaleMode.ScaleAndCrop);

        //縮放按鈕
        if (!isHideGUI)
        {
            scale = 0.05f;
            if (GUI.Button(new Rect(screenBlack + screen_width * 0.62f - texture_width * scale * 0.5f, screen_height * 0.8f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), zoomTexture[1], "Zoom"))
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
            if (GUI.Button(new Rect(screenBlack + screen_width * 0.67f - texture_width * scale * 0.9f, screen_height * 0.8f - texture_height * scale * 0.5f, texture_width * scale * 1.8f, texture_height * scale), zoomPercent.ToString() + "%", "ZoomPercent"))
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
            if (GUI.Button(new Rect(screenBlack + screen_width * 0.72f - texture_width * scale * 0.5f, screen_height * 0.8f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), zoomTexture[0], "Zoom"))
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

        scale = 0.22f;
        //if (isShowSetting)
        //{
        gSkin.FindStyle("Settings").fontSize = (int)(texture_height * scale * 0.2f);
        gSkin.FindStyle("Settings").contentOffset = new Vector2(0, (int)(texture_height * scale * 0.01f));
        //設定們
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.9f - texture_width * scale * 0.5f, screen_height * 0.26f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), "另存圖片", "Settings"))
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
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.9f - texture_width * scale * 0.5f, screen_height * 0.44f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), countText, "Settings2"))
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
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.9f - texture_width * scale * 0.5f, screen_height * 0.62f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), lineText, "Settings"))
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
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.9f - texture_width * scale * 0.5f, screen_height * 0.8f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), "回主選單", "Settings"))
        {
            Common.init();
            //isShowSetting = false;
            Application.LoadLevel("MainMenu");
        }
        //} 

        //分層按鈕
        gSkin.FindStyle(allSeparateBtnStyle).fontSize = (int)(texture_height * scale * 0.2f);
        gSkin.FindStyle(allSeparateBtnStyle).contentOffset = new Vector2(0, (int)(texture_height * scale * 0.01f));
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.1f - texture_width * scale * 0.5f, screen_height * 0.35f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), allSeparateBtnString, allSeparateBtnStyle))
        {
            isSeparateBtn_All = true;
            isSeparateBtn = false;
            isMergeBtn = false;

            if (isSeparate_All)
            {
                for (int i = 0; i < totLevel_Y; i++)
                    GameObject.Find(i.ToString()).transform.position = toPos_All[i];

                allSeparateBtnString = "全部分開";
                allSeparateBtnStyle = "SpAll";
            }
            else
            {
                for (int i = 0; i < totLevel_Y; i++)
                    GameObject.Find(i.ToString()).transform.position = fromPos[i];

                allSeparateBtnString = "全部組合";
                allSeparateBtnStyle = "CloseAll";
            }

            isSeparate_All = !isSeparate_All;


            //for (int i = 0; i < levelMax_Y; i++)
            //{
            //    Renderer[] listOfChildren = GameObject.Find(i.ToString()).GetComponentsInChildren<Renderer>();
            //    if (i != currLevelShow)
            //        foreach (Renderer child in listOfChildren)
            //            child.enabled = false;
            //    else
            //        foreach (Renderer child in listOfChildren)
            //            child.enabled = true;
            //}

            //currLevelShow++;
            //if (currLevelShow >= levelMax_Y)
            //    currLevelShow = 0;
        }
        gSkin.FindStyle("SpOne").fontSize = (int)(texture_height * scale * 0.2f);
        gSkin.FindStyle("SpOne").contentOffset = new Vector2(0, (int)(texture_height * scale * 0.01f));
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.1f - texture_width * scale * 0.5f, screen_height * 0.55f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), "單層分開", "SpOne"))
        {
            isSeparateBtn_All = false;
            isMergeBtn = false;

            for (int i = 0; i < totLevel_Y; i++)
            {
                toPos_One[i] = GameObject.Find(i.ToString()).transform.position;
                isMoved[i] = false;
            }

            isSeparateBtn = !isSeparateBtn;

        }
        gSkin.FindStyle("CloseOne").fontSize = (int)(texture_height * scale * 0.2f);
        gSkin.FindStyle("CloseOne").contentOffset = new Vector2(0, (int)(texture_height * scale * 0.01f));
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.1f - texture_width * scale * 0.5f, screen_height * 0.7f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), "單層組合", "CloseOne"))
        {
            isSeparateBtn_All = false;
            isSeparateBtn = false;

            for (int i = 0; i < totLevel_Y; i++)
            {
                toPos_One[i] = GameObject.Find(i.ToString()).transform.position;
                isMoved[i] = false;
            }

            isMergeBtn = !isMergeBtn;

        }

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


}
