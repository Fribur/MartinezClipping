using System.Collections.Generic;

namespace Martinez
{        
    public static class Helper
    {
        public static int specialCases(SweepEvent e1, SweepEvent e2, double2 p1, double2 p2)
        {
            // Same coordinates, but one is a left endpoint and the other is
            // a right endpoint. The right endpoint is processed first
            if (e1.left != e2.left)
                return e1.left ? 1 : -1;

            // const p2 = e1.otherEvent.point, p3 = e2.otherEvent.point;
            // const sa = (p1[0] - p3[0]) * (p2[1] - p3[1]) - (p2[0] - p3[0]) * (p1[1] - p3[1])
            // Same coordinates, both events
            // are left endpoints or right endpoints.
            // not collinear
            if (signedArea(p1, e1.otherEvent.point, e2.otherEvent.point) != 0)
            {
                // the event associate to the bottom segment is processed first
                return (!e1.IsBelow(e2.otherEvent.point)) ? 1 : -1;
            }
            if (e1.isSubject != e2.isSubject)
                return (!e1.isSubject && e2.isSubject) ? 1 : -1;
            return 0;
        }
        public static int orient2d(double2 p0, double2 p1, double2 p2)
        {
            var res = (p0.x - p2.x) * (p1.y - p2.y) - (p0.y - p2.y) * (p1.x - p2.x);
            if (res > 0) return -1; //review hwat happens if res == ZERO
            if (res < 0) return 1;
            return 0;
        }
        public static int signedArea(double2 p0, double2 p1, double2 p2)
        {
            return orient2d(p0, p1, p2);
        }

        const double absTol = 0.000000001f;
        const double relTol = 0.000000001f;
        public static bool Equals(double a, double b)
        {
            //return a == b;
            return (Math.Abs(a - b) <= Math.Max(absTol, relTol * Math.Max(Math.Abs(a), Math.Abs(b))));
        }
        /// <summary>
        /// https://realtimecollisiondetection.net/blog/?p=89
        /// </summary>

        public static bool Equals(double2 a, double2 b)
        {
            //return a.x == b.x && a.y == b.y;
            return Equals(a.x, b.x) && Equals(a.y, b.y);
        }
    }
}
