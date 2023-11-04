namespace Autodesk.Revit.DB
{
    public static class CurveExtensions
    {
        public static Curve ProjectToPlane(this Curve curve, Plane plane)
        {
            var startPoint = curve.GetEndPoint(0).ProjectToPlane(plane);
            var endPoint = curve.GetEndPoint(1).ProjectToPlane(plane);
            return Line.CreateBound(startPoint, endPoint);
        }
    }
}
