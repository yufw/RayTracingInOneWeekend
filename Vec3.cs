using System;
public class Vec3
{
    private float[] e = new float[3];
    public Vec3() { }
    public Vec3(float e0, float e1, float e2) { e[0] = e0; e[1] = e1; e[2] = e2; }
    public float x() { return e[0]; }
    public float y() { return e[1]; }
    public float z() { return e[2]; }
    public float r() { return e[0]; }
    public float g() { return e[1]; }
    public float b() { return e[2]; }
    public float Length() { return (float)Math.Sqrt(e[0] * e[0] + e[1] * e[1] + e[2] * e[2]); }
    public float SquaredLength() { return e[0] * e[0] + e[1] * e[1] + e[2] * e[2]; }
    public void MakeUnitVector()
    {
        float k = 1.0F / (float)Math.Sqrt(e[0] * e[0] + e[1] * e[1] + e[2] * e[2]);
        e[0] *= k;
        e[1] *= k;
        e[2] *= k;
    }
    public static Vec3 operator +(Vec3 v1, Vec3 v2)
    {
        return new Vec3(v1.e[0] + v2.e[0], v1.e[1] + v2.e[1], v1.e[2] + v2.e[2]);
    }
    public static Vec3 operator -(Vec3 v1, Vec3 v2)
    {
        return new Vec3(v1.e[0] - v2.e[0], v1.e[1] - v2.e[1], v1.e[2] - v2.e[2]);
    }
    public static Vec3 operator -(Vec3 v)
    {
        return new Vec3(-v.e[0], -v.e[1], -v.e[2]);
    }
    public static Vec3 operator *(Vec3 v1, Vec3 v2)
    {
        return new Vec3(v1.e[0] * v2.e[0], v1.e[1] * v2.e[1], v1.e[2] * v2.e[2]);
    }
    public static Vec3 operator *(Vec3 v, float t)
    {
        return new Vec3(v.e[0] * t, v.e[1] * t, v.e[2] * t);
    }
    public static Vec3 operator *(float t, Vec3 v)
    {
        return new Vec3(v.e[0] * t, v.e[1] * t, v.e[2] * t);
    }
    public static Vec3 operator /(Vec3 v1, Vec3 v2)
    {
        return new Vec3(v1.e[0] / v2.e[0], v1.e[1] / v2.e[1], v1.e[2] / v2.e[2]);
    }
    public static Vec3 operator /(Vec3 v, float t)
    {
        return new Vec3(v.e[0] / t, v.e[1] / t, v.e[2] / t);
    }
    public float this[int i]
    {
        get { return e[i]; }
    }

    public static Vec3 unitVector(Vec3 v)
    {
        return v / v.Length();
    }

    public static float dot(Vec3 v1, Vec3 v2)
    {
        return v1.e[0] * v2.e[0] + v1.e[1] * v2.e[1] + v1.e[2] * v2.e[2];
    }
    public static Vec3 cross(Vec3 v1, Vec3 v2)
    {
        return new Vec3(v1.e[1] * v2.e[2] - v1.e[2] * v2.e[1], -(v1.e[0] * v2.e[2] - v1.e[2] * v2.e[0]), v1.e[0] * v2.e[1] - v1.e[1] * v2.e[0]);
    }
}
