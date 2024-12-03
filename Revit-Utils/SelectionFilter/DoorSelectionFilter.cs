using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;

namespace WT.Revit.SelectionFilter
{
    public class DoorSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (elem is FamilyInstance && elem.Category.BuiltInCategory == BuiltInCategory.OST_Doors)
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            throw new NotImplementedException();
        }
    }
}
