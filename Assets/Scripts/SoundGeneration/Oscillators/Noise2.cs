using DarkArtsStudios.SoundGenerator.Module.Oscillator;

namespace Assets.Scripts.SoundGeneration.Oscillators
{
    public class Noise2 : BaseOscillator
    {
        public static string MenuEntry() => MenuEntryProvider.Oscillator("Noise");

        private System.Random random = new System.Random();

        public override float OnAmplitude(float frequency, float time, float duration, int depth)
        {
            var val = (float)(random.NextDouble() * 2.0 - 1.0);
            // Debug.Log("Generated: " + val);
            return val;
        }
    }
}