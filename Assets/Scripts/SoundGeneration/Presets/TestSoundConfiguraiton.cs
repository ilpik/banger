using System.Linq;
using Assets.Scripts.SoundGeneration.Adsr;
using Assets.Scripts.SoundGeneration.DarkArtsStudios.SoundGenerator.DarkArtsStudios.SoundGenerator.Module.Filter;
using Assets.Scripts.SoundGeneration.Modules;
using Assets.Scripts.SoundGeneration.Oscillators;
using Assets.Scripts.SoundGeneration.Util;
using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio;
using DarkArtsStudios.SoundGenerator.Module;
using DarkArtsStudios.SoundGenerator.Module.Filter;
using DarkArtsStudios.SoundGenerator.Module.Oscillator;
using Composition = DarkArtsStudios.SoundGenerator.Composition;

namespace Assets.Scripts.SoundGeneration.Presets
{
    class TestSoundConfiguraiton : BaseSoundConfiguration
    {
        protected override void OnConfigure(Composition composition)
        {
            base.OnConfigure(composition);
            var seq = AddModule<SequencerX4>(composition);
            var notes = seq.attributes.Where(x => x.type == BaseModule.Attribute.AttributeType.FREQUENCY).ToArray();

            int startNote = 3; //C
            for (int i = 0; i < notes.Length; i++)
            {
                notes[i].value = Music.Frequency(4, startNote + 2 * i);
            }
            var osc = AddModule<Sine>(composition);
            var lfo = AddModule<LFO>(composition);
            var filter = AddModule<SGexSoften>(composition);

            osc.frequency.generator = seq;
            filter.attribute("Distance").generator = lfo;
            filter.attribute("Input").generator = osc;


            //var lfo = AddModule<LFO>(composition);
            //lfo.Period.value = 50.0f;
            //var multiplier = AddModule<Multiplier>(composition);

            //multiplier.attributes[0].generator = osc;
            //multiplier.attributes[1].generator = lfo;
            //var adsr = AddModule<AdsrEnvelope>(composition);

            //adsr.GetInputFrom(osc);
            output.GetInputFrom(filter);
        }
    }
}