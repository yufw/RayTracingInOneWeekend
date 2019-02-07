using System;
using System.Collections.Generic;

namespace RayTracingInOneWeekend
{
    class Program
    {
        static float hitSphere(Vec3 center, float radius, Ray r)
        {
            Vec3 oc = r.origin() - center;
            float a = Vec3.dot(r.direction(), r.direction());
            float b = 2.0F * Vec3.dot(oc, r.direction());
            float c = Vec3.dot(oc, oc) - radius * radius;
            float discriminant = b * b - 4 * a * c;
            if (discriminant < 0)
            {
                return -1;
            }
            else
            {
                return (-b - (float)Math.Sqrt(discriminant)) / (2.0F * a);
            }
        }
        static Vec3 randomInUnitSphere()
        {
            Random r = new Random();
            Vec3 p;
            do
            {
                p = 2.0F * new Vec3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble()) - new Vec3(1, 1, 1);
            } while (p.SquaredLength() >= 1.0);
            return p;
        }
        static Vec3 color(Ray r, IHitable world, int depth)
        {
            HitRecord rec = new HitRecord();
            if (world.hit(r, 0.001F, float.MaxValue, ref rec))
            {
                Ray scattered = new Ray();
                Vec3 attenuation = new Vec3();
                if (depth < 50 && rec.mat.scatter(r, rec, ref attenuation, ref scattered))
                {
                    return attenuation * color(scattered, world, depth + 1);
                }
                else
                {
                    return new Vec3(0, 0, 0);
                }
            }
            else
            {
                Vec3 unitDirection = Vec3.unitVector(r.direction());
                float t = 0.5F * (unitDirection.y() + 1.0F);
                return (1.0F - t) * new Vec3(1.0F, 1.0F, 1.0F) + t * new Vec3(0.5F, 0.7F, 1.0F);
            }
        }
        static HitableList randomScene()
        {
            List<IHitable> list = new List<IHitable>();
            list.Add(new Sphere(new Vec3(0, -1000, 0), 1000, new Lambertian(new Vec3(0.5F, 0.5F, 0.5F))));
            Random r = new Random();
            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    float chooseMat = (float)r.NextDouble();
                    Vec3 center = new Vec3((float)(a + 0.9 * r.NextDouble()), 0.2F, (float)(b + 0.9 * r.NextDouble()));
                    if ((center - new Vec3(4, 0.2F, 0)).Length() > 0.9)
                    {
                        if (chooseMat < 0.8)
                        {
                            list.Add(new Sphere(center, 0.2F, new Lambertian(new Vec3((float)(r.NextDouble() * r.NextDouble()), (float)(r.NextDouble() * r.NextDouble()), (float)(r.NextDouble() * r.NextDouble())))));
                        }
                        else if (chooseMat < 0.95)
                        {
                            list.Add(new Sphere(center, 0.2F, new Metal(new Vec3((float)(0.5 * (1 + r.NextDouble())), (float)(0.5 * (1 + r.NextDouble())), (float)(0.5 * (1 + r.NextDouble()))), (float)(0.5 * r.NextDouble()))));
                        }
                        else
                        {
                            list.Add(new Sphere(center, 0.2F, new Dielectric(1.5F)));
                        }
                    }
                }
            }
            list.Add(new Sphere(new Vec3(0, 1, 0), 1, new Dielectric(1.5F)));
            list.Add(new Sphere(new Vec3(-4, 1, 0), 1, new Lambertian(new Vec3(0.4F, 0.2F, 0.1F))));
            list.Add(new Sphere(new Vec3(4, 1, 0), 1, new Metal(new Vec3(0.7F, 0.6F, 0.5F), 0.0F)));
            return new HitableList(list);
        }
        static void Main(string[] args)
        {
            int nx = 1200;
            int ny = 800;
            int ns = 10;

            Console.WriteLine("P3");
            Console.WriteLine("1200 800");
            Console.WriteLine("255");

            // List<IHitable> list = new List<IHitable>();
            // list.Add(new Sphere(new Vec3(0, 0, -1), 0.5F, new Lambertian(new Vec3(0.1F, 0.2F, 0.5F))));
            // list.Add(new Sphere(new Vec3(0, -100.5F, -1), 100, new Lambertian(new Vec3(0.8F, 0.8F, 0.0F))));
            // list.Add(new Sphere(new Vec3(1, 0, -1), 0.5F, new Metal(new Vec3(0.8F, 0.6F, 0.2F), 0.0F)));
            // list.Add(new Sphere(new Vec3(-1, 0, -1), 0.5F, new Dielectric(1.5F)));
            // list.Add(new Sphere(new Vec3(-1, 0, -1), -0.45F, new Dielectric(1.5F)));

            // float R = (float)Math.Cos(Math.PI/4);
            // list.Add(new Sphere(new Vec3(-R, 0, -1), R, new Lambertian(new Vec3(0, 0, 1))));
            // list.Add(new Sphere(new Vec3(R, 0, -1), R, new Lambertian(new Vec3(1, 0, 0))));
            HitableList world = randomScene();

            Vec3 lookfrom = new Vec3(13, 2, 3);
            Vec3 lookat = new Vec3(0, 0, 0);
            float distToFocus = 10;
            float aperture = 0.1F;
            Camera cam = new Camera(lookfrom, lookat, new Vec3(0, 1, 0), 20, (float)nx / (float)ny, aperture, distToFocus);
            Random randObj = new Random();
            for (int j = ny - 1; j >= 0; j--)
            {
                for (int i = 0; i < nx; i++)
                {
                    Vec3 col = new Vec3(0, 0, 0);
                    for (int s = 0; s < ns; s++)
                    {
                        float u = (float)(i + randObj.NextDouble()) / (float)nx;
                        float v = (float)(j + randObj.NextDouble()) / (float)ny;
                        Ray r = cam.getRay(u, v);
                        col += color(r, world, 0);

                    }
                    col /= ns;
                    int ir = (int)(255.99 * Math.Sqrt(col[0]));
                    int ig = (int)(255.99 * Math.Sqrt(col[1]));
                    int ib = (int)(255.99 * Math.Sqrt(col[2]));
                    Console.WriteLine(String.Format("{0} {1} {2}", ir, ig, ib));
                }
            }
        }
    }
}
