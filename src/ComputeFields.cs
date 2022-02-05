namespace Martinez
{

    public partial class MartinezClipper
    {
        void ComputeFields(SweepEvent m_Event, SweepEvent prev, ClipType operation)
        {
            // compute inOut and otherInOut fields
            if (prev == null)
            {
                m_Event.inOut = false;
                m_Event.otherInOut = true;
            }
            // previous line segment in sweepline belongs to the same polygon
            else
            {
                if (m_Event.isSubject == prev.isSubject)
                {
                    m_Event.inOut = !prev.inOut;
                    m_Event.otherInOut = prev.otherInOut;                    
                }
                // previous line segment in sweepline belongs to the clipping polygon
                else
                {
                    m_Event.inOut = !prev.otherInOut;
                    m_Event.otherInOut = prev.IsVertical() ? !prev.inOut : prev.inOut;
                }

                // compute prevInResult field
                if (prev != null)
                {
                    m_Event.prevInResult = (!inResult(prev, operation) || prev.IsVertical())
                      ? prev.prevInResult : prev;
                }
            }

            // check if the line segment belongs to the Boolean operation
            bool isInResult = inResult(m_Event, operation);
            if (isInResult)
                m_Event.resultTransition = determineResultTransition(m_Event, operation);
            else
                m_Event.resultTransition = 0;
        }
        // check if the line segment belongs to the Boolean operation
        bool inResult(SweepEvent m_event, ClipType operation)
        {
            switch (m_event.type)
            {
                case EdgeType.NORMAL:
                    switch (operation)
                    {
                        case ClipType.Intersection:
                            return !m_event.otherInOut;
                        case ClipType.Union:
                            return m_event.otherInOut;
                        case ClipType.Difference:
                            return (m_event.isSubject && m_event.otherInOut) ||
                                   (!m_event.isSubject && !m_event.otherInOut);
                        case ClipType.Xor:
                            return true;
                    }
                    break;
                case EdgeType.SAME_TRANSITION:
                    return operation == ClipType.Intersection || operation == ClipType.Union;
                case EdgeType.DIFFERENT_TRANSITION:
                    return operation == ClipType.Difference;
                case EdgeType.NON_CONTRIBUTING:
                    return false;
            }
            return false;
        }
        int determineResultTransition(SweepEvent m_Event, ClipType operation)
        {
            bool thisIn = !m_Event.inOut;
            bool thatIn = !m_Event.otherInOut;

            bool isIn = false;
            switch (operation)
            {
                case ClipType.Intersection:
                    isIn = thisIn && thatIn; break;
                case ClipType.Union:
                    isIn = thisIn || thatIn; break;
                case ClipType.Xor:
                    isIn = thisIn ^ thatIn; break;
                case ClipType.Difference:
                    if (m_Event.isSubject)
                        isIn = thisIn && !thatIn;
                    else
                        isIn = thatIn && !thisIn;
                break;
            }
            return isIn ? +1 : -1;
        }
    }
} 

