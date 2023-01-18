using Autodesk.Revit.DB;

namespace Autodesk.Revit.DB
{
    public static class CurveExtensions
    {
        public static Curve ProjectToPlane(this Curve curve, Plane plane)
        {
            XYZ startPoint = curve.GetEndPoint(0).ProjectToPlane(plane);
            XYZ endPoint = curve.GetEndPoint(1).ProjectToPlane(plane);
            return Line.CreateBound(startPoint, endPoint);
        }
    }
}
