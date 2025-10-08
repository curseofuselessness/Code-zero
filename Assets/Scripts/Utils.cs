using UnityEngine;

public class Utils
{
    public static float NormalizeAngle(float angle)
    {
        angle = angle % 360f;
        if (angle > 180f)
            angle -= 360f;
        return angle;
    }
}
