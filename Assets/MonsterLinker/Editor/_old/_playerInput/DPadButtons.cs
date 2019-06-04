using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPadButtons : MonoBehaviour
{
    public static bool Up;
    public static bool Down;
    public static bool Right;
    public static bool Left;

    private float lastX;
    private float lastY;

    void Start()
    {
        Up = Down = Left = Right = false;
        lastX = lastY = 0;
    }

    void Update()
    {
        float lastDpadX = lastX;
        float lastDpadY = lastY;

        if (Input.GetAxis("PADH") != 0)
        {
            float DPadX = Input.GetAxis("PADH");

            Right = DPadX == 1 && lastDpadX != 1;
            Left = DPadX == -1 && lastDpadX != -1;
            lastX = DPadX;
        }
        else
        {
            lastX = 0;
        }

        if (Input.GetAxis("PADV") != 0)
        {
            float DPadY = Input.GetAxis("PADV");

            Up = DPadY == 1 && lastDpadY != 1;
            Down = DPadY == -1 && lastDpadY != -1;

            lastY = DPadY;
        }
        else
        {
            lastY = 0;
        }
    }
}
