using Autodesk.Revit.DB;

namespace WT.Revit.Extensions
{
    public static class  OutlineExtensions
    {
        public static double Length(this Outline outline)
        {
            return outline.MaximumPoint.X - outline.MinimumPoint.X;
        }

        public static double Width(this Outline outline)
        {
            return outline.MaximumPoint.Y - outline.MinimumPoint.Y;
        }

        public static double Height(this Outline outline)
        {
            return outline.MaximumPoint.X - outline.MinimumPoint.X;
        }
    }
}
