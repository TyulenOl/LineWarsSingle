using System;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public static class CustomMath
{
    public static bool PointInCircle(Vector2 checkedPoint, Vector2 centerOfCircle, float radius)
    {
        return Mathf.Pow(checkedPoint.x - centerOfCircle.x, 2) 
               + Mathf.Pow(checkedPoint.y - centerOfCircle.y, 2) <= Mathf.Pow(radius, 2);
    }
    
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

    /// <summary>
    /// Пример (4,4) (2,1) -> (8,4)
    /// (4,4) (1,2) -> (4,8)
    /// (4,4) (12,24) -> (4,8)
    /// </summary>
    public static Vector2 TransferProportions(Vector2 vector2, Vector2 proportions)
    {
        if (proportions.x <= 0 || proportions.y <= 0)
            throw new ArgumentException();
        
        var proportionValue = proportions.x / proportions.y;
        if (proportionValue >= 1)
            return new Vector2(proportionValue * vector2.x, vector2.y);
        else
            return new Vector2(vector2.x, vector2.y / proportionValue);
    }
    
    public static Vector2Int TransferVector2ByMaxSide(Vector2Int vector2, int maxSide)
    {
        if (vector2.x <= 0 || vector2.y <= 0 || maxSide <= 0)
            throw new ArgumentException();
        
        if (vector2.x > vector2.y)
        {
            return new Vector2Int(maxSide, Convert.ToInt32(Convert.ToSingle(vector2.y * maxSide) / vector2.x));
        }
        else
        {
            return new Vector2Int(Convert.ToInt32(Convert.ToSingle(vector2.x * maxSide) / vector2.y), maxSide);
        }
    }

    public static int GreatestCommonDivisor(int m, int n)
    {
        if (m <= 0 || n <= 0)
            throw new ArgumentException();
        
        while (m != n)
        {
            if (m > n)
            {
                m = m - n;
            }
            else
            {
                n = n - m;
            }
        }

        return n;
    }
}