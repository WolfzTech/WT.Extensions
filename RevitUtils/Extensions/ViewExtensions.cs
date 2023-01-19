using System.Linq;

namespace Autodesk.Revit.DB
{
    public static class ViewExtensions
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
