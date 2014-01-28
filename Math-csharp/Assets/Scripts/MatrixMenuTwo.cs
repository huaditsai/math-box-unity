using UnityEngine;
using System.Collections;

public class MatrixMenuTwo : MonoBehaviour
{
    private float screen_width, screen_height;
    public GUISkin gSkin;

    public Texture backgroundTexture;
    public Texture[] renderTexture; //要有兩個camera才Build成功
    private bool isBigRenderTexture = false;

    public Texture[] planeTexture;
    private int planeTextureIndex = 0;

    private int planeSize = 2;
    private string planeSizeText = "2 x 2 x 2";

    private int level = 1;
    private string levelText = "第 1 層";

    private int[, ,] matrix = new int[2, 2, 2];


    public Texture[] btnGoBackTexture;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (gSkin)
            GUI.skin = gSkin;

        screen_width = Screen.width;
        screen_height = Screen.height;

        float texture_width, texture_height;
        float texture_scale = 0.3f;

        float plane_width, plane_height;
        float plane_scale = 0.7f;

        if (screen_width < screen_height)
        {
            texture_width = screen_width * texture_scale;
            texture_height = screen_width * texture_scale;

            plane_width = screen_width * plane_scale;
            plane_height = screen_width * plane_scale;
        }
        else
        {
            texture_width = screen_height * texture_scale;
            texture_height = screen_height * texture_scale;

            plane_width = screen_height * plane_scale;
            plane_height = screen_height * plane_scale;
        }

        GUI.DrawTexture(new Rect(0, 0, screen_width, screen_height), backgroundTexture, ScaleMode.StretchToFill);

        if (GUI.Button(new Rect(screen_width * 0.05f - texture_width / 4, screen_height * 0.9f - texture_height / 4, texture_width / 2, texture_height / 2), btnGoBackTexture[0], "BtnGoBack"))
        {
            Application.LoadLevel("MainMenu");
        }

        if (GUI.Button(new Rect(screen_width * 0.95f - texture_width / 4, screen_height * 0.9f - texture_height / 4, texture_width / 2, texture_height / 2), btnGoBackTexture[1], "BtnGoBack"))
        {
            Application.LoadLevel("Main");
            Common.lastLevel = "MatrixMenuTwo";
        }

        for (int i = 1; i <= planeSize; i++)
        {
            for (int j = 1; j <= planeSize; j++)
            {
                if (matrix[i - 1, j - 1, level - 1] == 1)
                    planeTextureIndex = 1;
                else
                    planeTextureIndex = 0;

                if (GUI.Button(new Rect(screen_width * 0.2f + (plane_width / planeSize) * (i - 1),
                    screen_height * 0.2f + (plane_height / planeSize) * (j - 1)
                    , plane_width / planeSize, plane_height / planeSize), planeTexture[planeTextureIndex], "Plane"))
                {
                    if (matrix[i - 1, j - 1, level - 1] == 1)
                    {
                        matrix[i - 1, j - 1, level - 1] = 0;
                        planeTextureIndex = 0;
                    }
                    else
                    {
                        matrix[i - 1, j - 1, level - 1] = 1;
                        planeTextureIndex = 1;
                    }
                }
            }

        }

        // 層數
        if (level < planeSize && GUI.Button(new Rect(screen_width * 0.7f - texture_width * 0.25f, screen_height * 0.1f + plane_height / 2f - texture_height * 0.125f, texture_width * 0.5f, texture_height * 0.25f), "", "Btn_U"))
        {
            level++;
            levelText = "第 " + level + " 層";
        }
        GUI.Label(new Rect(screen_width * 0.7f - texture_width * 0.5f, screen_height * 0.2f + plane_height / 2f - texture_height / 2, texture_width, texture_height), levelText, "Level");
        if (level > 1 && GUI.Button(new Rect(screen_width * 0.7f - texture_width * 0.25f, screen_height * 0.3f + plane_height / 2f - texture_height * 0.125f, texture_width * 0.5f, texture_height * 0.25f), "", "Btn_D"))
        {
            level--;
            levelText = "第 " + level + " 層";
        }

        //方塊尺寸
        if (planeSize > 2 && GUI.Button(new Rect(screen_width * 0.1f + plane_width / 2 - texture_width * 0.125f, screen_height * 0.15f - texture_height * 0.125f, texture_width * 0.25f, texture_height * 0.25f), "", "Btn_L"))
        {
            planeSize--;
            planeSizeText = planeSize + " x " + planeSize + " x " + planeSize;
            matrix = new int[planeSize, planeSize, planeSize];

            level = 1;
            levelText = "第 " + level + " 層";
        }
        GUI.Label(new Rect(screen_width * 0.2f + plane_width / 2 - texture_width * 0.25f, screen_height * 0.15f - texture_height * 0.25f, texture_width * 0.5f, texture_height * 0.5f), planeSizeText, "BoxSize");
        if (planeSize < 10 && GUI.Button(new Rect(screen_width * 0.3f + plane_width / 2 - texture_width * 0.125f, screen_height * 0.15f - texture_height * 0.125f, texture_width * 0.25f, texture_height * 0.25f), "", "Btn_R"))
        {
            planeSize++;
            planeSizeText = planeSize + " x " + planeSize + " x " + planeSize;
            matrix = new int[planeSize, planeSize, planeSize];

            level = 1;
            levelText = "第 " + level + " 層";
        }


        if (GUI.Button(new Rect(screen_width * 0.9f - texture_width / 2, screen_height * 0.2f - texture_height / 2, texture_width, texture_height), renderTexture[0], "Render"))
        {
            isBigRenderTexture = true;
        }

        if (isBigRenderTexture)
        {
            GUI.DrawTexture(new Rect(screen_width * 0.8f - texture_width * 0.9f, screen_height * 0.2f - texture_height / 2, texture_width * 2f, texture_height * 2f), renderTexture[1]);
            GUI.DrawTexture(new Rect(screen_width * 0.8f - texture_width * 0.9f, screen_height * 0.2f - texture_height / 2, texture_width * 2f, texture_height * 2f), renderTexture[0]);
        }

        if (Input.anyKeyDown)
            isBigRenderTexture = false;

    }

 
}
