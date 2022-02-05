using System.Collections.Generic;

namespace Martinez
{ 
    public class Contour
    {
        public List<double2> points;
        public List<int> holeIds;
        public int holeOf;
        public int depth;
        bool clockwise;
        bool orientationCalculated;
        public Contour()
        {
            points = new List<double2>();
            holeIds = new List<int>();
        }
        public bool isExterior
        {
            get { return this.holeOf== -1 ; }
        }
        public bool isClockwise
        {
            get 
            {
                if (orientationCalculated) 
                    return clockwise;
                orientationCalculated = true;
                var signedArea = SignedArea(points);
                clockwise = signedArea < 0 ? true : false;
                return clockwise;
            }
        }
        public double SignedArea(List<double2> data)
        {
            int start = 0, end = data.Count - 1;
            double area = default;
            for (int i = start, j = end - 1; i < end; j = i++) //from (0, prev) until (end, prev)
                area += (data[i].x - data[j].x) * (data[i].y + data[j].y) * 0.5;
            return area;
        }
        
    }
}