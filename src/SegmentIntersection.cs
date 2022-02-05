using System.Collections.Generic;
using System;

namespace Martinez
{
    public partial class MartinezClipper
    {
        const double EPSILON_DBL = 0.000000001f;

        /// <summary>
        /// Finds the magnitude of the cross product of two vectors (if we pretend 
        /// they're in three dimensions)
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>The magnitude of the cross product</returns>
        double crossProduct(double2 a, double2 b)
        {
            return (a.x * b.y) - (a.y * b.x);
        }

        /// <summary>
        /// Finds the dot product of two vectors.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>The dot product</returns>
        double dotProduct(double2 a, double2 b)
        {
            return (a.x * b.x) + (a.y * b.y);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1">point of first line</param>
        /// <param name="a2">point of first line</param>
        /// <param name="b1">point of second line</param>
        /// <param name="b2">point of second line</param>
        /// <param name="noEndpointTouch">whether to skip single touchpoints (meaning connected segments) as intersections</param>
        /// <param name="intersection"></param>
        /// <returns>If the lines intersect, the point of intersection.If they overlap, the two end points 
        /// of the overlapping segment. Otherwise, null.</returns>
       List<double2> intersection(double2 a1, double2 a2, double2 b1, double2 b2, bool noEndpointTouch)
        {            
            List<double2> returnList= new List<double2>(2); 
            // The algorithm expects our lines in the form P + sd, where P is a point,
            // s is on the interval [0, 1], and d is a vector.
            // We are passed two points. P can be the first point of each pair. The
            // vector, then, could be thought of as the distance (in x and y components)
            // from the first point to the second point.
            // So first, let's make our vectors:
            double2 va = a2 - a1;
            double2 vb = b2 - b1;
            // We also define a function to convert back to regular point form:

            double2 toPoint(double2 p, double s, double2 d)
            {
                return p + s * d;
            }

            // The rest is pretty much a straight port of the algorithm.
            double2 e = b1 - a1;
            double kross = crossProduct(va, vb);
            double sqrKross = kross * kross;
            double sqrLenA = dotProduct(va, va);



            // Check for line intersection. This works because of the properties of the
            // cross product -- specifically, two vectors are parallel if and only if the
            // cross product is the 0 vector. The full calculation involves relative error
            // to account for possible very small line segments. See Schneider & Eberly
            // for details.
            if (sqrKross > 0/* EPS * sqrLenB * sqLenA */)
            {
                // If they're not parallel, then (because these are line segments) they
                // still might not actually intersect. This code checks that the
                // intersection point of the lines is actually on both line segments.
                double s = crossProduct(e, vb) / kross;
               


                if (s < 0 || s > 1) // not on line segment a
                {                    
                    return null;
                }

                double t = crossProduct(e, va) / kross;                

                if (t < 0 || t > 1)
                {                    
                    return null; // not on line segment b
                }

                //if (s == 0 || s == 1)
                if(Helper.Equals(s, 0) || Helper.Equals(s, 1))
                {
                    // on an endpoint of line segment a
                    if (noEndpointTouch)
                        return null;
                    else
                    {
                        returnList.Add(toPoint(a1, s, va));
                        return returnList;
                    }
                }
                //if (t == 0 || t == 1)
                if (Helper.Equals(t, 0) || Helper.Equals(t, 1))
                {
                    // on an endpoint of line segment b
                    if (noEndpointTouch)
                        return null;
                    else
                    {
                        returnList.Add(toPoint(b1, t, vb));
                        return returnList;
                    }
                }
                returnList.Add(toPoint(a1, s, va));
                return returnList;
            }
            // If we've reached this point, then the lines are either parallel or the
            // same, but the segments could overlap partially or fully, or not at all.
            // So we need to find the overlap, if any. To do that, we can use e, which is
            // the (vector) difference between the two initial points. If this is parallel
            // with the line itself, then the two lines are the same line, and there will
            // be overlap.
            //const sqrLenE = dotProduct(e, e);
            kross = crossProduct(e, va);
            sqrKross = kross * kross;

            if (sqrKross > 0 /* EPS * sqLenB * sqLenE */)
            {
                // Lines are just parallel, not the same. No overlap.
                return null;
            }
            double sa = dotProduct(va, e) / sqrLenA;
            double sb = sa + dotProduct(va, vb) / sqrLenA;
            double smin = Math.Min(sa, sb);
            double smax = Math.Max(sa, sb);


            // this is, essentially, the FindIntersection acting on floats from
            // Schneider & Eberly, just inlined into this function.
            if (smin <= 1 && smax >= 0)
            {
                // overlap on an end point
                //if (smin == 1)
                if (Helper.Equals(smin, 1))
                {
                    if (noEndpointTouch)
                        return null;
                    else
                    {
                        returnList.Add(toPoint(a1, smin > 0 ? smin : 0, va));
                        return returnList;
                    }
                }

                //if (smax == 0)
                if (Helper.Equals(smax, 0)) 
                {
                    if (noEndpointTouch)
                        return null;
                    else
                    {
                        returnList.Add(toPoint(a1, smax < 1 ? smax : 1, va));
                        return returnList;
                    }
                }

                //if (noEndpointTouch && smin == 0 && smax == 1) return null;
                if (noEndpointTouch && Helper.Equals(smin, 0) && Helper.Equals(smax, 1)) return null;

                // There's overlap on a segment -- two points of intersection. Return both.
                returnList.Add(toPoint(a1, smin > 0 ? smin : 0, va));
                returnList.Add(toPoint(a1, smax < 1 ? smax : 1, va));
                return returnList;
            }
            return null;
        }        
    }
} 


