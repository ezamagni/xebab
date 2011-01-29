using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xebab.Helpers.Maths
{
    public static class RandomProvider
    {
        private static Random rnd = new Random();

        public static int Next()
        {
            return rnd.Next();
        }

        public static int Next(int max)
        {
            return rnd.Next(max);
        }

        public static float NextFloat(float max)
        {
            return (float)(rnd.NextDouble() * max);
        }

        public static int Next(int min, int max)
        {
            return rnd.Next(min, max);
        }

        public static float NextFloat(float min, float max)
        {
            return min + (float)(rnd.NextDouble() * (max - min));
        }

    }

}
