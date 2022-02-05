using Martinez;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No arguments provided");
            return;
        }

        ClipType cliptype;
        int argID = 0;
        if (args[argID] == "-union")
        {
            cliptype = ClipType.Union;
            Console.WriteLine("Computing Union");
            argID++;
        }
        else
        {
            cliptype = ClipType.Intersection;
            Console.WriteLine("Computing Difference");
        }

        if (args.Length > 2)
        {
            List<Polygon> subject = new List<Polygon>();
            List<Polygon> clipping = new List<Polygon>();
            if (File.Exists(args[argID]))
                subject.Add(GetPolygonFromFile(args[argID++]));
            else
            {
                Console.WriteLine("Cannot access Subject Polygon");
                return;
            }
            if (File.Exists(args[argID]))
                clipping.Add(GetPolygonFromFile(args[argID++]));
            else
            {
                Console.WriteLine("Cannot access Clipper Polygon");
                return;
            }

            MartinezClipper martinezClipper = new MartinezClipper();
            List<Polygon> result  = martinezClipper.Compute(subject, clipping, cliptype);
            WriteToFile(args[argID++], result);
        }
    }
    static Polygon GetPolygonFromFile(string path)
    {
        string[] splitDatasetA = File.ReadAllLines(path);
        var end = splitDatasetA.Length;
        var polygon = new Polygon(end);
        int componentStart = 0;
        polygon.AddComponent();
        for (int i = 0; i < end; i++)
        {
            bool startNewComponent = splitDatasetA[i].Contains(";");
            var line = splitDatasetA[i].Split(' ');
            if (line.Length == 2)
            {
                line[1] = line[1].Trim('\r');
                line[1] = line[1].Trim(new char[] { ',', ';' });

                polygon.nodes.Add(new double2(double.Parse(line[0]), double.Parse(line[1])));
                if (startNewComponent)
                {
                    int componentEnd = polygon.nodes.Count;
                    if (!Helper.Equals(polygon.nodes[componentStart], polygon.nodes[componentEnd - 1]))
                        polygon.nodes.Add(polygon.nodes[componentStart]);
                    componentStart=componentEnd;
                    polygon.AddComponent();
                }
            }
        }
        return polygon;
    }
    static public void WriteToFile(string path, List<Polygon> polygons)
    {
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, false);
        foreach (Polygon polygon in polygons)
        {
            for (int i = 0, length = polygon.startIDs.Count - 1; i < length; i++)
            {
                int start = polygon.startIDs[i];
                int end = polygon.startIDs[i + 1];
                for (int j = start; j < end - 1; j++)
                {
                    writer.WriteLine(polygon.nodes[j].x + " " + polygon.nodes[j].y + ",");
                }
                writer.WriteLine(polygon.nodes[end - 1].x + " " + polygon.nodes[end - 1].y + ";");
                writer.WriteLine();
            }
        }
        writer.Close();
    }
}