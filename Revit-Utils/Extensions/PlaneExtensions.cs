using System;

namespace Autodesk.Revit.DB
{
    public static class PlaneExtensions
    {
        public static bool IsOnPlane(this Plane plane, XYZ point)
        {
            return Math.Abs(plane.Normal.DotProduct(point - plane.Origin)) < 1e-9;
        }

        public static XYZ GetIntersection(this Plane plane, XYZ point)
        {
            XYZ normal = plane.Normal;
            return plane.GetIntersection(point, normal);
        }

        public static XYZ GetIntersection(this Plane plane, XYZ point, XYZ vector)
        {
            XYZ origin = plane.Origin;
            XYZ normal = plane.Normal;

            if (plane.IsOnPlane(point)) // The point is already in the plane
            {
                return point;
            }

            // Check if the vector is parallel to the plane (orthogonal to the normal)
            bool isVectorParallelWithPlane = Math.Abs(vector.AngleTo(normal) - Math.PI / 2) < 1e-6;

            if (isVectorParallelWithPlane) // The vector is parallel to the plane, no intersection
            {
                return null;
            }

            if (vector.IsAlmostEqualTo(-normal)) // The vector is opposite to the normal
            {
                vector = -vector;// Reverse the vector
            }

            double x1 = point.X;
            double y1 = point.Y;
            double z1 = point.Z;
            double x2 = normal.X;
            double y2 = normal.Y;
            double z2 = normal.Z;
            double x3 = origin.X;
            double y3 = origin.Y;
            double z3 = origin.Z;
            double x4 = vector.X;
            double y4 = vector.Y;
            double z4 = vector.Z;
            double num = (x2 * (x3 - x1) + y2 * (y3 - y1) + z2 * (z3 - z1)) / (x2 * x4 + y2 * y4 + z2 * z4);
            var xyz = new XYZ(x1 + x4 * num, y1 + y4 * num, z1 + z4 * num);

            return xyz;
        }
    }
}
