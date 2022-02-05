using System.Collections.Generic;


namespace Martinez
{
    public struct Polygon
    {
        public List<double2> nodes;
        public List<int> startIDs;
        public Polygon(int size)
        {
            nodes = new List<double2>(size);
            startIDs = new List<int>();
        }
        public Polygon(int NodeSize, int Components)
        {
            nodes = new List<double2>(NodeSize);
            startIDs = new List<int>(Components);
        }
        public void AddComponent()
        {
            startIDs.Add(this.nodes.Count);
        }        
        public void Clear()
        {
            nodes.Clear();
            startIDs.Clear();
        }
    }
}

