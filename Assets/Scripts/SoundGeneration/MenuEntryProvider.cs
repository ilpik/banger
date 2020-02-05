using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.SoundGeneration
{
    internal static class MenuEntryProvider
    {
        public static string Get(params string[] entry) => string.Join("/", new string[] {"Banger"}.Concat(entry));

        public static string Oscillator(string name) => Get("Oscillator", name);

        public static string Adsr(string name) => Get("ADSR", name);

        public static string Filter(string name) => Get("Filter", name);

        public static string Sequencer(string name) => Get("Sequencer", name);
    }
}
