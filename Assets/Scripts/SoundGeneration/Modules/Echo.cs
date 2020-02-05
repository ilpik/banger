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

        public override double OnAmplitude(double time, int depth, int sampleRate)
        {
            float dur = 0;
            float decay = Decay.value;
            double value = this.GetInput().amplitude(time, depth, sampleRate);
            value += this.GetInput().amplitude(time - Delay.value, depth, sampleRate) * decay;
            return value;
        }
    }
}