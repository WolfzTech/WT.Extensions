using System;
using SysColor = System.Drawing.Color;

namespace Autodesk.Revit.DB
{
    public static class ElementTypeExtension
    {
        public static bool SetColor(this ElementType elementType, int red, int green, int blue)
        {
            Parameter colorParam = elementType.get_Parameter(BuiltInParameter.LINE_COLOR);
            if (colorParam != null)
            {
                SysColor sysColor = SysColor.FromArgb(red, green, blue);
                int intColor = GetLineColorFromSystemColor(sysColor);
                return colorParam.Set(intColor);
            }
            return false;
        }
        
        private static int GetLineColorFromSystemColor(SysColor color)
        {
            return color.R + color.G * (int)Math.Pow(2, 8) + color.B * (int)Math.Pow(2, 16);
        }
    }
}
