using Autodesk.Revit.DB;
using System.Globalization;
using System.Windows.Controls;

namespace WT.Revit.UI.Validation
{
    public class UnitValidation : ValidationRule
    {
        public Validation ValidationType { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
#if R20
            UnitType unitType = ValidationType switch
            {
                Validation.Angle => UnitType.UT_Angle,
                _ => UnitType.UT_Length,
            };

            if (UnitFormatUtils.TryParse(RevitApp.Doc.GetUnits(), unitType, value as string, out _))
            {
                return new ValidationResult(true, null);
            }
#else
            ForgeTypeId unitType = ValidationType switch
            {
                Validation.Angle => SpecTypeId.Angle,
                _ => SpecTypeId.Length,
            };

            if (UnitFormatUtils.TryParse(RevitApp.Doc.GetUnits(), unitType, value as string, out _))
            {
                return new ValidationResult(true, null);
            }

#endif
            return new ValidationResult(false, "Invalid unit format");
        }
        public enum Validation
        {
            Angle,
            Length
        }
    }
}
