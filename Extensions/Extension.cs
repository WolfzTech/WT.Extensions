using System;
using System.Collections.Generic;
using System.Linq;

namespace Autodesk.Revit.DB
{
    public static class GeometryElementExtensions
    {
        public static (List<Line> GetLines, List<Solid> GetSolids) GetGeomertyObjects(this GeometryElement geometryElement)
        {
            List<Line> GetLines = new List<Line>();
            List<Solid> GetSolids = new List<Solid>();
            foreach (GeometryObject geometryObject in geometryElement)
            {
                if (geometryObject is Solid)
                {
                    GetSolids.Add(geometryObject as Solid);
                }
                else if (geometryObject is Line)
                {
                    GetLines.Add(geometryObject as Line);
                }
                else if (geometryObject is GeometryInstance)
                {
                    GetLines.AddRange((geometryObject as GeometryInstance).GetInstanceGeometry().GetGeomertyObjects().GetLines);
                    GetSolids.AddRange((geometryObject as GeometryInstance).GetInstanceGeometry().GetGeomertyObjects().GetSolids);
                }
            }
            return (GetLines, GetSolids);
        }
    }
    
    public static class PlaneExtensions
    {
        public static bool IsInPlane(this Plane plane, XYZ point)
        {
            bool IsInPlane = false;
            XYZ normal = plane.Normal;
            XYZ origin = plane.Origin;
            double x1 = normal.X;
            double y1 = normal.Y;
            double z1 = normal.Z;
            double x2 = origin.X;
            double y2 = origin.Y;
            double z2 = origin.Z;
            double x3 = point.X;
            double y3 = point.Y;
            double z3 = point.Z;
            if (Math.Round(x1 * (x3 - x2) + y1 * (y3 - y2) + z1 * (z3 - z2), 3, MidpointRounding.AwayFromZero) == 0.0)
                IsInPlane = true;
            return IsInPlane;
        }

        public static XYZ GetIntersection(this Plane plane, XYZ point)
        {
            XYZ normal = plane.Normal;
            return plane.GetIntersection(point, normal);
        }

        public static XYZ GetIntersection(this Plane plane, XYZ point, XYZ vector)
        {
            XYZ xyz = null;
            XYZ origin = plane.Origin;
            XYZ normal = plane.Normal;
            bool isInPlane = plane.IsInPlane(point);
            bool isVectorParalellWithNormal = false;
            if (Math.Round(vector.AngleTo(normal), 3, MidpointRounding.AwayFromZero) == Math.Round(Math.PI / 4.0, 3, MidpointRounding.AwayFromZero))
                isVectorParalellWithNormal = true;
            if (isInPlane)
            {
                XYZExtensions.Result = "LineInView";
                return point;
            }
            if (isVectorParalellWithNormal && !isInPlane)
            {
                XYZExtensions.Result = "LineParalleView";
                return null;
            }
            if (!isVectorParalellWithNormal && !isInPlane)
            {
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
                xyz = new XYZ(x1 + x4 * num, y1 + y4 * num, z1 + z4 * num);
            }
            return xyz;
        }
    }

}

