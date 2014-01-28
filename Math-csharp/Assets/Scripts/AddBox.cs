using UnityEngine;
using System.Collections;

public class AddBox : MonoBehaviour
{
    public Camera caamera;
    public GameObject box;

    public GameObject plane;


    // Use this for initialization
    void Start()
    {
        //Common.matrix_size = 10;
        for (int x = 0; x < Common.matrix_size; x++)
            for (int y = 0; y < Common.matrix_size; y++)
                for (int z = 0; z < Common.matrix_size; z++)
                {
                    if (Common.matrix_size % 2 == 0)
                        Instantiate(plane, new Vector3(-Common.matrix_size / 2 + 0.5f + x, -Common.matrix_size / 2 + 0.5f + y, -Common.matrix_size / 2 + 0.5f + z), Quaternion.identity);
                    else
                        Instantiate(plane, new Vector3(-Common.matrix_size / 2 + x, -Common.matrix_size / 2 + y, -Common.matrix_size / 2 + z), Quaternion.identity);
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

    public float rotateY = 0.0F;
    private float rotateZ = 0.0F;

    public Texture backgroundTexture;

    public GUISkin gSkin;
    public Texture[] btnGoBackTexture;
    public Texture[] zoomTexture;
    public Texture[] btnSettingTexture;
    public Texture[] renderTexture; //要有兩個camera才Build成功

    private bool isShowSetting = false;
    private bool isShowCount = false;

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
        rotateY = GUI.HorizontalSlider(new Rect(screen_width * 0.35f - texture_width * scale * 0.5f, screen_height * 0.9f - texture_height * scale * 0.5f * 0.1f, texture_width * scale, texture_height * scale * 0.03f), rotateY, -180.0f, 180.0f, "horizontalslider", "horizontalsliderthumb");
        rotateZ = GUI.VerticalSlider(new Rect(screen_width * 0.6f - texture_width * scale * 0.5f * 0.1f, screen_height * 0.45f - texture_height * scale * 0.5f, texture_width * scale * 0.03f, texture_height * scale), rotateZ, -180.0f, 180.0f);

        //回上頁
        scale = 0.15f;
        if (GUI.Button(new Rect(screen_width * 0.05f - texture_width * scale * 0.5f, screen_height * 0.9f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), btnGoBackTexture[0], "BtnGoBack"))
        {
            Application.LoadLevel("MatrixMenuTwo");
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
                isShowSetting = false;
            }
            if (GUI.Button(new Rect(screen_width * 0.95f - texture_width * scale * 0.5f, screen_height * 0.6f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), "觀看數量", "Settings"))
            {
                isShowSetting = false;
                isShowCount = !isShowCount;
            }            
            if (GUI.Button(new Rect(screen_width * 0.95f - texture_width * scale * 0.5f, screen_height * 0.7f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), "隱藏線條", "Settings"))
            {
                isShowSetting = false;
            }
            if (GUI.Button(new Rect(screen_width * 0.95f - texture_width * scale * 0.5f, screen_height * 0.8f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale * 0.5f), "回主選單", "Settings"))
            {
                isShowSetting = false;
                Application.LoadLevel("Splash");
            }
        }

        if (isShowCount)
        {
            scale = 0.5f;
            gSkin.FindStyle("Count").fontSize = (int)(texture_height * scale * 0.27f);
            GUI.Label(new Rect(screen_width * 0.8f - texture_width * scale * 0.5f, screen_height * 0.7f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), "16 個", "Count");
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
