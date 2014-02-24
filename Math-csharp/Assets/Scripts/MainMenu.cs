using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    private float screen_width, screen_height;

    public GUISkin gSkin;
    private string[] style = new string[] { "BtnMatrix", "BtnMatrix", "BtnMatrix", "BtnMatrixNO", "BtnMatrixNO", "BtnMatrixNO", "BtnMatrixNO", "BtnMatrixNO", "BtnMatrixNO", "BtnMatrixNO", "BtnMatrixNO" };

    public Texture backgroundTexture;
    public Texture TitleTexture;

    public Texture[] BookSampleTexture;
    //private int pageNum = 0;
    //private int totalPage = 1;

    private int examplePre = 0;
    private int exampleCur = 0;
    private int exampleNxt = 0;

    public Texture[] btnTexture;
    public Texture[] pagerTexture;
    Rect pager_first;

    // Use this for initialization
    void Start()
    {
        Common.init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Screen.fullScreen = false;
    }

    void OnGUI()
    {
        if (gSkin)
            GUI.skin = gSkin;

        screen_width = Screen.width;
        screen_height = Screen.height;

        GUI.DrawTexture(new Rect(0, 0, screen_width, screen_height), backgroundTexture, ScaleMode.StretchToFill);

        float texture_width;
        float texture_height;
        float scale = 0.5f;

        if (screen_width < screen_height)
        {
            texture_width = screen_width * scale;
            texture_height = screen_width * scale;
        }
        else
        {
            texture_width = screen_height * scale;
            texture_height = screen_height * scale;
        }

        examplePre = exampleCur - 1;
        if (examplePre < 0)
            examplePre = BookSampleTexture.Length - 1;

        exampleNxt = exampleCur + 1;
        if (exampleNxt > BookSampleTexture.Length - 1)
            exampleNxt = 0;

        if (GUI.Button(new Rect(-texture_width / 2, screen_height * 0.5f - texture_height / 2, texture_width, texture_height), BookSampleTexture[examplePre], style[examplePre]))
        {
            if (examplePre == 0 || examplePre == 1 || examplePre == 2)
                Examples(examplePre);
        }

        if (GUI.Button(new Rect(screen_width * 0.5f - texture_width / 2, screen_height * 0.5f - texture_height / 2, texture_width, texture_height), BookSampleTexture[exampleCur], style[exampleCur]))
        {
            if (exampleCur == 0 || exampleCur == 1 || exampleCur == 2)
                Examples(exampleCur);
        }

        if (GUI.Button(new Rect(screen_width - texture_width / 2, screen_height * 0.5f - texture_height / 2, texture_width, texture_height), BookSampleTexture[exampleNxt], style[exampleNxt]))
        {
            if (exampleNxt == 0 || exampleNxt == 1 || exampleNxt == 2)
                Examples(exampleNxt);
        }

        //變化組合
        gSkin.FindStyle("BtnChange").fontSize = (int)(texture_height * scale * 0.27f);
        if (GUI.Button(new Rect(screen_width * 0.5f - texture_width / 2, screen_height * 0.9f - texture_height * 0.125f, texture_width, texture_height * 0.25f), "", "BtnChange"))
        {
            Application.LoadLevel("MatrixMenu");
        }

        //換頁
        if (GUI.Button(new Rect(screen_width * 0.05f - texture_width / 0.5f * 0.13f * 0.5f, screen_height * 0.9f - texture_height / 0.5f * 0.13f * 0.5f, texture_width / 0.5f * 0.13f, texture_height / 0.5f * 0.13f), btnTexture[0], "BtnPage"))
        {
            exampleCur--;
            if (exampleCur < 0)
                exampleCur = BookSampleTexture.Length - 1;
        }
        if (GUI.Button(new Rect(screen_width * 0.95f - texture_width / 0.5f * 0.13f * 0.5f, screen_height * 0.9f - texture_height / 0.5f * 0.13f * 0.5f, texture_width / 0.5f * 0.13f, texture_height / 0.5f * 0.13f), btnTexture[1], "BtnPage"))
        {
            exampleCur++;
            if (exampleCur > BookSampleTexture.Length - 1)
                exampleCur = 0;
        }

        ////Title
        ////gSkin.FindStyle("Title").fontSize = (int)(texture_height * 0.3f);
        ////GUI.Label(new Rect(screen_width * 0.5f - texture_width / 2, screen_height * 0.15f - texture_height / 2, texture_width, texture_height), "選擇課本範例", "Title");
        //GUI.DrawTexture(new Rect(screen_width * 0.5f - texture_width * 1.25f, screen_height * 0.15f - texture_height * 0.25f, texture_width * 2.5f, texture_height * 0.5f), TitleTexture, ScaleMode.StretchToFill);

        ////變化組合
        //gSkin.FindStyle("BtnMatrix").fontSize = (int)(texture_height * 0.3f);
        //if (pageNum == 0 && GUI.Button(new Rect(screen_width * 0.3f - texture_width / 2, screen_height * 0.4f - texture_height / 2, texture_width, texture_height), "變化\n組合", "BtnMatrix"))
        //{
        //    Application.LoadLevel("MatrixMenu");
        //}

        //if (pageNum != 0 && pageNum * 6 - 1 < BookSampleTexture.Length)
        //    if (GUI.Button(new Rect(screen_width * 0.3f - texture_width / 2, screen_height * 0.4f - texture_height / 2, texture_width, texture_height), BookSampleTexture[pageNum * 6 - 1], "BtnMatrix"))
        //    {
        //        Examples(pageNum * 6 - 1);
        //        Application.LoadLevel("Main");
        //        Common.lastLevel = "MainMenu";
        //    }

        //if (pageNum * 6 + 0 < BookSampleTexture.Length)
        //    if (GUI.Button(new Rect(screen_width * 0.5f - texture_width / 2, screen_height * 0.4f - texture_height / 2, texture_width, texture_height), BookSampleTexture[pageNum * 6 + 0], "BtnMatrix"))
        //    {
        //        Examples(pageNum * 6 + 0);
        //        Application.LoadLevel("Main");
        //        Common.lastLevel = "MainMenu";
        //    }
        //if (pageNum * 6 + 1 < BookSampleTexture.Length)
        //    if (GUI.Button(new Rect(screen_width * 0.7f - texture_width / 2, screen_height * 0.4f - texture_height / 2, texture_width, texture_height), BookSampleTexture[pageNum * 6 + 1], "BtnMatrix"))
        //    {
        //        Examples(pageNum * 6 + 1);
        //        Application.LoadLevel("Main");
        //        Common.lastLevel = "MainMenu";
        //    }
        //if (pageNum * 6 + 2 < BookSampleTexture.Length)
        //    if (GUI.Button(new Rect(screen_width * 0.3f - texture_width / 2, screen_height * 0.7f - texture_height / 2, texture_width, texture_height), BookSampleTexture[pageNum * 6 + 2], "BtnMatrix"))
        //    {
        //        Examples(pageNum * 6 + 2);
        //        Application.LoadLevel("Main");
        //        Common.lastLevel = "MainMenu";
        //    }
        //if (pageNum * 6 + 3 < BookSampleTexture.Length)
        //    if (GUI.Button(new Rect(screen_width * 0.5f - texture_width / 2, screen_height * 0.7f - texture_height / 2, texture_width, texture_height), BookSampleTexture[pageNum * 6 + 3], "BtnMatrix"))
        //    {
        //        Examples(pageNum * 6 + 3);
        //        Application.LoadLevel("Main");
        //        Common.lastLevel = "MainMenu";
        //    }
        //if (pageNum * 6 + 4 < BookSampleTexture.Length)
        //    if (GUI.Button(new Rect(screen_width * 0.7f - texture_width / 2, screen_height * 0.7f - texture_height / 2, texture_width, texture_height), BookSampleTexture[pageNum * 6 + 4], "BtnMatrix"))
        //    {
        //        Examples(pageNum * 6 + 4);
        //        Application.LoadLevel("Main");
        //        Common.lastLevel = "MainMenu";
        //    }        

        ////換頁
        //if (pageNum > 0)
        //    if (GUI.Button(new Rect(screen_width * 0.1f - texture_width * 0.375f, screen_height * 0.55f - texture_height * 0.375f, texture_width * 0.75f, texture_height * 0.75f), btnTexture[0], "BtnPage"))
        //    {
        //        pageNum--;
        //    }

        //if (pageNum + 1 < totalPage)
        //    if (GUI.Button(new Rect(screen_width * 0.9f - texture_width * 0.375f, screen_height * 0.55f - texture_height * 0.375f, texture_width * 0.75f, texture_height * 0.75f), btnTexture[1], "BtnPage"))
        //    {
        //        pageNum++;
        //    }

        ////頁數
        //totalPage = (BookSampleTexture.Length + 1) / 6;
        //if ((BookSampleTexture.Length + 1) % 6 != 0)
        //    totalPage++;

        //if (totalPage % 2 != 0)
        //{
        //    for (int i = 1; i <= totalPage / 2; i++)
        //    {
        //        pager_first = new Rect(screen_width * 0.5f - texture_width * 0.05f - texture_width * 0.05f * 2 * i, screen_height * 0.9f - texture_height * 0.05f, texture_width * 0.1f, texture_height * 0.1f);
        //        GUI.DrawTexture(pager_first, pagerTexture[0], ScaleMode.StretchToFill);
        //        GUI.DrawTexture(new Rect(screen_width * 0.5f - texture_width * 0.05f + texture_width * 0.05f * 2 * i, screen_height * 0.9f - texture_height * 0.05f, texture_width * 0.1f, texture_height * 0.1f), pagerTexture[0], ScaleMode.StretchToFill);
        //    }
        //    GUI.DrawTexture(new Rect(screen_width * 0.5f - texture_width * 0.05f, screen_height * 0.9f - texture_height * 0.05f, texture_width * 0.1f, texture_height * 0.1f), pagerTexture[0], ScaleMode.StretchToFill);
        //}
        //else
        //{
        //    for (int i = 0; i < totalPage / 2; i++)
        //    {
        //        pager_first = new Rect(screen_width * 0.5f - texture_width * 0.05f - texture_width * 0.05f - texture_width * 0.05f * 2 * i, screen_height * 0.9f - texture_height * 0.05f, texture_width * 0.1f, texture_height * 0.1f);
        //        GUI.DrawTexture(pager_first, pagerTexture[0], ScaleMode.StretchToFill);
        //        GUI.DrawTexture(new Rect(screen_width * 0.5f - texture_width * 0.05f + texture_width * 0.05f + texture_width * 0.05f * 2 * i, screen_height * 0.9f - texture_height * 0.05f, texture_width * 0.1f, texture_height * 0.1f), pagerTexture[0], ScaleMode.StretchToFill);
        //    }
        //}
        ////目前頁數
        //GUI.DrawTexture(new Rect(pager_first.xMin + texture_width * 0.05f * 2 * pageNum, pager_first.yMin, pager_first.width, pager_first.height), pagerTexture[1], ScaleMode.StretchToFill);

    }

    void Examples(int index)
    {
        switch (index)
        {
            case 0:
                Common.SetMatrix(2);                                    // y   z
                for (int i = 0; i < Common.matrix_size; i++)            // |  /
                    for (int j = 0; j < Common.matrix_size; j++)        // | /
                        for (int k = 0; k < Common.matrix_size; k++)    // |/_____ x
                        {
                            Common.matrix[i, j, k] = 1;
                        }
                Common.matrix[0, 1, 0] = 0;
                break;
            case 1:
                Common.SetMatrix(3);
                for (int i = 0; i < Common.matrix_size; i++)
                    for (int j = 0; j < Common.matrix_size - 1; j++)
                        for (int k = 0; k < Common.matrix_size; k++)
                            Common.matrix[i, j, k] = 1;
                Common.matrix[1, 1, 0] = 0;
                Common.matrix[2, 1, 0] = 0;
                Common.matrix[1, 1, 1] = 0;
                Common.matrix[2, 1, 1] = 0;
                break;
            case 2:
                Common.SetMatrix(4);
                for (int i = 0; i < Common.matrix_size; i++)
                    for (int j = 0; j < Common.matrix_size - 1; j++)
                        Common.matrix[i, j, 0] = 1;
                Common.matrix[2, 2, 0] = 0;
                Common.matrix[3, 2, 0] = 0;
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            //case 11:                
            //    break;
            //case 12:
            //    break;
            default:
                break;
        }

        Common.lastLevel = "MainMenu";
        Application.LoadLevel("Main");
    }
}
