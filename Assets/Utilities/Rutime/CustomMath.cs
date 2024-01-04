using System;
using UnityEngine;

public static class CustomMath
{
    public static bool SegmentsIsIntersects(Vector2 v11, Vector2 v12, Vector2 v21, Vector2 v22)
    {
        var cut1 = v12 - v11;
        var cut2 = v22 - v21;

        var prod1 = Vector3.Cross(cut1, v21 - v11);
        var prod2 = Vector3.Cross(cut1, v22 - v11);

        if (Math.Sign(prod1.z) == Math.Sign(prod2.z))
            return false;
        
        prod1 = Vector3.Cross(cut2, v11 - v21);
        prod2 = Vector3.Cross(cut2, v12 - v21);
        
        if (Math.Sign(prod1.z) == Math.Sign(prod2.z))
            return false;

        return true;
    }
}