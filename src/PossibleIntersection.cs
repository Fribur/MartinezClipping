using Chart3D.Helper.MinHeap;
using System.Collections.Generic;


namespace Martinez
{
    public partial class MartinezClipper
    {
        /// <summary>
        /// Process a possible intersection between the segment associated to the left events e1 and e2
        /// </summary>
        int possibleIntersection(SweepEvent se1, SweepEvent se2, MinHeap<SweepEvent> queue)
        {   
            // that disallows self-intersecting polygons,
            // did cost us half a day, so I'll leave it
            // out of respect
            // if (se1.isSubject === se2.isSubject) return;
            List<double2> inter = intersection(se1.point, se1.otherEvent.point, se2.point, se2.otherEvent.point, false);

            int nintersections = inter!=null ? inter.Count : 0;
            if (nintersections == 0) return 0; // no intersection

            // the line segments intersect at an endpoint of both line segments
            if ((nintersections == 1) &&
                (Helper.Equals(se1.point,se2.point) ||
                 Helper.Equals(se1.otherEvent.point,se2.otherEvent.point)))
            {
                return 0;
            }

            if (nintersections == 2 && se1.isSubject == se2.isSubject)
            {
                // if(se1.contourId === se2.contourId){
                // console.warn('Edges of the same polygon overlap',
                //   se1.point, se1.otherEvent.point, se2.point, se2.otherEvent.point);
                // }
                //throw new Error('Edges of the same polygon overlap');
                return 0;
            }

            // The line segments associated to se1 and se2 intersect
            if (nintersections == 1)
            {
                // if the intersection point is not an endpoint of se1
                if (!Helper.Equals(se1.point,(inter[0])) && !Helper.Equals(se1.otherEvent.point,(inter[0])))
                    DivideSegment(se1, inter[0], ref queue);

                // if the intersection point is not an endpoint of se2
                if (!Helper.Equals(se2.point, (inter[0])) && !Helper.Equals(se2.otherEvent.point, (inter[0])))
                    DivideSegment(se2, inter[0], ref queue);
                return 1;
            }

            // The line segments associated to se1 and se2 overlap
            List<SweepEvent> events = new List<SweepEvent>();
            bool leftCoincide = false;
            bool rightCoincide = false;

            if (Helper.Equals(se1.point,se2.point))
                leftCoincide = true; // linked
            else if (m_compareEvents.Compare(se1, se2) == 1)
            {
                events.Add(se2);
                events.Add(se1);
            }
            else
            {
                events.Add(se1);
                events.Add(se2);
            }

            if (Helper.Equals(se1.otherEvent.point, se2.otherEvent.point))
                rightCoincide = true;
            else if (m_compareEvents.Compare(se1.otherEvent, se2.otherEvent) == 1)
            {
                events.Add(se2.otherEvent);
                events.Add(se1.otherEvent);
            }
            else
            {
                events.Add(se1.otherEvent);
                events.Add(se2.otherEvent);
            }

            if ((leftCoincide && rightCoincide) || leftCoincide)
            {
                // both line segments are equal or share the left endpoint
                se2.type = EdgeType.NON_CONTRIBUTING;
                se1.type = (se2.inOut == se1.inOut) ? EdgeType.SAME_TRANSITION : EdgeType.DIFFERENT_TRANSITION;

                if (leftCoincide && !rightCoincide)
                {
                    // honestly no idea, but changing events selection from [2, 1]
                    // to [0, 1] fixes the overlapping self-intersecting polygons issue
                    DivideSegment(events[1].otherEvent, events[0].point, ref queue);
                }
                return 2;
            }

            // the line segments share the right endpoint
            if (rightCoincide)
            {
                DivideSegment(events[0], events[1].point, ref queue);
                return 3;
            }

            // no line segment includes totally the other one
            if (events[0] != events[3].otherEvent)
            {
                DivideSegment(events[0], events[1].point, ref queue);
                DivideSegment(events[1], events[2].point, ref queue);
                return 3;
            }
            // one line segment includes the other one
            DivideSegment(events[0], events[1].point, ref queue);
            DivideSegment(events[3].otherEvent, events[2].point, ref queue);
            return 3;
        }
    }
} 

