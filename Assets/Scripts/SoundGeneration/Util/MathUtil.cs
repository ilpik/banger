using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.SoundGeneration.Util
{
    static class MathUtil
    {
        public static double RadToDeg(double rad) => (rad * 180.0 / Math.PI);

        public static double DegToRad(double deg) => (deg * Math.PI / 180.0);

        public static int FloorToInt(double value) => (int) Math.Floor(value);
        
        public static int RoundToInt(double value) => (int) Math.Round(value);

        public static double Repeat(double t, double length)
        {
            return t - Math.Floor(t / length) * length;
        }


    }
}
