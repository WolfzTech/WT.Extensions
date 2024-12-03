using Autodesk.Revit.DB;
using System;
using WT.Sys.Extensions;

namespace WT.Revit.Extensions
{
    public class ArcExtensions
    {
        public static Arc Create(XYZ centerPoint, XYZ startPoint, XYZ endPoint, bool isClockwise)
        {
            if (!centerPoint.DistanceTo(startPoint).IsAlmostEqualTo(centerPoint.DistanceTo(endPoint)))
            {
                throw new ArgumentException("The start and end points must be equidistant from the center point.");
            }

            // Calculate the normal vector to the plane defined by the three points
            //XYZ normal = (startPoint - centerPoint).CrossProduct(endPoint - centerPoint).Normalize();
            XYZ normal = RevitApp.ActiveView.ViewDirection;

            // Calculate the angle between the start and end points
            double angle = (endPoint - centerPoint).AngleOnPlaneTo(startPoint - centerPoint, normal);

            // If the arc is counterclockwise, negate the angle
            if (!isClockwise)
            {
                angle = 2 * Math.PI - angle;
            }
            //MessageBox.Show((angle * 180 / Math.PI).ToString());
            // Calculate the radius
            double radius = centerPoint.DistanceTo(startPoint);

            // Calculate the x and y axes
            XYZ xAxis = (startPoint - centerPoint).Normalize();
            XYZ yAxis = normal.CrossProduct(xAxis);

            // Create the arc
            if (!isClockwise)
            {
                return Arc.Create(centerPoint, radius, 0, angle, xAxis, yAxis);
            }
            return Arc.Create(centerPoint, radius, 2 * Math.PI - angle, 2 * Math.PI, xAxis, yAxis);
        }
    }
}
