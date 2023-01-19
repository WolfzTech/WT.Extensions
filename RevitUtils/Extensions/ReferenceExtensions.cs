using System.Collections.Generic;

namespace Autodesk.Revit.DB
{
    public static class ReferenceExtensions
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
}
