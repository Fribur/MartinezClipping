using Chart3D.Helper.MinHeap;

namespace Martinez
{

    public partial class MartinezClipper
    {
        CompareEvents m_compareEvents = new CompareEvents();

        // /** @brief Divide the segment associated to left event e, updating pq and (implicitly) the status line */
        void DivideSegment(SweepEvent se, double2 p, ref MinHeap<SweepEvent> queue)
        {
            if(p.x == se.point.x && p.y < se.point.y)
                p.x = p.x + EPSILON_DBL;
                
            // "Left event" of the "right line segment" resulting from dividing e (the line segment associated to e)
            SweepEvent r = new SweepEvent(p, false, se, se.isSubject);
            SweepEvent l = new SweepEvent(p, true, se.otherEvent, se.isSubject);

            if (Helper.Equals(se.point,se.otherEvent.point))
                Console.WriteLine("what is that, a collapsed segment?" + se.ToString());

            r.contourId = l.contourId = se.contourId;

            // avoid a rounding error. The left event would be processed after the right event
            if (m_compareEvents.Compare(l, se.otherEvent) > 0)
            {
                se.otherEvent.left = true;
                l.left = false;
            }
            se.otherEvent.otherEvent = l;
            se.otherEvent = r;

            queue.Push(l);
            queue.Push(r);
        }
    }
} 

