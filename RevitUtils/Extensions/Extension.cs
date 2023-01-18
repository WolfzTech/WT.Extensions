using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI.Selection;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using SysColor = System.Drawing.Color;

namespace Autodesk.Revit.DB
{
    public static class ElementTypeExtension
    {
        public static bool SetColor(this ElementType elementType, int red, int green, int blue)
        {
            Parameter colorParam = elementType.get_Parameter(BuiltInParameter.LINE_COLOR);
            if (colorParam != null)
            {
                SysColor sysColor = SysColor.FromArgb(red, green, blue);
                int intColor = GetLineColorFromSystemColor(sysColor);
                return colorParam.Set(intColor);
            }
            return false;
        }
        private static int GetLineColorFromSystemColor(SysColor color)
        {
            return (int)color.R + (int)color.G * (int)Math.Pow(2, 8) + (int)color.B * (int)Math.Pow(2, 16);
        }
    }
    public static class FaceExtension
    {
        //public static FaceIntersectionFaceResult _Intersect(this Face face, Plane plane, out Curve result)
        //{
        //    List<XYZ> projectedPoints = new List<XYZ>();
        //    EdgeArrayArray edgeArrayArray = face.EdgeLoops;
        //    foreach (EdgeArray edgeArray in edgeArrayArray)
        //    {
        //        foreach (Edge edge in edgeArray)
        //        {
        //            projectedPoints.Add(edge.AsCurve().GetEndPoint(0));
        //            projectedPoints.Add(edge.AsCurve().GetEndPoint(1));
        //        }
        //    }
        //    List<UV> projectedUVs = new List<UV>();
        //    foreach (XYZ point in projectedPoints)
        //    {
        //        UV projectedUV;
        //        double dist;
        //        plane.Project(point, out projectedUV, out dist);
        //        projectedUVs.Add(projectedUV);
        //    }
        //    double offset = 100;
        //    double MaxU = projectedPoints.Select(x => x.X).Max() + offset._CmToFeet();
        //    double MaxV = projectedPoints.Select(x => x.Y).Max() - offset._CmToFeet();
        //    double MinU = projectedPoints.Select(x => x.X).Min() - offset._CmToFeet();
        //    double MinV = projectedPoints.Select(x => x.Y).Min() - offset._CmToFeet();

