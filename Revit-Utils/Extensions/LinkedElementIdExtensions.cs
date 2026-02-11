using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;
using WT.Revit.Models;

namespace WT.Revit.Extensions
{
    public static class LinkedElementIdExtensions
    {
        public static IEnumerable<Element> GetInsulations(this IList<LinkElementId> selectedElementIds, Document doc, View view)
        {
            var selectedElements = selectedElementIds.Where(x => x.LinkInstanceId == ElementId.InvalidElementId).Select(x => new LinkElement(x.GetLinkElement(doc)));

            return selectedElements.ToList().GetInsulations(doc, view);
        }

        public static Element GetLinkElement(this LinkElementId linkElementId, Document document = null)
        {
            if (linkElementId == null)
            {
                return null;
            }

            if (document == null)
            {
                document = RevitApp.Doc;
            }


            if (linkElementId.HostElementId != ElementId.InvalidElementId)
            {
                return document.GetElement(linkElementId.HostElementId);
            }

            if (linkElementId.LinkInstanceId != ElementId.InvalidElementId)
            {
                var linkInstance = document.GetElement(linkElementId.LinkInstanceId) as RevitLinkInstance;
                var linkDocument = linkInstance?.GetLinkDocument();
                return linkDocument?.GetElement(linkElementId.LinkedElementId);
            }

            return null;
        }
    }
}
