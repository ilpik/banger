using Assets.Scripts.SoundGeneration.Adsr;
using Assets.Scripts.SoundGeneration.Oscillators;
using Assets.Scripts.SoundGeneration.Util;
using DarkArtsStudios.SoundGenerator;

namespace Assets.Scripts.SoundGeneration.Presets
{
    class TestSoundConfiguraiton : BaseSoundConfiguration
    {
        protected override void OnConfigure(Composition composition)
        {
            base.OnConfigure(composition);
            var osc = AddModule<Sin>(composition);
            var adsr = AddModule<AdsrEnvelope>(composition);
            var nu = AddModule<AdsrEnvelope>(composition);
            var nu2 = AddModule<AdsrEnvelope>(composition);
            var nu3 = AddModule<AdsrEnvelope>(composition);

            adsr.GetInputFrom(osc);
            output.GetInputFrom(adsr);
        }
    }
}