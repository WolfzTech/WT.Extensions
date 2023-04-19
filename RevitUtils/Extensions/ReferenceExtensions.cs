using TWolfz.Revit;
using System.Collections.Generic;
using System.Linq;

namespace Autodesk.Revit.DB
{
    public static class ReferenceExtensions
    {
        public static List<Element> LinkedElements(this IList<Reference> references)
        {
            return references.Select(x => x.LinkedElement()).ToList();
        }

        public static Element LinkedElement(this Reference reference, out Transform transform)
        {
            var revitLinkInstance = reference.Element<RevitLinkInstance>();
            transform = revitLinkInstance.GetTransform();
            var linkedDoc = revitLinkInstance.GetLinkDocument();
            return reference.LinkedElementId.Element(linkedDoc);
        }

        public static Element LinkedElement(this Reference reference)
        {
            return reference.LinkedElement(out Transform _);
        }

        public static Element Element(this Reference reference, Document doc = null)
        {
            if (doc != null)
            {
                return doc.GetElement(reference);
            }
            else
            {
                return RevitApp.Doc.GetElement(reference);
            }
        }

        public static T Element<T>(this Reference reference, Document doc = null) where T : Element
        {
            return reference.Element(doc) as T;
        }

        public static List<Element> Elements(this IList<Reference> references)
        {
            return references.Select(x => x.Element()).ToList();
        }

        public static List<T> Elements<T>(this IList<Reference> references) where T : Element
        {
            return references.Select(x => x.Element<T>()).ToList();
        }
    }
}
