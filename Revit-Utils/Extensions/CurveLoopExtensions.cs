using System.Linq;

namespace Autodesk.Revit.DB
{
    public static class CurveLoopExtensions
    {
        public static CurveLoop Purge(this CurveLoop curveLoop)//still need to check for arcs, elipses, etc.
        {
            var result = new CurveLoop();

            for (int i = 0; i < curveLoop.NumberOfCurves(); i++)
            {
                var curve = curveLoop.ElementAt(i);
                var nextCurve = curveLoop.ElementAt((i + 1) % curveLoop.NumberOfCurves());
                var previouseCurve = curveLoop.ElementAt((i - 1 + curveLoop.NumberOfCurves()) % curveLoop.NumberOfCurves());

                if (curve is Line line && nextCurve is Line nextLine)
                {
                    if (line.Direction.Normalize().IsAlmostEqualTo(nextLine.Direction.Normalize()))
                    {
                        if (i == curveLoop.Count() - 1)//if it is the last line, we should remove the first line from the result
                        {
                            var newLoopWithoutFirstLine = new CurveLoop();
                            for (int j = 1; j < result.Count(); j++)
                            {
                                newLoopWithoutFirstLine.Append(result.ElementAt(j));
                            }

                            result = newLoopWithoutFirstLine;
                        }
                        result.Append(Line.CreateBound(curve.GetEndPoint(0), nextCurve.GetEndPoint(1)));
                        i++;
                    }
                    else
                    {
                        result.Append(curve);
                    }
                }
                else
                {
                    result.Append(curve);
                }
            }

            if (result.NumberOfCurves() == curveLoop.NumberOfCurves())
            {
                return result;
            }
            else
            {
                return Purge(result);
            }
        }

        public static bool IsInside(this CurveLoop curveLoop, CurveLoop targetCurveLoop)
        {
            return curveLoop.All(curve => curve.IsInside(targetCurveLoop));
        }
    }
}
