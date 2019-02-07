public struct HitRecord
{
    public float t;
    public Vec3 p;
    public Vec3 normal;
    public Material mat;
}

public interface IHitable
{
    bool hit(Ray r, float tMin, float tMax, ref HitRecord rec);
}