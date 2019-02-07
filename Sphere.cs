using System;

public class Sphere : IHitable
{
    private Vec3 center;
    private float radius;
    private Material mat;
    public Sphere() { }
    public Sphere(Vec3 cen, float r, Material m) { center = cen; radius = r; mat = m; }
    public bool hit(Ray r, float tMin, float tMax, ref HitRecord rec)
    {
        Vec3 oc = r.origin() - center;
        float a = Vec3.dot(r.direction(), r.direction());
        float b = Vec3.dot(oc, r.direction());
        float c = Vec3.dot(oc, oc) - radius * radius;
        float discriminant = b * b - a * c;
        if (discriminant > 0)
        {
            float temp = (-b - (float)Math.Sqrt(b*b - a*c))/a;
            if (temp > tMin && temp < tMax)
            {
                rec.t = temp;
                rec.p = r.pointAtParameter(rec.t);
                rec.normal = (rec.p - center) / radius;
                rec.mat = mat;
                return true;
            }
            temp = (-b + (float)Math.Sqrt(b*b - a*c))/a;
            if (temp > tMin && temp < tMax)
            {
                rec.t = temp;
                rec.p = r.pointAtParameter(rec.t);
                rec.normal = (rec.p - center) / radius;
                rec.mat = mat;
                return true;
            }
        }
        return false;
    }
}