using Autodesk.Revit.DB;

namespace System
{
    public static class UnitExtensions
    {
        public static double FeetToCm(this double dou)
        {
            return dou * 30.48;
        }

        /// <summary>
        /// Multiply the length value by 304.8
        /// </summary>
        /// <param name="dou"></param>
        /// <returns></returns>
        public static double FeetToMm(this double dou)
        {
            return dou * 304.8;
        }
        public static double FeetToM(this double dou)
        {
            return dou / 3.281;
        }
        public static double CmToFeet(this double dou)
        {
            return dou / 30.48;
        }
        public static double MmToFeet(this double dou)
        {
            return dou / 304.8;
        }
        public static double MToFeet(this double dou)
        {
            return dou * 3.28084;
        }
        public static double RadianToDegree(this double dou)
        {
            return dou * 180 / Math.PI;
        }
        public static double DegreeToRadian(this double dou)
        {
            return dou * Math.PI / 180;
        }

#if Revit2018||Revit2019||Revit2020
        public static double UnitConvert(this double value, DisplayUnitType sourceUnit, DisplayUnitType destinationUnit)
        {
            var internalValue = UnitUtils.ConvertFromInternalUnits(value, sourceUnit);
            return UnitUtils.ConvertToInternalUnits(internalValue, destinationUnit);
        }

        public static double UnitConvert(this int value, DisplayUnitType sourceUnit, DisplayUnitType destinationUnit)
        {
            return value * 1.0.UnitConvert(sourceUnit, destinationUnit);
        }
#else
        public static double UnitConvert(this double value, ForgeTypeId sourceUnit, ForgeTypeId destinationUnit)
        {
            var internalValue=UnitUtils.ConvertFromInternalUnits(value, sourceUnit);
            return UnitUtils.ConvertToInternalUnits(internalValue, destinationUnit);
        }

        public static double UnitConvert(this int value, ForgeTypeId sourceUnit, ForgeTypeId destinationUnit)
        {
            return value * 1.0.UnitConvert(sourceUnit, destinationUnit);
        }
#endif
    }
}
