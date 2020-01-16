using Assets.Scripts.SoundGeneration.Adsr;
using Assets.Scripts.SoundGeneration.DarkArtsStudios.SoundGenerator.DarkArtsStudios.SoundGenerator.Module.Filter;
using Assets.Scripts.SoundGeneration.Oscillators;
using Assets.Scripts.SoundGeneration.Util;
using DarkArtsStudios.SoundGenerator;
using DarkArtsStudios.SoundGenerator.Module.Filter;
using DarkArtsStudios.SoundGenerator.Module.Oscillator;

namespace Assets.Scripts.SoundGeneration.Presets
{
    class TestSoundConfiguraiton : BaseSoundConfiguration
    {
        protected override void OnConfigure(Composition composition)
        {
            base.OnConfigure(composition);
            var osc = AddModule<Sine>(composition);
            var lfo = AddModule<LFO>(composition);
            lfo.Period.value = 50.0f;
            var multiplier = AddModule<Multiplier>(composition);

            multiplier.attributes[0].generator = osc;
            multiplier.attributes[1].generator = lfo;
            //var adsr = AddModule<AdsrEnvelope>(composition);

            //adsr.GetInputFrom(osc);
            output.GetInputFrom(multiplier);
        }
    }
}