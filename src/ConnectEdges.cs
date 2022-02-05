using System;

namespace Martinez
{
    public partial class MartinezClipper
    {
        List<SweepEvent> orderEvents(List<SweepEvent> sortedEvents)
        {
            SweepEvent m_event, tmp;
            int i, len, tmp2;
            List<SweepEvent> resultEvents = new List<SweepEvent>();
            for (i = 0, len = sortedEvents.Count; i < len; i++)
            {
                m_event = sortedEvents[i];
                if ((m_event.left && m_event.inResult) || 
                    (!m_event.left && m_event.otherEvent.inResult))
                    resultEvents.Add(m_event);
            }

            // Due to overlapping edges the resultEvents array can be not wholly sorted
            bool sorted = false;
            while (!sorted)
            {
                sorted = true;
                for (i = 0, len = resultEvents.Count; i < len; i++)
                {
                    if ((i + 1) < len && 
                        m_compareEvents.Compare(resultEvents[i], resultEvents[i + 1]) == 1)
                    {
                        tmp = resultEvents[i];
                        resultEvents[i] = resultEvents[i + 1];
                        resultEvents[i + 1] = tmp;
                        sorted = false;
                    }
                }
            }

            for (i = 0, len = resultEvents.Count; i < len; i++)
            {
                m_event = resultEvents[i];
                m_event.otherPos = i;
            }

            // imagine, the right event is found in the beginning of the queue,
            // when his left counterpart is not marked yet
            for (i = 0, len = resultEvents.Count; i < len; i++)
            {
                m_event = resultEvents[i];
                if (!m_event.left)
                {
                    tmp2 = m_event.otherPos;
                    m_event.otherPos = m_event.otherEvent.otherPos;
                    m_event.otherEvent.otherPos = tmp2;
                }
            }
            return resultEvents;
        }
        int nextPos(int pos, List<SweepEvent> resultEvents, bool[] processed, int origPos)
        {
            int newPos = pos + 1;
            double2 p = resultEvents[pos].point;
            double2 p1 = default;
            int length = resultEvents.Count;

            if (newPos < length)
                p1 = resultEvents[newPos].point;

            //while (newPos < length && p1.x == p.x && p1.y == p.y)
            while (newPos < length && Helper.Equals(p1, p))
            {
                if (!processed[newPos])
                    return newPos;
                else
                    newPos++;
                p1 = resultEvents[newPos].point;
            }

            newPos = pos - 1;

            while (processed[newPos] && newPos > origPos)
                newPos--;

            return newPos;
        }
        Contour initializeContourFromContext(SweepEvent m_event, List<Contour> contours, int contourId)
        {
            Contour contour = new Contour();
            if (m_event.prevInResult != null)
            {
                SweepEvent prevInResult = m_event.prevInResult;
                // Note that it is valid to query the "previous in result" for its output contour id,
                // because we must have already processed it (i.e., assigned an output contour id)
                // in an earlier iteration, otherwise it wouldn't be possible that it is "previous in
                // result".
                int lowerContourId = prevInResult.contourId;
                int lowerResultTransition = prevInResult.resultTransition;
                if (lowerResultTransition > 0)
                {
                    // We are inside. Now we have to check if the thing below us is another hole or
                    // an exterior contour.
                    Contour lowerContour = contours[lowerContourId];
                    if (lowerContour.holeOf != -1)
                    {
                        // The lower contour is a hole => Connect the new contour as a hole to its parent,
                        // and use same depth.
                        int parentContourId = lowerContour.holeOf;
                        contours[parentContourId].holeIds.Add(contourId);
                        contour.holeOf = parentContourId;
                        contour.depth = contours[lowerContourId].depth;
                    }
                    else
                    {
                        // The lower contour is an exterior contour => Connect the new contour as a hole,
                        // and increment depth.
                        contours[lowerContourId].holeIds.Add(contourId);
                        contour.holeOf = lowerContourId;
                        contour.depth = contours[lowerContourId].depth + 1;
                    }
                }
                else
                {
                    // We are outside => this contour is an exterior contour of same depth.
                    contour.holeOf = -1;
                    contour.depth = contours[lowerContourId].depth;
                }
            }
            else
            {
                // There is no lower/previous contour => this contour is an exterior contour of depth 0.
                contour.holeOf = -1;
                contour.depth = 0;
            }
            return contour;
        }
        List<Contour> connectEdges(List<SweepEvent> sortedEvents)
        {
            int i, len;
            List<SweepEvent> resultEvents = orderEvents(sortedEvents);
            len = resultEvents.Count;

            // "false"-filled array
            bool[] processed = new bool[len];
            List<Contour> contours = new List<Contour>(len);

            for (i = 0; i < len; i++)
            {
                if (processed[i])
                    continue;

                int contourId = contours.Count;
                Contour contour = initializeContourFromContext(resultEvents[i], contours, contourId);

                //// Helper function that combines marking an event as processed with assigning its output contour ID
                Action<int> markAsProcessed = (pos) =>
                {
                    processed[pos] = true;
                    resultEvents[pos].contourId = contourId;
                };

                int pos = i;
                int origPos = i;

                double2 initial = resultEvents[i].point;
                contour.points.Add(initial);

                while (true)
                {
                    markAsProcessed(pos);

                    pos = resultEvents[pos].otherPos;

                    markAsProcessed(pos);
                    contour.points.Add(resultEvents[pos].point);

                    pos = nextPos(pos, resultEvents, processed, origPos);

                    if (pos == origPos)
                        break;
                }
                contours.Add(contour);
            }
            return contours;
        }
    }
} 