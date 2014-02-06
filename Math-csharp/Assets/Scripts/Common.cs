using UnityEngine;
using System.Collections;

public class Common : MonoBehaviour
{
    public static int matrix_size = 2;
    public static int[, ,] matrix = new int[2, 2, 2];
    public static string lastLevel = "MatrixMenuTwo";

    public static void init()
    {
        matrix_size = 2;
        matrix = new int[2, 2, 2];
        lastLevel = "MatrixMenuTwo";
    }
}
