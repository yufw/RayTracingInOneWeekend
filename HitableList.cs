using System.Collections.Generic;

public class HitableList : IHitable
{
    private List<IHitable> list;
    public HitableList() { }
    public HitableList(List<IHitable> l) { list = l; }
    public bool hit(Ray r, float tMin, float tMax, ref HitRecord rec)
    {
        HitRecord tempRec = new HitRecord();
        bool hitAnything = false;
        float closestSoFar = tMax;
        foreach (var item in list)
        {
            if (item.hit(r, tMin, closestSoFar, ref tempRec))
            {
                hitAnything = true;
                closestSoFar = tempRec.t;
                rec = tempRec;
            }
        }
        return hitAnything;
    }
}