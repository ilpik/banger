using System;
using Assets.Scripts.SoundGeneration.Util;
using DarkArtsStudios.SoundGenerator.Module;
using UnityEngine;

namespace Assets.Scripts.SoundGeneration
{
    class LFO : DAAudioFilter
    {
        public static string MenuEntry() => MenuEntryProvider.Get("LFO");

        public Attribute Frequency;

        public Attribute Phase;

        public override void InitializeAttributes()
        {
            base.InitializeAttributes();
            Frequency = AddAttribute("Frequency", b => b.Slider(0, 200));
            Phase = AddAttribute("Phase", b=>b.Slider(0, 360));
      }
            
        public override double OnAmplitude(double time, int depth, int sampleRate)
        {
            //float input = this.GetInput().amplitude(frequency, time, duration, depth, sampleRate);
            double freq = Frequency.getAmplitudeOrValue(time, depth + 1, sampleRate);
            double phase = Phase.getAmplitudeOrValue(time, depth + 1, sampleRate);
            return Math.Sin(MathUtil.DegToRad((time * freq + phase) * 360.0f));
        }
    }
}
