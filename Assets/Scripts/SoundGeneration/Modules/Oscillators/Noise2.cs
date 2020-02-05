using DarkArtsStudios.SoundGenerator.Module.Oscillator;

namespace Assets.Scripts.SoundGeneration.Oscillators
{
    public class Noise2 : BaseOscillator
    {
        public static string MenuEntry() => MenuEntryProvider.Oscillator("Noise");

        private System.Random random = new System.Random();

        public override double OnAmplitude(double time, int depth, int sampleRate)
        {
            var val = (float)(random.NextDouble() * 2.0 - 1.0);
            // Debug.Log("Generated: " + val);
            return val;
        }
    }
}