        //}
    }
    public static class ReferenceExtension
    {
        public static List<Element> GetLinkedElements(this IList<Reference> references)
        {
            var linkedElements = new List<Element>();
            foreach (Reference reference in references)
            {
                RevitLinkInstance rlin = reference.ElementId.Element<RevitLinkInstance>();
                var linkedDoc = rlin.GetLinkDocument();
                Element linkedElement = reference.LinkedElementId.Element(linkedDoc);
                linkedElements.Add(linkedElement);
            }
            return linkedElements;
        }
        public static Element GetLinkedElement(this Reference reference, out Transform transform)
        {
            RevitLinkInstance rlin = reference.ElementId.Element<RevitLinkInstance>();
            transform = rlin.GetTransform();
            Document linkedDoc = rlin.GetLinkDocument();
            return reference.LinkedElementId.Element(linkedDoc);
        }
        public static Element GetLinkedElement(this Reference reference)
        {
            RevitLinkInstance rlin = reference.ElementId.Element<RevitLinkInstance>();
            Document linkedDoc = rlin.GetLinkDocument();
            return reference.LinkedElementId.Element(linkedDoc);
        }
    }
    public static class GeometryElementExtension
    {
        public static (List<Line> GetLines, List<Solid> GetSolids) GetGeomertyObjects(this GeometryElement geometryElement)
        {
            List<Line> GetLines = new List<Line>();
            List<Solid> GetSolids = new List<Solid>();
            foreach (GeometryObject geometryObject in geometryElement)
            {
                if (geometryObject is Solid)
                {
                    GetSolids.Add(geometryObject as Solid);
                }
                else if (geometryObject is Line)
                {
                    GetLines.Add(geometryObject as Line);
                }
                else if (geometryObject is GeometryInstance)
                {
                    GetLines.AddRange((geometryObject as GeometryInstance).GetInstanceGeometry().GetGeomertyObjects().GetLines);
                    GetSolids.AddRange((geometryObject as GeometryInstance).GetInstanceGeometry().GetGeomertyObjects().GetSolids);
                }
            }
            return (GetLines, GetSolids);
        }
    }
    public static class GridExtension
    {
        public static Line GetLine(this Grid grid, View view)
        {
            Options options = new Options
            {
                View = view
            };
            GeometryElement geometryelement = grid.get_Geometry(options);
            List<GeometryObject> geometryobjects = geometryelement.GetEnumerator().ToList();
            Line gridLine = null;
            foreach (GeometryObject geometryObject in geometryobjects)
            {
                gridLine = geometryObject as Line;
            }
            return gridLine;
        }
    }
    public static class LineExtension
    {
        public static Line CreateUnbound(this Line line)
        {
            if (!line.IsBound)
                return line;
            return Line.CreateUnbound(line.GetEndPoint(0), line.Direction);
        }
        public static Line ProjectToLine(this Line line, Line projectLine)
        {
            Line projectLineUb = projectLine.CreateUnbound();
            XYZ p1 = line.GetEndPoint(0).ProjectToLine(projectLineUb);
            XYZ p2 = line.GetEndPoint(1).ProjectToLine(projectLineUb);
            return Line.CreateBound(p1, p2);
        }
    }
    public static class RebarExtension
    {
        public static List<DirectShape> GetDirecShapeHosts(this List<Rebar> rebars, Document doc)
        {
            List<DirectShape> directShapes = new List<DirectShape>();
            foreach (Rebar rebar in rebars)
            {
                if (rebar.GetDirecShapeHost(doc) != null)
                {
                    directShapes.Add(rebar.GetDirecShapeHost(doc));
                }
            }
            return directShapes;
        }
        public static DirectShape GetDirecShapeHost(this Rebar rebar, Document doc)
        {
            Element host = rebar.GetHostId().Element(doc);
            if (host is DirectShape && string.IsNullOrEmpty(host.Name))
            {
                return host as DirectShape;
            }
            else return null;
        }
        public static RebarStyle GetRebarStyle(this Rebar rebar)
        {
            return (RebarStyle)rebar.get_Parameter(BuiltInParameter.REBAR_ELEM_HOOK_STYLE).AsInteger();
        }
    }
    public static class ParameterExtension
    {
        public static string StringValue(this Parameter parameter)
        {
            string stringValue = parameter.AsValueString();
            if (string.IsNullOrEmpty(stringValue)) stringValue = parameter.AsString();
            if (string.IsNullOrEmpty(stringValue)) stringValue = parameter.AsInteger().ToString();
            if (string.IsNullOrEmpty(stringValue) && parameter.AsInteger() == 0) stringValue = parameter.AsDouble().ToString();
            return stringValue;
        }
    }
    public static class PlaneExtension
    {
        public static bool IsInPlane(this Plane plane, XYZ point)
        {
            bool IsInPlane = false;
            XYZ normal = plane.Normal;
            XYZ origin = plane.Origin;
            double x1 = normal.X;
            double y1 = normal.Y;
            double z1 = normal.Z;
            double x2 = origin.X;
            double y2 = origin.Y;
            double z2 = origin.Z;
            double x3 = point.X;
            double y3 = point.Y;
            double z3 = point.Z;
            if (Math.Round(x1 * (x3 - x2) + y1 * (y3 - y2) + z1 * (z3 - z2), 3, MidpointRounding.AwayFromZero) == 0.0)
                IsInPlane = true;
            return IsInPlane;
        }
        
        public static XYZ GetIntersection(this Plane plane, XYZ point)
        {
            XYZ normal = plane.Normal;
            return plane.GetIntersection(point, normal);
        }
        
