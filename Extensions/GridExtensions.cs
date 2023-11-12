using System;
using System.Collections.Generic;
using System.Linq;

namespace Autodesk.Revit.DB
{
    public static class GridExtensions
    {
        public static List<Line> GetLines(this Grid grid, View view)
        {
            List<Line> gridLines = new List<Line>();
            Options options = new Options
            {
                View = view
            };
            GeometryElement geometryRlement = grid.get_Geometry(options);
            if(geometryRlement == null)
            {
                return null;
            }
            List<GeometryObject> geometryobjects = geometryRlement.GetEnumerator().ToList();
           
            foreach (GeometryObject geometryObject in geometryobjects)
            {
                if(geometryObject is Line)
                {
                    gridLines.Add( geometryObject as Line);
                }
            }
            return gridLines;
        }

        public static bool IsHorizontal(this Grid grid, double tolerance = 1.0e-09)
        {
            if (!grid.IsCurved)
            {
                var line = grid.Curve as Line;
                if (Math.Abs(Math.Abs(line.Direction.Normalize().DotProduct(XYZ.BasisX)) - 1) < tolerance)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsVertical(this Grid grid,double tolerance= 1.0e-09)
        {
            if (!grid.IsCurved)
            {
                var line = grid.Curve as Line;
                if (Math.Abs(Math.Abs(line.Direction.Normalize().DotProduct(XYZ.BasisY)) - 1) < tolerance)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
