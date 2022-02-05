using System.Collections.Generic;

namespace Martinez
{
    public class CompareEvents : IComparer<SweepEvent>
    {
        public int Compare(SweepEvent e1, SweepEvent e2)
        {
            double2 p1 = e1.point;
            double2 p2 = e2.point;

            // Different x-coordinate
            if (p1.x > p2.x) return 1;
            if (p1.x < p2.x) return -1;

            // Different points, but same x-coordinate
            // Event with lower y-coordinate is processed first
            if (!(p1.y == p2.y)) return p1.y > p2.y ? 1 : -1; //exact equality test is needed here!
            return Helper.specialCases(e1, e2, p1, p2);
        }
    }    
}