        public static XYZ GetIntersection(this Plane plane, XYZ point, XYZ vector)
        {
            XYZ xyz = null;
            XYZ origin = plane.Origin;
            XYZ normal = plane.Normal;
            bool isInPlane = plane.IsInPlane(point);
            bool isVectorParalellWithNormal = false;
            if (Math.Round(vector.AngleTo(normal), 3, MidpointRounding.AwayFromZero) == Math.Round(Math.PI / 4.0, 3, MidpointRounding.AwayFromZero))
                isVectorParalellWithNormal = true;
            if (isInPlane)
            {
                XYZExtension.Result = "LineInView";
                return point;
            }
            if (isVectorParalellWithNormal && !isInPlane)
            {
                XYZExtension.Result = "LineParalleView";
                return null;
            }
            if (!isVectorParalellWithNormal && !isInPlane)
            {
                double x1 = point.X;
                double y1 = point.Y;
                double z1 = point.Z;
                double x2 = normal.X;
                double y2 = normal.Y;
                double z2 = normal.Z;
                double x3 = origin.X;
                double y3 = origin.Y;
                double z3 = origin.Z;
                double x4 = vector.X;
                double y4 = vector.Y;
                double z4 = vector.Z;
                double num = (x2 * (x3 - x1) + y2 * (y3 - y1) + z2 * (z3 - z1)) / (x2 * x4 + y2 * y4 + z2 * z4);
                xyz = new XYZ(x1 + x4 * num, y1 + y4 * num, z1 + z4 * num);
            }
            return xyz;
        }
    }
    public static class SelectionExtension
    {
        public static List<Element> GetElements(this Selection sel)
        {
            return Enumerable.ToList(sel.GetElementIds().Select(x => x.Element()));
        }

