using System.Collections.Generic;


namespace Martinez
{
    public partial struct double2x2
    {
        public double2 c0;
        public double2 c1;


        /// <summary>Constructs a double2x2 matrix from two double2 vectors.</summary>
        public double2x2(double2 c0, double2 c1)
        {
            this.c0 = c0;
            this.c1 = c1;
        }
        public double2x2(double2 v)
        {
            this.c0 = v;
            this.c1 = v;
        }

    }
}

