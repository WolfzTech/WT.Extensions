using System.Collections.Generic;

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

        public static Line Project(this Line line, Line projectLine)
        {
            Line projectLineUb = projectLine.CreateUnbound();
            var p1 = line.GetEndPoint(0).Project(projectLineUb);
            var p2 = line.GetEndPoint(1).Project(projectLineUb);
            return Line.CreateBound(p1, p2);
        }

        public static Line Project(this Line line, Plane plane)
        {
            if (line.IsBound)
            {
                var p1 = line.GetEndPoint(0).Project(plane);
                var p2 = line.GetEndPoint(1).Project(plane);
                return Line.CreateBound(p1, p2);
            }
            else
            {
                var origin = line.Origin.Project(plane);
                var direction = line.Direction.Project(plane);
                return Line.CreateUnbound(origin, direction);
            }
        }

        public static Line Project(this Line line, SketchPlane sketchPlane)
        {
            return line.Project(sketchPlane.GetPlane());
        }

        public static bool IsAlmostEqualTo(this Line line, Line other, double tolerance = 1e-9)
        {
            return (line.GetEndPoint(0).IsAlmostEqualTo(other.GetEndPoint(0), tolerance) &&
                   line.GetEndPoint(1).IsAlmostEqualTo(other.GetEndPoint(1), tolerance))
                   || (line.GetEndPoint(0).IsAlmostEqualTo(other.GetEndPoint(1), tolerance) &&
                   line.GetEndPoint(1).IsAlmostEqualTo(other.GetEndPoint(0), tolerance));
        }

        public static bool IsParallelTo(this Line line, Line other, double tolerance = 1e-9)
        {
            return line.Direction.Normalize().IsAlmostEqualTo(other.Direction.Normalize(), tolerance)
                || line.Direction.Normalize().IsAlmostEqualTo(-other.Direction.Normalize(), tolerance);
        }

        public static bool IsAlmostConnectTo(this Line line, Line other, double tolerance = 1e-9)
        {
            return (line.GetEndPoint(0).IsAlmostEqualTo(other.GetEndPoint(0), tolerance) &&
               !line.GetEndPoint(1).IsAlmostEqualTo(other.GetEndPoint(1), tolerance))
               || (line.GetEndPoint(0).IsAlmostEqualTo(other.GetEndPoint(1), tolerance) &&
               !line.GetEndPoint(1).IsAlmostEqualTo(other.GetEndPoint(0), tolerance))
               || (line.GetEndPoint(1).IsAlmostEqualTo(other.GetEndPoint(1), tolerance) &&
               !line.GetEndPoint(0).IsAlmostEqualTo(other.GetEndPoint(0), tolerance))
               || (line.GetEndPoint(1).IsAlmostEqualTo(other.GetEndPoint(0), tolerance) &&
               !line.GetEndPoint(0).IsAlmostEqualTo(other.GetEndPoint(1), tolerance));
        }

        public static Line Combine(this Line line, Line other, double tolerance = 1e-9)
        {
            if (!line.IsAlmostConnectTo(other, tolerance))
                throw new System.ArgumentException("The two lines are not connected.");
            if (!line.IsParallelTo(other, tolerance))
                throw new System.ArgumentException("The two lines are not aligned.");
            if (line.GetEndPoint(0).IsAlmostEqualTo(other.GetEndPoint(0), tolerance))
                return Line.CreateBound(line.GetEndPoint(1), other.GetEndPoint(1));
            if (line.GetEndPoint(0).IsAlmostEqualTo(other.GetEndPoint(1), tolerance))
                return Line.CreateBound(line.GetEndPoint(1), other.GetEndPoint(0));
            if (line.GetEndPoint(1).IsAlmostEqualTo(other.GetEndPoint(0), tolerance))
                return Line.CreateBound(line.GetEndPoint(0), other.GetEndPoint(1));
            if (line.GetEndPoint(1).IsAlmostEqualTo(other.GetEndPoint(1), tolerance))
                return Line.CreateBound(line.GetEndPoint(0), other.GetEndPoint(0));
            return null;
        }

        public static Line ExtendTo(this Line line, Line destinationLine)
        {
            var unboundLine = line.CreateUnbound();
            var unboundDestinationLine = destinationLine.CreateUnbound();

#if R26_OR_GREATER
            var intersection = unboundLine.Intersect(unboundDestinationLine, CurveIntersectResultOption.Detailed );
            if (intersection.Result == SetComparisonResult.Overlap)
            {
                foreach (CurveOverlapPoint result in intersection.GetOverlaps() )
                {
                    var resultPoint = result.Point;
#else
  var intersection = unboundLine.Intersect(unboundDestinationLine, out IntersectionResultArray resultArray);
     if (intersection == SetComparisonResult.Overlap)
            {
                foreach (IntersectionResult result in resultArray)
                {
                   var resultPoint = result.XYZPoint;
#endif
                    if (resultPoint.IsAlmostEqualTo(line.GetEndPoint(0)) || resultPoint.IsAlmostEqualTo(line.GetEndPoint(1)))
                        return line;
                    if (resultPoint.IsBetween(line.GetEndPoint(0), line.GetEndPoint(1)))
                        return line;
                    if (resultPoint.DistanceTo(line.GetEndPoint(0)) < resultPoint.DistanceTo(line.GetEndPoint(1)))
                        return Line.CreateBound(resultPoint, line.GetEndPoint(1));
                    else
                        return Line.CreateBound(line.GetEndPoint(0), resultPoint);
                }
            }
            return null;
        }

        public static bool IsHorizontal(this Line line, double tolerance = 1e-9)
        {
            return line.Direction.Normalize().IsAlmostEqualTo(XYZ.BasisX, tolerance)
                || line.Direction.Normalize().IsAlmostEqualTo(-XYZ.BasisX, tolerance);
        }

        public static bool IsVertical(this Line line, double tolerance = 1e-9)
        {
            return line.Direction.Normalize().IsAlmostEqualTo(XYZ.BasisY, tolerance)
                || line.Direction.Normalize().IsAlmostEqualTo(-XYZ.BasisY, tolerance);
        }

        public class LineComparer : IEqualityComparer<Line>
        {
            public bool Equals(Line x, Line y)
            {
                return x.IsAlmostEqualTo(y);
            }

            public int GetHashCode(Line obj)
            {
                return 0;
            }
        }

        public static XYZ MidPoint(this Line line)
        {
            return new XYZ((line.GetEndPoint(0).X + line.GetEndPoint(1).X) / 2,
                                          (line.GetEndPoint(0).Y + line.GetEndPoint(1).Y) / 2,
                                                                    (line.GetEndPoint(0).Z + line.GetEndPoint(1).Z) / 2);
        }
    }
}
