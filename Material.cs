using System;
abstract public class Material
{
    public static Vec3 randomInUnitSphere()
    {
        Random r = new Random();
        Vec3 p;
        do
        {
            p = 2.0F * new Vec3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble()) - new Vec3(1, 1, 1);
        } while (p.SquaredLength() >= 1.0);
        return p;
    }
    public static Vec3 reflect(Vec3 v, Vec3 n)
    {
        return v - 2 * Vec3.dot(v, n) * n;
    }
    abstract public bool scatter(Ray rin, HitRecord rec, ref Vec3 attenuation, ref Ray scattered);
}

public class Lambertian : Material
{
    private Vec3 albedo;
    public Lambertian(Vec3 a) { albedo = a; }
    public override bool scatter(Ray rin, HitRecord rec, ref Vec3 attenuation, ref Ray scattered)
    {
        Vec3 target = rec.p + rec.normal + randomInUnitSphere();
        scattered = new Ray(rec.p, target - rec.p);
        attenuation = albedo;
        return true;
    }
}

public class Metal : Material
{
    private Vec3 albedo;
    private float fuzz;
    public Metal(Vec3 a, float f)
    {
        albedo = a;
        if (f < 1)
        {
            fuzz = f;
        }
        else
        {
            fuzz = 1;
        }
    }
    public override bool scatter(Ray rin, HitRecord rec, ref Vec3 attenuation, ref Ray scattered)
    {
        Vec3 reflected = reflect(Vec3.unitVector(rin.direction()), rec.normal);
        scattered = new Ray(rec.p, reflected + fuzz * randomInUnitSphere());
        attenuation = albedo;
        return Vec3.dot(scattered.direction(), rec.normal) > 0;
    }
}

public class Dielectric : Material
{
    static bool refract(Vec3 v, Vec3 n, float niOverNt, ref Vec3 refracted)
    {
        Vec3 uv = Vec3.unitVector(v);
        float dt = Vec3.dot(uv, n);
        float discriminant = 1.0F - niOverNt * niOverNt * (1 - dt * dt);
        if (discriminant > 0)
        {
            refracted = niOverNt * (uv - n * dt) - n * (float)Math.Sqrt(discriminant);
            return true;
        }
        else
        {
            return false;
        }
    }
    static float schlick(float cosine, float refIdx)
    {
        float r0 = (1-refIdx) / (1+refIdx);
        r0 = r0 * r0;
        return r0 + (1-r0) * (float)Math.Pow(1-cosine, 5);
    }
    private float refIdx;
    public Dielectric(float ri) { refIdx = ri; }
    public override bool scatter(Ray rin, HitRecord rec, ref Vec3 attenuation, ref Ray scattered)
    {
        Vec3 outwardNormal;
        Vec3 reflected = reflect(rin.direction(), rec.normal);
        float niOverNt;
        attenuation = new Vec3(1.0F, 1.0F, 1.0F);
        Vec3 refracted = new Vec3();
        float reflectProb;
        float cosine;
        if (Vec3.dot(rin.direction(), rec.normal) > 0)
        {
            outwardNormal = -rec.normal;
            niOverNt = refIdx;
            cosine = refIdx * Vec3.dot(rin.direction(), rec.normal) / rin.direction().Length();
        }
        else
        {
            outwardNormal = rec.normal;
            niOverNt = 1 / refIdx;
            cosine = -Vec3.dot(rin.direction(), rec.normal) / rin.direction().Length();
        }
        if (refract(rin.direction(), outwardNormal, niOverNt, ref refracted))
        {
            reflectProb = schlick(cosine, refIdx);
        }
        else
        {
            scattered = new Ray(rec.p, reflected);
            reflectProb = 1.0F;
        }
        if (new Random().NextDouble() < reflectProb)
        {
            scattered = new Ray(rec.p, reflected);
        }
        else
        {
            scattered = new Ray(rec.p, refracted);
        }
        return true;
    }
}
