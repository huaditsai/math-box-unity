using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour
{
    private float screen_width, screen_height;

    public Texture splashBackgroundTexture;

    //float time = 1f;

    // Use this for initialization
    void Start()
    {
        Screen.SetResolution(1280, 720, false);
    }

    // Update is called once per frame
    void Update()
    {
        //time -= Time.fixedDeltaTime;
        if (Input.anyKeyDown)
            Application.LoadLevel("MainMenu");
    }

    void OnGUI()
    {
        screen_width = Screen.width;
        screen_height = Screen.height;

        GUI.DrawTexture(new Rect(0, 0, screen_width, screen_height), splashBackgroundTexture, ScaleMode.ScaleToFit);
    }
}
