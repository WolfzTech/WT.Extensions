namespace Autodesk.Revit.DB
{
    public static class LineExtensions
    {
        public static Line CreateUnbound(this Line line)
        {
            if (!line.IsBound)
                return line;
            return Line.CreateUnbound(line.GetEndPoint(0), line.Direction);
        }
        
        public static Line ProjectToLine(this Line line, Line projectLine)
        {
            Line projectLineUb = projectLine.CreateUnbound();
            XYZ p1 = line.GetEndPoint(0).ProjectToLine(projectLineUb);
            XYZ p2 = line.GetEndPoint(1).ProjectToLine(projectLineUb);
            return Line.CreateBound(p1, p2);
        }
    }
}
