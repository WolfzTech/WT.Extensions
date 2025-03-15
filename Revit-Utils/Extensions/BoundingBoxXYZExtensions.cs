using System;
using System.Collections.Generic;
using System.Linq;

namespace Autodesk.Revit.DB
{
    public static class BoundingBoxXYZExtensions
    {
        public static double Length(this BoundingBoxXYZ outline)
        {
            return outline.Max.X - outline.Min.X;
        }

        public static double Width(this BoundingBoxXYZ outline)
        {
            return outline.Max.Y - outline.Min.Y;
        }

        public static double Height(this BoundingBoxXYZ outline)
        {
            return outline.Max.Z - outline.Min.Z;
        }

        public static Outline ToOutline(this BoundingBoxXYZ boundingBox)
        {
            return new Outline(boundingBox.Min, boundingBox.Max);
        }

        public static BoundingBoxXYZ Add(this BoundingBoxXYZ left, BoundingBoxXYZ right)
        {
            return new BoundingBoxXYZ
            {
                Min = new XYZ(Math.Min(left.Min.X, right.Min.X), Math.Min(left.Min.Y, right.Min.Y), Math.Min(left.Min.Z, right.Min.Z)),
                Max = new XYZ(Math.Max(left.Max.X, right.Max.X), Math.Max(left.Max.Y, right.Max.Y), Math.Max(left.Max.Z, right.Max.Z))
            };
        }

        public static BoundingBoxXYZ Subtract(this BoundingBoxXYZ left, BoundingBoxXYZ right)
        {
            return new BoundingBoxXYZ
            {
                Min = new XYZ(Math.Max(left.Min.X, right.Min.X), Math.Max(left.Min.Y, right.Min.Y), Math.Max(left.Min.Z, right.Min.Z)),
                Max = new XYZ(Math.Min(left.Max.X, right.Max.X), Math.Min(left.Max.Y, right.Max.Y), Math.Min(left.Max.Z, right.Max.Z))
            };
        }

        public static XYZ Center(this BoundingBoxXYZ boundingBox)
        {
            return new XYZ((boundingBox.Min.X + boundingBox.Max.X) / 2, (boundingBox.Min.Y + boundingBox.Max.Y) / 2, (boundingBox.Min.Z + boundingBox.Max.Z) / 2);
        }

        public static BoundingBoxXYZ Create(IList<XYZ> points)
        {
            var minX = points.Min(p => p.X);
            var minY = points.Min(p => p.Y);
            var minZ = points.Min(p => p.Z);
            var maxX = points.Max(p => p.X);
            var maxY = points.Max(p => p.Y);
            var maxZ = points.Max(p => p.Z);

            return new BoundingBoxXYZ
            {
                Min = new XYZ(minX, minY, minZ),
                Max = new XYZ(maxX, maxY, maxZ)
            };
        }
    }
}
