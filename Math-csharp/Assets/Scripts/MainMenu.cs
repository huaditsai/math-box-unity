using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    private float screen_width, screen_height;

    public GUISkin gSkin;

    public Texture backgroundTexture;

    public Texture[] BookSampleTexture;
    public int pageNum = 0;
    public int totalPage = 1;

    public Texture[] btnTexture;
    public Texture[] pagerTexture;
    Rect pager_first;

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

        GUI.DrawTexture(new Rect(0, 0, screen_width, screen_height), backgroundTexture, ScaleMode.StretchToFill);

        float texture_width;
        float texture_height;
        float scale = 0.25f;

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

        GUI.Label(new Rect(screen_width * 0.5f - texture_width / 2, screen_height * 0.15f - texture_height / 2, texture_width, texture_height), "選擇課本範例", "Title");

        if (pageNum == 0 && GUI.Button(new Rect(screen_width * 0.3f - texture_width / 2, screen_height * 0.4f - texture_height / 2, texture_width, texture_height), "變化\n組合", "BtnMatrix"))
        {
            Application.LoadLevel("MatrixMenu");
        }

        if (pageNum != 0 && pageNum * 6 - 1 < BookSampleTexture.Length)
            if (GUI.Button(new Rect(screen_width * 0.3f - texture_width / 2, screen_height * 0.4f - texture_height / 2, texture_width, texture_height), BookSampleTexture[pageNum * 6 - 1], "BtnMatrix"))
            {
                Application.LoadLevel("Main");
            }

        if (pageNum * 6 + 0 < BookSampleTexture.Length)
            if (GUI.Button(new Rect(screen_width * 0.5f - texture_width / 2, screen_height * 0.4f - texture_height / 2, texture_width, texture_height), BookSampleTexture[pageNum * 6 + 0], "BtnMatrix"))
            {
                Application.LoadLevel("Main");
            }
        if (pageNum * 6 + 1 < BookSampleTexture.Length)
            if (GUI.Button(new Rect(screen_width * 0.7f - texture_width / 2, screen_height * 0.4f - texture_height / 2, texture_width, texture_height), BookSampleTexture[pageNum * 6 + 1], "BtnMatrix"))
            {
                Application.LoadLevel("Main");
            }
        if (pageNum * 6 + 2 < BookSampleTexture.Length)
            if (GUI.Button(new Rect(screen_width * 0.3f - texture_width / 2, screen_height * 0.7f - texture_height / 2, texture_width, texture_height), BookSampleTexture[pageNum * 6 + 2], "BtnMatrix"))
            {
                Application.LoadLevel("Main");
            }
        if (pageNum * 6 + 3 < BookSampleTexture.Length)
            if (GUI.Button(new Rect(screen_width * 0.5f - texture_width / 2, screen_height * 0.7f - texture_height / 2, texture_width, texture_height), BookSampleTexture[pageNum * 6 + 3], "BtnMatrix"))
            {
                Application.LoadLevel("Main");
            }
        if (pageNum * 6 + 4 < BookSampleTexture.Length)
            if (GUI.Button(new Rect(screen_width * 0.7f - texture_width / 2, screen_height * 0.7f - texture_height / 2, texture_width, texture_height), BookSampleTexture[pageNum * 6 + 4], "BtnMatrix"))
            {
                Application.LoadLevel("Main");
            }


        totalPage = (BookSampleTexture.Length + 1) / 6;
        if ((BookSampleTexture.Length + 1) % 6 != 0)
            totalPage++;

        if (pageNum > 0)
            if (GUI.Button(new Rect(screen_width * 0.1f - texture_width * 0.375f, screen_height * 0.55f - texture_height * 0.375f, texture_width * 0.75f, texture_height * 0.75f), btnTexture[0], "BtnPage"))
            {
                pageNum--;
            }

        if (pageNum + 1 < totalPage)
            if (GUI.Button(new Rect(screen_width * 0.9f - texture_width * 0.375f, screen_height * 0.55f - texture_height * 0.375f, texture_width * 0.75f, texture_height * 0.75f), btnTexture[1], "BtnPage"))
            {
                pageNum++;
            }

        if (totalPage % 2 != 0)
        {
            for (int i = 1; i <= totalPage / 2; i++)
            {
                pager_first = new Rect(screen_width * 0.5f - texture_width * 0.05f - texture_width * 0.05f * 2 * i, screen_height * 0.9f - texture_height * 0.05f, texture_width * 0.1f, texture_height * 0.1f);
                GUI.DrawTexture(pager_first, pagerTexture[0], ScaleMode.StretchToFill);
                GUI.DrawTexture(new Rect(screen_width * 0.5f - texture_width * 0.05f + texture_width * 0.05f * 2 * i, screen_height * 0.9f - texture_height * 0.05f, texture_width * 0.1f, texture_height * 0.1f), pagerTexture[0], ScaleMode.StretchToFill);
            }
            GUI.DrawTexture(new Rect(screen_width * 0.5f - texture_width * 0.05f, screen_height * 0.9f - texture_height * 0.05f, texture_width * 0.1f, texture_height * 0.1f), pagerTexture[0], ScaleMode.StretchToFill);
        }
        else
        {
            for (int i = 0; i < totalPage / 2; i++)
            {
                pager_first = new Rect(screen_width * 0.5f - texture_width * 0.05f - texture_width * 0.05f - texture_width * 0.05f * 2 * i, screen_height * 0.9f - texture_height * 0.05f, texture_width * 0.1f, texture_height * 0.1f);
                GUI.DrawTexture(pager_first, pagerTexture[0], ScaleMode.StretchToFill);
                GUI.DrawTexture(new Rect(screen_width * 0.5f - texture_width * 0.05f + texture_width * 0.05f + texture_width * 0.05f * 2 * i, screen_height * 0.9f - texture_height * 0.05f, texture_width * 0.1f, texture_height * 0.1f), pagerTexture[0], ScaleMode.StretchToFill);
            }
        }

        GUI.DrawTexture(new Rect(pager_first.xMin + texture_width * 0.05f * 2 * pageNum, pager_first.yMin, pager_first.width, pager_first.height), pagerTexture[1], ScaleMode.StretchToFill);

    }
}
