using System.Linq;

namespace LineWars.Model
{
    public static class PointExtension
    {
        public static bool HasLine(this Point from, Point to)
        {
            return GetLine(from, to) != null;
        }
        
        public static Line GetLine(this Point from, Point to)
        {
            return from.Lines.Intersect(to.Lines).FirstOrDefault();
        }
    }
}