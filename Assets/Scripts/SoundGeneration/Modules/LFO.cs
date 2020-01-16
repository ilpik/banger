using Assets.Scripts.SoundGeneration.Util;
using DarkArtsStudios.SoundGenerator.Module;
using UnityEngine;

namespace Assets.Scripts.SoundGeneration
{
    class LFO : BaseModule
    {
        public static string MenuEntry() => MenuEntryProvider.Get("LFO");

        public Attribute Period;

        public Attribute Phase;

        public override void InitializeAttributes()
        {
            AddInput();
            Period = AddAttribute("Period");
            Phase = AddAttribute("Phase");
      }

        public override float OnAmplitude(float frequency, float time, float duration, int depth, int sampleRate)
        {
            //float input = this.GetInput().amplitude(frequency, time, duration, depth, sampleRate);
            float period = Period.value;
            float phase = Phase.value;
            return Mathf.Sin(MathUtil.DegToRad((time * frequency / period + phase) * 360.0f));
        }
    }
}
