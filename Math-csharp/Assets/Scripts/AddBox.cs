using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

public class AddBox : MonoBehaviour
{
    public Camera caamera;
    public GameObject box;

    //public GameObject plane;

    // Use this for initialization
    void Start()
    {
        //Common.matrix_size = 10;
        for (int x = 0; x < Common.matrix_size; x++)
            for (int y = 0; y < Common.matrix_size; y++)
                for (int z = 0; z < Common.matrix_size; z++)
                {
                    if (Common.matrix[x, y, z] == 1)
                    {
                        if (Common.matrix_size % 2 == 0)
                            Instantiate(box, new Vector3(-Common.matrix_size / 2 + 0.5f + x, -Common.matrix_size / 2 + 0.5f + y, -Common.matrix_size / 2 + 0.5f + z), Quaternion.identity);
                        else
                            Instantiate(box, new Vector3(-Common.matrix_size / 2 + x, -Common.matrix_size / 2 + y, -Common.matrix_size / 2 + z), Quaternion.identity);
                    }
                }

        caamera.orthographicSize = Common.matrix_size;
        transform.LookAt(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
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

    public GUISkin gSkin;
    public Texture[] btnGoBackTexture;
    public Texture[] zoomTexture;
    public Texture[] btnSettingTexture;
    public Texture[] renderTexture; //要有兩個camera才Build成功

    private bool isShowSetting = false;
    private string countText = "觀看數量";
    private bool isShowCount = false;
    private int count = 0;

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
        GUI.DrawTexture(new Rect(screen_width * 0.35f - texture_width * scale * 0.5f, screen_height * 0.45f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), renderTexture[1]);
        scale = 0.7f;
        GUI.DrawTexture(new Rect(screen_width * 0.35f - texture_width * scale * 0.5f, screen_height * 0.45f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), renderTexture[0]);

        //旋轉
        rotateZ = GUI.HorizontalSlider(new Rect(screen_width * 0.35f - texture_width * scale * 0.5f, screen_height * 0.9f - texture_height * scale * 0.5f * 0.1f, texture_width * scale, texture_height * scale * 0.03f), rotateZ, 0.0001f, 2f * Mathf.PI, "horizontalslider", "horizontalsliderthumb");
        rotateY = GUI.VerticalSlider(new Rect(screen_width * 0.6f - texture_width * scale * 0.5f * 0.1f, screen_height * 0.45f - texture_height * scale * 0.5f, texture_width * scale * 0.03f, texture_height * scale), rotateY, Mathf.PI - 0.0001f, 0.0001f, "VerticalSlider", "VerticalSliderthumb");
        caamera.transform.position = new Vector3(
            7 * Mathf.Sin(rotateY) * Mathf.Cos(rotateZ),
            7 * Mathf.Cos(rotateY),
            -7 * Mathf.Sin(rotateY) * Mathf.Sin(rotateZ));
        caamera.transform.LookAt(Vector3.zero);

        //回上頁
        scale = 0.15f;
        if (GUI.Button(new Rect(screen_width * 0.05f - texture_width * scale * 0.5f, screen_height * 0.9f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), btnGoBackTexture[0], "BtnGoBack"))
        {
            Application.LoadLevel(Common.lastLevel); //變化組合回到編輯, 範例回到範例選擇
        }

        //設定
        if (GUI.Button(new Rect(screen_width * 0.95f - texture_width * scale * 0.5f, screen_height * 0.9f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), btnSettingTexture[0], "BtnGoBack"))
        {
            isShowSetting = !isShowSetting;
        }
        if (isShowSetting)
        {
            gSkin.FindStyle("Settings").fontSize = (int)(texture_height * scale * 0.2f);

            if (GUI.Button(new Rect(screen_width * 0.95f - texture_width * scale * 0.5f, screen_height * 0.5f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), "另存圖片", "Settings"))
            {
                var path = EditorUtility.SaveFilePanel("Save", "", "", "jpg");

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

                    isShowSetting = false;
                }
            }
            if (GUI.Button(new Rect(screen_width * 0.95f - texture_width * scale * 0.5f, screen_height * 0.6f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), countText, "Settings"))
            {
                count = 0;
                if (!isShowCount)
                {
                    for (int x = 0; x < Common.matrix_size; x++)
                        for (int y = 0; y < Common.matrix_size; y++)
                            for (int z = 0; z < Common.matrix_size; z++)
                                if (Common.matrix[x, y, z] == 1)
                                    count++;
                    countText = "隱藏數量";
                }
                else
                    countText = "觀看數量";

                isShowSetting = false;
                isShowCount = !isShowCount;
            }
            if (GUI.Button(new Rect(screen_width * 0.95f - texture_width * scale * 0.5f, screen_height * 0.7f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), lineText, "Settings"))
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

                isShowSetting = false;
                isHideLine = !isHideLine;
            }
            if (GUI.Button(new Rect(screen_width * 0.95f - texture_width * scale * 0.5f, screen_height * 0.8f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), "回主選單", "Settings"))
            {
                Common.init();
                isShowSetting = false;
                Application.LoadLevel("Splash");
            }
        }

        //顯示數量
        if (isShowCount)
        {
            scale = 0.5f;
            gSkin.FindStyle("Count").fontSize = (int)(texture_height * scale * 0.27f);
            GUI.Label(new Rect(screen_width * 0.8f - texture_width * scale * 0.5f, screen_height * 0.7f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), count + " 個", "Count");
        }


        //縮放按鈕
        scale = 0.06f;
        if (GUI.Button(new Rect(screen_width * 0.48f - texture_width * scale * 0.5f, screen_height * 0.75f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), zoomTexture[1], "Zoom"))
        {
            if (caamera.orthographicSize < 10f)
                caamera.orthographicSize += 0.1f;
        }
        if (GUI.Button(new Rect(screen_width * 0.52f - texture_width * scale * 0.5f, screen_height * 0.75f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), zoomTexture[0], "Zoom"))
        {
            if (caamera.orthographicSize > 0.5f)
                caamera.orthographicSize -= 0.1f;
        }

        //標題        
        scale = 0.5f;
        gSkin.FindStyle("Title").fontSize = (int)(texture_height * scale * 0.27f);
        GUI.Label(new Rect(screen_width * 0.8f - texture_width * scale * 0.5f, screen_height * 0.35f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), "數一數", "Title");
        gSkin.FindStyle("SubTitle").fontSize = (int)(texture_height * scale * 0.18f);
        GUI.Label(new Rect(screen_width * 0.8f - texture_width * scale * 0.5f, screen_height * 0.37f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), "有幾個？", "SubTitle");

    }


}
