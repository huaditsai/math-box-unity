using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

public class AddBox : MonoBehaviour
{
    public Camera caamera;
    public GameObject box;

    //public GameObject plane;

    public Vector3 cameraLook;

    // Use this for initialization
    void Start()
    {
        float posX, posY, posZ;
        float minX = 500, minY = 500, minZ = 500;
        float maxX = 500, maxY = 500, maxZ = 500;

        //Common.matrix_size = 10;
        for (int x = 0; x < Common.matrix_size; x++)
            for (int y = 0; y < Common.matrix_size; y++)
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

                        Instantiate(box, new Vector3(posX, posY, posZ), Quaternion.identity);
                        if (minX == 500)
                        {
                            minX = posX;
                            maxX = posX;
                        }

                        if (posX < minX)
                            minX = posX;
                        if (posX > minX)
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

        caamera.orthographicSize = Common.matrix_size;
        cameraLook = new Vector3((maxX + minX) / 2, (maxY + minY) / 2, (maxZ + minZ) / 2);
        transform.LookAt(cameraLook);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Screen.fullScreen = false;

        if (isZoom)
        {
            caamera.orthographicSize = Mathf.Lerp(caamera.orthographicSize, zoomTo, Time.deltaTime * 5f);
        }
        //Ray ray = caamera.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit = new RaycastHit();
        //if (Physics.Raycast(ray, out hit, 1000f))
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        //print("hit!");                    

        //        if (hit.transform.position.y.Equals(0)) //點選在平面上
        //            Instantiate(box, hit.transform.position + Vector3.up * 0.5f, Quaternion.identity);
        //        //else
        //        //    Instantiate(box, hit.transform.position + Vector3.up * 1f, Quaternion.identity);
        //    }

        //}

    }

    private float rotateY = Mathf.PI / 3;
    private float rotateZ = 2 * Mathf.PI / 3;

    public Texture backgroundTexture;
    public Texture titleTexture;

    public GUISkin gSkin;
    public Texture[] btnGoBackTexture;
    public Texture[] zoomTexture;
    public Texture[] btnSettingTexture;
    public Texture[] renderTexture; //要有兩個camera才Build成功

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

