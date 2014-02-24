using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour
{
    private float screen_width, screen_height;

    public Texture[] splashBackgroundTexture;
    public Texture[] btnGoBackTexture;
    private int index = 0;
    public GUISkin gSkin;

    //float time = 1f;

    // Use this for initialization
    void Start()
    {
        Screen.SetResolution(800, 500, false);
    }

    // Update is called once per frame
    void Update()
    {
        //time -= Time.fixedDeltaTime;
        if (Input.anyKeyDown)
            Application.LoadLevel("MainMenu");        
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

        GUI.DrawTexture(new Rect(0, 0, screen_width, screen_height), splashBackgroundTexture[index], ScaleMode.StretchToFill);

        //if (GUI.Button(new Rect(screen_width * 0.95f - texture_width / 0.3f * 0.13f * 0.5f, screen_height * 0.93f - texture_height / 0.3f * 0.13f * 0.5f, texture_width / 0.3f * 0.13f, texture_height / 0.3f * 0.13f), btnGoBackTexture[0], "BtnGoBack"))
        //{
        //    Application.LoadLevel("MainMenu");
        //}

        
    }
}
