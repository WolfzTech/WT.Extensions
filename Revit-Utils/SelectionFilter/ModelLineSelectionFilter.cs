﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;

namespace WT.Revit.SelectionFilter
{
     class ModelLineSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
#if R20 || R21 || R22
            if (elem is ModelLine && elem.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Lines)
#else
            if (elem is ModelLine )
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