    void OnGUI()
    {
        if (gSkin)
            GUI.skin = gSkin;

        screen_width = Screen.width;
        screen_height = Screen.height;

        float texture_width;
        float texture_height;
        float scale = 0.3f;

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

        GUI.DrawTexture(new Rect(0, 0, screen_width, screen_height), backgroundTexture, ScaleMode.StretchToFill);

        //主要
        scale = 0.71f;
        GUI.DrawTexture(new Rect(screen_width * 0.4f - texture_width * scale * 0.5f, screen_height * 0.5f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), renderTexture[1]);
        scale = 0.68f;
        GUI.DrawTexture(new Rect(screen_width * 0.4f - texture_width * scale * 0.5f, screen_height * 0.5f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), renderTexture[0]);

        //旋轉
        gSkin.FindStyle("horizontalsliderthumb").overflow = new RectOffset(0, 0, (int)(texture_height * scale * 0.02f), -(int)(texture_height * scale * 0.05f));
        rotateZ = GUI.HorizontalSlider(new Rect(screen_width * 0.4f - texture_width * scale * 0.5f, screen_height * 0.91f - texture_height * scale * 0.5f * 0.1f, texture_width * scale, texture_height * scale * 0.1f), rotateZ, 0.0001f, 2f * Mathf.PI, "horizontalslider", "horizontalsliderthumb");
        rotateY = GUI.VerticalSlider(new Rect(screen_width * 0.66f - texture_width * scale * 0.5f * 0.1f, screen_height * 0.5f - texture_height * scale * 0.5f, texture_width * scale * 0.03f, texture_height * scale), rotateY, Mathf.PI - 0.0001f, 0.0001f, "VerticalSlider", "VerticalSliderthumb");
        caamera.transform.position = new Vector3(
           cameraLook.x + 7 * Mathf.Sin(rotateY) * Mathf.Cos(rotateZ),
           cameraLook.y + 7 * Mathf.Cos(rotateY),
           cameraLook.z - 7 * Mathf.Sin(rotateY) * Mathf.Sin(rotateZ));
        caamera.transform.LookAt(cameraLook);

        //縮放按鈕
        scale = 0.06f;
        if (GUI.Button(new Rect(screen_width * 0.49f - texture_width * scale * 0.5f, screen_height * 0.8f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), zoomTexture[1], "Zoom"))
        {
            zoomTo = caamera.orthographicSize + 0.2f;
            if (zoomTo < 10f)
                isZoom = true;
            else
                zoomTo = 10f;

            if (zoomTo > Common.matrix_size)
                zoomPercent = 100 - (int)((zoomTo - Common.matrix_size) / (10f - Common.matrix_size) * 100f);
            else if (zoomTo < Common.matrix_size)
                zoomPercent = 100 + (int)((Common.matrix_size - zoomTo) / (Common.matrix_size - 0.5f) * 100f);
            else
                zoomPercent = 100;
        }
        if (GUI.Button(new Rect(screen_width * 0.54f - texture_width * scale * 0.5f, screen_height * 0.8f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), zoomPercent.ToString() + "%", "ZoomPercent"))
        {
            //caamera.orthographicSize = Common.matrix_size;
            zoomTo = Common.matrix_size;
            isZoom = true;
            zoomPercent = 100;
        }
        if (GUI.Button(new Rect(screen_width * 0.59f - texture_width * scale * 0.5f, screen_height * 0.8f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), zoomTexture[0], "Zoom"))
        {
            zoomTo = caamera.orthographicSize - 0.2f;
            if (zoomTo > 0.5f)           
                isZoom = true;            
            else
                zoomTo = 0.5f;

            if (zoomTo > Common.matrix_size)
                zoomPercent = 100 - (int)((zoomTo - Common.matrix_size) / (10f - Common.matrix_size) * 100f);
            else if (zoomTo < Common.matrix_size)
                zoomPercent = 100 + (int)((Common.matrix_size - zoomTo) / (Common.matrix_size - 0.5f) * 100f);
            else
                zoomPercent = 100;
        }

        //回上頁
        scale = 0.13f;
        if (Common.lastLevel == "MatrixMenuTwo")
            if (GUI.Button(new Rect(screen_width * 0.05f - texture_width * scale * 0.5f, screen_height * 0.93f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), btnGoBackTexture[0], "BtnGoBack"))
            {
                Application.LoadLevel("MatrixMenuTwo"); //變化組合回到編輯, 範例回到範例選擇
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

        scale = 0.25f;
        //if (isShowSetting)
        //{
        gSkin.FindStyle("Settings").fontSize = (int)(texture_height * scale * 0.2f);
        gSkin.FindStyle("Settings").contentOffset = new Vector2(0, (int)(texture_height * scale * 0.1f));

        if (GUI.Button(new Rect(screen_width * 0.85f - texture_width * scale * 0.5f, screen_height * 0.51f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), "另存圖片", "Settings"))
        {
            string path = "";
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                //1. Copy ".\Program Files (x86)\Unity\Editor\Data\Mono\lib\mono\2.0\System.Windows.Forms.dll"
                //To Assets\Plugins 2. Change player setting ".NET 2.0 Subset" To ".NET 2.0"
                System.Windows.Forms.SaveFileDialog saveLog = new System.Windows.Forms.SaveFileDialog();
                saveLog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
                saveLog.Filter = "Image Files(*.JPG)|*.jpg;*|All files (*.*)|*.*";
                System.Windows.Forms.DialogResult result = saveLog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                    path = saveLog.FileName;
            }
            else
                path = Application.persistentDataPath + "/SavedScreen.jpg";

            if (path.Length != 0)
            {
                RenderTexture mainRender = renderTexture[0] as RenderTexture;
                Texture2D myTexture2D = new Texture2D(mainRender.width, mainRender.height);
                RenderTexture.active = mainRender;
                myTexture2D.ReadPixels(new Rect(0, 0, mainRender.width, mainRender.height), 0, 0);
                myTexture2D.Apply();

                //var bytes = myTexture2D.EncodeToPNG();
                //var file = File.Open(Application.persistentDataPath + "/SavedScreen.jpg", FileMode.Create);
                //var binary = new BinaryWriter(file);
                //binary.Write(bytes);
                //file.Close();

                File.WriteAllBytes(path, myTexture2D.EncodeToPNG());

                //isShowSetting = false;
            }

        }
        if (GUI.Button(new Rect(screen_width * 0.85f - texture_width * scale * 0.5f, screen_height * 0.64f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), countText, "Settings"))
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
        if (GUI.Button(new Rect(screen_width * 0.85f - texture_width * scale * 0.5f, screen_height * 0.77f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), lineText, "Settings"))
        {
            if (!isHideLine) //隱藏
            {
                foreach (GameObject item in GameObject.FindGameObjectsWithTag("Box"))
                    item.renderer.material = materials[1];
                lineText = "顯示線條";
            }
            else //顯示線條
            {
                foreach (GameObject item in GameObject.FindGameObjectsWithTag("Box"))
                    item.renderer.material = materials[0];
                lineText = "隱藏線條";
            }

            //isShowSetting = false;
            isHideLine = !isHideLine;
        }
        if (GUI.Button(new Rect(screen_width * 0.85f - texture_width * scale * 0.5f, screen_height * 0.9f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), "回主選單", "Settings"))
        {
            Common.init();
            //isShowSetting = false;
            Application.LoadLevel("MainMenu");
        }
        //}        

        //標題        
        scale = 0.4f;
        //gSkin.FindStyle("Title").fontSize = (int)(texture_height * scale * 0.2f);
        //GUI.Label(new Rect(screen_width * 0.85f - texture_width * scale * 0.5f, screen_height * 0.25f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), "數一數，\n有幾個？", "Title");
        GUI.DrawTexture(new Rect(screen_width * 0.85f - texture_width * scale * 0.5f, screen_height * 0.23f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), titleTexture, ScaleMode.StretchToFill);
        //gSkin.FindStyle("SubTitle").fontSize = (int)(texture_height * scale * 0.18f);
        //GUI.Label(new Rect(screen_width * 0.83f - texture_width * scale * 0.5f, screen_height * 0.27f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), "有幾個？", "SubTitle");

    }


}
