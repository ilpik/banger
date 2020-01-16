using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.SoundGeneration.Util
{
    static class MathUtil
    {
        public static float RadToDeg(float rad) => (float)(rad * 180.0 / Math.PI);

        public static float DegToRad(float deg) => (float) (deg * Math.PI / 180.0);
    }
}
