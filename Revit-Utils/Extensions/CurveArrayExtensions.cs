using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace WT.Revit.Extensions
{
    public static class CurveArrayExtensions
    {
        public static List<Curve> ToList(this CurveArray curveArray)
        {
            List<Curve> curves = new List<Curve>();
            foreach (Curve curve in curveArray)
            {
                curves.Add(curve);
            }
            return curves;
        }

        public static CurveArray ToArray(this List<Curve> curves)
        {
            CurveArray curveArray = new CurveArray();
            foreach (Curve curve in curves)
            {
                curveArray.Append(curve);
            }
            return curveArray;
        }
    }
}
