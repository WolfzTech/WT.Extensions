using System.Collections.Generic;

namespace WT.Revit.Utils
{
    public class NamingComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == null)
            {
                return y == null ? 0 : -1;
            }
            if (y == null)
            {
                return 1;
            }
            return Autodesk.Revit.DB.NamingUtils.CompareNames(x, y);
        }
        public static int CompareResult(string x, string y)
        {
            if (x == null)
            {
                return y == null ? 0 : -1;
            }
            if (y == null)
            {
                return 1;
            }
            return Autodesk.Revit.DB.NamingUtils.CompareNames(x, y);
        }
    }
}
