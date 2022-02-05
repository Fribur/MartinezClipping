using System.Collections.Generic;

namespace Martinez
{    

    public class CompareSegments : IComparer<SweepEvent>
    {
        static CompareEvents m_compareEvents = new CompareEvents();

        public int Compare(SweepEvent le1, SweepEvent le2)
        {
            if (le1 == le2) return 0;

            // Segments are not collinear
            if (Helper.signedArea(le1.point, le1.otherEvent.point, le2.point) != 0 ||
              Helper.signedArea(le1.point, le1.otherEvent.point, le2.otherEvent.point) != 0)
            {

                // If they share their left endpoint use the right endpoint to sort
                if (Helper.Equals(le1.point,le2.point)) return le1.IsBelow(le2.otherEvent.point) ? -1 : 1;

                // Different left endpoint: use the left endpoint to sort
                if (Helper.Equals(le1.point.x,le2.point.x)) return le1.point.y < le2.point.y ? -1 : 1;

                // has the line segment associated to e1 been inserted
                // into S after the line segment associated to e2 ?
                if (m_compareEvents.Compare(le1, le2) == 1) return le2.IsAbove(le1.point) ? -1 : 1;

                // The line segment associated to e2 has been inserted
                // into S after the line segment associated to e1
                return le1.IsBelow(le2.point) ? -1 : 1;
            }

            if (le1.isSubject == le2.isSubject) // same polygon
            { 
                double2 p1 = le1.point;
                double2 p2 = le2.point;
                if (p1.x == p2.x && p1.y == p2.y/*equals(le1.point, le2.point)*/) //use exact comparison here!
                {
                    p1 = le1.otherEvent.point; p2 = le2.otherEvent.point;
                    if (p1.x == p2.x && p1.y == p2.y) return 0; //use exact comparison here!
                    else return le1.contourId > le2.contourId ? 1 : -1;
                }
            }
            else
            { // Segments are collinear, but belong to separate polygons
                return le1.isSubject ? -1 : 1;
            }

            return m_compareEvents.Compare(le1, le2) == 1 ? 1 : -1;
        }
        
    }    
}
