using Autodesk.Revit.DB.Architecture;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public enum CurvePosition
        {
            StartPoint,
            EndPoint,
            MidPoint
        }

        public static XYZ LocationPoint(this Element element, CurvePosition curvePosition = CurvePosition.StartPoint)
        {
            var location = element.Location;
            if (location is LocationCurve locationCurve)
            {
                return curvePosition switch
                {
                    CurvePosition.StartPoint => locationCurve.Curve.GetEndPoint(0),
                    CurvePosition.EndPoint => locationCurve.Curve.GetEndPoint(1),
                    CurvePosition.MidPoint => locationCurve.Curve.Evaluate(0.5, true),
                    _ => locationCurve.Curve.GetEndPoint(0),
                };
            }
            else if (location is LocationPoint locationPoint)
            {
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
                line = line.Project(view.SketchPlane.GetPlane()) as Line;
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
                line = line.Project(view.SketchPlane.GetPlane()) as Line;
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

        public class ElementEqualityComparer : IEqualityComparer<Element>
        {
            public bool Equals(Element x, Element y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return Equals(x.Id, y.Id) && Equals(x.Document, y.Document);
            }

            public int GetHashCode(Element obj)
            {
                unchecked
                {
                    if (ReferenceEquals(obj, null)) return 0;

                    var hashCode = (obj.Id != null ? obj.Id.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.Document != null ? obj.Document.GetHashCode() : 0);
                    return hashCode;
                }
            }
        }

        public static Element GetParentElement(this Element element)
        {
            if (element.GroupId != ElementId.InvalidElementId) return element.Document.GetElement(element.GroupId);
            if (element is FamilyInstance familyInstance && familyInstance.SuperComponent != null) return familyInstance.SuperComponent;
            if (element is InsulationLiningBase insulation) return element.Document.GetElement(insulation.HostElementId);
            return null;
        }

        public static IEnumerable<Element> GetMembers(this Group group)
        {
            if (group == null) return new List<Element>();

            var members = new List<Element>();
            foreach (var memberId in group.GetMemberIds())
            {
                var element = group.Document.GetElement(memberId);
                if (element is Group subGroup) members.AddRange(subGroup.GetMembers());
                else if (element != null) members.Add(element);
            }

            return members;
        }

        public static IEnumerable<Element> GetMembers(this AssemblyInstance instance)
        {
            IEnumerable<Element> members = instance.GetMemberIds().Select(instance.Document.GetElement);
            return members;
        }

        public static IEnumerable<Element> GetMembers(this FamilyInstance instance)
        {
            IEnumerable<Element> members = instance.GetSubComponentIds().Select(instance.Document.GetElement);
            return members;
        }
    }
}
