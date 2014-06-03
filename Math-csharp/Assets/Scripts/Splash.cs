using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour
{
    private float screen_width, screen_height;
    private float screenBlack = 0;

    public Texture[] splashBackgroundTexture;
    public Texture[] btnGoBackTexture;

    public MovieTexture splashMovie;

    //private int index = 0;
    public GUISkin gSkin;

    float time = 4f;

    // Use this for initialization
    void Start()
    {
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        //Screen.SetResolution(800, 600, true);
        //Screen.fullScreen = true;

        splashMovie.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //
        //if (Input.anyKeyDown)
        //    Application.LoadLevel("MainMenu");
    }
    void FixedUpdate()
    {
        time -= Time.fixedDeltaTime;
        if (time <= 0)
        //if (!splashMovie.isPlaying && int.Parse(System.DateTime.Now.ToString("yyyyMMdd")) <= 20140430)
            Application.LoadLevel("MainMenu");

        //index++;
        //if (index > 1)
        //    index = 0;
    }

    Vector3 scale;
    void OnGUI()
    {
        if (gSkin)
            GUI.skin = gSkin;


        screen_width = screen_height * (4f / 3f);
        screen_height = Screen.height;

        screenBlack = (Screen.width - screen_height * (4f / 3f)) / 2f;


        //float texture_width;
        //float texture_height;
        //float scale = 0.3f;

        //if (screen_width < screen_height)
        //{
        //    texture_width = screen_width * scale;
        //    texture_height = screen_width * scale;
        //}
        //else
        //{
        //    texture_width = screen_height * scale;
        //    texture_height = screen_height * scale;
        //}

        GUI.DrawTexture(new Rect(screenBlack + 0, 0, screen_width, screen_height), splashMovie, ScaleMode.ScaleToFit);

        //if (GUI.Button(new Rect(screen_width * 0.95f - texture_width / 0.3f * 0.13f * 0.5f, screen_height * 0.93f - texture_height / 0.3f * 0.13f * 0.5f, texture_width / 0.3f * 0.13f, texture_height / 0.3f * 0.13f), btnGoBackTexture[0], "BtnGoBack"))
        //{
        //    Application.LoadLevel("MainMenu");
        //}
    }
}
