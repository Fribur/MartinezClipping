using System.Collections.Generic;


namespace Martinez
{        
    public struct double2
    {
        public double x;
        public double y;

        public double2(double x, double y)
        {
            this.x = x; this.y = y;
        }
        public double2(double v)
        {
            this.x = v;
            this.y = v;
        }
        public static double2 operator *(double2 lhs, double2 rhs) { return new double2(lhs.x * rhs.x, lhs.y * rhs.y); }
        public static double2 operator *(double2 lhs, double rhs) { return new double2(lhs.x * rhs, lhs.y * rhs); }
        public static double2 operator *(double lhs, double2 rhs) { return new double2(lhs * rhs.x, lhs * rhs.y); }
        public static double2 operator +(double2 lhs, double2 rhs) { return new double2(lhs.x + rhs.x, lhs.y + rhs.y); }
        public static double2 operator -(double2 lhs, double2 rhs) { return new double2(lhs.x - rhs.x, lhs.y - rhs.y); }
        public static double2 operator /(double2 lhs, double2 rhs) { return new double2(lhs.x / rhs.x, lhs.y / rhs.y); }
        public static double2 operator %(double2 lhs, double2 rhs) { return new double2(lhs.x % rhs.x, lhs.y % rhs.y); }
        public static double2 operator ++(double2 val) { return new double2(++val.x, ++val.y); }
        public static double2 operator --(double2 val) { return new double2(--val.x, --val.y); }
        public override string ToString()
        {
            return string.Format("{0}, {1}", x, y);
        }

    }
}

