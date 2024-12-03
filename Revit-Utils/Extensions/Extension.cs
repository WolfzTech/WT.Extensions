using System;
using System.Collections.Generic;

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
    
   

}

