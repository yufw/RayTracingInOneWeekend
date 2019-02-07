public class Ray
{
    private Vec3 A;
    private Vec3 B;
    
    public Ray() {}
    public Ray(Vec3 a, Vec3 b) { A = a; B = b; }
    public Vec3 origin() { return A; }
    public Vec3 direction() { return B; }
    public Vec3 pointAtParameter(float t) { return A + t * B; }
}