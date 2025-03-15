using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;

namespace WT.Revit.SelectionFilter
{
    class DoorSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
#if R20 || R21 || R22
            if (elem is FamilyInstance && elem.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Doors)
#else
            if (elem is FamilyInstance && elem.Category.BuiltInCategory == BuiltInCategory.OST_Doors)
#endif
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
