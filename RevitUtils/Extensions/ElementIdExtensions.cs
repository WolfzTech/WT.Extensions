using WT.Revit;

namespace Autodesk.Revit.DB
{
    public static class ElementIdExtension
    {
        public static Element Element(this ElementId elementId, Document doc = null)
        {
            if (doc != null)
            {
                return doc.GetElement(elementId);
            }
            else
            {
                return RevitApp.Doc.GetElement(elementId);
            }
        }

        public static T Element<T>(this ElementId elementId, Document doc = null) where T : Element
        {
            return elementId.Element(doc) as T;
        }
    }
}
