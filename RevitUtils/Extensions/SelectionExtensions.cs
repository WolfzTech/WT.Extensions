using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;

namespace Autodesk.Revit.UI.Selection
{
    public static class SelectionExtensions
    {
        public static List<Element> GetElements(this Selection sel)
        {
            return sel.GetElementIds().Select(x => x.Element()).ToList();
        }

        public static Element GetElement(this Selection sel)
        {
            return sel.GetElementIds().Select(x => x.Element()).FirstOrDefault();
        }
    }
}
