using Autodesk.Revit.DB;
using System;

namespace WT.Revit.Extensions
{
    public static class EllipseExtensions
    {
        public static (XYZ Focus1, XYZ Focus2) Foci(this Ellipse ellipse)
        {
            // Get the ellipse parameters
            XYZ center = ellipse.Center;
            XYZ majorAxis = ellipse.XDirection.Normalize(); // Direction of major axis
            XYZ minorAxis = ellipse.YDirection.Normalize(); // Direction of minor axis
            double majorRadius = ellipse.RadiusX; // Semi-major axis
            double minorRadius = ellipse.RadiusY; // Semi-minor axis

            // Calculate the focal distance (c = sqrt(a^2 - b^2))
            double focalDistance = Math.Sqrt(Math.Pow(majorRadius, 2) - Math.Pow(minorRadius, 2));

            // Calculate the foci
            XYZ focus1 = center + focalDistance * majorAxis;
            XYZ focus2 = center - focalDistance * majorAxis;

            return (focus1, focus2);
        }
    }
}
