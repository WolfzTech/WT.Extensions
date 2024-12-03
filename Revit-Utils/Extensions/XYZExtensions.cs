using System;
using System.Collections.Generic;
using System.Linq;

namespace Autodesk.Revit.DB
{
    public static class XYZExtensions
    {
        public static XYZ Project(this XYZ point, XYZ p1, XYZ vector)
        {
            return Project(p1, Plane.CreateByNormalAndOrigin(vector, point));
        }

        public static XYZ Project(this XYZ point, Curve curve)
        {
            return curve.Project(point).XYZPoint;
        }

        public static double ProjectDistance(this XYZ point, Curve curve)
        {
            return curve.Project(point).Distance;
        }

        public static XYZ Project(this XYZ point, Plane plane)
        {
            return point.Project(plane, out _, out _);
        }

        public static XYZ Project(this XYZ point, SketchPlane sketchPlane)
        {
            return point.Project(sketchPlane.GetPlane());
        }

        public static double ProjectDistance(this XYZ point, Plane plane)
        {
            plane.Project(point, out _, out var distance);
            return distance;
        }

        public static XYZ Project(this XYZ point, Plane plane, out UV uv, out double distance)
        {
            plane.Project(point, out uv, out distance);
            return plane.Origin + uv.U * plane.XVec + uv.V * plane.YVec;
        }

        public static string Value(this XYZ xyz)
        {
            return "X: " + xyz.X.ToString() + " Y: " + xyz.Y.ToString() + " Z: " + xyz.Z.ToString();
        }

        public static XYZ MidPoint(XYZ xyz1, XYZ xyz2)
        {
            return new XYZ((xyz1.X + xyz2.X) / 2, (xyz1.Y + xyz2.Y) / 2, (xyz1.Z + xyz2.Z) / 2);
        }

        /// <summary>
        /// Determines whether a point is between two other points within a given tolerance.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <param name="tolerance">The tolerance for equality check.</param>
        /// <returns>True if the point is between the two other points within the tolerance, otherwise false.</returns>
        public static bool IsBetween(this XYZ point, XYZ point1, XYZ point2, bool isContainingEndpoints = true, double tolerance = 1e-9)
        {
            if (isContainingEndpoints)
            {
                if (point.IsAlmostEqualTo(point1, tolerance) || point.IsAlmostEqualTo(point2, tolerance))
                {
                    return true;
                }
            }
            else
            {
                if (point.IsAlmostEqualTo(point1, tolerance) || point.IsAlmostEqualTo(point2, tolerance))
                {
                    return false;
                }
            }

            return Math.Abs(point.DistanceTo(point1) + point.DistanceTo(point2) - point1.DistanceTo(point2)) < tolerance;
        }

        public static bool IsOn(this XYZ point, Curve curve)
        {
            if (point.ProjectDistance(curve) < 1e-9)
            {
                return true;
            }
            return false;
        }

        public static bool IsOn(this XYZ point, Plane plane)
        {
            return plane.IsOnPlane(point);
        }

        public static List<XYZ> Purge(this List<XYZ> points, double tolerance = 1e-9)
        {
            return points.Distinct(new XYZComparer()).ToList();
        }

        // Summary:
        //     Returns the angle between this vector and the specified vector.
        //
        // Parameters:
        //   source:
        //     The specified vector.
        //
        // Returns:
        //     The real number between 0 and 2PI equal to the angle between the two vectors in
        //     radians..
        //
        // Exceptions:
        //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
        //     Thrown when source is null.
        //
        // Remarks:
        //     The angle between the two vectors is measured in the plane spanned by them.
        public static double FullAngleTo(this XYZ vector, XYZ source)
        {
            double angle = vector.AngleTo(source);
            XYZ crossProduct = vector.CrossProduct(source);
            if (crossProduct.Z < 0)
            {
                angle = 2 * Math.PI - angle;
            }
            return angle;
        }

        public class XYZComparer(double tolerance = 1e-9) : IEqualityComparer<XYZ>
        {
            private readonly double _tolerance = tolerance;

            public bool Equals(XYZ x, XYZ y)
            {
                return x.IsAlmostEqualTo(y, _tolerance);
            }

            public int GetHashCode(XYZ obj)
            {
                return 0;
            }
        }

#if !Revit2018
        public static XYZ SurveyRelative(this XYZ xyz, Document doc)
        {
            BasePoint pBasePoint = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_ProjectBasePoint).OfClass(typeof(BasePoint)).Cast<BasePoint>().FirstOrDefault();
            XYZ project = pBasePoint.SharedPosition;
            double alpha = pBasePoint.get_Parameter(BuiltInParameter.BASEPOINT_ANGLETON_PARAM).AsDouble();
            alpha = Math.PI * 2 - alpha;
            XYZ eLoc = xyz;
            XYZ eTransLoc = new(eLoc.X * Math.Cos(alpha) - eLoc.Y * Math.Sin(alpha),
                 eLoc.X * Math.Sin(alpha) + eLoc.Y * Math.Cos(alpha)
                 , eLoc.Z);
            return eTransLoc.Add(project);
        }
#endif
    }
}
