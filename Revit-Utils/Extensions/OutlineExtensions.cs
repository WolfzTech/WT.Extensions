namespace Autodesk.Revit.DB
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

        public static XYZ Center(this Outline outline)
        {
            return new XYZ((outline.MinimumPoint.X + outline.MaximumPoint.X) / 2, (outline.MinimumPoint.Y + outline.MaximumPoint.Y) / 2, (outline.MinimumPoint.Z + outline.MaximumPoint.Z) / 2);
        }
    }
}
