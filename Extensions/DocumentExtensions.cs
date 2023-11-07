namespace Autodesk.Revit.DB
{
    public static class DocumentExtensions
    {
        public static T GetElement<T>(this Document document, ElementId elementId) where T : Element
        {
            return document.GetElement(elementId) as T;
        }

        public static T GetElement<T>(this Document document, string uniqueId) where T : Element
        {
            return document.GetElement(uniqueId) as T;
        }
    }
}
