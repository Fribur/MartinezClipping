using System.Collections.Generic;
using Chart3D.Helper.MinHeap;

namespace Martinez
{
    public partial class MartinezClipper
    {        
        void processPolygon(List<double2> contourOrHole, bool isSubject, int depth, ref MinHeap<SweepEvent> Q, ref double2x2 bbox)
        {
            int i, len;
            double2 s1, s2;
            SweepEvent e1, e2;
            for (i = 0, len = contourOrHole.Count - 1; i < len; i++)
            {
                s1 = contourOrHole[i];
                s2 = contourOrHole[i + 1];
                e1 = new SweepEvent(s1, false, null, isSubject);
                e2 = new SweepEvent(s2, false, e1, isSubject);
                e1.otherEvent = e2;

                //if (s1.x == s2.x && s1.y == s2.y)
                if (Helper.Equals(s1, s2))
                    continue; // skip collapsed edges, or it breaks

                e1.contourId = e2.contourId = depth;

                if (m_compareEvents.Compare(e1, e2) > 0)
                    e2.left = true;
                else
                    e1.left = true;

                double x = s1.x, y = s1.y;
                bbox.c0.x = Math.Min(bbox.c0.x, x);
                bbox.c0.y = Math.Min(bbox.c0.y, y);
                bbox.c1.x = Math.Max(bbox.c1.x, x);
                bbox.c1.y = Math.Max(bbox.c1.y, y);

                // Pushing it so the queue is sorted from left to right,
                // with object on the left having the highest priority.
                Q.Push(e1);
                Q.Push(e2);
            }
        }
        MinHeap<SweepEvent> fillQueue(List<Polygon> subject, List<Polygon> clipping, ref double2x2 sbbox, ref double2x2 cbbox, ClipType operation)
        {
            int contourId = 0;
            MinHeap<SweepEvent> eventQueue = new MinHeap<SweepEvent>(new CompareEvents());
            Polygon polygonSet;
            bool isExteriorRing;
            int i, ii, j, jj, k;//, kk;

            for (i = 0, ii = subject.Count; i < ii; i++)
            {
                polygonSet = subject[i];
                for (j = 0, jj = polygonSet.startIDs.Count - 1; j < jj; j++)
                {
                    isExteriorRing = j == 0;
                    if (isExteriorRing) contourId++;
                    int start = polygonSet.startIDs[j];
                    int end = polygonSet.startIDs[j+1];
                    List<double2> component=new List<double2>();
                    for (k = start; k < end; k++)
                        component.Add(polygonSet.nodes[k]);
                    component.Add(polygonSet.nodes[start]); //close the ring so intersection between end and start are detected.
                    processPolygon(component, true, contourId, ref eventQueue, ref sbbox);
                }
            }

            for (i = 0, ii = clipping.Count; i < ii; i++)
            {
                polygonSet = clipping[i];
                for (j = 0, jj = polygonSet.startIDs.Count - 1; j < jj; j++)
                {
                    isExteriorRing = j == 0;
                    if (operation == ClipType.Difference) isExteriorRing = false;
                    if (isExteriorRing) contourId++;
                    int start = polygonSet.startIDs[j];
                    int end = polygonSet.startIDs[j + 1];
                    List<double2> component = new List<double2>();
                    for (k = start; k < end; k++)
                        component.Add(polygonSet.nodes[k]);
                    component.Add(polygonSet.nodes[start]); //close the ring so intersection between end and start are detected.
                    processPolygon(component, false, contourId, ref eventQueue, ref cbbox);
                }
            }
            return eventQueue;
        }
    }
} 

