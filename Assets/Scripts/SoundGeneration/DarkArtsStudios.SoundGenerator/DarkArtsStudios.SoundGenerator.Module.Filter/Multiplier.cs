using DarkArtsStudios.SoundGenerator.Module;

namespace Assets.Scripts.SoundGeneration.DarkArtsStudios.SoundGenerator.DarkArtsStudios.SoundGenerator.Module.Filter
{
    public class Multiplier : BaseModule
    {
        public static string MenuEntry() => MenuEntryProvider.Get("Multiplier");

        public override void InitializeAttributes()
        {
            attributes.Add(new Attribute("Input", _hiddenValue: true));
            attributes.Add(new Attribute("Input", _hiddenValue: true));
            attributes.Add(new Attribute("Input", _hiddenValue: true));
            attributes.Add(new Attribute("Input", _hiddenValue: true));
        }

        public override float OnAmplitude(float frequency, float time, float duration, int depth, int sampleRate)
        {
            float? num = null;

            foreach (Attribute attribute in attributes)
            {
                if ((bool)attribute.generator)
                {
                    var value = attribute.generator.amplitude(frequency, time, duration, depth + 1, sampleRate);
                    num = num == null ? value : num * value;
                }
            }
            return num ?? 0.0f;
        }
    }
}