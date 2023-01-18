namespace Autodesk.Revit.DB
{
    public static class ElementIdExtension
    {
        public static Element Element(this ElementId elementId, Document doc)
        {
            return doc.GetElement(elementId);
        }

        public static Element Element(this ElementId elementId)
        {
            return PT_REVIT.RevitApp.Doc.GetElement(elementId);
        }

        public static T Element<T>(this ElementId elementId) where T : Element
        {
            return PT_REVIT.RevitApp.Doc.GetElement(elementId) as T;
        }

        public static T Element<T>(this ElementId elementId, Document doc) where T : Element
        {
            return doc.GetElement(elementId) as T;
        }
    }
}
