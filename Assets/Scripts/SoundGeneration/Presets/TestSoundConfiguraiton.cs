using Assets.Scripts.SoundGeneration;
using Assets.Scripts.SoundGeneration.Adsr;
using DarkArtsStudios.SoundGenerator;
using DarkArtsStudios.SoundGenerator.Module.Oscillator;
using UnityEngine;

class TestSoundConfiguraiton : BaseSoundConfiguration
{
    public override void Configure(Composition composition)
    {
        base.Configure(composition);
        var osc = AddModule<Sin>(composition);
        var adsr = AddModule<AdsrEnvelope>(composition);

        adsr.GetInputFrom(osc);
        output.GetInputFrom(adsr);
    }
}