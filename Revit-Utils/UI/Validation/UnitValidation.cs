using Autodesk.Revit.DB;
using System.Globalization;
using System.Windows.Controls;

namespace WT.Revit.UI.Validation
{
    public class UnitValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
#if R20
            if (UnitFormatUtils.TryParse(RevitApp.Doc.GetUnits(), UnitType.UT_Length, value as string, out _))
            {
                return new ValidationResult(true, null);
            }
#else
            if (UnitFormatUtils.TryParse(RevitApp.Doc.GetUnits(), SpecTypeId.Length, value as string, out _))
            {
                return new ValidationResult(true, null);
            }
#endif

            return new ValidationResult(false, "Invalid unit format");
        }
    }
}
