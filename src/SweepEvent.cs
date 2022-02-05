
namespace Martinez
{
    public class SweepEvent
    {
        public AVLNode<SweepEvent> positionInSweepLine;
        public double2 point;
        public bool left;                       // Is left endpoint?
        public SweepEvent otherEvent;           // Other edge reference
        public bool isSubject;                  //Belongs to source or clipping polygon
        public EdgeType type;               //Edge contribution type       
        public bool inOut;                      //In-out transition for the sweepline crossing polygon
        public bool otherInOut;                 // a vertical ray from (p.x, -infinite) that crosses the edge
        public SweepEvent prevInResult;         //Previous event in result?
        public int resultTransition;            //Type of result transition (0 = not in result, +1 = out-in, -1, in-out)
        public bool inside;                     // Is the edge inside of another polygon
        public int otherPos;
        public int contourId;

        /// <summary>
        /// Sweepline event
        /// </summary>
        /// <param name="point"></param>
        /// <param name="left"></param>
        /// <param name="otherEvent"></param>
        /// <param name="isSubject"></param>
        /// <param name="edgeType"></param>
        public SweepEvent(double2 point, bool left, SweepEvent otherEvent, bool isSubject, EdgeType edgeType = EdgeType.NORMAL)
        {
            this.point      = point;
            this.left       = left;
            this.isSubject  = isSubject;
            this.otherEvent = otherEvent;
            this.type   = edgeType;
            this.inOut = false;
            this.otherInOut = false;
            this.prevInResult = null;
            this.resultTransition = 0;
            this.otherPos = -1;
            this.contourId = -1;
            this.positionInSweepLine = null;
        }

        public bool IsBelow(double2 p)
        {
            double2 p0 = point;
            double2 p1 = otherEvent.point;
            return left
                ? (p0.x - p.x) * (p1.y - p.y) - (p1.x - p.x) * (p0.y - p.y) > 0  // signedArea(this.point, this.otherEvent.point, p) > 0 :
                : (p1.x - p.x) * (p0.y - p.y) - (p0.x - p.x) * (p1.y - p.y) > 0; //signedArea(this.otherEvent.point, this.point, p) > 0;
        }
        public bool IsAbove(double2 p)
        {
            return !IsBelow(p);
        }
        public bool IsVertical()
        {
            return this.point.x == this.otherEvent.point.x;
        }
        // Does event belong to result?
        public bool inResult
        {
            get { return resultTransition != 0; }
        }
        public override string ToString()
        {
            string result;
            if (left)
                result = $" event {point}───>{otherEvent.point} ";
            else
                result = $" other {otherEvent.point}───>{point}  ";
            return result;
        }
    };
} 
