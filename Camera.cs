using System;
public class Camera
{
    public static Vec3 randomInUnitDisk()
    {
        Random r = new Random();
        Vec3 p;
        do
        {
            p = 2.0F * new Vec3((float)r.NextDouble(), (float)r.NextDouble(), 0) - new Vec3(1, 1, 0);
        } while (Vec3.dot(p, p) >= 1.0);
        return p;
    }
    private Vec3 lowerLeftCorner;
    private Vec3 origin;
    private Vec3 horizontal;
    private Vec3 vertical;
    private Vec3 u, v, w;
    private float lensRadius;
    public Camera(Vec3 lookfrom, Vec3 lookat, Vec3 vup, float vfov, float aspect, float aperture, float focusDist)
    {
        lensRadius = aperture / 2;
        float theta = vfov * (float)Math.PI / 180;
        float halfHeight = (float)Math.Tan(theta / 2);
        float halfWidth = aspect * halfHeight;
        origin = lookfrom;
        w = Vec3.unitVector(lookfrom - lookat);
        u = Vec3.unitVector(Vec3.cross(vup, w));
        v = Vec3.cross(w, u);
        lowerLeftCorner = origin - halfWidth * focusDist * u - halfHeight * focusDist * v - focusDist * w;
        horizontal = 2 * halfWidth * focusDist * u;
        vertical = 2 * halfHeight * focusDist * v;
    }
    public Ray getRay(float s, float t)
    {
        Vec3 rd = lensRadius * randomInUnitDisk();
        Vec3 offset = u * rd.x() + v * rd.y();
        return new Ray(origin + offset, lowerLeftCorner + s * horizontal + t * vertical - origin - offset);
    }
}