        public static Element GetElement(this Selection sel)
        {
            return sel.GetElementIds().Select(x => x.Element()).FirstOrDefault();
        }
    }
    public static class ViewScheduleExtension
    {
        public static List<Element> GetElements(this ViewSchedule viewSchedule)
        {
            List<Element> elements;
            Document doc = viewSchedule.Document;
            elements = Enumerable.ToList(new FilteredElementCollector(doc, viewSchedule.Id).WhereElementIsNotElementType());

            ScheduleDefinition scheduleDefinition = viewSchedule.Definition;
            IList<ScheduleFilter> scheduleFilters = scheduleDefinition.GetFilters();
            foreach (ScheduleFilter scheduleFilter in scheduleFilters)
            {
                ScheduleFilterType scheduleFilterType = scheduleFilter.FilterType;
                ScheduleFieldId scheduleFieldId = scheduleFilter.FieldId;
                ScheduleField scheduleField = scheduleDefinition.GetField(scheduleFieldId);
                string value = "";
                try
                {
                    value = scheduleFilter.GetStringValue();
                }
                catch (Exception) { }
                switch (scheduleFilterType)
                {
                    case ScheduleFilterType.Equal:
                        elements = Enumerable.ToList(elements.Where(x => x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().Equals(value)));
                        break;
                    case ScheduleFilterType.NotEqual:
                        elements = Enumerable.ToList(elements.Where(x => !x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().Equals(value)));
                        break;
                    case ScheduleFilterType.Contains:
                        elements = Enumerable.ToList(elements.Where(x => x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().Contains(value)));
                        break;
                    case ScheduleFilterType.NotContains:
                        elements = Enumerable.ToList(elements.Where(x => !x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().Contains(value)));
                        break;
                    case ScheduleFilterType.BeginsWith:
                        elements = Enumerable.ToList(elements.Where(x => x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().StartsWith(value)));
                        break;
                    case ScheduleFilterType.NotBeginsWith:
                        elements = Enumerable.ToList(elements.Where(x => !x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().StartsWith(value)));
                        break;
                    case ScheduleFilterType.EndsWith:
                        elements = Enumerable.ToList(elements.Where(x => x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().EndsWith(value)));
                        break;
                    case ScheduleFilterType.NotEndsWith:
                        elements = Enumerable.ToList(elements.Where(x => !x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString().EndsWith(value)));
                        break;
#if !Revit2018&&!Revit2019&&!Revit2018C&&!Revit2019C
                    case ScheduleFilterType.HasValue:
                        elements = Enumerable.ToList(elements.Where(x => !string.IsNullOrEmpty(x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString())));
                        break;
                    case ScheduleFilterType.HasNoValue:
                        elements = Enumerable.ToList(elements.Where(x => string.IsNullOrEmpty(x.get_Parameter((BuiltInParameter)scheduleField.ParameterId.IntegerValue).AsString())));
                        break;
                    default:
                        MessageBox.Show("Can't get elements" + "\n" + scheduleField.GetName());
                        break;

#endif
                }
            }
            return elements;
        }
    }
    public static class ViewExtension
    {
        public static void SetCategoryHidden(this View view, BuiltInCategory builtInCategory, bool hide, Document doc)
        {
            Category category = doc.Settings.Categories.get_Item(builtInCategory);
            view.SetCategoryHidden(category.Id, hide);
        }
        public static void SetCategoryOverrides(this View view, BuiltInCategory categoryId, bool projectionFill, bool cutFill, Document doc)
        {
            OverrideGraphicSettings oGS = view.GetCategoryOverrides(doc.Settings.Categories.get_Item(categoryId).Id);
#if !Revit2018 && !Revit2018C
            oGS.SetSurfaceForegroundPatternVisible(projectionFill);
            oGS.SetCutForegroundPatternVisible(cutFill);
#endif
            view.SetCategoryOverrides(doc.Settings.Categories.get_Item(categoryId).Id, oGS);
        }
#if !Revit2018 && !Revit2018C
        public static void SetCategoryOverrides(this View view, BuiltInCategory categoryId, bool projectionFill, bool cutFill, Color color, Document doc)
        {
            OverrideGraphicSettings oGS = view.GetCategoryOverrides(doc.Settings.Categories.get_Item(categoryId).Id);
            oGS.SetSurfaceForegroundPatternVisible(projectionFill);
            oGS.SetCutForegroundPatternVisible(cutFill);
            FillPatternElement fillPatternElement = new FilteredElementCollector(doc).OfClass(typeof(FillPatternElement)).Cast<FillPatternElement>().First(x => x.Name == "<Solid fill>");
            if (projectionFill)
            {
                try
                {
                    oGS.SetSurfaceForegroundPatternColor(color);
                    oGS.SetSurfaceForegroundPatternId(fillPatternElement.Id);
                }
                catch { }
            }
            if (cutFill)
            {
                try
                {
                    oGS.SetCutForegroundPatternColor(color);
                    oGS.SetCutForegroundPatternId(fillPatternElement.Id);
                }
                catch { }
            }
            view.SetCategoryOverrides(doc.Settings.Categories.get_Item(categoryId).Id, oGS);
        }
#endif
        //
        // Summary:
        //     Sets graphic overrides for a category in view.
        //
        // Parameters:
        //   categoryId:
        //     Category to be overridden
        //
        //   overrideGraphicSettings:
        //     Object representing all graphic overrides of the category categoryId in view.
        //
        // Exceptions:
        //   T:Autodesk.Revit.Exceptions.ArgumentException:
        //     Category cannot be overridden. -or- Fill pattern must be a drafting pattern.
        //     -or- Fill pattern Id must be invalidElementId or point to a LinePattern element.
        //
        //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
        //     A non-optional argument was NULL
        //
        //   T:Autodesk.Revit.Exceptions.InvalidOperationException:
        //     The view type does not support Visibility/Graphics Overriddes.
        public static void SetCategoryOverrides(this View view, BuiltInCategory categoryId, int projectionLineWeight, int cutLineWeight, Color color, int transparency, Document doc)
        {
            OverrideGraphicSettings oGS = new OverrideGraphicSettings();
            oGS.SetProjectionLineWeight(projectionLineWeight);
            oGS.SetCutLineWeight(cutLineWeight);
            try
            {
                oGS.SetProjectionLineColor(color);
                oGS.SetCutLineColor(color);
            }
            catch { }
            oGS.SetSurfaceTransparency(transparency);
            view.SetCategoryOverrides(doc.Settings.Categories.get_Item(categoryId).Id, oGS);
            if (categoryId == BuiltInCategory.OST_Lines)
            {
                Category linesCategory = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);
                CategoryNameMap categories = linesCategory.SubCategories;
                foreach (Category category in categories)
                {
                    try
                    {
                        view.SetCategoryOverrides(category.Id, oGS);
                    }
                    catch { }
                }
            }
        }
        public static SketchPlane SetAsWorkPlane(this View view)
        {
            Document doc = view.Document;
            Plane plane = Plane.CreateByNormalAndOrigin(view.ViewDirection, view.Origin);
            SketchPlane sp = SketchPlane.Create(doc, plane);
            view.SketchPlane = sp;
            return sp;
        }
    }
}

    namespace System.Linq
{
    public static class ListExtension
        {
            public static ObservableCollection<T> ToObservableCollection<T>(this List<T> source)
            {
                ObservableCollection<T> obs = new ObservableCollection<T>();
                foreach (T member in source)
                {
                    obs.Add(member);
                }
                return obs;
            }
        }

    }

