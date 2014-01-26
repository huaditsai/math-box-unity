using UnityEngine;
using System.Collections;

public class AddBox : MonoBehaviour
{
    public Camera caamera;
    public GameObject box;

    //public GameObject[] plane;
    public GameObject plane;



    // Use this for initialization
    void Start()
    {
        //showMenu = false;
        for (int x = 0; x < Common.matrix_size; x++)
            for (int y = 0; y < Common.matrix_size; y++)
                Instantiate(plane, new Vector3(x, 0, y), Quaternion.identity);

        caamera.orthographicSize = Common.matrix_size;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = caamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            if (Input.GetMouseButtonDown(0))
            {
                //print("hit!");                    

                if (hit.transform.position.y.Equals(0)) //點選在平面上
                    Instantiate(box, hit.transform.position + Vector3.up * 0.5f, Quaternion.identity);
                //else
                //    Instantiate(box, hit.transform.position + Vector3.up * 1f, Quaternion.identity);
            }

        }

    }

    public Texture menuTexture;
    private float screen_width, screen_height;
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

        GUI.DrawTexture(new Rect(0, 0, screen_width, screen_height), menuTexture, ScaleMode.StretchToFill);

        if (GUI.Button(new Rect(screen_width * 0.88f - texture_width / 2, screen_height * 0.1f - texture_height / 4, texture_width, texture_height / 2), "重新設定"))
        {
            Application.LoadLevel("MatrixMenu2");
        }

        if (GUI.Button(new Rect(screen_width * 0.88f - texture_width / 2, screen_height * 0.3f - texture_height / 4, texture_width, texture_height / 2), "另存圖片"))
        {

        }

        if (GUI.Button(new Rect(screen_width * 0.88f - texture_width / 2, screen_height * 0.5f - texture_height / 4, texture_width, texture_height / 2), "隱藏線條"))
        {

        }

        if (GUI.Button(new Rect(screen_width * 0.88f - texture_width / 2, screen_height * 0.7f - texture_height / 4, texture_width, texture_height / 2), "看數量"))
        {

        }

        if (GUI.Button(new Rect(screen_width * 0.88f - texture_width / 2, screen_height * 0.9f - texture_height / 4, texture_width, texture_height / 2), "回主選單"))
        {
            Application.LoadLevel("MainMenu");
        }



        if (GUI.Button(new Rect(screen_width * 0.1f - texture_width / 6, screen_height * 0.9f - texture_height / 6, texture_width / 3, texture_height / 3), "-"))
        {

        }

        GUI.Label(new Rect(screen_width * 0.19f - texture_width / 6, screen_height * 0.93f - texture_height / 6, texture_width / 3, texture_height / 3), "100 %");

        if (GUI.Button(new Rect(screen_width * 0.25f - texture_width / 6, screen_height * 0.9f - texture_height / 6, texture_width / 3, texture_height / 3), "+"))
        {

        }
        if (GUI.Button(new Rect(screen_width * 0.35f - texture_width / 6, screen_height * 0.9f - texture_height / 6, texture_width / 3, texture_height / 3), "^"))
        {

        }
        if (GUI.Button(new Rect(screen_width * 0.45f - texture_width / 6, screen_height * 0.9f - texture_height / 6, texture_width / 3, texture_height / 3), "v"))
        {

        }
        if (GUI.Button(new Rect(screen_width * 0.55f - texture_width / 6, screen_height * 0.9f - texture_height / 6, texture_width / 3, texture_height / 3), "<"))
        {

        }
        if (GUI.Button(new Rect(screen_width * 0.65f - texture_width / 6, screen_height * 0.9f - texture_height / 6, texture_width / 3, texture_height / 3), ">"))
        {

        }

    }


}
