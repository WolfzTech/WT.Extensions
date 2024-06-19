using Autodesk.Revit.DB;

namespace WT.Revit.Utils
{
    public class UnitUtils
    {
        public static double GetLengthFromString(string text)
        {
            if (string.IsNullOrEmpty(text)) return 0;
#if R20
            var canParse = UnitFormatUtils.TryParse(RevitApp.Doc.GetUnits(), UnitType.UT_Length, text, out var value);
#else
            var canParse = UnitFormatUtils.TryParse(RevitApp.Doc.GetUnits(), SpecTypeId.Length, text, out var value);
#endif
            if (canParse)
            {
                return value;
            }
            throw new System.Exception("Invalid unit format");
        }

        /// <summary>
        /// Formats a length into a string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="forEditing">
        ///  True if the formatting should be modified as necessary so that the formatted
        ///  string can be successfully parsed, for example by suppressing digit grouping.
        ///  False if unmodified settings should be used, suitable for display only.
        ///  </param>
        /// <returns>The formatted string.</returns>
        public static string LengthToString(double value, bool forEditing = true)
        {
#if R20
            return UnitFormatUtils.Format(RevitApp.Doc.GetUnits(), UnitType.UT_Length, value, true, forEditing);
#else
            return UnitFormatUtils.Format(RevitApp.Doc.GetUnits(), SpecTypeId.Length, value, forEditing);
#endif

        }
    }
}
