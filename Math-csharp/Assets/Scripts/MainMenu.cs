using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    private float screen_width, screen_height;
    private float screenBlack = 0;

    public GUISkin gSkin;
    private enum styleNames : int
    {
        Title, BtnMatrix, BtnPage, BtnChange, BtnMatrixNO,
        ExitWindow, BtnGoExit, ExitOk, ExitCancle
    }

    public Texture[] backgroundTexture;
    private int index = 0;

    private Texture2D blackTexture;

    public Texture TitleTexture;

    public Texture[] BookSampleTexture;

    private int examplePre = 0;
    private int exampleCur = 0;
    private int exampleNxt = 0;

    private string exampleCurString = "0";
    private string exampleNxtString = "0";

    public Texture[] btnTexture;

    public Texture saveWindowBackTexture;
    private bool isExitDialog = false;

    // Use this for initialization
    void Start()
    {
        blackTexture = new Texture2D(1, 1);
        blackTexture.SetPixel(0, 0, Color.black);
        blackTexture.wrapMode = TextureWrapMode.Repeat;
        blackTexture.Apply();

        Common.init();

        examplePre = exampleCur - 1;
        if (examplePre < 0)
            examplePre = BookSampleTexture.Length - 1;

        exampleNxt = exampleCur + 1;
        if (exampleNxt > BookSampleTexture.Length - 1)
            exampleNxt = 0;

        exampleCurString = (exampleCur + 1).ToString();
        exampleNxtString = (exampleNxt + 1).ToString();
    }

    void FixedUpdate()
    {
        index++;
        if (index > 1)
            index = 0;
    }

    float texture_width;
    float texture_height;
    float scale = 0.5f;

    void OnGUI()
    {
        if (gSkin)
            GUI.skin = gSkin;

        screen_width = screen_height * (4f / 3f);
        screen_height = Screen.height;

        screenBlack = (Screen.width - screen_height * (4f / 3f)) / 2f;

        GUI.DrawTexture(new Rect(screenBlack + 0, 0, screen_width, screen_height), backgroundTexture[index], ScaleMode.ScaleToFit);


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

        scale = 0.1f;
        GUI.DrawTexture(new Rect(screenBlack + screen_width * 0.5f - texture_width * scale * 4f * 0.5f, screen_height * 0.1f - texture_height * scale * 0.5f, texture_width * scale * 4f, texture_height * scale), TitleTexture, ScaleMode.ScaleToFit);

        scale = 0.4f;
        gSkin.customStyles[(int)styleNames.Title].fontSize = (int)(texture_height * scale * 0.1f);
        gSkin.customStyles[(int)styleNames.BtnMatrix].contentOffset = new Vector2(0, -texture_height * scale * 0.1f);

        if (GUI.Button(new Rect(screenBlack - texture_width * scale * 0.7f, screen_height * 0.5f - texture_height * scale * 0.7f, texture_width * scale * 1.4f, texture_height * scale * 1.4f), BookSampleTexture[examplePre], "BtnMatrix"))
        {
            Examples(examplePre);
        }
        
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.5f - texture_width * scale * 0.7f, screen_height * 0.5f - texture_height * scale * 0.7f, texture_width * scale * 1.4f, texture_height * scale * 1.4f), BookSampleTexture[exampleCur], "BtnMatrix"))
        {
            Examples(exampleCur);
        }
        GUI.Label(new Rect(screenBlack + screen_width * 0.308f - texture_width * scale * 0.6f, screen_height * 0.29f - texture_height * scale * 0.7f, texture_width * scale * 1.2f, texture_height * scale * 1.2f), exampleCurString, "Title");

        if (GUI.Button(new Rect(screenBlack + screen_width - texture_width * scale * 0.7f, screen_height * 0.5f - texture_height * scale * 0.7f, texture_width * scale * 1.4f, texture_height * scale * 1.4f), BookSampleTexture[exampleNxt], "BtnMatrix"))
        {
            Examples(exampleNxt);
        }
        GUI.Label(new Rect(screenBlack + screen_width * 0.808f - texture_width * scale * 0.6f, screen_height * 0.29f - texture_height * scale * 0.7f, texture_width * scale * 1.2f, texture_height * scale * 1.2f), exampleNxtString, "Title");

        //變化組合
        scale = 0.5f;
        gSkin.customStyles[(int)styleNames.BtnChange].fontSize = (int)(texture_height * scale * scale * 0.27f);
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.5f - texture_width * scale / 2, screen_height * 0.93f - texture_height * scale * 0.125f, texture_width * scale, texture_height * scale * 0.25f), "", "BtnChange"))
        {
            Application.LoadLevel("MatrixMenu");
        }

        scale = 0.11f;
        //換頁
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.05f - texture_width * scale * 0.5f, screen_height * 0.93f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), btnTexture[0], "BtnPage"))
        {
            exampleCur--;
            if (exampleCur < 0)
                exampleCur = BookSampleTexture.Length - 1;

            examplePre = exampleCur - 1;
            if (examplePre < 0)
                examplePre = BookSampleTexture.Length - 1;

            exampleNxt = exampleCur + 1;
            if (exampleNxt > BookSampleTexture.Length - 1)
                exampleNxt = 0;

            exampleCurString = (exampleCur + 1).ToString();
            exampleNxtString = (exampleNxt + 1).ToString();
        }
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.95f - texture_width * scale * 0.5f, screen_height * 0.93f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), btnTexture[1], "BtnPage"))
        {
            exampleCur++;
            if (exampleCur > BookSampleTexture.Length - 1)
                exampleCur = 0;

            examplePre = exampleCur - 1;
            if (examplePre < 0)
                examplePre = BookSampleTexture.Length - 1;

            exampleNxt = exampleCur + 1;
            if (exampleNxt > BookSampleTexture.Length - 1)
                exampleNxt = 0;

            exampleCurString = (exampleCur + 1).ToString();
            exampleNxtString = (exampleNxt + 1).ToString();
        }

        scale = 0.15f;
        if (GUI.Button(new Rect(screenBlack + screen_width * 0.05f - texture_width * scale * 0.5f, screen_height * 0.07f - texture_height * scale * 0.5f, texture_width * scale, texture_height * scale), "", "BtnGoExit"))
        {
            isExitDialog = true;
            //Application.Quit(); //離開
        }

        if (isExitDialog)
        {
            GUI.DrawTexture(new Rect(screenBlack + 0, 0, screen_width, screen_height), saveWindowBackTexture, ScaleMode.StretchToFill);
            scale = 0.7f;
            GUI.ModalWindow(0, new Rect(screenBlack + screen_width * 0.5f - texture_width * scale * 1.2f * 0.5f, screen_height * 0.5f - texture_height * scale * 0.5f, texture_width * scale * 1.2f, texture_height * scale), ExitWindow, "", "ExitWindow");
        }


        GUI.DrawTexture(new Rect(0, 0, screenBlack, Screen.height), blackTexture);
        GUI.DrawTexture(new Rect(Screen.width - screenBlack, 0, screenBlack, Screen.height), blackTexture);
    }

    private void ExitWindow(int id) //離開畫面
    {
        scale = 0.25f;

        if (GUI.Button(new Rect(screen_width * 0.22f - texture_width * scale * 0.5f, screen_height * 0.49f - texture_height * scale * 0.3f * 0.5f, texture_width * scale, texture_height * scale * 0.3f), "", "ExitOk"))
        {
            Application.Quit();
        }

        if (GUI.Button(new Rect(screen_width * 0.42f - texture_width * scale * 0.5f, screen_height * 0.49f - texture_height * scale * 0.3f * 0.5f, texture_width * scale, texture_height * scale * 0.3f), "", "ExitCancle"))
        {
            isExitDialog = false;
        }
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
                Common.SetMatrix(4);
                for (int i = 0; i < Common.matrix_size; i++)
                    for (int j = 0; j < Common.matrix_size - 2; j++)
                        for (int k = 0; k < Common.matrix_size - 2; k++)
                            Common.matrix[i, j, k] = 1;
                for (int i = 0; i < Common.matrix_size; i++)
                    Common.matrix[i, 1, 0] = 0;
                break;
            case 4:
                Common.SetMatrix(4);
                for (int i = 0; i < Common.matrix_size; i++)
                    for (int j = 0; j < Common.matrix_size - 1; j++)
                        for (int k = 0; k < Common.matrix_size - 2; k++)
                            Common.matrix[i, j, k] = 1;
                for (int i = 0; i < Common.matrix_size; i++)
                    for (int j = 1; j < Common.matrix_size - 1; j++)
                        Common.matrix[i, j, 0] = 0;
                break;
            case 5:
                Common.SetMatrix(5);
                for (int i = 0; i < Common.matrix_size; i++)
                    for (int j = 0; j < Common.matrix_size - 3; j++)
                        for (int k = 0; k < Common.matrix_size - 3; k++)
                            Common.matrix[i, j, k] = 1;
                for (int i = Common.matrix_size - 2; i < Common.matrix_size; i++)
                    for (int j = 0; j < Common.matrix_size - 3; j++)
                        Common.matrix[i, 1, j] = 0;
                break;
            case 6:
                Common.SetMatrix(3);
                Common.matrix[0, 0, 0] = 1;
                Common.matrix[0, 0, 1] = 1;
                Common.matrix[0, 0, 2] = 1;
                Common.matrix[1, 0, 1] = 1;
                Common.matrix[1, 0, 2] = 1;
                Common.matrix[2, 0, 2] = 1;
                Common.matrix[0, 1, 1] = 1;
                Common.matrix[0, 1, 2] = 1;
                Common.matrix[1, 1, 2] = 1;
                Common.matrix[0, 2, 2] = 1;
                break;
            case 7:
                Common.SetMatrix(3);
                for (int j = 0; j < Common.matrix_size; j++)
                    for (int i = 0; i < Common.matrix_size - j; i++)
                        for (int k = j; k < Common.matrix_size; k++)
                            Common.matrix[i, j, k] = 1;
                break;
            case 8:
                Common.SetMatrix(3);
                for (int j = 0; j < Common.matrix_size; j++)
                    for (int i = 0; i < Common.matrix_size - j; i++)
                        for (int k = 0; k < Common.matrix_size - 1; k++)
                            Common.matrix[i, j, k] = 1;
                break;
            case 9:
                Common.SetMatrix(4);
                for (int j = 0; j < Common.matrix_size; j++)
                    for (int i = 0; i < Common.matrix_size - j; i++)
                    {
                        Common.matrix[i, j, 3] = 1;
                        for (int k = j; k <= Common.matrix_size - j; k++)
                        {
                            if (k < Common.matrix_size)
                                Common.matrix[0, j, k] = 1;
                        }
                    }
                break;
            case 10:
                Common.SetMatrix(7);
                for (int j = 0; j < 3; j++)
                    for (int i = 0; i < Common.matrix_size; i++)
                        Common.matrix[i, 0, j] = 1;
                Common.matrix[1, 1, 2] = 1;
                Common.matrix[2, 1, 2] = 1;
                Common.matrix[3, 1, 2] = 1;

                for (int i = 4; i < Common.matrix_size; i++)
                    Common.matrix[i, 0, 0] = 0;
                Common.matrix[Common.matrix_size - 1, 0, 1] = 0;

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
