using System;
using GXPEngine;

public struct Vec2
{
    public float x;
    public float y;

    public Vec2(float pX = 0, float pY = 0)
    {
        x = pX;
        y = pY;
    }

    public float Length()
    {
        return Mathf.Sqrt(x * x + y * y);
    }

    public void Normalize()
    {
        float length = Length();
        if (length != 0)
        {
            x = x / length;
            y = y / length;
        }
    }

    public Vec2 Normalized()
    {
        float length = Length();
        if (length != 0) { return new Vec2(x / length, y / length); }
        else { return new Vec2(x, y); }
    }

    public void SetXY(float vectorX, float vectorY)
    {
        x = vectorX;
        y = vectorY;
    }

    public static Vec2 operator +(Vec2 left, Vec2 right)
    {
        return new Vec2(left.x + right.x, left.y + right.y);
    }

    public static Vec2 operator -(Vec2 left, Vec2 right)
    {
        return new Vec2(left.x - right.x, left.y - right.y);
    }
    public static Vec2 operator *(Vec2 vector, float scaleAmount)
    {
        return new Vec2(vector.x * scaleAmount, vector.y * scaleAmount);
    }
    public static Vec2 operator *(float scaleAmount, Vec2 vector)
    {
        return new Vec2(scaleAmount * vector.x, scaleAmount * vector.y);
    }

    public static float Deg2Rad(float degree)
    {
        return degree * (Mathf.PI / 180.0f);
    }

    public static float Rad2Deg(float radians)
    {
        return radians * (180.0f / Mathf.PI);
    }
    public static Vec2 GetUnitVectorRad(float radian)
    {
        Vec2 vector = new Vec2();
        vector.SetXY(Mathf.Cos(radian), Mathf.Sin(radian));
        return vector;
    }
    public static Vec2 GetUnitVectorDeg(float degree)
    {
        float radian = Deg2Rad(degree);
        return GetUnitVectorRad(radian);
    }

    public static Vec2 RandomUnitVector()
    {
        float randomAngle = Utils.Random(0, 360);
        Console.WriteLine(randomAngle);
        return GetUnitVectorDeg(randomAngle);
    }

    /// <summary>
    /// Sets the angle of the vector to the given angle in degrees without modifying the length of the vector
    /// </summary>
    public void SetAngleDegrees(float angleDeg)
    {
        Vec2 vector = new Vec2();
        vector = GetUnitVectorDeg(angleDeg);
        float length = Length();
        vector *= length;
        x = vector.x;
        y = vector.y;
    }
    public void SetAngleRadians(float angleRad)
    {
        //Vec2 vector = new Vec2();
        Vec2 vector = GetUnitVectorRad(angleRad);
        float length = Length();
        vector *= length;
        x = vector.x;
        y = vector.y;
    }

    public float GetAngleDegrees()
    {
        return Rad2Deg(Mathf.Atan2(y, x));
    }
    public float GetAngleRadians()
    {
        return Mathf.Atan2(y, x);
    }

    public void RotateDegrees(float angleDeg)
    {
        Vec2 v = new Vec2();
        v = Vec2.GetUnitVectorDeg(angleDeg); // ok I get it - it's just weird
        v.SetXY(x * v.x - y * v.y, x * v.y + y * v.x);
        x = v.x;
        y = v.y;
    }
    public void RotateRadians(float angleRad)
    {
        Vec2 v = new Vec2();
        v = Vec2.GetUnitVectorRad(angleRad);
        v.SetXY(x * v.x - y * v.y, x * v.y + y * v.x);
        x = v.x;
        y = v.y;
    }

    public void RotateAroundDegrees(Vec2 rotationPoint, float angleDeg)
    {
        x -= rotationPoint.x;
        y -= rotationPoint.y;
        RotateDegrees(angleDeg);
        x += rotationPoint.x;
        y += rotationPoint.y;
    }

    public void RotateAroundRadians(Vec2 rotationPoint, float angleRad)
    {
        x -= rotationPoint.x;
        y -= rotationPoint.y;
        RotateRadians(angleRad);
        x += rotationPoint.x;
        y += rotationPoint.y;
    }

    public float Dot(Vec2 vector)
    {
        return (x * vector.x + y * vector.y);
    }

    public Vec2 Normal()
    {
        Vec2 v = new Vec2(-y, x);
        v.Normalize();
        return v;
    }

    public void Reflect(Vec2 normal, float bounciness)
    {
        Vec2 velocityOut = new Vec2((1 + bounciness) * Dot(normal) * normal.x, (1 + bounciness) * Dot(normal) * normal.y);
        x -= velocityOut.x;
        y -= velocityOut.y;
    }

    public override string ToString()
    {
        return String.Format("({0},{1})", x, y);
    }
}