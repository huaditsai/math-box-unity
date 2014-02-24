using UnityEngine;
using System.Collections;

public class MatrixMenu : MonoBehaviour
{
    float screen_width, screen_height;
    public GUISkin gSkin;

    public Texture matrixMenuBackgroundTexture;
    public Texture[] matrixTexture;
    public Texture[] btnGoBackTexture;

    // Use this for initialization
    void Start()
    {

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

        float texture_width;
        float texture_height;
        float scale = 0.3f;

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

        GUI.DrawTexture(new Rect(0, 0, screen_width, screen_height), matrixMenuBackgroundTexture, ScaleMode.StretchToFill);

        int matrix_size = 2;
        for (int j = 1; j <= 3; j++)
            for (int i = 1; i <= 3; i++)
            {
                if (GUI.Button(new Rect(screen_width * ((screen_width - texture_width * 3f) / 4f / screen_width) + screen_width * ((screen_width - texture_width * 3f) / 8f / screen_width) * i + texture_width * (i - 1),
                    screen_height * ((screen_height - texture_height * 3f) / 3f / screen_height) + screen_height * ((screen_height - texture_height * 3f) / 8f / screen_height) * j + texture_height * (j - 1)
                    , texture_width, texture_height), matrixTexture[matrix_size - 2], "BtnMatrix"))
                {
                    //print(matrix_size);
                    //GameObject planeClone = Instantiate(plane[matrix_index], Vector3.zero, Quaternion.identity) as GameObject;

                    Common.SetMatrix(matrix_size);
                    Application.LoadLevel("MatrixMenuTwo");

                }

                matrix_size++;
            }

        //if (GUI.Button(new Rect(screen_width * 0.05f - texture_width / 0.3f * 0.13f * 0.5f, screen_height * 0.93f - texture_height / 0.3f * 0.13f * 0.5f, texture_width / 0.3f * 0.13f, texture_height / 0.3f * 0.13f), btnGoBackTexture[0], "BtnPage"))
        //{
        //    Application.LoadLevel("MainMenu");
        //}
    }
}
