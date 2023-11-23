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

        public static double UnitConvert(this double value, ForgeTypeId sourceUnit, ForgeTypeId destinationUnit)
        {
            var internalValue=UnitUtils.ConvertToInternalUnits(value, sourceUnit);
            return UnitUtils.ConvertFromInternalUnits(internalValue, destinationUnit);
        }

        public static double UnitConvert(this int value, ForgeTypeId sourceUnit, ForgeTypeId destinationUnit)
        {
            return value * 1.0.UnitConvert(sourceUnit, destinationUnit);
        }

        public static double SqMeterToSqFeet(this double dou)
        {
            return dou * 10.7639;
        }

        public static double SqFeetToSqMeter(this double dou)
        {
            return dou / 10.7639;
        }
    }
}
