using System;
using System.Linq;

namespace Autodesk.Revit.DB
{
    public static class XYZExtension
    {
        public static string Result = string.Empty;
        
        public static XYZ ProjectToLine(this XYZ point, XYZ p1, XYZ vector)
        {
            return ProjectToPlane(p1, Plane.CreateByNormalAndOrigin(vector, point));
        }
        
        public static XYZ ProjectToLine(this XYZ point, Line line)
        {
            return line.Project(point).XYZPoint;
        }
        
        public static XYZ ProjectToPlane(this XYZ point, Plane plane)
        {
            return point.ProjectToPlane(plane, out _, out _);
        }
        
        public static XYZ ProjectToPlane(this XYZ point, Plane plane, out UV uv, out double distance)
        {
            plane.Project(point, out uv, out distance);
            return plane.Origin + uv.U * plane.XVec + uv.V * plane.YVec;
        }
        
        public static string Value(this XYZ xyz)
        {
            return "X: " + xyz.X.ToString() + " Y: " + xyz.Y.ToString() + " Z: " + xyz.Z.ToString();
        }

        public static XYZ MidPoint( XYZ xyz1, XYZ xyz2)
        {
            return new XYZ((xyz1.X + xyz2.X) / 2, (xyz1.Y + xyz2.Y) / 2, (xyz1.Z + xyz2.Z) / 2);
        }


#if !Revit2018
        public static XYZ SurveyRelative(this XYZ xyz, Document doc)
        {
            BasePoint pBasePoint = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_ProjectBasePoint).OfClass(typeof(BasePoint)).Cast<BasePoint>().FirstOrDefault();
            XYZ project = pBasePoint.SharedPosition;
            double alpha = pBasePoint.get_Parameter(BuiltInParameter.BASEPOINT_ANGLETON_PARAM).AsDouble();
            alpha = Math.PI * 2 - alpha;
            XYZ eLoc = xyz;
            XYZ eTransLoc = new XYZ(eLoc.X * Math.Cos(alpha) - eLoc.Y * Math.Sin(alpha),
                 eLoc.X * Math.Sin(alpha) + eLoc.Y * Math.Cos(alpha)
                 , eLoc.Z);
            return eTransLoc.Add(project);
        }
#endif
    }
}
