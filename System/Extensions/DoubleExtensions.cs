using System;
using System.Collections.Generic;

namespace WT.Sys.Extensions
{
    public static class DoubleExtensions
    {
        public static bool IsAlmostEqualTo(this double value, double other, double tolerance = 1e-9)
        {
            return Math.Abs(value - other) < tolerance;
        }

        public static DoubleComparer Comparer(double tolerance = 1e-9)
        {
            return new DoubleComparer(tolerance);
        }

        public static DoubleEqualityComparer EqualityComparer(double tolerance = 1e-9)
        {
            return new DoubleEqualityComparer(tolerance);
        }

        public class DoubleComparer(double tolerance = 1e-9) : IComparer<double>
        {
            public int Compare(double x, double y)
            {
                if (Math.Abs(x - y) < tolerance)
                {
                    return 0;
                }
                if (x < y) return -1;
                else return 1;
            }
        }

        public class DoubleEqualityComparer(double tolerance = 1e-9) : IEqualityComparer<double>
        {
            public bool Equals(double x, double y)
            {
                return Math.Abs(x - y) < tolerance;
            }

            public int GetHashCode(double obj)
            {
                return 0;
            }
        }
    }
}
