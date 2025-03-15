using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;

namespace WT.Revit.Utils
{
    public class GeometryCreationUtils
    {
        public static Solid CreateUnionExtrusionGeometry(IList<CurveLoop> profileLoops, XYZ extrusionDir, double extrusionDist)
        {
            Solid solid = null;
            List<List<CurveLoop>> groupCurveLoops = new List<List<CurveLoop>>();

            foreach (CurveLoop loop in profileLoops)
            {
                var containCurveLoop = groupCurveLoops.FirstOrDefault(x => x.Any(y => loop.IsInside(y)));

                if (containCurveLoop != null)
                {
                    containCurveLoop.Add(loop);
                }
                else
                {
                    groupCurveLoops.Add(new List<CurveLoop> { loop });
                }
            }

            foreach (var groupCurve in groupCurveLoops)
            {
                var extrusion = GeometryCreationUtilities.CreateExtrusionGeometry(groupCurve, extrusionDir, extrusionDist);
                solid = solid == null ? extrusion : BooleanOperationsUtils.ExecuteBooleanOperation(solid, extrusion, BooleanOperationsType.Union);
            }

            return solid;
        }
    }
}
