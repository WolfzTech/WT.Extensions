using Autodesk.Revit.DB.Architecture;
using JetBrains.Annotations;
using System;
using PureAttribute = JetBrains.Annotations.PureAttribute;

namespace Autodesk.Revit.DB
{
    public static class ElementExtensions
    {
        public static string TypeName(this Element element)
        {
            return element.GetName().TypeName;
        }

        public static string FamilyName(this Element element)
        {
            return element.GetName().FamilyName;
        }

        private static (string FamilyName, string TypeName) GetName(this Element element)
        {
            string familyName;
            string typeName;
            switch (element)
            {
                case FamilySymbol familySymbol:
                    familyName = familySymbol.FamilyName;
                    typeName = familySymbol.Name;
                    break;
                case WallType wallType:
                    familyName = wallType.FamilyName;
                    typeName = wallType.Name;
                    break;
                case StairsLandingType stairsLandingType:
                    familyName = stairsLandingType.FamilyName;
                    typeName = stairsLandingType.Name;
                    break;
                case TextNoteType textNoteType:
                    familyName = textNoteType.FamilyName;
                    typeName = textNoteType.Name;
                    break;
                case DimensionType dimensionType:
                    familyName = dimensionType.FamilyName;
                    typeName = dimensionType.Name;
                    break;
                case DetailLine detailLine:
                    familyName = detailLine.Name;
                    GraphicsStyle graphicsStyle = detailLine.LineStyle as GraphicsStyle;
                    typeName = graphicsStyle.Name;
                    break;
                default:
                    var typeById = element.GetTypeId().Element();
                    if (typeById != null)
                    {
                        familyName = GetName(typeById).FamilyName;
                        typeName = GetName(typeById).TypeName;
                    }
                    else
                    {
                        familyName = "";
                        typeName = element.Name;
                    }
                    break;
            }
            return (familyName, typeName);
        }

        public static XYZ LocationPoint(this Element element)
        {
            var location = element.Location;
            if (location is LocationCurve)
            {
                LocationCurve locationCurve = element.Location as LocationCurve;
                return locationCurve.Curve.GetEndPoint(0);
            }
            else if (location is LocationPoint)
            {
                LocationPoint locationPoint = element.Location as LocationPoint;
                return locationPoint.Point;
            }
            else return new XYZ();
        }

        public static XYZ MidPoint(this Element element)
        {
            var location = element.Location;
            if (location is LocationCurve)
            {
                LocationCurve locationCurve = element.Location as LocationCurve;
                XYZ p0 = locationCurve.Curve.GetEndPoint(0);
                XYZ p1 = locationCurve.Curve.GetEndPoint(1);
                return new XYZ((p0.X + p1.X) / 2, (p0.Y + p1.Y) / 2, (p0.Z + p1.Z) / 2);
            }
            else if (location is LocationPoint)
            {
                LocationPoint locationPoint = element.Location as LocationPoint;
                return locationPoint.Point;
            }
            else return new XYZ();
        }

        public static bool IsHorizontal(this Element element, View view)
        {
            LocationCurve locationCurve = element.Location as LocationCurve;
            Curve curve = locationCurve.Curve;
            if (curve is Line)
            {
                Line line = curve as Line;
                line = line.ProjectToPlane(view.SketchPlane.GetPlane()) as Line;
                if (Math.Round(line.Direction.AngleTo(view.RightDirection).RadianToDegree(), 2) == 0.00
                    || Math.Round(line.Direction.AngleTo(view.RightDirection).RadianToDegree(), 2) == 180.00)
                { return true; }
            }
            return false;
        }

        public static bool IsVertical(this Element element, View view)
        {
            LocationCurve locationCurve = element.Location as LocationCurve;
            Curve curve = locationCurve.Curve;
            if (curve is Line)
            {
                Line line = curve as Line;
                line = line.ProjectToPlane(view.SketchPlane.GetPlane()) as Line;
                if (Math.Round(line.Direction.AngleTo(view.RightDirection).RadianToDegree(), 2) == 90.00
                   || Math.Round(line.Direction.AngleTo(view.RightDirection).RadianToDegree(), 2) == 270.00)
                { return true; }
            }
            return false;
        }

        [Pure]
        [CanBeNull]
        public static Parameter GetParameter([NotNull] this Element element, Definition definition)
        {
            Parameter parameter = element.get_Parameter(definition);
            if (parameter != null && parameter.HasValue)
            {
                return parameter;
            }

            ElementId typeId = element.GetTypeId();
            if (typeId == ElementId.InvalidElementId)
            {
                return parameter;
            }

            return element.Document.GetElement(typeId).get_Parameter(definition) ?? parameter;
        }

        [Pure]
        [CanBeNull]
        public static Parameter GetParameter([NotNull] this Element element, Guid guid)
        {
            Parameter parameter = element.get_Parameter(guid);
            if (parameter != null && parameter.HasValue)
            {
                return parameter;
            }

            ElementId typeId = element.GetTypeId();
            if (typeId == ElementId.InvalidElementId)
            {
                return parameter;
            }

            return element.Document.GetElement(typeId).get_Parameter(guid) ?? parameter;
        }
    }
}
