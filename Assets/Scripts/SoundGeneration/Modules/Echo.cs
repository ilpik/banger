using Assets.Scripts.SoundGeneration.Util;
using DarkArtsStudios.SoundGenerator.Module;

namespace Assets.Scripts.SoundGeneration
{
    class Echo : BaseModule
    {
        public static string MenuEntry() => MenuEntryProvider.Get("Echo");

        private Attribute Delay;
        private Attribute Decay;

        public override void InitializeAttributes()
        {
            Delay = AddAttribute("Delay");
            Decay = AddAttribute("Decay");
            AddInput();
        }

        public override float OnAmplitude(float frequency, float time, float duration, int depth, int sampleRate)
        {
            float dur = 0;
            float decay = Decay.value;
            float value = this.GetInput().amplitude(frequency, time, duration, depth, sampleRate);
            value += this.GetInput().amplitude(frequency, time - Delay.value, duration, depth, sampleRate) * decay;
            return value;
        }
    }
}