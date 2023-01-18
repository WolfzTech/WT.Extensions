namespace System
{
    public static class DoubleExtenstions
    {
        public static double FeetToCm(this double dou)
        {
            return dou * 30.48;
        }
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
    }
}
