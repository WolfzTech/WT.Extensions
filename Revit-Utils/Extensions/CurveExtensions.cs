﻿namespace Autodesk.Revit.DB
{
    public static class CurveExtensions
    {
        public static bool IsInside(this Curve curve, CurveLoop targetCurveLoop)
        {
            foreach (var targetCurve in targetCurveLoop)
            {
#if R26_OR_GREATER
                if (curve.Intersect(targetCurve, CurveIntersectResultOption.Detailed).Result == SetComparisonResult.Overlap)
#else
if (curve.Intersect(targetCurve) == SetComparisonResult.Overlap)
#endif
                {
                    return false;
                }

                if (!curve.GetEndPoint(0).IsInside(targetCurveLoop) || !curve.GetEndPoint(1).IsInside(targetCurveLoop))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
