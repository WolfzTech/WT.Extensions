using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;

namespace WT.Revit.SelectionFilter
{
    class DetailCurveSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
#if R20 || R21 || R22
            if (elem is DetailCurve )
#else
            if (elem is DetailCurve)
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
