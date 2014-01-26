using UnityEngine;
using System.Collections;

public class MatrixMenu : MonoBehaviour
{
    float screen_width, screen_height;
    public Texture matrixMenuBackgroundTexture;
    public Texture[] matrixTexture;

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
                    screen_height * ((screen_height - texture_height * 3f) / 4f / screen_height) + screen_height * ((screen_height - texture_height * 3f) / 8f / screen_height) * j + texture_height * (j - 1)
                    , texture_width, texture_height), matrixTexture[matrix_size - 2]))
                {
                    //print(matrix_index);
                    //GameObject planeClone = Instantiate(plane[matrix_index], Vector3.zero, Quaternion.identity) as GameObject;

                    Common.matrix_size = matrix_size;
                    Application.LoadLevel("MatrixMenu2");

                }

                matrix_size++;
            }
    }
}
