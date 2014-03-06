using UnityEngine;
using System.Collections;

public class MatrixMenuTwo : MonoBehaviour
{
    public Camera caamera;

    private float screen_width, screen_height;
    public GUISkin gSkin;

    public Texture[] backgroundTexture;
    private int index = 0;

    public Texture titleTexture;

    public Texture[] renderTexture; //要有兩個camera才Build成功
    private bool isBigRenderTexture = false;
    private Rect BigRenderRect;

    public Texture[] planeTexture;
    private int planeTextureIndex = 0;

    private string planeSizeText = Common.matrix_size + " x " + Common.matrix_size + " x " + Common.matrix_size;

    private int level = 1;
    private string levelText = "第 1 層";

    //int x = 0, y = 0, z = 0;
    public GameObject box;
    private GameObject cloneBox;

    private bool isCanGo = false;
    public int clickCount = 0;
    public Texture[] btnGoBackTexture;

    // Use this for initialization
    void Start()
    {
        for (int x = 0; x < Common.matrix_size; x++)
            for (int y = 0; y < Common.matrix_size; y++)
                for (int z = 0; z < Common.matrix_size; z++)
                {
                    if (Common.matrix[x, y, z] == 1)
                    {
                        if (Common.matrix_size % 2 == 0)
                            cloneBox = Instantiate(box, new Vector3(-Common.matrix_size / 2 + 0.5f + x, -Common.matrix_size / 2 + 0.5f + y, -Common.matrix_size / 2 + 0.5f + z), Quaternion.identity) as GameObject;
                        else
                            cloneBox = Instantiate(box, new Vector3(-Common.matrix_size / 2 + x, -Common.matrix_size / 2 + y, -Common.matrix_size / 2 + z), Quaternion.identity) as GameObject;

                        cloneBox.name = string.Format("{0}{1}{2}", x, y, z);
                        isCanGo = true;
                    }
                }

        caamera.orthographicSize = Common.matrix_size;
        transform.LookAt(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Screen.fullScreen = false;
    }

    void FixedUpdate()
    {
        index++;
        if (index > 1)
            index = 0;
    }

    void OnGUI()
    {
        if (gSkin)
            GUI.skin = gSkin;

        screen_width = Screen.width;
        screen_height = Screen.height;

        float texture_width, texture_height;
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

        GUI.DrawTexture(new Rect(0, 0, screen_width, screen_height), backgroundTexture[index], ScaleMode.StretchToFill);

        scale = 0.11f;
        if (GUI.Button(new Rect(screen_width * 0.05f - texture_width * scale * 0.5f, screen_height * 0.93f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), btnGoBackTexture[0], "BtnGoBack"))
        {
            Application.LoadLevel("MatrixMenu");
        }

        if (isCanGo) //確認成形
        {
            if (GUI.Button(new Rect(screen_width * 0.95f - texture_width * scale * 0.5f, screen_height * 0.93f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), btnGoBackTexture[1], "BtnGoBack"))
            {
                Application.LoadLevel("Main");
                Common.lastLevel = "MatrixMenuTwo";
            }

            gSkin.FindStyle("OkGO").fontSize = (int)(texture_height * scale * 0.4f);
            GUI.Label(new Rect(screen_width * 0.82f - texture_width * scale / 0.3f * 0.13f * 0.5f, screen_height * 0.93f - texture_height * scale / 0.3f * 0.13f * 0.5f, texture_width * scale / 0.3f * 0.13f, texture_height * scale / 0.3f * 0.13f), "確認成型", "OkGO");
        }

        //Title
        //gSkin.FindStyle("BoxSize").fontSize = (int)(texture_height * scale * 0.2f);
        //GUI.Label(new Rect(screen_width * 0.15f + texture_width * scale / 2 - texture_width * scale * 0.25f, screen_height * 0.08f - texture_height * scale * 0.25f, texture_width * scale * 0.5f, texture_height * scale * 0.5f), "請點選要出現的方格", "BoxSize");
        scale = 0.65f;
        GUI.DrawTexture(new Rect(screen_width * 0.4f - texture_width * scale * 0.45f, screen_height * 0.07f - texture_height * scale * 0.06f, texture_width * scale * 0.9f, texture_height * scale * 0.12f), titleTexture, ScaleMode.StretchToFill);

        scale = 0.65f;
        //點選要的
        GUI.DrawTexture(new Rect(screen_width * 0.13f - texture_width * scale * 0.095f, screen_height * 0.15f - texture_height * scale * 0.05f, texture_width * scale * 1.28f, texture_height * scale * 1.28f), planeTexture[2], ScaleMode.StretchToFill);
        for (int i = 1; i <= Common.matrix_size; i++)
        {
            for (int j = 1; j <= Common.matrix_size; j++)
            {
                if (Common.matrix[i - 1, level - 1, j - 1] == 1)
                    planeTextureIndex = 1;
                else
                    planeTextureIndex = 0;

                if (GUI.Button(new Rect(screen_width * 0.13f + (texture_width * scale * 1.1f / Common.matrix_size) * (i - 1),
                    screen_height * 0.12f + (texture_height * scale * 1.1f / Common.matrix_size) * (Common.matrix_size - j) //(0,0)要在左下
                    , texture_width * scale * 1.1f / Common.matrix_size, texture_height * scale * 1.1f / Common.matrix_size), planeTexture[planeTextureIndex], "Plane"))
                {
                    if (!isBigRenderTexture)
                    {
                        //print((i -1) + ", " + (j - 1));
                        if (Common.matrix[i - 1, level - 1, j - 1] == 1)
                        {
                            Common.matrix[i - 1, level - 1, j - 1] = 0; //0不放
                            planeTextureIndex = 0; //0白色

                            Destroy(GameObject.Find(string.Format("{0}{1}{2}", i - 1, level - 1, j - 1)));

                            if (clickCount > 0)
                                clickCount--;
                            if (clickCount <= 0)
                                isCanGo = false;
                        }
                        else
                        {
                            Common.matrix[i - 1, level - 1, j - 1] = 1; //1要放
                            planeTextureIndex = 1; //1粉紅色

                            if (Common.matrix_size % 2 == 0) //將(0,0)放在物體的中心
                                cloneBox = Instantiate(box, new Vector3(-Common.matrix_size / 2 + 0.5f + i - 1, -Common.matrix_size / 2 + 0.5f + level - 1, -Common.matrix_size / 2 + 0.5f + j - 1), Quaternion.identity) as GameObject;
                            else
                                cloneBox = Instantiate(box, new Vector3(-Common.matrix_size / 2 + i - 1, -Common.matrix_size / 2 + level - 1, -Common.matrix_size / 2 + j - 1), Quaternion.identity) as GameObject;

                            cloneBox.name = string.Format("{0}{1}{2}", i - 1, level - 1, j - 1); //為了刪除用

                            clickCount++;
                            isCanGo = true;
                        }
                    }
                }
            }
        }

        scale = 0.12f;
        // 層數
        if (level < Common.matrix_size && GUI.Button(new Rect(screen_width * 0.77f - texture_width * scale * 0.5f, screen_height * 0.43f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), "", "Btn_U"))
        {
            if (!isBigRenderTexture)
            {
                level++;
                levelText = "第 " + level + " 層";
            }
        }
        scale = 0.25f;
        gSkin.FindStyle("Level").fontSize = (int)(texture_height * scale * 0.2f);
        GUI.Label(new Rect(screen_width * 0.77f - texture_width * scale * 0.5f, screen_height * 0.5f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), levelText, "Level");
        scale = 0.12f;
        if (level > 1 && GUI.Button(new Rect(screen_width * 0.77f - texture_width * scale * 0.5f, screen_height * 0.57f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), "", "Btn_D"))
        {
            if (!isBigRenderTexture)
            {
                level--;
                levelText = "第 " + level + " 層";
            }
        }

        ////方塊尺寸
        //if (Common.matrix_size > 2 && GUI.Button(new Rect(screen_width * 0.05f + texture_width * scale / 2 - texture_width * scale * 0.125f, screen_height * 0.15f - texture_height * scale * 0.125f, texture_width * scale * 0.25f, texture_height * scale * 0.25f), "", "Btn_L"))
        //{
        //    if (!isBigRenderTexture)
        //    {
        //        //清空方塊
        //        for (int i = 0; i < Common.matrix_size; i++)
        //            for (int j = 0; j < Common.matrix_size; j++)
        //                for (int k = 0; k < Common.matrix_size; k++)
        //                    Destroy(GameObject.Find(string.Format("{0}{1}{2}", i, j, k)));

        //        Common.matrix_size--;
        //        planeSizeText = Common.matrix_size + " x " + Common.matrix_size + " x " + Common.matrix_size;
        //        Common.matrix = new int[Common.matrix_size, Common.matrix_size, Common.matrix_size];

        //        caamera.orthographicSize = Common.matrix_size; //調整鏡頭

        //        level = 1;
        //        levelText = "第 " + level + " 層"; 
        //    }
        //}
        scale = 0.3f;
        gSkin.FindStyle("BoxSize").fontSize = (int)(texture_height * scale * 0.2f);
        GUI.Label(new Rect(screen_width * 0.4f - texture_width * scale * 0.5f, screen_height * 0.875f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), planeSizeText, "BoxSize");
        //if (Common.matrix_size < 3 && GUI.Button(new Rect(screen_width * 0.25f + texture_width * scale / 2 - texture_width * scale * 0.125f, screen_height * 0.15f - texture_height * scale * 0.125f, texture_width * scale * 0.25f, texture_height * scale * 0.25f), "", "Btn_R"))
        //{
        //    if (!isBigRenderTexture)
        //    {
        //        //清空方塊
        //        for (int i = 0; i < Common.matrix_size; i++)
        //            for (int j = 0; j < Common.matrix_size; j++)
        //                for (int k = 0; k < Common.matrix_size; k++)
        //                    Destroy(GameObject.Find(string.Format("{0}{1}{2}", i, j, k)));

        //        Common.matrix_size++;
        planeSizeText = Common.matrix_size + " x " + Common.matrix_size + " x " + Common.matrix_size;
        //        Common.matrix = new int[Common.matrix_size, Common.matrix_size, Common.matrix_size];

        //        caamera.orthographicSize = Common.matrix_size; //調整鏡頭

        //        level = 1;
        //        levelText = "第 " + level + " 層"; 
        //    }
        //}


        if (GUI.Button(new Rect(screen_width * 0.85f - texture_width * scale / 2, screen_height * 0.2f - texture_height * scale / 2, texture_width * scale, texture_height * scale), renderTexture[0], "Render"))
        {
            isBigRenderTexture = true; //放大看
        }

        BigRenderRect = new Rect(screen_width * 0.75f - texture_width * scale * 0.9f, screen_height * 0.2f - texture_height * scale * 0.5f, texture_width * scale * 2f, texture_height * scale * 2f);
        if (isBigRenderTexture) //放大看
        {
            GUI.DrawTexture(new Rect(BigRenderRect), renderTexture[1]);
            GUI.DrawTexture(new Rect(BigRenderRect), renderTexture[0]);

            if (!BigRenderRect.Contains(new Vector3(Input.mousePosition.x, screen_height - Input.mousePosition.y, Input.mousePosition.z)))
                isBigRenderTexture = false;
        }

    }

}
